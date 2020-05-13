namespace BoolExpressions.DisjunctiveNormalForm.Operation
{
    public interface IDnfVariable<T>
    {
        T Value { get; }
    }
}