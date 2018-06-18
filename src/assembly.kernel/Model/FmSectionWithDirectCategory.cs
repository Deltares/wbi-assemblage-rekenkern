#region Copyright (c) 2018 Technolution BV. All Rights Reserved. 

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

namespace Assembly.Kernel.Model
{
    /// <summary>
    /// Direct failure mechanism with assessment result category.
    /// </summary>
    public class FmSectionWithDirectCategory : FmSectionWithCategory
    {
        /// <summary>
        /// Indirect failure mechanism with category
        /// </summary>
        /// <param name="sectionStart">The start of the section in meters from the beginning of the assessment section.
        ///  Must be greater than 0</param>
        /// <param name="sectionEnd">The end of the section in meters from the beginning of the assessment section.
        ///  Must be greater than 0 and greater than the start of the section</param>
        /// <param name="category">The assessment result of the failure mechanism section</param>
        public FmSectionWithDirectCategory(double sectionStart, double sectionEnd, EFmSectionCategory category) :
            base(sectionStart, sectionEnd, EAssembledAssessmentResultType.AssessmentCategoryWithoutFailureProbability)
        {
            Category = category;
        }

        /// <summary>
        /// The assessment result of the direct failure mechanism of this section.
        /// </summary>
        public EFmSectionCategory Category { get; set; }
    }
}