﻿// Copyright (C) Rijkswaterstaat 2022. All rights reserved.
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
            void Call() => assembler.CalculateFailureMechanismFailureProbabilityBoi1A1(1.0, null, false);

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
            void Call() => assembler.CalculateFailureMechanismFailureProbabilityBoi1A1(1.0, Enumerable.Empty<Probability>(), false);

            // Assert
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call, new[]
            {
                new AssemblyErrorMessage("failureMechanismSectionAssemblyResults", EAssemblyErrors.EmptyResultsList)
            });
        }

        [Test]
        public void CalculateFailureMechanismFailureProbabilityBoi1A1_LengthEffectFactorInvalid_ThrowsAssemblyException()
        {
            // Setup
            var assembler = new FailureMechanismResultAssembler();

            // Call
            void Call() => assembler.CalculateFailureMechanismFailureProbabilityBoi1A1(0.0, new[]
            {
                new Probability(0.0)
            }, false);

            // Assert
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call, new[]
            {
                new AssemblyErrorMessage("lengthEffectFactor", EAssemblyErrors.LengthEffectFactorOutOfRange)
            });
        }

        [Test]
        public void CalculateFailureMechanismFailureProbabilityBoi1A1_AssemblyResultsWithUndefinedProbabilitiesAndPartialAssemblyFalse_ThrowsAssemblyException()
        {
            // Setup
            var assembler = new FailureMechanismResultAssembler();

            // Call
            void Call() => assembler.CalculateFailureMechanismFailureProbabilityBoi1A1(1.0, new[]
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
            FailureMechanismAssemblyResult result = assembler.CalculateFailureMechanismFailureProbabilityBoi1A1(1.0, new[]
            {
                Probability.Undefined,
                Probability.Undefined
            }, true);

            // Assert
            var expectedResult = new FailureMechanismAssemblyResult(new Probability(0.0), EFailureMechanismAssemblyMethod.Correlated);
            AssertFailureMechanismAssemblyResult(expectedResult, result);
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
            FailureMechanismAssemblyResult assemblyResult = assembler.CalculateFailureMechanismFailureProbabilityBoi1A1(1.0, new[]
            {
                new Probability(failureMechanismSectionAssemblyResult)
            }, false);

            // Assert
            var expectedAssemblyResult = new FailureMechanismAssemblyResult(
                new Probability(failureMechanismSectionAssemblyResult), EFailureMechanismAssemblyMethod.Uncorrelated);
            AssertFailureMechanismAssemblyResult(expectedAssemblyResult, assemblyResult);
        }

        [Test]
        [TestCaseSource(nameof(GetCalculateFailureMechanismFailureProbabilityBoi1A1Cases))]
        public void CalculateFailureMechanismFailureProbabilityBoi1A1_FailureMechanismSectionAssemblyResultsAndPartialAssemblyFalse_ReturnsExpectedResult(
            double lenghtEffectFactor, IEnumerable<Probability> failureMechanismSectionAssemblyResults,
            FailureMechanismAssemblyResult expectedAssemblyResult)
        {
            // Setup
            var assembler = new FailureMechanismResultAssembler();

            // Call
            FailureMechanismAssemblyResult assemblyResult = assembler.CalculateFailureMechanismFailureProbabilityBoi1A1(
                lenghtEffectFactor, failureMechanismSectionAssemblyResults, false);

            // Assert
            AssertFailureMechanismAssemblyResult(expectedAssemblyResult, assemblyResult);
        }

        [Test]
        [TestCaseSource(nameof(GetCalculateFailureMechanismFailureProbabilityBoi1A1Cases))]
        public void CalculateFailureMechanismFailureProbabilityBoi1A1_FailureMechanismSectionAssemblyResultsAndPartialAssemblyTrue_ReturnsExpectedResult(
            double lenghtEffectFactor, IEnumerable<Probability> failureMechanismSectionAssemblyResults,
            FailureMechanismAssemblyResult expectedAssemblyResult)
        {
            // Setup
            failureMechanismSectionAssemblyResults = failureMechanismSectionAssemblyResults.Concat(new[]
            {
                Probability.Undefined
            });

            var assembler = new FailureMechanismResultAssembler();

            // Call
            FailureMechanismAssemblyResult assemblyResult = assembler.CalculateFailureMechanismFailureProbabilityBoi1A1(
                lenghtEffectFactor, failureMechanismSectionAssemblyResults, true);

            // Assert
            AssertFailureMechanismAssemblyResult(expectedAssemblyResult, assemblyResult);
        }

        private static IEnumerable<TestCaseData> GetCalculateFailureMechanismFailureProbabilityBoi1A1Cases()
        {
            yield return new TestCaseData(
                14.4,
                new[]
                {
                    new Probability(0.0),
                    new Probability(0.1)
                },
                new FailureMechanismAssemblyResult(new Probability(0.1), EFailureMechanismAssemblyMethod.Uncorrelated));

            yield return new TestCaseData(
                14.4,
                new[]
                {
                    new Probability(0.0),
                    new Probability(0.0001),
                    new Probability(0.0005),
                    new Probability(0.0007)
                },
                new FailureMechanismAssemblyResult(new Probability(0.00129953), EFailureMechanismAssemblyMethod.Uncorrelated));

            yield return new TestCaseData(
                14.4,
                new[]
                {
                    new Probability(0.0005),
                    new Probability(0.00005)
                },
                new FailureMechanismAssemblyResult(new Probability(0.000549975), EFailureMechanismAssemblyMethod.Uncorrelated));

            yield return new TestCaseData(
                14.4,
                new[]
                {
                    new Probability(1.0 / 23.0),
                    new Probability(0.0),
                    new Probability(1.0 / 781.0)
                },
                new FailureMechanismAssemblyResult(new Probability(0.0447030006), EFailureMechanismAssemblyMethod.Uncorrelated));

            yield return new TestCaseData(
                2,
                new[]
                {
                    new Probability(1.0 / 230.0),
                    new Probability(1.0 / 264.0),
                    new Probability(1.0 / 781.0)
                },
                new FailureMechanismAssemblyResult(new Probability(0.00869565217), EFailureMechanismAssemblyMethod.Correlated));

            yield return new TestCaseData(
                14.4,
                new[]
                {
                    new Probability(0.0),
                    new Probability(0.0),
                    new Probability(0.0)
                },
                new FailureMechanismAssemblyResult(new Probability(0.0), EFailureMechanismAssemblyMethod.Correlated));
        }

        #endregion

        #region CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2

        [Test]
        public void CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2_FailureMechanismSectionAssemblyResultsNull_ThrowsArgumentNullException()
        {
            // Setup
            var assembler = new FailureMechanismResultAssembler();

            // Call
            void Call() => assembler.CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2(1.0, null, false);

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
                1.0, Enumerable.Empty<ResultWithProfileAndSectionProbabilities>(), false);

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
            void Call() => assembler.CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2(0.0, new[]
            {
                new ResultWithProfileAndSectionProbabilities(new Probability(0.0), new Probability(0.0))
            }, false);

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
            void Call() => assembler.CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2(1.0, new[]
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
        public void CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2_AssemblyResultsWithOnlyUndefinedProbabilitiesAndPartialAssemblyTrue_ReturnsExpectedResult()
        {
            // Setup
            var assembler = new FailureMechanismResultAssembler();

            // Call
            FailureMechanismAssemblyResult assemblyResult = assembler.CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2(1.0, new[]
            {
                new ResultWithProfileAndSectionProbabilities(Probability.Undefined, Probability.Undefined),
                new ResultWithProfileAndSectionProbabilities(Probability.Undefined, Probability.Undefined)
            }, true);

            // Assert
            var expectedAssemblyResult = new FailureMechanismAssemblyResult(new Probability(0.0), EFailureMechanismAssemblyMethod.Correlated);
            AssertFailureMechanismAssemblyResult(expectedAssemblyResult, assemblyResult);
        }

        [Test]
        [TestCaseSource(nameof(GetCalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2Cases))]
        public void CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2_FailureMechanismSectionAssemblyResultsAndPartialAssemblyFalse_ReturnsExpectedResult(
            IEnumerable<ResultWithProfileAndSectionProbabilities> failureMechanismSectionAssemblyResults,
            FailureMechanismAssemblyResult expectedAssemblyResult)
        {
            // Setup
            var assembler = new FailureMechanismResultAssembler();

            // Call
            FailureMechanismAssemblyResult assemblyResult = assembler.CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2(
                14.4, failureMechanismSectionAssemblyResults, false);

            // Assert
            AssertFailureMechanismAssemblyResult(expectedAssemblyResult, assemblyResult);
        }

        [Test]
        [TestCaseSource(nameof(GetCalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2Cases))]
        public void CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2_FailureMechanismSectionAssemblyResultsAndPartialAssemblyTrue_ReturnsExpectedResult(
            IEnumerable<ResultWithProfileAndSectionProbabilities> failureMechanismSectionAssemblyResults,
            FailureMechanismAssemblyResult expectedAssemblyResult)
        {
            // Setup
            failureMechanismSectionAssemblyResults = failureMechanismSectionAssemblyResults.Concat(new[]
            {
                new ResultWithProfileAndSectionProbabilities(Probability.Undefined, Probability.Undefined)
            });

            var assembler = new FailureMechanismResultAssembler();

            // Call
            FailureMechanismAssemblyResult assemblyResult = assembler.CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2(
                14.4, failureMechanismSectionAssemblyResults, true);

            // Assert
            AssertFailureMechanismAssemblyResult(expectedAssemblyResult, assemblyResult);
        }

        private static IEnumerable<TestCaseData> GetCalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2Cases()
        {
            yield return new TestCaseData(
                new[]
                {
                    new ResultWithProfileAndSectionProbabilities(new Probability(0.0), new Probability(0.0)),
                    new ResultWithProfileAndSectionProbabilities(new Probability(0.1), new Probability(0.1))
                },
                new FailureMechanismAssemblyResult(new Probability(0.1), EFailureMechanismAssemblyMethod.Uncorrelated));

            yield return new TestCaseData(
                new[]
                {
                    new ResultWithProfileAndSectionProbabilities(new Probability(0.0), new Probability(0.0)),
                    new ResultWithProfileAndSectionProbabilities(new Probability(0.0001), new Probability(0.001)),
                    new ResultWithProfileAndSectionProbabilities(new Probability(0.0005), new Probability(0.005)),
                    new ResultWithProfileAndSectionProbabilities(new Probability(0.0007), new Probability(0.0008))
                },
                new FailureMechanismAssemblyResult(new Probability(0.006790204), EFailureMechanismAssemblyMethod.Uncorrelated));

            yield return new TestCaseData(
                new[]
                {
                    new ResultWithProfileAndSectionProbabilities(new Probability(0.0005), new Probability(0.0005)),
                    new ResultWithProfileAndSectionProbabilities(new Probability(0.00005), new Probability(0.00005))
                },
                new FailureMechanismAssemblyResult(new Probability(0.000549975), EFailureMechanismAssemblyMethod.Uncorrelated));

            yield return new TestCaseData(
                new[]
                {
                    new ResultWithProfileAndSectionProbabilities(new Probability(1.0 / 1234.0), new Probability(1.0 / 23.0)),
                    new ResultWithProfileAndSectionProbabilities(new Probability(1.0 / 1820.0), new Probability(1.0 / 781.0))
                },
                new FailureMechanismAssemblyResult(new Probability(0.0116693679), EFailureMechanismAssemblyMethod.Correlated));

            yield return new TestCaseData(
                new[]
                {
                    new ResultWithProfileAndSectionProbabilities(new Probability(0.0), new Probability(0.0)),
                    new ResultWithProfileAndSectionProbabilities(new Probability(0.0), new Probability(0.0))
                },
                new FailureMechanismAssemblyResult(new Probability(0.0), EFailureMechanismAssemblyMethod.Correlated));
        }

        #endregion
    }
}