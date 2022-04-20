#region Copyright (C) Rijkswaterstaat 2022. All rights reserved.

// Copyright (C) Rijkswaterstaat 2022. All rights reserved.
//
// This file is part of the Assembly kernel.
//
// Assembly kernel is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>.
//
// All names, logos, and references to "Rijkswaterstaat" are registered trademarks of
// Rijkswaterstaat and remain full property of Rijkswaterstaat at all times.
// All rights reserved.

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Implementations;
using Assembly.Kernel.Interfaces;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.Categories;
using Assembly.Kernel.Model.FailureMechanismSections;
using NUnit.Framework;

namespace Assembly.Kernel.Tests.Implementations
{
    [TestFixture]
    public class FailureMechanismResultAssemblerBoi1A2Tests
    {
        private readonly double lengthEffectFactor1 = 14.4;
        private readonly double lengthEffectFactor2 = 10;
        private IFailureMechanismResultAssembler assembler;

        [SetUp]
        public void Init()
        {
            assembler = new FailureMechanismResultAssembler();
        }

        [Test, TestCaseSource(typeof(AssembleFailureMechanismTestData), nameof(AssembleFailureMechanismTestData.Boi1A2))]
        public void Boi1A2FailureProbabilityTests(Tuple<Probability, Probability, EInterpretationCategory>[] failureProbabilities, bool partialAssembly, Probability expectedProbability, EFailureMechanismAssemblyMethod expectedMethod)
        {
            var result = assembler.CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2(lengthEffectFactor1,
                failureProbabilities.Select(sectionResultTuple =>
                    new
                        FailureMechanismSectionAssemblyResultWithLengthEffect(
                            sectionResultTuple.Item1, sectionResultTuple.Item2, sectionResultTuple.Item3)),
                partialAssembly);

            AssertResultAsExpected(result, expectedMethod, expectedProbability);
        }

        [Test]
        public void Boi1A2LengthEffectFactor()
        {
            var result = assembler.CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2(5,
                new[]
                {
                    new FailureMechanismSectionAssemblyResultWithLengthEffect((Probability) 0.001, (Probability) 0.001,
                        EInterpretationCategory.II),
                    new FailureMechanismSectionAssemblyResultWithLengthEffect((Probability) 0.001, (Probability) 0.001,
                        EInterpretationCategory.II),
                    new FailureMechanismSectionAssemblyResultWithLengthEffect((Probability) 0.001, (Probability) 0.001,
                        EInterpretationCategory.IIIMin),
                    new FailureMechanismSectionAssemblyResultWithLengthEffect((Probability) 0.001, (Probability) 0.001,
                        EInterpretationCategory.IMin),
                    new FailureMechanismSectionAssemblyResultWithLengthEffect((Probability) 0.001, (Probability) 0.001,
                        EInterpretationCategory.IIMin),
                    new FailureMechanismSectionAssemblyResultWithLengthEffect((Probability) 0.001, (Probability) 0.001,
                        EInterpretationCategory.IIIMin)
                },
                false);

            AssertResultAsExpected(result, EFailureMechanismAssemblyMethod.Correlated, 0.005);
        }

        [Test]
        public void Boi1A2AllNotApplicable()
        {
            var result = assembler.CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2(lengthEffectFactor2,
                new[]
                {
                    new FailureMechanismSectionAssemblyResultWithLengthEffect((Probability) 0.0, (Probability) 0.0,
                        EInterpretationCategory.III),
                    new FailureMechanismSectionAssemblyResultWithLengthEffect((Probability) 0.0, (Probability) 0.0,
                        EInterpretationCategory.III),
                    new FailureMechanismSectionAssemblyResultWithLengthEffect((Probability) 0.0, (Probability) 0.0,
                        EInterpretationCategory.III)
                },
                false);

            AssertResultAsExpected(result, EFailureMechanismAssemblyMethod.Correlated, 0.0); 
        }

