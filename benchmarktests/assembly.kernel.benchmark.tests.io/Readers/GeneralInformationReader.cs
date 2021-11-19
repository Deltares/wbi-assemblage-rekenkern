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
using assembly.kernel.benchmark.tests.data.Input;
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
            worksheetPart, workbookPart) {}

        /// <summary>
        /// Reads the general information of an assessment section.
        /// </summary>
        /// <param name="benchmarkTestInput">The test input.</param>
        public void Read(BenchmarkTestInput benchmarkTestInput)
        {
            benchmarkTestInput.SignallingNorm = GetCellValueAsDouble("D", "Signaleringswaarde [terugkeertijd]");
            benchmarkTestInput.LowerBoundaryNorm = GetCellValueAsDouble("D", "Ondergrens [terugkeertijd]");
            benchmarkTestInput.Length = GetCellValueAsDouble("B", "Trajectlengte [m]");

            var list = new List<AssessmentSectionCategory>();
            var startRowCategories = GetRowId("Categorie") + 1;
            for (int iRow = startRowCategories; iRow <= startRowCategories + 4; iRow++)
            {
                list.Add(new AssessmentSectionCategory(
                             GetCellValueAsString("A", iRow).ToAssessmentGrade(),
                             GetCellValueAsDouble("B", iRow),
                             GetCellValueAsDouble("C", iRow)));
            }

            benchmarkTestInput.ExpectedSafetyAssessmentAssemblyResult.ExpectedAssessmentSectionCategories =
                new CategoriesList<AssessmentSectionCategory>(list);
        }
    }
}