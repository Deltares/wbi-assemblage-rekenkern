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

using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.Categories;
using NUnit.Framework;

namespace Assembly.Kernel.Test.Model.Categories
{
    [TestFixture]
    public class CategoryLimitsTest
    {
        [Test]
        [TestCase(double.NaN, 1.0, "lowerLimit")]
        [TestCase(1.0, double.NaN, "upperLimit")]
        [TestCase(double.NaN, double.NaN, "lowerLimit")]
        public void Constructor_UndefinedLimits_ThrowsAssemblyException(double lowerLimit, double upperLimit, string errorEntityId)
        {
            // Call
            void Call() => new TestCategoryLimits(0, new Probability(lowerLimit), new Probability(upperLimit));

            // Assert
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call, new[]
            {
                new AssemblyErrorMessage(errorEntityId, EAssemblyErrors.UndefinedProbability)
            });
        }

        [Test]
        public void Constructor_LowerLimitLargerThanUpperLimit_ThrowsAssemblyException()
        {
            // Call
            void Call() => new TestCategoryLimits(0, new Probability(1.0), new Probability(0.9));

            // Assert
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call, new[]
            {
                new AssemblyErrorMessage("lowerLimit", EAssemblyErrors.LowerLimitIsAboveUpperLimit)
            });
        }

        [Test]
        public void Constructor_ExpectedValues()
        {
            // Setup
            const int categoryValue = 4;
            var lowerLimit = new Probability(0.0);
            var upperLimit = new Probability(1.0);

            // Call
            var category = new TestCategoryLimits(categoryValue, lowerLimit, upperLimit);

            // Assert
            Assert.IsInstanceOf<IHasBoundaryLimits>(category);
            Assert.AreEqual(categoryValue, category.Category);
            Assert.AreEqual(lowerLimit, category.LowerLimit, 1e-6);
            Assert.AreEqual(upperLimit, category.UpperLimit, 1e-6);
        }

        private class TestCategoryLimits : CategoryLimits<int>
        {
            public TestCategoryLimits(int category, Probability lowerLimit, Probability upperLimit)
                : base(category, lowerLimit, upperLimit) {}
        }
    }
}