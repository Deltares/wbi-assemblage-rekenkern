﻿#region Copyright (C) Rijkswaterstaat 2022. All rights reserved

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

using System.Linq;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.Categories;
using NUnit.Framework;

namespace Assembly.Kernel.Tests.Model.Categories
{
    [TestFixture]
    public class CategoryBaseTest
    {
        [Test]
        public void CategoryBasePerformsInputCheck()
        {
            try
            {
                var category = new TestCategoryBase(0.2, (Probability)0.02, (Probability)0.01);
            }
            catch (AssemblyException e)
            {
                Assert.AreEqual(1,e.Errors.Count());
                Assert.AreEqual(EAssemblyErrors.LowerLimitIsAboveUpperLimit, e.Errors.First().ErrorCode);
                Assert.Pass();
            }
            Assert.Fail("Expected error was not thrown");
        }

        [Test]
        public void CategoryBasePassesInput()
        {
            var categoryValue = 0.2;
            var lowerValue = (Probability)0.01;
            var upperValue = (Probability)0.02;
            var category = new TestCategoryBase(categoryValue, lowerValue, upperValue);
            Assert.AreEqual(categoryValue, category.Category);
            Assert.AreEqual(lowerValue, category.LowerLimit);
            Assert.AreEqual(upperValue, category.UpperLimit);
        }


        private class TestCategoryBase : CategoryBase<double>
        {
            public TestCategoryBase(double category, Probability lowerLimit, Probability upperLimit) : base(category, lowerLimit, upperLimit)
            {
            }
        }
    }
}
