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
            void Call() => assembler.CalculateAssessmentSectionFailureProbabilityBoi2A1(Array.Empty<Probability>(), false);

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
                new AssemblyErrorMessage("failureMechanismProbability", EAssemblyErrors.UndefinedProbability)
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
        [TestCaseSource(nameof(GetFailureMechanismProbabilities))]
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
        public void DetermineAssessmentGradeBoi2B1_dCategoriesNull_ThrowsArgumentNullException()
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

        private static IEnumerable<TestCaseData> GetFailureMechanismProbabilities()
        {
            yield return new TestCaseData(false, new Probability(0.0), new Probability(0.1), new Probability(0.1));
            yield return new TestCaseData(false, new Probability(0.0005), new Probability(0.00005), new Probability(0.000549975));
            yield return new TestCaseData(true, new Probability(0.00003), Probability.Undefined, new Probability(0.00003));
        }
    }
}