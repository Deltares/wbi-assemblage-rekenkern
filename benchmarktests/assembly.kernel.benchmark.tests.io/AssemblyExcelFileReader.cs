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

using System.IO;
using System.Linq;
using assembly.kernel.benchmark.tests.data.Input;
using assembly.kernel.benchmark.tests.io.Readers;
using DocumentFormat.OpenXml.Packaging;

namespace assembly.kernel.benchmark.tests.io
{
    /// <summary>
    /// Reader to read the assembly excel file.
    /// </summary>
    public static class AssemblyExcelFileReader
    {
        /// <summary>
        /// Creates a new instance of <see cref="BenchmarkTestInput"/>.
        /// </summary>
        /// <param name="excelFileName">The name of the excel file.</param>
        /// <param name="testName">The test name.</param>
        /// <returns>A <see cref="BenchmarkTestInput"/>.</returns>
        public static BenchmarkTestInput Read(string excelFileName, string testName)
        {
            if (!File.Exists(excelFileName))
            {
                return null;
            }

            var benchmarkTestInput = new BenchmarkTestInput
            {
                FileName = excelFileName,
                TestName = testName
            };

            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(excelFileName, false))
            {
                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                var workSheetParts = ExcelReaderHelper.ReadWorkSheetParts(workbookPart);

                ReadGeneralAssessmentSectionInformation(workSheetParts["Normen en duidingsklassen"], workbookPart, benchmarkTestInput);

                var tabsToIgnore = new[]
                {
                    "Informatiepagina", "Normen en duidingsklassen", "Veiligheidsoordeel", "Gecombineerd vakoordeel"
                };
                var failureMechanismsTabs = workSheetParts.Select(wsp => wsp.Key).Except(tabsToIgnore).ToArray();

                foreach (var failureMechanismsTab in failureMechanismsTabs)
                {
                    ReadFailureMechanism(failureMechanismsTab, workSheetParts[failureMechanismsTab],workbookPart, benchmarkTestInput);
                }

                ReadSafetyAssessmentFinalResult(workSheetParts["Veiligheidsoordeel"], workbookPart, benchmarkTestInput);
                ReadCombinedAssessmentSectionResults(workSheetParts["Gecombineerd vakoordeel"], workbookPart, benchmarkTestInput);

                return benchmarkTestInput;
            }
        }

        private static void ReadGeneralAssessmentSectionInformation(WorksheetPart workSheetPart,
                                                                    WorkbookPart workbookPart,
                                                                    BenchmarkTestInput benchmarkTestInput)
        {
            new GeneralInformationReader(workSheetPart, workbookPart).Read(benchmarkTestInput);
        }

        private static void ReadSafetyAssessmentFinalResult(WorksheetPart worksheetPart, WorkbookPart workbookPart,
                                                            BenchmarkTestInput benchmarkTestInput)
        {
            new SafetyAssessmentFinalResultReader(worksheetPart, workbookPart).Read(benchmarkTestInput);
        }

        private static void ReadCombinedAssessmentSectionResults(WorksheetPart worksheetPart,
                                                                 WorkbookPart workbookPart, BenchmarkTestInput benchmarkTestInput)
        {
            new CommonAssessmentSectionResultsReader(worksheetPart, workbookPart).Read(benchmarkTestInput);
        }

        private static void ReadFailureMechanism(string mechanismId, WorksheetPart worksheetPart,
                                                 WorkbookPart workbookPart, BenchmarkTestInput benchmarkTestInput)
        {
            new FailureMechanismsReader(worksheetPart, workbookPart).Read(benchmarkTestInput, mechanismId);
        }
    }
}