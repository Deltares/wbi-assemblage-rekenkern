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

using System.ComponentModel;

namespace assembly.kernel.benchmark.tests.data.Result
{
    /// <summary>
    /// Factory for creating instances of <see cref="BenchmarkFailureMechanismTestResult"/>.
    /// </summary>
    public static class BenchmarkTestFailureMechanismResultFactory
    {
        /// <summary>
        /// Creates a default benchmark test result for the specified failure mechanism mechanismId.
        /// </summary>
        /// <param name="mechanismName">Name of the failure mechanism</param>
        /// <param name="mechanismId">String representing the mechanism id.</param>
        /// <param name="hasLengthEffect">Indicates whether the failure mechanism has length effect within sections</param>
        /// <returns>A new <see cref="BenchmarkFailureMechanismTestResult"/>.</returns>
        public static BenchmarkFailureMechanismTestResult CreateFailureMechanismTestResult(string mechanismName, string mechanismId, bool hasLengthEffect)
        {
            if (string.IsNullOrWhiteSpace(mechanismId))
            {
                throw new InvalidEnumArgumentException();
            }

            return new BenchmarkFailureMechanismTestResult(mechanismName, mechanismId, hasLengthEffect);
        }
    }
}