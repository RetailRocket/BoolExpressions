namespace BoolExpressions.NonCanonicalForm
{
    using System.Collections.Generic;
    using System.Linq;

    public class NonCanonicalBoolExpressionBuilder<T>
    {
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

        public INcfExpression<T> Or(
            INcfExpression<T> first,
            INcfExpression<T> second,
            params INcfExpression<T>[] tail)
        {
            var head = new NcfOrBlock<T>(
                termA: first,
                termB: second);

            if (!tail.Any())
                return head;

            return this.Or(
                first: head,
                second: tail.First(),
                tail: tail.Skip(1)
                    .ToArray());
        }

        public INcfExpression<T> And(
            INcfExpression<T> first,
            INcfExpression<T> second,
            params INcfExpression<T>[] tail)
        {
            var head = new NcfAndBlock<T>(
                termA: first,
                termB: second);

            if (!tail.Any())
                return head;

            return this.And(
                first: head,
                second: tail.First(),
                tail: tail.Skip(1)
                    .ToArray());
        }
    }
}