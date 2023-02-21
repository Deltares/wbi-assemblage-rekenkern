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

using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.Categories;

namespace Assembly.Kernel.Interfaces
{
    /// <summary>
    /// Interface for calculating category limits.
    /// </summary>
    public interface ICategoryLimitsCalculator
    {
        /// <summary>
        /// Calculate the interpretation category limits for an assessment section.
        /// </summary>
        /// <param name="assessmentSection">The assessment section to calculate the category limits for.</param>
        /// <returns>A <see cref="CategoriesList{TCategory}"/> of <see cref="InterpretationCategory"/>
        /// based on the maximum allowable flooding probability and the signal flooding probability.</returns>
        /// <exception cref="AssemblyException">Thrown when <paramref name="assessmentSection"/> is <c>null</c>.</exception>
        CategoriesList<InterpretationCategory> CalculateInterpretationCategoryLimitsBoi01(AssessmentSection assessmentSection);
        
        /// <summary>
        /// Calculate the category limits for an assessment section.
        /// </summary>
        /// <param name="assessmentSection">The assessment section to calculate the category limits for.</param>
        /// <returns>A <see cref="CategoriesList{TCategory}"/> of <see cref="AssessmentSectionCategory"/>
        /// based on the maximum allowable flooding probability and the signal flooding probability.</returns>
        /// <exception cref="AssemblyException">Thrown when <paramref name="assessmentSection"/> is <c>null</c>.</exception>
        CategoriesList<AssessmentSectionCategory> CalculateAssessmentSectionCategoryLimitsBoi21(AssessmentSection assessmentSection);
    }
}