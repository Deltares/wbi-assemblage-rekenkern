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

namespace Assembly.Kernel.Model
{
    /// <summary>
    /// Failure mechanism assembly result with the calculated probability
    /// and the method that was used to obtain the probability.
    /// </summary>
    public class FailureMechanismAssemblyResult
    {
        /// <summary>
        /// Create a new instance of <see cref="FailureMechanismAssemblyResult"/>.
        /// </summary>
        /// <param name="probability">The calculated probability.</param>
        /// <param name="assemblyMethod">The used assembly method.</param>
        public FailureMechanismAssemblyResult(Probability probability, EFailureMechanismAssemblyMethod assemblyMethod)
        {
            Probability = probability;
            AssemblyMethod = assemblyMethod;
        }

        /// <summary>
        /// Gets the calculated probability.
        /// </summary>
        public Probability Probability { get; }

        /// <summary>
        /// Gets the used assembly method.
        /// </summary>
        public EFailureMechanismAssemblyMethod AssemblyMethod { get; }
    }
}