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
    /// Validator for failure mechanism objects.
    /// </summary>
    public static class FailureMechanismValidator
    {
        /// <summary>
        /// Checks if failure mechanism data is valid.
        /// </summary>
        /// <param name="lengthEffectFactor">The length effect factor to check. Has to be &gt;= 1</param>
        /// <param name="failureProbabilityMarginFactor">The failure probaility margin factor to check. 
        ///     Must be &gt;= 0 &lt;= 1</param>
        /// <exception cref="AssemblyException">Thrown when input is not valid</exception>
        public static void CheckFailureMechanismInput(double lengthEffectFactor,
                                                      double failureProbabilityMarginFactor)
        {
            var errors = new List<AssemblyErrorMessage>();

            if (failureProbabilityMarginFactor < 0 ||
                failureProbabilityMarginFactor > 1)
            {
                errors.Add(new AssemblyErrorMessage("FailureMechanism",
                                                    EAssemblyErrors.FailurePropbabilityMarginOutOfRange));
            }

            if (lengthEffectFactor < 1)
            {
                errors.Add(new AssemblyErrorMessage("FailureMechanism", EAssemblyErrors.LengthEffectFactorOutOfRange));
            }

            if (errors.Count > 0)
            {
                throw new AssemblyException(errors);
            }
        }
    }
}