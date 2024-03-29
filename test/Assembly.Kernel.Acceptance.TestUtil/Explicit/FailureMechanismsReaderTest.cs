// Copyright (C) Stichting Deltares and State of the Netherlands 2023. All rights reserved.
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
// All names, logos, and references to "Deltares" are registered trademarks of
// Stichting Deltares and remain full property of Stichting Deltares at all times.
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
    [Explicit("Only for local use.")]
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

                Assert.AreEqual("STPH", expectedFailureMechanismResult.MechanismId);
                Assert.IsTrue(expectedFailureMechanismResult.HasLengthEffect);
                Assert.AreEqual("P1", expectedFailureMechanismResult.AssemblyMethod);
                Assert.IsFalse(expectedFailureMechanismResult.IsCorrelated);
                Assert.AreEqual(6.07e-2, expectedFailureMechanismResult.ExpectedCombinedProbability, 1e-4);
                Assert.AreEqual(6.07e-2, expectedFailureMechanismResult.ExpectedCombinedProbabilityPartial, 1e-4);

                Assert.AreEqual(3.31e-2, expectedFailureMechanismResult.ExpectedTheoreticalBoundaries.LowerLimit, 1e-4);
                Assert.AreEqual(6.07e-2, expectedFailureMechanismResult.ExpectedTheoreticalBoundaries.UpperLimit, 1e-4);
                Assert.AreEqual(3.31e-2, expectedFailureMechanismResult.ExpectedTheoreticalBoundariesPartial.LowerLimit, 1e-4);
                Assert.AreEqual(6.07e-2, expectedFailureMechanismResult.ExpectedTheoreticalBoundariesPartial.UpperLimit, 1e-4);
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
                ExpectedFailureMechanismResult expectedFailureMechanismResult = result.ExpectedFailureMechanismsResults.First();

                Assert.AreEqual("GEKB", expectedFailureMechanismResult.MechanismId);
                Assert.IsFalse(expectedFailureMechanismResult.HasLengthEffect);
                Assert.AreEqual("P2", expectedFailureMechanismResult.AssemblyMethod);
                Assert.IsTrue(expectedFailureMechanismResult.IsCorrelated);
                Assert.AreEqual(4.46e-6, expectedFailureMechanismResult.ExpectedCombinedProbability, 1e-4);
                Assert.AreEqual(4.46e-6, expectedFailureMechanismResult.ExpectedCombinedProbabilityPartial, 1e-4);

                Assert.AreEqual(2.23e-6, expectedFailureMechanismResult.ExpectedTheoreticalBoundaries.LowerLimit, 1e-4);
                Assert.AreEqual(1.26e-5, expectedFailureMechanismResult.ExpectedTheoreticalBoundaries.UpperLimit, 1e-4);
                Assert.AreEqual(2.23e-6, expectedFailureMechanismResult.ExpectedTheoreticalBoundariesPartial.LowerLimit, 1e-4);
                Assert.AreEqual(1.26e-5, expectedFailureMechanismResult.ExpectedTheoreticalBoundariesPartial.UpperLimit, 1e-4);
            }
        }
    }
}