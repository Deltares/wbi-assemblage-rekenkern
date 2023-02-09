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

using System.Collections.Generic;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Model.FailureMechanismSections;

namespace Assembly.Kernel.Interfaces
{
    /// <summary>
    /// Assemble failure mechanism section results of multiple failure mechanisms to 
    /// a greatest denominator section result.
    /// </summary>
    public interface ICommonFailureMechanismSectionAssembler
    {
        /// <summary>
        /// Assemble failure mechanism section results into a greatest common denominator assembly result.
        /// </summary>
        /// <param name="failureMechanismSectionLists">The list of failure mechanism section results 
        /// grouped by failure mechanism.</param>
        /// <param name="assessmentSectionLength">The total length of the assessment section. 
        /// The sum of the section lengths must be equal to this length.</param>
        /// <param name="partialAssembly">True if this assembly call is a partial call.</param>
        /// <returns>The greatest common denominator assembly result.</returns>
        /// <exception cref="AssemblyException">Thrown when the failure mechanism sections aren't consecutive, 
        /// or when the sum of failure mechanism sections is not the same as the total assessment section 
        /// length.</exception>
        /// <exception cref="AssemblyException">Thrown in case <paramref name="failureMechanismSectionLists" /> equals null.</exception>
        /// <exception cref="AssemblyException">Thrown when the sections in <paramref name="failureMechanismSectionLists"/>
        /// are not of type <see cref="FailureMechanismSectionWithCategory"/>.</exception>
        GreatestCommonDenominatorAssemblyResult AssembleCommonFailureMechanismSections(
            IEnumerable<FailureMechanismSectionList> failureMechanismSectionLists, double assessmentSectionLength,
            bool partialAssembly);

        /// <summary>
        /// Find the greatest common denominator sections based on a list of all sections for various failure mechanisms.
        /// </summary>
        /// <param name="failureMechanismSectionLists">A list of all failure mechanism sections for all relevant failure mechanisms.</param>
        /// <param name="assessmentSectionLength">The total length of the assessment section. 
        /// The sum of the section lengths must be equal to this length.</param>
        /// <returns>The greatest common denominator sections spanning the complete assessment section length.</returns>
        /// <exception cref="AssemblyException">Thrown when the failure mechanism sections aren't consecutive, 
        /// or when the sum of failure mechanism sections is not the same as the total assessment section 
        /// length.</exception>
        FailureMechanismSectionList FindGreatestCommonDenominatorSectionsBoi3A1(
            IEnumerable<FailureMechanismSectionList> failureMechanismSectionLists,
            double assessmentSectionLength);

        /// <summary>
        /// Translate the results per section of a failure mechanism to results per common greatest denominator section.
        /// </summary>
        /// <param name="failureMechanismSectionList">This list needs to have also categories.</param>
        /// <param name="commonSections">The of all common failure mechanism sections.</param>
        /// <returns>The assembly result per common denominator section for the specified failure mechanism.</returns>
        /// <exception cref="AssemblyException">Thrown when <paramref name="failureMechanismSectionList"/> equals null.</exception>
        /// <exception cref="AssemblyException">Thrown when <paramref name="commonSections"/> equals null.</exception>
        /// <exception cref="AssemblyException">Thrown when the length of the combined sections in <paramref name="commonSections"/>
        /// does not equal the length of the sections in <paramref name="failureMechanismSectionList"/>.</exception>
        /// <exception cref="AssemblyException">Thrown when the sections in <paramref name="failureMechanismSectionList"/>
        /// are not of type <see cref="FailureMechanismSectionWithCategory"/>.</exception>
        FailureMechanismSectionList TranslateFailureMechanismResultsToCommonSectionsBoi3B1(
            FailureMechanismSectionList failureMechanismSectionList,
            FailureMechanismSectionList commonSections);

        /// <summary>
        /// This method determines the combined result per common greatest denominator section based on a list of results for those
        /// sections per failure mechanism.
        /// </summary>
        /// <param name="failureMechanismResults">The list of results per failure mechanism translated to the greatest denominator
        /// sections across all failure mechanisms. All lists should have equal sections (start and end).</param>
        /// <param name="partialAssembly">True if this assembly call is a partial call.</param>
        /// <returns>The greatest common denominator assembly result.</returns>
        /// <exception cref="AssemblyException">Thrown when the failure mechanism sections lists do not have equal sections.</exception>
        /// <exception cref="AssemblyException">Thrown when none of the sections lists in <paramref name="failureMechanismResults"/> have
        /// sections.</exception>
        /// <exception cref="AssemblyException">Thrown when <paramref name="failureMechanismResults"/> equals null.</exception>
        IEnumerable<FailureMechanismSectionWithCategory> DetermineCombinedResultPerCommonSectionBoi3C1(
            IEnumerable<FailureMechanismSectionList> failureMechanismResults, bool partialAssembly);
    }
}