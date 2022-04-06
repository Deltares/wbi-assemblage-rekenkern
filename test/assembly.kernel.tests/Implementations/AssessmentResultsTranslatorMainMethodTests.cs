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

using System;
using System.Linq;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Implementations;
using Assembly.Kernel.Interfaces;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.Categories;
using NUnit.Framework;

namespace Assembly.Kernel.Tests.Implementations
{
    public class AssessmentResultsTranslatorMainMethodTests
    {
        private IAssessmentResultsTranslator translator;

        [SetUp]
        public void Init()
        {
            translator = new AssessmentResultsTranslator();
        }

        [Test]
        public void NotRelevantTest()
        {
            var categories = new CategoriesList<InterpretationCategory>(
                new[]
                {
                    new InterpretationCategory(EInterpretationCategory.III, (Probability)0,(Probability) 0.02),
                    new InterpretationCategory(EInterpretationCategory.II, (Probability) 0.02,(Probability) 0.04),
                    new InterpretationCategory(EInterpretationCategory.I, (Probability) 0.04,(Probability) 1.0)
                });

            var result = translator.TranslateAssessmentResultWithLengthEffectAggregatedMethod(
                ESectionInitialMechanismProbabilitySpecification.NotRelevant,
                Probability.Undefined,
                Probability.Undefined,
                ERefinementStatus.NotNecessary,
                Probability.Undefined,
                Probability.Undefined,
                categories);

            Assert.IsNotNull(result);
            Assert.AreEqual(EInterpretationCategory.NotRelevant, result.InterpretationCategory);
            Assert.AreEqual(0.0, result.ProbabilityProfile);
            Assert.AreEqual(0.0, result.ProbabilitySection);
            Assert.AreEqual(1.0, result.LengthEffectFactor);
        }

        [Test]
        public void NotRelevantIgnoresOtherInputTest()
        {
            var categories = new CategoriesList<InterpretationCategory>(
                new[]
                {
                    new InterpretationCategory(EInterpretationCategory.III, (Probability)0,(Probability) 0.02),
                    new InterpretationCategory(EInterpretationCategory.II, (Probability) 0.02,(Probability) 0.04),
                    new InterpretationCategory(EInterpretationCategory.I, (Probability) 0.04,(Probability) 1.0)
                });

            var result = translator.TranslateAssessmentResultWithLengthEffectAggregatedMethod(
                ESectionInitialMechanismProbabilitySpecification.NotRelevant,
                new Probability(0.1),
                new Probability(0.01),
                ERefinementStatus.Necessary,
                new Probability(0.1),
                Probability.Undefined,
                categories);

            Assert.IsNotNull(result);
            Assert.AreEqual(EInterpretationCategory.NotRelevant, result.InterpretationCategory);
            Assert.AreEqual(0.0, result.ProbabilityProfile);
            Assert.AreEqual(0.0, result.ProbabilitySection);
            Assert.AreEqual(1.0, result.LengthEffectFactor);
        }

        [Test]
        public void InitialMechanismPrevailsTest()
        {
            var categories = new CategoriesList<InterpretationCategory>(
                new[]
                {
                    new InterpretationCategory(EInterpretationCategory.III, (Probability)0,(Probability) 0.02),
                    new InterpretationCategory(EInterpretationCategory.II, (Probability) 0.02,(Probability) 0.04),
                    new InterpretationCategory(EInterpretationCategory.I, (Probability) 0.04,(Probability) 1.0)
                });

            Probability probabilityProfile = new Probability(0.01);
            Probability probabilitySection = new Probability(0.02);
            var result = translator.TranslateAssessmentResultWithLengthEffectAggregatedMethod(
                ESectionInitialMechanismProbabilitySpecification.RelevantWithProbabilitySpecification, 
                probabilityProfile, 
                probabilitySection,
                ERefinementStatus.NotNecessary, 
                Probability.Undefined, 
                Probability.Undefined, 
                categories);

            Assert.IsNotNull(result);
            Assert.AreEqual(EInterpretationCategory.III, result.InterpretationCategory);
            Assert.AreEqual(0.01, result.ProbabilityProfile);
            Assert.AreEqual(0.02, result.ProbabilitySection);
            Assert.AreEqual(2.0, result.LengthEffectFactor);
        }

