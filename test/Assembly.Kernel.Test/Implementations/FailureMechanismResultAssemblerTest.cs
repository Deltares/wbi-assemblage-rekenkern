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

using System;
using System.Collections.Generic;
using System.Linq;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Implementations;
using Assembly.Kernel.Interfaces;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.FailureMechanismSections;
using NUnit.Framework;

namespace Assembly.Kernel.Test.Implementations
{
    [TestFixture]
    public class FailureMechanismResultAssemblerTest
    {
        [Test]
        public void Constructor_ExpectedValues()
        {
            // Call
            var assembler = new FailureMechanismResultAssembler();

            // Assert
            Assert.IsInstanceOf<IFailureMechanismResultAssembler>(assembler);
        }

        private static void AssertFailureMechanismAssemblyResult(FailureMechanismAssemblyResult expectedResult, FailureMechanismAssemblyResult actualResult)
        {
            Assert.AreEqual(expectedResult.Probability, actualResult.Probability, 1e-6);
            Assert.AreEqual(expectedResult.AssemblyMethod, actualResult.AssemblyMethod);
        }

        #region CalculateFailureMechanismFailureProbabilityBoi1A1

        [Test]
        public void CalculateFailureMechanismFailureProbabilityBoi1A1_FailureMechanismSectionAssemblyResultsNull_ThrowsArgumentNullException()
        {
            // Setup
            var assembler = new FailureMechanismResultAssembler();

            // Call
            void Call() => assembler.CalculateFailureMechanismFailureProbabilityBoi1A1(null, false);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(Call);
            Assert.AreEqual("failureMechanismSectionAssemblyResults", exception.ParamName);
        }

        [Test]
        public void CalculateFailureMechanismFailureProbabilityBoi1A1_FailureMechanismSectionAssemblyResultsEmpty_ThrowsAssemblyException()
        {
            // Setup
            var assembler = new FailureMechanismResultAssembler();

            // Call
            void Call() => assembler.CalculateFailureMechanismFailureProbabilityBoi1A1(Enumerable.Empty<Probability>(), false);

            // Assert
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call, new[]
            {
                new AssemblyErrorMessage("failureMechanismSectionAssemblyResults", EAssemblyErrors.EmptyResultsList)
            });
        }

