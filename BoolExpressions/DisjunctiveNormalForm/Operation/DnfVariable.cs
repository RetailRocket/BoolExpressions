using System;

namespace BoolExpressions.DisjunctiveNormalForm.Operation
{
    public class DnfVariable<T>
        : IDnfVariable<T>
    {
        public DnfVariable(
            T value)
        {
            this.Value = value;
        }

        public T Value { get; }

        override public bool Equals(
            object obj)
        {
            return obj is DnfVariable<T> that && this.Equals(that);
        }

        public bool Equals(
            DnfVariable<T> that)
        {
            return that != null && that.Value.Equals(this.Value);
        }

        override public int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}