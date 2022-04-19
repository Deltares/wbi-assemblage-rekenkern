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
using Assembly.Kernel.Interfaces;

namespace Assembly.Kernel.Model.FailureMechanismSections
{
    /// <summary>
    /// The assessment section result per greatest common denominator section.
    /// </summary>
    public class GreatestCommonDenominatorAssemblyResult
    {
        /// <summary>
        /// Constructor for the result of <see cref="ICommonFailureMechanismSectionAssembler.AssembleCommonFailureMechanismSections"/>.
        /// This class holds the results per failure mechanism, but also the combined results.
        /// </summary>
        /// <param name="resultPerFailureMechanism">The greatest common denominator section results per 
        /// Failure mechanism.</param>
        /// <param name="combinedSectionResult">The greatest common denominator section results for 
        /// all failure mechanisms combined.</param>
        /// <exception cref="AssemblyException">Thrown in case <paramref name="resultPerFailureMechanism"/> equals null.</exception>
        /// <exception cref="AssemblyException">Thrown in case <paramref name="combinedSectionResult"/> equals null.</exception>
        public GreatestCommonDenominatorAssemblyResult(IEnumerable<FailureMechanismSectionList> resultPerFailureMechanism,
        IEnumerable<FailureMechanismSectionWithCategory> combinedSectionResult)
        {
            if (resultPerFailureMechanism == null || combinedSectionResult == null)
            {
                throw new AssemblyException(nameof(GreatestCommonDenominatorAssemblyResult), EAssemblyErrors.ValueMayNotBeNull);
            }

            ResultPerFailureMechanism = resultPerFailureMechanism;
            CombinedSectionResult = combinedSectionResult;
        }

        /// <summary>
        /// The greatest common denominator section results per Failure mechanism.
        /// </summary>
        public IEnumerable<FailureMechanismSectionList> ResultPerFailureMechanism { get; }

        /// <summary>
        /// The greatest common denominator section results for all failure mechanisms combined.
        /// </summary>
        public IEnumerable<FailureMechanismSectionWithCategory> CombinedSectionResult { get; }
    }
}