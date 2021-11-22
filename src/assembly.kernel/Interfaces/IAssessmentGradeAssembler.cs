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

using System.Collections.Generic;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Model.AssessmentSection;
using Assembly.Kernel.Model.Categories;
using Assembly.Kernel.Model.FailurePaths;

namespace Assembly.Kernel.Interfaces
{
    /// <summary>
    /// Assemble Failure path assembly results into one AssessmentResult
    /// </summary>
    public interface IAssessmentGradeAssembler
    {
        /// <summary>
        /// Assembles Failure path results with failure probability into one assembly section result.
        /// </summary>
        /// <param name="failurePathProbabilities">failure path assembly result with failure probability</param>
        /// <param name="categories">Categories list that should be used to translate the combined probability of failure to the correct category</param>
        /// <param name="partialAssembly">true if this assembly call is for a partial assembly call</param>
        /// <returns>An assembled assessment section result</returns>
        /// <exception cref="AssemblyException">Thrown when input category requires an failure probability 
        /// but none is provided</exception>
        AssessmentSectionResult AssembleAssessmentSectionWbi2B1(
            IEnumerable<FailurePathAssemblyResult> failurePathProbabilities,
            CategoriesList<AssessmentSectionCategory> categories,
            bool partialAssembly);
    }
}