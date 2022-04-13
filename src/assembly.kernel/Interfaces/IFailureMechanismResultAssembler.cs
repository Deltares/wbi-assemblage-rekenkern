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

using System.Collections.Generic;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.FailureMechanismSections;

namespace Assembly.Kernel.Interfaces
{
    /// <summary>
    /// Assemble Failure mechanism section results into one result for the failure mechanism.
    /// </summary>
    public interface IFailureMechanismResultAssembler
    {
        /// <summary>
        /// Assemble a list of failure mechanism section assembly results with failure probability to
        /// a single failure mechanism assembly result.
        /// </summary>
        /// <param name="lengthEffectFactor">The failure mechanism to assemble the result for.</param>
        /// <param name="failureMechanismSectionAssemblyResults">The list of failure mechanism section assembly results 
        /// with failure probability to use for this assembly step.</param>
        /// <param name="partialAssembly">True if the assembly input is part of a partial assembly.</param>
        /// <returns>The combined probability together with the used method in a <seealso cref="FailureMechanismAssemblyResult"/>.</returns>
        /// <exception cref="AssemblyException">Thrown when: <list type="bullet">
        /// <item><paramref name="failureMechanismSectionAssemblyResults"/> is null or empty.</item>
        /// <item><paramref name="lengthEffectFactor"/> $lt; 1</item>
        /// <item><paramref name="partialAssembly"/> equals false and one or more of the results in <paramref name="failureMechanismSectionAssemblyResults"/> has an undefined probability.</item>
        /// </list>
        /// </exception>
        FailureMechanismAssemblyResult CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2(
            double lengthEffectFactor,
            IEnumerable<ResultWithProfileAndSectionProbabilities> failureMechanismSectionAssemblyResults,
            bool partialAssembly);

        /// <summary>
        /// Assemble a list of section failure probabilities to a single <seealso cref="FailureMechanismAssemblyResult"/>.
        /// </summary>
        /// <param name="lengthEffectFactor">The failure mechanism to assemble the result for.</param>
        /// <param name="failureMechanismSectionProbabilities">The list of failure probabilities
        /// to use for this assembly step.</param>
        /// <param name="partialAssembly">True if the assembly input is part of a partial assembly.</param>
        /// <returns>The combined probability together with the used method in a <seealso cref="FailureMechanismAssemblyResult"/>.</returns>
        /// <exception cref="AssemblyException">Thrown when <paramref name="failureMechanismSectionProbabilities"/> equals null or is empty.</exception>
        /// <exception cref="AssemblyException">Thrown when <paramref name="failureMechanismSectionProbabilities"/> $lt;1.</exception>
        /// <exception cref="AssemblyException">Thrown when <paramref name="partialAssembly"/> equals false and one or more of the
        /// probabilities in <paramref name="failureMechanismSectionProbabilities"/> is undefined.</exception>
        FailureMechanismAssemblyResult CalculateFailureMechanismFailureProbabilityBoi1A1(
            double lengthEffectFactor,
            IEnumerable<Probability> failureMechanismSectionProbabilities,
            bool partialAssembly);
    }
}