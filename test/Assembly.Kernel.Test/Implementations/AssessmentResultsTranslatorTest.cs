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
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Implementations;
using Assembly.Kernel.Interfaces;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.Categories;
using Assembly.Kernel.Model.FailureMechanismSections;
using NUnit.Framework;

namespace Assembly.Kernel.Test.Implementations
{
    [TestFixture]
    public class AssessmentResultsTranslatorTest
    {
        [Test]
        public void Constructor_ExpectedValues()
        {
            // Call
            var translator = new AssessmentResultsTranslator();

            // Assert
            Assert.IsInstanceOf<IAssessmentResultsTranslator>(translator);
        }

        [Test]
        [TestCase(true, 0.2)]
        [TestCase(false, 0.1)]
        public void DetermineRepresentativeProbabilityBoi0A1_Always_ReturnsExpectedProbability(
            bool refinementNecessary,
            double expectedProbability)
        {
            // Setup
            var translator = new AssessmentResultsTranslator();

            // Call
            Probability probability = translator.DetermineRepresentativeProbabilityBoi0A1(
                refinementNecessary, new Probability(0.1), new Probability(0.2));

            // Assert
            Assert.AreEqual(expectedProbability, probability, 1e-6);
        }

        [Test]
        [TestCase(false, 0.1, 0.2)]
        [TestCase(true, 0.3, 0.4)]
        public void DetermineRepresentativeProbabilitiesBoi0A2_Always_ReturnsExpectedProbabilities(
            bool refinementNecessary,
            double expectedProbabilityProfile,
            double expectedProbabilitySection)
        {
            // Setup
            var translator = new AssessmentResultsTranslator();

            // Call
            ResultWithProfileAndSectionProbabilities result = translator.DetermineRepresentativeProbabilitiesBoi0A2(
                refinementNecessary, new Probability(0.1), new Probability(0.2), new Probability(0.3), new Probability(0.4));

            // Assert
            Assert.AreEqual(expectedProbabilityProfile, result.ProbabilityProfile, 1e-6);
            Assert.AreEqual(expectedProbabilitySection, result.ProbabilitySection, 1e-6);
        }

        [Test]
        public void DetermineInterpretationCategoryFromFailureMechanismSectionProbabilityBoi0B1_CategoriesNull_ThrowsArgumentNullException()
        {
            // Setup
            var translator = new AssessmentResultsTranslator();

            // Call
            void Call() => translator.DetermineInterpretationCategoryFromFailureMechanismSectionProbabilityBoi0B1(
                new Probability(0.1), null);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(Call);
            Assert.AreEqual("categories", exception.ParamName);
        }

        [Test]
        [TestCase(EAnalysisState.ProbabilityEstimated)]
        [TestCase(99)]
        public void DetermineInterpretationCategoryWithoutProbabilityEstimationBoi0C1_InvalidValue_ThrowsAssemblyException(
            EAnalysisState analysisState)
        {
            // Setup
            var translator = new AssessmentResultsTranslator();

            // Call
            void Call() => translator.DetermineInterpretationCategoryWithoutProbabilityEstimationBoi0C1(analysisState);

            // Assert
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call, new[]
            {
                new AssemblyErrorMessage("analysisState", EAssemblyErrors.InvalidEnumValue)
            });
        }

        [Test]
        [TestCase(EAnalysisState.NotRelevant, EInterpretationCategory.NotRelevant)]
        [TestCase(EAnalysisState.NoProbabilityEstimationNecessary, EInterpretationCategory.NotDominant)]
        [TestCase(EAnalysisState.ProbabilityEstimationNecessary, EInterpretationCategory.Dominant)]
        public void DetermineInterpretationCategoryWithoutProbabilityEstimationBoi0C1_ValidAnalysisState_ReturnsExpectedInterpretationCategory(
            EAnalysisState analysisState, EInterpretationCategory expectedInterpretationCategory)
        {
            // Setup
            var translator = new AssessmentResultsTranslator();

            // Call
            EInterpretationCategory category = translator.DetermineInterpretationCategoryWithoutProbabilityEstimationBoi0C1(analysisState);

            // Assert
            Assert.AreEqual(expectedInterpretationCategory, category);
        }

