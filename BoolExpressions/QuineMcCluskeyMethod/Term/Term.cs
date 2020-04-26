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
            object obj)
        {
            return Equals(obj as Term<T>);
        }

        public bool Equals(
            Term<T> obj)
        {
            return obj != null && obj.Variable == this.Variable;
        }

        public override int GetHashCode()
        {
            return Variable.GetHashCode();
        }
    }
}
