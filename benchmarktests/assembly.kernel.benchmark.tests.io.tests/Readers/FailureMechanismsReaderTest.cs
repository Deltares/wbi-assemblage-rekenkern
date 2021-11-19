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
using Assembly.Kernel.Model.FmSectionTypes;
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

                reader.Read(result);

                Assert.AreEqual(1, result.ExpectedFailureMechanismsResults.Count);
                IExpectedFailureMechanismResult expectedFailureMechanismResult = result.ExpectedFailureMechanismsResults.First();
                Assert.AreEqual(3, expectedFailureMechanismResult.Group);
                Assert.AreEqual(MechanismType.AGK, expectedFailureMechanismResult.Type);
                Assert.AreEqual(true, expectedFailureMechanismResult.AccountForDuringAssembly);
                Assert.AreEqual(EFailureMechanismCategory.VIIt, expectedFailureMechanismResult.ExpectedAssessmentResult);
                Assert.AreEqual(EFailureMechanismCategory.IVt, expectedFailureMechanismResult.ExpectedAssessmentResultTemporal);
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

                reader.Read(result);

                Assert.AreEqual(1, result.ExpectedFailureMechanismsResults.Count);
                IExpectedFailureMechanismResult expectedFailureMechanismResult =
                    result.ExpectedFailureMechanismsResults.First();
                Assert.AreEqual(2, expectedFailureMechanismResult.Group);
                Assert.AreEqual(MechanismType.STPH, expectedFailureMechanismResult.Type);
                Assert.AreEqual(true, expectedFailureMechanismResult.AccountForDuringAssembly);
                Assert.AreEqual(EFailureMechanismCategory.VIIt,
                    expectedFailureMechanismResult.ExpectedAssessmentResult);
                Assert.AreEqual(EFailureMechanismCategory.IIt,
                    expectedFailureMechanismResult.ExpectedAssessmentResultTemporal);
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

                reader.Read(result);

                Assert.AreEqual(1, result.ExpectedFailureMechanismsResults.Count);
                IExpectedFailureMechanismResult expectedFailureMechanismResult = result.ExpectedFailureMechanismsResults.First();
                Assert.AreEqual(1, expectedFailureMechanismResult.Group);
                Assert.AreEqual(MechanismType.GEKB, expectedFailureMechanismResult.Type);
                Assert.AreEqual(true, expectedFailureMechanismResult.AccountForDuringAssembly);
                Assert.AreEqual(EFailureMechanismCategory.VIIt, expectedFailureMechanismResult.ExpectedAssessmentResult);
                Assert.AreEqual(EFailureMechanismCategory.IIt, expectedFailureMechanismResult.ExpectedAssessmentResultTemporal);
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

                reader.Read(result);

                Assert.AreEqual(1, result.ExpectedFailureMechanismsResults.Count);
                IExpectedFailureMechanismResult expectedFailureMechanismResult = result.ExpectedFailureMechanismsResults.First();
                Assert.AreEqual(4, expectedFailureMechanismResult.Group);
                Assert.AreEqual(MechanismType.STKWl, expectedFailureMechanismResult.Type);
                Assert.AreEqual(true, expectedFailureMechanismResult.AccountForDuringAssembly);
                Assert.AreEqual(EFailureMechanismCategory.VIIt, expectedFailureMechanismResult.ExpectedAssessmentResult);
                Assert.AreEqual(EFailureMechanismCategory.IIt, expectedFailureMechanismResult.ExpectedAssessmentResultTemporal);
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

                reader.Read(result);

                Assert.AreEqual(1, result.ExpectedFailureMechanismsResults.Count);
                IExpectedFailureMechanismResult expectedFailureMechanismResult = result.ExpectedFailureMechanismsResults.First();
                Assert.AreEqual(4, expectedFailureMechanismResult.Group);
                Assert.AreEqual(MechanismType.STBU, expectedFailureMechanismResult.Type);
                Assert.AreEqual(true, expectedFailureMechanismResult.AccountForDuringAssembly);
                Assert.AreEqual(EFailureMechanismCategory.VIIt, expectedFailureMechanismResult.ExpectedAssessmentResult);
                Assert.AreEqual(EFailureMechanismCategory.Vt, expectedFailureMechanismResult.ExpectedAssessmentResultTemporal);

                var stbuFailureMechanism = expectedFailureMechanismResult as StbuExpectedFailureMechanismResult;
                Assert.IsNotNull(stbuFailureMechanism);
                Assert.AreEqual(0.04, stbuFailureMechanism.FailureMechanismProbabilitySpace);
                Assert.AreEqual(13.7, stbuFailureMechanism.LengthEffectFactor, 9e-2);
                AssertAreEqualProbabilities(9.71e-6, stbuFailureMechanism.ExpectedSectionsCategoryDivisionProbability);
            }
        }

        [Test]
        public void ReaderReadsGroup5InformationCorrectly()
        {
            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(testFile, false))
            {
                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                var workSheetParts = ReadWorkSheetParts(workbookPart);
                var vLGAWorkSheetPart = workSheetParts["VLGA"];

                var reader = new FailureMechanismsReader(vLGAWorkSheetPart, workbookPart);

                var result = new BenchmarkTestInput();

                reader.Read(result);

                Assert.AreEqual(1, result.ExpectedFailureMechanismsResults.Count);
                IExpectedFailureMechanismResult expectedFailureMechanismResult = result.ExpectedFailureMechanismsResults.First();
                Assert.AreEqual(5, expectedFailureMechanismResult.Group);
                Assert.AreEqual(MechanismType.VLGA, expectedFailureMechanismResult.Type);
                Assert.AreEqual(true, expectedFailureMechanismResult.AccountForDuringAssembly);
                Assert.AreEqual(EIndirectAssessmentResult.Ngo, expectedFailureMechanismResult.ExpectedAssessmentResult);
                Assert.AreEqual(EIndirectAssessmentResult.FactoredInOtherFailureMechanism, expectedFailureMechanismResult.ExpectedAssessmentResultTemporal);
            }
        }
    }
}