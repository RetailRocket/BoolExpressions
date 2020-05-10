using System;
using System.Collections.Generic;
using System.Linq;
using BoolExpressions.QuineMcCluskeyMethod.Term;
using System.Runtime.CompilerServices;
using BoolExpressions.DisjunctiveNormalForm;

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
                var candidateImplicantSet = finalImplicantSet
                    .Where(implicant => implicant.IsContainsMinterm(minterm));

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
