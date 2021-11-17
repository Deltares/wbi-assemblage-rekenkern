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

namespace Assembly.Kernel.Model
{
    /// <summary>
    /// The assembly result class of a direct failure mechanism.
    /// </summary>
    public class FailurePathAssemblyResult
    {
        /// <summary>
        /// Failure mechanism assembly direct result constructor, with failure probability.
        /// </summary>
        /// <param name="category">The resulting category of the failure mechanism assembly step</param>
        /// <param name="failureProbability">The assembled failure probability of the failure mechanism</param>
        public FailurePathAssemblyResult(double failureProbability)
        {
            FailureProbability = failureProbability;
        }

        /// <summary>
        /// The failure probability of the failure mechanism. 
        /// If no failure probability is present this value is null.
        /// </summary>
        public double FailureProbability { get; }
    }
}