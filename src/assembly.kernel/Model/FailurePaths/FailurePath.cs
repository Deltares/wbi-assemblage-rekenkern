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

using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Implementations.Validators;

namespace Assembly.Kernel.Model.FailurePaths
{
    /// <summary>
    /// Failure path data object
    /// </summary>
    public class FailurePath
    {
        /// <summary>
        /// FailurePath Constructor
        /// </summary>
        /// <param name="lengthEffectFactor">factor to correct for length of the section. 
        /// Has to be greater or equal to 1</param>
        /// <exception cref="AssemblyException">Thrown when one of the input values is not valid</exception>
        public FailurePath(double lengthEffectFactor)
        {
            FailurePathValidator.CheckFailurePathInput(lengthEffectFactor);

            LengthEffectFactor = lengthEffectFactor;
        }

        /// <summary>
        /// Factor to correct for length of the section.
        /// </summary>
        public double LengthEffectFactor { get; }

        /// /// <summary>
        /// Generates string from failure paths object.
        /// </summary>
        /// <returns>Text representation of the failure path object</returns>
        public override string ToString()
        {
            return $"Length effect factor: {LengthEffectFactor}";
        }
    }
}