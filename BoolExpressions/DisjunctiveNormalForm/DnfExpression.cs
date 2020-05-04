namespace BoolExpressions.DisjunctiveNormalForm
{
    using System.Collections.Generic;

    public class DnfExpression<T> where T : class
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
            return Equals(obj as DnfExpression<T>);
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