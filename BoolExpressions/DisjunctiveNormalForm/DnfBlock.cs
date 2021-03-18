namespace BoolExpressions.DisjunctiveNormalForm
{
    using BoolExpressions.DisjunctiveNormalForm.Operation;

    public class DnfBlock<T>
    {
        private readonly ImmutableNotEmptyHashSet<IDnfVariable<T>> varSet;

        public DnfBlock(
            ImmutableNotEmptyHashSet<IDnfVariable<T>> varSet)
        {
            this.varSet = varSet;
        }

        public override int GetHashCode()
        {
            return this.varSet
                .GetHashCode();
        }
    }
}