        [Test]
        public void CalculateFailureMechanismFailureProbabilityBoi1A1_AssemblyResultsWithUndefinedProbabilitiesAndPartialAssemblyFalse_ThrowsAssemblyException()
        {
            // Setup
            var assembler = new FailureMechanismResultAssembler();

            // Call
            void Call() => assembler.CalculateFailureMechanismFailureProbabilityBoi1A1(new[]
            {
                new Probability(0.0),
                Probability.Undefined,
                new Probability(0.0)
            }, false);

            // Assert
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call, new[]
            {
                new AssemblyErrorMessage("failureMechanismSectionAssemblyResult", EAssemblyErrors.EncounteredOneOrMoreSectionsWithoutResult)
            });
        }

        [Test]
        public void CalculateFailureMechanismFailureProbabilityBoi1A1_AssemblyResultsWithOnlyUndefinedProbabilitiesAndPartialAssemblyTrue_ReturnsExpectedResult()
        {
            // Setup
            var assembler = new FailureMechanismResultAssembler();

            // Call
            Probability result = assembler.CalculateFailureMechanismFailureProbabilityBoi1A1(new[]
            {
                Probability.Undefined,
                Probability.Undefined
            }, true);

            // Assert
            Assert.AreEqual(0.0, result);
        }

        [Test]
        [TestCase(0.0)]
        [TestCase(0.33)]
        [TestCase(0.66)]
        [TestCase(1.0)]
        public void CalculateFailureMechanismFailureProbabilityBoi1A1_OneFailureMechanismSectionAssemblyResult_ReturnsExpectedResult(
            double failureMechanismSectionAssemblyResult)
        {
            // Setup
            var assembler = new FailureMechanismResultAssembler();

            // Call
            Probability assemblyResult = assembler.CalculateFailureMechanismFailureProbabilityBoi1A1(new[]
            {
                new Probability(failureMechanismSectionAssemblyResult)
            }, false);

            // Assert
            Assert.AreEqual(failureMechanismSectionAssemblyResult, assemblyResult, 1e-3);
        }

        [Test]
        [TestCaseSource(nameof(GetCalculateFailureMechanismFailureProbabilityBoi1A1Cases))]
        public void CalculateFailureMechanismFailureProbabilityBoi1A1_FailureMechanismSectionAssemblyResultsAndPartialAssemblyFalse_ReturnsExpectedResult(
            IEnumerable<Probability> failureMechanismSectionAssemblyResults,
            Probability expectedAssemblyResult)
        {
            // Setup
            var assembler = new FailureMechanismResultAssembler();

            // Call
            Probability assemblyResult = assembler.CalculateFailureMechanismFailureProbabilityBoi1A1(
                failureMechanismSectionAssemblyResults, false);

            // Assert
            Assert.AreEqual(expectedAssemblyResult, assemblyResult, 1e-10);
        }

        [Test]
        [TestCaseSource(nameof(GetCalculateFailureMechanismFailureProbabilityBoi1A1Cases))]
        public void CalculateFailureMechanismFailureProbabilityBoi1A1_FailureMechanismSectionAssemblyResultsAndPartialAssemblyTrue_ReturnsExpectedResult(
            IEnumerable<Probability> failureMechanismSectionAssemblyResults,
            Probability expectedAssemblyResult)
        {
            // Setup
            failureMechanismSectionAssemblyResults = failureMechanismSectionAssemblyResults.Concat(new[]
            {
                Probability.Undefined
            });

            var assembler = new FailureMechanismResultAssembler();

            // Call
            Probability assemblyResult = assembler.CalculateFailureMechanismFailureProbabilityBoi1A1(
                failureMechanismSectionAssemblyResults, true);

            // Assert
            Assert.AreEqual(expectedAssemblyResult, assemblyResult, 1e-10);
        }

        private static IEnumerable<TestCaseData> GetCalculateFailureMechanismFailureProbabilityBoi1A1Cases()
        {
            yield return new TestCaseData(
                new[]
                {
                    new Probability(0.0),
                    new Probability(0.1)
                },
                new Probability(0.1));

            yield return new TestCaseData(
                new[]
                {
                    new Probability(0.0),
                    new Probability(0.0001),
                    new Probability(0.0005),
                    new Probability(0.0007)
                },
                new Probability(0.00129953));

            yield return new TestCaseData(
                new[]
                {
                    new Probability(0.0005),
                    new Probability(0.00005)
                },
                new Probability(0.000549975));

            yield return new TestCaseData(
                new[]
                {
                    new Probability(1.0 / 23.0),
                    new Probability(0.0),
                    new Probability(1.0 / 781.0)
                },
                new Probability(0.0447030006));

            yield return new TestCaseData(
                new[]
                {
                    new Probability(1.0 / 230.0),
                    new Probability(1.0 / 264.0),
                    new Probability(1.0 / 781.0)
                },
                new Probability(0.0093892496));

            yield return new TestCaseData(
                new[]
                {
                    new Probability(0.0),
                    new Probability(0.0),
                    new Probability(0.0)
                },
                new Probability(0.0));
        }

        #endregion

        #region CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2

        [Test]
        public void CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2_FailureMechanismSectionAssemblyResultsNull_ThrowsArgumentNullException()
        {
            // Setup
            var assembler = new FailureMechanismResultAssembler();

            // Call
            void Call() => assembler.CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2(null, 1.0, false);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(Call);
            Assert.AreEqual("failureMechanismSectionAssemblyResults", exception.ParamName);
        }

        [Test]
        public void CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2_FailureMechanismSectionAssemblyResultsEmpty_ThrowsAssemblyException()
        {
            // Setup
            var assembler = new FailureMechanismResultAssembler();

            // Call
            void Call() => assembler.CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2(
                Enumerable.Empty<Probability>(), 1.0, false);

            // Assert
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call, new[]
            {
                new AssemblyErrorMessage("failureMechanismSectionAssemblyResults", EAssemblyErrors.EmptyResultsList)
            });
        }

        [Test]
        public void CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2_LengthEffectFactorInvalid_ThrowsAssemblyException()
        {
            // Setup
            var assembler = new FailureMechanismResultAssembler();

            // Call
            void Call() => assembler.CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2(new[]
            {
                new Probability(0.0)
            }, 0.0, false);

            // Assert
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call, new[]
            {
                new AssemblyErrorMessage("lengthEffectFactor", EAssemblyErrors.LengthEffectFactorOutOfRange)
            });
        }

        [Test]
        public void CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2_AssemblyResultsWithUndefinedProbabilitiesAndPartialAssemblyFalse_ThrowsAssemblyException()
        {
            // Setup
            var assembler = new FailureMechanismResultAssembler();

            // Call
            void Call() => assembler.CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2(new[]
            {
                new Probability(0.0),
                Probability.Undefined,
                new Probability(0.0)
            }, 1.0, false);

            // Assert
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call, new[]
            {
                new AssemblyErrorMessage("failureMechanismSectionAssemblyResult", EAssemblyErrors.EncounteredOneOrMoreSectionsWithoutResult)
            });
        }

        [Test]
        public void CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2_AssemblyResultsWithOnlyUndefinedProbabilitiesAndPartialAssemblyTrue_ReturnsExpectedResult()
        {
            // Setup
            var assembler = new FailureMechanismResultAssembler();

            // Call
            Probability assemblyResult = assembler.CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2(new[]
            {
                Probability.Undefined,
                Probability.Undefined
            }, 1.0, true);

            // Assert
            Assert.AreEqual(0.0, assemblyResult);
        }

        [Test]
        [TestCaseSource(nameof(GetCalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2Cases))]
        public void CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2_FailureMechanismSectionAssemblyResultsAndPartialAssemblyFalse_ReturnsExpectedResult(
            IEnumerable<Probability> failureMechanismSectionAssemblyResults,
            Probability expectedAssemblyResult)
        {
            // Setup
            var assembler = new FailureMechanismResultAssembler();

            // Call
            Probability assemblyResult = assembler.CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2(
                failureMechanismSectionAssemblyResults, 14.4, false);

            // Assert
            Assert.AreEqual(expectedAssemblyResult, assemblyResult, 1e-10);
        }

        [Test]
        [TestCaseSource(nameof(GetCalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2Cases))]
        public void CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2_FailureMechanismSectionAssemblyResultsAndPartialAssemblyTrue_ReturnsExpectedResult(
            IEnumerable<Probability> failureMechanismSectionAssemblyResults,
            Probability expectedAssemblyResult)
        {
            // Setup
            failureMechanismSectionAssemblyResults = failureMechanismSectionAssemblyResults.Concat(new[]
            {
                Probability.Undefined
            });

            var assembler = new FailureMechanismResultAssembler();

            // Call
            Probability assemblyResult = assembler.CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2(
                failureMechanismSectionAssemblyResults, 14.4, true);

            // Assert
            Assert.AreEqual(expectedAssemblyResult, assemblyResult, 1e-10);
        }

        private static IEnumerable<TestCaseData> GetCalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2Cases()
        {
            yield return new TestCaseData(
                new[]
                {
                    new Probability(0.0),
                    new Probability(0.1)
                },
                new Probability(1.0));

            yield return new TestCaseData(
                new[]
                {
                    new Probability(0.0),
                    new Probability(0.0001),
                    new Probability(0.0005),
                    new Probability(0.0007)
                },
                new Probability(0.01008));

            yield return new TestCaseData(
                new[]
                {
                    new Probability(0.0005),
                    new Probability(0.00005)
                },
                new Probability(0.0072));

            yield return new TestCaseData(
                new[]
                {
                    new Probability(1.0 / 23.0),
                    new Probability(0.0),
                    new Probability(1.0 / 781.0)
                },
                new Probability(0.6260869565));

            yield return new TestCaseData(
                new[]
                {
                    new Probability(1.0 / 230.0),
                    new Probability(1.0 / 264.0),
                    new Probability(1.0 / 781.0)
                },
                new Probability(0.0626086956));

            yield return new TestCaseData(
                new[]
                {
                    new Probability(0.0),
                    new Probability(0.0),
                    new Probability(0.0)
                },
                new Probability(0.0));
        }

        #endregion

        #region CalculateFailureMechanismFailureProbabilityBoi1A3

        [Test]
        public void CalculateFailureMechanismFailureProbabilityBoi1A3_FailureMechanismSectionAssemblyResultsNull_ThrowsArgumentNullException()
        {
            // Setup
            var assembler = new FailureMechanismResultAssembler();

            // Call
            void Call() => assembler.CalculateFailureMechanismFailureProbabilityBoi1A3(null, false);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(Call);
            Assert.AreEqual("failureMechanismSectionAssemblyResults", exception.ParamName);
        }

        [Test]
        public void CalculateFailureMechanismFailureProbabilityBoi1A3_FailureMechanismSectionAssemblyResultsEmpty_ThrowsAssemblyException()
        {
            // Setup
            var assembler = new FailureMechanismResultAssembler();

            // Call
            void Call() => assembler.CalculateFailureMechanismFailureProbabilityBoi1A3(
                Enumerable.Empty<ResultWithProfileAndSectionProbabilities>(), false);

            // Assert
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call, new[]
            {
                new AssemblyErrorMessage("failureMechanismSectionAssemblyResults", EAssemblyErrors.EmptyResultsList)
            });
        }

        [Test]
        public void CalculateFailureMechanismFailureProbabilityBoi1A3_AssemblyResultsWithUndefinedProbabilitiesAndPartialAssemblyFalse_ThrowsAssemblyException()
        {
            // Setup
            var assembler = new FailureMechanismResultAssembler();

            // Call
            void Call() => assembler.CalculateFailureMechanismFailureProbabilityBoi1A3(new[]
            {
                new ResultWithProfileAndSectionProbabilities(new Probability(0.0), new Probability(0.0)),
                new ResultWithProfileAndSectionProbabilities(Probability.Undefined, Probability.Undefined),
                new ResultWithProfileAndSectionProbabilities(new Probability(0.0), new Probability(0.0))
            }, false);

            // Assert
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call, new[]
            {
                new AssemblyErrorMessage("failureMechanismSectionAssemblyResult", EAssemblyErrors.EncounteredOneOrMoreSectionsWithoutResult)
            });
        }

        [Test]
        public void CalculateFailureMechanismFailureProbabilityBoi1A3_AssemblyResultsWithOnlyUndefinedProbabilitiesAndPartialAssemblyTrue_ReturnsExpectedResult()
        {
            // Setup
            var assembler = new FailureMechanismResultAssembler();

            // Call
            Probability assemblyResult = assembler.CalculateFailureMechanismFailureProbabilityBoi1A3(new[]
            {
                new ResultWithProfileAndSectionProbabilities(Probability.Undefined, Probability.Undefined),
                new ResultWithProfileAndSectionProbabilities(Probability.Undefined, Probability.Undefined)
            }, true);

            // Assert
            Assert.AreEqual(0.0, assemblyResult);
        }

        [Test]
        [TestCaseSource(nameof(GetCalculateFailureMechanismFailureProbabilityBoi1A3Cases))]
        public void CalculateFailureMechanismFailureProbabilityBoi1A3_FailureMechanismSectionAssemblyResultsAndPartialAssemblyFalse_ReturnsExpectedResult(
            IEnumerable<ResultWithProfileAndSectionProbabilities> failureMechanismSectionAssemblyResults,
            Probability expectedAssemblyResult)
        {
            // Setup
            var assembler = new FailureMechanismResultAssembler();

            // Call
            Probability assemblyResult = assembler.CalculateFailureMechanismFailureProbabilityBoi1A3(
                failureMechanismSectionAssemblyResults, false);

            // Assert
            Assert.AreEqual(expectedAssemblyResult, assemblyResult, 1e-10);
        }

        [Test]
        [TestCaseSource(nameof(GetCalculateFailureMechanismFailureProbabilityBoi1A3Cases))]
        public void CalculateFailureMechanismFailureProbabilityBoi1A3_FailureMechanismSectionAssemblyResultsAndPartialAssemblyTrue_ReturnsExpectedResult(
            IEnumerable<ResultWithProfileAndSectionProbabilities> failureMechanismSectionAssemblyResults,
            Probability expectedAssemblyResult)
        {
            // Setup
            failureMechanismSectionAssemblyResults = failureMechanismSectionAssemblyResults.Concat(new[]
            {
                new ResultWithProfileAndSectionProbabilities(Probability.Undefined, Probability.Undefined)
            });

            var assembler = new FailureMechanismResultAssembler();

            // Call
            Probability assemblyResult = assembler.CalculateFailureMechanismFailureProbabilityBoi1A3(
                failureMechanismSectionAssemblyResults, true);

            // Assert
            Assert.AreEqual(expectedAssemblyResult, assemblyResult, 1e-10);
        }

        private static IEnumerable<TestCaseData> GetCalculateFailureMechanismFailureProbabilityBoi1A3Cases()
        {
            yield return new TestCaseData(
                new[]
                {
                    new ResultWithProfileAndSectionProbabilities(new Probability(0.0), new Probability(0.0)),
                    new ResultWithProfileAndSectionProbabilities(new Probability(0.1), new Probability(0.1))
                },
                new Probability(0.1));

            yield return new TestCaseData(
                new[]
                {
                    new ResultWithProfileAndSectionProbabilities(new Probability(0.0), new Probability(0.0)),
                    new ResultWithProfileAndSectionProbabilities(new Probability(0.0001), new Probability(0.001)),
                    new ResultWithProfileAndSectionProbabilities(new Probability(0.0005), new Probability(0.005)),
                    new ResultWithProfileAndSectionProbabilities(new Probability(0.0007), new Probability(0.0008))
                },
                new Probability(0.006790204));

            yield return new TestCaseData(
                new[]
                {
                    new ResultWithProfileAndSectionProbabilities(new Probability(0.0005), new Probability(0.0005)),
                    new ResultWithProfileAndSectionProbabilities(new Probability(0.00005), new Probability(0.00005))
                },
                new Probability(0.000549975));

            yield return new TestCaseData(
                new[]
                {
                    new ResultWithProfileAndSectionProbabilities(new Probability(1.0 / 1234.0), new Probability(1.0 / 23.0)),
                    new ResultWithProfileAndSectionProbabilities(new Probability(1.0 / 1820.0), new Probability(1.0 / 781.0))
                },
                new Probability(0.0447030006));

            yield return new TestCaseData(
                new[]
                {
                    new ResultWithProfileAndSectionProbabilities(new Probability(0.0), new Probability(0.0)),
                    new ResultWithProfileAndSectionProbabilities(new Probability(0.0), new Probability(0.0))
                },
                new Probability(0.0));
        }

        #endregion

        #region CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A4

        [Test]
        public void CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A4_FailureMechanismSectionAssemblyResultsNull_ThrowsArgumentNullException()
        {
            // Setup
            var assembler = new FailureMechanismResultAssembler();

            // Call
            void Call() => assembler.CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A4(null, 1.0, false);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(Call);
            Assert.AreEqual("failureMechanismSectionAssemblyResults", exception.ParamName);
        }

        [Test]
        public void CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A4_FailureMechanismSectionAssemblyResultsEmpty_ThrowsAssemblyException()
        {
            // Setup
            var assembler = new FailureMechanismResultAssembler();

            // Call
            void Call() => assembler.CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A4(
                Enumerable.Empty<ResultWithProfileAndSectionProbabilities>(), 1.0, false);

            // Assert
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call, new[]
            {
                new AssemblyErrorMessage("failureMechanismSectionAssemblyResults", EAssemblyErrors.EmptyResultsList)
            });
        }

        [Test]
        public void CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A4_AssemblyResultsWithUndefinedProbabilitiesAndPartialAssemblyFalse_ThrowsAssemblyException()
        {
            // Setup
            var assembler = new FailureMechanismResultAssembler();

            // Call
            void Call() => assembler.CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A4(new[]
            {
                new ResultWithProfileAndSectionProbabilities(new Probability(0.0), new Probability(0.0)),
                new ResultWithProfileAndSectionProbabilities(Probability.Undefined, Probability.Undefined),
                new ResultWithProfileAndSectionProbabilities(new Probability(0.0), new Probability(0.0))
            }, 1.0, false);

            // Assert
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call, new[]
            {
                new AssemblyErrorMessage("failureMechanismSectionAssemblyResult", EAssemblyErrors.EncounteredOneOrMoreSectionsWithoutResult)
            });
        }

        [Test]
        public void CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A4_LengthEffectFactorInvalid_ThrowsAssemblyException()
        {
            // Setup
            var assembler = new FailureMechanismResultAssembler();

            // Call
            void Call() => assembler.CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A4(new[]
            {
                new ResultWithProfileAndSectionProbabilities(new Probability(0.0), new Probability(0.0))
            }, 0.0, false);

            // Assert
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call, new[]
            {
                new AssemblyErrorMessage("lengthEffectFactor", EAssemblyErrors.LengthEffectFactorOutOfRange)
            });
        }

        [Test]
        public void CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A4_AssemblyResultsWithOnlyUndefinedProbabilitiesAndPartialAssemblyTrue_ReturnsExpectedResult()
        {
            // Setup
            var assembler = new FailureMechanismResultAssembler();

            // Call
            Probability assemblyResult = assembler.CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A4(new[]
            {
                new ResultWithProfileAndSectionProbabilities(Probability.Undefined, Probability.Undefined),
                new ResultWithProfileAndSectionProbabilities(Probability.Undefined, Probability.Undefined)
            }, 1.0, true);

            // Assert
            Assert.AreEqual(0.0, assemblyResult);
        }

        [Test]
        [TestCaseSource(nameof(GetCalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A4Cases))]
        public void CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A4_FailureMechanismSectionAssemblyResultsAndPartialAssemblyFalse_ReturnsExpectedResult(
            IEnumerable<ResultWithProfileAndSectionProbabilities> failureMechanismSectionAssemblyResults,
            Probability expectedAssemblyResult)
        {
            // Setup
            var assembler = new FailureMechanismResultAssembler();

            // Call
            Probability assemblyResult = assembler.CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A4(
                failureMechanismSectionAssemblyResults, 14.4, false);

            // Assert
            Assert.AreEqual(expectedAssemblyResult, assemblyResult, 1e-10);
        }

        [Test]
        [TestCaseSource(nameof(GetCalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A4Cases))]
        public void CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A4_FailureMechanismSectionAssemblyResultsAndPartialAssemblyTrue_ReturnsExpectedResult(
            IEnumerable<ResultWithProfileAndSectionProbabilities> failureMechanismSectionAssemblyResults,
            Probability expectedAssemblyResult)
        {
            // Setup
            failureMechanismSectionAssemblyResults = failureMechanismSectionAssemblyResults.Concat(new[]
            {
                new ResultWithProfileAndSectionProbabilities(Probability.Undefined, Probability.Undefined)
            });

            var assembler = new FailureMechanismResultAssembler();

            // Call
            Probability assemblyResult = assembler.CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A4(
                failureMechanismSectionAssemblyResults, 14.4, true);

            // Assert
            Assert.AreEqual(expectedAssemblyResult, assemblyResult, 1e-10);
        }

        private static IEnumerable<TestCaseData> GetCalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A4Cases()
        {
            yield return new TestCaseData(
                new[]
                {
                    new ResultWithProfileAndSectionProbabilities(new Probability(0.0), new Probability(0.0)),
                    new ResultWithProfileAndSectionProbabilities(new Probability(0.1), new Probability(0.1))
                },
                new Probability(1.0));

            yield return new TestCaseData(
                new[]
                {
                    new ResultWithProfileAndSectionProbabilities(new Probability(0.0), new Probability(0.0)),
                    new ResultWithProfileAndSectionProbabilities(new Probability(0.0001), new Probability(0.001)),
                    new ResultWithProfileAndSectionProbabilities(new Probability(0.0005), new Probability(0.005)),
                    new ResultWithProfileAndSectionProbabilities(new Probability(0.0007), new Probability(0.0008))
                },
                new Probability(0.01008));

            yield return new TestCaseData(
                new[]
                {
                    new ResultWithProfileAndSectionProbabilities(new Probability(0.0005), new Probability(0.0005)),
                    new ResultWithProfileAndSectionProbabilities(new Probability(0.00005), new Probability(0.00005))
                },
                new Probability(0.0072));

            yield return new TestCaseData(
                new[]
                {
                    new ResultWithProfileAndSectionProbabilities(new Probability(1.0 / 1234.0), new Probability(1.0 / 23.0)),
                    new ResultWithProfileAndSectionProbabilities(new Probability(1.0 / 1820.0), new Probability(1.0 / 781.0))
                },
                new Probability(0.0116693679));

            yield return new TestCaseData(
                new[]
                {
                    new ResultWithProfileAndSectionProbabilities(new Probability(0.0), new Probability(0.0)),
                    new ResultWithProfileAndSectionProbabilities(new Probability(0.0), new Probability(0.0))
                },
                new Probability(0.0));
        }

        #endregion
    }
}