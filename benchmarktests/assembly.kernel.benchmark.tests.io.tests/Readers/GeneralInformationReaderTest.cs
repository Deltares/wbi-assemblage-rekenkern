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
using assembly.kernel.benchmark.tests.io.Readers;
using Assembly.Kernel.Model.Categories;
using DocumentFormat.OpenXml.Packaging;
using NUnit.Framework;

namespace assembly.kernel.benchmark.tests.io.tests.Readers
{
    [TestFixture]
    public class GeneralInformationReaderTest : TestFileReaderTestBase
    {
        [Test]
        public void ReaderReadsInformationCorrectly()
        {
            var testFile = Path.Combine(GetTestDir(), "Benchmarktool Excel assemblagetool - General information.xlsx");

            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(testFile, false))
            {
                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                WorksheetPart workSheetPart = workbookPart.WorksheetParts.First();

                var reader = new GeneralInformationReader(workSheetPart, workbookPart);

                var result = new BenchmarkTestInput();

                reader.Read(result);

                Assert.AreEqual(1 / 3000.0, result.SignallingNorm, 1e-8);
                Assert.AreEqual(1 / 1000.0, result.LowerBoundaryNorm, 1e-8);
                Assert.AreEqual(10.4, result.Length, 1e-8);

                var categories = result.ExpectedAssessmentSectionCategories.Categories;
                Assert.AreEqual(5, categories.Length);
                AssertAreEqualCategories(EAssessmentGrade.APlus, 0.0, result.SignallingNorm / 30.0, categories[0]);
                AssertAreEqualCategories(EAssessmentGrade.A, result.SignallingNorm / 30.0, result.SignallingNorm, categories[1]);
                AssertAreEqualCategories(EAssessmentGrade.B, result.SignallingNorm, result.LowerBoundaryNorm, categories[2]);
                AssertAreEqualCategories(EAssessmentGrade.C, result.LowerBoundaryNorm, result.LowerBoundaryNorm * 30.0, categories[3]);
                AssertAreEqualCategories(EAssessmentGrade.D, result.LowerBoundaryNorm * 30.0, 1.0, categories[4]);
            }
        }

        private void AssertAreEqualCategories(EAssessmentGrade expectedCategory, double expectedLowerLimit,
                                              double expectedUpperLimit, AssessmentSectionCategory assessmentSectionCategory)
        {
            Assert.AreEqual(expectedCategory, assessmentSectionCategory.Category);
            Assert.AreEqual(expectedLowerLimit, assessmentSectionCategory.LowerLimit);
            Assert.AreEqual(expectedUpperLimit, assessmentSectionCategory.UpperLimit);
        }
    }
}