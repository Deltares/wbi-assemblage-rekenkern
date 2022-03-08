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
        [TestCase(0.2,0.4, EInterpretationCategory.IMin, 2.0)]
        [TestCase(0.01, 0.1, EInterpretationCategory.IIIMin, 10.0)]
        [TestCase(double.NaN, double.NaN, EInterpretationCategory.Dominant, 1.0)]
        [TestCase(double.NaN, double.NaN, EInterpretationCategory.NotDominant, 1.0)]
        [TestCase(double.NaN, double.NaN, EInterpretationCategory.Gr, 1.0)]
        [TestCase(0.01, 0.1, EInterpretationCategory.IIIMin, 10.0)]
        [TestCase(0.0, 0.0, EInterpretationCategory.NotRelevant, 1.0)]
        public void FailureMechanismSectionAssemblyResultConstructorChecksValidProbabilities(double probabilityProfile, double probabilitySection, EInterpretationCategory interpretationCategory, double expectedNValue)
        {
            var result = new FailureMechanismSectionAssemblyResult((Probability) probabilityProfile, (Probability) probabilitySection, interpretationCategory);
            Assert.AreEqual(expectedNValue, result.NSection);
            Assert.AreEqual(probabilityProfile, result.ProbabilityProfile.Value);
            Assert.AreEqual(probabilitySection, result.ProbabilitySection.Value);
            Assert.AreEqual(interpretationCategory, result.InterpretationCategory);
        }

        [Test]
        [TestCase(0.05, 0.01, EInterpretationCategory.III)]
        public void ConstructorChecksInputForInconsistentProbabilities(double profileProbability, double sectionProbability, EInterpretationCategory interpretationCategory)
        {
            try
            {
                var result = new FailureMechanismSectionAssemblyResult((Probability)profileProbability, (Probability)sectionProbability, interpretationCategory);
            }
            catch (AssemblyException e)
            {
                Assert.AreEqual(1, e.Errors.Count());
                Assert.AreEqual(EAssemblyErrors.ProfileProbabilityGreaterThanSectionProbability, e.Errors.First().ErrorCode);
                Assert.Pass();
            }
            Assert.Fail("Expected error was not thrown");
        }

        [Test]
        [TestCase(0.05, 0.01, EInterpretationCategory.NotRelevant)]
        [TestCase(double.NaN, 0.01, EInterpretationCategory.NotRelevant)]
        [TestCase(0.01, double.NaN, EInterpretationCategory.NotRelevant)]
        [TestCase(0.0, 0.01, EInterpretationCategory.NotRelevant)]
        [TestCase(0.01, 0.0, EInterpretationCategory.NotRelevant)]
        public void ConstructorChecksInputForInconsistentProbabilitiesNotRelevantCategory(double profileProbability, double sectionProbability, EInterpretationCategory interpretationCategory)
        {
            try
            {
                var result = new FailureMechanismSectionAssemblyResult((Probability)profileProbability, (Probability)sectionProbability, interpretationCategory);
            }
            catch (AssemblyException e)
            {
                Assert.AreEqual(1, e.Errors.Count());
                Assert.AreEqual(EAssemblyErrors.NonMatchingProbabilityValues, e.Errors.First().ErrorCode);
                Assert.Pass();
            }
            Assert.Fail("Expected error was not thrown");
        }

        [Test]
        [TestCase(double.NaN, 0.4, EInterpretationCategory.NotDominant)]
        [TestCase(0.4, double.NaN, EInterpretationCategory.NotDominant)]
        [TestCase(0.1, 0.2, EInterpretationCategory.NotDominant)]
        [TestCase(double.NaN, 0.4, EInterpretationCategory.Dominant)]
        [TestCase(0.4, double.NaN, EInterpretationCategory.Dominant)]
        [TestCase(0.1, 0.2, EInterpretationCategory.Dominant)]
        [TestCase(double.NaN, 0.4, EInterpretationCategory.Gr)]
        [TestCase(0.4, double.NaN, EInterpretationCategory.Gr)]
        [TestCase(0.1, 0.2, EInterpretationCategory.Gr)]
        public void ConstructorChecksInputForNaNValuesWithCorrespondingCategories(double profileValue, double sectionValue, EInterpretationCategory category)
        {
            try
            {
                var result = new FailureMechanismSectionAssemblyResult((Probability)profileValue, (Probability)sectionValue, category);
            }
            catch (AssemblyException e)
            {
                Assert.AreEqual(1, e.Errors.Count());
                Assert.AreEqual(EAssemblyErrors.NonMatchingProbabilityValues, e.Errors.First().ErrorCode);
                Assert.Pass();
            }
            Assert.Fail("Expected error was not thrown");
        }

        [Test]
        [TestCase(double.NaN, 0.4, EInterpretationCategory.III)]
        [TestCase(0.4, double.NaN, EInterpretationCategory.III)]
        [TestCase(double.NaN, double.NaN, EInterpretationCategory.III)]
        [TestCase(double.NaN, 0.4, EInterpretationCategory.II)]
        [TestCase(0.4, double.NaN, EInterpretationCategory.II)]
        [TestCase(double.NaN, double.NaN, EInterpretationCategory.II)]
        [TestCase(double.NaN, 0.4, EInterpretationCategory.I)]
        [TestCase(0.4, double.NaN, EInterpretationCategory.I)]
        [TestCase(double.NaN, double.NaN, EInterpretationCategory.I)]
        [TestCase(double.NaN, 0.4, EInterpretationCategory.Zero)]
        [TestCase(0.4, double.NaN, EInterpretationCategory.Zero)]
        [TestCase(double.NaN, double.NaN, EInterpretationCategory.Zero)]
        [TestCase(double.NaN, 0.4, EInterpretationCategory.IMin)]
        [TestCase(0.4, double.NaN, EInterpretationCategory.IMin)]
        [TestCase(double.NaN, double.NaN, EInterpretationCategory.IMin)]
        [TestCase(double.NaN, 0.4, EInterpretationCategory.IIMin)]
        [TestCase(0.4, double.NaN, EInterpretationCategory.IIMin)]
        [TestCase(double.NaN, double.NaN, EInterpretationCategory.IIMin)]
        [TestCase(double.NaN, 0.4, EInterpretationCategory.IIIMin)]
        [TestCase(0.4, double.NaN, EInterpretationCategory.IIIMin)]
        [TestCase(double.NaN, double.NaN, EInterpretationCategory.IIIMin)]
        public void ConstructorChecksInputForNotNaNValuesWithCorrespondingCategories(double profileValue, double sectionValue, EInterpretationCategory category)
        {
            try
            {
                var result = new FailureMechanismSectionAssemblyResult((Probability)profileValue, (Probability)sectionValue, category);
            }
            catch (AssemblyException e)
            {
                Assert.AreEqual(1, e.Errors.Count());
                Assert.AreEqual(EAssemblyErrors.ValueMayNotBeNaN, e.Errors.First().ErrorCode);
                Assert.Pass();
            }
            Assert.Fail("Expected error was not thrown");
        }

        [Test]
        public void ConstructorChecksForValidCategory()
        {
            try
            {
                var result = new FailureMechanismSectionAssemblyResult((Probability)0.1, (Probability)0.2, (EInterpretationCategory)(-1));
            }
            catch (AssemblyException e)
            {
                Assert.AreEqual(1, e.Errors.Count());
                Assert.AreEqual(EAssemblyErrors.InvalidCategoryValue, e.Errors.First().ErrorCode);
                Assert.Pass();
            }
            Assert.Fail("Expected error was not thrown");
        }

        [Test]
        public void FailureMechanismSectionAssemblyResultToStringTest()
        {
            var result = new FailureMechanismSectionAssemblyResult((Probability)0.2,(Probability)0.4,EInterpretationCategory.III);

            Assert.AreEqual("FailureMechanismSectionAssemblyResult [III Pprofile:0.2, Psection:0.4]", result.ToString());
        }
    }
}