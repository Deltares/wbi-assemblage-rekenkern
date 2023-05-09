// Copyright (C) Stichting Deltares and State of the Netherlands 2023. All rights reserved.
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
// All names, logos, and references to "Deltares" are registered trademarks of
// Stichting Deltares and remain full property of Stichting Deltares at all times.
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
                new AssemblyErrorMessage("failureMechanismSectionAssemblyResults", EAssemblyErrors.EncounteredOneOrMoreSectionsWithoutResult)
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

        #region CalculateFailureMechanismFailureProbabilityBoi1A2

        [Test]
        public void CalculateFailureMechanismFailureProbabilityBoi1A2_FailureMechanismSectionAssemblyResultsNull_ThrowsArgumentNullException()
        {
            // Setup
            var assembler = new FailureMechanismResultAssembler();

            // Call
            void Call() => assembler.CalculateFailureMechanismFailureProbabilityBoi1A2(null, 1.0, false);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(Call);
            Assert.AreEqual("failureMechanismSectionAssemblyResults", exception.ParamName);
        }

        [Test]
        public void CalculateFailureMechanismFailureProbabilityBoi1A2_FailureMechanismSectionAssemblyResultsEmpty_ThrowsAssemblyException()
        {
            // Setup
            var assembler = new FailureMechanismResultAssembler();

            // Call
            void Call() => assembler.CalculateFailureMechanismFailureProbabilityBoi1A2(
                Enumerable.Empty<Probability>(), 1.0, false);

            // Assert
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call, new[]
            {
                new AssemblyErrorMessage("failureMechanismSectionAssemblyResults", EAssemblyErrors.EmptyResultsList)
            });
        }

        [Test]
        public void CalculateFailureMechanismFailureProbabilityBoi1A2_LengthEffectFactorInvalid_ThrowsAssemblyException()
        {
            // Setup
            var assembler = new FailureMechanismResultAssembler();

            // Call
            void Call() => assembler.CalculateFailureMechanismFailureProbabilityBoi1A2(new[]
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
        public void CalculateFailureMechanismFailureProbabilityBoi1A2_AssemblyResultsWithUndefinedProbabilitiesAndPartialAssemblyFalse_ThrowsAssemblyException()
        {
            // Setup
            var assembler = new FailureMechanismResultAssembler();

            // Call
            void Call() => assembler.CalculateFailureMechanismFailureProbabilityBoi1A2(new[]
            {
                new Probability(0.0),
                Probability.Undefined,
                new Probability(0.0)
            }, 1.0, false);

            // Assert
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call, new[]
            {
                new AssemblyErrorMessage("failureMechanismSectionAssemblyResults", EAssemblyErrors.EncounteredOneOrMoreSectionsWithoutResult)
            });
        }

        [Test]
        public void CalculateFailureMechanismFailureProbabilityBoi1A2_AssemblyResultsWithOnlyUndefinedProbabilitiesAndPartialAssemblyTrue_ReturnsExpectedResult()
        {
            // Setup
            var assembler = new FailureMechanismResultAssembler();

            // Call
            Probability assemblyResult = assembler.CalculateFailureMechanismFailureProbabilityBoi1A2(new[]
            {
                Probability.Undefined,
                Probability.Undefined
            }, 1.0, true);

            // Assert
            Assert.AreEqual(0.0, assemblyResult);
        }

        [Test]
        [TestCaseSource(nameof(GetCalculateFailureMechanismFailureProbabilityBoi1A2Cases))]
        public void CalculateFailureMechanismFailureProbabilityBoi1A2_FailureMechanismSectionAssemblyResultsAndPartialAssemblyFalse_ReturnsExpectedResult(
            IEnumerable<Probability> failureMechanismSectionAssemblyResults,
            Probability expectedAssemblyResult)
        {
            // Setup
            var assembler = new FailureMechanismResultAssembler();

            // Call
            Probability assemblyResult = assembler.CalculateFailureMechanismFailureProbabilityBoi1A2(
                failureMechanismSectionAssemblyResults, 14.4, false);

            // Assert
            Assert.AreEqual(expectedAssemblyResult, assemblyResult, 1e-10);
        }

        [Test]
        [TestCaseSource(nameof(GetCalculateFailureMechanismFailureProbabilityBoi1A2Cases))]
        public void CalculateFailureMechanismFailureProbabilityBoi1A2_FailureMechanismSectionAssemblyResultsAndPartialAssemblyTrue_ReturnsExpectedResult(
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
            Probability assemblyResult = assembler.CalculateFailureMechanismFailureProbabilityBoi1A2(
                failureMechanismSectionAssemblyResults, 14.4, true);

            // Assert
            Assert.AreEqual(expectedAssemblyResult, assemblyResult, 1e-10);
        }

        private static IEnumerable<TestCaseData> GetCalculateFailureMechanismFailureProbabilityBoi1A2Cases()
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

        #region CalculateFailureMechanismBoundariesBoi1B1

        [Test]
        public void CalculateFailureMechanismBoundariesBoi1B1_FailureMechanismSectionAssemblyResultsNull_ThrowsArgumentNullException()
        {
            // Setup
            var assembler = new FailureMechanismResultAssembler();

            // Call
            void Call() => assembler.CalculateFailureMechanismBoundariesBoi1B1(null, false);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(Call);
            Assert.AreEqual("failureMechanismSectionAssemblyResults", exception.ParamName);
        }

        [Test]
        public void CalculateFailureMechanismBoundariesBoi1B1_FailureMechanismSectionAssemblyResultsEmpty_ThrowsAssemblyException()
        {
            // Setup
            var assembler = new FailureMechanismResultAssembler();

            // Call
            void Call() => assembler.CalculateFailureMechanismBoundariesBoi1B1(Enumerable.Empty<Probability>(), false);

            // Assert
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call, new[]
            {
                new AssemblyErrorMessage("failureMechanismSectionAssemblyResults", EAssemblyErrors.EmptyResultsList)
            });
        }

        [Test]
        public void CalculateFailureMechanismBoundariesBoi1B1_AssemblyResultsWithUndefinedProbabilitiesAndPartialAssemblyFalse_ThrowsAssemblyException()
        {
            // Setup
            var assembler = new FailureMechanismResultAssembler();

            // Call
            void Call() => assembler.CalculateFailureMechanismBoundariesBoi1B1(new[]
            {
                new Probability(0.0),
                Probability.Undefined,
                new Probability(0.0)
            }, false);

            // Assert
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call, new[]
            {
                new AssemblyErrorMessage("failureMechanismSectionAssemblyResults", EAssemblyErrors.EncounteredOneOrMoreSectionsWithoutResult)
            });
        }

        [Test]
        public void CalculateFailureMechanismBoundariesBoi1B1_AssemblyResultsWithOnlyUndefinedProbabilitiesAndPartialAssemblyTrue_ReturnsExpectedResult()
        {
            // Setup
            var assembler = new FailureMechanismResultAssembler();

            // Call
            BoundaryLimits result = assembler.CalculateFailureMechanismBoundariesBoi1B1(new[]
            {
                Probability.Undefined,
                Probability.Undefined
            }, true);

            // Assert
            Assert.AreEqual(0.0, result.LowerLimit);
            Assert.AreEqual(0.0, result.UpperLimit);
        }

        [Test]
        [TestCaseSource(nameof(GetCalculateFailureMechanismBoundariesBoi1B1Cases))]
        public void CalculateFailureMechanismBoundariesBoi1B1_FailureMechanismSectionAssemblyResultsAndPartialAssemblyFalse_ReturnsExpectedResult(
            IEnumerable<Probability> failureMechanismSectionAssemblyResults,
            BoundaryLimits expectedBoundaryLimits)
        {
            // Setup
            var assembler = new FailureMechanismResultAssembler();

            // Call
            BoundaryLimits boundaryLimits = assembler.CalculateFailureMechanismBoundariesBoi1B1(
                failureMechanismSectionAssemblyResults, false);

            // Assert
            Assert.AreEqual(expectedBoundaryLimits.LowerLimit, boundaryLimits.LowerLimit, 1e-10);
            Assert.AreEqual(expectedBoundaryLimits.UpperLimit, boundaryLimits.UpperLimit, 1e-10);
        }

        [Test]
        [TestCaseSource(nameof(GetCalculateFailureMechanismBoundariesBoi1B1Cases))]
        public void CalculateFailureMechanismBoundariesBoi1B1_FailureMechanismSectionAssemblyResultsAndPartialAssemblyTrue_ReturnsExpectedResult(
            IEnumerable<Probability> failureMechanismSectionAssemblyResults,
            BoundaryLimits expectedBoundaryLimits)
        {
            // Setup
            failureMechanismSectionAssemblyResults = failureMechanismSectionAssemblyResults.Concat(new[]
            {
                Probability.Undefined
            });

            var assembler = new FailureMechanismResultAssembler();

            // Call
            BoundaryLimits boundaryLimits = assembler.CalculateFailureMechanismBoundariesBoi1B1(
                failureMechanismSectionAssemblyResults, true);

            // Assert
            Assert.AreEqual(expectedBoundaryLimits.LowerLimit, boundaryLimits.LowerLimit, 1e-10);
            Assert.AreEqual(expectedBoundaryLimits.UpperLimit, boundaryLimits.UpperLimit, 1e-10);
        }

        private static IEnumerable<TestCaseData> GetCalculateFailureMechanismBoundariesBoi1B1Cases()
        {
            yield return new TestCaseData(
                new[]
                {
                    new Probability(0.0),
                    new Probability(0.1)
                },
                new BoundaryLimits(new Probability(0.1), new Probability(0.1)));

            yield return new TestCaseData(
                new[]
                {
                    new Probability(0.0),
                    new Probability(0.0001),
                    new Probability(0.0005),
                    new Probability(0.0007)
                },
                new BoundaryLimits(new Probability(0.0007), new Probability(0.00129953)));

            yield return new TestCaseData(
                new[]
                {
                    new Probability(0.0005),
                    new Probability(0.00005)
                },
                new BoundaryLimits(new Probability(0.0005), new Probability(0.0005499749)));

            yield return new TestCaseData(
                new[]
                {
                    new Probability(1.0 / 23.0),
                    new Probability(0.0),
                    new Probability(1.0 / 781.0)
                },
                new BoundaryLimits(new Probability(0.0434782608), new Probability(0.0447030006)));

            yield return new TestCaseData(
                new[]
                {
                    new Probability(1.0 / 230.0),
                    new Probability(1.0 / 264.0),
                    new Probability(1.0 / 781.0)
                },
                new BoundaryLimits(new Probability(0.0043478261), new Probability(0.0093892496)));

            yield return new TestCaseData(
                new[]
                {
                    new Probability(0.0),
                    new Probability(0.0),
                    new Probability(0.0)
                },
                new BoundaryLimits(new Probability(0.0), new Probability(0.0)));
        }

        #endregion

        #region CalculateFailureMechanismBoundariesBoi1B2

        [Test]
        public void CalculateFailureMechanismBoundariesBoi1B2_FailureMechanismSectionAssemblyResultsNull_ThrowsArgumentNullException()
        {
            // Setup
            var assembler = new FailureMechanismResultAssembler();

            // Call
            void Call() => assembler.CalculateFailureMechanismBoundariesBoi1B2(null, false);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(Call);
            Assert.AreEqual("failureMechanismSectionAssemblyResults", exception.ParamName);
        }

        [Test]
        public void CalculateFailureMechanismBoundariesBoi1B2_FailureMechanismSectionAssemblyResultsEmpty_ThrowsAssemblyException()
        {
            // Setup
            var assembler = new FailureMechanismResultAssembler();

            // Call
            void Call() => assembler.CalculateFailureMechanismBoundariesBoi1B2(
                Enumerable.Empty<ResultWithProfileAndSectionProbabilities>(), false);

            // Assert
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call, new[]
            {
                new AssemblyErrorMessage("failureMechanismSectionAssemblyResults", EAssemblyErrors.EmptyResultsList)
            });
        }

        [Test]
        public void CalculateFailureMechanismBoundariesBoi1B2_AssemblyResultsWithUndefinedProbabilitiesAndPartialAssemblyFalse_ThrowsAssemblyException()
        {
            // Setup
            var assembler = new FailureMechanismResultAssembler();

            // Call
            void Call() => assembler.CalculateFailureMechanismBoundariesBoi1B2(new[]
            {
                new ResultWithProfileAndSectionProbabilities(new Probability(0.0), new Probability(0.0)),
                new ResultWithProfileAndSectionProbabilities(Probability.Undefined, Probability.Undefined),
                new ResultWithProfileAndSectionProbabilities(new Probability(0.0), new Probability(0.0))
            }, false);

            // Assert
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call, new[]
            {
                new AssemblyErrorMessage("failureMechanismSectionAssemblyResults", EAssemblyErrors.EncounteredOneOrMoreSectionsWithoutResult)
            });
        }

        [Test]
        public void CalculateFailureMechanismBoundariesBoi1B21_AssemblyResultsWithOnlyUndefinedProbabilitiesAndPartialAssemblyTrue_ReturnsExpectedResult()
        {
            // Setup
            var assembler = new FailureMechanismResultAssembler();

            // Call
            BoundaryLimits result = assembler.CalculateFailureMechanismBoundariesBoi1B2(new[]
            {
                new ResultWithProfileAndSectionProbabilities(Probability.Undefined, Probability.Undefined),
                new ResultWithProfileAndSectionProbabilities(Probability.Undefined, Probability.Undefined)
            }, true);

            // Assert
            Assert.AreEqual(0.0, result.LowerLimit);
            Assert.AreEqual(0.0, result.UpperLimit);
        }

        [Test]
        [TestCaseSource(nameof(GetCalculateFailureMechanismBoundariesBoi1B2Cases))]
        public void CalculateFailureMechanismBoundariesBoi1B2_FailureMechanismSectionAssemblyResultsAndPartialAssemblyFalse_ReturnsExpectedResult(
            IEnumerable<ResultWithProfileAndSectionProbabilities> failureMechanismSectionAssemblyResults,
            BoundaryLimits expectedBoundaryLimits)
        {
            // Setup
            var assembler = new FailureMechanismResultAssembler();

            // Call
            BoundaryLimits boundaryLimits = assembler.CalculateFailureMechanismBoundariesBoi1B2(
                failureMechanismSectionAssemblyResults, false);

            // Assert
            Assert.AreEqual(expectedBoundaryLimits.LowerLimit, boundaryLimits.LowerLimit, 1e-10);
            Assert.AreEqual(expectedBoundaryLimits.UpperLimit, boundaryLimits.UpperLimit, 1e-10);
        }

        [Test]
        [TestCaseSource(nameof(GetCalculateFailureMechanismBoundariesBoi1B2Cases))]
        public void CalculateFailureMechanismBoundariesBoi1B2_FailureMechanismSectionAssemblyResultsAndPartialAssemblyTrue_ReturnsExpectedResult(
            IEnumerable<ResultWithProfileAndSectionProbabilities> failureMechanismSectionAssemblyResults,
            BoundaryLimits expectedBoundaryLimits)
        {
            // Setup
            failureMechanismSectionAssemblyResults = failureMechanismSectionAssemblyResults.Concat(new[]
            {
                new ResultWithProfileAndSectionProbabilities(Probability.Undefined, Probability.Undefined)
            });

            var assembler = new FailureMechanismResultAssembler();

            // Call
            BoundaryLimits boundaryLimits = assembler.CalculateFailureMechanismBoundariesBoi1B2(
                failureMechanismSectionAssemblyResults, true);

            // Assert
            Assert.AreEqual(expectedBoundaryLimits.LowerLimit, boundaryLimits.LowerLimit, 1e-10);
            Assert.AreEqual(expectedBoundaryLimits.UpperLimit, boundaryLimits.UpperLimit, 1e-10);
        }

        private static IEnumerable<TestCaseData> GetCalculateFailureMechanismBoundariesBoi1B2Cases()
        {
            yield return new TestCaseData(
                new[]
                {
                    new ResultWithProfileAndSectionProbabilities(new Probability(0.0), new Probability(0.0)),
                    new ResultWithProfileAndSectionProbabilities(new Probability(0.1), new Probability(0.1))
                },
                new BoundaryLimits(new Probability(0.1), new Probability(0.1)));

            yield return new TestCaseData(
                new[]
                {
                    new ResultWithProfileAndSectionProbabilities(new Probability(0.0), new Probability(0.0)),
                    new ResultWithProfileAndSectionProbabilities(new Probability(0.0001), new Probability(0.001)),
                    new ResultWithProfileAndSectionProbabilities(new Probability(0.0005), new Probability(0.005)),
                    new ResultWithProfileAndSectionProbabilities(new Probability(0.0007), new Probability(0.0008))
                },
                new BoundaryLimits(new Probability(0.005), new Probability(0.006790204)));

            yield return new TestCaseData(
                new[]
                {
                    new ResultWithProfileAndSectionProbabilities(new Probability(0.0005), new Probability(0.0005)),
                    new ResultWithProfileAndSectionProbabilities(new Probability(0.00005), new Probability(0.00005))
                },
                new BoundaryLimits(new Probability(0.0005), new Probability(0.0005499749)));

            yield return new TestCaseData(
                new[]
                {
                    new ResultWithProfileAndSectionProbabilities(new Probability(1.0 / 1234.0), new Probability(1.0 / 23.0)),
                    new ResultWithProfileAndSectionProbabilities(new Probability(1.0 / 1820.0), new Probability(1.0 / 781.0))
                },
                new BoundaryLimits(new Probability(0.04347826086), new Probability(0.0447030006)));

            yield return new TestCaseData(
                new[]
                {
                    new ResultWithProfileAndSectionProbabilities(new Probability(0.0), new Probability(0.0)),
                    new ResultWithProfileAndSectionProbabilities(new Probability(0.0), new Probability(0.0))
                },
                new BoundaryLimits(new Probability(0.0), new Probability(0.0)));
        }

        #endregion
    }
}