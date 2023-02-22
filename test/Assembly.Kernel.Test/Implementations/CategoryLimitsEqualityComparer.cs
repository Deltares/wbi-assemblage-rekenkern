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

using System.Collections;
using Assembly.Kernel.Model.Categories;

namespace Assembly.Kernel.Test.Implementations
{
    /// <summary>
    /// Comparer for <see cref="ICategoryLimits"/>.
    /// </summary>
    public class CategoryLimitsEqualityComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            return x is ICategoryLimits categoryLimitsX
                   && y is ICategoryLimits categoryLimitsY
                   && categoryLimitsX.LowerLimit.IsNegligibleDifference(categoryLimitsY.LowerLimit)
                   && categoryLimitsX.UpperLimit.IsNegligibleDifference(categoryLimitsY.UpperLimit)
                       ? 0
                       : 1;
        }
    }
}