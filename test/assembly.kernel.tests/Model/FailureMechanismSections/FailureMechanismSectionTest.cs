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
using Assembly.Kernel.Model.FailureMechanismSections;
using NUnit.Framework;

namespace Assembly.Kernel.Tests.Model.FailureMechanismSections
{
    [TestFixture]
    public class FailureMechanismSectionTest
    {
        [Test]
        public void ConstructorPassesInput()
        {
            var sectionStart = 2.0;
            var sectionEnd = 20.0;
            var section = new FailureMechanismSection(sectionStart,sectionEnd);

            Assert.AreEqual(sectionStart,section.Start);
            Assert.AreEqual(sectionEnd, section.End);
        }

        [Test]
        public void CenterIsCalculatedCorrectly()
        {
            var sectionStart = 2.0;
            var sectionEnd = 3.0;
            var section = new FailureMechanismSection(sectionStart, sectionEnd);

            Assert.AreEqual(2.5, section.Center);
        }

        [Test,
        TestCase(-0.1,30),
        TestCase(10.0, 4.0),
        TestCase(10.0, 10.0)]
        public void ConstructorChecksIncorrectInput(double start, double end)
        {
            TestHelper.AssertExpectedErrorMessage(() =>
            {
                var section = new FailureMechanismSection(start,end);
            }, EAssemblyErrors.FailureMechanismSectionSectionStartEndInvalid);
        }

        [Test,
         TestCase(double.NaN, 30),
         TestCase(10.0, double.NaN),
         TestCase(double.NaN, double.NaN)]
        public void ConstructorChecksForNaNValues(double start, double end)
        {
            TestHelper.AssertExpectedErrorMessage(() =>
            {
                var section = new FailureMechanismSection(start, end);
            }, EAssemblyErrors.ProbabilityMayNotBeUndefined);
        }
    }
}
