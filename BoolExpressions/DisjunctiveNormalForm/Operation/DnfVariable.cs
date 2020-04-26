namespace BoolExpressions.DisjunctiveNormalForm.Operation
{
    public class DnfVariable<T>
        : IDnfOperation<T>
    {
        public DnfVariable(
            T value)
        {
            this.Value = value;
        }

        public T Value { get; set; }
    }
}