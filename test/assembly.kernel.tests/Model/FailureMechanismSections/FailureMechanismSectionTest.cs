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

using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Model.FailureMechanismSections;
using NUnit.Framework;

namespace Assembly.Kernel.Tests.Model.FailureMechanismSections
{
    [TestFixture]
    public class FailureMechanismSectionTest
    {
        [Test]
        [TestCase(double.NaN, EAssemblyErrors.UndefinedProbability)]
        [TestCase(-0.1, EAssemblyErrors.FailureMechanismSectionSectionStartEndInvalid)]
        public void Constructor_InvalidStart_ThrowsAssemblyException(double start, EAssemblyErrors expectedError)
        {
            // Call
            void Call() => new FailureMechanismSection(start, 10.0);

            // Assert
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call, new[]
            {
                new AssemblyErrorMessage("start", expectedError)
            });
        }

        [Test]
        [TestCase(double.NaN, EAssemblyErrors.UndefinedProbability)]
        [TestCase(9.0, EAssemblyErrors.FailureMechanismSectionSectionStartEndInvalid)]
        public void Constructor_InvalidEnd_ThrowsAssemblyException(double end, EAssemblyErrors expectedError)
        {
            // Call
            void Call() => new FailureMechanismSection(10.0, end);

            // Assert
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call, new[]
            {
                new AssemblyErrorMessage("end", expectedError)
            });
        }

        [Test]
        public void Constructor_ExpectedValues()
        {
            // Setup
            const double sectionStart = 2.0;
            const double sectionEnd = 20.0;

            // Call
            var section = new FailureMechanismSection(sectionStart, sectionEnd);

            // Assert
            Assert.AreEqual(sectionStart, section.Start);
            Assert.AreEqual(sectionEnd, section.End);
            Assert.AreEqual(11.0, section.Center);
        }
    }
}