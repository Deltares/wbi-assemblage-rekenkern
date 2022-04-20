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
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.Categories;
using Assembly.Kernel.Model.FailureMechanismSections;
using NUnit.Framework;

namespace Assembly.Kernel.Tests.Model.FailureMechanismSections
{
    [TestFixture]
    public class FailureMechanismSectionAssemblyResultWithLengthEffectTests
    {
        [TestCase(0.2,0.4, EInterpretationCategory.IMin, 2.0)]
        [TestCase(0.01, 0.1, EInterpretationCategory.IIIMin, 10.0)]
        [TestCase(double.NaN, double.NaN, EInterpretationCategory.Dominant, 1.0)]
        [TestCase(double.NaN, double.NaN, EInterpretationCategory.NotDominant, 1.0)]
        [TestCase(double.NaN, double.NaN, EInterpretationCategory.NoResult, 1.0)]
        [TestCase(0.01, 0.1, EInterpretationCategory.IIIMin, 10.0)]
        [TestCase(0.0, 0.0, EInterpretationCategory.NotRelevant, 1.0)]
        public void FailureMechanismSectionAssemblyResultConstructorChecksValidProbabilities(double probabilityProfile, double probabilitySection, EInterpretationCategory interpretationCategory, double expectedLengthEffectFactorValue)
        {
            var result = new FailureMechanismSectionAssemblyResultWithLengthEffect((Probability) probabilityProfile, (Probability) probabilitySection, interpretationCategory);
            Assert.AreEqual(expectedLengthEffectFactorValue, result.LengthEffectFactor);
            Assert.AreEqual(probabilityProfile, result.ProbabilityProfile);
            Assert.AreEqual(probabilitySection, result.ProbabilitySection);
            Assert.AreEqual(interpretationCategory, result.InterpretationCategory);
        }

        [TestCase(0.05, 0.01, EInterpretationCategory.III)]
        public void ConstructorChecksInputForInconsistentProbabilities(double profileProbability, double sectionProbability, EInterpretationCategory interpretationCategory)
        {
            TestHelper.AssertExpectedErrorMessage(() =>
            {
                var result = new FailureMechanismSectionAssemblyResultWithLengthEffect((Probability)profileProbability, (Probability)sectionProbability, interpretationCategory);
            }, EAssemblyErrors.ProfileProbabilityGreaterThanSectionProbability);
        }

        [TestCase(0.05, 0.01, EInterpretationCategory.NotRelevant, EAssemblyErrors.ProfileProbabilityGreaterThanSectionProbability)]
        [TestCase(double.NaN, 0.01, EInterpretationCategory.NotRelevant, EAssemblyErrors.ProbabilitiesNotBothDefinedOrUndefined)]
        [TestCase(0.01, double.NaN, EInterpretationCategory.NotRelevant, EAssemblyErrors.ProbabilitiesNotBothDefinedOrUndefined)]
        [TestCase(0.0, 0.01, EInterpretationCategory.NotRelevant, EAssemblyErrors.NonMatchingProbabilityValues)]
        [TestCase(0.01, 0.0, EInterpretationCategory.NotRelevant, EAssemblyErrors.ProfileProbabilityGreaterThanSectionProbability)]
        public void ConstructorChecksInputForInconsistentProbabilitiesNotRelevantCategory(double profileProbability, double sectionProbability, EInterpretationCategory interpretationCategory, EAssemblyErrors expectedError)
        {
            TestHelper.AssertExpectedErrorMessage(() =>
            {
                var result = new FailureMechanismSectionAssemblyResultWithLengthEffect((Probability)profileProbability, (Probability)sectionProbability, interpretationCategory);
            }, expectedError);
        }

        [TestCase(EInterpretationCategory.NotDominant)]
        [TestCase(EInterpretationCategory.Dominant)]
        [TestCase(EInterpretationCategory.NoResult)]
        public void ConstructorChecksInputForUndefinedProbabilitiesWithCorrespondingCategories(EInterpretationCategory category)
        {
            var definedProbability = new Probability(0.02);
            TestHelper.AssertExpectedErrorMessage(() =>
            {
                var result = new FailureMechanismSectionAssemblyResultWithLengthEffect(definedProbability, definedProbability, category);
            }, EAssemblyErrors.NonMatchingProbabilityValues);
        }

