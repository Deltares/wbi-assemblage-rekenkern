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

using System;
using System.Collections.Generic;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Model.Categories;
using Assembly.Kernel.Model.FailureMechanismSections;

namespace Assembly.Kernel.Interfaces
{
    /// <summary>
    /// Interface to assemble failure mechanism section results of multiple failure mechanisms to 
    /// a greatest denominator section result.
    /// </summary>
    public interface ICommonFailureMechanismSectionAssembler
    {
        /// <summary>
        /// Finds the greatest common denominator sections.
        /// </summary>
        /// <param name="failureMechanismSectionLists">The list of failure mechanism section lists.</param>
        /// <param name="assessmentSectionLength">The total length of the assessment section.</param>
        /// <returns>A <see cref="FailureMechanismSectionList"/> with the greatest common denominator sections.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="failureMechanismSectionLists"/> is <c>null</c>.</exception>
        /// <exception cref="AssemblyException">Thrown when:
        /// <list type="bullet">
        /// <item><paramref name="failureMechanismSectionLists"/> is <c>empty</c>;</item>
        /// <item><paramref name="assessmentSectionLength"/> is <see cref="double.NaN"/> or not &gt; 0;</item>
        /// <item>The sum of the failure mechanism section lengths is not equal to the <paramref name="assessmentSectionLength"/>.</item>
        /// </list>
        /// </exception>
        FailureMechanismSectionList FindGreatestCommonDenominatorSectionsBoi3A1(
            IEnumerable<FailureMechanismSectionList> failureMechanismSectionLists,
            double assessmentSectionLength);

        /// <summary>
        /// Translates the results per failure mechanism section to results per common greatest denominator section.
        /// </summary>
        /// <param name="failureMechanismSections">The list of failure mechanism sections.</param>
        /// <param name="commonSections">The list of common failure mechanism sections.</param>
        /// <returns>A <see cref="FailureMechanismSectionList"/> with the assembly result per common denominator section.</returns>
        /// <exception cref="ArgumentNullException">Thrown when any parameter is <c>null</c>.</exception>
        /// <exception cref="AssemblyException">Thrown when:
        /// <list type="bullet">
        /// <item>The length of the <paramref name="commonSections"/> is not equal to the lenght of the <paramref name="failureMechanismSections"/>;</item>
        /// <item>The elements of <paramref name="failureMechanismSections"/> are not of type <see cref="FailureMechanismSectionWithCategory"/>.</item>
        /// </list>
        /// </exception>
        FailureMechanismSectionList TranslateFailureMechanismResultsToCommonSectionsBoi3B1(
            FailureMechanismSectionList failureMechanismSections,
            FailureMechanismSectionList commonSections);

        /// <summary>
        /// Determines the combined result per common greatest denominator section.
        /// </summary>
        /// <param name="failureMechanismResultsForCommonSections">The list of common section results per failure mechanism.</param>
        /// <param name="partialAssembly">Indicator whether partial assembly is required.</param>
        /// <returns>A list with assembly results per greatest common denominator section.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="failureMechanismResultsForCommonSections"/> is <c>null</c>.</exception>
        /// <exception cref="AssemblyException">Thrown when:
        /// <list type="bullet">
        /// <item><paramref name="failureMechanismResultsForCommonSections"/> is <c>empty</c>;</item>
        /// <item>The elements of <paramref name="failureMechanismResultsForCommonSections"/> are not of type <see cref="FailureMechanismSectionWithCategory"/>;</item>
        /// <item>The elements of <paramref name="failureMechanismResultsForCommonSections"/> do not have equal sections.</item>
        /// </list>
        /// </exception>
        /// <remarks>When <paramref name="partialAssembly"/> is <c>true</c>, categories <see cref="EInterpretationCategory.Dominant"/> and
        /// <see cref="EInterpretationCategory.NoResult"/> are ignored.</remarks>
        IEnumerable<FailureMechanismSectionWithCategory> DetermineCombinedResultPerCommonSectionBoi3C1(
            IEnumerable<FailureMechanismSectionList> failureMechanismResultsForCommonSections, bool partialAssembly);
    }
}