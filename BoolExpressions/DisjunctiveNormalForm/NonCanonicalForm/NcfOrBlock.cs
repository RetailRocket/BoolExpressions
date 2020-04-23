namespace BoolExpressions.DisjunctiveNormalForm.NonCanonicalForm
{
    public class NcfOrBlock<T>
        : INcfBlock<T>
    {
        public NcfOrBlock(
            params INcfExpression<T>[] termList)
        {
            this.TermList = termList;
        }

        public INcfExpression<T>[] TermList { get; }
    }
}