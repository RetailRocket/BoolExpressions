namespace ConditionTree.BoolExpression.NonCanonicalForm
{
    public class NcfVariable<T>
        : INcfExpression<T>
    {
        public NcfVariable(
            T value)
        {
            this.Value = value;
        }

        public T Value { get; set; }
    }
}