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

using System.Collections.Generic;
using System.Linq;
using Assembly.Kernel.Exceptions;

namespace Assembly.Kernel.Model.Categories
{
    /// <summary>
    /// List to of categories.
    /// </summary>
    /// <typeparam name="TCategory">The type of category.</typeparam>
    public class CategoriesList<TCategory>
        where TCategory : ICategoryLimits
    {
        /// <summary>
        /// Creates a new instance of <see cref="CategoriesList{TCategory}"/>.
        /// </summary>
        /// <param name="categories">The categories.</param>
        /// <exception cref="AssemblyException">Thrown when:
        /// <list type="bullet">
        /// <item><paramref name="categories"/> is <c>null</c>;</item>
        /// <item>The first category lower limit is not equal to 0.0;</item>
        /// <item>The last category upper limit is not equal to 1.0;</item>
        /// <item>The limits of the categories are not consecutive.</item>
        /// </list>
        /// </exception>
        public CategoriesList(IEnumerable<TCategory> categories)
        {
            ValidateCategories(categories);
            Categories = categories;
        }

        /// <summary>
        /// Gets the categories.
        /// </summary>
        public IEnumerable<TCategory> Categories { get; }

        /// <summary>
        /// Get the category that belongs to the given <paramref name="failureProbability"/>.
        /// </summary>
        /// <param name="failureProbability">The failure probability to get the category for.</param>
        /// <returns>The category based on the <paramref name="failureProbability"/>.</returns>
        /// <exception cref="AssemblyException">Thrown when <paramref name="failureProbability"/>
        /// is <see cref="Probability.Undefined"/>.</exception>
        public TCategory GetCategoryForFailureProbability(Probability failureProbability)
        {
            if (!failureProbability.IsDefined)
            {
                throw new AssemblyException(nameof(failureProbability), EAssemblyErrors.UndefinedProbability);
            }

            return Categories.First(category => failureProbability <= category.UpperLimit);
        }

        /// <summary>
        /// Validates the categories.
        /// </summary>
        /// <param name="categories">The categories to validate.</param>
        /// <exception cref="AssemblyException">Thrown when:
        /// <list type="bullet">
        /// <item><paramref name="categories"/> is <c>null</c>;</item>
        /// <item>The first category lower limit is not equal to 0.0;</item>
        /// <item>The last category upper limit is not equal to 1.0;</item>
        /// <item>The limits of the categories are not consecutive.</item>
        /// </list>
        /// </exception>
        private static void ValidateCategories(IEnumerable<TCategory> categories)
        {
            if (categories == null)
            {
                throw new AssemblyException(nameof(categories), EAssemblyErrors.ValueMayNotBeNull);
            }

            const double epsilon = 1e-10;
            var lastKnownUpperLimit = new Probability(0.0);

            foreach (TCategory category in categories)
            {
                if (!category.LowerLimit.IsNegligibleDifference(lastKnownUpperLimit, epsilon))
                {
                    throw new AssemblyException(nameof(categories), EAssemblyErrors.InvalidCategoryLimits);
                }

                lastKnownUpperLimit = category.UpperLimit;
            }

            if (!lastKnownUpperLimit.IsNegligibleDifference(new Probability(1.0), epsilon))
            {
                throw new AssemblyException(nameof(categories), EAssemblyErrors.InvalidCategoryLimits);
            }
        }
    }
}