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

using Assembly.Kernel.Interfaces;

namespace Assembly.Kernel.Model
{
    /// <summary>
    /// Enum that specifies the followed calculation method in <seealso cref="IFailureMechanismResultAssembler.CalculateFailureMechanismFailureProbabilityBoi1A1"/>
    /// to combine probabilities of failure mechanism sections to a failure probability of the failure mechanism.
    /// </summary>
    public enum EFailureMechanismAssemblyMethod
    {
        /// <summary>
        /// Method for fully correlated probabilities was used (maximum of the probabilities times the length effect).
        /// </summary>
        Correlated = 1,

        /// <summary>
        /// Method for fully uncorrelated probabilities was used (product of all 1-probabilities of failure).
        /// </summary>
        Uncorrelated = 2
    }
}