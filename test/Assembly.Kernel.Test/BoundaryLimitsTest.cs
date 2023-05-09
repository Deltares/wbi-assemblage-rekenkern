// Copyright (C) Stichting Deltares and State of the Netherlands 2023. All rights reserved.
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
// All names, logos, and references to "Deltares" are registered trademarks of
// Stichting Deltares and remain full property of Stichting Deltares at all times.
// All rights reserved.

using Assembly.Kernel.Model;
using NUnit.Framework;

namespace Assembly.Kernel.Test
{
    [TestFixture]
    public class BoundaryLimitsTest
    {
        [Test]
        public void Constructor_ExpectedValues()
        {
            // Setup
            var lowerLimit = new Probability(0.0);
            var upperLimit = new Probability(1.0);

            // Call
            var boundaryLimits = new BoundaryLimits(lowerLimit, upperLimit);

            // Assert
            Assert.IsInstanceOf<IHasBoundaryLimits>(boundaryLimits);
            Assert.AreEqual(lowerLimit, boundaryLimits.LowerLimit);
            Assert.AreEqual(upperLimit, boundaryLimits.UpperLimit);
        }
    }
}