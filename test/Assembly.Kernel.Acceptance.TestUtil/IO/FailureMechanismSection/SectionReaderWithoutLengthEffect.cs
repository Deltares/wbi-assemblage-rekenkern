﻿// Copyright (C) Stichting Deltares and State of the Netherlands 2023. All rights reserved.
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

using Assembly.Kernel.Acceptance.TestUtil.Data.Input.FailureMechanismSections;
using Assembly.Kernel.Model;
using Assembly.Kernel.Model.Categories;
using DocumentFormat.OpenXml.Packaging;

namespace Assembly.Kernel.Acceptance.TestUtil.IO.FailureMechanismSection
{
    /// <summary>
    /// Section reader for sections that include length effect.
    /// </summary>
    public class SectionReaderWithoutLengthEffect : ExcelSheetReaderBase, ISectionReader<ExpectedFailureMechanismSection>
    {
        /// <summary>
        /// Constructor of the section reader.
        /// </summary>
        /// <param name="worksheetPart">Required <see cref="WorksheetPart"/>.</param>
        /// <param name="workbookPart">Required <see cref="WorkbookPart"/>.</param>
        public SectionReaderWithoutLengthEffect(WorksheetPart worksheetPart, WorkbookPart workbookPart)
            : base(worksheetPart, workbookPart, "B") {}

        /// <summary>
        /// Read the section on a specific row.
        /// </summary>
        /// <param name="iRow">Row index of the row in Excel that must be read.</param>
        /// <param name="startMeters">Already read start of the section in meters along the assessment section.</param>
        /// <param name="endMeters">Already read end of the section in meters along the assessment section.</param>
        /// <returns>The expected input and output for the specified section.</returns>
        public ExpectedFailureMechanismSection ReadSection(int iRow, double startMeters, double endMeters)
        {
            string sectionName = GetCellValueAsString("B", iRow);
            bool isRelevant = GetCellValueAsString("E", iRow) == "Ja";
            var probabilityInitialMechanismSection = new Probability(GetCellValueAsDouble("G", iRow));
            bool refinedAnalysisNecessary = GetCellValueAsString("H", iRow) == "Ja";
            var refinedProbabilitySection = new Probability(GetCellValueAsDouble("I", iRow));
            var expectedCombinedProbabilitySection = new Probability(GetCellValueAsDouble("J", iRow));
            EInterpretationCategory expectedInterpretationCategory = GetCellValueAsString("K", iRow).ToInterpretationCategory();

            ERefinementStatus eRefinementStatus;
            if (!refinedAnalysisNecessary)
            {
                eRefinementStatus = ERefinementStatus.NotNecessary;
            }
            else
            {
                eRefinementStatus = double.IsNaN(refinedProbabilitySection)
                                        ? ERefinementStatus.Necessary
                                        : ERefinementStatus.Performed;
            }

            return new ExpectedFailureMechanismSection(
                sectionName, startMeters, endMeters, isRelevant, probabilityInitialMechanismSection,
                eRefinementStatus, refinedProbabilitySection, expectedCombinedProbabilitySection,
                expectedInterpretationCategory);
        }
    }
}