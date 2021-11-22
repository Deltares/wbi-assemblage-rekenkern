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

using Assembly.Kernel.Model.AssessmentSection;
using Assembly.Kernel.Model.Categories;

namespace Assembly.Kernel.Interfaces
{
    /// <summary>
    /// Interface for determining category limits.
    /// </summary>
    public interface ICategoryLimitsCalculator
    {
        /// <summary>
        /// Calculate the category limits for an assessment section as specified in WBI-2-1.
        /// </summary>
        /// <param name="section">The assessment section to calculate the category limits for.</param>
        /// <returns>A list of all the categories with their lower and upper limits.</returns>
        CategoriesList<AssessmentSectionCategory> CalculateAssessmentSectionCategoryLimitsWbi21(
            AssessmentSection section);

        /// <summary>
        /// Calculate the interpretation category limits for a section as specified in WBI-0-3.
        /// </summary>
        /// <param name="section">The assessment section to calculate the category limits for.</param>
        /// <returns>A list of all the categories with their lower and upper limits.</returns>
        CategoriesList<InterpretationCategory> CalculateInterpretationCategoryLimitsWbi03(AssessmentSection section);
    }
}