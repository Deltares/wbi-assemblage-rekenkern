#region Copyright (C) Rijkswaterstaat 2022. All rights reserved

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

using System.Collections.Generic;
using System.Linq;
using Assembly.Kernel.Exceptions;

namespace Assembly.Kernel.Model.Categories
{
    /// <summary>
    /// This object is used to obtain a category from a list of categories for a given probability.
    /// </summary>
    public class CategoriesList<TCategory> where TCategory : ICategoryLimits
    {
        private readonly Probability requiredMaximumProbability = new Probability(1.0);

        /// <summary>
        /// The maximum allowed difference between reliabilities of probabilities that is used when comparing category boundaries. Gaps between category boundaries smaller than Epsilon will not be taken into account.
        /// </summary>
        private static readonly double Epsilon = 1e-10;

        /// <summary>
        /// This constructor validates a list of category limits and assigns the correct list to the Categories property.
        /// </summary>
        /// <param name="categoryLimits">An IEnumerable with categories. This list assumes the categories are already sorted from bad (low) to good (high)</param>
        public CategoriesList(IEnumerable<TCategory> categoryLimits)
        {
            var categories = categoryLimits as TCategory[] ?? categoryLimits.ToArray();
            CheckCategories(categories);
            Categories = categories;
        }

        /// <summary>
        /// The list with categories. This list is guaranteed to span the complete probability range between 0 and 1.
        /// The categories in this list are ordered from best (low probabilities) to worst (high probabilities).
        /// </summary>
        public TCategory[] Categories { get; }

        /// <summary>
        /// Returns the first category where the upper limit equals or is less then the specified failure probability.
        /// </summary>
        /// <param name="failureProbability">The failure probability that should be translated.</param>
        /// <returns>The category based on the <paramref name="failureProbability"/>.</returns>
        public TCategory GetCategoryForFailureProbability(Probability failureProbability)
        {
            if (!failureProbability.IsDefined)
            {
                throw new AssemblyException(nameof(failureProbability), EAssemblyErrors.ProbabilityMayNotBeUndefined);
            }

            return Categories.First(category => failureProbability <= category.UpperLimit);
        }

        private void CheckCategories(TCategory[] categories)
        {
            Probability lastKnownUpperBoundary = (Probability) 0.0;

            foreach (var category in categories)
            {
                if (category.LowerLimit.IsNegligibleDifference(lastKnownUpperBoundary, Epsilon))
                {
                    throw new AssemblyException(nameof(categories), EAssemblyErrors.InvalidCategoryLimits);
                }

                lastKnownUpperBoundary = category.UpperLimit;
            }

            if (lastKnownUpperBoundary.IsNegligibleDifference(requiredMaximumProbability,Epsilon))
            {
                throw new AssemblyException(nameof(categories), EAssemblyErrors.InvalidCategoryLimits);
            }
        }
    }
}