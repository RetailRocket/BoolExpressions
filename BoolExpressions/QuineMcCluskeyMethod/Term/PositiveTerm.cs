using System;
using System.Collections.Generic;
using System.Linq;

namespace BoolExpressions.QuineMcCluskeyMethod.Term
{
    internal class PositiveTerm<T>
        : Term<T> where T : class
    {
        public PositiveTerm(
            T variable) : base(variable)
        {
        }

        override public string ToString()
        {
            return $"Positive({Variable})";
        }
    }
}
