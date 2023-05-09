// Copyright (C) Stichting Deltares and State of the Netherlands 2023. All rights reserved.
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
// All names, logos, and references to "Deltares" are registered trademarks of
// Stichting Deltares and remain full property of Stichting Deltares at all times.
// All rights reserved.

using System.Collections.Generic;
using Assembly.Kernel.Acceptance.TestUtil.Data.Input.FailureMechanisms;
using Assembly.Kernel.Model.Categories;
using Assembly.Kernel.Model.FailureMechanismSections;

namespace Assembly.Kernel.Acceptance.TestUtil.Data.Input
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
            ExpectedCombinedSectionResultPartial = new List<FailureMechanismSectionWithCategory>();
            ExpectedCombinedSectionResultPerFailureMechanism = new List<FailureMechanismSectionListWithFailureMechanismId>();
            ExpectedCombinedSectionResult = new List<FailureMechanismSectionWithCategory>();
            ExpectedSafetyAssessmentAssemblyResult = new ExpectedSafetyAssessmentAssemblyResult();
            ExpectedFailureMechanismsResults = new List<ExpectedFailureMechanismResult>();
        }

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
        public ExpectedSafetyAssessmentAssemblyResult ExpectedSafetyAssessmentAssemblyResult { get; }

        /// <summary>
        /// The greatest common denominator section results per Failure mechanism.
        /// </summary>
        public List<FailureMechanismSectionListWithFailureMechanismId> ExpectedCombinedSectionResultPerFailureMechanism { get; }

        /// <summary>
        /// The greatest common denominator section results for all failure mechanisms combined.
        /// </summary>
        public List<FailureMechanismSectionWithCategory> ExpectedCombinedSectionResult { get; }

        /// <summary>
        /// The greatest common denominator section results for all failure mechanisms combined after partial assembly.
        /// </summary>
        public List<FailureMechanismSectionWithCategory> ExpectedCombinedSectionResultPartial { get; }
    }
}