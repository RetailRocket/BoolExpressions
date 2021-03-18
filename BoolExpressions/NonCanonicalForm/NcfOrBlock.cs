namespace BoolExpressions.NonCanonicalForm
{
    public class NcfOrBlock<T>
        : INcfExpression<T>
    {
        public NcfOrBlock(
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