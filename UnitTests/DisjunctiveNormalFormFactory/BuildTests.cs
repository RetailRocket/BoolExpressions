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

            BoolExpressions.DisjunctiveNormalFormFactory
                .Factory
                .Build(ncfExpression: exp);
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
                .Factory
                .Build(ncfExpression: exp);
        }
    }
}