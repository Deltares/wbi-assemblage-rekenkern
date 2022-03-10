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

namespace assembly.kernel.benchmark.tests.data.Result
{
    /// <summary>
    /// The benchmark test result.
    /// </summary>
    public class BenchmarkTestResult
    {
        /// <summary>
        /// Creates a new instance of <see cref="BenchmarkTestResult"/>.
        /// </summary>
        /// <param name="fileName">The name of the file.</param>
        /// <param name="testName">The name of the test.</param>
        public BenchmarkTestResult(string fileName, string testName)
        {
            FileName = fileName;
            TestName = testName;
            FailureMechanismResults = new List<BenchmarkFailureMechanismTestResult>();
            MethodResults = new MethodResultsListing();
        }

        /// <summary>
        /// Name of the file that contains the definition of the benchmark test.
        /// </summary>
        public string FileName { get; }

        /// <summary>
        /// Name of the benchmark test.
        /// </summary>
        public string TestName { get; }

        /// <summary>
        /// Indicates whether category boundaries were calculated correctly.
        /// </summary>
        public bool AreEqualCategoriesListAssessmentSection { get; set; }

        /// <summary>
        /// Indicates whether category boundaries of the interpretation categories were calculated correctly.
        /// </summary>
        public bool AreEqualCategoriesListInterpretationCategories { get; set; }

        /// <summary>
        /// Provides a list of benchmark test results per failure mechanism.
        /// </summary>
        public IList<BenchmarkFailureMechanismTestResult> FailureMechanismResults { get; }

        /// <summary>
        /// Indicates whether assembly of the final verdict was correctly.
        /// </summary>
        /// TODO: Test and use this.
        public bool AreEqualAssemblyResultFinalVerdict { get; set; }

        /// <summary>
        /// Indicates whether assembly of the final verdict was correctly.
        /// </summary>
        /// TODO: Test and use this.
        public bool AreEqualAssemblyResultFinalVerdictProbability { get; set; }

        /// <summary>
        /// Indicates whether assembly of the final verdict was correctly during partial assembly.
        /// </summary>
        public bool AreEqualAssemblyResultFinalVerdictTemporal { get; set; }

        /// <summary>
        /// Indicates whether assembly of the final verdict was correctly during partial assembly.
        /// </summary>
        public bool AreEqualAssemblyResultFinalVerdictProbabilityTemporal { get; set; }

        /// <summary>
        /// Indicates whether the combined sections where determined correctly
        /// </summary>
        public bool AreEqualAssemblyResultCombinedSections { get; set; }

        /// <summary>
        /// Indicates whether the assessment results per combined section were calculated correctly.
        /// </summary>
        public bool AreEqualAssemblyResultCombinedSectionsResults { get; set; }

        /// <summary>
        /// Indicates whether the assessment results per combined section were calculated correctly during partial assembly.
        /// </summary>
        public bool AreEqualAssemblyResultCombinedSectionsResultsTemporal { get; set; }

        /// <summary>
        /// Provides a list of benchmark test results per assembly method.
        /// </summary>
        public MethodResultsListing MethodResults { get; }
    }
}