namespace BoolExpressions.DisjunctiveNormalForm.NonCanonicalForm
{
    public class NonCanonicalBoolExpressionBuilder<T>
    {
        public NcfAndBlock<T> And(
            params INcfExpression<T>[] termList)
        {
            return new NcfAndBlock<T>(
                termList: termList);
        }

        public NcfOrBlock<T> Or(
            params INcfExpression<T>[] termList)
        {
            return new NcfOrBlock<T>(
                termList: termList);
        }

        public NcfVariable<T> Variable(
            T value)
        {
            return new NcfVariable<T>(
                value: value);
        }

        public NcfNot<T> Not(
            INcfExpression<T> ncfExpression)
        {
            return new NcfNot<T>(
                ncfExpression: ncfExpression);
        }
    }
}