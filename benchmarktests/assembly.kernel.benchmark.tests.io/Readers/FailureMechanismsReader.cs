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

using System.Collections.Generic;
using assembly.kernel.benchmark.tests.data.Input;
using assembly.kernel.benchmark.tests.data.Input.FailureMechanisms;
using assembly.kernel.benchmark.tests.data.Input.FailureMechanismSections;
using assembly.kernel.benchmark.tests.io.Readers.FailureMechanismSection;
using Assembly.Kernel.Model.CategoryLimits;
using Assembly.Kernel.Model.FmSectionTypes;
using DocumentFormat.OpenXml.Packaging;

namespace assembly.kernel.benchmark.tests.io.Readers
{
    /// <summary>
    /// Reader to read failure mechanism.
    /// </summary>
    public class FailureMechanismsReader : ExcelSheetReaderBase
    {
        private readonly SectionReaderFactory sectionReaderFactory;

        /// <summary>
        /// Creates a new instance of <see cref="FailureMechanismsReader"/>.
        /// </summary>
        /// <param name="worksheetPart">The worksheet for which to create a dictionary</param>
        /// <param name="workbookPart">Thw workbook part of the workbook that contains this worksheet</param>
        public FailureMechanismsReader(WorksheetPart worksheetPart, WorkbookPart workbookPart) : base(worksheetPart, workbookPart)
        {
            sectionReaderFactory = new SectionReaderFactory(worksheetPart, workbookPart);
        }

        /// <summary>
        /// Reads all relevant input and expected output for a specific failure mechanism.
        /// </summary>
        /// <param name="benchmarkTestInput">The test input.</param>
        public void Read(BenchmarkTestInput benchmarkTestInput)
        {
            IExpectedFailureMechanismResult expectedFailureMechanismResult =
                FailureMechanismResultFactory.CreateFailureMechanism(
                    GetCellValueAsString("B", "Toetsspoor").ToMechanismType());

            ReadGeneralInformation(expectedFailureMechanismResult);
            ReadGroup3FailureMechanismProperties(expectedFailureMechanismResult);
            ReadProbabilisticFailureMechanismProperties(expectedFailureMechanismResult);
            ReadSTBUFailureMechanismSpecificProperties(expectedFailureMechanismResult);
            ReadFailureMechanismSections(expectedFailureMechanismResult);

            benchmarkTestInput.ExpectedFailureMechanismsResults.Add(expectedFailureMechanismResult);
        }

        private void ReadGeneralInformation(IExpectedFailureMechanismResult expectedFailureMechanismResult)
        {
            expectedFailureMechanismResult.AccountForDuringAssembly = GetCellValueAsString("D", 1) == "Ja";
            var assessmentResultString = GetCellValueAsString("D", "Toetsoordeel per toetsspoor per traject");
            var temporalAssessmentResultString = GetCellValueAsString("D", "Tijdelijk Toetsoordeel per toetsspoor per traject");
            if (expectedFailureMechanismResult.Group > 4)
            {
                expectedFailureMechanismResult.ExpectedAssessmentResult = assessmentResultString.ToIndirectFailureMechanismSectionCategory();
                expectedFailureMechanismResult.ExpectedAssessmentResultTemporal = temporalAssessmentResultString.ToIndirectFailureMechanismSectionCategory();
            }
            else
            {
                expectedFailureMechanismResult.ExpectedAssessmentResult = assessmentResultString.ToFailureMechanismCategory();
                expectedFailureMechanismResult.ExpectedAssessmentResultTemporal = temporalAssessmentResultString.ToFailureMechanismCategory();
            }
        }

        private void ReadSTBUFailureMechanismSpecificProperties(IExpectedFailureMechanismResult expectedFailureMechanismResult)
        {
            var stbuFailureMechanism = expectedFailureMechanismResult as StbuExpectedFailureMechanismResult;
            if (stbuFailureMechanism != null)
            {
                stbuFailureMechanism.FailureMechanismProbabilitySpace = GetCellValueAsDouble("B", "ω Faalkansruimtefactor");
                stbuFailureMechanism.LengthEffectFactor = GetCellValueAsDouble("B", "Ndsn (lengte effectfactor)");
                stbuFailureMechanism.UseSignallingNorm = GetCellValueAsString("A", 14) == "Signaleringswaarde";
                stbuFailureMechanism.ExpectedSectionsCategoryDivisionProbability = GetCellValueAsDouble("B", "Peis;dsn ≤");
            }
        }

