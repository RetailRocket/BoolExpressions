using System;
using System.Collections.Generic;
using System.Linq;
using BoolExpressions.QuineMcCluskeyMethod.Term;
using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("UnitTests")]

namespace BoolExpressions.QuineMcCluskeyMethod
{    
    internal static class ImplicantSetFinalizeExtension
    {
        private static Implicant<T> CombineImplicants<T>(
            Implicant<T> implicantA,
            Implicant<T> implicantB) where T : class
        {
            var variableTermMapB = implicantB.TermSet.ToDictionary(term => term.Variable, term => term);
            var combinedMinterm = implicantA
              .TermSet
              .Select(termA =>
              {
                  var variable = termA.Variable;
                  var termB = variableTermMapB[variable];
                  return (termA, termB) switch
                  {
                      (PositiveTerm<T> _, NegativeTerm<T> _) => new CombinedTerm<T>(variable),
                      (NegativeTerm<T> _, PositiveTerm<T> _) => new CombinedTerm<T>(variable),
                      _ => termA
                  };
              })
              .ToHashSet();
            return new Implicant<T>(combinedMinterm);
        }

        private static int GetCombinedVariableDistance<T>(
            Implicant<T> implicantA,
            Implicant<T> implicantB) where T : class
        {
            Func<Implicant<T>, HashSet<T>> getCombinedVariables = (Implicant<T> implicant) =>
            {
                return implicant
                    .TermSet
                    .Where(term =>
                    {
                        return term switch
                        {
                            CombinedTerm<T> _ => true,
                            _ => false
                        };
                    })
                    .Select(term => term.Variable)
                    .ToHashSet();
            };

            var combinedVariablesA = getCombinedVariables(implicantA);
            var combinedVariablesB = getCombinedVariables(implicantB);

            var difference = combinedVariablesA
                .Union(combinedVariablesB)
                .Except(combinedVariablesA
                    .Intersect(combinedVariablesB));

            return difference.Count();
        }

        private static void ProcessCurrentLevelImplicantSet<T>(
            HashSet<Implicant<T>> currentWightImplicantSet,
            HashSet<Implicant<T>> nextWeightImplicantSet,
            out HashSet<Implicant<T>> currentLevelProcessedImplicantSet,
            out HashSet<Implicant<T>> nextLevelImplicantSet) where T : class
        {
            currentLevelProcessedImplicantSet = new HashSet<Implicant<T>>();
            nextLevelImplicantSet = new HashSet<Implicant<T>>();

            foreach (var currentWightImplicant in currentWightImplicantSet)
            {
                foreach (var nextWeightImplicant in nextWeightImplicantSet)
                {
                    var implicantsDistance = GetCombinedVariableDistance(currentWightImplicant, nextWeightImplicant);
                    if (implicantsDistance != 0) continue;
                    var nextLevelImplicantCandidate = CombineImplicants(currentWightImplicant, nextWeightImplicant);
                    var nextLevelImplicantCandidateDistance = GetCombinedVariableDistance(currentWightImplicant, nextLevelImplicantCandidate);
                    if (nextLevelImplicantCandidateDistance != 1) continue;

                    nextLevelImplicantSet.Add(nextLevelImplicantCandidate);
                    currentLevelProcessedImplicantSet.Add(currentWightImplicant);
                    currentLevelProcessedImplicantSet.Add(nextWeightImplicant);
                }
            }
        }

        internal static HashSet<Implicant<T>> GetFinalImplicantSet<T>(
            this HashSet<Implicant<T>> implicantSet) where T : class
        {
            var finalImplicantSet = new HashSet<Implicant<T>>();
            var currentLevelImplicantSet = implicantSet;

            while(currentLevelImplicantSet.Count() > 0) {
                var implicantWeightImplicantMap = currentLevelImplicantSet
                    .GroupBy(implicant => implicant.GetPositiveWeight())
                    .ToDictionary(group => group.Key, group => group.ToHashSet());

                var processedImplicantSet = new HashSet<Implicant<T>>();                
                var nextLevelImplicantSet = new HashSet<Implicant<T>>();

                var weights = implicantWeightImplicantMap
                    .Keys
                    .OrderBy(weight => weight)
                    .ToList();

                foreach(var (currentWeight, nextWeight) in weights.Zip(weights.Skip(1), Tuple.Create))
                {
                    var currentLevelProcessedImplicantSet = new HashSet<Implicant<T>>();
                    HashSet<Implicant<T>> currentWeightAndNextLevelImplicantSet;

                    ProcessCurrentLevelImplicantSet(
                        currentWightImplicantSet: implicantWeightImplicantMap[currentWeight],
                        nextWeightImplicantSet: implicantWeightImplicantMap[nextWeight],
                        currentLevelProcessedImplicantSet: out currentLevelProcessedImplicantSet,
                        nextLevelImplicantSet: out currentWeightAndNextLevelImplicantSet);

                    processedImplicantSet.UnionWith(currentLevelProcessedImplicantSet);
                    nextLevelImplicantSet.UnionWith(currentWeightAndNextLevelImplicantSet);
                }

                finalImplicantSet.UnionWith(currentLevelImplicantSet.Except(processedImplicantSet));
                currentLevelImplicantSet = nextLevelImplicantSet;
            }

            return finalImplicantSet;
        }
    }
}
