using System;
using System.Collections.Generic;
using System.Linq;

namespace BoolExpressions.QuineMcCluskeyMethod.Term
{
    internal abstract class Term<T> where T : class
    {
        public Term(
            T value)
        {
            Value = value;
        }

        public T Value { get; }

        public override bool Equals(
            object that)
        {
            return Equals(that as Term<T>);
        }

        public bool Equals(
            Term<T> that)
        {
            return that != null && that.GetType() == this.GetType() && that.Value.Equals(this.Value);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(GetType().GetHashCode(), Value.GetHashCode());
        }
    }
}
