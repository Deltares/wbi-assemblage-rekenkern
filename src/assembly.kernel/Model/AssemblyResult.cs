#region Copyright (C) Rijkswaterstaat 2019. All rights reserved
// Copyright (C) Rijkswaterstaat 2019. All rights reserved.
//
// This file is part of the Assembly kernel.
//
// Assembly kernel is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>.
//
// All names, logos, and references to "Rijkswaterstaat" are registered trademarks of
// Rijkswaterstaat and remain full property of Rijkswaterstaat at all times.
// All rights reserved.
#endregion

using System.Collections.Generic;
using Assembly.Kernel.Exceptions;

namespace Assembly.Kernel.Model
{
    /// <summary>
    /// The assessment section result per greatest common denominator section.
    /// </summary>
    public class AssemblyResult
    {
        /// <summary>
        /// Assembly result constructor
        /// </summary>
        /// <param name="resultPerFailureMechanism">The greatest common denominator section results per 
        /// Failure mechanism.</param>
        /// <param name="combinedSectionResult">The greatest common denominator section results for 
        /// all failure mechnisms combined.</param>
        /// <exception cref="AssemblyException">Thrown when any of the inputs is null</exception>
        public AssemblyResult(IEnumerable<FailureMechanismSectionList> resultPerFailureMechanism,
            IEnumerable<FmSectionWithDirectCategory> combinedSectionResult)
        {
            if (resultPerFailureMechanism == null || combinedSectionResult == null)
            {
                throw new AssemblyException("AssemblyResult", EAssemblyErrors.ValueMayNotBeNull);
            }

            ResultPerFailureMechanism = resultPerFailureMechanism;
            CombinedSectionResult = combinedSectionResult;
        }

        /// <summary>
        /// The greatest common denominator section results per Failure mechanism.
        /// </summary>
        public IEnumerable<FailureMechanismSectionList> ResultPerFailureMechanism { get; }

        /// <summary>
        /// The greatest common denominator section results for all failure mechnisms combined.
        /// </summary>
        public IEnumerable<FmSectionWithDirectCategory> CombinedSectionResult { get; }
    }
}