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
using Assembly.Kernel.Acceptance.TestUtil.IO;
using Assembly.Kernel.Model.Categories;
using DocumentFormat.OpenXml.Packaging;
using NUnit.Framework;

namespace Assembly.Kernel.Acceptance.TestUtil.Explicit
{
    [TestFixture]
    [Explicit("Only for local use.")]
    public class GeneralInformationReaderTest : TestFileReaderTestBase
    {
        [Test]
        public void ReaderReadsInformationCorrectly()
        {
            string testFile = Path.Combine(BenchmarkTestHelper.GetTestDataPath("Assembly.Kernel.Acceptance.TestUtil"),
                                           "Benchmarktool Excel assemblagetool - General information.xlsx");

            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(testFile, false))
            {
                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                WorksheetPart workSheetPart = workbookPart.WorksheetParts.First();

                var reader = new GeneralInformationReader(workSheetPart, workbookPart);

                var result = new BenchmarkTestInput();

                reader.Read(result);

                Assert.AreEqual(1 / 3000.0, result.SignalFloodingProbability, 1e-8);
                Assert.AreEqual(1 / 1000.0, result.MaximumAllowableFloodingProbability, 1e-8);
                Assert.AreEqual(10400, result.Length, 1e-8);

                IEnumerable<AssessmentSectionCategory> assessmentGradeCategories = result.ExpectedAssessmentSectionCategories.Categories;
                Assert.AreEqual(5, assessmentGradeCategories.Count());
                AssertAreEqualGradeCategories(EAssessmentGrade.APlus, 0.0, result.SignalFloodingProbability / 30.0, assessmentGradeCategories.ElementAt(0));
                AssertAreEqualGradeCategories(EAssessmentGrade.A, result.SignalFloodingProbability / 30.0, result.SignalFloodingProbability, assessmentGradeCategories.ElementAt(1));
                AssertAreEqualGradeCategories(EAssessmentGrade.B, result.SignalFloodingProbability, result.MaximumAllowableFloodingProbability, assessmentGradeCategories.ElementAt(2));
                AssertAreEqualGradeCategories(EAssessmentGrade.C, result.MaximumAllowableFloodingProbability, result.MaximumAllowableFloodingProbability * 30.0, assessmentGradeCategories.ElementAt(3));
                AssertAreEqualGradeCategories(EAssessmentGrade.D, result.MaximumAllowableFloodingProbability * 30.0, 1.0, assessmentGradeCategories.ElementAt(4));

                IEnumerable<InterpretationCategory> interpretationCategories = result.ExpectedInterpretationCategories.Categories;
                Assert.AreEqual(7, interpretationCategories.Count());
                AssertAreEqualInterpretationCategories(EInterpretationCategory.III, 0.0, result.SignalFloodingProbability / 1000.0, interpretationCategories.ElementAt(0));
                AssertAreEqualInterpretationCategories(EInterpretationCategory.II, result.SignalFloodingProbability / 1000.0, result.SignalFloodingProbability / 100.0, interpretationCategories.ElementAt(1));
                AssertAreEqualInterpretationCategories(EInterpretationCategory.I, result.SignalFloodingProbability / 100.0, result.SignalFloodingProbability / 10.0, interpretationCategories.ElementAt(2));
                AssertAreEqualInterpretationCategories(EInterpretationCategory.Zero, result.SignalFloodingProbability / 10.0, result.SignalFloodingProbability, interpretationCategories.ElementAt(3));
                AssertAreEqualInterpretationCategories(EInterpretationCategory.IMin, result.SignalFloodingProbability, result.MaximumAllowableFloodingProbability, interpretationCategories.ElementAt(4));
                AssertAreEqualInterpretationCategories(EInterpretationCategory.IIMin, result.MaximumAllowableFloodingProbability, result.MaximumAllowableFloodingProbability * 10.0, interpretationCategories.ElementAt(5));
                AssertAreEqualInterpretationCategories(EInterpretationCategory.IIIMin, result.MaximumAllowableFloodingProbability * 10.0, 1.0, interpretationCategories.ElementAt(6));
            }
        }

        private static void AssertAreEqualGradeCategories(EAssessmentGrade expectedCategory, double expectedLowerLimit,
                                                          double expectedUpperLimit, AssessmentSectionCategory assessmentSectionCategory)
        {
            Assert.AreEqual(expectedCategory, assessmentSectionCategory.Category);
            Assert.AreEqual(expectedLowerLimit, assessmentSectionCategory.LowerLimit, 1e-15);
            Assert.AreEqual(expectedUpperLimit, assessmentSectionCategory.UpperLimit, 1e-15);
        }

        private static void AssertAreEqualInterpretationCategories(EInterpretationCategory expectedCategory, double expectedLowerLimit,
                                                                   double expectedUpperLimit, InterpretationCategory interpretationCategory)
        {
            Assert.AreEqual(expectedCategory, interpretationCategory.Category);
            Assert.AreEqual(expectedLowerLimit, interpretationCategory.LowerLimit, 1e-15);
            Assert.AreEqual(expectedUpperLimit, interpretationCategory.UpperLimit, 1e-15);
        }
    }
}