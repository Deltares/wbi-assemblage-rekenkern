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

using assembly.kernel.benchmark.tests.data.Input.FailureMechanisms;

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
        /// <param name="type">The type of the failure mechanism.</param>
        /// <param name="group">The assembly group of the failure mechanism.</param>
        public BenchmarkFailureMechanismTestResult(string name, MechanismType type, int group)
        {
            Name = name;
            Type = type;
            Group = group;
        }

        /// <summary>
        /// Name of the failure mechanism.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Type / code of the failure mechanism.
        /// </summary>
        public MechanismType Type { get; }

        /// <summary>
        /// Assembly group of the failure mechanism (1, 2, 3, 4 or 5).
        /// </summary>
        public int Group { get; }

        /// <summary>
        /// Indicates whether category boundaries where calculated correctly.
        /// </summary>
        public bool? AreEqualCategoryBoundaries { get; set; }

        /// <summary>
        /// Indicates whether all simple assessment results where translated correctly during assembly.
        /// </summary>
        public bool AreEqualSimpleAssessmentResults { get; set; }

        /// <summary>
        /// Indicates whether all detailed assessment results where translated correctly during assembly.
        /// </summary>
        public bool? AreEqualDetailedAssessmentResults { get; set; }

        /// <summary>
        /// Indicates whether all tailor made assessment results where translated correctly during assembly.
        /// </summary>
        public bool AreEqualTailorMadeAssessmentResults { get; set; }

        /// <summary>
        /// Indicates whether all assessment results per section where translated correctly to a combined assesment result per mechanism section during assembly.
        /// </summary>
        public bool AreEqualCombinedAssessmentResultsPerSection { get; set; }

        /// <summary>
        /// Indicates whether all assessment results per section where translated correctly to a combined assesment result for the failure mechanism during assembly.
        /// </summary>
        public bool AreEqualAssessmentResultPerAssessmentSection { get; set; }

        /// <summary>
        /// Indicates whether all assessment results per section where translated correctly to a combined assesment result for the failure mechanism during assembly while performing partial assembly.
        /// </summary>
        public bool AreEqualAssessmentResultPerAssessmentSectionTemporal { get; set; }

        /// <summary>
        /// Indicates whether all combined results for the combined sections where translated correctly during assembly.
        /// </summary>
        public bool AreEqualCombinedResultsCombinedSections { get; set; }
    }
}