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

namespace Assembly.Kernel.Acceptance.TestUtil.Data.Input.FailureMechanismSections
{
    /// <summary>
    /// Interface for a failure mechanism section.
    /// </summary>
    public interface IExpectedFailureMechanismSection
    {
        /// <summary>
        /// The start position of the section as a length along the assessment section in meters.
        /// </summary>
        double Start { get; }

        /// <summary>
        /// The end position of the section as a length along the assessment section in meters.
        /// </summary>
        double End { get; }
    }
}