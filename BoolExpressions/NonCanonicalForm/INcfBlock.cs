namespace BoolExpressions.NonCanonicalForm
{
    public interface INcfBlock<T>
        : INcfExpression<T>
    {
        INcfExpression<T>[] TermList { get; }
    }
}