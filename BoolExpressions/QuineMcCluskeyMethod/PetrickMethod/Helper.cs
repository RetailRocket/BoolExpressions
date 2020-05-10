using System;
using System.Collections.Generic;
using System.Linq;
using BoolExpressions.QuineMcCluskeyMethod.Term;
using System.Runtime.CompilerServices;
using BoolExpressions.DisjunctiveNormalForm;

[assembly:InternalsVisibleTo("UnitTests")]

namespace BoolExpressions.QuineMcCluskeyMethod.PetrickMethod
{    
    internal static class Helper
    {
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

        internal static HashSet<Implicant<T>> GetMinimalImplicantSet<T>(
            HashSet<DnfAnd<T>> mintermSet,
            HashSet<Implicant<T>> implicantSet) where T : class
        {
            var sufficientImplicantSetOfSet = new HashSet<HashSet<Implicant<T>>>(HashSet<Implicant<T>>.CreateSetComparer());

            foreach(var minterm in mintermSet) {
                var sufficientImplicantSet = new HashSet<Implicant<T>>();

                foreach(var implicant in implicantSet) {
                    if(implicant.IsContainsMinterm(minterm))
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
                        .Select(implicant => implicant.GetUncombinedWeight())
                        .Sum();

                    return weight;
                })
                .DefaultIfEmpty(new HashSet<Implicant<T>>())
                .First();

            return minimalImplicantSet;
        }
    }
}
