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
using System.Linq;
using Assembly.Kernel.Acceptance.TestUtil.Data.Input;
using Assembly.Kernel.Acceptance.TestUtil.Data.Input.FailureMechanisms;
using Assembly.Kernel.Acceptance.TestUtil.IO;
using DocumentFormat.OpenXml.Packaging;
using NUnit.Framework;

namespace Assembly.Kernel.Acceptance.TestUtil.Explicit
{
    [TestFixture]
    public class FailureMechanismsReaderTest : TestFileReaderTestBase
    {
        [Test]
        public void ReaderReadsFailureMechanismWithLengthEffectInformationCorrectly()
        {
            string testFile = Path.Combine(BenchmarkTestHelper.GetTestDataPath("Assembly.Kernel.Acceptance.TestUtil"),
                                           "Benchmarktool assemblage - Failure mechanism with length-effect.xlsx");
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(testFile, false))
            {
                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                Dictionary<string, WorksheetPart> workSheetParts = ReadWorkSheetParts(workbookPart);
                WorksheetPart workSheetPart = workSheetParts["Sheet1"];

                var reader = new FailureMechanismsReader(workSheetPart, workbookPart);

                var result = new BenchmarkTestInput();

                reader.Read(result, "STPH");

                Assert.AreEqual(1, result.ExpectedFailureMechanismsResults.Count);
                ExpectedFailureMechanismResult expectedFailureMechanismResult = result.ExpectedFailureMechanismsResults.First();
                Assert.AreEqual(true, expectedFailureMechanismResult.HasLengthEffect);
                Assert.AreEqual("STPH", expectedFailureMechanismResult.MechanismId);
                Assert.AreEqual(6.07e-02, expectedFailureMechanismResult.ExpectedCombinedProbability, 1e-4);
                Assert.AreEqual(6.07e-02, expectedFailureMechanismResult.ExpectedCombinedProbabilityPartial, 1e-4);
            }
        }

        [Test]
        public void ReaderReadsFailureMechanismWithoutLengthEffectInformationCorrectly()
        {
            string testFile = Path.Combine(BenchmarkTestHelper.GetTestDataPath("Assembly.Kernel.Acceptance.TestUtil"),
                                           "Benchmarktool assemblage - Failure mechanism without length-effect.xlsx");
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(testFile, false))
            {
                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                Dictionary<string, WorksheetPart> workSheetParts = ReadWorkSheetParts(workbookPart);
                WorksheetPart workSheetPart = workSheetParts["Sheet1"];

                var reader = new FailureMechanismsReader(workSheetPart, workbookPart);

                var result = new BenchmarkTestInput();

                reader.Read(result, "GEKB");

                Assert.AreEqual(1, result.ExpectedFailureMechanismsResults.Count);
                ExpectedFailureMechanismResult expectedFailureMechanismResult =
                    result.ExpectedFailureMechanismsResults.First();
                Assert.AreEqual(false, expectedFailureMechanismResult.HasLengthEffect);
                Assert.AreEqual("GEKB", expectedFailureMechanismResult.MechanismId);
                Assert.AreEqual(4.46e-06, expectedFailureMechanismResult.ExpectedCombinedProbability, 1e-4);
                Assert.AreEqual(4.46e-06, expectedFailureMechanismResult.ExpectedCombinedProbabilityPartial, 1e-4);
            }
        }
    }
}