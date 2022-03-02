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
using System.Linq;
using assembly.kernel.benchmark.tests.data.Input;
using assembly.kernel.benchmark.tests.data.Input.FailureMechanisms;
using assembly.kernel.benchmark.tests.io.Readers;
using Assembly.Kernel.Model;
using DocumentFormat.OpenXml.Packaging;
using NUnit.Framework;

namespace assembly.kernel.benchmark.tests.io.tests.Readers
{
    [TestFixture, Ignore("Broken due to shift to new kernel")]
    public class FailureMechanismsReaderTest : TestFileReaderTestBase
    {
        private string testFile;

        [SetUp]
        public void Setup()
        {
            testFile = Path.Combine(GetTestDir(), "Benchmarktool Excel assemblagetool (v1_0_1_0) 0_03.xlsm");
        }

        [Test]
        public void ReaderReadsGroup3InformationCorrectly()
        {
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(testFile, false))
            {
                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                var workSheetParts = ReadWorkSheetParts(workbookPart);
                var aGKWorkSheetPart = workSheetParts["AGK"];

                var reader = new FailureMechanismsReader(aGKWorkSheetPart, workbookPart);

                var result = new BenchmarkTestInput();

                reader.Read(result, "AGK");

                Assert.AreEqual(1, result.ExpectedFailureMechanismsResults.Count);
                ExpectedFailureMechanismResult expectedFailureMechanismResult = result.ExpectedFailureMechanismsResults.First();
                Assert.AreEqual(false, expectedFailureMechanismResult.HasLengthEffect);
                Assert.AreEqual("AGK", expectedFailureMechanismResult.MechanismId);
                /*Assert.AreEqual(EFailureMechanismCategory.VIIt, expectedFailureMechanismResult.ExpectedCombinedProbability);
                Assert.AreEqual(EFailureMechanismCategory.IVt, expectedFailureMechanismResult.ExpectedCombinedProbabilityTemporal);*/
            }
        }

        [Test]
        public void ReaderReadsGroup2InformationCorrectly()
        {
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(testFile, false))
            {
                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                var workSheetParts = ReadWorkSheetParts(workbookPart);
                var sTPHWorkSheetPart = workSheetParts["STPH"];

                var reader = new FailureMechanismsReader(sTPHWorkSheetPart, workbookPart);

                var result = new BenchmarkTestInput();

                reader.Read(result, "STPH");

                Assert.AreEqual(1, result.ExpectedFailureMechanismsResults.Count);
                ExpectedFailureMechanismResult expectedFailureMechanismResult =
                    result.ExpectedFailureMechanismsResults.First();
                Assert.AreEqual(true, expectedFailureMechanismResult.HasLengthEffect);
                Assert.AreEqual("STPH", expectedFailureMechanismResult.MechanismId);
                /*Assert.AreEqual(EFailureMechanismCategory.VIIt,
                    expectedFailureMechanismResult.ExpectedCombinedProbability);
                Assert.AreEqual(EFailureMechanismCategory.IIt,
                    expectedFailureMechanismResult.ExpectedCombinedProbabilityTemporal);*/
            }
        }

        [Test]
        public void ReaderReadsGroup1InformationCorrectly()
        {
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(testFile, false))
            {
                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                var workSheetParts = ReadWorkSheetParts(workbookPart);
                var gEKBWorkSheetPart = workSheetParts["GEKB"];

                var reader = new FailureMechanismsReader(gEKBWorkSheetPart, workbookPart);

                var result = new BenchmarkTestInput();

                reader.Read(result, "GEKB");

                Assert.AreEqual(1, result.ExpectedFailureMechanismsResults.Count);
                ExpectedFailureMechanismResult expectedFailureMechanismResult = result.ExpectedFailureMechanismsResults.First();
                Assert.AreEqual(false, expectedFailureMechanismResult.HasLengthEffect);
                Assert.AreEqual("GEKB", expectedFailureMechanismResult.MechanismId);
                /*Assert.AreEqual(EFailureMechanismCategory.VIIt, expectedFailureMechanismResult.ExpectedCombinedProbability);
                Assert.AreEqual(EFailureMechanismCategory.IIt, expectedFailureMechanismResult.ExpectedCombinedProbabilityTemporal);*/
            }
        }

        [Test]
        public void ReaderReadsGroup4InformationCorrectly()
        {
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(testFile, false))
            {
                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                var workSheetParts = ReadWorkSheetParts(workbookPart);
                var sTKWlWorkSheetPart = workSheetParts["STKWl"];

                var reader = new FailureMechanismsReader(sTKWlWorkSheetPart, workbookPart);

                var result = new BenchmarkTestInput();

                reader.Read(result, "STKWl");

                Assert.AreEqual(1, result.ExpectedFailureMechanismsResults.Count);
                ExpectedFailureMechanismResult expectedFailureMechanismResult = result.ExpectedFailureMechanismsResults.First();
                Assert.AreEqual(false, expectedFailureMechanismResult.HasLengthEffect);
                Assert.AreEqual("STKWl", expectedFailureMechanismResult.MechanismId);
                /*Assert.AreEqual(EFailureMechanismCategory.VIIt, expectedFailureMechanismResult.ExpectedCombinedProbability);
                Assert.AreEqual(EFailureMechanismCategory.IIt, expectedFailureMechanismResult.ExpectedCombinedProbabilityTemporal);*/
            }
        }

        [Test]
        public void ReaderReadsSTBUInformationCorrectly()
        {
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(testFile, false))
            {
                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                var workSheetParts = ReadWorkSheetParts(workbookPart);
                var sTBUWorkSheetPart = workSheetParts["STBU"];

                var reader = new FailureMechanismsReader(sTBUWorkSheetPart, workbookPart);

                var result = new BenchmarkTestInput();

                reader.Read(result,"STBU");

                Assert.AreEqual(1, result.ExpectedFailureMechanismsResults.Count);
                ExpectedFailureMechanismResult expectedFailureMechanismResult = result.ExpectedFailureMechanismsResults.First();
                Assert.AreEqual(true, expectedFailureMechanismResult.HasLengthEffect);
                Assert.AreEqual("STBU", expectedFailureMechanismResult.MechanismId);
                /*Assert.AreEqual(EFailureMechanismCategory.VIIt, expectedFailureMechanismResult.ExpectedCombinedProbability);
                Assert.AreEqual(EFailureMechanismCategory.Vt, expectedFailureMechanismResult.ExpectedCombinedProbabilityTemporal);
                */

                Assert.IsNotNull(expectedFailureMechanismResult);
                Assert.AreEqual(13.7, expectedFailureMechanismResult.LengthEffectFactor, 9e-2);
            }
        }
    }
}