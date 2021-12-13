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
using Assembly.Kernel.Model.FailurePathSections;
using NUnit.Framework;

namespace Assembly.Kernel.Tests.Implementations
{
    [TestFixture]
    public class FailurePathResultAssemblerTests
    {
        private readonly double lengthEffectFactor1 = 14.4;
        private readonly double lengthEffectFactor2 = 10;
        private IFailurePathResultAssembler assembler;

        [SetUp]
        public void Init()
        {
            assembler = new FailurePathResultAssembler();
        }

        [Test, TestCaseSource(typeof(AssembleFailurePathTestData), nameof(AssembleFailurePathTestData.Wbi1B1))]
        public void Wbi1B1FailureProbabilityTests(Tuple<Probability, Probability>[] failureProbabilities, Probability expectedResult)
        {
            // Use correct probabilities
            var result = assembler.AssembleFailurePathWbi1B1(lengthEffectFactor1,
                failureProbabilities.Select(failureProbability =>
                    new
                        FailurePathSectionAssemblyResult(
                            failureProbability.Item1, failureProbability.Item2, EInterpretationCategory.III)),
                true);

            Assert.NotNull(result);
            Assert.AreEqual(expectedResult, result, 1e-10);
        }

        [Test]
        public void Wbi1B1LengthEffectFactor()
        {
            var result = assembler.AssembleFailurePathWbi1B1(5,
                new[]
                {
                    new FailurePathSectionAssemblyResult((Probability) 0.001, (Probability) 0.001,
                        EInterpretationCategory.II),
                    new FailurePathSectionAssemblyResult((Probability) 0.001, (Probability) 0.001,
                        EInterpretationCategory.II),
                    new FailurePathSectionAssemblyResult((Probability) 0.001, (Probability) 0.001,
                        EInterpretationCategory.IIIMin),
                    new FailurePathSectionAssemblyResult((Probability) 0.001, (Probability) 0.001,
                        EInterpretationCategory.IMin),
                    new FailurePathSectionAssemblyResult((Probability) 0.001, (Probability) 0.001,
                        EInterpretationCategory.IIMin),
                    new FailurePathSectionAssemblyResult((Probability) 0.001, (Probability) 0.001,
                        EInterpretationCategory.IIIMin)
                },
                false);

            Assert.NotNull(result);
            Assert.AreEqual(0.005, result, 1e-4);
        }

        [Test]
        public void Wbi1B1AllNotApplicable()
        {
            var result = assembler.AssembleFailurePathWbi1B1(lengthEffectFactor2,
                new[]
                {
                    new FailurePathSectionAssemblyResult((Probability) 0.0, (Probability) 0.0,
                        EInterpretationCategory.III),
                    new FailurePathSectionAssemblyResult((Probability) 0.0, (Probability) 0.0,
                        EInterpretationCategory.III),
                    new FailurePathSectionAssemblyResult((Probability) 0.0, (Probability) 0.0,
                        EInterpretationCategory.III)
                },
                false);

            Assert.AreEqual(0.0, result);
        }

        [Test]
        public void Wbi1B1NoResult()
        {
            var result = assembler.AssembleFailurePathWbi1B1(lengthEffectFactor2,
                new[]
                {
                    new FailurePathSectionAssemblyResult(Probability.NaN, Probability.NaN, EInterpretationCategory.D),
                    new FailurePathSectionAssemblyResult(Probability.NaN, Probability.NaN, EInterpretationCategory.D),
                    new FailurePathSectionAssemblyResult(Probability.NaN, Probability.NaN, EInterpretationCategory.D)
                },
                false);

            Assert.IsNaN(result.Value);
        }

        [Test]
        public void Wbi1B1Partial()
        {
            var result = assembler.AssembleFailurePathWbi1B1(lengthEffectFactor2,
                new[]
                {
                    new FailurePathSectionAssemblyResult((Probability) 0.9, (Probability) 0.9,
                        EInterpretationCategory.D),
                    new FailurePathSectionAssemblyResult((Probability) 0.000026, (Probability) 0.000026,
                        EInterpretationCategory.D),
                    new FailurePathSectionAssemblyResult((Probability) 0.000010, (Probability) 0.000010,
                        EInterpretationCategory.II),
                    new FailurePathSectionAssemblyResult((Probability) 0.0000011, (Probability) 0.0000011,
                        EInterpretationCategory.II),
                    new FailurePathSectionAssemblyResult((Probability) 0.000015, (Probability) 0.000015,
                        EInterpretationCategory.II),
                    new FailurePathSectionAssemblyResult((Probability) 0.000009, (Probability) 0.000009,
                        EInterpretationCategory.III),
                    new FailurePathSectionAssemblyResult(Probability.NaN, Probability.NaN, EInterpretationCategory.III),
                    new FailurePathSectionAssemblyResult((Probability) 0.0, (Probability) 0.0,
                        EInterpretationCategory.IIIMin)
                },
                true);

            Assert.NotNull(result);
            Assert.AreEqual(0.9, result, 1e-4);
        }

        [Test]
        public void Wbi1B1DominantPartial()
        {
            var result = assembler.AssembleFailurePathWbi1B1(lengthEffectFactor2,
                new[]
                {
                    new FailurePathSectionAssemblyResult(Probability.NaN, Probability.NaN,
                        EInterpretationCategory.IMin),
                    new FailurePathSectionAssemblyResult((Probability) 0.00026, (Probability) 0.00026,
                        EInterpretationCategory.D),
                    new FailurePathSectionAssemblyResult((Probability) 0.00026, (Probability) 0.00026,
                        EInterpretationCategory.D)
                },
                false);

            Assert.IsNaN(result.Value);
        }


        #region Input handling

        [Test]
        public void Wbi1B1EmptyResults()
        {
            try
            {
                var result = assembler.AssembleFailurePathWbi1B1(lengthEffectFactor2,
                    new FailurePathSectionAssemblyResult[]{}, 
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
                var result = assembler.AssembleFailurePathWbi1B1(lengthEffectFactor2,
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
                assembler.AssembleFailurePathWbi1B1(lengthEffectFactor,
                    new[] { new FailurePathSectionAssemblyResult(Probability.NaN, Probability.NaN, EInterpretationCategory.D) },
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

        private class AssembleFailurePathTestData
        {
            public static IEnumerable Wbi1B1
            {
                get
                {
                    yield return new TestCaseData(
                        new[]
                        {
                            new Tuple<Probability, Probability>((Probability) 0.0, (Probability) 0.0),
                            new Tuple<Probability, Probability>((Probability) 0.1, (Probability) 0.1)
                        },
                        (Probability) 0.1);

                    yield return new TestCaseData(
                        new[]
                        {
                            new Tuple<Probability, Probability>((Probability) 0.0, (Probability) 0.0),
                            new Tuple<Probability, Probability>((Probability) 0.0001, (Probability) 0.001),
                            new Tuple<Probability, Probability>((Probability) 0.0005, (Probability) 0.005),
                            new Tuple<Probability, Probability>((Probability) 0.0007, (Probability) 0.0008)
                        },
                        (Probability) 0.006790204);

                    yield return new TestCaseData(
                        new[]
                        {
                            new Tuple<Probability, Probability>((Probability) 0.0005, (Probability) 0.0005),
                            new Tuple<Probability, Probability>((Probability) 0.00005, (Probability) 0.00005)
                        },
                        (Probability) 0.000549975);
                }
            }
        }
    }
}