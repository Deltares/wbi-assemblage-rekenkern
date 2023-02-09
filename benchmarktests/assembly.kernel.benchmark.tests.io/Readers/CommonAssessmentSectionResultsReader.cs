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

using System.Collections.Generic;
using System.Linq;
using assembly.kernel.benchmark.tests.data.Input;
using Assembly.Kernel.Model.FailureMechanismSections;
using DocumentFormat.OpenXml.Packaging;

namespace assembly.kernel.benchmark.tests.io.Readers
{
    /// <summary>
    /// Reader to read common assessment section results.
    /// </summary>
    public class CommonAssessmentSectionResultsReader : ExcelSheetReaderBase
    {
        private const int CommonSectionsHeaderRowId = 2;
        private const double KilometersToMeters = 1000.0;

        private readonly string[] columnStrings =
        {
            "A",
            "B",
            "C",
            "D",
            "E",
            "F",
            "G",
            "H",
            "I",
            "J",
            "K",
            "L",
            "M",
            "N",
            "O",
            "P",
            "Q",
            "R",
            "S",
            "T",
            "U",
            "V",
            "W",
            "X",
            "Y",
            "Z",
            "AA",
            "AB",
            "AC",
            "AD",
            "AE"
        };

        /// <summary>
        /// Creates a new instance of <see cref="CommonAssessmentSectionResultsReader"/>.
        /// </summary>
        /// <param name="worksheetPart">The WorksheetPart that contains information on the combined assessment section sections.</param>
        /// <param name="workbookPart">The workbook containing the specified worksheet.</param>
        public CommonAssessmentSectionResultsReader(WorksheetPart worksheetPart, WorkbookPart workbookPart) : base(
            worksheetPart, workbookPart,"B") {}

        /// <summary>
        /// Reads the input and expected output of assembly of the combined section results.
        /// </summary>
        /// <param name="benchmarkTestInput">The input to set the results on.</param>
        public void Read(BenchmarkTestInput benchmarkTestInput)
        {
            var failureMechanismSpecificCommonSectionsWithResults = new Dictionary<string, List<FailureMechanismSectionWithCategory>>();
            foreach (var failureMechanismsKey in benchmarkTestInput.ExpectedFailureMechanismsResults.Select(r => r.MechanismId).ToArray())
            {
                failureMechanismSpecificCommonSectionsWithResults[failureMechanismsKey] = new List<FailureMechanismSectionWithCategory>();
            }

            var columnKeys = MatchColumnNamesWithFailureMechanismCodes();

            int iRow = 3;
            while (iRow <= MaxRow)
            {
                var startMeters = GetCellValueAsDouble("B", iRow) * KilometersToMeters;
                var endMeters = GetCellValueAsDouble("C", iRow) * KilometersToMeters;
                if (double.IsNaN(startMeters) || double.IsNaN(endMeters))
                {
                    break;
                }

                AddSectionToList(benchmarkTestInput.ExpectedCombinedSectionResult, "D", iRow, startMeters, endMeters);
                AddSectionToList(benchmarkTestInput.ExpectedCombinedSectionResultPartial, "E", iRow, startMeters, endMeters);
                foreach (var keyValuePair in failureMechanismSpecificCommonSectionsWithResults)
                {
                    AddSectionToList(keyValuePair.Value, columnKeys[keyValuePair.Key], iRow, startMeters, endMeters);
                }

                iRow++;
            }

            benchmarkTestInput.ExpectedCombinedSectionResultPerFailureMechanism.AddRange(
                failureMechanismSpecificCommonSectionsWithResults.Select(kv =>
                    new FailureMechanismSectionListWithFailureMechanismId(kv.Key, kv.Value)));
        }

        private void AddSectionToList(List<FailureMechanismSectionWithCategory> list, string columnReference, int iRow,
                                      double startMeters, double endMeters)
        {
            list.Add(new FailureMechanismSectionWithCategory(startMeters, endMeters, GetCellValueAsString(columnReference, iRow).ToInterpretationCategory()));
        }

        private Dictionary<string, string> MatchColumnNamesWithFailureMechanismCodes()
        {
            var dict = new Dictionary<string, string>();
            foreach (var columnString in columnStrings.Skip(4))
            {
                var type = GetCellValueAsString(columnString, CommonSectionsHeaderRowId);
                dict[type] = columnString;

            }
            return dict;
        }
    }
}