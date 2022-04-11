#region Copyright (C) Rijkswaterstaat 2022. All rights reserved.

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

using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Implementations;
using Assembly.Kernel.Interfaces;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.Categories;
using NUnit.Framework;

namespace Assembly.Kernel.Tests.Implementations
{
    [TestFixture]
    public class AssessmentResultsTranslatorTest
    {
        private IAssessmentResultsTranslator translator;

        [SetUp]
        public void Init()
        {
            translator = new AssessmentResultsTranslator();
        }

        [TestCase(0.1, 0.2, true, 0.2)]
        [TestCase(0.2, 0.1, true, 0.1)]
        [TestCase(0.2, 0.1, false, 0.2)]
        [TestCase(double.NaN, 0.1, false, double.NaN)]
        [TestCase(0.2, double.NaN, false, 0.2)]
        [TestCase(double.NaN, 0.2, true, 0.2)]
        public void Boi0A1DetermineRepresentativeProbability(
            double initialMechanismProbabilityValue,
            double refinedProbabilityValue,
            bool refinementNecessary,
            double expectedProbabilityValue
        )
        {
            var calculatedProbability = translator.DetermineRepresentativeProbabilityBoi0A1(
                refinementNecessary,
                new Probability(initialMechanismProbabilityValue),
                new Probability(refinedProbabilityValue));

            if (double.IsNaN(expectedProbabilityValue))
            {
                Assert.IsFalse(calculatedProbability.IsDefined);
            }
            else
            {
                Assert.AreEqual(expectedProbabilityValue, calculatedProbability);
            }
        }

        [TestCase(0.1, 0.2, 0.3, 0.4, true, 0.3, 0.4)]
        [TestCase(0.3, 0.4, 0.1, 0.2, true, 0.1, 0.2)]
        [TestCase(0.3, 0.4, 0.1, 0.2, false, 0.3, 0.4)]
        [TestCase(double.NaN, double.NaN, 0.1, 0.2 , false, double.NaN, double.NaN)]
        [TestCase(0.3, 0.4, double.NaN, double.NaN, false, 0.3, 0.4)]
        [TestCase(double.NaN, double.NaN, 0.3, 0.4, true, 0.3, 0.4)]
        public void Boi0A2DetermineRepresentativeProbability(
            double initialMechanismProfileProbabilityValue,
            double initialMechanismSectionProbabilityValue,
            double refinedProfileProbabilityValue,
            double refinedSectionProbabilityValue,
            bool refinementNecessary,
            double expectedProfileProbabilityValue,
            double expectedSectionProbabilityValue
        )
        {
            var calculatedProbabilities = translator.DetermineRepresentativeProbabilitiesBoi0A2(
                refinementNecessary,
                new Probability(initialMechanismProfileProbabilityValue),
                new Probability(initialMechanismSectionProbabilityValue),
                new Probability(refinedProfileProbabilityValue),
                new Probability(refinedSectionProbabilityValue));

            if (double.IsNaN(expectedProfileProbabilityValue))
            {
                Assert.IsFalse(calculatedProbabilities.ProbabilitySection.IsDefined);
                Assert.IsFalse(calculatedProbabilities.ProbabilityProfile.IsDefined);
            }
            else
            {
                Assert.AreEqual(expectedProfileProbabilityValue, calculatedProbabilities.ProbabilityProfile);
                Assert.AreEqual(expectedSectionProbabilityValue, calculatedProbabilities.ProbabilitySection);
            }
        }

        [Test]
        public void Boi0A2ChecksProfileProbabilityDoesNotExceedSectionProbabilityInitialMechanism()
        {
            TestHelper.AssertExpectedErrorMessage(() =>
            {
                translator.DetermineRepresentativeProbabilitiesBoi0A2(
                    false,
                    new Probability(0.3),
                    new Probability(0.04),
                    new Probability(0.1),
                    new Probability(0.2));
            }, EAssemblyErrors.ProfileProbabilityGreaterThanSectionProbability);
        }

        [Test]
        public void Boi0A2ChecksProfileProbabilityDoesNotExceedSectionProbabilityRefinedProbability()
        {
            TestHelper.AssertExpectedErrorMessage(() =>
            {
                translator.DetermineRepresentativeProbabilitiesBoi0A2(
                    true,
                    new Probability(0.3),
                    new Probability(0.4),
                    new Probability(0.1),
                    new Probability(0.02));
            }, EAssemblyErrors.ProfileProbabilityGreaterThanSectionProbability);
        }

