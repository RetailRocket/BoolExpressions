using System;

namespace BoolExpressions.DisjunctiveNormalForm.Operation
{
    public class DnfVariable<T>
        : IDnfOperation<T> where T : class
    {
        public DnfVariable(
            T value)
        {
            this.Value = value;
        }

        public T Value { get; set; }

        override public string ToString()
        {
            return $"{Value}";
        }

        public override bool Equals(
            object that)
        {
            return Equals(that as DnfVariable<T>);
        }

        public bool Equals(
            DnfVariable<T> that)
        {
            return that != null && that.Value.Equals(this.Value);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(GetType().GetHashCode(), Value.GetHashCode());
        }
    }
}