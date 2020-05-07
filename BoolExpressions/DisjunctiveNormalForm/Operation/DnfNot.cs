using System;

namespace BoolExpressions.DisjunctiveNormalForm.Operation
{
    public class DnfNot<T>
        : IDnfOperation<T> where T : class
    {
        public DnfNot(
            DnfVariable<T> variable)
        {
            this.Variable = variable;
        }

        public DnfVariable<T> Variable { get; set; }

        override public bool Equals(
            object that)
        {
            return Equals(that as DnfNot<T>);
        }

        public bool Equals(
            DnfNot<T> that)
        {
            return that != null && that.Variable.Equals(this.Variable);
        }

        override public int GetHashCode()
        {
            return HashCode.Combine(GetType().GetHashCode(), Variable.GetHashCode());
        }
    }
}