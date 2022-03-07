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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using assembly.kernel.benchmark.tests.data.Input;
using assembly.kernel.benchmark.tests.data.Input.FailureMechanisms;
using assembly.kernel.benchmark.tests.data.Input.FailureMechanismSections;
using assembly.kernel.benchmark.tests.io.Readers;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.Categories;
using Assembly.Kernel.Model.FailureMechanismSections;
using DocumentFormat.OpenXml.Packaging;
using NUnit.Framework;

namespace assembly.kernel.benchmark.tests.io.tests.Readers
{
    [TestFixture]
    public class CommonAssessmentSectionResultsReaderTest : TestFileReaderTestBase
    {
        private readonly Dictionary<string, EInterpretationCategory> expectedDirectResults =
            new Dictionary<string, EInterpretationCategory>
            {
                {"STPH", EInterpretationCategory.I},
                {"STBI", EInterpretationCategory.III},
                {"GEKB", EInterpretationCategory.II},
                {"DA", EInterpretationCategory.III},
                {"STKWp", EInterpretationCategory.III},
                {"BSKW", EInterpretationCategory.III},
                {"HTKW", EInterpretationCategory.III}
            };

        [Test]
        public void ReaderReadsInformationCorrectly()
        {
            var testFile = Path.Combine(GetTestDir(), "Benchmarktool Excel assemblage - Gecombineerd vakoordeeel.xlsx");

            using (SpreadsheetDocument spreadsheetDocument = SpreadsheetDocument.Open(testFile, false))
            {
                WorkbookPart workbookPart = spreadsheetDocument.WorkbookPart;
                var workSheetParts = ReadWorkSheetParts(workbookPart);
                var workSheetPart = workSheetParts["Gecombineerd vakoordeel"];

                var reader = new CommonAssessmentSectionResultsReader(workSheetPart, workbookPart);

                var result = new BenchmarkTestInput();
                result.ExpectedFailureMechanismsResults.AddRange(new[]
                {
                    new ExpectedFailureMechanismResult("Piping", "STPH", true),
                    new ExpectedFailureMechanismResult("Macrostabiliteit binnen", "STBI", true),
                    new ExpectedFailureMechanismResult("Graserosie kruin en binnentalud", "GEKB", true),
                    new ExpectedFailureMechanismResult("Duinafslag", "DA", true),
                    new ExpectedFailureMechanismResult("Kunstwerken puntconstructies", "STKWp", true),
                    new ExpectedFailureMechanismResult("Betrouwbaarheid sluiting kunstwerk", "BSKW", true),
                    new ExpectedFailureMechanismResult("Hoogte kunstwerk", "HTKW", true),
                });

                reader.Read(result);

                Assert.AreEqual(104, result.ExpectedCombinedSectionResult.Count());
                AssertResultsIsAsExpected(342.187662, 910, EInterpretationCategory.III, result.ExpectedCombinedSectionResult.ElementAt(9));
                AssertResultsIsAsExpected(2519.652041,  3010, EInterpretationCategory.I, result.ExpectedCombinedSectionResult.ElementAt(18));
                AssertResultsIsAsExpected(3010, 3313.767881, EInterpretationCategory.I, result.ExpectedCombinedSectionResult.ElementAt(19));

                AssertResultsIsAsExpected(342.187662, 910, EInterpretationCategory.III, result.ExpectedCombinedSectionResultTemporal.ElementAt(9));
                AssertResultsIsAsExpected(2519.652041, 3010, EInterpretationCategory.I, result.ExpectedCombinedSectionResultTemporal.ElementAt(18));
                AssertResultsIsAsExpected(3010, 3313.767881, EInterpretationCategory.I, result.ExpectedCombinedSectionResultTemporal.ElementAt(19));

                Assert.AreEqual(7, result.ExpectedCombinedSectionResultPerFailureMechanism.Count());
                foreach (var failureMechanismSectionList in result.ExpectedCombinedSectionResultPerFailureMechanism)
                {
                    Assert.AreEqual(104, failureMechanismSectionList.Sections.Count());
                    FailureMechanismSection ninethSection = failureMechanismSectionList.Sections.ElementAt(13);
                    var mechanismId = failureMechanismSectionList.FailureMechanismId;
                    if (ninethSection is FailureMechanismSectionWithCategory)
                    {
                        var sectionWithDirectCategory = (FailureMechanismSectionWithCategory) ninethSection;
                        AssertResultsIsAsExpected(1440, 1545.093896, expectedDirectResults[mechanismId], sectionWithDirectCategory);
                    }
                }
            }
        }

        private void AssertResultsIsAsExpected(double start, double end, EInterpretationCategory category,
                                               FailureMechanismSectionWithCategory section)
        {
            Assert.AreEqual(start, section.SectionStart, 1e-8);
            Assert.AreEqual(end, section.SectionEnd, 1e-8);
            Assert.AreEqual(category, section.Category);
        }
    }
}