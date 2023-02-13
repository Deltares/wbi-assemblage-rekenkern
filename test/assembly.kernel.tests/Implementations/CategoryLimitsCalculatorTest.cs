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

using System.Collections.Generic;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Implementations;
using Assembly.Kernel.Interfaces;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.AssessmentSection;
using Assembly.Kernel.Model.Categories;
using NUnit.Framework;

namespace Assembly.Kernel.Tests.Implementations
{
    [TestFixture]
    public class CategoryLimitsCalculatorTest
    {
        [Test]
        public void Constructor_ExpectedValues()
        {
            // Call
            var calculator = new CategoryLimitsCalculator();

            // Assert
            Assert.IsInstanceOf<ICategoryLimitsCalculator>(calculator);
        }

        [Test]
        public void CalculateInterpretationCategoryLimitsBoi01_AssessmentSectionNull_ThrowsAssemblyException()
        {
            // Setup
            var calculator = new CategoryLimitsCalculator();

            // Call
            void Call() => calculator.CalculateInterpretationCategoryLimitsBoi01(null);

            // Assert
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call, new[]
            {
                new AssemblyErrorMessage("assessmentSection", EAssemblyErrors.ValueMayNotBeNull)
            });
        }

        [Test]
        [TestCaseSource(nameof(GetInterpretationCategoryCases))]
        public void CalculateInterpretationCategoryLimitsBoi01_WithAssessmentSection_ReturnsExpectedCategories(
            AssessmentSection assessmentSection, InterpretationCategory[] expectedCategories)
        {
            // Setup
            var calculator = new CategoryLimitsCalculator();

            // Call
            CategoriesList<InterpretationCategory> categories = calculator.CalculateInterpretationCategoryLimitsBoi01(assessmentSection);

            // Assert
            CollectionAssert.AreEqual(expectedCategories, categories.Categories, new CategoryLimitsEqualityComparer());
        }

        [Test]
        public void CalculateAssessmentSectionCategoryLimitsBoi21_AssessmentSectionNull_ThrowsAssemblyException()
        {
            // Setup
            var calculator = new CategoryLimitsCalculator();

            // Call
            void Call() => calculator.CalculateAssessmentSectionCategoryLimitsBoi21(null);

            // Assert
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call, new[]
            {
                new AssemblyErrorMessage("assessmentSection", EAssemblyErrors.ValueMayNotBeNull)
            });
        }

        [Test]
        [TestCaseSource(nameof(GetAssessmentSectionCategoryCases))]
        public void CalculateAssessmentSectionCategoryLimitsBoi21_WithAssessmentSection_ReturnsExpectedCategories(
            AssessmentSection assessmentSection, AssessmentSectionCategory[] expectedCategories)
        {
            // Setup
            var calculator = new CategoryLimitsCalculator();

            // Call
            CategoriesList<AssessmentSectionCategory> categories = calculator.CalculateAssessmentSectionCategoryLimitsBoi21(assessmentSection);

            // Assert
            CollectionAssert.AreEqual(expectedCategories, categories.Categories, new CategoryLimitsEqualityComparer());
        }

        private static IEnumerable<TestCaseData> GetInterpretationCategoryCases()
        {
            yield return new TestCaseData(
                new AssessmentSection((Probability) 0.0003, (Probability) 0.034),
                new[]
                {
                    new InterpretationCategory(EInterpretationCategory.III, (Probability) 0.0, (Probability) 0.0000003),
                    new InterpretationCategory(EInterpretationCategory.II, (Probability) 0.0000003, (Probability) 0.000003),
                    new InterpretationCategory(EInterpretationCategory.I, (Probability) 0.000003, (Probability) 0.00003),
                    new InterpretationCategory(EInterpretationCategory.Zero, (Probability) 0.00003, (Probability) 0.0003),
                    new InterpretationCategory(EInterpretationCategory.IMin, (Probability) 0.0003, (Probability) 0.034),
                    new InterpretationCategory(EInterpretationCategory.IIMin, (Probability) 0.034, (Probability) 0.34),
                    new InterpretationCategory(EInterpretationCategory.IIIMin, (Probability) 0.34, (Probability) 1.0)
                });

            yield return new TestCaseData(
                new AssessmentSection((Probability) 0.001, (Probability) 0.5),
                new[]
                {
                    new InterpretationCategory(EInterpretationCategory.III, (Probability) 0.0, (Probability) 0.000001),
                    new InterpretationCategory(EInterpretationCategory.II, (Probability) 0.000001, (Probability) 0.00001),
                    new InterpretationCategory(EInterpretationCategory.I, (Probability) 0.00001, (Probability) 0.0001),
                    new InterpretationCategory(EInterpretationCategory.Zero, (Probability) 0.0001, (Probability) 0.001),
                    new InterpretationCategory(EInterpretationCategory.IMin, (Probability) 0.001, (Probability) 0.5),
                    new InterpretationCategory(EInterpretationCategory.IIMin, (Probability) 0.5, (Probability) 1.0),
                    new InterpretationCategory(EInterpretationCategory.IIIMin, (Probability) 1.0, (Probability) 1.0)
                });
        }

        private static IEnumerable<TestCaseData> GetAssessmentSectionCategoryCases()
        {
            yield return new TestCaseData(
                new AssessmentSection((Probability) 0.003, (Probability) 0.03),
                new[]
                {
                    new AssessmentSectionCategory(EAssessmentGrade.APlus, (Probability) 0.0, (Probability) 0.0001),
                    new AssessmentSectionCategory(EAssessmentGrade.A, (Probability) 0.0001, (Probability) 0.003),
                    new AssessmentSectionCategory(EAssessmentGrade.B, (Probability) 0.003, (Probability) 0.03),
                    new AssessmentSectionCategory(EAssessmentGrade.C, (Probability) 0.03, (Probability) 0.9),
                    new AssessmentSectionCategory(EAssessmentGrade.D, (Probability) 0.9, (Probability) 1.0)
                });

            yield return new TestCaseData(
                new AssessmentSection((Probability) 0.003, (Probability) 0.034),
                new[]
                {
                    new AssessmentSectionCategory(EAssessmentGrade.APlus, (Probability) 0.0, (Probability) 0.0001),
                    new AssessmentSectionCategory(EAssessmentGrade.A, (Probability) 0.0001, (Probability) 0.003),
                    new AssessmentSectionCategory(EAssessmentGrade.B, (Probability) 0.003, (Probability) 0.034),
                    new AssessmentSectionCategory(EAssessmentGrade.C, (Probability) 0.034, (Probability) 1.0),
                    new AssessmentSectionCategory(EAssessmentGrade.D, (Probability) 1.0, (Probability) 1.0)
                });
        }
    }
}