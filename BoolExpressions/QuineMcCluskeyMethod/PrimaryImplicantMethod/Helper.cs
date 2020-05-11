using System;
using System.Collections.Generic;
using System.Linq;
using BoolExpressions.QuineMcCluskeyMethod.Term;
using System.Runtime.CompilerServices;
using BoolExpressions.DisjunctiveNormalForm;
using Optional.Collections;
using Optional.Unsafe;

[assembly:InternalsVisibleTo("UnitTests")]

namespace BoolExpressions.QuineMcCluskeyMethod.PrimaryImplicantMethod
{
    internal static class Helper
    {
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
                var primaryImplicantOptional = finalImplicantSet
                    .Where(implicant => implicant.IsContainsMinterm(minterm))
                    .SingleOrNone();

                foreach(var primaryImplicant in primaryImplicantOptional)
                {
                    processedMintermSet.Add(minterm);
                    primaryImplicantSet.Add(primaryImplicant);
                }
            }

            finalMintermSet = mintermSet
                .Except(processedMintermSet)
                .ToHashSet();
        }
    }
}
