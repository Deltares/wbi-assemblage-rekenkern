﻿#region Copyright (C) Rijkswaterstaat 2019. All rights reserved

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
using Assembly.Kernel.Model.FailurePaths;

namespace Assembly.Kernel.Interfaces
{
    /// <summary>
    /// Assemble Failure path section results into one result for the failure path.
    /// </summary>
    public interface IFailurePathResultAssembler
    {
        /// <summary>
        /// Assemble a list of failure path section assembly results with failure probability to
        /// a single failure path assembly result.
        /// </summary>
        /// <param name="failurePath">The failure path to assemble the result for</param>
        /// <param name="fpSectionAssemblyResults">The list of failure path section assembly results 
        /// with failure probability to use for this assembly step.</param>
        /// <param name="partialAssembly">true if the assembly input is part of a partial assembly</param>
        /// <returns>An assambled Failure path result</returns>
        /// /// <exception cref="AssemblyException">Thrown when:<br/>
        /// - result input is null or empty<br/>
        /// - one or more of the results doesn't have a failure probability<br/>
        /// </exception>
        FailurePathAssemblyResult AssembleFailurePathWbi1B1(
            FailurePath failurePath,
            IEnumerable<FailurePathSectionAssemblyResult> fpSectionAssemblyResults,
            bool partialAssembly);
    }
}