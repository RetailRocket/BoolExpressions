namespace BoolExpressions.DisjunctiveNormalForm
{
    using System.Collections.Generic;

    public class DnfExpression<T> where T : class
    {
        public DnfExpression(
            IReadOnlyCollection<DnfAnd<T>> andBlockList)
        {
            this.AndBlockList = andBlockList;
        }

        public IReadOnlyCollection<DnfAnd<T>> AndBlockList { get; }
    }
}