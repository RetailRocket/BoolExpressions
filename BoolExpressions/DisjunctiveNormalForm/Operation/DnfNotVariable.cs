using System;

namespace BoolExpressions.DisjunctiveNormalForm.Operation
{
    public class DnfNotVariable<T>
        : IDnfVariable<T> where T : class
    {
        public DnfNotVariable(
            T value)
        {
            this.Value = value;
        }

        public T Value { get; }

        override public bool Equals(
            object that)
        {
            return Equals(that as DnfNotVariable<T>);
        }

        public bool Equals(
            DnfNotVariable<T> that)
        {
            return that != null && that.Value.Equals(this.Value);
        }

        override public int GetHashCode()
        {
            return HashCode.Combine(GetType().GetHashCode(), Value.GetHashCode());
        }
    }
}