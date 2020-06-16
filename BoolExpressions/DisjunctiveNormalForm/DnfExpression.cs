namespace BoolExpressions.DisjunctiveNormalForm
{
    using System.Collections.Generic;

    public class DnfExpression<T>
    {
        public DnfExpression(
            HashSet<DnfAnd<T>> andBlockSet)
        {
            this.AndBlockSet = andBlockSet;
        }

        public HashSet<DnfAnd<T>> AndBlockSet { get; }

        public override bool Equals(
            object obj)
        {
            return obj is DnfExpression<T> that && this.Equals(that);
        }

        public bool Equals(
            DnfExpression<T> that)
        {
            return that != null && HashSet<DnfAnd<T>>.CreateSetComparer().Equals(this.AndBlockSet, that.AndBlockSet);
        }

        public override int GetHashCode()
        {
            return HashSet<DnfAnd<T>>.CreateSetComparer().GetHashCode(AndBlockSet);
        }
    }
}