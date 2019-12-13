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

using System.IO;
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

            var assessmentSection = new BenchmarkTestInput
            {
                FileName = excelFileName,
                TestName = testName
            };

            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(excelFileName, false))
            {
                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                var workSheetParts = ExcelReaderHelper.ReadWorkSheetParts(workbookPart);

                ReadGeneralAssessmentSectionInformation(workSheetParts["Trajectgegevens"], workbookPart, assessmentSection);

                ReadFailureMechanism(workSheetParts["STBI"], workbookPart, assessmentSection);
                ReadFailureMechanism(workSheetParts["STBU"], workbookPart, assessmentSection);
                ReadFailureMechanism(workSheetParts["STPH"], workbookPart, assessmentSection);
                ReadFailureMechanism(workSheetParts["STMI"], workbookPart, assessmentSection);
                ReadFailureMechanism(workSheetParts["AGK"], workbookPart, assessmentSection);
                ReadFailureMechanism(workSheetParts["AWO"], workbookPart, assessmentSection);
                ReadFailureMechanism(workSheetParts["GEBU"], workbookPart, assessmentSection);
                ReadFailureMechanism(workSheetParts["GABU"], workbookPart, assessmentSection);
                ReadFailureMechanism(workSheetParts["GEKB"], workbookPart, assessmentSection);
                ReadFailureMechanism(workSheetParts["GABI"], workbookPart, assessmentSection);
                ReadFailureMechanism(workSheetParts["ZST"], workbookPart, assessmentSection);
                ReadFailureMechanism(workSheetParts["DA"], workbookPart, assessmentSection);
                ReadFailureMechanism(workSheetParts["HTKW"], workbookPart, assessmentSection);
                ReadFailureMechanism(workSheetParts["BSKW"], workbookPart, assessmentSection);
                ReadFailureMechanism(workSheetParts["PKW"], workbookPart, assessmentSection);
                ReadFailureMechanism(workSheetParts["STKWp"], workbookPart, assessmentSection);
                ReadFailureMechanism(workSheetParts["STKWl"], workbookPart, assessmentSection);
                ReadFailureMechanism(workSheetParts["VLGA"], workbookPart, assessmentSection);
                ReadFailureMechanism(workSheetParts["VLAF"], workbookPart, assessmentSection);
                ReadFailureMechanism(workSheetParts["VLZV"], workbookPart, assessmentSection);
                ReadFailureMechanism(workSheetParts["NWObe"], workbookPart, assessmentSection);
                ReadFailureMechanism(workSheetParts["NWObo"], workbookPart, assessmentSection);
                ReadFailureMechanism(workSheetParts["NWOkl"], workbookPart, assessmentSection);
                ReadFailureMechanism(workSheetParts["NWOoc"], workbookPart, assessmentSection);
                ReadFailureMechanism(workSheetParts["HAV"], workbookPart, assessmentSection);
                ReadFailureMechanism(workSheetParts["INN"], workbookPart, assessmentSection);

                ReadSafetyAssessmentFinalResult(workSheetParts["Gecombineerd veiligheidsoordeel"], workbookPart, assessmentSection);

                ReadCombinedAssessmentSectionResults(workSheetParts["Gecombineerd totaal vakoordeel"], workbookPart, assessmentSection);

                return assessmentSection;
            }
        }

        private static void ReadGeneralAssessmentSectionInformation(WorksheetPart workSheetPart,
            WorkbookPart workbookPart, BenchmarkTestInput benchmarkTestInput)
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

        private static void ReadFailureMechanism(WorksheetPart worksheetPart,
            WorkbookPart workbookPart, BenchmarkTestInput benchmarkTestInput)
        {
            new FailureMechanismsReader(worksheetPart, workbookPart).Read(benchmarkTestInput);
        }
    }
}
