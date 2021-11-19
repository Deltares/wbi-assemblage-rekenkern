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

using System;
using System.IO;
using System.Linq;
using assembly.kernel.benchmark.tests.data.Input;
using assembly.kernel.benchmark.tests.data.Input.FailureMechanisms;
using assembly.kernel.benchmark.tests.io.Readers;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.Categories;
using Assembly.Kernel.Model.FailurePaths;
using DocumentFormat.OpenXml.Packaging;
using NUnit.Framework;

namespace assembly.kernel.benchmark.tests.io.tests.Readers
{
    [TestFixture]
    public class CommonAssessmentSectionResultsReaderTest : TestFileReaderTestBase
    {
        private readonly MechanismType[] directMechanismTypes =
        {
            MechanismType.STBI,
            MechanismType.STBU,
            MechanismType.STPH,
            MechanismType.STMI,
            MechanismType.AGK,
            MechanismType.AWO,
            MechanismType.GEBU,
            MechanismType.GABU,
            MechanismType.GEKB,
            MechanismType.GABI,
            MechanismType.ZST,
            MechanismType.DA,
            MechanismType.HTKW,
            MechanismType.BSKW,
            MechanismType.PKW,
            MechanismType.STKWp,
            MechanismType.STKWl,
            MechanismType.INN
        };

        [Test]
        public void ReaderReadsInformationCorrectly()
        {
            var testFile = Path.Combine(GetTestDir(), "Benchmarktool Excel assemblagetool (v1_0_1_0) 0_03.xlsm");

            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(testFile, false))
            {
                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                var workSheetParts = ReadWorkSheetParts(workbookPart);
                var workSheetPart = workSheetParts["Gecombineerd totaal vakoordeel"];

                var reader = new CommonAssessmentSectionResultsReader(workSheetPart, workbookPart);

                var result = new BenchmarkTestInput();

                reader.Read(result);

                Assert.AreEqual(40, result.ExpectedCombinedSectionResult.Count());
                AssertResultsIsAsExpected(6700, 7100, EInterpretationCategory.Gr, result.ExpectedCombinedSectionResult.ElementAt(9));
                AssertResultsIsAsExpected(11800,  12100, EInterpretationCategory.Gr, result.ExpectedCombinedSectionResult.ElementAt(18));
                AssertResultsIsAsExpected(12100, 12700, EInterpretationCategory.Gr, result.ExpectedCombinedSectionResult.ElementAt(19));

                AssertResultsIsAsExpected(6700, 7100, EInterpretationCategory.Gr, result.ExpectedCombinedSectionResultTemporal.ElementAt(9));
                AssertResultsIsAsExpected(11800, 12100, EInterpretationCategory.Gr, result.ExpectedCombinedSectionResultTemporal.ElementAt(18));
                AssertResultsIsAsExpected(12100, 12700, EInterpretationCategory.Gr, result.ExpectedCombinedSectionResultTemporal.ElementAt(19));

                Assert.AreEqual(18, result.ExpectedCombinedSectionResultPerFailureMechanism.Count());
                /*foreach (var failureMechanismSectionList in result.ExpectedCombinedSectionResultPerFailureMechanism)
                {
                    Assert.AreEqual(40, failureMechanismSectionList.Sections.Count());
                    FailurePathSection ninethSection = failureMechanismSectionList.Sections.ElementAt(9);
                    var type = failureMechanismSectionList.FailurePathId.ToMechanismType();
                    if (ninethSection is FailurePathSectionWithCategory)
                    {
                        var sectionWithDirectCategory = (FailurePathSectionWithCategory) ninethSection;
                        AssertResultsIsAsExpected(6700, 7100, expectedDirectResults[Array.IndexOf(directMechanismTypes, type)],
                                                  sectionWithDirectCategory);
                    }
                }*/
            }
        }

        private void AssertResultsIsAsExpected(double start, double end, EInterpretationCategory category,
                                               FailurePathSectionWithCategory section)
        {
            Assert.AreEqual(start, section.SectionStart);
            Assert.AreEqual(end, section.SectionEnd);
            Assert.AreEqual(category, section.Category);
        }
    }
}