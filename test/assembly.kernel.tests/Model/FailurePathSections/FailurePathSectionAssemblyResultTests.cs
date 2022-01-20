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
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.Categories;
using Assembly.Kernel.Model.FailurePathSections;
using NUnit.Framework;

namespace Assembly.Kernel.Tests.Model.FailurePathSections
{
    [TestFixture]
    public class FailurePathSectionAssemblyResultTests
    {
        [Test]
        [TestCase(0.2,0.4, 2.0, EInterpretationCategory.IMin)]
        [TestCase(0.01, 0.1, 10.0, EInterpretationCategory.IIIMin)]
        [TestCase(double.NaN, 0.4, 1.0, EInterpretationCategory.Gr)]
        [TestCase(0.5, double.NaN, 1.0, EInterpretationCategory.Zero)]
        public void FailurePathSectionAssemblyResultConstructorChecksValidProbabilities(double probabilityProfile, double probabilitySection, double expectedNValue, EInterpretationCategory interpretationCategory)
        {
            var result = new FailurePathSectionAssemblyResult((Probability) probabilityProfile, (Probability) probabilitySection, interpretationCategory);
            Assert.AreEqual(expectedNValue, result.NSection);
            Assert.AreEqual(probabilityProfile, result.ProbabilityProfile);
            Assert.AreEqual(probabilitySection, result.ProbabilitySection);
            Assert.AreEqual(interpretationCategory, result.InterpretationCategory);
        }

        [Test]
        public void ConstructorChecksInput()
        {
            try
            {
                var result = new FailurePathSectionAssemblyResult((Probability)0.05, (Probability)0.01, EInterpretationCategory.III);
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
        public void FailurePathSectionAssemblyResultToStringTest()
        {
            var result = new FailurePathSectionAssemblyResult((Probability)0.2,(Probability)0.4,EInterpretationCategory.III);

            Assert.AreEqual("FailurePathSectionAssemblyResult [III Pprofile:0.2, Psection:0.4]", result.ToString());
        }
    }
}