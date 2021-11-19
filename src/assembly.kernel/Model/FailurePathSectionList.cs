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

using System;
using System.Collections.Generic;
using System.Linq;
using Assembly.Kernel.Exceptions;

namespace Assembly.Kernel.Model
{
    /// <summary>
    /// Failure path section categories of a single failure path.
    /// </summary>
    public class FailurePathSectionList
    {
        /// <summary>
        /// Failure path section list constructor.
        /// </summary>
        /// <param name="failurePathId">The failure path to which the section results belong</param>
        /// <param name="sectionResults">The fmSection categories, this list will be sorted by section start</param>
        /// <exception cref="AssemblyException">Thrown when:<br/>- Any of the inputs are null<br/>- The list is empty 
        /// <br/>- The sections aren't consecutive<br/>- Duplicate sections are present<br/>
        ///  - All the sectionResults are of the same type</exception>
        public FailurePathSectionList(string failurePathId,
                                           IEnumerable<FailurePathSection> sectionResults)
        {
            Sections = CheckSectionResults(sectionResults);
            FailurePathId = failurePathId ?? "";
        }

        /// <summary>
        /// The list of failure path section assessment results grouped.
        /// </summary>
        public IEnumerable<FailurePathSection> Sections { get; }

        /// <summary>
        /// The failure path to which the section results belong.
        /// </summary>
        /// TODO: Does this still exist? also for 
        public string FailurePathId { get; }

        /// <summary>
        /// Get the section with category which belongs to the point in the assessment section.
        /// </summary>
        /// <param name="pointInAssessmentSection">The point in the assessment section in meters 
        /// from the beginning of the assessment section</param>
        /// <returns>The section with category belonging to the point in the assessment section</returns>
        public FailurePathSection GetSectionResultForPoint(double pointInAssessmentSection)
        {
            foreach (var section in Sections)
            {
                if (section.SectionEnd >= pointInAssessmentSection)
                {
                    return section;
                }
            }

            throw new AssemblyException("GetSectionResultForPoint", EAssemblyErrors.RequestedPointOutOfRange);
        }

        private static IEnumerable<FailurePathSection> CheckSectionResults(
            IEnumerable<FailurePathSection> sectionResults)
        {
            if (sectionResults == null)
            {
                throw new AssemblyException("FailurePathSectionList", EAssemblyErrors.ValueMayNotBeNull);
            }

            var sectionResultsArray = sectionResults.ToArray();

            if (sectionResultsArray.Length == 0)
            {
                throw new AssemblyException("FailurePathSectionList",
                                            EAssemblyErrors.CommonFailurePathSectionsInvalid);
            }

            // Check if all entries are either direct or indirect, not a combination.
            if (sectionResultsArray.GroupBy(r => r.GetType()).Count() > 1)
            {
                throw new AssemblyException("FailurePathSectionList",
                                            EAssemblyErrors.InputNotTheSameType);
            }

            var orderedResults = sectionResultsArray
                                 .OrderBy(sectionResult => sectionResult.SectionStart)
                                 .ToArray();

            FailurePathSection previousFailurePathSection = null;
            foreach (var section in orderedResults)
            {
                if (previousFailurePathSection == null)
                {
                    // The current section start should be 0 when no previous section is present.
                    if (section.SectionStart > 0.0)
                    {
                        throw new AssemblyException("FailurePathSectionList",
                                                    EAssemblyErrors.CommonFailurePathSectionsInvalid);
                    }
                }
                else
                {
                    // check if sections are consecutive with a margin of 1 cm
                    if (Math.Abs(previousFailurePathSection.SectionEnd - section.SectionStart) > 0.01)
                    {
                        throw new AssemblyException("FailurepathSectionList",
                                                    EAssemblyErrors.CommonFailurePathSectionsNotConsecutive);
                    }
                }

                previousFailurePathSection = section;
            }

            return orderedResults;
        }
    }
}