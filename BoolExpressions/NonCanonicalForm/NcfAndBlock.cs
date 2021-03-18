namespace BoolExpressions.NonCanonicalForm
{
    public class NcfAndBlock<T>
        : INcfExpression<T>
    {
        public NcfAndBlock(
            INcfExpression<T> termA,
            INcfExpression<T> termB)
        {
            this.TermA = termA;
            this.TermB = termB;
        }

        public INcfExpression<T> TermA { get; set; }

        public INcfExpression<T> TermB { get; set; }
    }
}