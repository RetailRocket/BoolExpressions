using System;
using System.Collections.Generic;
using System.Linq;
using BoolExpressions.QuineMcCluskeyMethod.Term;
using System.Runtime.CompilerServices;
using BoolExpressions.DisjunctiveNormalForm;
using BoolExpressions.DisjunctiveNormalForm.Operation;

[assembly:InternalsVisibleTo("UnitTests")]

namespace BoolExpressions.QuineMcCluskeyMethod
{    
    internal static class ImplicantExtension
    {
        internal static int GetPositiveWeight<T>(
            this Implicant<T> implicant) where T : class
        {
            return implicant
                .TermSet
                .Count(term => term is PositiveTerm<T>);
        }

        internal static int GetUncombinedWeight<T>(
            this Implicant<T> implicant) where T : class
        {
            return implicant
                .TermSet
                .Count(term =>
                    term is PositiveTerm<T> ||
                    term is NegativeTerm<T>);
        }

        internal static bool IsContainsMinterm<T>(
            this Implicant<T> implicant,
            DnfAnd<T> minterm) where T : class 
        {
            var variableMintermMapB = minterm
                .ElementSet
                .ToDictionary(term => term.Value, term => term);

            return !implicant
                .TermSet
                .Any(implicantTerm =>
                {
                    var mintermTerm = variableMintermMapB[implicantTerm.Value];
                    return (implicantTerm, mintermTerm) switch
                    {
                        (PositiveTerm<T> _, DnfNotVariable<T> _) => true,
                        (NegativeTerm<T> _, DnfVariable<T> _) => true,
                        _ => false
                    };
                });
        }
    }
}
