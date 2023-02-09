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

namespace assembly.kernel.benchmark.tests.data.Result
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
        public BenchmarkFailureMechanismTestResult(string name, string mechanismId, bool hasLengthEffect)
        {
            Name = name;
            MechanismId = mechanismId;
            HasLengthEffect = hasLengthEffect;
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
        /// Indicates whether all assessment results per section were translated correctly to a combined assessment result per mechanism section during assembly.
        /// </summary>
        public bool AreEqualCombinedAssessmentResultsPerSection { get; set; }

        /// <summary>
        /// Indicates whether all assessment results per section were translated correctly to a combined assessment result for the failure mechanism during assembly.
        /// </summary>
        public bool AreEqualAssessmentResultPerAssessmentSection { get; set; }

        /// <summary>
        /// Indicates whether all assessment results per section were translated correctly to a combined assessment result for the failure mechanism during assembly while performing partial assembly.
        /// </summary>
        public bool AreEqualAssessmentResultPerAssessmentSectionPartial { get; set; }

        /// <summary>
        /// Indicates whether all combined results for the combined sections were translated correctly during assembly.
        /// </summary>
        public bool AreEqualCombinedResultsCombinedSections { get; set; }
    }
}