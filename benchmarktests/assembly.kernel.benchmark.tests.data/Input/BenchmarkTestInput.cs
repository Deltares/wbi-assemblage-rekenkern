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
using assembly.kernel.benchmark.tests.data.Input.FailureMechanisms;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.FailureMechanismSections;

namespace assembly.kernel.benchmark.tests.data.Input
{
    /// <summary>
    /// Benchmark test input.
    /// </summary>
    public class BenchmarkTestInput
    {
        /// <summary>
        /// Creates a new instance of <see cref="BenchmarkTestInput"/>.
        /// </summary>
        public BenchmarkTestInput()
        {
            ExpectedSafetyAssessmentAssemblyResult = new SafetyAssessmentAssemblyResult();
            ExpectedFailureMechanismsResults = new List<IExpectedFailureMechanismResult>();
        }

        /// <summary>
        /// The file name that contains the benchmark testdefinition and expected results
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Name of the benchmark test
        /// </summary>
        public string TestName { get; set; }

        /// <summary>
        /// Total length of the assessment section. Make sure this length equals the combined length of all sections per failure mechanism
        /// </summary>
        public double Length { get; set; }

        /// <summary>
        /// The signalling norm for this assessment section
        /// </summary>
        public double SignallingNorm { get; set; }

        /// <summary>
        /// The lower boundary norm for this assessment section
        /// </summary>
        public double LowerBoundaryNorm { get; set; }

        /// <summary>
        /// The greatest common denominator section results per Failure mechanism.
        /// </summary>
        public IEnumerable<FailureMechanismSectionList> ExpectedCombinedSectionResultPerFailureMechanism { get; set; }

        /// <summary>
        /// The greatest common denominator section results for all failure mechanisms combined.
        /// </summary>
        public IEnumerable<FailureMechanismSectionWithCategory> ExpectedCombinedSectionResult { get; set; }

        /// <summary>
        /// The greatest common denominator section results for all failure mechanisms combined.
        /// </summary>
        public IEnumerable<FailureMechanismSectionWithCategory> ExpectedCombinedSectionResultTemporal { get; set; }

        /// <summary>
        /// Expected input and results per failure mechanism.
        /// </summary>
        public List<IExpectedFailureMechanismResult> ExpectedFailureMechanismsResults { get; }

        /// <summary>
        /// The expected safety assessment result on assessment section level.
        /// </summary>
        public SafetyAssessmentAssemblyResult ExpectedSafetyAssessmentAssemblyResult { get; }
    }
}