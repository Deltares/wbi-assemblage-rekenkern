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

using Assembly.Kernel.Model;
using Assembly.Kernel.Model.Categories;
using NUnit.Framework;

namespace Assembly.Kernel.Tests.Model.Categories
{
    [TestFixture]
    public class InterpretationCategoryTest
    {
        [Test]
        public void Constructor_ExpectedValues()
        {
            // Setup
            const EInterpretationCategory categoryValue = EInterpretationCategory.II;
            var lowerLimit = new Probability(0.001);
            var upperLimit = new Probability(0.01);

            // Call
            var category = new InterpretationCategory(categoryValue, lowerLimit, upperLimit);

            // Assert
            Assert.IsInstanceOf<CategoryLimits<EInterpretationCategory>>(category);
            Assert.AreEqual(categoryValue, category.Category);
            Assert.AreEqual(lowerLimit, category.LowerLimit);
            Assert.AreEqual(upperLimit, category.UpperLimit);
        }
    }
}