using System;
using System.Collections.Generic;
using System.Linq;
using BoolExpressions.DisjunctiveNormalForm;
using BoolExpressions.DisjunctiveNormalForm.Operation;
using BoolExpressions.QuineMcCluskeyMethod.Term;
using static BoolExpressions.QuineMcCluskeyMethod.Factories;
using static BoolExpressions.QuineMcCluskeyMethod.Term.Factories;
using static BoolExpressions.DisjunctiveNormalForm.Factories;
using static BoolExpressions.DisjunctiveNormalForm.Operation.Factories;

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

        public static int GetImplicantPositiveWeight<T>(
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
            HashSet<Implicant<T>> implicantSet) where T : class
        {
            var finalImplicantSet = new HashSet<Implicant<T>>();
            var currentLevelImplicantSet = implicantSet;

            while(currentLevelImplicantSet.Count() > 0) {
                var implicantWeightImplicantMap = currentLevelImplicantSet
                    .GroupBy(implicant => GetImplicantPositiveWeight(implicant))
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
            HashSet<DnfAnd<T>> mintermSet,
            HashSet<Implicant<T>> finalImplicantSet,
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

        public static HashSet<HashSet<Implicant<T>>> TruncateImplicantSetOfSet<T>(
            HashSet<HashSet<Implicant<T>>> implicantSetOfSet) where T : class
        {
            var truncatedImplicantSetOfSet = new HashSet<HashSet<Implicant<T>>>();

            foreach(var implicantLongSet in implicantSetOfSet) {
                if(!implicantSetOfSet
                    .Any(implicantShortSet =>
                        implicantLongSet != implicantShortSet &&
                        implicantShortSet.IsSubsetOf(implicantLongSet)))
                {
                    truncatedImplicantSetOfSet.Add(implicantLongSet);
                }
            }

            return truncatedImplicantSetOfSet;
        }

        public static int GetImplicantUncombinedWeight<T>(
            Implicant<T> implicant) where T : class
        {
            return implicant
                .TermSet
                .Where(term =>
                {
                    return term switch
                    {
                        PositiveTerm<T> _ => true,
                        NegativeTerm<T> _ => true,
                        _ => false
                    };
                })
                .Count();
        }

        public static HashSet<Implicant<T>> PetrickMethod<T>(
            HashSet<DnfAnd<T>> mintermSet,
            HashSet<Implicant<T>> implicantSet) where T : class
        {
            var sufficientImplicantSetOfSet = new HashSet<HashSet<Implicant<T>>>(HashSet<Implicant<T>>.CreateSetComparer());

            foreach(var minterm in mintermSet) {
                var sufficientImplicantSet = new HashSet<Implicant<T>>();

                foreach(var implicant in implicantSet) {
                    if(IsImplicantContains(
                        implicant: implicant,
                        minterm: minterm))
                    {
                        sufficientImplicantSet.Add(implicant);
                    }
                }

                if(sufficientImplicantSet.Count() > 0)
                {
                    sufficientImplicantSetOfSet.Add(sufficientImplicantSet);
                }
            }

            var truncatedImplicantSetOfSet = TruncateImplicantSetOfSet(sufficientImplicantSetOfSet);
            
            var minimalImplicantSet = sufficientImplicantSetOfSet
                .OrderBy(implicantSet =>
                {
                    var weight = implicantSet
                        .Select(implicant => GetImplicantUncombinedWeight(implicant))
                        .Sum();
                    return Tuple.Create(weight, implicantSet.GetHashCode());
                })
                .DefaultIfEmpty(new HashSet<Implicant<T>>())
                .First();

            return minimalImplicantSet;
        }

        public static Implicant<T> MintermToImplicant<T>(
            HashSet<IDnfOperation<T>> minterm) where T : class
        {
            return ImplicantOf(
                minterm
                    .Select(operation =>
                    {
                        Term<T> term = operation switch
                        {
                            DnfVariable<T> variable => PositiveTermOf(variable.Value),
                            DnfNot<T> notOperation => NegativeTermOf(notOperation.Variable.Value),
                            _ => throw new ArgumentException(
                                message: "pattern matching in C# is sucks",
                                paramName: nameof(operation))
                        };
                        return term;
                    }));
        }

        public static HashSet<IDnfOperation<T>> ImplicantToMinset<T>(
             Implicant<T> implicant) where T : class
        {
            return implicant.TermSet
                .SelectMany(term =>
                {
                    return term switch
                    {
                        PositiveTerm<T> _ => new List<IDnfOperation<T>> { DnfVariableOf(term.Variable) },
                        NegativeTerm<T> _ => new List<IDnfOperation<T>> { DnfNotOf(DnfVariableOf(term.Variable)) },
                        _ => new List<IDnfOperation<T>>()
                    };
                })
                .ToHashSet();
        }
        public static DnfExpression<T> ProcessDnf<T>(
            in DnfExpression<T> dnfExpression) where T : class
        {
            var mintermSet = dnfExpression.AndBlockSet.ToHashSet();

            var implicantSet = mintermSet
                .Select(andBlock => MintermToImplicant(
                    andBlock.ElementSet))
                .ToHashSet();

            var finalImplicantSet = GetFinalImplicantSet(
                implicantSet: implicantSet);

            HashSet<DnfAnd<T>> finalMintermSet;
            HashSet<Implicant<T>> primaryImplicantSet;

            GetPrimaryImplicantSet(
                mintermSet: mintermSet,
                finalImplicantSet: finalImplicantSet,
                finalMintermSet: out finalMintermSet,
                primaryImplicantSet: out primaryImplicantSet);

            var minimalImplicantSet = PetrickMethod(
                mintermSet: finalMintermSet,
                implicantSet: implicantSet.Except(primaryImplicantSet).ToHashSet());

            var primaryAndMinimalMintermSet = primaryImplicantSet
                .Union(minimalImplicantSet)
                .Select(implicant => new DnfAnd<T>(ImplicantToMinset(implicant)))
                .ToHashSet();

            return new DnfExpression<T>(primaryAndMinimalMintermSet);
        }
    }
}
