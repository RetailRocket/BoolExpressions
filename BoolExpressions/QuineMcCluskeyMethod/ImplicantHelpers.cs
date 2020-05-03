using System;
using System.Collections.Generic;
using System.Linq;
using BoolExpressions.DisjunctiveNormalForm;
using BoolExpressions.DisjunctiveNormalForm.Operation;
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

        public static int GetImplicantWeight<T>(
            Implicant<T> implicant) where T : class
        {
            return implicant
                .TermSet
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

        public static bool IsImplicantContains<T>(
            Implicant<T> implicant,
            DnfAnd<T> minterm) where T : class 
        {
            var variableMintermMapB = minterm.ElementSet.ToDictionary(term =>
                term switch
                {
                    DnfVariable<T> operation => operation.Value,
                    DnfNot<T> operation => operation.Variable.Value,
                    _ => throw new ArgumentException(
                        message: "pattern matching in C# is sucks",
                        paramName: nameof(term))
                },
                term => term);

            return implicant
                .TermSet
                .Where(implicantTerm =>
                {
                    var mintermTerm = variableMintermMapB[implicantTerm.Variable];
                    return (implicantTerm, mintermTerm) switch
                    {
                        (PositiveTerm<T> _, DnfNot<T> _) => true,
                        (NegativeTerm<T> _, DnfVariable<T> _) => true,
                        _ => false
                    };
                })
                .Count() == 0;
        }

        public static void GetPrimaryImplicantSet<T>(
            in HashSet<DnfAnd<T>> mintermSet,
            in HashSet<Implicant<T>> finalImplicantSet,
            out HashSet<DnfAnd<T>> finalMintermSet,
            out HashSet<Implicant<T>> primaryImplicantSet) where T : class
        {
            var processedMintermSet = new HashSet<DnfAnd<T>>();
            primaryImplicantSet = new HashSet<Implicant<T>>();

            foreach(var minterm in mintermSet)
            {
                var candidateImplicantSet = finalImplicantSet
                    .Where(implicant =>
                    {
                        return IsImplicantContains(
                            implicant: implicant,
                            minterm: minterm);
                    });

                if(candidateImplicantSet.Count() == 1)
                {
                    processedMintermSet.Add(minterm);
                    primaryImplicantSet.Add(candidateImplicantSet.First());
                }
            }

            finalMintermSet = mintermSet.Except(processedMintermSet).ToHashSet();
        }
    }
}