        [Test]
        public void Boi1A2Partial()
        {
            var result = assembler.CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2(lengthEffectFactor2,
                new[]
                {
                    new FailureMechanismSectionAssemblyResultWithLengthEffect(Probability.Undefined, Probability.Undefined,
                        EInterpretationCategory.Dominant),
                    new FailureMechanismSectionAssemblyResultWithLengthEffect(Probability.Undefined, Probability.Undefined,
                        EInterpretationCategory.Dominant),
                    new FailureMechanismSectionAssemblyResultWithLengthEffect((Probability) 0.000010, (Probability) 0.000010,
                        EInterpretationCategory.II),
                    new FailureMechanismSectionAssemblyResultWithLengthEffect((Probability) 0.0000011, (Probability) 0.0000011,
                        EInterpretationCategory.II),
                    new FailureMechanismSectionAssemblyResultWithLengthEffect((Probability) 0.9, (Probability) 0.9,
                        EInterpretationCategory.II),
                    new FailureMechanismSectionAssemblyResultWithLengthEffect((Probability) 0.000009, (Probability) 0.000009,
                        EInterpretationCategory.III),
                    new FailureMechanismSectionAssemblyResultWithLengthEffect(Probability.Undefined, Probability.Undefined, EInterpretationCategory.NotDominant),
                    new FailureMechanismSectionAssemblyResultWithLengthEffect((Probability) 0.0, (Probability) 0.0, EInterpretationCategory.IIIMin)
                },
                true);

            AssertResultAsExpected(result, EFailureMechanismAssemblyMethod.Uncorrelated, 0.90000201);
        }

        [Test]
        public void Boi1A2DominantPartial()
        {
            var result = assembler.CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2(lengthEffectFactor2,
                new[]
                {
                    new FailureMechanismSectionAssemblyResultWithLengthEffect(Probability.Undefined, Probability.Undefined,
                        EInterpretationCategory.Dominant),
                    new FailureMechanismSectionAssemblyResultWithLengthEffect((Probability)0.01, (Probability)0.1,
                        EInterpretationCategory.IMin),
                    new FailureMechanismSectionAssemblyResultWithLengthEffect(Probability.Undefined, Probability.Undefined,
                        EInterpretationCategory.Dominant)
                },
                true);

            AssertResultAsExpected(result, EFailureMechanismAssemblyMethod.Uncorrelated, 0.1);
        }

        [Test]
        public void Boi1A2AllDominantPartial()
        {
            var result = assembler.CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2(lengthEffectFactor2,
                new[]
                {
                    new FailureMechanismSectionAssemblyResultWithLengthEffect(Probability.Undefined, Probability.Undefined, EInterpretationCategory.Dominant),
                    new FailureMechanismSectionAssemblyResultWithLengthEffect(Probability.Undefined, Probability.Undefined, EInterpretationCategory.Dominant),
                    new FailureMechanismSectionAssemblyResultWithLengthEffect(Probability.Undefined, Probability.Undefined, EInterpretationCategory.Dominant)
                },
                true);
            AssertResultAsExpected(result, EFailureMechanismAssemblyMethod.Correlated, 0.0);
        }

        [Test]
        public void Boi1A2DominantAndNotDominantPartial()
        {
            var result = assembler.CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2(lengthEffectFactor2,
                new[]
                {
                    new FailureMechanismSectionAssemblyResultWithLengthEffect(Probability.Undefined, Probability.Undefined, EInterpretationCategory.Dominant),
                    new FailureMechanismSectionAssemblyResultWithLengthEffect((Probability)0, (Probability)0, EInterpretationCategory.NotDominant),
                    new FailureMechanismSectionAssemblyResultWithLengthEffect((Probability)0, (Probability)0, EInterpretationCategory.NotDominant)
                },
                true);
            AssertResultAsExpected(result, EFailureMechanismAssemblyMethod.Correlated, 0.0);
        }

        [Test]
        public void Boi1A2DominantAndSectionWithoutResultPartial()
        {
            var result = assembler.CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2(lengthEffectFactor2,
                new[]
                {
                    new FailureMechanismSectionAssemblyResultWithLengthEffect(Probability.Undefined, Probability.Undefined,
                        EInterpretationCategory.Dominant),
                    new FailureMechanismSectionAssemblyResultWithLengthEffect(Probability.Undefined, Probability.Undefined,
                        EInterpretationCategory.NoResult),
                    new FailureMechanismSectionAssemblyResultWithLengthEffect((Probability) 0.00026, (Probability) 0.00026,
                        EInterpretationCategory.II)
                },
                true);

            AssertResultAsExpected(result, EFailureMechanismAssemblyMethod.Uncorrelated, 0.00026);
        }

