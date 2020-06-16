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
            HashSet<HashSet<Implicant<T>>> implicantSetOfSet)
        {
            return implicantSetOfSet
                .Where(implicantLongSet =>
                    !implicantSetOfSet
                        .Any(implicantShortSet =>
                            implicantLongSet != implicantShortSet &&
                            implicantShortSet.IsSubsetOf(implicantLongSet)))
                .ToHashSet();
        }

        internal static HashSet<Implicant<T>> GetMinimalImplicantSet<T>(
            HashSet<DnfAnd<T>> mintermSet,
            HashSet<Implicant<T>> implicantSet)
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

            if (truncatedImplicantSetOfSet.Count > 0)
            {
                var minimalImplicantSetWeight = truncatedImplicantSetOfSet
                    .Min(implicantSet => implicantSet.GetUncombinedWeight());

                return truncatedImplicantSetOfSet
                    .First(implicantSet => implicantSet.GetUncombinedWeight() == minimalImplicantSetWeight);
            }
            else
            {
                return new HashSet<Implicant<T>>();
            }
        }
    }
}