        [Test]
        [TestCase(0.01, 0.02)]
        [TestCase(double.NaN, 0.02)]
        [TestCase(double.NaN, double.NaN)]
        public void RelevantNoProbabilityEstimationTest(double probabilityProfileValue, double probabilitySectionValue)
        {
            var categories = new CategoriesList<InterpretationCategory>(
                new[]
                {
                    new InterpretationCategory(EInterpretationCategory.III, (Probability)0,(Probability) 0.02),
                    new InterpretationCategory(EInterpretationCategory.II, (Probability) 0.02,(Probability) 0.04),
                    new InterpretationCategory(EInterpretationCategory.I, (Probability) 0.04,(Probability) 1.0)
                });

            Probability probabilityProfile = new Probability(probabilityProfileValue);
            Probability probabilitySection = new Probability(probabilitySectionValue);
            var result = translator.TranslateAssessmentResultWithLengthEffectAggregatedMethod(
                ESectionInitialMechanismProbabilitySpecification.RelevantNoProbabilitySpecification,
                probabilityProfile,
                probabilitySection,
                ERefinementStatus.NotNecessary,
                Probability.Undefined,
                Probability.Undefined,
                categories);

            Assert.IsNotNull(result);
            Assert.AreEqual(EInterpretationCategory.NotDominant, result.InterpretationCategory);
            Assert.AreEqual((Probability)0.0, result.ProbabilityProfile);
            Assert.AreEqual((Probability)0.0, result.ProbabilitySection);
            Assert.AreEqual(1.0, result.LengthEffectFactor);
        }

        [Test]
        [TestCase(0.01, 0.02, 0.03, 0.04, EInterpretationCategory.II, 0.03, 0.04)]
        [TestCase(0.02, 0.01, 0.03, 0.04, EInterpretationCategory.II, 0.03, 0.04)]
        [TestCase(double.NaN, 0.01, 0.03, 0.04, EInterpretationCategory.II, 0.03, 0.04)]
        [TestCase(0.5, double.NaN, 0.03, 0.04, EInterpretationCategory.II, 0.03, 0.04)]
        [TestCase(double.NaN, double.NaN, 0.03, 0.04, EInterpretationCategory.II, 0.03, 0.04)]
        public void RefinedProbabilityEstimationTest(double probabilityProfileValue, double probabilitySectionValue,
            double refinedProbabilityProfileValue, double refinedProbabilitySectionValue,
            EInterpretationCategory expectedCategory, double expectedProbabilityProfileValue, double expectedProbabilitySectionValue)
        {
            var categories = new CategoriesList<InterpretationCategory>(
                new[]
                {
                    new InterpretationCategory(EInterpretationCategory.III, (Probability)0,(Probability) 0.02),
                    new InterpretationCategory(EInterpretationCategory.II, (Probability) 0.02,(Probability) 0.04),
                    new InterpretationCategory(EInterpretationCategory.I, (Probability) 0.04,(Probability) 1.0)
                });

            Probability probabilityInitialMechanismProfile = new Probability(probabilityProfileValue);
            Probability probabilityInitialMechanismSection = new Probability(probabilitySectionValue);
            Probability refinedProbabilityProfile = new Probability(refinedProbabilityProfileValue);
            Probability refinedProbabilitySection = new Probability(refinedProbabilitySectionValue);
            var result = translator.TranslateAssessmentResultWithLengthEffectAggregatedMethod(
                ESectionInitialMechanismProbabilitySpecification.RelevantWithProbabilitySpecification,
                probabilityInitialMechanismProfile,
                probabilityInitialMechanismSection,
                ERefinementStatus.Performed,
                refinedProbabilityProfile,
                refinedProbabilitySection,
                categories);

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedCategory, result.InterpretationCategory);
            Assert.AreEqual(expectedProbabilityProfileValue, result.ProbabilityProfile);
            Assert.AreEqual(expectedProbabilitySectionValue, result.ProbabilitySection);
            var expectedN = double.IsNaN(expectedProbabilityProfileValue) | double.IsNaN(expectedProbabilityProfileValue)
                ? 1.0
                : expectedProbabilitySectionValue / expectedProbabilityProfileValue;
            Assert.AreEqual(expectedN, result.LengthEffectFactor);
        }

