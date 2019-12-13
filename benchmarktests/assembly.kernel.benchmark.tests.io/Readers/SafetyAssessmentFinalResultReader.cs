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
using Assembly.Kernel.Model.CategoryLimits;
using DocumentFormat.OpenXml.Packaging;

namespace assembly.kernel.benchmark.tests.io.Readers
{
    public class SafetyAssessmentFinalResultReader : ExcelSheetReaderBase
    {
        /// <summary>
        /// Creates an instance of the SafetyAssessmentFinalResultReader.
        /// </summary>
        /// <param name="worksheetPart"></param>
        /// <param name="workbookPart"></param>
        public SafetyAssessmentFinalResultReader(WorksheetPart worksheetPart, WorkbookPart workbookPart) : base(worksheetPart, workbookPart)
        {
        }

        /// <summary>
        /// Reads the final verdict worksheet of a benchmark test definition.
        /// </summary>
        /// <param name="benchmarkTestInput"></param>
        public void Read(BenchmarkTestInput benchmarkTestInput)
        {
            benchmarkTestInput.ExpectedSafetyAssessmentAssemblyResult.ExpectedAssemblyResultGroups1and2 =
                GetCellValueAsString("D", "Toetssporen in groep 1 en 2").ToFailureMechanismCategory();
            benchmarkTestInput.ExpectedSafetyAssessmentAssemblyResult.ExpectedAssemblyResultGroups1and2Probability =
                GetCellValueAsDouble("E", "Toetssporen in groep 1 en 2");
            benchmarkTestInput.ExpectedSafetyAssessmentAssemblyResult.ExpectedAssemblyResultGroups3and4 =
                GetCellValueAsString("D", "Toetssporen in groep 3 en 4").ToFailureMechanismCategory();
            benchmarkTestInput.ExpectedSafetyAssessmentAssemblyResult.ExpectedSafetyAssessmentAssemblyResult =
                GetCellValueAsString("D", "Combineren tot veiligheidsoordeel").ToAssessmentGrade();

            benchmarkTestInput.ExpectedSafetyAssessmentAssemblyResult.ExpectedAssemblyResultGroups1and2Temporal =
                GetCellValueAsString("F", "Toetssporen in groep 1 en 2").ToFailureMechanismCategory();
            benchmarkTestInput.ExpectedSafetyAssessmentAssemblyResult.ExpectedAssemblyResultGroups1and2ProbabilityTemporal =
                GetCellValueAsDouble("G", "Toetssporen in groep 1 en 2");
            benchmarkTestInput.ExpectedSafetyAssessmentAssemblyResult.ExpectedAssemblyResultGroups3and4Temporal =
                GetCellValueAsString("F", "Toetssporen in groep 3 en 4").ToFailureMechanismCategory();
            benchmarkTestInput.ExpectedSafetyAssessmentAssemblyResult.ExpectedSafetyAssessmentAssemblyResultTemporal =
                GetCellValueAsString("F", "Combineren tot veiligheidsoordeel").ToAssessmentGrade();

            benchmarkTestInput.ExpectedSafetyAssessmentAssemblyResult.CombinedFailureMechanismProbabilitySpace =
                GetCellValueAsDouble("M", 10);

            var list = new List<FailureMechanismCategory>();
            var startRowCategories = GetRowId("Categorie") + 1;
            for (int iRow = startRowCategories; iRow <= startRowCategories + 5; iRow++)
            {
                list.Add(new FailureMechanismCategory(
                    GetCellValueAsString("D", iRow).ToFailureMechanismCategory(),
                    GetCellValueAsDouble("E", iRow),
                    GetCellValueAsDouble("F", iRow)));
            }

            benchmarkTestInput.ExpectedSafetyAssessmentAssemblyResult.ExpectedCombinedFailureMechanismCategoriesGroup1and2 = new CategoriesList<FailureMechanismCategory>(list);
        }
    }
}
