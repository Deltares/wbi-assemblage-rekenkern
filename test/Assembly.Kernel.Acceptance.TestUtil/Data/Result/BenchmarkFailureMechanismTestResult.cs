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

namespace Assembly.Kernel.Acceptance.TestUtil.Data.Result
{
    /// <summary>
    /// The benchmark failure mechanism test result.
    /// </summary>
    public class BenchmarkFailureMechanismTestResult
    {
        /// <summary>
        /// Create a new instance of <see cref="BenchmarkFailureMechanismTestResult"/>.
        /// </summary>
        /// <param name="name">The name of the failure mechanism.</param>
        /// <param name="mechanismId">The Id of the failure mechanism.</param>
        /// <param name="hasLengthEffect">The assembly hasLengthEffect of the failure mechanism.</param>
        /// <param name="assemblyMethod">The used assembly method.</param>
        public BenchmarkFailureMechanismTestResult(string name, string mechanismId, bool hasLengthEffect, string assemblyMethod)
        {
            Name = name;
            MechanismId = mechanismId;
            HasLengthEffect = hasLengthEffect;
            AssemblyMethod = assemblyMethod;
        }

        /// <summary>
        /// Name of the failure mechanism.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// MechanismId / code of the failure mechanism.
        /// </summary>
        public string MechanismId { get; }

        /// <summary>
        /// Indicates whether this failure mechanism has length effect within a section.
        /// </summary>
        public bool HasLengthEffect { get; }
        
        /// <summary>
        /// Gets the used assembly method.
        /// </summary>
        public string AssemblyMethod { get; }

        /// <summary>
        /// Gets or sets whether all failure mechanism section results were translated correctly during assembly.
        /// </summary>
        public bool AreEqualFailureMechanismSectionsResults { get; set; }

        /// <summary>
        /// Gets or sets whether all failure mechanism section results were translated correctly during assembly.
        /// </summary>
        public bool AreEqualFailureMechanismResult { get; set; }

        /// <summary>
        /// Gets or sets whether all failure mechanism section results were translated correctly during a partial assembly.
        /// </summary>
        public bool AreEqualFailureMechanismResultPartial { get; set; }

        /// <summary>
        /// Gets or sets whether all results for the combined sections were translated correctly during assembly.
        /// </summary>
        public bool AreEqualCombinedResultsCombinedSections { get; set; }
    }
}