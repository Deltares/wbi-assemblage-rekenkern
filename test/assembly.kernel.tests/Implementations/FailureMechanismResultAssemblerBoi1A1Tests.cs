#region Copyright (C) Rijkswaterstaat 2022. All rights reserved

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

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Implementations;
using Assembly.Kernel.Interfaces;
using Assembly.Kernel.Model;
using NUnit.Framework;

namespace Assembly.Kernel.Tests.Implementations
{
    [TestFixture]
    public class FailureMechanismResultAssemblerBoi1A1Tests
    {
        private readonly double lengthEffectFactor2 = 10;
        private IFailureMechanismResultAssembler assembler;

        [SetUp]
        public void Init()
        {
            assembler = new FailureMechanismResultAssembler();
        }

        #region Functional tests

        [TestCaseSource(typeof(AssembleFailureMechanismTestData), nameof(AssembleFailureMechanismTestData.Boi1A1))]
        public void Boi1A1FailureProbabilityTests(double lengthEffectParameter, Probability[] failureProbabilities, bool partialAssembly, Probability expectedResult, EFailureMechanismAssemblyMethod expectedMethod)
        {
            var result = assembler.CalculateFailureMechanismFailureProbabilityBoi1A1(lengthEffectParameter,
                failureProbabilities,
                partialAssembly);

            Assert.NotNull(result);
            Assert.AreEqual(expectedMethod, result.AssemblyMethod);
            Assert.AreEqual(expectedResult, result.Probability, 1e-10);
        }

        [Test]
        public void Boi1A1LengthEffectFactor()
        {
            var result = assembler.CalculateFailureMechanismFailureProbabilityBoi1A1(5,
                new[]
                {
                    (Probability) 0.001,
                    (Probability) 0.001,
                    (Probability) 0.001,
                    (Probability) 0.001,
                    (Probability) 0.001,
                    (Probability) 0.001
                },
                false);

            Assert.NotNull(result);
            Assert.AreEqual(EFailureMechanismAssemblyMethod.Correlated, result.AssemblyMethod);
            Assert.AreEqual(0.005, result.Probability, 1e-4);
        }

        [Test]
        public void Boi1A1AllNotApplicable()
        {
            var result = assembler.CalculateFailureMechanismFailureProbabilityBoi1A1(lengthEffectFactor2,
                new[]
                {
                    (Probability) 0.0,
                    (Probability) 0.0,
                    (Probability) 0.0
                },
                false);

            Assert.IsNotNull(result);
            Assert.AreEqual(EFailureMechanismAssemblyMethod.Correlated, result.AssemblyMethod);
            Assert.AreEqual(0.0, result.Probability);
        }

        [Test]
        public void Boi1A1Partial()
        {
            var result = assembler.CalculateFailureMechanismFailureProbabilityBoi1A1(lengthEffectFactor2,
                new[]
                {
                    Probability.Undefined,
                    Probability.Undefined,
                    (Probability) 0.000010,
                    (Probability) 0.0000011,
                    (Probability) 0.9,
                    (Probability) 0.000009,
                    Probability.Undefined,
                    (Probability) 0.0
                },
                true);

            Assert.NotNull(result);
            Assert.AreEqual(EFailureMechanismAssemblyMethod.Uncorrelated, result.AssemblyMethod);
            Assert.AreEqual(0.9, result.Probability, 1e-4);
        }

        [Test]
        public void Boi1A1DominantPartial()
        {
            var result = assembler.CalculateFailureMechanismFailureProbabilityBoi1A1(lengthEffectFactor2,
                new[]
                {
                    Probability.Undefined,
                    (Probability)0.1,
                    Probability.Undefined
                },
                true);

            Assert.IsNotNull(result);
            Assert.AreEqual(EFailureMechanismAssemblyMethod.Uncorrelated, result.AssemblyMethod);
            Assert.AreEqual(0.1, result.Probability, 1E-10);
        }

