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

using System.Collections.Generic;
using Assembly.Kernel.Exceptions;

namespace Assembly.Kernel.Implementations.Validators
{
    /// <summary>
    /// Validator for failure path objects.
    /// </summary>
    public static class FailurePathValidator
    {
        /// <summary>
        /// Checks if failure path data is valid.
        /// </summary>
        /// <param name="lengthEffectFactor">The length effect factor to check. Has to be &gt;= 1</param>
        /// <exception cref="AssemblyException">Thrown when input is not valid</exception>
        public static void CheckFailurePathInput(double lengthEffectFactor)
        {
            var errors = new List<AssemblyErrorMessage>();

            if (lengthEffectFactor < 1)
            {
                errors.Add(new AssemblyErrorMessage("FailurePath", EAssemblyErrors.LengthEffectFactorOutOfRange));
            }

            if (errors.Count > 0)
            {
                throw new AssemblyException(errors);
            }
        }
    }
}