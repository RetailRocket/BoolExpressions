namespace UnitTests.QuineMcCluskeyMethod
{
    using System.Collections.Generic;
    using BoolExpressions.QuineMcCluskeyMethod;
    using Xunit;
    using BoolExpressions.QuineMcCluskeyMethod.Term;
    using static BoolExpressions.QuineMcCluskeyMethod.Term.Factories;

    public class BuildTests
    {
        [Fact]
        public void CombinedVariableDistance()
        {
            var a = new Implicant<string>(new HashSet<Term<string>> {
                PositiveTermOf("A"),
                PositiveTermOf("B"),
                CombinedTermOf("C")
            });

            var b = new Implicant<string>(new HashSet<Term<string>> {
                NegativeTermOf("A"),
                CombinedTermOf("B"),
                CombinedTermOf("C")
            });

            Assert.Equal(
              expected: 1,
              actual: ImplicantHelpers.getCombinedVariableDistance(a, b));
        }

        [Fact]
        public void CombineImplicants()
        {
            var a = new Implicant<string>(new HashSet<Term<string>> {
                NegativeTermOf("A"),
                PositiveTermOf("B"),
                NegativeTermOf("C"),
                NegativeTermOf("D")
            });

            var b = new Implicant<string>(new HashSet<Term<string>> {
                PositiveTermOf("A"),
                PositiveTermOf("B"),
                NegativeTermOf("C"),
                NegativeTermOf("D")
            });

            var actual = ImplicantHelpers.combineImplicants(a, b);

            var expected = new Implicant<string>(new HashSet<Term<string>> {
                CombinedTermOf("A"),
                PositiveTermOf("B"),
                NegativeTermOf("C"),
                NegativeTermOf("D")
            });

            Assert.Equal(
                expected: expected,
                actual: actual);
        }
    }
}
