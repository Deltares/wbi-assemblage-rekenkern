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
using assembly.kernel.benchmark.tests.data.Input;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.Categories;
using DocumentFormat.OpenXml.Packaging;

namespace assembly.kernel.benchmark.tests.io.Readers
{
    /// <summary>
    /// Reader to read general information.
    /// </summary>
    public class GeneralInformationReader : ExcelSheetReaderBase
    {
        /// <summary>
        /// Creates a new instance of <see cref="GeneralInformationReader"/>.
        /// </summary>
        /// <param name="worksheetPart">The worksheet for which to create a dictionary</param>
        /// <param name="workbookPart">Thw workbook part of the workbook that contains this worksheet</param>
        public GeneralInformationReader(WorksheetPart worksheetPart, WorkbookPart workbookPart) : base(
            worksheetPart, workbookPart, "A") {}

        /// <summary>
        /// Reads the general information of an assessment section.
        /// </summary>
        /// <param name="benchmarkTestInput">The test input.</param>
        public void Read(BenchmarkTestInput benchmarkTestInput)
        {
            benchmarkTestInput.SignallingNorm = GetCellValueAsDouble("B", "Signaleringskans");
            benchmarkTestInput.LowerBoundaryNorm = GetCellValueAsDouble("B", "Ondergrens");
            benchmarkTestInput.Length = GetCellValueAsDouble("B", "Trajectlengte");

            var assessmentGradeCategories = new List<AssessmentSectionCategory>();
            for (int iRow = 4; iRow <= 8; iRow++)
            {
                assessmentGradeCategories.Add(new AssessmentSectionCategory(
                             GetCellValueAsString("D", iRow).ToAssessmentGrade(),
                             new Probability(GetCellValueAsDouble("F", iRow)),
                             new Probability(GetCellValueAsDouble("E", iRow))));
            }

            benchmarkTestInput.ExpectedAssessmentSectionCategories =
                new CategoriesList<AssessmentSectionCategory>(assessmentGradeCategories);

            var interpretationCategories = new List<InterpretationCategory>();
            var lastKnownBoundary = new Probability(0);
            for (int iRow = 13; iRow <= 21; iRow++)
            {
                var newBoundary = new Probability(GetCellValueAsDouble("E", iRow));
                interpretationCategories.Add(new InterpretationCategory(
                    GetCellValueAsString("D", iRow).ToInterpretationCategory(),
                    lastKnownBoundary,
                    newBoundary));
                lastKnownBoundary = newBoundary;
            }

            benchmarkTestInput.ExpectedInterpretationCategories =
                new CategoriesList<InterpretationCategory>(interpretationCategories);

        }
    }
}