        [Test]
        public void Boi1A1AllDominantPartial()
        {
            var result = assembler.CalculateFailureMechanismFailureProbabilityBoi1A1(lengthEffectFactor2,
                new[]
                {
                    Probability.Undefined,
                    Probability.Undefined,
                    Probability.Undefined
                },
                true);
            Assert.IsNotNull(result);
            Assert.AreEqual(EFailureMechanismAssemblyMethod.Correlated, result.AssemblyMethod);
            Assert.AreEqual(0.0, result.Probability);
        }

        [Test]
        public void Boi1A1DominantAndNotDominantPartial()
        {
            var result = assembler.CalculateFailureMechanismFailureProbabilityBoi1A1(lengthEffectFactor2,
                new[]
                {
                    Probability.Undefined,
                    Probability.Undefined,
                    Probability.Undefined
                },
                true);
            Assert.IsNotNull(result);
            Assert.AreEqual(EFailureMechanismAssemblyMethod.Correlated, result.AssemblyMethod);
            Assert.AreEqual(0.0, result.Probability);
        }

        [Test]
        public void Boi1A1DominantAndSectionWithoutResultPartial()
        {
            var result = assembler.CalculateFailureMechanismFailureProbabilityBoi1A1(lengthEffectFactor2,
                new[]
                {
                    Probability.Undefined,
                    Probability.Undefined,
                    (Probability) 0.00026
                },
                true);

            Assert.IsNotNull(result);
            Assert.AreEqual(EFailureMechanismAssemblyMethod.Uncorrelated, result.AssemblyMethod);
            Assert.AreEqual(0.00026, result.Probability,1e-8);
        }

        [Test]
        public void Boi1A1OneSectionNeverCorrelated()
        {
            var result = assembler.CalculateFailureMechanismFailureProbabilityBoi1A1(lengthEffectFactor2,
                new[]
                {
                    (Probability) 0.0
                },
                true);

            Assert.IsNotNull(result);
            Assert.AreEqual(EFailureMechanismAssemblyMethod.Uncorrelated, result.AssemblyMethod);
            Assert.AreEqual(0.0, result.Probability, 1e-8);
        }

        #endregion

        #region Error handling

        [Test]
        public void Boi1A1AllDominant()
        {
            try
            {
                var result = assembler.CalculateFailureMechanismFailureProbabilityBoi1A1(lengthEffectFactor2,
                new[]
                {
                    Probability.Undefined,Probability.Undefined,Probability.Undefined
                },
                false);
            }
            catch (AssemblyException exception)
            {
                Assert.IsNotNull(exception.Errors);
                Assert.AreEqual(1, exception.Errors.Count());
                Assert.AreEqual(EAssemblyErrors.EncounteredOneOrMoreSectionsWithoutResult, exception.Errors.First().ErrorCode);
                Assert.Pass();
            }
        }

        [Test]
        public void Boi1A1SectionWithoutResult()
        {
            try
            {
                var result = assembler.CalculateFailureMechanismFailureProbabilityBoi1A1(lengthEffectFactor2,
                    new[]
                    {
                        (Probability) 0.00026,
                        Probability.Undefined,
                        (Probability) 0.00026
                    },
                    false);
            }
            catch (AssemblyException exception)
            {
                Assert.IsNotNull(exception.Errors);
                Assert.AreEqual(1, exception.Errors.Count());
                Assert.AreEqual(EAssemblyErrors.EncounteredOneOrMoreSectionsWithoutResult, exception.Errors.First().ErrorCode);
                Assert.Pass();
            }
        }

        [Test]
        public void Boi1A1DominantAndSectionWithoutResult()
        {
            try
            {
                var result = assembler.CalculateFailureMechanismFailureProbabilityBoi1A1(lengthEffectFactor2,
                    new[]
                    {
                        Probability.Undefined,
                        Probability.Undefined,
                        (Probability) 0.00026
                    },
                    false);
            }
            catch (AssemblyException exception)
            {
                Assert.IsNotNull(exception.Errors);
                Assert.AreEqual(1, exception.Errors.Count());
                Assert.AreEqual(EAssemblyErrors.EncounteredOneOrMoreSectionsWithoutResult, exception.Errors.First().ErrorCode);
                Assert.Pass();
            }
        }

