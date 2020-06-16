using System;
using System.Collections.Generic;
using System.Linq;

namespace BoolExpressions.QuineMcCluskeyMethod.Term
{
    internal abstract class Term<T>
    {
        public Term(
            T value)
        {
            Value = value;
        }

        public T Value { get; }
    }
}
