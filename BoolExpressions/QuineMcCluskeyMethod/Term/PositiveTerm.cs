using System;
using System.Collections.Generic;
using System.Linq;

namespace BoolExpressions.QuineMcCluskeyMethod.Term
{
    internal class PositiveTerm<T>
        : Term<T>
    {
        public PositiveTerm(
            T value) : base(value)
        {
        }

        public override bool Equals(
            object obj)
        {
            return obj is PositiveTerm<T> that && this.Equals(that);
        }

        public bool Equals(
            PositiveTerm<T> that)
        {
            return that != null && that.Value.Equals(this.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