        [Test]
        public void Boi1A2OneSectionNeverCorrelated()
        {
            var result = assembler.CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2(lengthEffectFactor2,
                new[]
                {
                    new FailureMechanismSectionAssemblyResultWithLengthEffect((Probability) 0.0, (Probability) 0.0,
                        EInterpretationCategory.III)
                },
                true);

            AssertResultAsExpected(result, EFailureMechanismAssemblyMethod.Uncorrelated, 0.0);
        }

        [Test]
        public void Boi1A2AllDominant()
        {
            TestHelper.AssertExpectedErrorMessage(() =>
            {
                var result = assembler.CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2(lengthEffectFactor2,
                new[]
                {
                    new FailureMechanismSectionAssemblyResultWithLengthEffect(Probability.Undefined, Probability.Undefined, EInterpretationCategory.Dominant),
                    new FailureMechanismSectionAssemblyResultWithLengthEffect(Probability.Undefined, Probability.Undefined, EInterpretationCategory.Dominant),
                    new FailureMechanismSectionAssemblyResultWithLengthEffect(Probability.Undefined, Probability.Undefined, EInterpretationCategory.Dominant)
                },
                false);
            }, EAssemblyErrors.EncounteredOneOrMoreSectionsWithoutResult);
        }

        [Test]
        public void Boi1A2SectionWithoutResult()
        {
            TestHelper.AssertExpectedErrorMessage(() =>
            {
                var result = assembler.CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2(lengthEffectFactor2,
                    new[]
                    {
                        new FailureMechanismSectionAssemblyResultWithLengthEffect((Probability) 0.00026, (Probability) 0.00026,
                            EInterpretationCategory.II),
                        new FailureMechanismSectionAssemblyResultWithLengthEffect(Probability.Undefined, Probability.Undefined,
                            EInterpretationCategory.
                                NoResult),
                        new FailureMechanismSectionAssemblyResultWithLengthEffect((Probability) 0.00026, (Probability) 0.00026,
                            EInterpretationCategory.II)
                    },
                    false);
            }, EAssemblyErrors.EncounteredOneOrMoreSectionsWithoutResult);
        }

        [Test]
        public void Boi1A2DominantAndSectionWithoutResult()
        {
            TestHelper.AssertExpectedErrorMessage(() =>
            {
                var result = assembler.CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2(lengthEffectFactor2,
                    new[]
                    {
                        new FailureMechanismSectionAssemblyResultWithLengthEffect(Probability.Undefined, Probability.Undefined,
                            EInterpretationCategory.Dominant),
                        new FailureMechanismSectionAssemblyResultWithLengthEffect(Probability.Undefined, Probability.Undefined,
                            EInterpretationCategory.NoResult),
                        new FailureMechanismSectionAssemblyResultWithLengthEffect((Probability) 0.00026, (Probability) 0.00026,
                            EInterpretationCategory.II)
                    },
                    false);
            }, EAssemblyErrors.EncounteredOneOrMoreSectionsWithoutResult);
        }

        [Test]
        public void Boi1A2EmptyResults()
        {
            TestHelper.AssertExpectedErrorMessage(() =>
            {
                var result = assembler.CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2(lengthEffectFactor2,
                    new FailureMechanismSectionAssemblyResultWithLengthEffect[]{}, 
                    false);
            }, EAssemblyErrors.EmptyResultsList);
        }

        [Test]
        public void Boi1A2NullResults()
        {
            TestHelper.AssertExpectedErrorMessage(() =>
            {
                var result = assembler.CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2(lengthEffectFactor2,
                    null,
                    false);
            }, EAssemblyErrors.ValueMayNotBeNull);
        }

        [Test]
        public void Boi1A2MultipleErrorsTest()
        {
            TestHelper.AssertExpectedErrorMessage(() =>
            {
                var result = assembler.CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2(0.1,
                    new ResultWithProfileAndSectionProbabilities[] { }, 
                    false);
            }, EAssemblyErrors.EmptyResultsList, EAssemblyErrors.LengthEffectFactorOutOfRange );
        }

        [Test,
         TestCaseSource(typeof(LengthEffectTestData), nameof(LengthEffectTestData.TestCases))]
        public List<EAssemblyErrors> LengthEffectCheckTest(double lengthEffectFactor)
        {
            try
            {
                assembler.CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2(lengthEffectFactor,
                    new[] { new FailureMechanismSectionAssemblyResultWithLengthEffect((Probability)0.0001, (Probability) 0.001, EInterpretationCategory.II) },
                    false);
            }
            catch (AssemblyException e)
            {
                Assert.NotNull(e.Errors);
                return e.Errors.Select(message => message.ErrorCode).ToList();
            }

            return null;
        }

