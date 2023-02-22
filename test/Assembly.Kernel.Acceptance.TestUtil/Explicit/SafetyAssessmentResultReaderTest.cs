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
using System.IO;
using Assembly.Kernel.Acceptance.TestUtil.Data.Input;
using Assembly.Kernel.Acceptance.TestUtil.IO;
using DocumentFormat.OpenXml.Packaging;
using NUnit.Framework;

namespace Assembly.Kernel.Acceptance.TestUtil.Explicit
{
    [TestFixture]
    [Explicit("Only for local use.")]
    public class SafetyAssessmentResultReaderTest : TestFileReaderTestBase
    {
        [Test]
        public void ReaderReadsInformationCorrectly()
        {
            string testFile = Path.Combine(BenchmarkTestHelper.GetTestDataPath("Assembly.Kernel.Acceptance.TestUtil"),
                                           "Benchmarktool assemblage - Veiligheidsoordeel.xlsx");

            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(testFile, false))
            {
                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                Dictionary<string, WorksheetPart> workSheetParts = ReadWorkSheetParts(workbookPart);
                WorksheetPart workSheetPart = workSheetParts["Veiligheidsoordeel"];

                var reader = new SafetyAssessmentFinalResultReader(workSheetPart, workbookPart);

                var result = new BenchmarkTestInput();

                reader.Read(result);

                ExpectedSafetyAssessmentAssemblyResult assemblyResult = result.ExpectedSafetyAssessmentAssemblyResult;
                Assert.AreEqual(0.64, assemblyResult.CombinedProbability, 1e-2);
                Assert.AreEqual(0.64, assemblyResult.CombinedProbabilityPartial, 1e-2);
                Assert.AreEqual(EExpectedAssessmentGrade.APlus, assemblyResult.CombinedAssessmentGrade);
                Assert.AreEqual(EExpectedAssessmentGrade.APlus, assemblyResult.CombinedAssessmentGradePartial);
            }
        }
    }
}