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

using Assembly.Kernel.Model;
using Assembly.Kernel.Model.CategoryLimits;

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
        /// Calculate the category limits for a failure mechanism as specified in WBI-1-1.
        /// </summary>
        /// <param name="section">The assessment section information.</param>
        /// <param name="failureMechanism">The failure mechanism to calculate the category limits for.</param>
        /// <returns>A list of all the categories with their lower and upper limits.</returns>
        CategoriesList<FailureMechanismCategory> CalculateFailureMechanismCategoryLimitsWbi11(AssessmentSection section,
                                                                                              FailureMechanism failureMechanism);

        /// <summary>
        /// Calculate the category limits for a failure mechanism section as specified in WBI-0-1.
        /// </summary>
        /// <param name="section">The assessment section information.</param>
        /// <param name="failureMechanism">The failure mechanism information.</param>
        /// <returns>A list of all the categories with their lower and upper limits.</returns>
        CategoriesList<FmSectionCategory> CalculateFmSectionCategoryLimitsWbi01(AssessmentSection section,
                                                                                FailureMechanism failureMechanism);

        /// <summary>
        /// Calculate the category limits for a section of failure mechanism STBU as specified in WBI-0-2.
        /// </summary>
        /// <param name="assessmentSectionNorm">The norm of the assessment section.</param>
        /// <param name="failureMechanism">The failure mechanism information.</param>
        /// <returns>A list of all the categories with their lower and upper limits.</returns>
        CategoriesList<FmSectionCategory> CalculateFmSectionCategoryLimitsWbi02(double assessmentSectionNorm,
                                                                                FailureMechanism failureMechanism);
    }
}