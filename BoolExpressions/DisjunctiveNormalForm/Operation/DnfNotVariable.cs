using System;

namespace BoolExpressions.DisjunctiveNormalForm.Operation
{
    public class DnfNotVariable<T>
        : IDnfVariable<T>
    {
        public DnfNotVariable(
            T value)
        {
            this.Value = value;
        }

        public T Value { get; }

        override public bool Equals(
            object obj)
        {
            return obj is DnfNotVariable<T> that && this.Equals(that);
        }

        public bool Equals(
            DnfNotVariable<T> that)
        {
            return that != null && that.Value.Equals(this.Value);
        }

        override public int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}