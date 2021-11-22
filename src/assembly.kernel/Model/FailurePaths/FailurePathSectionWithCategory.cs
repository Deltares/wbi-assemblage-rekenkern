﻿#region Copyright (C) Rijkswaterstaat 2019. All rights reserved

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

using Assembly.Kernel.Model.Categories;

namespace Assembly.Kernel.Model.FailurePaths
{
    /// <summary>
    /// Failure path with assessment result category.
    /// </summary>
    public class FailurePathSectionWithCategory : FailurePathSection
    {
        /// <summary>
        /// Failure path with category
        /// </summary>
        /// <param name="sectionStart">The start of the section in meters from the beginning of the assessment section.
        ///  Must be greater than 0</param>
        /// <param name="sectionEnd">The end of the section in meters from the beginning of the assessment section.
        ///  Must be greater than 0 and greater than the start of the section</param>
        /// <param name="category">The assessment result of the failure path section</param>
        public FailurePathSectionWithCategory(double sectionStart, double sectionEnd,
            EInterpretationCategory category) :
            base(sectionStart, sectionEnd)
        {
            Category = category;
        }

        /// <summary>
        /// The assessment result of the failure path of this section.
        /// </summary>
        public EInterpretationCategory Category { get; set; }
    }
}