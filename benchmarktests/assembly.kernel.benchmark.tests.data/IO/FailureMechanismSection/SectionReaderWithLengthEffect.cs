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

using assembly.kernel.benchmark.tests.data.Data.Input.FailureMechanismSections;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.Categories;
using DocumentFormat.OpenXml.Packaging;

namespace assembly.kernel.benchmark.tests.data.IO.FailureMechanismSection
{
    /// <summary>
    /// Section reader for sections that include length effect.
    /// </summary>
    public class SectionReaderWithLengthEffect : ExcelSheetReaderBase, ISectionReader<ExpectedFailureMechanismSectionWithLengthEffect>
    {
        /// <summary>
        /// Constructor of the section reader.
        /// </summary>
        /// <param name="worksheetPart">Required <see cref="WorksheetPart"/>.</param>
        /// <param name="workbookPart">Required <see cref="WorkbookPart"/>.</param>
        public SectionReaderWithLengthEffect(WorksheetPart worksheetPart, WorkbookPart workbookPart)
            : base(worksheetPart, workbookPart, "B") {}

        /// <summary>
        /// Read the section on a specific row.
        /// </summary>
        /// <param name="iRow">Row index of the row in Excel that must be read.</param>
        /// <param name="startMeters">Already read start of the section in meters along the assessment section.</param>
        /// <param name="endMeters">Already read end of the section in meters along the assessment section.</param>
        /// <returns>The expected input and output for the specified section.</returns>
        public ExpectedFailureMechanismSectionWithLengthEffect ReadSection(int iRow, double startMeters, double endMeters)
        {
            string sectionName = GetCellValueAsString("B", iRow);
            bool isRelevant = GetCellValueAsString("E", iRow) == "Ja";
            var probabilityInitialMechanismProfile = new Probability(GetCellValueAsDouble("G", iRow));
            var probabilityInitialMechanismSection = new Probability(GetCellValueAsDouble("H", iRow));
            bool refinedAnalysisNecessary = GetCellValueAsString("I", iRow) == "Ja";
            var refinedProbabilityProfile = new Probability(GetCellValueAsDouble("J", iRow));
            var refinedProbabilitySection = new Probability(GetCellValueAsDouble("K", iRow));
            var expectedCombinedProbabilityProfile = new Probability(GetCellValueAsDouble("L", iRow));
            var expectedCombinedProbabilitySection = new Probability(GetCellValueAsDouble("M", iRow));
            EInterpretationCategory expectedInterpretationCategory = GetCellValueAsString("O", iRow).ToInterpretationCategory();

            ERefinementStatus eRefinementStatus;
            if (!refinedAnalysisNecessary)
            {
                eRefinementStatus = ERefinementStatus.NotNecessary;
            }
            else
            {
                eRefinementStatus = double.IsNaN(refinedProbabilityProfile)
                                        ? ERefinementStatus.Necessary
                                        : ERefinementStatus.Performed;
            }

            return new ExpectedFailureMechanismSectionWithLengthEffect(
                sectionName, startMeters, endMeters, isRelevant, probabilityInitialMechanismProfile,
                probabilityInitialMechanismSection, eRefinementStatus, refinedProbabilityProfile, refinedProbabilitySection,
                expectedCombinedProbabilityProfile, expectedCombinedProbabilitySection, expectedInterpretationCategory);
        }
    }
}