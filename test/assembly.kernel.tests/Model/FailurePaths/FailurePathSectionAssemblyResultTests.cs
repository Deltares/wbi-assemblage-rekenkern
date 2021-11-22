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
using Assembly.Kernel.Model.FailurePaths;
using NUnit.Framework;

namespace Assembly.Kernel.Tests.Model.FailurePaths
{
    [TestFixture]
    public class FailurePathSectionAssemblyResultTests
    {
        [Test]
        [TestCase(0.2,0.4, 2.0)]
        [TestCase(0.01, 0.1, 10.0)]
        [TestCase(double.NaN, 0.4, 1.0)]
        [TestCase(0.5, double.NaN, 1.0)]
        public void FailurePathSectionAssemblyResultConstructorChecksValidProbabilities(double probabilityProfile, double probabilitySection, double expectedNValue)
        {
            var result = new FailurePathSectionAssemblyResult((Probability) probabilityProfile, (Probability) probabilitySection, EInterpretationCategory.D);
            Assert.AreEqual(expectedNValue, result.NSection);
        }

        [Test]
        public void FailurePathSectionAssemblyResultToStringTest()
        {
            var result = new FailurePathSectionAssemblyResult((Probability)0.2,(Probability)0.1,EInterpretationCategory.III);

            Assert.AreEqual("FailurePathSectionAssemblyResult [III Pprofile:0.2, Psection:0.1]", result.ToString());
        }
    }
}