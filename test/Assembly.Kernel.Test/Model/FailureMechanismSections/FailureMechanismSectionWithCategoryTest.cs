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

using Assembly.Kernel.Model.Categories;
using Assembly.Kernel.Model.FailureMechanismSections;
using NUnit.Framework;

namespace Assembly.Kernel.Test.Model.FailureMechanismSections
{
    [TestFixture]
    public class FailureMechanismSectionWithCategoryTest
    {
        [Test]
        public void Constructor_ExpectedValues()
        {
            // Setup
            const double sectionStart = 0.10;
            const double sectionEnd = 5189.015;
            const EInterpretationCategory category = EInterpretationCategory.II;

            // Call
            var section = new FailureMechanismSectionWithCategory(sectionStart, sectionEnd, category);

            // Assert
            Assert.IsInstanceOf<FailureMechanismSection>(section);
            Assert.AreEqual(sectionStart, section.Start);
            Assert.AreEqual(sectionEnd, section.End);
            Assert.AreEqual(category, section.Category);
        }
    }
}