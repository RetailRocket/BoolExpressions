namespace BoolExpressions.DisjunctiveNormalForm.Operation
{
    public interface IDnfVariable<T>
    {
        public T Value { get; }
    }
}