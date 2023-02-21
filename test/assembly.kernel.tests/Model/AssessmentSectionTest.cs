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

namespace Assembly.Kernel.Tests.Model
{
    [TestFixture]
    public class AssessmentSectionTest
    {
        [Test]
        public void Constructor_SignalFloodingProbabilityLargerThanMaximumAllowableFloodingProbability_ThrowsAssemblyException()
        {
            // Call
            void Call() => new AssessmentSection(new Probability(1.0), new Probability(0.0));

            // Assert
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call, new[]
            {
                new AssemblyErrorMessage("AssessmentSection", EAssemblyErrors.SignalFloodingProbabilityAboveMaximumAllowableFloodingProbability)
            });
        }

        [Test]
        public void Constructor_ExpectedValues()
        {
            // Setup
            var signalFloodingProbability = new Probability(0.9);
            var maximumAllowableFloodingProbability = new Probability(1.0);

            // Call
            var assessmentSection = new AssessmentSection(signalFloodingProbability, maximumAllowableFloodingProbability);

            // Assert
            Assert.AreEqual(signalFloodingProbability, assessmentSection.SignalFloodingProbability, 1e-6);
            Assert.AreEqual(maximumAllowableFloodingProbability, assessmentSection.MaximumAllowableFloodingProbability, 1e-6);

            string expectedString = $"Signal flooding probability: {signalFloodingProbability}, "
                                    + Environment.NewLine
                                    + $"Maximum allowable flooding probability: {maximumAllowableFloodingProbability}";
            Assert.AreEqual(expectedString, assessmentSection.ToString());
        }
    }
}