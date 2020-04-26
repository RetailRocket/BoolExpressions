namespace BoolExpressions.DisjunctiveNormalForm
{
    using System.Collections.Generic;

    public class DnfExpression<T>
    {
        public DnfExpression(
            IReadOnlyCollection<DnfAnd<T>> andBlockList)
        {
            this.AndBlockList = andBlockList;
        }

        public IReadOnlyCollection<DnfAnd<T>> AndBlockList { get; }
    }
}