        [Test]
        [TestCase(EInterpretationCategory.Zero)]
        [TestCase(EInterpretationCategory.III)]
        [TestCase(EInterpretationCategory.II)]
        [TestCase(EInterpretationCategory.I)]
        [TestCase(EInterpretationCategory.Zero)]
        [TestCase(EInterpretationCategory.IMin)]
        [TestCase(EInterpretationCategory.IIMin)]
        [TestCase(EInterpretationCategory.IIIMin)]
        public void TranslateInterpretationCategoryToProbabilityBoi0C2_InvalidValue_ThrowsAssemblyException(
            EInterpretationCategory interpretationCategory)
        {
            // Setup
            var translator = new AssessmentResultsTranslator();

            // Call
            void Call() => translator.TranslateInterpretationCategoryToProbabilityBoi0C2(interpretationCategory);

            // Assert
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call, new[]
            {
                new AssemblyErrorMessage("category", EAssemblyErrors.InvalidCategoryValue)
            });
        }

        [Test]
        public void CalculateProfileProbabilityToSectionProbabilityBoi0D1_InvalidLengthEffectFactory_ThrowsAssemblyException()
        {
            // Setup
            var translator = new AssessmentResultsTranslator();

            // Call
            void Call() => translator.CalculateProfileProbabilityToSectionProbabilityBoi0D1(new Probability(0.1), 0.99);

            // Assert
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call, new[]
            {
                new AssemblyErrorMessage("lengthEffectFactor", EAssemblyErrors.LengthEffectFactorOutOfRange)
            });
        }

        [Test]
        [TestCase(0.01, 1, 0.01)]
        [TestCase(0.01, 2, 0.02)]
        [TestCase(0.02, 1.5, 0.03)]
        [TestCase(0.02, 1000, 1.0)]
        public void CalculateProfileProbabilityToSectionProbabilityBoi0D1_ValidData_ReturnsExpectedProbability(
            double profileProbability, double lengthEffectFactor, double expectedSectionProbability)
        {
            // Setup
            var translator = new AssessmentResultsTranslator();

            // Call
            Probability probability = translator.CalculateProfileProbabilityToSectionProbabilityBoi0D1(
                new Probability(profileProbability), lengthEffectFactor);

            // Assert
            Assert.AreEqual(expectedSectionProbability, probability, 1e-6);
        }

        [Test]
        public void CalculateSectionProbabilityToProfileProbabilityBoi0D2_InvalidLengthEffectFactory_ThrowsAssemblyException()
        {
            // Setup
            var translator = new AssessmentResultsTranslator();

            // Call
            void Call() => translator.CalculateSectionProbabilityToProfileProbabilityBoi0D2(new Probability(0.1), 0.99);

            // Assert
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call, new[]
            {
                new AssemblyErrorMessage("lengthEffectFactor", EAssemblyErrors.LengthEffectFactorOutOfRange)
            });
        }

        [Test]
        [TestCase(0.2, 2.0, 0.1)]
        [TestCase(0.52, 5.2, 0.1)]
        public void CalculateSectionProbabilityToProfileProbabilityBoi0D2_ValidData_ReturnsExpectedProbability(
            double sectionProbabilityValue, double lengthEffectFactor, double expectedProfileProbability)
        {
            // Setup
            var translator = new AssessmentResultsTranslator();

            // Call
            Probability profileProbability = translator.CalculateSectionProbabilityToProfileProbabilityBoi0D2(
                new Probability(sectionProbabilityValue), lengthEffectFactor);

            // Assert
            Assert.AreEqual(expectedProfileProbability, profileProbability, 1e-6);
        }

        [TestCase(0.01, EInterpretationCategory.III)]
        [TestCase(0.2, EInterpretationCategory.II)]
        [TestCase(0.45, EInterpretationCategory.IMin)]
        [TestCase(0.0, EInterpretationCategory.III)]
        [TestCase(1.0, EInterpretationCategory.IIIMin)]
        public void DetermineInterpretationCategoryFromFailureMechanismSectionProbabilityBoi0B1_WithValidData_ReturnsExpectedCategory(
            double probabilityValue, EInterpretationCategory expectedCategory)
        {
            // Setup
            var categories = new CategoriesList<InterpretationCategory>(new[]
            {
                new InterpretationCategory(EInterpretationCategory.III, new Probability(0), new Probability(0.1)),
                new InterpretationCategory(EInterpretationCategory.II, new Probability(0.1), new Probability(0.2)),
                new InterpretationCategory(EInterpretationCategory.I, new Probability(0.2), new Probability(0.3)),
                new InterpretationCategory(EInterpretationCategory.Zero, new Probability(0.3), new Probability(0.4)),
                new InterpretationCategory(EInterpretationCategory.IMin, new Probability(0.4), new Probability(0.5)),
                new InterpretationCategory(EInterpretationCategory.IIMin, new Probability(0.5), new Probability(0.6)),
                new InterpretationCategory(EInterpretationCategory.IIIMin, new Probability(0.6), new Probability(1))
            });

            var translator = new AssessmentResultsTranslator();

            // Call
            EInterpretationCategory category = translator.DetermineInterpretationCategoryFromFailureMechanismSectionProbabilityBoi0B1(
                new Probability(probabilityValue), categories);

            // Assert
            Assert.AreEqual(expectedCategory, category);
        }

        [TestCase(EInterpretationCategory.NotDominant, 0)]
        [TestCase(EInterpretationCategory.NotRelevant, 0)]
        [TestCase(EInterpretationCategory.Dominant, double.NaN)]
        [TestCase(EInterpretationCategory.NoResult, double.NaN)]
        public void TranslateInterpretationCategoryToProbabilityBoi0C2_ValidValue_ReturnsExpectedProbability(
            EInterpretationCategory category, double expectedProbability)
        {
            // Setup
            var translator = new AssessmentResultsTranslator();

            // Call
            Probability probability = translator.TranslateInterpretationCategoryToProbabilityBoi0C2(category);

            // Assert
            Assert.AreEqual(expectedProbability, probability, 1e-6);
        }
    }
}