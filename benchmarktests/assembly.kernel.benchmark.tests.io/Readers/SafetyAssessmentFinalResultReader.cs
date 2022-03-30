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

using assembly.kernel.benchmark.tests.data.Input;
using Assembly.Kernel.Model;
using DocumentFormat.OpenXml.Packaging;

namespace assembly.kernel.benchmark.tests.io.Readers
{
    /// <summary>
    /// Reader to read the final result of a safety assessment.
    /// </summary>
    public class SafetyAssessmentFinalResultReader : ExcelSheetReaderBase
    {
        /// <summary>
        /// Creates a new instance of <see cref="SafetyAssessmentFinalResultReader"/>.
        /// </summary>
        /// <param name="worksheetPart">The worksheet for which to create a dictionary</param>
        /// <param name="workbookPart">Thw workbook part of the workbook that contains this worksheet</param>
        public SafetyAssessmentFinalResultReader(WorksheetPart worksheetPart, WorkbookPart workbookPart)
            : base(worksheetPart, workbookPart, "B") {}

        /// <summary>
        /// Reads the final verdict worksheet of a benchmark test definition.
        /// </summary>
        /// <param name="benchmarkTestInput">The test input.</param>
        public void Read(BenchmarkTestInput benchmarkTestInput)
        {
            benchmarkTestInput.ExpectedSafetyAssessmentAssemblyResult.ExpectedCombinedProbability =
                new Probability(GetCellValueAsDouble("D", "Overstromingskans traject"));
            benchmarkTestInput.ExpectedSafetyAssessmentAssemblyResult.ExpectedCombinedProbabilityPartial =
                new Probability(GetCellValueAsDouble("D", "Overstromingskans traject (tussentijds)"));

            benchmarkTestInput.ExpectedSafetyAssessmentAssemblyResult.ExpectedCombinedAssessmentGrade =
                GetCellValueAsString("E", "Overstromingskans traject").ToExpectedAssessmentGrade();
            benchmarkTestInput.ExpectedSafetyAssessmentAssemblyResult.ExpectedCombinedAssessmentGradePartial =
                GetCellValueAsString("E", "Overstromingskans traject (tussentijds)").ToExpectedAssessmentGrade();
        }
    }
}