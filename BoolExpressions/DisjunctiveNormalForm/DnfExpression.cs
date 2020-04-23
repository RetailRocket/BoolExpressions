namespace BoolExpressions.DisjunctiveNormalForm
{
    using System.Collections.Generic;

    public class DnfExpression<T>
    {
        public DnfExpression(
            IReadOnlyCollection<DfnAndBlock<T>> andBlockList)
        {
            this.AndBlockList = andBlockList;
        }

        public IReadOnlyCollection<DfnAndBlock<T>> AndBlockList { get; }
    }
}