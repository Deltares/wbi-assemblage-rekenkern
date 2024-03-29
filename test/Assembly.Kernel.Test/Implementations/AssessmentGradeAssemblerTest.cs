﻿// Copyright (C) Stichting Deltares and State of the Netherlands 2023. All rights reserved.
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
using Assembly.Kernel.Model.Categories;
using NUnit.Framework;

namespace Assembly.Kernel.Test.Implementations
{
    [TestFixture]
    public class AssessmentGradeAssemblerTest
    {
        [Test]
        public void Constructor_ExpectedValues()
        {
            // Call
            var assembler = new AssessmentGradeAssembler();

            // Assert
            Assert.IsInstanceOf<IAssessmentGradeAssembler>(assembler);
        }

        [Test]
        public void CalculateAssessmentSectionFailureProbabilityBoi2A1_FailureMechanismProbabilitiesNull_ThrowsArgumentNullException()
        {
            // Setup
            var assembler = new AssessmentGradeAssembler();

            // Call
            void Call() => assembler.CalculateAssessmentSectionFailureProbabilityBoi2A1(null, false);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(Call);
            Assert.AreEqual("failureMechanismProbabilities", exception.ParamName);
        }

        [Test]
        public void CalculateAssessmentSectionFailureProbabilityBoi2A1_FailureMechanismProbabilitiesEmpty_ThrowsAssemblyException()
        {
            // Setup
            var assembler = new AssessmentGradeAssembler();

            // Call
            void Call() => assembler.CalculateAssessmentSectionFailureProbabilityBoi2A1(Enumerable.Empty<Probability>(), false);

            // Assert
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call, new[]
            {
                new AssemblyErrorMessage("failureMechanismProbabilities", EAssemblyErrors.EmptyResultsList)
            });
        }