        [TestCase(0.04,double.NaN)]
        [TestCase(double.NaN, 0.04)]
        public void Boi0A2ChecksIsDefinedInitialMechanism(double profileProbability, double sectionProbability)
        {
            TestHelper.AssertExpectedErrorMessage(() =>
            {
                translator.DetermineRepresentativeProbabilitiesBoi0A2(
                    false,
                    new Probability(profileProbability),
                    new Probability(sectionProbability),
                    new Probability(0.1),
                    new Probability(0.2));
            }, EAssemblyErrors.ProbabilitiesShouldEitherBothBeDefinedOrUndefined);
        }

        [TestCase(0.04, double.NaN)]
        [TestCase(double.NaN, 0.04)]
        public void Boi0A2ChecksIsDefinedRefinedProbability(double profileProbability, double sectionProbability)
        {
            TestHelper.AssertExpectedErrorMessage(() =>
            {
                translator.DetermineRepresentativeProbabilitiesBoi0A2(
                    true,
                    new Probability(0.04),
                    new Probability(0.1),
                    new Probability(profileProbability),
                    new Probability(sectionProbability)
                );
            }, EAssemblyErrors.ProbabilitiesShouldEitherBothBeDefinedOrUndefined);
        }

        [Test]
        public void Boi0A2ChecksIsDefinedRefinedProbability2()
        {
            TestHelper.AssertExpectedErrorMessage(() =>
            {
                translator.DetermineRepresentativeProbabilitiesBoi0A2(
                    true,
                    new Probability(0.04),
                    new Probability(0.1),
                    new Probability(0.2),
                    Probability.Undefined
                );
            }, EAssemblyErrors.ProbabilitiesShouldEitherBothBeDefinedOrUndefined);
        }

        [TestCase(0.01, EInterpretationCategory.III)]
        [TestCase(0.2, EInterpretationCategory.II)]
        [TestCase(0.45, EInterpretationCategory.IMin)]
        [TestCase(0.0, EInterpretationCategory.III)]
        [TestCase(1.0, EInterpretationCategory.IIIMin)]
        public void Boi0B1DeterminesCorrectCategory(double probabilityValue, EInterpretationCategory expectedCategory)
        {
            var categories = new CategoriesList<InterpretationCategory>(
                new []
                {
                    new InterpretationCategory(EInterpretationCategory.III,new Probability(0), new Probability(0.1)),
                    new InterpretationCategory(EInterpretationCategory.II,new Probability(0.1), new Probability(0.2)),
                    new InterpretationCategory(EInterpretationCategory.I,new Probability(0.2), new Probability(0.3)),
                    new InterpretationCategory(EInterpretationCategory.Zero,new Probability(0.3), new Probability(0.4)),
                    new InterpretationCategory(EInterpretationCategory.IMin,new Probability(0.4), new Probability(0.5)),
                    new InterpretationCategory(EInterpretationCategory.IIMin,new Probability(0.5), new Probability(0.6)),
                    new InterpretationCategory(EInterpretationCategory.IIIMin,new Probability(0.6), new Probability(1)),
                });
            var category = translator.DetermineInterpretationCategoryFromFailureMechanismSectionProbabilityBoi0B1(
                new Probability(probabilityValue),
                categories);

            Assert.AreEqual(expectedCategory, category);
        }

        [Test]
        public void Boi0B1HandlesNullCategories()
        {
            TestHelper.AssertExpectedErrorMessage(() =>
            {
                translator.DetermineInterpretationCategoryFromFailureMechanismSectionProbabilityBoi0B1(
                    new Probability(0.3),
                    null);
            }, EAssemblyErrors.ValueMayNotBeNull);
        }

        [TestCase(EAnalysisState.NotRelevant, EInterpretationCategory.NotRelevant)]
        [TestCase(EAnalysisState.NoProbabilityEstimationNecessary, EInterpretationCategory.NotDominant)]
        [TestCase(EAnalysisState.ProbabilityEstimationNecessary, EInterpretationCategory.Dominant)]
        public void Boi0C1TranslatesAnalysisStateToInterpretationCategory(EAnalysisState state,
            EInterpretationCategory expectedCategory)
        {
            var category = translator.DetermineInterpretationCategoryWithoutProbabilityEstimationBoi0C1(state);

            Assert.AreEqual(expectedCategory, category);
        }

