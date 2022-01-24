#region Copyright (C) Rijkswaterstaat 2019. All rights reserved
// Copyright (C) Rijkswaterstaat 2019. All rights reserved.
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
using System.CodeDom;
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
    public class FailureMechanismResultAssemblerTests
    {
        private readonly double lengthEffectFactor1 = 14.4;
        private readonly double lengthEffectFactor2 = 10;
        private IFailureMechanismResultAssembler assembler;

        [SetUp]
        public void Init()
        {
            assembler = new FailureMechanismResultAssembler();
        }

        #region Functional tests

        [Test, TestCaseSource(typeof(AssembleFailureMechanismTestData), nameof(AssembleFailureMechanismTestData.Wbi1B1))]
        public void Wbi1B1FailureProbabilityTests(Tuple<Probability, Probability, EInterpretationCategory>[] failureProbabilities, bool partialAssembly, Probability expectedResult, EFailureMechanismAssemblyMethod expectedMethod)
        {
            // Use correct probabilities
            var result = assembler.AssembleFailureMechanismWbi1B1(lengthEffectFactor1,
                failureProbabilities.Select(sectionResultTuple =>
                    new
                        FailureMechanismSectionAssemblyResult(
                            sectionResultTuple.Item1, sectionResultTuple.Item2, sectionResultTuple.Item3)),
                partialAssembly);

            Assert.NotNull(result);
            Assert.AreEqual(expectedMethod, result.AssemblyMethod);
            Assert.AreEqual(expectedResult, result.Probability, 1e-10);
        }

        [Test]
        public void Wbi1B1LengthEffectFactor()
        {
            var result = assembler.AssembleFailureMechanismWbi1B1(5,
                new[]
                {
                    new FailureMechanismSectionAssemblyResult((Probability) 0.001, (Probability) 0.001,
                        EInterpretationCategory.II),
                    new FailureMechanismSectionAssemblyResult((Probability) 0.001, (Probability) 0.001,
                        EInterpretationCategory.II),
                    new FailureMechanismSectionAssemblyResult((Probability) 0.001, (Probability) 0.001,
                        EInterpretationCategory.IIIMin),
                    new FailureMechanismSectionAssemblyResult((Probability) 0.001, (Probability) 0.001,
                        EInterpretationCategory.IMin),
                    new FailureMechanismSectionAssemblyResult((Probability) 0.001, (Probability) 0.001,
                        EInterpretationCategory.IIMin),
                    new FailureMechanismSectionAssemblyResult((Probability) 0.001, (Probability) 0.001,
                        EInterpretationCategory.IIIMin)
                },
                false);

            Assert.NotNull(result);
            Assert.AreEqual(EFailureMechanismAssemblyMethod.Correlated, result.AssemblyMethod);
            Assert.AreEqual(0.005, result.Probability, 1e-4);
        }

        [Test]
        public void Wbi1B1AllNotApplicable()
        {
            var result = assembler.AssembleFailureMechanismWbi1B1(lengthEffectFactor2,
                new[]
                {
                    new FailureMechanismSectionAssemblyResult((Probability) 0.0, (Probability) 0.0,
                        EInterpretationCategory.III),
                    new FailureMechanismSectionAssemblyResult((Probability) 0.0, (Probability) 0.0,
                        EInterpretationCategory.III),
                    new FailureMechanismSectionAssemblyResult((Probability) 0.0, (Probability) 0.0,
                        EInterpretationCategory.III)
                },
                false);

            Assert.IsNotNull(result);
            Assert.AreEqual(EFailureMechanismAssemblyMethod.Correlated, result.AssemblyMethod);
            Assert.AreEqual(0.0, result.Probability);
        }

        [Test]
        public void Wbi1B1Partial()
        {
            var result = assembler.AssembleFailureMechanismWbi1B1(lengthEffectFactor2,
                new[]
                {
                    new FailureMechanismSectionAssemblyResult(Probability.NaN, Probability.NaN,
                        EInterpretationCategory.Dominant),
                    new FailureMechanismSectionAssemblyResult(Probability.NaN, Probability.NaN,
                        EInterpretationCategory.Dominant),
                    new FailureMechanismSectionAssemblyResult((Probability) 0.000010, (Probability) 0.000010,
                        EInterpretationCategory.II),
                    new FailureMechanismSectionAssemblyResult((Probability) 0.0000011, (Probability) 0.0000011,
                        EInterpretationCategory.II),
                    new FailureMechanismSectionAssemblyResult((Probability) 0.9, (Probability) 0.9,
                        EInterpretationCategory.II),
                    new FailureMechanismSectionAssemblyResult((Probability) 0.000009, (Probability) 0.000009,
                        EInterpretationCategory.III),
                    new FailureMechanismSectionAssemblyResult(Probability.NaN, Probability.NaN, EInterpretationCategory.NotDominant),
                    new FailureMechanismSectionAssemblyResult((Probability) 0.0, (Probability) 0.0, EInterpretationCategory.IIIMin)
                },
                true);

            Assert.NotNull(result);
            Assert.AreEqual(EFailureMechanismAssemblyMethod.UnCorrelated, result.AssemblyMethod);
            Assert.AreEqual(0.9, result.Probability, 1e-4);
        }

        [Test]
        public void Wbi1B1DominantPartial()
        {
            var result = assembler.AssembleFailureMechanismWbi1B1(lengthEffectFactor2,
                new[]
                {
                    new FailureMechanismSectionAssemblyResult((Probability) 0.00026, (Probability) 0.00026,
                        EInterpretationCategory.Dominant),
                    new FailureMechanismSectionAssemblyResult((Probability)0.01, (Probability)0.1,
                        EInterpretationCategory.IMin),
                    new FailureMechanismSectionAssemblyResult((Probability) 0.00026, (Probability) 0.00026,
                        EInterpretationCategory.Dominant)
                },
                true);

            Assert.IsNotNull(result);
            Assert.AreEqual(EFailureMechanismAssemblyMethod.UnCorrelated, result.AssemblyMethod);
            Assert.AreEqual(0.1, result.Probability.Value, 1E-10);
        }

        [Test]
        public void Wbi1B1AllDominantPartial()
        {
            var result = assembler.AssembleFailureMechanismWbi1B1(lengthEffectFactor2,
                new[]
                {
                    new FailureMechanismSectionAssemblyResult(Probability.NaN, Probability.NaN, EInterpretationCategory.Dominant),
                    new FailureMechanismSectionAssemblyResult(Probability.NaN, Probability.NaN, EInterpretationCategory.Dominant),
                    new FailureMechanismSectionAssemblyResult(Probability.NaN, Probability.NaN, EInterpretationCategory.Dominant)
                },
                true);
            Assert.IsNotNull(result);
            Assert.AreEqual(EFailureMechanismAssemblyMethod.Correlated, result.AssemblyMethod);
            Assert.AreEqual(0.0, result.Probability);
        }

        [Test]
        public void Wbi1B1DominantAndNotDominantPartial()
        {
            var result = assembler.AssembleFailureMechanismWbi1B1(lengthEffectFactor2,
                new[]
                {
                    new FailureMechanismSectionAssemblyResult(Probability.NaN, Probability.NaN, EInterpretationCategory.Dominant),
                    new FailureMechanismSectionAssemblyResult(Probability.NaN, Probability.NaN, EInterpretationCategory.NotDominant),
                    new FailureMechanismSectionAssemblyResult(Probability.NaN, Probability.NaN, EInterpretationCategory.NotDominant)
                },
                true);
            Assert.IsNotNull(result);
            Assert.AreEqual(EFailureMechanismAssemblyMethod.Correlated, result.AssemblyMethod);
            Assert.AreEqual(0.0, result.Probability);
        }

        #endregion

        #region Error handling

        [Test]
        public void Wbi1B1AllDominant()
        {
            try
            {
                var result = assembler.AssembleFailureMechanismWbi1B1(lengthEffectFactor2,
                new[]
                {
                    new FailureMechanismSectionAssemblyResult(Probability.NaN, Probability.NaN, EInterpretationCategory.Dominant),
                    new FailureMechanismSectionAssemblyResult(Probability.NaN, Probability.NaN, EInterpretationCategory.Dominant),
                    new FailureMechanismSectionAssemblyResult(Probability.NaN, Probability.NaN, EInterpretationCategory.Dominant)
                },
                false);
            }
            catch (AssemblyException exception)
            {
                Assert.IsNotNull(exception.Errors);
                Assert.AreEqual(1, exception.Errors.Count());
                Assert.AreEqual(EAssemblyErrors.DominantSectionCannotBeAssembled, exception.Errors.First().ErrorCode);
                Assert.Pass();
            }
        }

        [Test]
        public void Wbi1B1SectionWithoutResult()
        {
            try
            {
                var result = assembler.AssembleFailureMechanismWbi1B1(lengthEffectFactor2,
                    new[]
                    {
                        new FailureMechanismSectionAssemblyResult((Probability) 0.00026, (Probability) 0.00026,
                            EInterpretationCategory.II),
                        new FailureMechanismSectionAssemblyResult(Probability.NaN, Probability.NaN,
                            EInterpretationCategory.IMin),
                        new FailureMechanismSectionAssemblyResult((Probability) 0.00026, (Probability) 0.00026,
                            EInterpretationCategory.II)
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
        public void Wbi1B1DominantAndSectionWithoutResult()
        {
            try
            {
                var result = assembler.AssembleFailureMechanismWbi1B1(lengthEffectFactor2,
                    new[]
                    {
                        new FailureMechanismSectionAssemblyResult((Probability) 0.00026, (Probability) 0.00026,
                            EInterpretationCategory.Dominant),
                        new FailureMechanismSectionAssemblyResult(Probability.NaN, Probability.NaN,
                            EInterpretationCategory.IMin),
                        new FailureMechanismSectionAssemblyResult((Probability) 0.00026, (Probability) 0.00026,
                            EInterpretationCategory.II)
                    },
                    false);
            }
            catch (AssemblyException exception)
            {
                Assert.IsNotNull(exception.Errors);
                Assert.AreEqual(2, exception.Errors.Count());
                Assert.AreEqual(EAssemblyErrors.EncounteredOneOrMoreSectionsWithoutResult, exception.Errors.First().ErrorCode);
                Assert.AreEqual(EAssemblyErrors.DominantSectionCannotBeAssembled, exception.Errors.ElementAt(1).ErrorCode);
                Assert.Pass();
            }
        }

        [Test]
        public void Wbi1B1DominantAndSectionWithoutResultPartial()
        {
            try
            {
                var result = assembler.AssembleFailureMechanismWbi1B1(lengthEffectFactor2,
                    new[]
                    {
                        new FailureMechanismSectionAssemblyResult((Probability) 0.00026, (Probability) 0.00026,
                            EInterpretationCategory.Dominant),
                        new FailureMechanismSectionAssemblyResult(Probability.NaN, Probability.NaN,
                            EInterpretationCategory.IMin),
                        new FailureMechanismSectionAssemblyResult((Probability) 0.00026, (Probability) 0.00026,
                            EInterpretationCategory.II)
                    },
                    true);
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
        public void Wbi1B1EmptyResults()
        {
            try
            {
                var result = assembler.AssembleFailureMechanismWbi1B1(lengthEffectFactor2,
                    new FailureMechanismSectionAssemblyResult[]{}, 
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
        public void Wbi1B1NullResults()
        {
            try
            {
                var result = assembler.AssembleFailureMechanismWbi1B1(lengthEffectFactor2,
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

        [Test,
         TestCaseSource(typeof(LengthEffectTestData), nameof(LengthEffectTestData.TestCases))]
        public List<EAssemblyErrors> LengthEffectCheckTest(double lengthEffectFactor)
        {
            try
            {
                assembler.AssembleFailureMechanismWbi1B1(lengthEffectFactor,
                    new[] { new FailureMechanismSectionAssemblyResult((Probability)0.0001, (Probability) 0.001, EInterpretationCategory.II) },
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
            public static IEnumerable Wbi1B1
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
                        (Probability) 0.1, EFailureMechanismAssemblyMethod.UnCorrelated);

                    yield return new TestCaseData(
                        new[]
                        {
                            new Tuple<Probability, Probability, EInterpretationCategory>((Probability) 0.0, (Probability) 0.0, EInterpretationCategory.III),
                            new Tuple<Probability, Probability, EInterpretationCategory>((Probability) 0.0001, (Probability) 0.001, EInterpretationCategory.III),
                            new Tuple<Probability, Probability, EInterpretationCategory>((Probability) 0.0005, (Probability) 0.005, EInterpretationCategory.III),
                            new Tuple<Probability, Probability, EInterpretationCategory>((Probability) 0.0007, (Probability) 0.0008, EInterpretationCategory.III)
                        },
                        false,
                        (Probability) 0.006790204, EFailureMechanismAssemblyMethod.UnCorrelated);

                    yield return new TestCaseData(
                        new[]
                        {
                            new Tuple<Probability, Probability, EInterpretationCategory>((Probability) 0.0005, (Probability) 0.0005, EInterpretationCategory.III),
                            new Tuple<Probability, Probability, EInterpretationCategory>((Probability) 0.00005, (Probability) 0.00005, EInterpretationCategory.III)
                        },
                        false,
                        (Probability) 0.000549975, EFailureMechanismAssemblyMethod.UnCorrelated);

                    yield return new TestCaseData(
                        new[]
                        {
                            new Tuple<Probability, Probability, EInterpretationCategory>((Probability) 1.0/1234.0, (Probability) 1.0/23.0, EInterpretationCategory.IIIMin),
                            new Tuple<Probability, Probability, EInterpretationCategory>(Probability.NaN, Probability.NaN, EInterpretationCategory.NotDominant),
                            new Tuple<Probability, Probability, EInterpretationCategory>((Probability) 1.0/1820.0, (Probability) 1.0/781.0, EInterpretationCategory.IIIMin)
                        },
                        false,
                        (Probability)0.0116693679, EFailureMechanismAssemblyMethod.Correlated);

                    yield return new TestCaseData(
                        new[]
                        {
                            new Tuple<Probability, Probability, EInterpretationCategory>(Probability.NaN, Probability.NaN, EInterpretationCategory.NotDominant),
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