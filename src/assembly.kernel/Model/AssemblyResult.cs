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
        /// <param name="resultPerFailurePath">The greatest common denominator section results per 
        /// Failure path.</param>
        /// <param name="combinedSectionResult">The greatest common denominator section results for 
        /// all failure paths combined.</param>
        /// <exception cref="AssemblyException">Thrown when any of the inputs is null</exception>
        public AssemblyResult(IEnumerable<FailurePathSectionList> resultPerFailurePath,
            IEnumerable<FailurePathSectionWithCategory> combinedSectionResult)
        {
            if (resultPerFailurePath == null || combinedSectionResult == null)
            {
                throw new AssemblyException("AssemblyResult", EAssemblyErrors.ValueMayNotBeNull);
            }

            ResultPerFailurePath = resultPerFailurePath;
            CombinedSectionResult = combinedSectionResult;
        }

        /// <summary>
        /// The greatest common denominator section results per Failure path.
        /// </summary>
        public IEnumerable<FailurePathSectionList> ResultPerFailurePath { get; }

        /// <summary>
        /// The greatest common denominator section results for all failure paths combined.
        /// </summary>
        public IEnumerable<FailurePathSectionWithCategory> CombinedSectionResult { get; }
    }
}