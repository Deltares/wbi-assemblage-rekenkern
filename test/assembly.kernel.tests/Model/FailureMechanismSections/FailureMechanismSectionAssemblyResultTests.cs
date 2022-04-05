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
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.Categories;
using Assembly.Kernel.Model.FailureMechanismSections;
using NUnit.Framework;

namespace Assembly.Kernel.Tests.Model.FailureMechanismSections
{
    [TestFixture]
    public class FailureMechanismSectionAssemblyResultTests
    {
        [Test]
        [TestCase(0.4, EInterpretationCategory.IMin)]
        [TestCase(0.1, EInterpretationCategory.IIIMin)]
        [TestCase(double.NaN, EInterpretationCategory.Dominant)]
        [TestCase(double.NaN, EInterpretationCategory.NotDominant)]
        [TestCase(double.NaN, EInterpretationCategory.NoResult)]
        [TestCase(0.1, EInterpretationCategory.IIIMin)]
        [TestCase(0.0, EInterpretationCategory.NotRelevant)]
        public void FailureMechanismSectionAssemblyResultConstructorChecksValidProbabilities(double probabilitySection, EInterpretationCategory interpretationCategory)
        {
            var result = new FailureMechanismSectionAssemblyResult((Probability) probabilitySection, interpretationCategory);
            Assert.AreEqual(probabilitySection, result.ProbabilitySection);
            Assert.AreEqual(interpretationCategory, result.InterpretationCategory);
        }

        [Test]
        [TestCase(0.01, EInterpretationCategory.NotRelevant, EAssemblyErrors.NonMatchingProbabilityValues)]
        public void ConstructorChecksInputForInconsistentProbabilitiesNotRelevantCategory(double sectionProbability, EInterpretationCategory interpretationCategory, EAssemblyErrors expectedError)
        {
            TestHelper.AssertExpectedErrorMessage(() =>
            {
                var result = new FailureMechanismSectionAssemblyResult((Probability)sectionProbability, interpretationCategory);
            }, expectedError);
        }

        [Test]
        [TestCase(0.2, EInterpretationCategory.NotDominant)]
        [TestCase(0.2, EInterpretationCategory.Dominant)]
        [TestCase(0.2, EInterpretationCategory.NoResult)]
        public void ConstructorChecksInputForNaNValuesWithCorrespondingCategories(double sectionValue, EInterpretationCategory category)
        {
            TestHelper.AssertExpectedErrorMessage(() =>
            {
                var result = new FailureMechanismSectionAssemblyResult((Probability)sectionValue, category);
            }, EAssemblyErrors.NonMatchingProbabilityValues);
        }

        [TestCase(double.NaN, EInterpretationCategory.III)]
        [TestCase(double.NaN, EInterpretationCategory.II)]
        [TestCase(double.NaN, EInterpretationCategory.I)]
        [TestCase(double.NaN, EInterpretationCategory.Zero)]
        [TestCase(double.NaN, EInterpretationCategory.IMin)]
        [TestCase(double.NaN, EInterpretationCategory.IIMin)]
        [TestCase(double.NaN, EInterpretationCategory.IIIMin)]
        public void ConstructorChecksInputForNotNaNValuesWithCorrespondingCategories(double sectionValue, EInterpretationCategory category)
        {
            TestHelper.AssertExpectedErrorMessage(() =>
            {
                var result = new FailureMechanismSectionAssemblyResult((Probability)sectionValue, category);
            }, EAssemblyErrors.ProbabilityMayNotBeUndefined);
        }

        [Test]
        public void ConstructorChecksForValidCategory()
        {
            TestHelper.AssertExpectedErrorMessage(() =>
            {
                var result = new FailureMechanismSectionAssemblyResult((Probability)0.2, (EInterpretationCategory)(-1));
            }, EAssemblyErrors.InvalidCategoryValue);
        }

        [Test]
        public void FailureMechanismSectionAssemblyResultToStringTest()
        {
            var result = new FailureMechanismSectionAssemblyResult((Probability)0.4,EInterpretationCategory.III);

            Assert.AreEqual("FailureMechanismSectionAssemblyResult [III, Psection:1/3]", result.ToString());
        }
    }
}