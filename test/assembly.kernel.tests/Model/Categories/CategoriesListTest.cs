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

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.Categories;
using NUnit.Framework;

namespace Assembly.Kernel.Tests.Model.Categories
{
    [TestFixture]
    public class CategoriesListTest
    {
        [TestCaseSource(
             typeof(CategoriesListTest),
             nameof(InconsistentCategoryBoundariesTestCases))]
        public void CheckForInconsistentCategories(IEnumerable<TestCategory> categories)
        {
            TestHelper.AssertExpectedErrorMessage(() =>
            {
                var list = new CategoriesList<TestCategory>(categories.ToArray());
            }, EAssemblyErrors.InvalidCategoryLimits);
        }

        [Test]
        public void ConstructorAcceptsCorrectListOfCategories()
        {
            var list = new CategoriesList<TestCategory>(new[]
            {
                new TestCategory(0.0, 0.5),
                new TestCategory(0.5, 1.0)
            });

            Assert.IsNotNull(list);
            Assert.AreEqual(2, list.Categories.Length);
        }

        [TestCase(0.0, "A")]
        [TestCase(0.2, "A")]
        [TestCase(0.3, "A")]
        [TestCase(0.4, "B")]
        [TestCase(1.0, "B")]
        public void GetCategoryForFailureProbabilityTest(double probability, string expectedCategory)
        {
            var list = new CategoriesList<TestCategory>(new[]
            {
                new TestCategory(0.0, 0.3, "A"),
                new TestCategory(0.3, 1.0, "B")
            });

            var category = list.GetCategoryForFailureProbability((Probability) probability);

            Assert.IsNotNull(category);
            Assert.AreEqual(expectedCategory, category.CategoryIdentifier);
        }

        [Test]
        public void GetCategoryForFailureProbabilityTestThrowsOnInvalidProbability()
        {
            var list = new CategoriesList<TestCategory>(new[]
            {
                new TestCategory(0.0, 0.3),
                new TestCategory(0.3, 1.0)
            });

            TestHelper.AssertExpectedErrorMessage(() =>
            {
                var category = list.GetCategoryForFailureProbability(Probability.Undefined);
            }, EAssemblyErrors.ProbabilityMayNotBeUndefined);
        }

        private static IEnumerable InconsistentCategoryBoundariesTestCases
        {
            get
            {
                yield return new TestCaseData(
                    new List<TestCategory>
                    {
                        new TestCategory(0.0, 0.9)
                    });

                yield return new TestCaseData(
                    new List<TestCategory>
                    {
                        new TestCategory(0.1, 0.2),
                        new TestCategory(0.2, 0.5),
                        new TestCategory(0.5, 1.0)
                    });

                yield return new TestCaseData(
                    new List<TestCategory>
                    {
                        new TestCategory(0.0, 2e-3),
                        new TestCategory(2e-4, 0.2),
                        new TestCategory(0.2, 0.5),
                        new TestCategory(0.5, 1.0)
                    });

                yield return new TestCaseData(
                    new List<TestCategory>
                    {
                        new TestCategory(0.0, 0.1),
                        new TestCategory(0.1, 0.2),
                        new TestCategory(0.15, 0.5),
                        new TestCategory(0.5, 1.0)
                    });

                yield return new TestCaseData(
                    new List<TestCategory>
                    {
                        new TestCategory(0.0, 0.1),
                        new TestCategory(0.1, 0.2),
                        new TestCategory(0.05, 0.5),
                        new TestCategory(0.5, 1.0)
                    });

                yield return new TestCaseData(
                    new List<TestCategory>
                    {
                        new TestCategory(0.0, 0.1),
                        new TestCategory(0.1, 0.2),
                        new TestCategory(0.2, 0.5),
                        new TestCategory(0.5 + 1e-7, 1.0)
                    });

                yield return new TestCaseData(
                    new List<TestCategory>
                    {
                        new TestCategory(0.0, 0.1),
                        new TestCategory(0.2, 0.5),
                        new TestCategory(0.5, 1.0)
                    });
            }
        }
    }
}