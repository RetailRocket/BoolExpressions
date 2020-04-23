namespace UnitTests.DisjunctiveNormalFormFactory
{
    using BoolExpressions.NonCanonicalForm;
    using Xunit;

    public class BuildTests
    {
        [Fact]
        public void WorkFineA()
        {
            var builder = new NonCanonicalBoolExpressionBuilder<string>();

            var exp = builder.Or(
                    new NcfVariable<string>("A"),
                builder.And(
                    new NcfVariable<string>("B"),
                    new NcfVariable<string>("C")));

            var dnfExp = BoolExpressions.DisjunctiveNormalFormFactory
                .Build(ncfExpression: exp);

            Assert.Equal(
                expected: 5,
                actual: dnfExp.AndBlockList.Count);
        }

        [Fact]
        public void WorkFineB()
        {
            var builder = new NonCanonicalBoolExpressionBuilder<string>();

            var exp = builder.Or(
                new NcfVariable<string>("A"),
                new NcfVariable<string>("B"),
                builder.And(
                    new NcfVariable<string>("C"),
                    new NcfVariable<string>("D")));

            var dnfExp = BoolExpressions.DisjunctiveNormalFormFactory
                .Build(ncfExpression: exp);

            Assert.Equal(
                expected: 13,
                actual: dnfExp.AndBlockList.Count);
        }
    }
}