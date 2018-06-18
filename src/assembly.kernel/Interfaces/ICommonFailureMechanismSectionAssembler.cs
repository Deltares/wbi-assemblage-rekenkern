#region Copyright (c) 2018 Technolution BV. All Rights Reserved. 

// // Copyright (C) Technolution BV. 2018. All rights reserved.
// //
// // This file is part of the Assembly kernel.
// //
// // Assembly kernel is free software: you can redistribute it and/or modify
// // it under the terms of the GNU Lesser General Public License as published by
// // the Free Software Foundation, either version 3 of the License, or
// // (at your option) any later version.
// // 
// // This program is distributed in the hope that it will be useful,
// // but WITHOUT ANY WARRANTY; without even the implied warranty of
// // MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// // GNU Lesser General Public License for more details.
// //
// // You should have received a copy of the GNU Lesser General Public License
// // along with this program. If not, see <http://www.gnu.org/licenses/>.
// //
// // All names, logos, and references to "Technolution BV" are registered trademarks of
// // Technolution BV and remain full property of Technolution BV at all times.
// // All rights reserved.

#endregion

using System.Collections.Generic;
using Assembly.Kernel.Exceptions;
using Assembly.Kernel.Model;

namespace Assembly.Kernel.Interfaces
{
    /// <summary>
    /// Assemble failure mechanism section results of multiple failure mechanisms to 
    /// a greatest denominator section result.
    /// </summary>
    public interface ICommonFailureMechanismSectionAssembler
    {
        /// <summary>
        /// Assemble failure mechanism section results into a greatest common denominator assembly result.
        /// </summary>
        /// <param name="failureMechanismSectionLists">The list of failure mechnism section results 
        /// grouped by failure mechnism</param>
        /// <param name="assessmentSectionLength">The total length of the assessment section. 
        /// The sum of the section lengths must be equal to this length.</param>
        /// <param name="partialAssembly">True if this assembly call is a partial call.</param>
        /// <returns>The greatest common denominator assembly result</returns>
        /// <exception cref="AssemblyException">Thrown when the failure mechanism sections aren't consecutive, 
        /// or when the sum of the failure mechanism sections is not the same as the total assessment section 
        /// length.</exception>
        AssemblyResult AssembleCommonFailureMechanismSections(
            IEnumerable<FailureMechanismSectionList> failureMechanismSectionLists, double assessmentSectionLength,
            bool partialAssembly);
    }
}