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
using System.Linq;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Implementations;
using Assembly.Kernel.Interfaces;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.Categories;
using NUnit.Framework;

// ReSharper disable UnusedMember.Local

namespace Assembly.Kernel.Tests.Implementations
{
    public class AssessmentResultsTranslatorWbi0A2Tests
    {
        private IAssessmentResultsTranslator translator;

        [SetUp]
        public void Init()
        {
            translator = new AssessmentResultsTranslator();
        }

        #region Functional tests with lengtheffect

        [Test]
        public void Wbi0A2NotRelevantTest()
        {
            var categories = new CategoriesList<InterpretationCategory>(
                new[]
                {
                    new InterpretationCategory(EInterpretationCategory.III, (Probability)0,(Probability) 0.02),
                    new InterpretationCategory(EInterpretationCategory.II, (Probability) 0.02,(Probability) 0.04),
                    new InterpretationCategory(EInterpretationCategory.I, (Probability) 0.04,(Probability) 1.0)
                });

            var relevanceInput = ESectionInitialMechanismProbabilitySpecification.NotRelevant;
            var result = translator.TranslateAssessmentResultWbi0A2(
                relevanceInput,
                Probability.NaN,
                Probability.NaN,
                false,
                Probability.NaN,
                Probability.NaN,
                categories);

            Assert.IsNotNull(result);
            Assert.AreEqual(EInterpretationCategory.III, result.InterpretationCategory);
            Assert.AreEqual(0.0, result.ProbabilityProfile);
            Assert.AreEqual(0.0, result.ProbabilitySection);
            Assert.AreEqual(1.0, result.NSection);
        }

