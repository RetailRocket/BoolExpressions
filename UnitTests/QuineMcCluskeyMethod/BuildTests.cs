namespace UnitTests.QuineMcCluskeyMethod
{
    using System.Collections.Generic;
    using BoolExpressions.QuineMcCluskeyMethod;
    using Xunit;
    using static BoolExpressions.QuineMcCluskeyMethod.Factories;
    using static BoolExpressions.QuineMcCluskeyMethod.Term.Factories;
    using System;

    public class BuildTests
    {
        [Fact]
        public void CombinedVariableDistance()
        {
            var a = ImplicantOf(
                PositiveTermOf("A"),
                PositiveTermOf("B"),
                CombinedTermOf("C")
            );

            var b = ImplicantOf(
                NegativeTermOf("A"),
                CombinedTermOf("B"),
                CombinedTermOf("C")
            );

            Assert.Equal(
              expected: 1,
              actual: ImplicantHelpers.GetCombinedVariableDistance(a, b));
        }

        [Fact]
        public void GetImplicantWeight()
        {
            var implicant = ImplicantOf(
                NegativeTermOf("A"),
                PositiveTermOf("B"),
                PositiveTermOf("C"),
                NegativeTermOf("D")
            );

            Assert.Equal(
              expected: 2,
              actual: ImplicantHelpers.GetImplicantWeight(implicant));
        }

        [Fact]
        public void CombineImplicants()
        {
            var a = ImplicantOf(
                NegativeTermOf("A"),
                PositiveTermOf("B"),
                NegativeTermOf("C"),
                NegativeTermOf("D")
            );

            var b = ImplicantOf(
                PositiveTermOf("A"),
                PositiveTermOf("B"),
                NegativeTermOf("C"),
                NegativeTermOf("D")
            );

            var actual = ImplicantHelpers.CombineImplicants(a, b);

            var expected = ImplicantOf(
                CombinedTermOf("A"),
                PositiveTermOf("B"),
                NegativeTermOf("C"),
                NegativeTermOf("D")
            );

            Assert.Equal(
                expected: expected,
                actual: actual);
        }

        [Fact]
        public void ProcessCurrentLevelImplicants()
        {
            var currentWightImplicantSet = new HashSet<Implicant<string>> {
                ImplicantOf(
                    NegativeTermOf("A"),
                    PositiveTermOf("B"),
                    NegativeTermOf("C"),
                    NegativeTermOf("D")
                ),
                ImplicantOf(
                    PositiveTermOf("A"),
                    NegativeTermOf("B"),
                    NegativeTermOf("C"),
                    NegativeTermOf("D")
                )
            };
            
            var nextWeightImplicantSet = new HashSet<Implicant<string>> {
                ImplicantOf(
                    PositiveTermOf("A"),
                    NegativeTermOf("B"),
                    NegativeTermOf("C"),
                    PositiveTermOf("D")
                ),
                ImplicantOf(
                    PositiveTermOf("A"),
                    NegativeTermOf("B"),
                    PositiveTermOf("C"),
                    NegativeTermOf("D")
                ),
                ImplicantOf(
                    PositiveTermOf("A"),
                    PositiveTermOf("B"),
                    NegativeTermOf("C"),
                    NegativeTermOf("D")
                ),
            };

            HashSet<Implicant<string>> actualCurrentLevelProcessedImplicants;
            HashSet<Implicant<string>> actualNextLevelImplicantSet;
            ImplicantHelpers.ProcessCurrentLevelImplicantSet(
                currentWightImplicantSet: currentWightImplicantSet,
                nextWeightImplicantSet: nextWeightImplicantSet,
                currentLevelProcessedImplicantSet: out actualCurrentLevelProcessedImplicants,
                nextLevelImplicantSet: out actualNextLevelImplicantSet);

            var expectedNextLevelImplicantSet = new HashSet<Implicant<string>> {
                ImplicantOf(
                    CombinedTermOf("A"),
                    PositiveTermOf("B"),
                    NegativeTermOf("C"),
                    NegativeTermOf("D")
                ),
                ImplicantOf(
                    PositiveTermOf("A"),
                    NegativeTermOf("B"),
                    NegativeTermOf("C"),
                    CombinedTermOf("D")
                ),
                ImplicantOf(
                    PositiveTermOf("A"),
                    NegativeTermOf("B"),
                    CombinedTermOf("C"),
                    NegativeTermOf("D")
                ),
                ImplicantOf(
                    PositiveTermOf("A"),
                    CombinedTermOf("B"),
                    NegativeTermOf("C"),
                    NegativeTermOf("D")
                )
            };

            Assert.Equal(
                expected: expectedNextLevelImplicantSet,
                actual: actualNextLevelImplicantSet);

            Assert.Equal(   
                expected: 5,
                actual: actualCurrentLevelProcessedImplicants.Count);
        }

        [Fact]
        public void GetFinalImplicantList()
        {
            var implicantSet = new HashSet<Implicant<string>> {
                ImplicantOf(
                    NegativeTermOf("A"),
                    PositiveTermOf("B"),
                    NegativeTermOf("C"),
                    NegativeTermOf("D")
                ),
                ImplicantOf(
                    PositiveTermOf("A"),
                    NegativeTermOf("B"),
                    NegativeTermOf("C"),
                    NegativeTermOf("D")
                ),
                ImplicantOf(
                    PositiveTermOf("A"),
                    NegativeTermOf("B"),
                    NegativeTermOf("C"),
                    PositiveTermOf("D")
                ),
                ImplicantOf(
                    PositiveTermOf("A"),
                    NegativeTermOf("B"),
                    PositiveTermOf("C"),
                    NegativeTermOf("D")
                ),
                ImplicantOf(
                    PositiveTermOf("A"),
                    PositiveTermOf("B"),
                    NegativeTermOf("C"),
                    NegativeTermOf("D")
                ),
                ImplicantOf(
                    PositiveTermOf("A"),
                    NegativeTermOf("B"),
                    PositiveTermOf("C"),
                    PositiveTermOf("D")
                ),
                ImplicantOf(
                    PositiveTermOf("A"),
                    PositiveTermOf("B"),
                    PositiveTermOf("C"),
                    NegativeTermOf("D")
                ),
                ImplicantOf(
                    PositiveTermOf("A"),
                    PositiveTermOf("B"),
                    PositiveTermOf("C"),
                    PositiveTermOf("D")
                )
            };

            var actualFinalImplicantSet = ImplicantHelpers.GetFinalImplicantSet(
                    implicantSet: implicantSet);

            var expectedFinalImplicantSet = new HashSet<Implicant<string>> {
                ImplicantOf(
                    CombinedTermOf("A"),
                    PositiveTermOf("B"),
                    NegativeTermOf("C"),
                    NegativeTermOf("D")
                ),
                ImplicantOf(
                    PositiveTermOf("A"),
                    NegativeTermOf("B"),
                    CombinedTermOf("C"),
                    CombinedTermOf("D")
                ),
                ImplicantOf(
                    PositiveTermOf("A"),
                    CombinedTermOf("B"),
                    CombinedTermOf("C"),
                    NegativeTermOf("D")
                ),
                ImplicantOf(
                    PositiveTermOf("A"),
                    CombinedTermOf("B"),
                    PositiveTermOf("C"),
                    CombinedTermOf("D")
                )
            };

            Assert.Equal(   
                expected: expectedFinalImplicantSet,
                actual: actualFinalImplicantSet); 
        }
    }
}
