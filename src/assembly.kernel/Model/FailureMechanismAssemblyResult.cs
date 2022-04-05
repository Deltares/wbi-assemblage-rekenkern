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

using Assembly.Kernel.Interfaces;

namespace Assembly.Kernel.Model
{
    /// <summary>
    /// Result class for <seealso cref="IAssessmentResultsTranslator.DetermineRepresentativeProbabilityBoi0A1"/>.
    /// This class holds a combination of the calculated probability and the method that was used to obtain the probability.
    /// </summary>
    public class FailureMechanismAssemblyResult
    {
        /// <summary>
        /// Constructor of the <see cref="FailureMechanismAssemblyResult"/> class.
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
        /// The method that was used to calculate the acquired probability.
        /// </summary>
        public EFailureMechanismAssemblyMethod AssemblyMethod { get; }
    }
}