        [Test]
        [TestCase(0.01, 0.02, 0.03, 0.04)]
        [TestCase(0.02, 0.01, 0.03, 0.04)]
        [TestCase(double.NaN, 0.01, 0.03, 0.04)]
        [TestCase(0.01, double.NaN, 0.03, 0.04)]
        [TestCase(double.NaN, double.NaN, 0.03, 0.04)]
        [TestCase(double.NaN, double.NaN, double.NaN, 0.04)]
        [TestCase(double.NaN, double.NaN, 0.03, double.NaN)]
        [TestCase(double.NaN, double.NaN, double.NaN, double.NaN)]
        public void RefinedProbabilityNecessaryTest(double probabilityProfileValue, double probabilitySectionValue,
            double refinedProbabilityProfileValue, double refinedProbabilitySectionValue)
        {
            var categories = new CategoriesList<InterpretationCategory>(
                new[]
                {
                    new InterpretationCategory(EInterpretationCategory.III, (Probability)0,(Probability) 0.02),
                    new InterpretationCategory(EInterpretationCategory.II, (Probability) 0.02,(Probability) 0.04),
                    new InterpretationCategory(EInterpretationCategory.I, (Probability) 0.04,(Probability) 1.0)
                });

            Probability probabilityInitialMechanismProfile = new Probability(probabilityProfileValue);
            Probability probabilityInitialMechanismSection = new Probability(probabilitySectionValue);
            Probability refinedProbabilityProfile = new Probability(refinedProbabilityProfileValue);
            Probability refinedProbabilitySection = new Probability(refinedProbabilitySectionValue);
            var result = translator.TranslateAssessmentResultWithLengthEffectAggregatedMethod(
                ESectionInitialMechanismProbabilitySpecification.RelevantWithProbabilitySpecification,
                probabilityInitialMechanismProfile,
                probabilityInitialMechanismSection,
                ERefinementStatus.Necessary,
                refinedProbabilityProfile,
                refinedProbabilitySection,
                categories);

            Assert.IsNotNull(result);
            Assert.AreEqual(EInterpretationCategory.Dominant, result.InterpretationCategory);
            Assert.AreEqual(double.NaN, result.ProbabilityProfile);
            Assert.AreEqual(double.NaN, result.ProbabilitySection);
            Assert.AreEqual(1.0, result.LengthEffectFactor);
        }

        [Test]
        public void NotRelevantNoLengthEffectTest()
        {
            var categories = new CategoriesList<InterpretationCategory>(
                new[]
                {
                    new InterpretationCategory(EInterpretationCategory.III, (Probability)0,(Probability) 0.02),
                    new InterpretationCategory(EInterpretationCategory.II, (Probability) 0.02,(Probability) 0.04),
                    new InterpretationCategory(EInterpretationCategory.I, (Probability) 0.04,(Probability) 1.0)
                });

            var result = translator.TranslateAssessmentResultAggregatedMethod(
                ESectionInitialMechanismProbabilitySpecification.NotRelevant,
                Probability.Undefined,
                ERefinementStatus.NotNecessary,
                Probability.Undefined,
                categories);

            Assert.IsNotNull(result);
            Assert.AreEqual(EInterpretationCategory.NotRelevant, result.InterpretationCategory);
            Assert.AreEqual(0.0, result.ProbabilitySection);
        }

