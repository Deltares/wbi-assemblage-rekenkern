﻿// Copyright (C) Stichting Deltares and State of the Netherlands 2023. All rights reserved.
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

namespace Assembly.Kernel.Model.Categories
{
    /// <summary>
    /// Base class for the category limits classes.
    /// </summary>
    /// <typeparam name="T">The type of category.</typeparam>
    public abstract class CategoryLimits<T> : IHasBoundaryLimits
    {
        /// <summary>
        /// Creates a new instance of <see cref="CategoryLimits{T}"/>
        /// </summary>
        /// <param name="category">Category to witch the limits belong.</param>
        /// <param name="lowerLimit">The lower limit of the category.</param>
        /// <param name="upperLimit">The upper limit of the category.</param>
        /// <exception cref="AssemblyException">Thrown when:
        /// <list type="bullet">
        /// <item><paramref name="lowerLimit"/> is <see cref="Probability.Undefined"/>;</item>
        /// <item><paramref name="upperLimit"/> is <see cref="Probability.Undefined"/>;</item>
        /// <item><paramref name="lowerLimit"/> &gt; <paramref name="upperLimit"/></item>.
        /// </list>
        /// </exception>
        protected CategoryLimits(T category, Probability lowerLimit, Probability upperLimit)
        {
            ValidateLimits(lowerLimit, upperLimit);

            Category = category;
            LowerLimit = lowerLimit;
            UpperLimit = upperLimit;
        }

        /// <summary>
        /// Gets the category to which the limits belong.
        /// </summary>
        public T Category { get; }

        /// <inheritdoc />
        public Probability LowerLimit { get; }

        /// <inheritdoc />
        public Probability UpperLimit { get; }

        /// <summary>
        /// Validates the category limits.
        /// </summary>
        /// <param name="lowerLimit">The lower limit of the category.</param>
        /// <param name="upperLimit">The upper limit of the category.</param>
        /// <exception cref="AssemblyException">Thrown when:
        /// <list type="bullet">
        /// <item><paramref name="lowerLimit"/> is <see cref="Probability.Undefined"/>;</item>
        /// <item><paramref name="upperLimit"/> is <see cref="Probability.Undefined"/>;</item>
        /// <item><paramref name="lowerLimit"/> &gt; <paramref name="upperLimit"/></item>.
        /// </list>
        /// </exception>
        private static void ValidateLimits(Probability lowerLimit, Probability upperLimit)
        {
            if (!lowerLimit.IsDefined)
            {
                throw new AssemblyException(nameof(lowerLimit), EAssemblyErrors.UndefinedProbability);
            }

            if (!upperLimit.IsDefined)
            {
                throw new AssemblyException(nameof(upperLimit), EAssemblyErrors.UndefinedProbability);
            }

            if (lowerLimit > upperLimit)
            {
                throw new AssemblyException(nameof(lowerLimit), EAssemblyErrors.LowerLimitIsAboveUpperLimit);
            }
        }
    }
}