        [Test]
        public void CalculateAssessmentSectionFailureProbabilityBoi2A1_PartialAssemblyFalseAndFailureMechanismProbabilitiesUndefined_ThrowsAssemblyException()
        {
            // Setup
            var assembler = new AssessmentGradeAssembler();

            // Call
            void Call() => assembler.CalculateAssessmentSectionFailureProbabilityBoi2A1(new[]
            {
                Probability.Undefined
            }, false);

            // Assert
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call, new[]
            {
                new AssemblyErrorMessage("failureMechanismProbabilities", EAssemblyErrors.UndefinedProbability)
            });
        }

        [Test]
        public void CalculateAssessmentSectionFailureProbabilityBoi2A1_PartialAssemblyTrueAndFailureMechanismProbabilitiesUndefined_ThrowsAssemblyException()
        {
            // Setup
            var assembler = new AssessmentGradeAssembler();

            // Call
            void Call() => assembler.CalculateAssessmentSectionFailureProbabilityBoi2A1(new[]
            {
                Probability.Undefined
            }, true);

            // Assert
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call, new[]
            {
                new AssemblyErrorMessage("failureMechanismProbabilities", EAssemblyErrors.EmptyResultsList)
            });
        }

        [Test]
        [TestCaseSource(nameof(GetBoi2A1Cases))]
        public void CalculateAssessmentSectionFailureProbabilityBoi2A1_WithFailureMechanismProbabilities_ReturnsExpectedResult(
            bool partialAssembly, Probability probability1, Probability probability2, Probability expectedProbability)
        {
            // Setup
            var assembler = new AssessmentGradeAssembler();

            // Call
            Probability actualProbability = assembler.CalculateAssessmentSectionFailureProbabilityBoi2A1(new[]
            {
                probability1,
                probability2
            }, partialAssembly);

            // Assert
            Assert.AreEqual(expectedProbability, actualProbability, 1e-6);
        }

        [Test]
        public void CalculateAssessmentSectionFailureProbabilityBoi2A2_UncorrelatedFailureMechanismProbabilitiesNull_ThrowsArgumentNullException()
        {
            // Setup
            var assembler = new AssessmentGradeAssembler();

            // Call
            void Call() => assembler.CalculateAssessmentSectionFailureProbabilityBoi2A2(Enumerable.Empty<Probability>(), null, false);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(Call);
            Assert.AreEqual("uncorrelatedFailureMechanismProbabilities", exception.ParamName);
        }

        [Test]
        public void CalculateAssessmentSectionFailureProbabilityBoi2A2_CorrelatedFailureMechanismProbabilitiesNull_ThrowsArgumentNullException()
        {
            // Setup
            var assembler = new AssessmentGradeAssembler();

            // Call
            void Call() => assembler.CalculateAssessmentSectionFailureProbabilityBoi2A2(null, Enumerable.Empty<Probability>(), false);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(Call);
            Assert.AreEqual("correlatedFailureMechanismProbabilities", exception.ParamName);
        }

        [Test]
        public void CalculateAssessmentSectionFailureProbabilityBoi2A2_CorrelatedFailureMechanismProbabilitiesEmpty_ThrowsAssemblyException()
        {
            // Setup
            var assembler = new AssessmentGradeAssembler();

            // Call
            void Call() => assembler.CalculateAssessmentSectionFailureProbabilityBoi2A2(Enumerable.Empty<Probability>(), new[]
            {
                new Probability(0.0)
            }, false);

            // Assert
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call, new[]
            {
                new AssemblyErrorMessage("correlatedFailureMechanismProbabilities", EAssemblyErrors.EmptyResultsList)
            });
        }

        [Test]
        public void CalculateAssessmentSectionFailureProbabilityBoi2A2_PartialAssemblyFalseAndCorrelatedFailureMechanismProbabilitiesUndefined_ThrowsAssemblyException()
        {
            // Setup
            var assembler = new AssessmentGradeAssembler();

            // Call
            void Call() => assembler.CalculateAssessmentSectionFailureProbabilityBoi2A2(new[]
            {
                Probability.Undefined
            }, new[]
            {
                new Probability(0.0)
            }, false);

            // Assert
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call, new[]
            {
                new AssemblyErrorMessage("correlatedFailureMechanismProbabilities", EAssemblyErrors.UndefinedProbability)
            });
        }

        [Test]
        public void CalculateAssessmentSectionFailureProbabilityBoi2A2_PartialAssemblyFalseAndUncorrelatedFailureMechanismProbabilitiesUndefined_ThrowsAssemblyException()
        {
            // Setup
            var assembler = new AssessmentGradeAssembler();

            // Call
            void Call() => assembler.CalculateAssessmentSectionFailureProbabilityBoi2A2(new[]
            {
                new Probability(0.0)
            }, new[]
            {
                Probability.Undefined
            }, false);

            // Assert
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call, new[]
            {
                new AssemblyErrorMessage("uncorrelatedFailureMechanismProbabilities", EAssemblyErrors.UndefinedProbability)
            });
        }

        [Test]
        public void CalculateAssessmentSectionFailureProbabilityBoi2A2_PartialAssemblyTrueAndCorrelatedFailureMechanismProbabilitiesUndefined_ThrowsAssemblyException()
        {
            // Setup
            var assembler = new AssessmentGradeAssembler();

            // Call
            void Call() => assembler.CalculateAssessmentSectionFailureProbabilityBoi2A2(new[]
            {
                Probability.Undefined
            }, new[]
            {
                new Probability(0.0)
            }, true);

            // Assert
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call, new[]
            {
                new AssemblyErrorMessage("correlatedFailureMechanismProbabilities", EAssemblyErrors.EmptyResultsList)
            });
        }

        [Test]
        [TestCaseSource(nameof(GetBoi2A2Cases))]
        public void CalculateAssessmentSectionFailureProbabilityBoi2A2_WithFailureMechanismProbabilities_ReturnsExpectedResult(
            bool partialAssembly, IEnumerable<Probability> correlatedFailureMechanismProbabilities,
            IEnumerable<Probability> uncorrelatedFailureMechanismProbabilities, Probability expectedProbability)
        {
            // Setup
            var assembler = new AssessmentGradeAssembler();

            // Call
            Probability actualProbability = assembler.CalculateAssessmentSectionFailureProbabilityBoi2A2(
                correlatedFailureMechanismProbabilities, uncorrelatedFailureMechanismProbabilities, partialAssembly);

            // Assert
            Assert.AreEqual(expectedProbability, actualProbability, 1e-6);
        }

        [Test]
        public void DetermineAssessmentGradeBoi2B1_CategoriesNull_ThrowsArgumentNullException()
        {
            // Setup
            var assembler = new AssessmentGradeAssembler();

            // Call
            void Call() => assembler.DetermineAssessmentGradeBoi2B1(new Probability(0.0), null);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(Call);
            Assert.AreEqual("categories", exception.ParamName);
        }

        [Test]
        public void DetermineAssessmentGradeBoi2B1_ProbabilityUndefined_ThrowsAssemblyException()
        {
            // Setup
            var assembler = new AssessmentGradeAssembler();

            // Call
            void Call() => assembler.DetermineAssessmentGradeBoi2B1(Probability.Undefined,
                                                                    new CategoriesList<AssessmentSectionCategory>(new[]
                                                                    {
                                                                        new AssessmentSectionCategory(EAssessmentGrade.APlus, new Probability(0.0), new Probability(0.1)),
                                                                        new AssessmentSectionCategory(EAssessmentGrade.A, new Probability(0.1), new Probability(1.0))
                                                                    }));

            // Assert
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call, new[]
            {
                new AssemblyErrorMessage("failureProbability", EAssemblyErrors.UndefinedProbability)
            });
        }

        [Test]
        [TestCase(0.3582, EAssessmentGrade.C)]
        [TestCase(0.000549975, EAssessmentGrade.APlus)]
        public void DetermineAssessmentGradeBoi2B1_WithValidData_ReturnsExpectedResult
            (double failureProbability, EAssessmentGrade expectedAssessmentGrade)
        {
            // Setup
            var categories = new CategoriesList<AssessmentSectionCategory>(new[]
            {
                new AssessmentSectionCategory(EAssessmentGrade.APlus, new Probability(0), new Probability(0.1)),
                new AssessmentSectionCategory(EAssessmentGrade.A, new Probability(0.1), new Probability(0.2)),
                new AssessmentSectionCategory(EAssessmentGrade.B, new Probability(0.2), new Probability(0.3)),
                new AssessmentSectionCategory(EAssessmentGrade.C, new Probability(0.3), new Probability(0.4)),
                new AssessmentSectionCategory(EAssessmentGrade.D, new Probability(0.4), new Probability(1))
            });

            var assembler = new AssessmentGradeAssembler();

            // Call
            EAssessmentGrade assessmentGrade = assembler.DetermineAssessmentGradeBoi2B1(new Probability(failureProbability), categories);

            // Assert
            Assert.AreEqual(expectedAssessmentGrade, assessmentGrade);
        }

        private static IEnumerable<TestCaseData> GetBoi2A1Cases()
        {
            yield return new TestCaseData(false, new Probability(0.0), new Probability(0.1), new Probability(0.1));
            yield return new TestCaseData(false, new Probability(0.0005), new Probability(0.00005), new Probability(0.000549975));
            yield return new TestCaseData(true, new Probability(0.00003), Probability.Undefined, new Probability(0.00003));
        }

        private static IEnumerable<TestCaseData> GetBoi2A2Cases()
        {
            yield return new TestCaseData(false,
                                          new[]
                                          {
                                              new Probability(0.0001),
                                              new Probability(0.1)
                                          },
                                          Enumerable.Empty<Probability>(),
                                          new Probability(0.1));

            yield return new TestCaseData(false,
                                          new[]
                                          {
                                              new Probability(0.0005),
                                              new Probability(0.00005)
                                          },
                                          new[]
                                          {
                                              new Probability(0.00034),
                                              new Probability(0.000034)
                                          },
                                          new Probability(0.0008738014));

            yield return new TestCaseData(true,
                                          new[]
                                          {
                                              new Probability(0.00003),
                                              Probability.Undefined
                                          },
                                          new[]
                                          {
                                              new Probability(0.0617),
                                              Probability.Undefined
                                          },
                                          new Probability(0.0617281489));
        }
    }
}