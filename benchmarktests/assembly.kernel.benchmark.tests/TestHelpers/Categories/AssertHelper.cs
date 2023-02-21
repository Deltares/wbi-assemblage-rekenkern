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
using System.Linq;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.Categories;
using MathNet.Numerics.Distributions;
using NUnit.Framework;

namespace assembly.kernel.benchmark.tests.TestHelpers.Categories
{
    /// <summary>
    /// Helper class to assert categories.
    /// </summary>
    public static class AssertHelper
    {
        /// <summary>
        /// Assert whether <paramref name="expectedCategories"/> and <paramref name="categories"/>
        /// are the same.
        /// </summary>
        /// <typeparam name="TCategory">The type of category.</typeparam>
        /// <typeparam name="TCategoryBase">The type of category base.</typeparam>
        /// <param name="expectedCategories">The expected categories.</param>
        /// <param name="categories">The actual categories.</param>
        /// <returns><c>tru</c> when the categories are equal; <c>false</c> otherwise.</returns>
        public static bool AssertEqualCategoriesList<TCategory, TCategoryBase>(CategoriesList<TCategory> expectedCategories,
                                                                               CategoriesList<TCategory> categories)
            where TCategory : CategoryLimits<TCategoryBase>, ICategoryLimits
        {
            try
            {
                Assert.AreEqual(expectedCategories.Categories.Count(), categories.Categories.Count());
                for (var i = 0; i < categories.Categories.Count(); i++)
                {
                    AssertAreEqualCategories(expectedCategories.Categories.ElementAt(i),
                                             categories.Categories.ElementAt(i));
                }

                return true;
            }
            catch (AssertionException e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        /// <summary>
        /// Assert whether two probabilities are equal.
        /// </summary>
        /// <param name="expectedProbability">The expected probability.</param>
        /// <param name="actualProbability">The actual probability.</param>
        public static void AssertAreEqualProbabilities(Probability expectedProbability, Probability actualProbability)
        {
            Assert.AreEqual(ProbabilityToReliability(expectedProbability), ProbabilityToReliability(actualProbability), 1e-3);
        }

        private static void AssertAreEqualCategories<TCategory>(CategoryLimits<TCategory> expectedCategory,
                                                                CategoryLimits<TCategory> calculatedCategory)
        {
            Assert.AreEqual(expectedCategory.Category, calculatedCategory.Category);
            AssertAreEqualProbabilities(expectedCategory.LowerLimit, calculatedCategory.LowerLimit);
            AssertAreEqualProbabilities(expectedCategory.UpperLimit, calculatedCategory.UpperLimit);
        }

        /// <summary>
        /// Calculates the reliability from a probability.
        /// </summary>
        /// <param name="probability">The probability to convert.</param>
        /// <returns>The reliability.</returns>
        private static double ProbabilityToReliability(double probability)
        {
            return Normal.InvCDF(0, 1, 1 - probability);
        }
    }
}