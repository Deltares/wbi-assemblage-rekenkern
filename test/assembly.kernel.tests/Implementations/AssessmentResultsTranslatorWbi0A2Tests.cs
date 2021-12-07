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

        [Test]
        [TestCase(false, 0.01, 0.02, false, 0.03, 0.04, EInterpretationCategory.III, 0.0,0.0)]
        [TestCase(true, 0.01, 0.02, false, 0.03, 0.04, EInterpretationCategory.III, 0.01, 0.02)]
        [TestCase(true, double.NaN, 0.02, false, double.NaN, double.NaN, EInterpretationCategory.III, 0.02, 0.02)]
        [TestCase(true, double.NaN, double.NaN, false, double.NaN, double.NaN, EInterpretationCategory.ND, double.NaN, double.NaN)]
        [TestCase(true, 0.01, 0.02, true, 0.03, 0.04, EInterpretationCategory.II, 0.03,0.04)]
        [TestCase(true, 0.01, 0.02, true, double.NaN, 0.04, EInterpretationCategory.II, 0.04, 0.04)]
        [TestCase(true, 0.01, 0.02, true, double.NaN, double.NaN, EInterpretationCategory.D, double.NaN, double.NaN)]
        public void Wbi0A2FunctionsTest(
            bool isRelevant, double probabilityProfileValue, double probabilitySectionValue,
            bool needsRefinement, double refinedProbabilityProfileValue,double refinedProbabilitySectionValue,
            EInterpretationCategory expectedCategory, double expectedProbabilityProfileValue, double expectedProbabilitySectionValue)
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
            Probability refinedProbabilityProfile = new Probability(refinedProbabilityProfileValue);
            Probability refinedProbabilitySection = new Probability(refinedProbabilitySectionValue);
            var result = translator.TranslateAssessmentResultWbi0A2(isRelevant, probabilityProfile, probabilitySection,
                needsRefinement, refinedProbabilityProfile, refinedProbabilitySection, categories);

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
        public void Wbi0A2ThrowsOnNullCategoriesTest()
        {
            try
            {
                var result = translator.TranslateAssessmentResultWbi0A2(true, (Probability) 0.1, (Probability) 0.1,
                    true, (Probability) 0.1, (Probability) 0.1, null);
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

                var result = translator.TranslateAssessmentResultWbi0A2(true, (Probability)0.1, (Probability)0.01, false,
                    Probability.NaN, Probability.NaN, categories);
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

                var result = translator.TranslateAssessmentResultWbi0A2(true, Probability.NaN, Probability.NaN, true,
                    (Probability) 0.1, (Probability) 0.01, categories);
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
    }
}