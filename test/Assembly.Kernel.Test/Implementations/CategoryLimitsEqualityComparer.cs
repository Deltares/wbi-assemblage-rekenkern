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

using System;
using System.Collections;
using Assembly.Kernel.Model.Categories;

namespace Assembly.Kernel.Test.Implementations
{
    /// <summary>
    /// Comparer for <see cref="CategoryLimits{TCategory}"/>.
    /// </summary>
    /// <typeparam name="TCategoryLimits">The type of category limits.</typeparam>
    /// <typeparam name="TCategory">The type of category.</typeparam>
    public class CategoryLimitsEqualityComparer<TCategoryLimits, TCategory> : IComparer
        where TCategoryLimits : CategoryLimits<TCategory>
        where TCategory : struct
    {
        public int Compare(object x, object y)
        {
            return x is TCategoryLimits categoryLimitsX
                   && y is TCategoryLimits categoryLimitsY
                   && categoryLimitsX.LowerLimit.IsNegligibleDifference(categoryLimitsY.LowerLimit)
                   && categoryLimitsX.UpperLimit.IsNegligibleDifference(categoryLimitsY.UpperLimit)
                   && Convert.ToInt32(categoryLimitsX.Category) == Convert.ToInt32(categoryLimitsY.Category)
                       ? 0
                       : 1;
        }
    }
}