namespace UnitTests.QuineMcCluskeyMethod
{
    using System.Collections.Generic;
    using BoolExpressions.QuineMcCluskeyMethod;
    using Xunit;
    using static BoolExpressions.QuineMcCluskeyMethod.Factories;
    using static BoolExpressions.QuineMcCluskeyMethod.Term.Factories;
    using static BoolExpressions.DisjunctiveNormalForm.Factories;
    using static BoolExpressions.DisjunctiveNormalForm.Operation.Factories;
    using System;
    using BoolExpressions.NonCanonicalForm;
    using BoolExpressions.DisjunctiveNormalForm;

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
            var mintermSet = new HashSet<Implicant<string>> {
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

            var finalImplicantSet = new HashSet<Implicant<string>> {
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
            
            var actualFinalImplicantSet = ImplicantHelpers.GetFinalImplicantSet(
                implicantSet: mintermSet);
            
            Assert.Equal(   
                expected: finalImplicantSet,
                actual: actualFinalImplicantSet); 
        }

        [Fact]
        public void IsImplicantContains()
        {
            var implicant = ImplicantOf(
                CombinedTermOf("A"),
                PositiveTermOf("B"),
                NegativeTermOf("C"),
                NegativeTermOf("D")
            );

            {
                var minterm = DnfAndOf(
                    DnfVariableOf("A"),
                    DnfVariableOf("B"),
                    DnfNotOf(
                        DnfVariableOf("C")),
                    DnfNotOf(
                        DnfVariableOf("D")));

                Assert.Equal(   
                    expected: true,
                    actual: ImplicantHelpers.IsImplicantContains(
                        implicant: implicant,
                        minterm: minterm));
            }

            {
                var minterm = DnfAndOf(
                    DnfVariableOf("A"),
                    DnfVariableOf("B"),
                    DnfNotOf(
                        DnfVariableOf("C")),
                    DnfVariableOf("D"));

                Assert.Equal(   
                    expected: false,
                    actual: ImplicantHelpers.IsImplicantContains(
                        implicant: implicant,
                        minterm: minterm));
            }
        }

        [Fact]
        public void GetPrimaryImplicantSet()
        {
            var mintermSet = new HashSet<DnfAnd<string>> {
                DnfAndOf(
                    DnfNotOf(
                        DnfVariableOf("A")),
                    DnfVariableOf("B"),
                    DnfNotOf(
                        DnfVariableOf("C")),
                    DnfNotOf(
                        DnfVariableOf("D"))
                ),
                DnfAndOf(
                    DnfVariableOf("A"),
                    DnfNotOf(
                        DnfVariableOf("B")),
                    DnfNotOf(
                        DnfVariableOf("C")),
                    DnfNotOf(
                        DnfVariableOf("D"))
                ),
                DnfAndOf(
                    DnfVariableOf("A"),
                    DnfNotOf(
                        DnfVariableOf("B")),
                    DnfVariableOf("C"),
                    DnfNotOf(
                        DnfVariableOf("D"))
                ),
                DnfAndOf(
                    DnfVariableOf("A"),
                    DnfVariableOf("B"),
                    DnfNotOf(
                        DnfVariableOf("C")),
                    DnfNotOf(
                        DnfVariableOf("D"))
                ),
                DnfAndOf(
                    DnfVariableOf("A"),
                    DnfNotOf(
                        DnfVariableOf("B")),
                    DnfVariableOf("C"),
                    DnfVariableOf("D")
                ),
                DnfAndOf(
                    DnfVariableOf("A"),
                    DnfVariableOf("B"),
                    DnfVariableOf("C"),
                    DnfVariableOf("D")
                )
            };

            var finalImplicantSet = new HashSet<Implicant<string>> {
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

            HashSet<DnfAnd<string>> actualFinalMintermSet;
            HashSet<Implicant<string>> actualPrimaryImplicantSet;
            
            ImplicantHelpers.GetPrimaryImplicantSet(
                mintermSet: mintermSet,
                finalImplicantSet: finalImplicantSet,
                finalMintermSet: out actualFinalMintermSet,
                primaryImplicantSet: out actualPrimaryImplicantSet);

            var expectedPrimaryImplicantSet = new HashSet<Implicant<string>> {
                ImplicantOf(
                    CombinedTermOf("A"),
                    PositiveTermOf("B"),
                    NegativeTermOf("C"),
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
                expected: expectedPrimaryImplicantSet,
                actual: actualPrimaryImplicantSet);

            var expectedFinalMintermSet = new HashSet<DnfAnd<string>> {
                DnfAndOf(
                    DnfVariableOf("A"),
                    DnfNotOf(
                        DnfVariableOf("B")),
                    DnfNotOf(
                        DnfVariableOf("C")),
                    DnfNotOf(
                        DnfVariableOf("D"))
                ),
                DnfAndOf(
                    DnfVariableOf("A"),
                    DnfNotOf(
                        DnfVariableOf("B")),
                    DnfVariableOf("C"),
                    DnfNotOf(
                        DnfVariableOf("D"))
                ),
                DnfAndOf(
                    DnfVariableOf("A"),
                    DnfVariableOf("B"),
                    DnfNotOf(
                        DnfVariableOf("C")),
                    DnfNotOf(
                        DnfVariableOf("D"))
                ),
                DnfAndOf(
                    DnfVariableOf("A"),
                    DnfNotOf(
                        DnfVariableOf("B")),
                    DnfVariableOf("C"),
                    DnfVariableOf("D")
                ),
            };

            Assert.Equal(
                expected: expectedFinalMintermSet,
                actual: actualFinalMintermSet);
        }
    }
}
