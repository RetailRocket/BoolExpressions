namespace ConditionTree.BoolExpression.NonCanonicalForm
{
    public class NcfNot<TCondition>
        : INcfExpression<TCondition>
    {
        public NcfNot(
            INcfExpression<TCondition> ncfExpression)
        {
            this.NcfExpression = ncfExpression;
        }

        public INcfExpression<TCondition> NcfExpression { get; }
    }
}