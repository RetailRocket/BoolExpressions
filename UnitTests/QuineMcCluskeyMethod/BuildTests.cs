namespace UnitTests.QuineMcCluskeyMethod
{
    using System.Collections.Generic;
    using BoolExpressions.QuineMcCluskeyMethod;
    using Xunit;
    using static BoolExpressions.QuineMcCluskeyMethod.Factory;
    using static BoolExpressions.QuineMcCluskeyMethod.Term.Factory;
    using static BoolExpressions.DisjunctiveNormalForm.Factory;
    using static BoolExpressions.DisjunctiveNormalForm.Operation.Factory;
    using System;
    using BoolExpressions.DisjunctiveNormalForm;
    using BoolExpressions.QuineMcCluskeyMethod.FinalImplicantMethod;

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
              actual: implicant.GetPositiveWeight());
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
            
            var actualFinalImplicantSet = implicantSet.GetFinalImplicantSet();
            
            Assert.Equal(   
                expected: finalImplicantSet,
                actual: actualFinalImplicantSet); 
        }

        [Fact]
        public void GetPrimaryImplicantSet()
        {
            var mintermSet = new HashSet<DnfAnd<string>> {
                DnfAndOf(
                    DnfNotVariableOf("A"),
                    DnfVariableOf("B"),
                    DnfNotVariableOf("C"),
                    DnfNotVariableOf("D")
                ),
                DnfAndOf(
                    DnfVariableOf("A"),
                    DnfNotVariableOf("B"),
                    DnfNotVariableOf("C"),
                    DnfNotVariableOf("D")
                ),
                DnfAndOf(
                    DnfVariableOf("A"),
                    DnfNotVariableOf("B"),
                    DnfVariableOf("C"),
                    DnfNotVariableOf("D")
                ),
                DnfAndOf(
                    DnfVariableOf("A"),
                    DnfVariableOf("B"),
                    DnfNotVariableOf("C"),
                    DnfNotVariableOf("D")
                ),
                DnfAndOf(
                    DnfVariableOf("A"),
                    DnfNotVariableOf("B"),
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

            BoolExpressions.QuineMcCluskeyMethod.PrimaryImplicantMethod.Helper.GetPrimaryImplicantSet(
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
                    DnfNotVariableOf("B"),
                    DnfNotVariableOf("C"),
                    DnfNotVariableOf("D")
                ),
                DnfAndOf(
                    DnfVariableOf("A"),
                    DnfNotVariableOf("B"),
                    DnfVariableOf("C"),
                    DnfNotVariableOf("D")
                ),
                DnfAndOf(
                    DnfVariableOf("A"),
                    DnfVariableOf("B"),
                    DnfNotVariableOf("C"),
                    DnfNotVariableOf("D")
                ),
                DnfAndOf(
                    DnfVariableOf("A"),
                    DnfNotVariableOf("B"),
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
                    DnfNotVariableOf("B"),
                    DnfNotVariableOf("C"),
                    DnfNotVariableOf("D")
                ),
                DnfAndOf(
                    DnfVariableOf("A"),
                    DnfNotVariableOf("B"),
                    DnfVariableOf("C"),
                    DnfNotVariableOf("D")
                ),
                DnfAndOf(
                    DnfVariableOf("A"),
                    DnfVariableOf("B"),
                    DnfNotVariableOf("C"),
                    DnfNotVariableOf("D")
                ),
                DnfAndOf(
                    DnfVariableOf("A"),
                    DnfNotVariableOf("B"),
                    DnfVariableOf("C"),
                    DnfVariableOf("D")
                ),
            };

            var actualMinimalImplicantSet = BoolExpressions.QuineMcCluskeyMethod.PetrickMethod.Helper.GetMinimalImplicantSet(
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
                    DnfNotVariableOf("A"),
                    DnfVariableOf("B"),
                    DnfNotVariableOf("C"),
                    DnfNotVariableOf("D")
                ),
                DnfAndOf(
                    DnfVariableOf("A"),
                    DnfNotVariableOf("B"),
                    DnfNotVariableOf("C"),
                    DnfNotVariableOf("D")
                ),
                DnfAndOf(
                    DnfVariableOf("A"),
                    DnfNotVariableOf("B"),
                    DnfNotVariableOf("C"),
                    DnfVariableOf("D")
                ),
                DnfAndOf(
                    DnfVariableOf("A"),
                    DnfNotVariableOf("B"),
                    DnfVariableOf("C"),
                    DnfNotVariableOf("D")
                ),
                DnfAndOf(
                    DnfVariableOf("A"),
                    DnfVariableOf("B"),
                    DnfNotVariableOf("C"),
                    DnfNotVariableOf("D")
                ),
                DnfAndOf(
                    DnfVariableOf("A"),
                    DnfNotVariableOf("B"),
                    DnfVariableOf("C"),
                    DnfVariableOf("D")
                ),
                DnfAndOf(
                    DnfVariableOf("A"),
                    DnfVariableOf("B"),
                    DnfVariableOf("C"),
                    DnfNotVariableOf("D")
                ),
                DnfAndOf(
                    DnfVariableOf("A"),
                    DnfVariableOf("B"),
                    DnfVariableOf("C"),
                    DnfVariableOf("D")
                )
            });

            var actualProcessedDnfExpression = BoolExpressions.QuineMcCluskeyMethod.Helper.ProcessDnf(
                dnfExpression: dnfExpression
            );

            var expectedProcessedDnfExpression1 = new DnfExpression<string>(new HashSet<DnfAnd<string>> {
                DnfAndOf(
                    DnfVariableOf("B"),
                    DnfNotVariableOf("C"),
                    DnfNotVariableOf("D")
                ),
                DnfAndOf(
                    DnfVariableOf("A"),
                    DnfNotVariableOf("B")
                ),
                DnfAndOf(
                    DnfVariableOf("A"),
                    DnfVariableOf("C")
                ),
                DnfAndOf(
                    DnfVariableOf("A"),
                    DnfNotVariableOf("B"),
                    DnfVariableOf("C"),
                    DnfVariableOf("D")
                )
            });

            var expectedProcessedDnfExpression2 = new DnfExpression<string>(new HashSet<DnfAnd<string>> {
                DnfAndOf(
                    DnfVariableOf("B"),
                    DnfNotVariableOf("C"),
                    DnfNotVariableOf("D")
                ),
                DnfAndOf(
                    DnfVariableOf("A"),
                    DnfNotVariableOf("B")
                ),
                DnfAndOf(
                    DnfVariableOf("A"),
                    DnfVariableOf("C")
                ),
                DnfAndOf(
                    DnfVariableOf("A"),
                    DnfNotVariableOf("B"),
                    DnfNotVariableOf("C"),
                    DnfNotVariableOf("D")
                )
            });

            Assert.True(
                actualProcessedDnfExpression.Equals(expectedProcessedDnfExpression1) ||
                actualProcessedDnfExpression.Equals(expectedProcessedDnfExpression2));
        }
    }
}
