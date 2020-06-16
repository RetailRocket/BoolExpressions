using System;
using System.Collections.Generic;
using System.Linq;

namespace BoolExpressions.QuineMcCluskeyMethod.Term
{
    internal class NegativeTerm<T>
        : Term<T>
    {
        public NegativeTerm(
            T value) : base(value)
        {
        }

        public override bool Equals(
            object obj)
        {
            return obj is NegativeTerm<T> that && this.Equals(that);
        }

        public bool Equals(
            NegativeTerm<T> that)
        {
            return that != null && that.Value.Equals(this.Value);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}
