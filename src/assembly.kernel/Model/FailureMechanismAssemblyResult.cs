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
    /// Result class for the WBI-1B-1 method.
    /// </summary>
    public class FailureMechanismAssemblyResult
    {
        /// <summary>
        /// Constructor of the FailureMechanismAssemblyResult class
        /// </summary>
        public FailureMechanismAssemblyResult(Probability probability, EFailureMechanismAssemblyMethod mechanismAssemblyMethod)
        {
            Probability = probability;
            AssemblyMethod = mechanismAssemblyMethod;
        }

        /// <summary>
        /// The calculated combined probability for all sections.
        /// </summary>
        public Probability Probability { get; }

        /// <summary>
        /// The method that was used to calculate the acquired probability:
        /// * Correlated -> The highest probability of all section (profiles) was taken and multiplied by the length effect factor.
        /// * Uncorrelated -> Probability equals the 1-product(1-probability of failure of all individual sections).
        /// </summary>
        public EFailureMechanismAssemblyMethod AssemblyMethod { get; }
    }
}
