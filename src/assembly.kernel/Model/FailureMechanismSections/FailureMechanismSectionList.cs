﻿#region Copyright (C) Rijkswaterstaat 2022. All rights reserved

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
        /// <param name="sectionResults">The interpretation categories, this list will be sorted by section start</param>
        /// <exception cref="AssemblyException">Thrown when:<br/>- Any of the inputs are null<br/>- The list is empty 
        /// <br/>- The sections aren't consecutive<br/>- Duplicate sections are present<br/>
        ///  - All the sectionResults are of the same type</exception>
        public FailureMechanismSectionList(IEnumerable<FailureMechanismSection> sectionResults)
        {
            Sections = CheckSectionResults(sectionResults);
        }

        /// <summary>
        /// The list of failure mechanism section assessment results grouped.
        /// </summary>
        public IEnumerable<FailureMechanismSection> Sections { get; }

        /// <summary>
        /// Get the section with category which belongs to the point in the assessment section.
        /// </summary>
        /// <param name="pointInAssessmentSection">The point in the assessment section in meters 
        /// from the beginning of the assessment section</param>
        /// <returns>The section with category belonging to the point in the assessment section</returns>
        public FailureMechanismSection GetSectionAtPoint(double pointInAssessmentSection)
        {
            foreach (var section in Sections)
            {
                if (section.End >= pointInAssessmentSection)
                {
                    return section;
                }
            }

            throw new AssemblyException(nameof(pointInAssessmentSection), EAssemblyErrors.RequestedPointOutOfRange);
        }

        private static IEnumerable<FailureMechanismSection> CheckSectionResults(
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
                    // The current section start should be 0 when no previous section is present.
                    if (section.Start > 0.0)
                    {
                        throw new AssemblyException(nameof(sectionResults),
                            EAssemblyErrors.CommonFailureMechanismSectionsInvalid);
                    }
                }
                else
                {
                    // check if sections are consecutive with a margin of 1 cm
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