        [Test]
        [TestCase(0.01, 0.02, EInterpretationCategory.III, 0.01, 0.02)]
        [TestCase(double.NaN, 0.02, EInterpretationCategory.III, 0.02, 0.02)]
        public void Wbi0A2InitialMechanismPrevailsTest(double probabilityProfileValue, double probabilitySectionValue, EInterpretationCategory expectedCategory, double expectedProbabilityProfileValue, double expectedProbabilitySectionValue)
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
            var result = translator.TranslateAssessmentResultWbi0A2(
                ESectionInitialMechanismProbabilitySpecification.RelevantWithProbabilitySpecification, 
                probabilityProfile, 
                probabilitySection,
                false, 
                Probability.NaN, 
                Probability.NaN, 
                categories);

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedCategory, result.InterpretationCategory);
            Assert.AreEqual(expectedProbabilityProfileValue, result.ProbabilityProfile);
            Assert.AreEqual(expectedProbabilitySectionValue, result.ProbabilitySection);
            var expectedN = double.IsNaN(expectedProbabilityProfileValue) | double.IsNaN(expectedProbabilityProfileValue)
                ? 1.0
                : expectedProbabilitySectionValue / expectedProbabilityProfileValue;
            Assert.AreEqual(expectedN, result.NSection);
        }

        [Test]
        [TestCase(0.01, 0.02)]
        [TestCase(double.NaN, 0.02)]
        [TestCase(double.NaN, double.NaN)]
        public void Wbi0A2RelevantNoProbabilityEstimationTest(double probabilityProfileValue, double probabilitySectionValue)
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
            var result = translator.TranslateAssessmentResultWbi0A2(
                ESectionInitialMechanismProbabilitySpecification.RelevantNoProbabilitySpecification,
                probabilityProfile,
                probabilitySection,
                false,
                Probability.NaN,
                Probability.NaN,
                categories);

            Assert.IsNotNull(result);
            Assert.AreEqual(EInterpretationCategory.NotDominant, result.InterpretationCategory);
            Assert.AreEqual(Probability.NaN, result.ProbabilityProfile);
            Assert.AreEqual(Probability.NaN, result.ProbabilitySection);
            Assert.AreEqual(1.0, result.NSection);
        }

        [Test]
        [TestCase(0.01, 0.02, 0.03, 0.04, EInterpretationCategory.II, 0.03, 0.04)]
        [TestCase(0.02, 0.01, 0.03, 0.04, EInterpretationCategory.II, 0.03, 0.04)]
        [TestCase(0.01, 0.02, double.NaN, 0.04, EInterpretationCategory.II, 0.04, 0.04)]
        [TestCase(0.01, 0.02, double.NaN, double.NaN, EInterpretationCategory.Dominant, double.NaN, double.NaN)]
        public void Wbi0A2RefinedProbabilityEstimationTest(double probabilityProfileValue, double probabilitySectionValue,
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
            var result = translator.TranslateAssessmentResultWbi0A2(
                ESectionInitialMechanismProbabilitySpecification.RelevantWithProbabilitySpecification,
                probabilityInitialMechanismProfile,
                probabilityInitialMechanismSection,
                true,
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
            Assert.AreEqual(expectedN, result.NSection);
        }

        #endregion

        #region Functional tests no lengtheffect

        [Test]
        public void Wbi0A2NotRelevantNoLengthEffectTest()
        {
            var categories = new CategoriesList<InterpretationCategory>(
                new[]
                {
                    new InterpretationCategory(EInterpretationCategory.III, (Probability)0,(Probability) 0.02),
                    new InterpretationCategory(EInterpretationCategory.II, (Probability) 0.02,(Probability) 0.04),
                    new InterpretationCategory(EInterpretationCategory.I, (Probability) 0.04,(Probability) 1.0)
                });

            var result = translator.TranslateAssessmentResultWbi0A2(
                ESectionInitialMechanismProbabilitySpecification.NotRelevant,
                Probability.NaN,
                false,
                Probability.NaN,
                categories);

            Assert.IsNotNull(result);
            Assert.AreEqual(EInterpretationCategory.III, result.InterpretationCategory);
            Assert.AreEqual(0.0, result.ProbabilityProfile);
            Assert.AreEqual(0.0, result.ProbabilitySection);
            Assert.AreEqual(1.0, result.NSection);
        }

        [Test]
        [TestCase(0.02, EInterpretationCategory.III, 0.02)]
        [TestCase(0.02, EInterpretationCategory.III, 0.02)]
        public void Wbi0A2InitialMechanismPrevailsNoLengthEffectTest(double probabilitySectionValue, EInterpretationCategory expectedCategory, double expectedProbabilityValues)
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
            var result = translator.TranslateAssessmentResultWbi0A2(
                ESectionInitialMechanismProbabilitySpecification.RelevantWithProbabilitySpecification,
                probabilitySection,
                false,
                (Probability)random.NextDouble(),
                categories);

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedCategory, result.InterpretationCategory);
            Assert.AreEqual(expectedProbabilityValues, result.ProbabilityProfile);
            Assert.AreEqual(expectedProbabilityValues, result.ProbabilitySection);
            Assert.AreEqual(1.0, result.NSection);
        }

        [Test]
        [TestCase(0.01)]
        [TestCase(double.NaN)]
        public void Wbi0A2RelevantNoProbabilityEstimationNoLengthEffectTest(double probabilitySectionValue)
        {
            var categories = new CategoriesList<InterpretationCategory>(
                new[]
                {
                    new InterpretationCategory(EInterpretationCategory.III, (Probability)0,(Probability) 0.02),
                    new InterpretationCategory(EInterpretationCategory.II, (Probability) 0.02,(Probability) 0.04),
                    new InterpretationCategory(EInterpretationCategory.I, (Probability) 0.04,(Probability) 1.0)
                });

            Probability probabilitySection = new Probability(probabilitySectionValue);
            var result = translator.TranslateAssessmentResultWbi0A2(
                ESectionInitialMechanismProbabilitySpecification.RelevantNoProbabilitySpecification,
                probabilitySection,
                false,
                Probability.NaN,
                categories);

            Assert.IsNotNull(result);
            Assert.AreEqual(EInterpretationCategory.NotDominant, result.InterpretationCategory);
            Assert.AreEqual(Probability.NaN, result.ProbabilityProfile);
            Assert.AreEqual(Probability.NaN, result.ProbabilitySection);
            Assert.AreEqual(1.0, result.NSection);
        }

        [Test]
        [TestCase(0.02, 0.04, EInterpretationCategory.II, 0.04)]
        [TestCase(0.01, 0.04, EInterpretationCategory.II, 0.04)]
        [TestCase(0.02, 0.04, EInterpretationCategory.II, 0.04)]
        [TestCase(0.02, double.NaN, EInterpretationCategory.Dominant, double.NaN)]
        public void Wbi0A2RefinedProbabilityEstimationNoLengthEffectTest(double probabilitySectionValue, double refinedProbabilitySectionValue,
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
            var result = translator.TranslateAssessmentResultWbi0A2(
                ESectionInitialMechanismProbabilitySpecification.RelevantWithProbabilitySpecification,
                probabilityInitialMechanismSection,
                true,
                refinedProbabilitySection,
                categories);

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedCategory, result.InterpretationCategory);
            Assert.AreEqual(expectedProbabilityValue, result.ProbabilityProfile);
            Assert.AreEqual(expectedProbabilityValue, result.ProbabilitySection);
            Assert.AreEqual(1.0, result.NSection);
        }

        #endregion


        #region input handling with length effect

        [Test]
        public void Wbi0A2ThrowsOnNullCategoriesTest()
        {
            try
            {
                var result = translator.TranslateAssessmentResultWbi0A2(
                    ESectionInitialMechanismProbabilitySpecification.RelevantWithProbabilitySpecification,
                    (Probability) 0.1,
                    (Probability) 0.1,
                    true,
                    (Probability) 0.1,
                    (Probability) 0.1,
                    null);
            }
            catch (AssemblyException e)
            {
                Assert.NotNull(e.Errors);
                var message = e.Errors.FirstOrDefault();
                Assert.NotNull(message);
                Assert.AreEqual(EAssemblyErrors.ValueMayNotBeNull, message.ErrorCode);
                Assert.Pass();
            }

            Assert.Fail("No expected exception not thrown");
        }

        [Test]
        public void Wbi0A2ThrowsOnLargeProfileProbabilityInitialMechanism()
        {
            try
            {
                var categories = new CategoriesList<InterpretationCategory>(
                    new[]
                    {
                        new InterpretationCategory(EInterpretationCategory.III, (Probability)0,(Probability) 0.02),
                        new InterpretationCategory(EInterpretationCategory.II, (Probability) 0.02,(Probability) 0.04),
                        new InterpretationCategory(EInterpretationCategory.I, (Probability) 0.04,(Probability) 1.0)
                    });

                var result = translator.TranslateAssessmentResultWbi0A2(
                    ESectionInitialMechanismProbabilitySpecification.RelevantWithProbabilitySpecification, 
                    (Probability)0.1, 
                    (Probability)0.01, 
                    false,
                    Probability.NaN, 
                    Probability.NaN, 
                    categories);
            }
            catch (AssemblyException e)
            {
                Assert.NotNull(e.Errors);
                var message = e.Errors.FirstOrDefault();
                Assert.NotNull(message);
                Assert.AreEqual(EAssemblyErrors.ProfileProbabilityGreaterThanSectionProbability, message.ErrorCode);
                Assert.Pass();
            }

            Assert.Fail("No expected exception not thrown");
        }

        [Test]
        public void Wbi0A2ThrowsOnLargeProfileRefinedProbability()
        {
            try
            {
                var categories = new CategoriesList<InterpretationCategory>(
                    new[]
                    {
                        new InterpretationCategory(EInterpretationCategory.III, (Probability)0,(Probability) 0.02),
                        new InterpretationCategory(EInterpretationCategory.II, (Probability) 0.02,(Probability) 0.04),
                        new InterpretationCategory(EInterpretationCategory.I, (Probability) 0.04,(Probability) 1.0)
                    });

                var result = translator.TranslateAssessmentResultWbi0A2(
                    ESectionInitialMechanismProbabilitySpecification.RelevantWithProbabilitySpecification, 
                    Probability.NaN, 
                    Probability.NaN, 
                    true,
                    (Probability) 0.1, 
                    (Probability) 0.01, 
                    categories);
            }
            catch (AssemblyException e)
            {
                Assert.NotNull(e.Errors);
                var message = e.Errors.FirstOrDefault();
                Assert.NotNull(message);
                Assert.AreEqual(EAssemblyErrors.ProfileProbabilityGreaterThanSectionProbability, message.ErrorCode);
                Assert.Pass();
            }

            Assert.Fail("No expected exception not thrown");
        }

        [Test]
        public void Wbi0A2ThrowsOnNaNSectionProbabilityInitialMechanism()
        {
            try
            {
                var categories = new CategoriesList<InterpretationCategory>(
                    new[]
                    {
                        new InterpretationCategory(EInterpretationCategory.III, (Probability)0,(Probability) 0.02),
                        new InterpretationCategory(EInterpretationCategory.II, (Probability) 0.02,(Probability) 0.04),
                        new InterpretationCategory(EInterpretationCategory.I, (Probability) 0.04,(Probability) 1.0)
                    });

                var result = translator.TranslateAssessmentResultWbi0A2(
                    ESectionInitialMechanismProbabilitySpecification.RelevantWithProbabilitySpecification,
                    new Probability(0.1),
                    Probability.NaN,
                    false,
                    (Probability)0.1,
                    (Probability)0.01,
                    categories);
            }
            catch (AssemblyException e)
            {
                Assert.NotNull(e.Errors);
                var message = e.Errors.FirstOrDefault();
                Assert.NotNull(message);
                Assert.AreEqual(EAssemblyErrors.ValueMayNotBeNaN, message.ErrorCode);
                Assert.Pass();
            }

            Assert.Fail("No expected exception not thrown");
        }

        #endregion

        #region input handleing no length effect

        [Test]
        public void Wbi0A2NoLengthEffectThrowsOnNullCategoriesTest()
        {
            try
            {
                var result = translator.TranslateAssessmentResultWbi0A2(
                    ESectionInitialMechanismProbabilitySpecification.RelevantWithProbabilitySpecification,
                    (Probability)0.1,
                    true,
                    (Probability)0.1,
                    null);
            }
            catch (AssemblyException e)
            {
                Assert.NotNull(e.Errors);
                var message = e.Errors.FirstOrDefault();
                Assert.NotNull(message);
                Assert.AreEqual(EAssemblyErrors.ValueMayNotBeNull, message.ErrorCode);
                Assert.Pass();
            }

            Assert.Fail("No expected exception not thrown");
        }

        [Test]
        public void Wbi0A2NoLengthEffectThrowsOnNaNSectionProbabilityInitialMechanism()
        {
            try
            {
                var categories = new CategoriesList<InterpretationCategory>(
                    new[]
                    {
                        new InterpretationCategory(EInterpretationCategory.III, (Probability)0,(Probability) 0.02),
                        new InterpretationCategory(EInterpretationCategory.II, (Probability) 0.02,(Probability) 0.04),
                        new InterpretationCategory(EInterpretationCategory.I, (Probability) 0.04,(Probability) 1.0)
                    });

                var result = translator.TranslateAssessmentResultWbi0A2(
                    ESectionInitialMechanismProbabilitySpecification.RelevantWithProbabilitySpecification,
                    Probability.NaN,
                    false,
                    (Probability)0.01,
                    categories);
            }
            catch (AssemblyException e)
            {
                Assert.NotNull(e.Errors);
                var message = e.Errors.FirstOrDefault();
                Assert.NotNull(message);
                Assert.AreEqual(EAssemblyErrors.ValueMayNotBeNaN, message.ErrorCode);
                Assert.Pass();
            }

            Assert.Fail("No expected exception not thrown");
        }

        #endregion
    }
}