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

        public override bool Equals(
            object obj)
        {
            return obj is DnfVariable<T> that && this.Equals(that);
        }

        public bool Equals(
            DnfVariable<T> that)
        {
            return that != null && that.Value.Equals(this.Value);
        }

        public override int GetHashCode()
        {
            return this.Value
                .GetHashCode();
        }
    }
}