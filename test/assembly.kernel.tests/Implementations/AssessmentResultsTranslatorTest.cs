#region Copyright (C) Rijkswaterstaat 2022. All rights reserved

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

using System.Linq;
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

        #region BOI-0A-1 functional tests

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
        #endregion

        #region BOI-0A-2 functional tests
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
        #endregion

        #region BOI-0A-2 Exception handling

        [Test]
        public void Boi0A2ChecksProfileProbabilityDoesNotExceedSectionProbabilityInitialMechanism()
        {
            try
            {
                translator.DetermineRepresentativeProbabilitiesBoi0A2(
                    false,
                    new Probability(0.3),
                    new Probability(0.04),
                    new Probability(0.1),
                    new Probability(0.2));
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
        public void Boi0A2ChecksProfileProbabilityDoesNotExceedSectionProbabilityRefinedProbability()
        {
            try
            {
                translator.DetermineRepresentativeProbabilitiesBoi0A2(
                    true,
                    new Probability(0.3),
                    new Probability(0.4),
                    new Probability(0.1),
                    new Probability(0.02));
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

        [TestCase(0.04,double.NaN)]
        [TestCase(double.NaN, 0.04)]
        public void Boi0A2ChecksIsDefinedInitialMechanism(double profileProbability, double sectionProbability)
        {
            try
            {
                translator.DetermineRepresentativeProbabilitiesBoi0A2(
                    false,
                    new Probability(profileProbability),
                    new Probability(sectionProbability),
                    new Probability(0.1),
                    new Probability(0.2));
            }
            catch (AssemblyException e)
            {
                Assert.NotNull(e.Errors);
                var message = e.Errors.FirstOrDefault();
                Assert.NotNull(message);
                Assert.AreEqual(EAssemblyErrors.ProbabilitiesShouldEitherBothBeDefinedOrUndefined, message.ErrorCode);
                Assert.Pass();
            }
            Assert.Fail("No expected exception not thrown");
        }

        [TestCase(0.04, double.NaN)]
        [TestCase(double.NaN, 0.04)]
        public void Boi0A2ChecksIsDefinedRefinedProbability(double profileProbability, double sectionProbability)
        {
            try
            {
                translator.DetermineRepresentativeProbabilitiesBoi0A2(
                    true,
                    new Probability(0.04),
                    new Probability(0.1),
                    new Probability(profileProbability),
                    new Probability(sectionProbability)
                );
            }
            catch (AssemblyException e)
            {
                Assert.NotNull(e.Errors);
                var message = e.Errors.FirstOrDefault();
                Assert.NotNull(message);
                Assert.AreEqual(EAssemblyErrors.ProbabilitiesShouldEitherBothBeDefinedOrUndefined, message.ErrorCode);
                Assert.Pass();
            }
            Assert.Fail("No expected exception not thrown");
        }

        [Test]
        public void Boi0A2ChecksIsDefinedRefinedProbability2()
        {
            try
            {
                translator.DetermineRepresentativeProbabilitiesBoi0A2(
                    true,
                    new Probability(0.04),
                    new Probability(0.1),
                    new Probability(0.2),
                    Probability.Undefined
                );
            }
            catch (AssemblyException e)
            {
                Assert.NotNull(e.Errors);
                var message = e.Errors.FirstOrDefault();
                Assert.NotNull(message);
                Assert.AreEqual(EAssemblyErrors.ProbabilitiesShouldEitherBothBeDefinedOrUndefined, message.ErrorCode);
                Assert.Pass();
            }

            Assert.Fail("No expected exception not thrown");
        }

        #endregion

        #region BOI-0B-1 functional tests

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
        #endregion

        #region BOI-0B-1 Exception handling

        [Test]
        public void Boi0B1HandlesNullCategories()
        {
            try
            {
                translator.DetermineInterpretationCategoryFromFailureMechanismSectionProbabilityBoi0B1(
                    new Probability(0.3),
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

        #endregion

        #region BOI-0C-1 functional tests

        [TestCase(EAnalysisState.NotRelevant, EInterpretationCategory.NotRelevant)]
        [TestCase(EAnalysisState.NoProbabilityEstimationNecessary, EInterpretationCategory.NotDominant)]
        [TestCase(EAnalysisState.ProbabilityEstimationNecessary, EInterpretationCategory.Dominant)]
        public void Boi0C1TranslatesAnalysisStateToInterpretationCategory(EAnalysisState state,
            EInterpretationCategory expectedCategory)
        {
            var category = translator.DetermineInterpretationCategoryWithoutProbabilityEstimationBoi0C1(state);

            Assert.AreEqual(expectedCategory, category);
        }

        #endregion

        #region BOI-0C-1 Exception handling
        [TestCase((EAnalysisState)(-1))]
        public void Boi0C1HandlesInvalidStates(EAnalysisState state)
        {
            try
            {
                translator.DetermineInterpretationCategoryWithoutProbabilityEstimationBoi0C1(state);
            }
            catch (AssemblyException e)
            {
                Assert.NotNull(e.Errors);
                var message = e.Errors.FirstOrDefault();
                Assert.NotNull(message);
                Assert.AreEqual(EAssemblyErrors.InvalidEnumValue, message.ErrorCode);
                Assert.Pass();
            }

            Assert.Fail("No expected exception not thrown");
        }
        #endregion

        #region BOI-0C-2 functional tests

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

        #endregion

        #region BOI-0C-2 Exception handling
        [TestCase(EInterpretationCategory.III)]
        [TestCase(EInterpretationCategory.II)]
        [TestCase(EInterpretationCategory.I)]
        [TestCase(EInterpretationCategory.Zero)]
        [TestCase(EInterpretationCategory.IMin)]
        [TestCase(EInterpretationCategory.IIMin)]
        [TestCase(EInterpretationCategory.IIIMin)]
        [TestCase((EInterpretationCategory)(-1))]
        public void Boi0C2HandlesInvalidCategoryValues(EInterpretationCategory category)
        {
            try
            {
                translator.TranslateInterpretationCategoryToProbabilityBoi0C2(category);
            }
            catch (AssemblyException e)
            {
                Assert.NotNull(e.Errors);
                var message = e.Errors.FirstOrDefault();
                Assert.NotNull(message);
                Assert.AreEqual(EAssemblyErrors.InvalidCategoryValue, message.ErrorCode);
                Assert.Pass();
            }

            Assert.Fail("No expected exception not thrown");
        }
        #endregion
    }
}