        [Test]
        [TestCase(0.02, EInterpretationCategory.III, 0.02)]
        [TestCase(0.02, EInterpretationCategory.III, 0.02)]
        public void InitialMechanismPrevailsNoLengthEffectTest(double probabilitySectionValue, EInterpretationCategory expectedCategory, double expectedProbabilityValues)
        {
            var random = new Random(10);
            var categories = new CategoriesList<InterpretationCategory>(
                new[]
                {
                    new InterpretationCategory(EInterpretationCategory.III, (Probability)0,(Probability) 0.02),
                    new InterpretationCategory(EInterpretationCategory.II, (Probability) 0.02,(Probability) 0.04),
                    new InterpretationCategory(EInterpretationCategory.I, (Probability) 0.04,(Probability) 1.0)
                });

            Probability probabilitySection = new Probability(probabilitySectionValue);
            var result = translator.TranslateAssessmentResultAggregatedMethod(
                ESectionInitialMechanismProbabilitySpecification.RelevantWithProbabilitySpecification,
                probabilitySection,
                ERefinementStatus.NotNecessary,
                (Probability)random.NextDouble(),
                categories);

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedCategory, result.InterpretationCategory);
            Assert.AreEqual(expectedProbabilityValues, result.ProbabilitySection);
        }

        [Test]
        [TestCase(0.01)]
        [TestCase(double.NaN)]
        public void RelevantNoProbabilityEstimationNoLengthEffectTest(double probabilitySectionValue)
        {
            var categories = new CategoriesList<InterpretationCategory>(
                new[]
                {
                    new InterpretationCategory(EInterpretationCategory.III, (Probability)0,(Probability) 0.02),
                    new InterpretationCategory(EInterpretationCategory.II, (Probability) 0.02,(Probability) 0.04),
                    new InterpretationCategory(EInterpretationCategory.I, (Probability) 0.04,(Probability) 1.0)
                });

            Probability probabilitySection = new Probability(probabilitySectionValue);
            var result = translator.TranslateAssessmentResultAggregatedMethod(
                ESectionInitialMechanismProbabilitySpecification.RelevantNoProbabilitySpecification,
                probabilitySection,
                ERefinementStatus.NotNecessary,
                Probability.Undefined,
                categories);

            Assert.IsNotNull(result);
            Assert.AreEqual(EInterpretationCategory.NotDominant, result.InterpretationCategory);
            Assert.AreEqual((Probability)0.0, result.ProbabilitySection);
        }

        [Test]
        [TestCase(0.02, 0.04, EInterpretationCategory.II, 0.04)]
        [TestCase(0.01, 0.04, EInterpretationCategory.II, 0.04)]
        [TestCase(0.02, 0.04, EInterpretationCategory.II, 0.04)]
        public void RefinedProbabilityEstimationNoLengthEffectTest(double probabilitySectionValue, double refinedProbabilitySectionValue,
            EInterpretationCategory expectedCategory, double expectedProbabilityValue)
        {
            var categories = new CategoriesList<InterpretationCategory>(
                new[]
                {
                    new InterpretationCategory(EInterpretationCategory.III, (Probability)0,(Probability) 0.02),
                    new InterpretationCategory(EInterpretationCategory.II, (Probability) 0.02,(Probability) 0.04),
                    new InterpretationCategory(EInterpretationCategory.I, (Probability) 0.04,(Probability) 1.0)
                });

            Probability probabilityInitialMechanismSection = new Probability(probabilitySectionValue);
            Probability refinedProbabilitySection = new Probability(refinedProbabilitySectionValue);
            var result = translator.TranslateAssessmentResultAggregatedMethod(
                ESectionInitialMechanismProbabilitySpecification.RelevantWithProbabilitySpecification,
                probabilityInitialMechanismSection,
                ERefinementStatus.Performed,
                refinedProbabilitySection,
                categories);

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedCategory, result.InterpretationCategory);
            Assert.AreEqual(expectedProbabilityValue, result.ProbabilitySection);
        }

        [Test]
        public void ThrowsOnNullCategoriesTest()
        {
            TestHelper.AssertExpectedErrorMessage(
                () =>
                {
                    var result = translator.TranslateAssessmentResultWithLengthEffectAggregatedMethod(
                        ESectionInitialMechanismProbabilitySpecification.RelevantWithProbabilitySpecification,
                        (Probability) 0.1,
                        (Probability) 0.1,
                        ERefinementStatus.Performed,
                        (Probability) 0.1,
                        (Probability) 0.1,
                        null);
                }, EAssemblyErrors.ValueMayNotBeNull
            );
        }

        [Test]
        public void ThrowsOnLargeProfileProbabilityInitialMechanism()
        {
            TestHelper.AssertExpectedErrorMessage(
                () =>
                {
                    var categories = new CategoriesList<InterpretationCategory>(
                        new[]
                        {
                            new InterpretationCategory(EInterpretationCategory.III, (Probability) 0, (Probability) 0.02),
                            new InterpretationCategory(EInterpretationCategory.II, (Probability) 0.02, (Probability) 0.04),
                            new InterpretationCategory(EInterpretationCategory.I, (Probability) 0.04, (Probability) 1.0)
                        });

                    var result = translator.TranslateAssessmentResultWithLengthEffectAggregatedMethod(
                        ESectionInitialMechanismProbabilitySpecification.RelevantWithProbabilitySpecification,
                        (Probability) 0.1,
                        (Probability) 0.01,
                        ERefinementStatus.NotNecessary,
                        Probability.Undefined,
                        Probability.Undefined,
                        categories);
                }
                , EAssemblyErrors.ProfileProbabilityGreaterThanSectionProbability);
        }

        [Test]
        public void ThrowsOnLargeProfileRefinedProbability()
        {
            TestHelper.AssertExpectedErrorMessage(() =>
            {
                var categories = new CategoriesList<InterpretationCategory>(
                    new[]
                    {
                        new InterpretationCategory(EInterpretationCategory.III, (Probability) 0, (Probability) 0.02),
                        new InterpretationCategory(EInterpretationCategory.II, (Probability) 0.02, (Probability) 0.04),
                        new InterpretationCategory(EInterpretationCategory.I, (Probability) 0.04, (Probability) 1.0)
                    });

                var result = translator.TranslateAssessmentResultWithLengthEffectAggregatedMethod(
                    ESectionInitialMechanismProbabilitySpecification.RelevantWithProbabilitySpecification,
                    Probability.Undefined,
                    Probability.Undefined,
                    ERefinementStatus.Performed,
                    (Probability) 0.1,
                    (Probability) 0.01,
                    categories);
            }, EAssemblyErrors.ProfileProbabilityGreaterThanSectionProbability);
        }

        [Test]
        [TestCase(0.01, double.NaN, EAssemblyErrors.ProbabilitiesShouldEitherBothBeDefinedOrUndefined)]
        [TestCase(double.NaN, 0.01, EAssemblyErrors.ProbabilitiesShouldEitherBothBeDefinedOrUndefined)]
        [TestCase(double.NaN, double.NaN, EAssemblyErrors.ProbabilityMayNotBeUndefined)]
        public void ThrowsInCaseOfMissingProbabilitiesInitialMechanism(double probabilityProfile, double probabilitySection, EAssemblyErrors expectedError)
        {
            TestHelper.AssertExpectedErrorMessage(() =>
            {
                var categories = new CategoriesList<InterpretationCategory>(
                    new[]
                    {
                        new InterpretationCategory(EInterpretationCategory.III, (Probability) 0, (Probability) 0.02),
                        new InterpretationCategory(EInterpretationCategory.II, (Probability) 0.02, (Probability) 0.04),
                        new InterpretationCategory(EInterpretationCategory.I, (Probability) 0.04, (Probability) 1.0)
                    });

                var result = translator.TranslateAssessmentResultWithLengthEffectAggregatedMethod(
                    ESectionInitialMechanismProbabilitySpecification.RelevantWithProbabilitySpecification,
                    new Probability(probabilityProfile),
                    new Probability(probabilitySection),
                    ERefinementStatus.NotNecessary,
                    Probability.Undefined,
                    Probability.Undefined,
                    categories);
            }, expectedError);
        }

        [Test]
        public void ThrowsInCaseOfMissingProbabilitiesInitialMechanism2()
        {
            TestHelper.AssertExpectedErrorMessage(() =>
            {
                var categories = new CategoriesList<InterpretationCategory>(
                    new[]
                    {
                        new InterpretationCategory(EInterpretationCategory.III, (Probability) 0, (Probability) 0.02),
                        new InterpretationCategory(EInterpretationCategory.II, (Probability) 0.02, (Probability) 0.04),
                        new InterpretationCategory(EInterpretationCategory.I, (Probability) 0.04, (Probability) 1.0)
                    });

                var result = translator.TranslateAssessmentResultWithLengthEffectAggregatedMethod(
                    ESectionInitialMechanismProbabilitySpecification.RelevantWithProbabilitySpecification,
                    Probability.Undefined,
                    new Probability(1 / 50.0),
                    ERefinementStatus.NotNecessary,
                    new Probability(1 / 50.0),
                    new Probability(1 / 50.0),
                    categories);
            }, EAssemblyErrors.ProbabilitiesShouldEitherBothBeDefinedOrUndefined);
        }

        [Test]
        [TestCase(0.01,double.NaN, EAssemblyErrors.ProbabilitiesShouldEitherBothBeDefinedOrUndefined)]
        [TestCase(double.NaN, 0.01, EAssemblyErrors.ProbabilitiesShouldEitherBothBeDefinedOrUndefined)]
        [TestCase(double.NaN, double.NaN, EAssemblyErrors.ProbabilityMayNotBeUndefined)]
        public void ThrowsInCaseOfMissingRefinedProbabilities(double probabilityProfile, double probabilitySection, EAssemblyErrors expectedError)
        {
            TestHelper.AssertExpectedErrorMessage(() =>
            {
                var categories = new CategoriesList<InterpretationCategory>(
                    new[]
                    {
                        new InterpretationCategory(EInterpretationCategory.III, (Probability)0,(Probability) 0.02),
                        new InterpretationCategory(EInterpretationCategory.II, (Probability) 0.02,(Probability) 0.04),
                        new InterpretationCategory(EInterpretationCategory.I, (Probability) 0.04,(Probability) 1.0)
                    });

                var result = translator.TranslateAssessmentResultWithLengthEffectAggregatedMethod(
                    ESectionInitialMechanismProbabilitySpecification.RelevantWithProbabilitySpecification,
                    Probability.Undefined,
                    Probability.Undefined,
                    ERefinementStatus.Performed,
                    new Probability(probabilityProfile),
                    new Probability(probabilitySection),
                    categories);
            },expectedError);
        }

        [Test]
        public void NoLengthEffectThrowsOnNullCategoriesTest()
        {
            TestHelper.AssertExpectedErrorMessage(() =>
            {
                var result = translator.TranslateAssessmentResultAggregatedMethod(
                    ESectionInitialMechanismProbabilitySpecification.RelevantWithProbabilitySpecification,
                    (Probability) 0.1,
                    ERefinementStatus.Performed,
                    (Probability) 0.1,
                    null);
            }, EAssemblyErrors.ValueMayNotBeNull);
        }

        [Test]
        public void NoLengthEffectThrowsOnNaNSectionProbabilityInitialMechanism()
        {
            TestHelper.AssertExpectedErrorMessage(() =>
            {
                var categories = new CategoriesList<InterpretationCategory>(
                    new[]
                    {
                        new InterpretationCategory(EInterpretationCategory.III, (Probability) 0, (Probability) 0.02),
                        new InterpretationCategory(EInterpretationCategory.II, (Probability) 0.02, (Probability) 0.04),
                        new InterpretationCategory(EInterpretationCategory.I, (Probability) 0.04, (Probability) 1.0)
                    });

                var result = translator.TranslateAssessmentResultAggregatedMethod(
                    ESectionInitialMechanismProbabilitySpecification.RelevantWithProbabilitySpecification,
                    Probability.Undefined,
                    ERefinementStatus.NotNecessary,
                    (Probability) 0.01,
                    categories);
            }, EAssemblyErrors.ProbabilityMayNotBeUndefined);
        }

        [Test]
        public void NoLengthEffectThrowsOnNaNRefinedSectionProbability()
        {
            TestHelper.AssertExpectedErrorMessage(() =>
            {
                var categories = new CategoriesList<InterpretationCategory>(
                    new[]
                    {
                        new InterpretationCategory(EInterpretationCategory.III, (Probability)0,(Probability) 0.02),
                        new InterpretationCategory(EInterpretationCategory.II, (Probability) 0.02,(Probability) 0.04),
                        new InterpretationCategory(EInterpretationCategory.I, (Probability) 0.04,(Probability) 1.0)
                    });

                var result = translator.TranslateAssessmentResultAggregatedMethod(
                    ESectionInitialMechanismProbabilitySpecification.RelevantWithProbabilitySpecification,
                    (Probability)0.01,
                    ERefinementStatus.Performed,
                    Probability.Undefined,
                    categories);
            },EAssemblyErrors.ProbabilityMayNotBeUndefined);
        }
    }
}