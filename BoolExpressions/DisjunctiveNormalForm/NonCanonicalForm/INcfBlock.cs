namespace ConditionTree.BoolExpression.NonCanonicalForm
{
    using System.Collections.Generic;

    public interface INcfBlock<T>
        : INcfExpression<T>
    {
        INcfExpression<T>[] TermList { get; }
    }
}