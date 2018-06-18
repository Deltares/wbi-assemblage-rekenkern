﻿#region Copyright (c) 2018 Technolution BV. All Rights Reserved. 

// // Copyright (C) Technolution BV. 2018. All rights reserved.
// //
// // This file is part of the Assembly kernel.
// //
// // Assembly kernel is free software: you can redistribute it and/or modify
// // it under the terms of the GNU Lesser General Public License as published by
// // the Free Software Foundation, either version 3 of the License, or
// // (at your option) any later version.
// // 
// // This program is distributed in the hope that it will be useful,
// // but WITHOUT ANY WARRANTY; without even the implied warranty of
// // MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// // GNU Lesser General Public License for more details.
// //
// // You should have received a copy of the GNU Lesser General Public License
// // along with this program. If not, see <http://www.gnu.org/licenses/>.
// //
// // All names, logos, and references to "Technolution BV" are registered trademarks of
// // Technolution BV and remain full property of Technolution BV at all times.
// // All rights reserved.

#endregion

using Assembly.Kernel.Model.FmSectionTypes;

namespace Assembly.Kernel.Model.CategoryLimits
{
    /// <inheritdoc />
    /// <summary>
    /// Category limits for an failure mechanism section
    /// </summary>
    public class FmSectionCategoryLimits : BaseCategoryLimits<EFmSectionCategory>
    {
        /// <summary>
        /// FmSectionCategoryLimits constructor
        /// </summary>
        /// <param name="category">category for which the limits are valid</param>
        /// <param name="lowerLimit">lower limit of the category</param>
        /// <param name="upperLimit">upper limit of the category</param>
        public FmSectionCategoryLimits(EFmSectionCategory category, double lowerLimit, double upperLimit) :
            base(category, lowerLimit, upperLimit)
        {
            // Construct super class
        }
    }
}