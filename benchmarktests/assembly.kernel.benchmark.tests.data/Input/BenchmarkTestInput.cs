#region Copyright (C) Rijkswaterstaat 2022. All rights reserved

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
using assembly.kernel.benchmark.tests.data.Input.FailureMechanisms;
using Assembly.Kernel.Model.Categories;
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
            ExpectedFailureMechanismsResults = new List<ExpectedFailureMechanismResult>();
        }

        /// <summary>
        /// The file name that contains the benchmark test definition and expected results.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Name of the benchmark test.
        /// </summary>
        public string TestName { get; set; }

        /// <summary>
        /// Total length of the assessment section. Make sure this length equals the combined length of all sections per failure mechanism.
        /// </summary>
        public double Length { get; set; }

        /// <summary>
        /// The signal flooding probability for this assessment section.
        /// </summary>
        public double SignalFloodingProbability { get; set; }

        /// <summary>
        /// The maximum allowable flooding probability for this assessment section.
        /// </summary>
        public double MaximumAllowableFloodingProbability { get; set; }

        /// <summary>
        /// The expected section categories (A+ to D) on the highest (assessment section) level.
        /// </summary>
        public CategoriesList<AssessmentSectionCategory> ExpectedAssessmentSectionCategories { get; set; }

        /// <summary>
        /// The expected interpretation categories.
        /// </summary>
        public CategoriesList<InterpretationCategory> ExpectedInterpretationCategories { get; set; }

        /// <summary>
        /// Expected input and results per failure mechanism.
        /// </summary>
        public List<ExpectedFailureMechanismResult> ExpectedFailureMechanismsResults { get; }

        /// <summary>
        /// The expected safety assessment result on assessment section level.
        /// </summary>
        public SafetyAssessmentAssemblyResult ExpectedSafetyAssessmentAssemblyResult { get; }

        /// <summary>
        /// The greatest common denominator section results per Failure mechanism.
        /// </summary>
        public IEnumerable<FailureMechanismSectionListWithFailureMechanismId> ExpectedCombinedSectionResultPerFailureMechanism { get; set; }

        /// <summary>
        /// The greatest common denominator section results for all failure mechanisms combined.
        /// </summary>
        public IEnumerable<FailureMechanismSectionWithCategory> ExpectedCombinedSectionResult { get; set; }

        /// <summary>
        /// The greatest common denominator section results for all failure mechanisms combined.
        /// </summary>
        public IEnumerable<FailureMechanismSectionWithCategory> ExpectedCombinedSectionResultPartial { get; set; }
    }
}