        private static void AssertResultAsExpected(FailureMechanismAssemblyResult result, EFailureMechanismAssemblyMethod expectedAssemblyMethod, double expectedProbabilityValue)
        {
            Assert.NotNull(result);
            Assert.AreEqual(expectedAssemblyMethod, result.AssemblyMethod);
            Assert.IsTrue(result.Probability.IsNegligibleDifference((Probability)expectedProbabilityValue));
        }

        private class LengthEffectTestData
        {
            public static IEnumerable TestCases
            {
                get
                {
                    yield return new TestCaseData(1).Returns(null);
                    yield return new TestCaseData(10).Returns(null);
                    yield return new TestCaseData(0.9).Returns(
                        new List<EAssemblyErrors>
                        {
                            EAssemblyErrors.LengthEffectFactorOutOfRange
                        });
                    yield return new TestCaseData(0).Returns(
                        new List<EAssemblyErrors>
                        {
                            EAssemblyErrors.LengthEffectFactorOutOfRange
                        });
                    yield return new TestCaseData(-2).Returns(
                        new List<EAssemblyErrors>
                        {
                            EAssemblyErrors.LengthEffectFactorOutOfRange
                        });
                }
            }
        }

        private class AssembleFailureMechanismTestData
        {
            public static IEnumerable Boi1A2
            {
                get
                {
                    yield return new TestCaseData(
                        new[]
                        {
                            new Tuple<Probability, Probability, EInterpretationCategory>((Probability) 0.0, (Probability) 0.0, EInterpretationCategory.III),
                            new Tuple<Probability, Probability, EInterpretationCategory>((Probability) 0.1, (Probability) 0.1, EInterpretationCategory.III)
                        },
                        false,
                        (Probability) 0.1, EFailureMechanismAssemblyMethod.Uncorrelated);

                    yield return new TestCaseData(
                        new[]
                        {
                            new Tuple<Probability, Probability, EInterpretationCategory>((Probability) 0.0, (Probability) 0.0, EInterpretationCategory.III),
                            new Tuple<Probability, Probability, EInterpretationCategory>((Probability) 0.0001, (Probability) 0.001, EInterpretationCategory.III),
                            new Tuple<Probability, Probability, EInterpretationCategory>((Probability) 0.0005, (Probability) 0.005, EInterpretationCategory.III),
                            new Tuple<Probability, Probability, EInterpretationCategory>((Probability) 0.0007, (Probability) 0.0008, EInterpretationCategory.III)
                        },
                        false,
                        (Probability) 0.006790204, EFailureMechanismAssemblyMethod.Uncorrelated);

                    yield return new TestCaseData(
                        new[]
                        {
                            new Tuple<Probability, Probability, EInterpretationCategory>((Probability) 0.0005, (Probability) 0.0005, EInterpretationCategory.III),
                            new Tuple<Probability, Probability, EInterpretationCategory>((Probability) 0.00005, (Probability) 0.00005, EInterpretationCategory.III)
                        },
                        false,
                        (Probability) 0.000549975, EFailureMechanismAssemblyMethod.Uncorrelated);

                    yield return new TestCaseData(
                        new[]
                        {
                            new Tuple<Probability, Probability, EInterpretationCategory>((Probability) 1.0/1234.0, (Probability) 1.0/23.0, EInterpretationCategory.IIIMin),
                            new Tuple<Probability, Probability, EInterpretationCategory>((Probability) 1.0/1820.0, (Probability) 1.0/781.0, EInterpretationCategory.IIIMin)
                        },
                        false,
                        (Probability)0.0116693679, EFailureMechanismAssemblyMethod.Correlated);

                    yield return new TestCaseData(
                        new[]
                        {
                            new Tuple<Probability, Probability, EInterpretationCategory>((Probability) 0.0, (Probability) 0.0, EInterpretationCategory.III),
                            new Tuple<Probability, Probability, EInterpretationCategory>((Probability) 0.0, (Probability) 0.0, EInterpretationCategory.III),
                        },
                        false,
                        (Probability)0.0, EFailureMechanismAssemblyMethod.Correlated);
                }
            }
        }
    }
}