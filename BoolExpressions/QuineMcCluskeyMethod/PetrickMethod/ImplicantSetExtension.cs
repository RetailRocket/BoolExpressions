using System;
using System.Collections.Generic;
using System.Linq;
using BoolExpressions.QuineMcCluskeyMethod.Term;
using System.Runtime.CompilerServices;

[assembly:InternalsVisibleTo("UnitTests")]

namespace BoolExpressions.QuineMcCluskeyMethod.PetrickMethod
{    
    internal static class ImplicantSetExtension
    {
        internal static int GetUncombinedWeight<T>(
            this HashSet<Implicant<T>> implicantSet) where T : class
        {
            return implicantSet
                .Select(implicant => implicant.GetUncombinedWeight())
                .Sum();
        }
    }
}
