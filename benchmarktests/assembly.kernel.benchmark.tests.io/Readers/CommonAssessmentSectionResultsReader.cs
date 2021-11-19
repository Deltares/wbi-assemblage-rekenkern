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

using System;
using System.Collections.Generic;
using System.Linq;
using assembly.kernel.benchmark.tests.data.Input;
using assembly.kernel.benchmark.tests.data.Input.FailureMechanisms;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.FmSectionTypes;
using DocumentFormat.OpenXml.Packaging;

namespace assembly.kernel.benchmark.tests.io.Readers
{
    /// <summary>
    /// Reader to read common assessment section results.
    /// </summary>
    public class CommonAssessmentSectionResultsReader : ExcelSheetReaderBase
    {
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

        private readonly Dictionary<MechanismType, bool> failureMechanisms = new Dictionary<MechanismType, bool>
        {
            {MechanismType.STBI, true},
            {MechanismType.STBU, true},
            {MechanismType.STPH, true},
            {MechanismType.STMI, true},
            {MechanismType.AGK, true},
            {MechanismType.AWO, true},
            {MechanismType.GEBU, true},
            {MechanismType.GABU, true},
            {MechanismType.GEKB, true},
            {MechanismType.GABI, true},
            {MechanismType.ZST, true},
            {MechanismType.DA, true},
            {MechanismType.HTKW, true},
            {MechanismType.BSKW, true},
            {MechanismType.PKW, true},
            {MechanismType.STKWp, true},
            {MechanismType.STKWl, true},
            {MechanismType.VLGA, false},
            {MechanismType.VLAF, false},
            {MechanismType.VLZV, false},
            {MechanismType.NWObe, false},
            {MechanismType.NWObo, false},
            {MechanismType.NWOkl, false},
            {MechanismType.NWOoc, false},
            {MechanismType.HAV, false},
            {MechanismType.INN, true}
        };

        /// <summary>
        /// Creates a new instance of <see cref="CommonAssessmentSectionResultsReader"/>.
        /// </summary>
        /// <param name="worksheetPart">The WorksheetPart that contains information on the combined assessment section sections</param>
        /// <param name="workbookPart">The workbook containing the specified worksheet</param>
        public CommonAssessmentSectionResultsReader(WorksheetPart worksheetPart, WorkbookPart workbookPart) : base(
            worksheetPart, workbookPart) {}

        /// <summary>
        /// Reads the input and expected output of assembly of the combined section results.
        /// </summary>
        /// <param name="benchmarkTestInput">The input to set the results on.</param>
        public void Read(BenchmarkTestInput benchmarkTestInput)
        {
            var commonSections = new List<FailurePathSectionWithResult>();
            var commonSectionsTemporal = new List<FailurePathSectionWithResult>();

            var failureMechanismSpecificCommonSectionsWithDirectResults = new Dictionary<MechanismType, List<FailurePathSectionWithResult>>();
            foreach (var failureMechanismsKey in failureMechanisms.Keys)
            {
                if (failureMechanisms[failureMechanismsKey])
                {
                    failureMechanismSpecificCommonSectionsWithDirectResults[failureMechanismsKey] = new List<FailurePathSectionWithResult>();
                }
            }

            var columnKeys = GetColumnKeys(3);

            int iRow = 4;
            while (iRow <= MaxRow)
            {
                var startMeters = GetCellValueAsDouble("A", iRow) * 1000.0;
                var endMeters = GetCellValueAsDouble("B", iRow) * 1000.0;
                if (double.IsNaN(startMeters) || double.IsNaN(endMeters))
                {
                    break;
                }

                AddSectionToList(commonSections, "C", iRow, startMeters, endMeters);
                AddSectionToList(commonSectionsTemporal, "D", iRow, startMeters, endMeters);
                foreach (var directResultPair in failureMechanismSpecificCommonSectionsWithDirectResults)
                {
                    AddSectionToList(directResultPair.Value, columnKeys[directResultPair.Key], iRow, startMeters, endMeters);
                }

                iRow++;
            }

            benchmarkTestInput.ExpectedCombinedSectionResult = commonSections;
            benchmarkTestInput.ExpectedCombinedSectionResultTemporal = commonSectionsTemporal;

            var resultsPerFailureMechanism =
                failureMechanismSpecificCommonSectionsWithDirectResults.Select(kv =>
                                                                                   new FailurePathSectionList(
                                                                                       kv.Key.ToString("D"), kv.Value));

            benchmarkTestInput.ExpectedCombinedSectionResultPerFailureMechanism = resultsPerFailureMechanism;
        }

        private void AddSectionToList(List<FailurePathSectionWithResult> list, string columnReference, int iRow,
                                      double startMeters, double endMeters)
        {
            var category = GetCellValueAsString(columnReference, iRow).ToFailureMechanismSectionCategory();
            list.Add(new FailurePathSectionWithResult(startMeters, endMeters, EInterpretationCategory.Gr));
        }

        private Dictionary<MechanismType, string> GetColumnKeys(int iRow)
        {
            var dict = new Dictionary<MechanismType, string>();
            foreach (var columnString in columnStrings.Skip(4))
            {
                try
                {
                    var type = GetCellValueAsString(columnString, iRow).ToMechanismType();
                    dict[type] = columnString;
                }
                catch (Exception e)
                {
                    // Skip column. Not important
                }
            }

            return dict;
        }
    }
}