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
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.AssessmentSection;
using Assembly.Kernel.Model.FailurePaths;

namespace Assembly.Kernel.Interfaces
{
    /// <summary>
    /// Assemble failure path section results of multiple failure paths to 
    /// a greatest denominator section result.
    /// </summary>
    public interface ICommonFailurePathSectionAssembler
    {
        /// <summary>
        /// Assemble failure path section results into a greatest common denominator assembly result.
        /// </summary>
        /// <param name="failurePathSectionLists">The list of failure path section results 
        /// grouped by failure path.</param>
        /// <param name="assessmentSectionLength">The total length of the assessment section. 
        /// The sum of the section lengths must be equal to this length.</param>
        /// <param name="partialAssembly">True if this assembly call is a partial call.</param>
        /// <returns>The greatest common denominator assembly result.</returns>
        /// <exception cref="AssemblyException">Thrown when the failure path sections aren't consecutive, 
        /// or when the sum of the failure path sections is not the same as the total assessment section 
        /// length.</exception>
        AssemblyResult AssembleCommonFailurePathSections(
            IEnumerable<FailurePathSectionList> failurePathSectionLists, double assessmentSectionLength,
            bool partialAssembly);

        /// <summary>
        /// Find the greatest common denominator sections based on a list of all sections for various failure paths.
        /// </summary>
        /// <param name="failurePathSectionLists">A list of all failure path sections for all relevant failure paths.</param>
        /// <param name="assessmentSectionLength">The total length of the assessment section. 
        /// The sum of the section lengths must be equal to this length.</param>
        /// <returns>The greatest common denominator sections spanning the complete assessment section length.</returns>
        /// <exception cref="AssemblyException">Thrown when the failure path sections aren't consecutive, 
        /// or when the sum of the failure path sections is not the same as the total assessment section 
        /// length.</exception>
        FailurePathSectionList FindGreatestCommonDenominatorSectionsWbi3A1(
            IEnumerable<FailurePathSectionList> failurePathSectionLists,
            double assessmentSectionLength);

        /// <summary>
        /// Translate the results per section of a failure path to results per common greatest denominator section.
        /// </summary>
        /// <param name="failurePathSectionList">This list needs to have also categories.</param>
        /// <param name="commonSections">The of all common failure path sections.</param>
        /// <returns>The assembly result per common denominator section for the specified failure path.</returns>
        FailurePathSectionList TranslateFailurePathResultsToCommonSectionsWbi3B1(
            FailurePathSectionList failurePathSectionList,
            FailurePathSectionList commonSections);

        /// <summary>
        /// This method determines the combined result per common greatest denominator section based on a list of results for those
        /// sections per failure path.
        /// </summary>
        /// <param name="failurePathResults">The list of results per failure path translated to the greatest denominator
        /// sections across all failure paths. All lists should have equal sections (start and end).</param>
        /// <param name="partialAssembly">True if this assembly call is a partial call.</param>
        /// <returns>The greatest common denominator assembly result.</returns>
        /// <exception cref="AssemblyException">Thrown when the failure path sections lists do not have equal sections.</exception>
        IEnumerable<FailurePathSectionWithCategory> DetermineCombinedResultPerCommonSectionWbi3C1(
            IEnumerable<FailurePathSectionList> failurePathResults, bool partialAssembly);
    }
}