using System;
using System.Collections.Generic;
using System.Linq;

namespace BoolExpressions.QuineMcCluskeyMethod.Term
{
    internal class CombinedTerm<T>
        : Term<T>
    {
        public CombinedTerm(
            T value) : base(value)
        {
        }

        public override bool Equals(
            object obj)
        {
            return obj is CombinedTerm<T> that && this.Equals(that);
        }

        public bool Equals(
            CombinedTerm<T> that)
        {
            return that != null && that.Value.Equals(this.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
