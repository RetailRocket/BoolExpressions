using System;
using System.Collections.Generic;
using System.Linq;

namespace BoolExpressions.QuineMcCluskeyMethod.Term
{
    internal class Factory {
        public static PositiveTerm<T> PositiveTermOf<T>(
            T value)
        {
            return new PositiveTerm<T>(value);
        }

        public static NegativeTerm<T> NegativeTermOf<T>(
            T value)
        {
            return new NegativeTerm<T>(value);
        }

        public static CombinedTerm<T> CombinedTermOf<T>(
            T value)
        {
            return new CombinedTerm<T>(value);
        }
    }
}