        private void ReadProbabilisticFailureMechanismProperties(IExpectedFailureMechanismResult expectedFailureMechanismResult)
        {
            var probabilisticFailureMechanism = expectedFailureMechanismResult as IProbabilisticExpectedFailureMechanismResult;
            if (probabilisticFailureMechanism != null)
            {
                probabilisticFailureMechanism.ExpectedAssessmentResultProbability =
                    GetCellValueAsDouble("F", "Toetsoordeel per toetsspoor per traject");
                probabilisticFailureMechanism.ExpectedAssessmentResultProbabilityTemporal =
                    GetCellValueAsDouble("F", "Tijdelijk Toetsoordeel per toetsspoor per traject");
                ReadFailureMechanismCategories(probabilisticFailureMechanism);
                ReadSectionCategories(probabilisticFailureMechanism);
            }
        }

        private void ReadGroup3FailureMechanismProperties(IExpectedFailureMechanismResult expectedFailureMechanismResult)
        {
            var group3FailureMechanism = expectedFailureMechanismResult as IGroup3ExpectedFailureMechanismResult;
            if (group3FailureMechanism != null)
            {
                group3FailureMechanism.FailureMechanismProbabilitySpace = GetCellValueAsDouble("B", "ω Faalkansruimtefactor");
                group3FailureMechanism.LengthEffectFactor = GetCellValueAsDouble("B", "Ndsn (lengte effectfactor)");
                if (group3FailureMechanism.Group == 3)
                {
                    ReadGroup3SectionCategoryBoundaries(group3FailureMechanism);
                }
            }
        }

        #region Read Sections

        private void ReadFailureMechanismSections(IExpectedFailureMechanismResult expectedFailureMechanismResult)
        {
            var sections = new List<IFailureMechanismSection>();
            var startRow = GetRowId("Vakindeling") + 3;
            var sectionReader = sectionReaderFactory.CreateReader(expectedFailureMechanismResult.Type);

            var iRow = startRow;
            while (iRow <= MaxRow)
            {
                var startMeters = GetCellValueAsDouble("A", iRow) * 1000.0;
                var endMeters = GetCellValueAsDouble("B", iRow) * 1000.0;

                if (double.IsNaN(startMeters) || double.IsNaN(endMeters))
                {
                    break;
                }

                sections.Add(sectionReader.ReadSection(iRow, startMeters, endMeters));

                iRow++;
            }

            expectedFailureMechanismResult.Sections = sections;
        }

        #endregion

        #region Read Categories

        private void ReadSectionCategories(IProbabilisticExpectedFailureMechanismResult expectedFailureMechanismResult)
        {
            var headerRowId = GetRowId("Categorie");

            var categories = new List<FmSectionCategory>();
            for (int i = headerRowId + 1; i < headerRowId + 7; i++)
            {
                var category = GetCellValueAsString("D", i).ToFailureMechanismSectionCategory();
                var lowerLimit = GetCellValueAsDouble("E", i);
                var upperLimit = GetCellValueAsDouble("F", i);
                categories.Add(new FmSectionCategory(category, lowerLimit, upperLimit));
            }

            expectedFailureMechanismResult.ExpectedFailureMechanismSectionCategories =
                new CategoriesList<FmSectionCategory>(categories);
        }

        private void ReadFailureMechanismCategories(IProbabilisticExpectedFailureMechanismResult expectedFailureMechanismResult)
        {
            var headerRowId = GetRowId("Categorie");

            var categories = new List<FailureMechanismCategory>();
            for (int i = headerRowId + 1; i < headerRowId + 7; i++)
            {
                var category = GetCellValueAsString("A", i).ToFailureMechanismCategory();
                var lowerLimit = GetCellValueAsDouble("B", i);
                var upperLimit = GetCellValueAsDouble("C", i);
                categories.Add(new FailureMechanismCategory(category, lowerLimit, upperLimit));
            }

            expectedFailureMechanismResult.ExpectedFailureMechanismCategories =
                new CategoriesList<FailureMechanismCategory>(categories);
        }

        private void ReadGroup3SectionCategoryBoundaries(IGroup3ExpectedFailureMechanismResult expectedFailureMechanismResult)
        {
            var headerRowId = GetRowId("Categorie");

            var categories = new List<FmSectionCategory>();
            var lastCategoryBoundary = 0.0;
            for (int i = headerRowId + 1; i < headerRowId + 6; i++)
            {
                var category = GetCellValueAsString("A", i).ToFailureMechanismSectionCategory();
                var currentBoundary = GetCellValueAsDouble("B", i);
                categories.Add(new FmSectionCategory(category, lastCategoryBoundary, currentBoundary));
                lastCategoryBoundary = currentBoundary;
            }

            categories.Add(new FmSectionCategory(EFmSectionCategory.VIv, lastCategoryBoundary, 1.0));

            expectedFailureMechanismResult.ExpectedFailureMechanismSectionCategories =
                new CategoriesList<FmSectionCategory>(categories);
        }

        #endregion
    }
}