        [TestCase((EAnalysisState)(-1))]
        public void Boi0C1HandlesInvalidStates(EAnalysisState state)
        {
            TestHelper.AssertExpectedErrorMessage(
                () => { translator.DetermineInterpretationCategoryWithoutProbabilityEstimationBoi0C1(state); },
                EAssemblyErrors.InvalidEnumValue);
        }

        [TestCase(EInterpretationCategory.NotDominant, 0)]
        [TestCase(EInterpretationCategory.NotRelevant, 0)]
        [TestCase(EInterpretationCategory.Dominant, double.NaN)]
        [TestCase(EInterpretationCategory.NoResult, double.NaN)]
        public void Boi0C2TranslatesToProbabilityCorrectly(EInterpretationCategory category, double expectedProbability)
        {
            var calculatedProbability = translator.TranslateInterpretationCategoryToProbabilityBoi0C2(category);

            if (double.IsNaN(expectedProbability))
            {
                Assert.IsFalse(calculatedProbability.IsDefined);
            }
            else
            {
                Assert.AreEqual(expectedProbability, calculatedProbability);
            }
        }

        [TestCase(EInterpretationCategory.III)]
        [TestCase(EInterpretationCategory.II)]
        [TestCase(EInterpretationCategory.I)]
        [TestCase(EInterpretationCategory.Zero)]
        [TestCase(EInterpretationCategory.IMin)]
        [TestCase(EInterpretationCategory.IIMin)]
        [TestCase(EInterpretationCategory.IIIMin)]
        [TestCase((EInterpretationCategory) (-1))]
        public void Boi0C2HandlesInvalidCategoryValues(EInterpretationCategory category)
        {
            TestHelper.AssertExpectedErrorMessage(
                () => { translator.TranslateInterpretationCategoryToProbabilityBoi0C2(category); },
                EAssemblyErrors.InvalidCategoryValue);
        }

        [TestCase(0.01, 1, 0.01)]
        [TestCase(0.01, 2, 0.02)]
        [TestCase(0.02, 1.5, 0.03)]
        [TestCase(0.02, 1000, 1.0)]
        public void Boi0D1TranslatesProfileProbabilitiesToSectionProbabilities(double profileProbabilityValue,
            double lengthEffectFactor, double expectedSectionProbability)
        {
            var profileProbability = new Probability(profileProbabilityValue);
            var sectionProbability =
                translator.CalculateProfileProbabilityToSectionProbabilityBoi0D1(profileProbability, lengthEffectFactor);
            Assert.AreEqual(expectedSectionProbability, sectionProbability);
        }

        [Test]
        public void Boi0D1ThrowsInCaseOfInvalidLengthEffect()
        {
            TestHelper.AssertExpectedErrorMessage(() =>
                {
                    translator.CalculateProfileProbabilityToSectionProbabilityBoi0D1(new Probability(0.1), 0.2);
                }, EAssemblyErrors.LengthEffectFactorOutOfRange);
        }

        [TestCase(0.2, 2.0, 0.1)]
        [TestCase(0.52, 5.2, 0.1)]
        public void Boi0D2TranslatesSectionProbabilitiesToProfileProbabilities(double sectionProbabilityValue,
            double lengthEffectFactor, double expectedProfileProbability)
        {
            var sectionProbability = new Probability(sectionProbabilityValue);
            var profileProbability =
                translator.CalculateSectionProbabilityToProfileProbabilityBoi0D2(sectionProbability, lengthEffectFactor);
            Assert.AreEqual(expectedProfileProbability, profileProbability);
        }

        [Test]
        public void Boi0D2ThrowsInCaseOfInvalidLengthEffect()
        {
            TestHelper.AssertExpectedErrorMessage(() =>
            {
                translator.CalculateSectionProbabilityToProfileProbabilityBoi0D2(new Probability(0.1), 0.2);
            }, EAssemblyErrors.LengthEffectFactorOutOfRange);
        }
    }
}
