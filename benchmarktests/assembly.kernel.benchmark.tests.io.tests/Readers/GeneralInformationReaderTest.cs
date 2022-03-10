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

                Assert.AreEqual(1 / 3000.0, result.SignalingNorm, 1e-8);
                Assert.AreEqual(1 / 1000.0, result.LowerBoundaryNorm, 1e-8);
                Assert.AreEqual(10400, result.Length, 1e-8);

                var assessmentGradeCategories = result.ExpectedAssessmentSectionCategories.Categories;
                Assert.AreEqual(5, assessmentGradeCategories.Length);
                AssertAreEqualGradeCategories(EAssessmentGrade.APlus, 0.0, result.SignalingNorm / 30.0, assessmentGradeCategories[0]);
                AssertAreEqualGradeCategories(EAssessmentGrade.A, result.SignalingNorm / 30.0, result.SignalingNorm, assessmentGradeCategories[1]);
                AssertAreEqualGradeCategories(EAssessmentGrade.B, result.SignalingNorm, result.LowerBoundaryNorm, assessmentGradeCategories[2]);
                AssertAreEqualGradeCategories(EAssessmentGrade.C, result.LowerBoundaryNorm, result.LowerBoundaryNorm * 30.0, assessmentGradeCategories[3]);
                AssertAreEqualGradeCategories(EAssessmentGrade.D, result.LowerBoundaryNorm * 30.0, 1.0, assessmentGradeCategories[4]);

                var interpretationCategories = result.ExpectedInterpretationCategories.Categories;
                Assert.AreEqual(7, interpretationCategories.Length);
                AssertAreEqualInterpretationCategories(EInterpretationCategory.III, 0.0, result.SignalingNorm / 1000.0, interpretationCategories[0]);
                AssertAreEqualInterpretationCategories(EInterpretationCategory.II, result.SignalingNorm / 1000.0, result.SignalingNorm / 100.0, interpretationCategories[1]);
                AssertAreEqualInterpretationCategories(EInterpretationCategory.I, result.SignalingNorm / 100.0, result.SignalingNorm / 10.0, interpretationCategories[2]);
                AssertAreEqualInterpretationCategories(EInterpretationCategory.Zero, result.SignalingNorm / 10.0, result.SignalingNorm, interpretationCategories[3]);
                AssertAreEqualInterpretationCategories(EInterpretationCategory.IMin, result.SignalingNorm, result.LowerBoundaryNorm, interpretationCategories[4]);
                AssertAreEqualInterpretationCategories(EInterpretationCategory.IIMin, result.LowerBoundaryNorm, result.LowerBoundaryNorm * 10.0, interpretationCategories[5]);
                AssertAreEqualInterpretationCategories(EInterpretationCategory.IIIMin, result.LowerBoundaryNorm * 10.0, 1.0, interpretationCategories[6]);
            }
        }

        private void AssertAreEqualGradeCategories(EAssessmentGrade expectedCategory, double expectedLowerLimit,
                                              double expectedUpperLimit, AssessmentSectionCategory assessmentSectionCategory)
        {
            Assert.AreEqual(expectedCategory, assessmentSectionCategory.Category);
            Assert.AreEqual(expectedLowerLimit, assessmentSectionCategory.LowerLimit, 1e-15);
            Assert.AreEqual(expectedUpperLimit, assessmentSectionCategory.UpperLimit, 1e-15);
        }

        private void AssertAreEqualInterpretationCategories(EInterpretationCategory expectedCategory, double expectedLowerLimit,
            double expectedUpperLimit, InterpretationCategory interpretationCategory)
        {
            Assert.AreEqual(expectedCategory, interpretationCategory.Category);
            Assert.AreEqual(expectedLowerLimit, interpretationCategory.LowerLimit, 1e-15);
            Assert.AreEqual(expectedUpperLimit, interpretationCategory.UpperLimit, 1e-15);
        }
    }
}