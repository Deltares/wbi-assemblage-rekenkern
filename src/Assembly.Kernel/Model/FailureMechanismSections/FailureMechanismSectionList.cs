// Copyright (C) Stichting Deltares and State of the Netherlands 2023. All rights reserved.
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
// All names, logos, and references to "Deltares" are registered trademarks of
// Stichting Deltares and remain full property of Stichting Deltares at all times.
// All rights reserved.

using System;
using System.Collections.Generic;
using System.Linq;
using Assembly.Kernel.Exceptions;

namespace Assembly.Kernel.Model.FailureMechanismSections
{
    /// <summary>
    /// Failure mechanism section list.
    /// </summary>
    public class FailureMechanismSectionList
    {
        /// <summary>
        /// Creates a new instance of <see cref="FailureMechanismSectionList"/>.
        /// </summary>
        /// <param name="sections">The failure mechanism sections.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="sections"/> is <c>null</c>.</exception>
        /// <exception cref="AssemblyException">Thrown when:
        /// <list type="bullet">
        /// <item><paramref name="sections"/> is <c>empty</c>;</item>
        /// <item><paramref name="sections"/> contains a mix of different types;</item>
        /// <item>The first section start is not equal to 0.0;</item>
        /// <item>The sections are not consecutive.</item>
        /// </list>
        /// </exception>
        public FailureMechanismSectionList(IEnumerable<FailureMechanismSection> sections)
        {
            if (sections == null)
            {
                throw new ArgumentNullException(nameof(sections));
            }

            ValidateSections(sections);
            Sections = sections;
        }

        /// <summary>
        /// Gets the sections.
        /// </summary>
        public IEnumerable<FailureMechanismSection> Sections { get; }

        /// <summary>
        /// Get the section that belongs to the given <paramref name="pointInAssessmentSection"/>.
        /// </summary>
        /// <param name="pointInAssessmentSection">The point in the assessment section in meters 
        /// from the beginning of the assessment section.</param>
        /// <returns>The section with category belonging to the point in the assessment section.</returns>
        /// <exception cref="AssemblyException">Thrown when <paramref name="pointInAssessmentSection"/>
        /// &gt; the end of the last section.</exception>
        public FailureMechanismSection GetSectionAtPoint(double pointInAssessmentSection)
        {
            FailureMechanismSection section = Sections.FirstOrDefault(s => s.End >= pointInAssessmentSection);

            if (section == null)
            {
                throw new AssemblyException(nameof(pointInAssessmentSection), EAssemblyErrors.RequestedPointOutOfRange);
            }

            return section;
        }

        /// <summary>
        /// Validates the sections.
        /// </summary>
        /// <param name="sections">The failure mechanism sections.</param>
        /// <exception cref="AssemblyException">Thrown when:
        /// <list type="bullet">
        /// <item><paramref name="sections"/> is <c>empty</c>;</item>
        /// <item><paramref name="sections"/> contains a mix of different types;</item>
        /// <item>The first section start is not equal to 0.0;</item>
        /// <item>The sections are not consecutive.</item>
        /// </list>
        /// </exception>
        private static void ValidateSections(IEnumerable<FailureMechanismSection> sections)
        {
            if (!sections.Any())
            {
                throw new AssemblyException(nameof(sections), EAssemblyErrors.CommonFailureMechanismSectionsInvalid);
            }

            if (sections.GroupBy(r => r.GetType()).Count() > 1)
            {
                throw new AssemblyException(nameof(sections), EAssemblyErrors.InputNotTheSameType);
            }

            const double threshold = 0.01;

            FailureMechanismSection firstSection = sections.First();
            if (Math.Abs(0.0 - firstSection.Start) > threshold)
            {
                throw new AssemblyException(nameof(sections), EAssemblyErrors.CommonFailureMechanismSectionsInvalid);
            }

            double previousSectionEnd = firstSection.End;
            foreach (FailureMechanismSection section in sections.Skip(1))
            {
                if (Math.Abs(previousSectionEnd - section.Start) > threshold)
                {
                    throw new AssemblyException(nameof(sections), EAssemblyErrors.CommonFailureMechanismSectionsNotConsecutive);
                }

                previousSectionEnd = section.End;
            }
        }
    }
}