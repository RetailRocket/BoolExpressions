using System;
using System.Collections.Generic;
using System.Linq;

namespace BoolExpressions.QuineMcCluskeyMethod.Term
{
    internal class NegativeTerm<T>
        : Term<T> where T : class
    {
        public NegativeTerm(
            T value) : base(value)
        {
        }
    }
}
