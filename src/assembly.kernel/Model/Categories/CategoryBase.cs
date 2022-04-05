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

using Assembly.Kernel.Exceptions;

namespace Assembly.Kernel.Model.Categories
{
    /// <summary>
    /// Base class for the category limits classes.
    /// </summary>
    /// <typeparam name="T">The category type.</typeparam>
    public abstract class CategoryBase<T> : ICategoryLimits
    {
        /// <summary>
        /// Constructor for <see cref="CategoryBase{T}"/>
        /// </summary>
        /// <param name="category">Category for which the limits are valid.</param>
        /// <param name="lowerLimit">The lower limit of the category.</param>
        /// <param name="upperLimit">The upper limit of the category.</param>
        /// <exception cref="AssemblyException">Thrown when <paramref name="lowerLimit"/> is undefined (<see cref="Probability.Undefined"/> equals true).</exception>
        /// <exception cref="AssemblyException">Thrown when <paramref name="upperLimit"/> is undefined (<see cref="Probability.Undefined"/> equals true).</exception>
        /// <exception cref="AssemblyException">Thrown when <paramref name="lowerLimit"/> &gt; <paramref name="upperLimit"/>.</exception>
        protected CategoryBase(T category, Probability lowerLimit, Probability upperLimit)
        {
            CheckInput(category, lowerLimit, upperLimit);

            Category = category;
            LowerLimit = lowerLimit;
            UpperLimit = upperLimit;
        }

        /// <summary>
        /// The category to which the limits belong.
        /// </summary>
        public T Category { get; }

        /// <summary>
        /// The upper limit of the category.
        /// </summary>
        public Probability UpperLimit { get; }

        /// <summary>
        /// The lower limit of the category.
        /// </summary>
        public Probability LowerLimit { get; }

        private static void CheckInput(T category, Probability lowerLimit, Probability upperLimit)
        {
            if (!lowerLimit.IsDefined || !upperLimit.IsDefined)
            {
                throw new AssemblyException(nameof(CategoryBase<T>), EAssemblyErrors.ProbabilityMayNotBeUndefined);
            }

            if (lowerLimit > upperLimit)
            {
                throw new AssemblyException(nameof(CategoryBase<T>), EAssemblyErrors.LowerLimitIsAboveUpperLimit);
            }
        }
    }
}