#region Copyright (C) Rijkswaterstaat 2019. All rights reserved
// Copyright (C) Rijkswaterstaat 2019. All rights reserved.
//
// This file is part of the Assembly kernel.
//
// Assembly kernel is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>.
//
// All names, logos, and references to "Rijkswaterstaat" are registered trademarks of
// Rijkswaterstaat and remain full property of Rijkswaterstaat at all times.
// All rights reserved.
#endregion

using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Implementations.Validators;

namespace Assembly.Kernel.Model
{
    /// <summary>
    /// Failure Mechanism data object
    /// </summary>
    public class FailureMechanism
    {
        /// <summary>
        /// FailureMechanism Constructor
        /// </summary>
        /// <param name="lengthEffectFactor">factor to correct for length of the section. 
        /// Has to be greater or equal to 1</param>
        /// <param name="failureProbabilityMarginFactor">Factor for the failure probability margin. 
        /// Has to be between 0 and 1</param>
        /// <exception cref="AssemblyException">Thrown when one of the input values is not valid</exception>
        public FailureMechanism(double lengthEffectFactor, double failureProbabilityMarginFactor)
        {
            FailureMechanismValidator.CheckFailureMechanismInput(lengthEffectFactor,
                failureProbabilityMarginFactor);

            LengthEffectFactor = lengthEffectFactor;
            FailureProbabilityMarginFactor = failureProbabilityMarginFactor;
        }

        /// <summary>
        /// Factor to correct for length of the section.
        /// </summary>
        public double LengthEffectFactor { get; }

        /// <summary>
        /// Factor for the failure probability margin.
        /// </summary>
        public double FailureProbabilityMarginFactor { get; }

        /// /// <summary>
        /// Generates string from failure mechanism object.
        /// </summary>
        /// <returns>Text representation of the failure mechanism object</returns>
        public override string ToString()
        {
            return
                $"Length effect factor: {LengthEffectFactor}, " +
                $"Failure probability margin factor: {FailureProbabilityMarginFactor}";
        }
    }
}