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
using System.Collections.Generic;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.Categories;
using NUnit.Framework;

namespace Assembly.Kernel.Test.Model.Categories
{
    [TestFixture]
    public class CategoriesListTest
    {
        [Test]
        public void Constructor_CategoriesNull_ThrowsArgumentNullException()
        {
            // Call
            void Call() => new CategoriesList<TestCategory>(null);

            // Assert
            var exception = Assert.Throws<ArgumentNullException>(Call);
            Assert.AreEqual("categories", exception.ParamName);
        }

        [Test]
        [TestCaseSource(nameof(GetInvalidCategories))]
        public void Constructor_InvalidCategories_ThrowsAssemblyException(
            IEnumerable<TestCategory> categories)
        {
            // Call
            void Call() => new CategoriesList<TestCategory>(categories);

            // Assert
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call, new[]
            {
                new AssemblyErrorMessage("categories", EAssemblyErrors.InvalidCategoryLimits)
            });
        }

        [Test]
        public void Constructor_ExpectedValues()
        {
            // Setup
            var categories = new[]
            {
                new TestCategory(0.0, 1.0)
            };

            // Call
            var categoriesList = new CategoriesList<TestCategory>(categories);

            // Assert
            CollectionAssert.AreEqual(categories, categoriesList.Categories);
        }

        [Test]
        public void GetCategoryForFailureProbability_UndefinedProbability_ThrowsAssemblyException()
        {
            // Setup
            var categoriesList = new CategoriesList<TestCategory>(new[]
            {
                new TestCategory(0.0, 1.0)
            });

            // Call
            void Call() => categoriesList.GetCategoryForFailureProbability(Probability.Undefined);

            // Assert
            TestHelper.AssertThrowsAssemblyExceptionWithAssemblyErrorMessages(Call, new[]
            {
                new AssemblyErrorMessage("failureProbability", EAssemblyErrors.UndefinedProbability)
            });
        }

        [Test]
        public void GetCategoryForFailureProbability_WithProbability_ReturnsExpectedCategory()
        {
            // Setup
            var expectedCategory = new TestCategory(0.33, 0.66);
            TestCategory[] categories =
            {
                new TestCategory(0.0, 0.33),
                expectedCategory,
                new TestCategory(0.66, 1.0)
            };

            var categoriesList = new CategoriesList<TestCategory>(categories);

            // Call
            TestCategory category = categoriesList.GetCategoryForFailureProbability(new Probability(0.53));

            // Assert
            Assert.AreEqual(expectedCategory.LowerLimit, category.LowerLimit);
            Assert.AreEqual(expectedCategory.UpperLimit, category.UpperLimit);
            Assert.AreEqual(expectedCategory.CategoryIdentifier, category.CategoryIdentifier);
        }

        private static IEnumerable<TestCaseData> GetInvalidCategories()
        {
            yield return new TestCaseData(new List<TestCategory>
            {
                new TestCategory(0.0 + 1e-9, 1.0)
            });

            yield return new TestCaseData(new List<TestCategory>
            {
                new TestCategory(0.0, 1.0 - 1e-9)
            });

            yield return new TestCaseData(new List<TestCategory>
            {
                new TestCategory(0.0, 0.6 + 1e-9),
                new TestCategory(0.6, 1.0)
            });

            yield return new TestCaseData(new List<TestCategory>
            {
                new TestCategory(0.0, 0.6),
                new TestCategory(0.6 + 1e-9, 1.0)
            });
        }
    }
}