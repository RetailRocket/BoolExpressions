namespace BoolExpressions.DisjunctiveNormalForm
{
    using System.Collections.Generic;

    public class DnfExpression<T>
    {
        public DnfExpression(
            IReadOnlyCollection<DfnAnd<T>> andBlockList)
        {
            this.AndBlockList = andBlockList;
        }

        public IReadOnlyCollection<DfnAnd<T>> AndBlockList { get; }
    }
}