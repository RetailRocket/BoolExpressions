using System;
using System.Collections.Generic;
using System.Linq;
using BoolExpressions.QuineMcCluskeyMethod.Term;
using System.Runtime.CompilerServices;

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
    }
}
