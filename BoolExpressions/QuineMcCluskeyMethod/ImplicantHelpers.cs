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
using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("UnitTests")]

namespace BoolExpressions.QuineMcCluskeyMethod
{
    public class ImplicantHelpers
    {
        private static bool IsImplicantContains<T>(
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

        internal static void GetPrimaryImplicantSet<T>(
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

        private static HashSet<HashSet<Implicant<T>>> TruncateImplicantSetOfSet<T>(
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

        private static int GetImplicantUncombinedWeight<T>(
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

        internal static HashSet<Implicant<T>> PetrickMethod<T>(
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

                    return weight;
                })
                .DefaultIfEmpty(new HashSet<Implicant<T>>())
                .First();

            return minimalImplicantSet;
        }

        private static DnfAnd<T> ImplicantToMinset<T>(
             Implicant<T> implicant) where T : class
        {
            var elementSet = implicant.TermSet
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
            return new DnfAnd<T>(elementSet);
        }

        public static DnfExpression<T> ProcessDnf<T>(
            in DnfExpression<T> dnfExpression) where T : class
        {
            var mintermSet = dnfExpression.AndBlockSet.ToHashSet();

            var implicantSet = mintermSet
                .Select(andBlock => ImplicantOf(andBlock))
                .ToHashSet();

            var finalImplicantSet = implicantSet.GetFinalImplicantSet();

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
                .Select(implicant => ImplicantToMinset(implicant))
                .ToHashSet();

            return new DnfExpression<T>(primaryAndMinimalMintermSet);
        }
    }
}
