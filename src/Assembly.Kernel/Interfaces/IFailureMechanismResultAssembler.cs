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

using System;
using System.Collections.Generic;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.FailureMechanismSections;

namespace Assembly.Kernel.Interfaces
{
    /// <summary>
    /// Interface to assemble failure mechanism section results to a failure mechanism result.
    /// </summary>
    public interface IFailureMechanismResultAssembler
    {
        /// <summary>
        /// Calculates a <see cref="FailureMechanismAssemblyResult"/> from <paramref name="failureMechanismSectionAssemblyResults"/>.
        /// </summary>
        /// <param name="lengthEffectFactor">The length effect factor.</param>
        /// <param name="failureMechanismSectionAssemblyResults">The list of failure mechanism section assembly results.</param>
        /// <param name="partialAssembly">Indicator whether partial assembly is required.</param>
        /// <returns>A <see cref="FailureMechanismAssemblyResult"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="failureMechanismSectionAssemblyResults"/>
        /// is <c>null</c>.</exception>
        /// <exception cref="AssemblyException">Thrown when:
        /// <list type="bullet">
        /// <item><paramref name="failureMechanismSectionAssemblyResults"/> is <c>empty</c>;</item>
        /// <item><paramref name="failureMechanismSectionAssemblyResults"/> contains <see cref="Probability.Undefined"/> probabilities
        /// when <paramref name="partialAssembly"/> is <c>false</c>.</item>
        /// <item><paramref name="lengthEffectFactor"/> is &lt; 1.</item>
        /// </list>
        /// </exception>
        /// <remarks>When <paramref name="partialAssembly"/> is <c>true</c>, all <see cref="Probability.Undefined"/> probabilities are ignored.</remarks>
        FailureMechanismAssemblyResult CalculateFailureMechanismFailureProbabilityBoi1A1(
            double lengthEffectFactor,
            IEnumerable<Probability> failureMechanismSectionAssemblyResults,
            bool partialAssembly);

        /// <summary>
        /// Calculates a <see cref="FailureMechanismAssemblyResult"/> from <paramref name="failureMechanismSectionAssemblyResults"/>.
        /// </summary>
        /// <param name="lengthEffectFactor">The length effect factor.</param>
        /// <param name="failureMechanismSectionAssemblyResults">The list of failure mechanism section assembly results.</param>
        /// <param name="partialAssembly">Indicator whether partial assembly is required.</param>
        /// <returns>A <see cref="FailureMechanismAssemblyResult"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="failureMechanismSectionAssemblyResults"/>
        /// is <c>null</c>.</exception>
        /// <exception cref="AssemblyException">Thrown when:
        /// <list type="bullet">
        /// <item><paramref name="failureMechanismSectionAssemblyResults"/> is <c>empty</c>;</item>
        /// <item><paramref name="failureMechanismSectionAssemblyResults"/> contains <see cref="Probability.Undefined"/> probabilities
        /// when <paramref name="partialAssembly"/> is <c>false</c>.</item>
        /// <item><paramref name="lengthEffectFactor"/> is &lt; 1.</item>
        /// </list>
        /// </exception>
        /// <remarks>When <paramref name="partialAssembly"/> is <c>true</c>, all <see cref="Probability.Undefined"/> probabilities are ignored.</remarks>
        FailureMechanismAssemblyResult CalculateFailureMechanismFailureProbabilityWithLengthEffectBoi1A2(
            double lengthEffectFactor,
            IEnumerable<ResultWithProfileAndSectionProbabilities> failureMechanismSectionAssemblyResults,
            bool partialAssembly);

        /// <summary>
        /// Calculates a <see cref="Probability"/> from <paramref name="failureMechanismSectionAssemblyResults"/>.
        /// </summary>
        /// <param name="failureMechanismSectionAssemblyResults">The list of failure mechanism section assembly results.</param>
        /// <param name="partialAssembly">Indicator whether partial assembly is required.</param>
        /// <returns>The calculated <see cref="Probability"/>.</returns>\
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="failureMechanismSectionAssemblyResults"/>
        /// is <c>null</c>.</exception>
        /// <exception cref="AssemblyException">Thrown when:
        /// <list type="bullet">
        /// <item><paramref name="failureMechanismSectionAssemblyResults"/> is <c>empty</c>;</item>
        /// <item><paramref name="failureMechanismSectionAssemblyResults"/> contains <see cref="Probability.Undefined"/> probabilities
        /// when <paramref name="partialAssembly"/> is <c>false</c>.</item>
        /// </list>
        /// </exception>
        /// <remarks>When <paramref name="partialAssembly"/> is <c>true</c>, all <see cref="Probability.Undefined"/> probabilities are ignored.</remarks>
        Probability CalculateFailureMechanismFailureProbabilityBoi1A3(
            IEnumerable<ResultWithProfileAndSectionProbabilities> failureMechanismSectionAssemblyResults,
            bool partialAssembly);
    }
}