namespace BoolExpressions.DisjunctiveNormalForm
{
    using System.Collections.Generic;

    public class DnfExpression<T>
    {
        public ImmutableNotEmptyHashSet<DnfBlock<T>> BlockSet { get; }

        public DnfExpression(
            ImmutableNotEmptyHashSet<DnfBlock<T>> blockSet)
        {
            this.BlockSet = blockSet;
        }

        public override bool Equals(
            object obj)
        {
            return obj is DnfExpression<T> that && this.Equals(that);
        }

        public bool Equals(
            DnfExpression<T> that)
        {
            return that != null && this.BlockSet.Equals(that.BlockSet);
        }

        public override int GetHashCode()
        {
            return this.BlockSet
                .GetHashCode();
        }
    }
}