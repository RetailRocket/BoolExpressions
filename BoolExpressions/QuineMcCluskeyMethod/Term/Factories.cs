using System;
using System.Collections.Generic;
using System.Linq;

namespace BoolExpressions.QuineMcCluskeyMethod.Term
{
    internal class Factories {
        public static PositiveTerm<T> PositiveTermOf<T>(
            T variable) where T : class
        {
            return new PositiveTerm<T>(variable);
        }

        public static NegativeTerm<T> NegativeTermOf<T>(
            T variable) where T : class
        {
            return new NegativeTerm<T>(variable);
        }

        public static CombinedTerm<T> CombinedTermOf<T>(
            T variable) where T : class
        {
            return new CombinedTerm<T>(variable);
        }
    }
}
