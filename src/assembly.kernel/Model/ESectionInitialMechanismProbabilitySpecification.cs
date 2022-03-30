#region Copyright (C) Rijkswaterstaat 2022. All rights reserved

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

namespace Assembly.Kernel.Model
{
    /// <summary>
    /// Specifies whether the failure mechanism for a specific section is relevant and if so,
    /// whether the probability of occurrence for the section is specified.
    /// </summary>
    public enum ESectionInitialMechanismProbabilitySpecification
    {
        /// <summary>
        /// Mechanism is not relevant for this section.
        /// </summary>
        NotRelevant = 1,

        /// <summary>
        /// Mechanism is relevant, but no probability specification is provided.
        /// </summary>
        RelevantNoProbabilitySpecification = 2,

        /// <summary>
        /// Mechanism is relevant and a probability specification is provided.
        /// </summary>
        RelevantWithProbabilitySpecification = 3
    }
}
