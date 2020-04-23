namespace BoolExpressions.DisjunctiveNormalForm.NonCanonicalForm
{
    public class NcfAndBlock<T>
        : INcfBlock<T>
    {
        public NcfAndBlock(
            params INcfExpression<T>[] termList)
        {
            this.TermList = termList;
        }

        public INcfExpression<T>[] TermList { get; set; }

        public static INcfExpression<T> Of(
            params INcfExpression<T>[] termList)
        {
            return new NcfAndBlock<T>(
                termList: termList);
        }
    }
}