#region Copyright (C) Rijkswaterstaat 2022. All rights reserved.

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

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using Assembly.Kernel.Exceptions;

namespace Assembly.Kernel.Model.FailureMechanismSections
{
    /// <summary>
    /// Failure mechanism section categories of a single failure mechanism.
    /// </summary>
    public class FailureMechanismSectionList
    {
        /// <summary>
        /// Failure mechanism section list constructor.
        /// </summary>
        /// <param name="sectionResults">The interpretation categories, this list will be sorted by section start.</param>
        /// <exception cref="AssemblyException">Thrown when <paramref name="sectionResults"/> equals null.</exception>
        /// <exception cref="AssemblyException">Thrown when <paramref name="sectionResults"/> is empty.</exception>
        /// <exception cref="AssemblyException">Thrown when <paramref name="sectionResults"/> contains a mix of results of different type.</exception>
        /// <exception cref="AssemblyException">Thrown when <seealso cref="FailureMechanismSection.Start"/> of the first section does not equal 0.0.</exception>
        /// <exception cref="AssemblyException">Thrown when the sections in <paramref name="sectionResults"/> are consecutive
        /// (<seealso cref="FailureMechanismSection.Start"/> of a section equals <seealso cref="FailureMechanismSection.End"/> of the previous section.</exception>
        public FailureMechanismSectionList(IEnumerable<FailureMechanismSection> sectionResults)
        {
            Sections = OrderAndCheckSectionResults(sectionResults);
        }

        /// <summary>
        /// The list of failure mechanism section assessment results grouped.
        /// </summary>
        public IEnumerable<FailureMechanismSection> Sections { get; }

        /// <summary>
        /// Get the section with category which belongs to the point in the assessment section.
        /// </summary>
        /// <param name="pointInAssessmentSection">The point in the assessment section in meters 
        /// from the beginning of the assessment section.</param>
        /// <exception cref="AssemblyException">Thrown when the requested point in the assessment section is $gt; the end of the last section.</exception>
        /// <returns>The section with category belonging to the point in the assessment section.</returns>
        public FailureMechanismSection GetSectionAtPoint(double pointInAssessmentSection)
        {
            var section = Sections.FirstOrDefault(s => s.End >= pointInAssessmentSection);

            if (section == null)
            {
                throw new AssemblyException(nameof(pointInAssessmentSection), EAssemblyErrors.RequestedPointOutOfRange);
            }

            return section;
        }

        private static IEnumerable<FailureMechanismSection> OrderAndCheckSectionResults(
            IEnumerable<FailureMechanismSection> sectionResults)
        {
            if (sectionResults == null)
            {
                throw new AssemblyException(nameof(sectionResults), EAssemblyErrors.ValueMayNotBeNull);
            }

            var sectionResultsArray = sectionResults.ToArray();

            if (sectionResultsArray.Length == 0)
            {
                throw new AssemblyException(nameof(sectionResults),
                    EAssemblyErrors.CommonFailureMechanismSectionsInvalid);
            }

            // Check if all entries are of the same type.
            if (sectionResultsArray.GroupBy(r => r.GetType()).Count() > 1)
            {
                throw new AssemblyException(nameof(sectionResults),
                    EAssemblyErrors.InputNotTheSameType);
            }

            var orderedResults = sectionResultsArray
                .OrderBy(sectionResult => sectionResult.Start)
                .ToArray();

            FailureMechanismSection previousFailureMechanismSection = null;
            foreach (var section in orderedResults)
            {
                if (previousFailureMechanismSection == null)
                {
                    if (section.Start > 0.0)
                    {
                        throw new AssemblyException(nameof(sectionResults),
                            EAssemblyErrors.CommonFailureMechanismSectionsInvalid);
                    }
                }
                else
                {
                    if (Math.Abs(previousFailureMechanismSection.End - section.Start) > 0.01)
                    {
                        throw new AssemblyException(nameof(sectionResults),
                            EAssemblyErrors.CommonFailureMechanismSectionsNotConsecutive);
                    }
                }

                previousFailureMechanismSection = section;
            }

            return orderedResults;
        }
    }
}