        #endregion

        #region Input handling

        [Test]
        public void Boi1A1EmptyResults()
        {
            try
            {
                var result = assembler.CalculateFailureMechanismFailureProbabilityBoi1A1(lengthEffectFactor2,
                    new Probability[]{}, 
                    false);
            }
            catch (AssemblyException exception)
            {
                Assert.IsNotNull(exception.Errors);
                Assert.AreEqual(1, exception.Errors.Count());
                Assert.AreEqual(EAssemblyErrors.EmptyResultsList, exception.Errors.First().ErrorCode);
                Assert.Pass();
            }

            Assert.Fail("Expected exception did not occur");
        }

        [Test]
        public void Boi1A1NullResults()
        {
            try
            {
                var result = assembler.CalculateFailureMechanismFailureProbabilityBoi1A1(lengthEffectFactor2,
                    null,
                    false);
            }
            catch (AssemblyException exception)
            {
                Assert.IsNotNull(exception.Errors);
                Assert.AreEqual(1, exception.Errors.Count());
                Assert.AreEqual(EAssemblyErrors.ValueMayNotBeNull, exception.Errors.First().ErrorCode);
                Assert.Pass();
            }

            Assert.Fail("Expected exception did not occur");
        }

        [TestCaseSource(typeof(LengthEffectTestData), nameof(LengthEffectTestData.TestCases))]
        public List<EAssemblyErrors> LengthEffectCheckTest(double lengthEffectFactor)
        {
            try
            {
                assembler.CalculateFailureMechanismFailureProbabilityBoi1A1(lengthEffectFactor,
                    new[] { (Probability) 0.001 },
                    false);
            }
            catch (AssemblyException e)
            {
                Assert.NotNull(e.Errors);
                return e.Errors.Select(message => message.ErrorCode).ToList();
            }

            return null;
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

        #endregion

        private class AssembleFailureMechanismTestData
        {
            public static IEnumerable Boi1A1
            {
                get
                {
                    yield return new TestCaseData(
                        14.4,
                        new[]
                        {
                            (Probability) 0.0,
                            (Probability) 0.1
                        },
                        false,
                        (Probability) 0.1, EFailureMechanismAssemblyMethod.Uncorrelated);

                    yield return new TestCaseData(
                        14.4,
                        new[]
                        {
                            (Probability) 0.0,
                            (Probability) 0.0001, 
                            (Probability) 0.0005, 
                            (Probability) 0.0007
                        },
                        false,
                        (Probability)0.00129953, EFailureMechanismAssemblyMethod.Uncorrelated);

                    yield return new TestCaseData(
                        14.4,
                        new[]
                        {
                            (Probability) 0.0005, 
                            (Probability) 0.00005
                        },
                        false,
                        (Probability) 0.000549975, EFailureMechanismAssemblyMethod.Uncorrelated);

                    yield return new TestCaseData(
                        14.4,
                        new[]
                        {
                            (Probability) 1.0/23.0,
                            (Probability)0,
                            (Probability) 1.0/781.0
                        },
                        false,
                        (Probability)0.0447030006, EFailureMechanismAssemblyMethod.Uncorrelated);

                    yield return new TestCaseData(
                        2,
                        new[]
                        {
                            (Probability) 1.0/230.0,
                            (Probability) 1.0/264.0,
                            (Probability) 1.0/781.0
                        },
                        false,
                        (Probability)0.00869565217, EFailureMechanismAssemblyMethod.Correlated);

                    yield return new TestCaseData(
                        14.4,
                        new[]
                        {
                            (Probability)0.0, 
                            (Probability) 0.0,
                            (Probability) 0.0
                        },
                        false,
                        (Probability)0.0, EFailureMechanismAssemblyMethod.Correlated);
                }
            }
        }
    }
}