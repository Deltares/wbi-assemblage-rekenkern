﻿// Copyright (C) Stichting Deltares and State of the Netherlands 2023. All rights reserved.
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

using Assembly.Kernel.Acceptance.TestUtil.Data.Input.FailureMechanismSections;

namespace Assembly.Kernel.Acceptance.TestUtil.IO.FailureMechanismSection
{
    /// <summary>
    /// Interface for a section reader.
    /// </summary>
    /// <typeparam name="TFailureMechanismSection">The type of failure mechanism section.</typeparam>
    public interface ISectionReader<out TFailureMechanismSection>
        where TFailureMechanismSection : IExpectedFailureMechanismSection
    {
        /// <summary>
        /// Reads the specified section.
        /// </summary>
        /// <param name="iRow">Row number in the excel sheet of the section that needs to be read.</param>
        /// <param name="startMeters">Start position of the section (already read).</param>
        /// <param name="endMeters">End position of the section (already read).</param>
        /// <returns>The read <see cref="TFailureMechanismSection"/>.</returns>
        TFailureMechanismSection ReadSection(int iRow, double startMeters, double endMeters);
    }
}