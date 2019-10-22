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

using System;
using System.Collections.Generic;
using System.Linq;
using Assembly.Kernel.Exceptions;

namespace Assembly.Kernel.Model.CategoryLimits
{
    /// <summary>
    /// This object us used to validate category boundaries once and remove this validation from all methods that use category boundaries.
    /// </summary>
    public class CategoriesList<TCategory> where TCategory : ICategoryLimits
    {
        /// <summary>
        /// The epsilon that is used when comparing category boundaries. Gaps between category boundaries smaller than EpsilonFactor will not be taken into account.
        /// </summary>
        public static readonly double EpsilonFactor = 1e-40;

        /// <summary>
        /// This constructor validates a list of category limits and assigns the correct list to the Categories property.
        /// </summary>
        /// <param name="categoryLimits">An IEnumerable with categories. This list assumes the categories are already sorted from bad (low) to good (high)</param>
        public CategoriesList(IEnumerable<TCategory> categoryLimits)
        {
            Categories = CheckCategories(categoryLimits);
        }

        /// <summary>
        /// The list with categories. This list is guaranteed to span the complete probability range between 0 and 1. The categories in this list are ordered from best (low probabilities) to worst (high probabilities).
        /// </summary>
        public TCategory[] Categories { get; }

        /// <summary>
        /// Returns the first category where the upper limit equals or is less then the specified failure probability 
        /// </summary>
        /// <param name="failureProbability">The failure probability that should be translated</param>
        /// <returns></returns>
        public TCategory GetCategoryForFailureProbability(double failureProbability)
        {
            if (double.IsNaN(failureProbability))
            {
                throw new AssemblyException("FailureProbability", EAssemblyErrors.ValueMayNotBeNull);
            }

            if (failureProbability < 0 || failureProbability > 1)
            {
                throw new AssemblyException("FailureProbability", EAssemblyErrors.FailureProbabilityOutOfRange);
            }

            return Categories.First(category => failureProbability <= category.UpperLimit);
        }

        private TCategory[] CheckCategories(IEnumerable<TCategory> categoryLimits)
        {
            var expectedCategoryBoundary = 0.0;

            var categories = categoryLimits as TCategory[] ?? categoryLimits.ToArray();
            foreach (var category in categories)
            {
                if (CompareProbabilities(category.LowerLimit, expectedCategoryBoundary))
                {
                    throw new AssemblyException(
                        "Categories are not subsequent and do not fully cover the probability range",
                        EAssemblyErrors.InvalidCategoryLimits);
                }

                expectedCategoryBoundary = category.UpperLimit;
            }

            if (Math.Abs(expectedCategoryBoundary - 1.0) > EpsilonFactor)
            {
                throw new AssemblyException(
                    "Categories are not subsequent and do not fully cover the probability range",
                    EAssemblyErrors.InvalidCategoryLimits);
            }

            return categories;
        }

        private static bool CompareProbabilities(double firstProbability, double secondprobability)
        {
            var epsilon = Math.Max(firstProbability, secondprobability) * EpsilonFactor;
            return Math.Abs(firstProbability - secondprobability) > epsilon;
        }
    }
}