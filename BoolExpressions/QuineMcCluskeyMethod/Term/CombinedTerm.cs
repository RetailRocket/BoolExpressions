using System;
using System.Collections.Generic;
using System.Linq;

namespace BoolExpressions.QuineMcCluskeyMethod.Term
{
    internal class CombinedTerm<T>
        : Term<T> where T : class
    {
        public CombinedTerm(
            T variable) : base(variable)
        {
        }
    }
}