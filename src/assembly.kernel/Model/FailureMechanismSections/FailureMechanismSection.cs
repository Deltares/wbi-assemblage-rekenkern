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

using Assembly.Kernel.Exceptions;

namespace Assembly.Kernel.Model.FailureMechanismSections
{
    /// <summary>
    /// Failure mechanism section.
    /// </summary>
    public class FailureMechanismSection
    {
        /// <summary>
        /// Creates a new instance of <see cref="FailureMechanismSection"/>.
        /// </summary>
        /// <param name="start">The start of the section in meters from the beginning of the assessment section.</param>
        /// <param name="end">The end of the section in meters from the beginning of the assessment section.</param>
        /// <exception cref="AssemblyException">Thrown when:
        /// <list type="bullet">
        /// <item><paramref name="start"/> is <see cref="double.NaN"/>;</item>
        /// <item><paramref name="end"/> is <see cref="double.NaN"/>;</item>
        /// <item><paramref name="start"/> &lt; 0;</item>
        /// <item><paramref name="end"/> &lt;= <paramref name="start"/>.</item>
        /// </list>
        /// </exception>
        public FailureMechanismSection(double start, double end)
        {
            if (double.IsNaN(start))
            {
                throw new AssemblyException(nameof(start), EAssemblyErrors.UndefinedProbability);
            }

            if (double.IsNaN(end))
            {
                throw new AssemblyException(nameof(end), EAssemblyErrors.UndefinedProbability);
            }

            if (start < 0.0)
            {
                throw new AssemblyException(nameof(start), EAssemblyErrors.FailureMechanismSectionSectionStartEndInvalid);
            }

            if (end <= start)
            {
                throw new AssemblyException(nameof(end), EAssemblyErrors.FailureMechanismSectionSectionStartEndInvalid);
            }

            Start = start;
            End = end;
            Center = End - (End - Start) / 2.0;
        }

        /// <summary>
        /// Gets the start of the section in meters from the beginning of the assessment section.
        /// </summary>
        public double Start { get; }

        /// <summary>
        /// Gets the end of the section in meters from the beginning of the assessment section.
        /// </summary>
        public double End { get; }

        /// <summary>
        /// Gets the center of the section in meters from the beginning of the assessment section.
        /// </summary>
        public double Center { get; }
    }
}