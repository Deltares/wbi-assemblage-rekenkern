#region Copyright (C) Rijkswaterstaat 2019. All rights reserved
// Copyright (C) Rijkswaterstaat 2019. All rights reserved.
//
// This file is part of the Assembly kernel.
//
// Assembly kernel is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>.
//
// All names, logos, and references to "Rijkswaterstaat" are registered trademarks of
// Rijkswaterstaat and remain full property of Rijkswaterstaat at all times.
// All rights reserved.
#endregion

using System.Collections.Generic;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.CategoryLimits;

namespace Assembly.Kernel.Interfaces
{
    /// <summary>
    /// Assemble Failure mechanism assembly results into one AssessmentResult
    /// </summary>
    public interface IAssessmentGradeAssembler
    {
        /// <summary>
        /// Assembles Failure mechanism results without failure probability into one assembly section result.
        /// </summary>
        /// <param name="failureMechanismAssemblyResults">failure mechanism assembly result without failure probability</param>
        /// <param name="partialAssembly">true if this assembly call is for a partial assembly call</param>
        /// <returns>The assembled assessment grade of the assessment section</returns>
        /// <exception cref="AssemblyException">Thrown when input list is null or empty, 
        /// or when a input category is not known</exception>
        EFailureMechanismCategory AssembleAssessmentSectionWbi2A1(
            IEnumerable<FailureMechanismAssemblyResult> failureMechanismAssemblyResults, bool partialAssembly);

        /// <summary>
        /// Assembles Failure mechanism results with failure probability into one assembly section result.
        /// </summary>
        /// <param name="failureMechanismAssemblyResults">failure mechanism assembly result with failure probability</param>
        /// <param name="categories">Categories list that should be used to translate the combined probability of failure to the correct category</param>
        /// <param name="partialAssembly">true if this assembly call is for a partial assembly call</param>
        /// <returns>An assembled assessment section result</returns>
        /// <exception cref="AssemblyException">Thrown when input category requires an failure probability 
        /// but none is provided</exception>
        FailureMechanismAssemblyResult AssembleAssessmentSectionWbi2B1(
            IEnumerable<FailureMechanismAssemblyResult> failureMechanismAssemblyResults,
            CategoriesList<FailureMechanismCategory> categories,
            bool partialAssembly);

        /// <summary>
        /// Assemble and assessment section assembly result with and without a failiure probability into 
        /// one assessment section result.
        /// </summary>
        /// <param name="assemblyResultNoFailureProbability">The assessment section assembly result 
        /// without failure probability. May not be null.</param>
        /// <param name="assemblyResultWithFailureProbability">The assessment section assembly result 
        /// with failure probability. May not be null.</param>
        /// <returns>A copy of the input result with the lowest assessment grade</returns>
        /// <exception cref="AssemblyException">Thrown when one of the inputs are null</exception>
        EAssessmentGrade AssembleAssessmentSectionWbi2C1(
            EFailureMechanismCategory assemblyResultNoFailureProbability,
            FailureMechanismAssemblyResult assemblyResultWithFailureProbability);
    }
}