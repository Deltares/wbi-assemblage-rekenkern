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
using Assembly.Kernel.Model.FailureMechanismSections;
using NUnit.Framework;

namespace Assembly.Kernel.Tests.Model.FailureMechanismSections
{
    [TestFixture]
    public class ResultWithProfileAndSectionProbabilitiesTest
    {
        [TestCase(0.2, 0.4, 2.0)]
        [TestCase(0.01, 0.1, 10.0)]
        [TestCase(double.NaN, double.NaN, 1.0)]
        [TestCase(0.0, 0.0, 1.0)]
        [TestCase(1E-5, 1E-5, 1.0)]

        public void ConstructorPassesArguments(double probabilityProfileValue, double probabilitySectionValue, double expectedLengthEffectFactor)
        {
            var probabilityProfile = new Probability(probabilityProfileValue);
            var probabilitySection = new Probability(probabilitySectionValue);
            var result = new ResultWithProfileAndSectionProbabilities(probabilityProfile, probabilitySection);

            Assert.AreEqual(probabilityProfile, result.ProbabilityProfile);
            Assert.AreEqual(probabilitySection, result.ProbabilitySection);
            Assert.AreEqual(expectedLengthEffectFactor, result.LengthEffectFactor);
        }

        [TestCase(double.NaN, 0.1)]
        [TestCase(0.1, double.NaN)]
        public void ConstructorChecksEquallyDefinedProbabilities(double profileProbability, double sectionProbability)
        {
            TestHelper.AssertExpectedErrorMessage(() =>
            {
                var result = new ResultWithProfileAndSectionProbabilities((Probability) profileProbability, (Probability) sectionProbability);
            }, EAssemblyErrors.ProbabilitiesShouldEitherBothBeDefinedOrUndefined);
        }

        [Test]
        public void ConstructorChecksForSmallerProfileProbabilities()
        {
            TestHelper.AssertExpectedErrorMessage(() =>
            {
                var result = new ResultWithProfileAndSectionProbabilities((Probability)0.1, (Probability)0.002);
            }, EAssemblyErrors.ProfileProbabilityGreaterThanSectionProbability);
        }
    }
}
