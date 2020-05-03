using System;
using System.Collections.Generic;
using System.Linq;

namespace BoolExpressions.QuineMcCluskeyMethod.Term
{
    public abstract class Term<T> where T : class
    {
        public Term(
            T variable)
        {
            Variable = variable;
        }

        public T Variable { get; }

        public override bool Equals(
            object that)
        {
            return Equals(that as Term<T>);
        }

        public bool Equals(
            Term<T> that)
        {
            return that != null && that.GetType() == this.GetType() && that.Variable.Equals(this.Variable);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(GetType().GetHashCode(), Variable.GetHashCode());
        }
    }
}
