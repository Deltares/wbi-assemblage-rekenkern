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

namespace Assembly.Kernel.Model
{
    /// <summary>
    /// Enum that indicates the status of the refined probability per section.
    /// </summary>
    public enum ERefinementStatus
    {
        /// <summary>
        /// No further refinement is necessary. Probability estimation based on the initial mechanism is sufficient.
        /// </summary>
        NotNecessary = 0,

        /// <summary>
        /// Further refinement is necessary. Probability estimation based on the initial mechanism is not sufficient.
        /// </summary>
        Necessary = 1,

        /// <summary>
        /// Further refinement is already performed. This means the refined probabilities are also specified as input.
        /// </summary>
        Performed = 2
    }
}