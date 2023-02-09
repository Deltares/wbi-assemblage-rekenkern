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

using System;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Model;
using NUnit.Framework;

namespace Assembly.Kernel.Tests.Model.AssessmentSection
{
    [TestFixture]
    public class AssessmentSectionTest
    {
        [TestCase(0,0 )]
        [TestCase(1, 1)]
        [TestCase(0.1, 0.2)]
        public void AssessmentSectionInputValidationTest(double signalFloodingProbability,
            double maximumAllowableFloodingProbability)
        {
            var section = new Kernel.Model.AssessmentSection.AssessmentSection((Probability) signalFloodingProbability,
                (Probability) maximumAllowableFloodingProbability);
            Assert.AreEqual(signalFloodingProbability, section.SignalFloodingProbability);
            Assert.AreEqual(maximumAllowableFloodingProbability, section.MaximumAllowableFloodingProbability);
        }

        [TestCase(0.1, 0.05)]
        [TestCase(0.2, 0.1)]
        public void AssessmentSectionInputValidationTestWithException(double signalFloodingProbability, double maximumAllowableFloodingProbability)
        {
            TestHelper.AssertExpectedErrorMessage(() =>
            {
                var section = new Kernel.Model.AssessmentSection.AssessmentSection((Probability)signalFloodingProbability, (Probability)maximumAllowableFloodingProbability);
            }, EAssemblyErrors.SignalFloodingProbabilityAboveMaximumAllowableFloodingProbability);
        }

        [Test]
        public void ToStringWorks()
        {
            var section = new Kernel.Model.AssessmentSection.AssessmentSection(Probability.Undefined, new Probability(0.001));
            Assert.AreEqual("Signal flooding probability: Undefined, " + Environment.NewLine + "Maximum allowable flooding probability: 1/1000", section.ToString());
        }
    }
}