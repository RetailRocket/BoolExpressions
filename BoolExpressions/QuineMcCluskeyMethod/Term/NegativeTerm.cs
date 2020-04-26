using System;
using System.Collections.Generic;
using System.Linq;

namespace BoolExpressions.QuineMcCluskeyMethod.Term
{
    public class NegativeTerm<T> : Term<T> where T : class
    {
        public NegativeTerm(
            T variable) : base(variable)
        {
        }
    }
}
