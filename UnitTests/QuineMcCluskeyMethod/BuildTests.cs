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
    using System.Collections.Immutable;

    public class BuildTests
    {
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
              actual: ImplicantSetFinalizeExtension.GetImplicantPositiveWeight(implicant));
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
            
            var actualFinalImplicantSet = ImplicantSetFinalizeExtension.GetFinalImplicantSet(
                implicantSet: implicantSet);
            
            Assert.Equal(   
                expected: finalImplicantSet,
                actual: actualFinalImplicantSet); 
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

        [Fact]
        public void PetrickMethod()
        {
            var implicantSet = new HashSet<Implicant<string>> {
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
                )
            };

            var mintermSet = new HashSet<DnfAnd<string>> {
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

            var actualMinimalImplicantSet = ImplicantHelpers.PetrickMethod(
                mintermSet: mintermSet,
                implicantSet: implicantSet
            );

            var expectedMinimalImplicantSet1 = new HashSet<Implicant<string>> {
                ImplicantOf(
                    PositiveTermOf("A"),
                    NegativeTermOf("B"),
                    CombinedTermOf("C"),
                    CombinedTermOf("D")
                )
            };

            var expectedMinimalImplicantSet2 = new HashSet<Implicant<string>> {
                ImplicantOf(
                    PositiveTermOf("A"),
                    CombinedTermOf("B"),
                    CombinedTermOf("C"),
                    NegativeTermOf("D")
                )
            };

            var comparer = HashSet<Implicant<string>>.CreateSetComparer();

            Assert.True(
                comparer.Equals(actualMinimalImplicantSet, expectedMinimalImplicantSet1) ||
                comparer.Equals(actualMinimalImplicantSet, expectedMinimalImplicantSet2));
        }

        [Fact]
        public void ProcessDnf()
        {
            var dnfExpression = new DnfExpression<string>(new HashSet<DnfAnd<string>> {
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
                    DnfNotOf(
                        DnfVariableOf("C")),
                    DnfVariableOf("D")
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
                    DnfNotOf(
                        DnfVariableOf("D"))
                ),
                DnfAndOf(
                    DnfVariableOf("A"),
                    DnfVariableOf("B"),
                    DnfVariableOf("C"),
                    DnfVariableOf("D")
                )
            });

            var actualProcessedDnfExpression = ImplicantHelpers.ProcessDnf(
                dnfExpression: dnfExpression
            );

            var expectedProcessedDnfExpression1 = new DnfExpression<string>(new HashSet<DnfAnd<string>> {
                DnfAndOf(
                    DnfVariableOf("B"),
                    DnfNotOf(
                        DnfVariableOf("C")),
                    DnfNotOf(
                        DnfVariableOf("D"))
                ),
                DnfAndOf(
                    DnfVariableOf("A"),
                    DnfNotOf(
                        DnfVariableOf("B"))
                ),
                DnfAndOf(
                    DnfVariableOf("A"),
                    DnfVariableOf("C")
                ),
                DnfAndOf(
                    DnfVariableOf("A"),
                    DnfNotOf(
                        DnfVariableOf("B")),
                    DnfVariableOf("C"),
                    DnfVariableOf("D")
                )
            });

            var expectedProcessedDnfExpression2 = new DnfExpression<string>(new HashSet<DnfAnd<string>> {
                DnfAndOf(
                    DnfVariableOf("B"),
                    DnfNotOf(
                        DnfVariableOf("C")),
                    DnfNotOf(
                        DnfVariableOf("D"))
                ),
                DnfAndOf(
                    DnfVariableOf("A"),
                    DnfNotOf(
                        DnfVariableOf("B"))
                ),
                DnfAndOf(
                    DnfVariableOf("A"),
                    DnfVariableOf("C")
                ),
                DnfAndOf(
                    DnfVariableOf("A"),
                    DnfNotOf(
                        DnfVariableOf("B")),
                    DnfNotOf(
                        DnfVariableOf("C")),
                    DnfNotOf(
                        DnfVariableOf("D"))
                )
            });

            Assert.True(
                actualProcessedDnfExpression.Equals(expectedProcessedDnfExpression1) ||
                actualProcessedDnfExpression.Equals(expectedProcessedDnfExpression2));
        }
    }
}
