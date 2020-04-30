using System;
using System.Collections.Generic;
using System.Linq;
using BoolExpressions.QuineMcCluskeyMethod.Term;
using static BoolExpressions.QuineMcCluskeyMethod.Term.Factories;

namespace BoolExpressions.QuineMcCluskeyMethod
{
    public class ImplicantHelpers
    {
        public static int GetCombinedVariableDistance<T>(
            Implicant<T> implicantA,
            Implicant<T> implicantB) where T : class
        {
            Func<Implicant<T>, HashSet<T>> getCombinedVariables = (Implicant<T> implicant) =>
            {
                return implicant
                    .Minterm
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

        public static int GetImplicantWeight<T>(
            Implicant<T> implicant) where T : class
        {
            return implicant
                .Minterm
                .Where(term =>
                {
                    return term switch
                    {
                        PositiveTerm<T> _ => true,
                        _ => false
                    };
                })
                .Count();
        }

        public static Implicant<T> CombineImplicants<T>(
            Implicant<T> implicantA,
            Implicant<T> implicantB) where T : class
        {
            var variableTermMapB = implicantB.Minterm.ToDictionary(term => term.Variable, term => term);
            var combinedMinterm = implicantA
              .Minterm
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

        public static void ProcessCurrentLevelImplicantSet<T>(
            in HashSet<Implicant<T>> currentWightImplicantSet,
            in HashSet<Implicant<T>> nextWeightImplicantSet,
            out HashSet<Implicant<T>> currentLevelProcessedImplicantSet,
            out HashSet<Implicant<T>> nextLevelImplicantSet) where T : class
        {
            currentLevelProcessedImplicantSet = new HashSet<Implicant<T>>();
            nextLevelImplicantSet = new HashSet<Implicant<T>>();

            foreach (var currentWightImplicant in currentWightImplicantSet)
            {
                foreach (var nextWeightImplicant in nextWeightImplicantSet)
                {
                    var implicantsDistance = ImplicantHelpers.GetCombinedVariableDistance(currentWightImplicant, nextWeightImplicant);
                    if (implicantsDistance != 0) continue;
                    var nextLevelImplicantCandidate = CombineImplicants(currentWightImplicant, nextWeightImplicant);
                    var nextLevelImplicantCandidateDistance = ImplicantHelpers.GetCombinedVariableDistance(currentWightImplicant, nextLevelImplicantCandidate);
                    if (nextLevelImplicantCandidateDistance != 1) continue;

                    nextLevelImplicantSet.Add(nextLevelImplicantCandidate);
                    currentLevelProcessedImplicantSet.Add(currentWightImplicant);
                    currentLevelProcessedImplicantSet.Add(nextWeightImplicant);
                }
            }
        }

        public static HashSet<Implicant<T>> GetFinalImplicantSet<T>(
            in HashSet<Implicant<T>> implicantSet) where T : class
        {
            var finalImplicantSet = new HashSet<Implicant<T>>();
            var currentLevelImplicantSet = implicantSet;

            while(currentLevelImplicantSet.Count() > 0) {
                var implicantWeightImplicantMap = currentLevelImplicantSet
                    .GroupBy(implicant => GetImplicantWeight(implicant))
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
