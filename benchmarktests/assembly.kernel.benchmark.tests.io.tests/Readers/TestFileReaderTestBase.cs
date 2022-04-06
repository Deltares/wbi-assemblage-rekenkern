#region Copyright (C) Rijkswaterstaat 2022. All rights reserved.

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

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assembly.Kernel.Model.Categories;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using MathNet.Numerics.Distributions;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace assembly.kernel.benchmark.tests.io.tests.Readers
{
    public class TestFileReaderTestBase
    {
        protected string GetTestDir()
        {
            return Path.Combine(GetSolutionRoot(), "benchmarktests", "assembly.kernel.benchmark.tests.io.tests", "test-data");
        }

        protected static Dictionary<string, WorksheetPart> ReadWorkSheetParts(WorkbookPart workbookPart)
        {
            var workSheetParts = new Dictionary<string, WorksheetPart>();

            foreach (var worksheetPart in workbookPart.WorksheetParts)
            {
                var sheet = GetSheetFromWorkSheet(workbookPart, worksheetPart);
                workSheetParts[sheet.Name] = worksheetPart;
            }

            return workSheetParts;
        }

        protected void AssertAreEqualCategories<TCategory>(TCategory expectedCategory, double expectedLowerLimit,
                                                           double expectedUpperLimit,
                                                           CategoryBase<TCategory> assessmentSectionCategory)
        {
            Assert.AreEqual(expectedCategory, assessmentSectionCategory.Category);
            AssertAreEqualProbabilities(expectedLowerLimit, assessmentSectionCategory.LowerLimit);
            AssertAreEqualProbabilities(expectedUpperLimit, assessmentSectionCategory.UpperLimit);
        }

        protected void AssertAreEqualProbabilities(double expectedProbability, double actualProbability)
        {
            Assert.AreEqual(ProbabilityToReliability(expectedProbability), ProbabilityToReliability(actualProbability), 1e-3);
        }

        private static string GetSolutionRoot()
        {
            const string solutionName = "Assembly.sln";
            var testContext = new TestContext(new TestExecutionContext.AdhocContext());
            string curDir = testContext.TestDirectory;
            while (Directory.Exists(curDir) && !File.Exists(curDir + @"\" + solutionName))
            {
                curDir += "/../";
            }

            if (!File.Exists(Path.Combine(curDir, solutionName)))
            {
                throw new InvalidOperationException(
                    $"Solution file '{solutionName}' not found in any folder of '{Directory.GetCurrentDirectory()}'.");
            }

            return Path.GetFullPath(curDir);
        }

        private static Sheet GetSheetFromWorkSheet
            (WorkbookPart workbookPart, WorksheetPart worksheetPart)
        {
            string relationshipId = workbookPart.GetIdOfPart(worksheetPart);
            IEnumerable<Sheet> sheets = workbookPart.Workbook.Sheets.Elements<Sheet>();
            return sheets.FirstOrDefault(s => s.Id.HasValue && s.Id.Value == relationshipId);
        }

        /// <summary>
        /// Calculates the reliability from a probability.
        /// </summary>
        /// <param name="probability">The probability to convert.</param>
        /// <returns>The reliability.</returns>
        private static double ProbabilityToReliability(double probability)
        {
            return Normal.InvCDF(0, 1, 1 - probability);
        }
    }
}