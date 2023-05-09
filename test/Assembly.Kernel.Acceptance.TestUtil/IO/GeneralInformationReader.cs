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
using Assembly.Kernel.Acceptance.TestUtil.Data.Input;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.Categories;
using DocumentFormat.OpenXml.Packaging;

namespace Assembly.Kernel.Acceptance.TestUtil.IO
{
    /// <summary>
    /// Reader to read general information.
    /// </summary>
    public class GeneralInformationReader : ExcelSheetReaderBase
    {
        /// <summary>
        /// Creates a new instance of <see cref="GeneralInformationReader"/>.
        /// </summary>
        /// <param name="worksheetPart">The worksheet for which to create a dictionary.</param>
        /// <param name="workbookPart">The workbook part of the workbook that contains this worksheet.</param>
        public GeneralInformationReader(WorksheetPart worksheetPart, WorkbookPart workbookPart)
            : base(worksheetPart, workbookPart, "A") {}

        /// <summary>
        /// Reads the general information of an assessment section.
        /// </summary>
        /// <param name="benchmarkTestInput">The test input.</param>
        public void Read(BenchmarkTestInput benchmarkTestInput)
        {
            benchmarkTestInput.SignalFloodingProbability = GetCellValueAsDouble("B", "Signaleringskans");
            benchmarkTestInput.MaximumAllowableFloodingProbability = GetCellValueAsDouble("B", "Ondergrens");
            benchmarkTestInput.Length = GetCellValueAsDouble("B", "Trajectlengte") * 1000.0;

            var assessmentGradeCategories = new List<AssessmentSectionCategory>();
            for (var iRow = 4; iRow <= 8; iRow++)
            {
                EExpectedAssessmentGrade expectedAssessmentGrade = GetCellValueAsString("D", iRow).ToExpectedAssessmentGrade();
                assessmentGradeCategories.Add(new AssessmentSectionCategory(
                                                  expectedAssessmentGrade.ToEAssessmentGrade(),
                                                  new Probability(GetCellValueAsDouble("F", iRow)),
                                                  new Probability(GetCellValueAsDouble("E", iRow))));
            }

            benchmarkTestInput.ExpectedAssessmentSectionCategories = new CategoriesList<AssessmentSectionCategory>(assessmentGradeCategories);

            var interpretationCategories = new List<InterpretationCategory>();
            var lastKnownBoundary = new Probability(0);
            for (var iRow = 13; iRow <= 19; iRow++)
            {
                var newBoundary = new Probability(GetCellValueAsDouble("E", iRow));
                interpretationCategories.Add(new InterpretationCategory(
                                                 GetCellValueAsString("D", iRow).ToInterpretationCategory(),
                                                 lastKnownBoundary,
                                                 newBoundary));
                lastKnownBoundary = newBoundary;
            }

            benchmarkTestInput.ExpectedInterpretationCategories = new CategoriesList<InterpretationCategory>(interpretationCategories);
        }
    }
}