        [TestCase(double.NaN, 0.4, EInterpretationCategory.NotDominant)]
        [TestCase(0.4, double.NaN, EInterpretationCategory.NotDominant)]
        [TestCase(double.NaN, 0.4, EInterpretationCategory.Dominant)]
        [TestCase(0.4, double.NaN, EInterpretationCategory.Dominant)]
        [TestCase(double.NaN, 0.4, EInterpretationCategory.NoResult)]
        [TestCase(0.4, double.NaN, EInterpretationCategory.NoResult)]
        public void ConstructorChecksInputForNonMatchingProbabilitiesWithCorrespondingCategories(double profileValue, double sectionValue, EInterpretationCategory category)
        {
            TestHelper.AssertExpectedErrorMessage(() =>
            {
                var result = new FailureMechanismSectionAssemblyResultWithLengthEffect((Probability)profileValue, (Probability)sectionValue, category);
            }, EAssemblyErrors.ProbabilitiesNotBothDefinedOrUndefined);
        }

        [TestCase(double.NaN, 0.4, EInterpretationCategory.III)]
        [TestCase(0.4, double.NaN, EInterpretationCategory.III)]
        [TestCase(double.NaN, 0.4, EInterpretationCategory.II)]
        [TestCase(0.4, double.NaN, EInterpretationCategory.II)]
        [TestCase(double.NaN, 0.4, EInterpretationCategory.I)]
        [TestCase(0.4, double.NaN, EInterpretationCategory.I)]
        [TestCase(double.NaN, 0.4, EInterpretationCategory.Zero)]
        [TestCase(0.4, double.NaN, EInterpretationCategory.Zero)]
        [TestCase(double.NaN, 0.4, EInterpretationCategory.IMin)]
        [TestCase(0.4, double.NaN, EInterpretationCategory.IMin)]
        [TestCase(double.NaN, 0.4, EInterpretationCategory.IIMin)]
        [TestCase(0.4, double.NaN, EInterpretationCategory.IIMin)]
        [TestCase(double.NaN, 0.4, EInterpretationCategory.IIIMin)]
        [TestCase(0.4, double.NaN, EInterpretationCategory.IIIMin)]
        public void ConstructorChecksInputForNotMatchingProbabilitiesWithCorrespondingCategories(double profileValue, double sectionValue, EInterpretationCategory category)
        {
            TestHelper.AssertExpectedErrorMessage(() =>
            {
                var result = new FailureMechanismSectionAssemblyResultWithLengthEffect((Probability)profileValue, (Probability)sectionValue, category);
            }, EAssemblyErrors.ProbabilitiesNotBothDefinedOrUndefined);
        }

        [TestCase(EInterpretationCategory.III)]
        [TestCase(EInterpretationCategory.II)]
        [TestCase(EInterpretationCategory.I)]
        [TestCase(EInterpretationCategory.Zero)]
        [TestCase(EInterpretationCategory.IMin)]
        [TestCase(EInterpretationCategory.IIMin)]
        [TestCase(EInterpretationCategory.IIIMin)]
        public void ConstructorChecksInputForUndefinedProbabilitiesCorrespondingCategories(EInterpretationCategory category)
        {
            var undefinedProbability = Probability.Undefined;
            TestHelper.AssertExpectedErrorMessage(() =>
            {
                var result = new FailureMechanismSectionAssemblyResultWithLengthEffect(undefinedProbability, undefinedProbability, category);
            }, EAssemblyErrors.UndefinedProbability);
        }

        [Test]
        public void ConstructorChecksForValidCategory()
        {
            TestHelper.AssertExpectedErrorMessage(() =>
            {
                var result = new FailureMechanismSectionAssemblyResultWithLengthEffect((Probability)0.1, (Probability)0.2, (EInterpretationCategory)(-1));
            }, EAssemblyErrors.InvalidCategoryValue);
        }

        [Test]
        public void FailureMechanismSectionAssemblyResultToStringTest()
        {
            var result = new FailureMechanismSectionAssemblyResultWithLengthEffect((Probability)0.2,(Probability)0.4,EInterpretationCategory.III);

            Assert.AreEqual("FailureMechanismSectionAssemblyResultWithLengthEffect [III Pprofile:1/5, Psection:1/3]", result.ToString());
        }
    }
}