#region Copyright (c) 2018 Technolution BV. All Rights Reserved. 
// // Copyright (C) Technolution BV. 2018. All rights reserved.
// //
// // This file is part of the Assembly kernel.
// //
// // Assembly kernel is free software: you can redistribute it and/or modify
// // it under the terms of the GNU Lesser General Public License as published by
// // the Free Software Foundation, either version 3 of the License, or
// // (at your option) any later version.
// // 
// // This program is distributed in the hope that it will be useful,
// // but WITHOUT ANY WARRANTY; without even the implied warranty of
// // MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// // GNU Lesser General Public License for more details.
// //
// // You should have received a copy of the GNU Lesser General Public License
// // along with this program. If not, see <http://www.gnu.org/licenses/>.
// //
// // All names, logos, and references to "Technolution BV" are registered trademarks of
// // Technolution BV and remain full property of Technolution BV at all times.
// // All rights reserved.
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using Assembly.Kernel.Exceptions;

namespace Assembly.Kernel.Model {
    /// <summary>
    /// Failure mechanism section categories of a single failure mechanism.
    /// </summary>
    public class FailureMechanismSectionList {
        /// <summary>
        /// The list of failure mechanism section asesssment results grouped.
        /// </summary>
        public List<FmSectionWithCategory> Results { get; }
        /// <summary>
        /// The failure mechanism to which the section results belong.
        /// </summary>
        public string FailureMechanismId { get; }

        /// <summary>
        /// True if assessment results in this object are of an indirect failure mechanism.
        /// </summary>
        public bool IsIndirectFailureMechanism { get;  }

        /// <summary>
        /// Failure mechanism section list constructor.
        /// </summary>
        /// <param name="failureMechanismId">The failure mechanism to which the section results belong</param>
        /// <param name="sectionResults">The fmSection categories, this list will be sorted by section start</param>
        /// <exception cref="AssemblyException">Thrown when:<br/>- Any of the inputs are null<br/>- The list is empty 
        /// <br/>- The sections aren't consecutive<br/>- Duplicate sections are present<br/>
        ///  - All the sectionResults are of the same type</exception>
        public FailureMechanismSectionList(string failureMechanismId,
            IEnumerable<FmSectionWithCategory> sectionResults) {
            if (failureMechanismId == null || sectionResults == null) {
                throw new AssemblyException("FailureMechanismSectionList", EAssemblyErrors.ValueMayNotBeNull);
            }

            // order the section results by start of the section.
            Results = sectionResults.OrderBy(sectionResult => sectionResult.SectionStart).ToList();

            FmSectionWithCategory previousSection = null;
            foreach (var section in Results) {
                if (previousSection == null) {
                    // The current section start should be 0 when no previous section is present.
                    if (section.SectionStart > 0.0) {
                        throw new AssemblyException("FailureMechanismSectionList",
                            EAssemblyErrors.CommonFailureMechanismSectionsInvalid);
                    }
                } else {
                    // Check for duplicate section starts
                    if (Math.Abs(section.SectionStart - previousSection.SectionStart) < 0.01) {
                        throw new AssemblyException("FailuremechanismSectionList", 
                            EAssemblyErrors.FailureMechanismDuplicateSection);
                    }

                    // check if sections are consecutive with a margin of 1 cm
                    if (Math.Abs(previousSection.SectionEnd - section.SectionStart) > 0.01) {
                        throw new AssemblyException("FailuremechanismSectionList",
                            EAssemblyErrors.CommonFailureMechanismSectionsNotConsecutive);
                    }
                }

                previousSection = section;
            }
            
            if (Results.Count == 0) {
                throw new AssemblyException("FailureMechanismSectionList", 
                    EAssemblyErrors.CommonFailureMechanismSectionsInvalid);
            }

            // Check if all entries are either direct or indirect, not a combination.
            if (Results.GroupBy(fmResult => fmResult.Type).Count() > 1) {
                throw new AssemblyException("FailureMechanismSectionList", 
                    EAssemblyErrors.InputNotTheSameType);
            }

            // Are the results of an indirect failure mechanism.
            IsIndirectFailureMechanism = Results[0].Type == EAssembledAssessmentResultType.IndirectAssessment;

            FailureMechanismId = failureMechanismId;
        }

        /// <summary>
        /// Get the section with category which belongs to the point in the assessment section.
        /// </summary>
        /// <param name="pointInAssessmentSection">The point in the assessment section in meters 
        /// from the beginning of the assessment section</param>
        /// <returns>The section with category belonging to the point in the assessment section</returns>
        public FmSectionWithCategory GetSectionCategoryForPoint(double pointInAssessmentSection) {
            foreach (var section in Results) {
                if (section.SectionEnd >= pointInAssessmentSection) {
                    return section;
                }
            }
            throw new AssemblyException("GetSectionCategoryForPoint", EAssemblyErrors.RequestedPointOutOfRange);
        }
    }
}