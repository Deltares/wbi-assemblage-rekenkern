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
using assembly.kernel.benchmark.tests.data.Input.FailureMechanismSections;
using Assembly.Kernel.Model.FmSectionTypes;
using DocumentFormat.OpenXml.Packaging;

namespace assembly.kernel.benchmark.tests.io.Readers.FailureMechanismSection
{
    /// <summary>
    /// Reader for failure mechanism sections in group 3 without simple assessment.
    /// </summary>
    public class Group3NoSimpleAssessmentFailureMechanismSectionReader : ExcelSheetReaderBase, ISectionReader<Group3NoSimpleAssessmentFailureMechanismSection>
    {
        /// <summary>
        /// Creates a new instance of <see cref="Group3NoSimpleAssessmentFailureMechanismSectionReader"/>.
        /// </summary>
        /// <param name="worksheetPart">The WorksheetPart that contains information on this failure mechanism</param>
        /// <param name="workbookPart">The workbook containing the specified worksheet</param>
        public Group3NoSimpleAssessmentFailureMechanismSectionReader(WorksheetPart worksheetPart, WorkbookPart workbookPart)
            : base(worksheetPart, workbookPart) {}

        public Group3NoSimpleAssessmentFailureMechanismSection ReadSection(int iRow, double startMeters, double endMeters)
        {
            var cellHValueAsString = GetCellValueAsString("H", iRow);

            return new Group3NoSimpleAssessmentFailureMechanismSection
            {
                SectionName = GetCellValueAsString("E", iRow),
                Start = startMeters,
                End = endMeters,
                SimpleAssessmentResult = GetCellValueAsString("F", iRow).ToEAssessmentResultTypeE2(),
                ExpectedSimpleAssessmentAssemblyResult =
                    new FmSectionAssemblyDirectResult(GetCellValueAsString("J", iRow).ToFailureMechanismSectionCategory()),
                DetailedAssessmentResult = GetCellValueAsString("G", iRow).ToEAssessmentResultTypeG2(false),
                DetailedAssessmentResultValue = GetCellValueAsString("G", iRow).ToFailureMechanismSectionCategory(),
                ExpectedDetailedAssessmentAssemblyResult =
                    new FmSectionAssemblyDirectResult(GetCellValueAsString("K", iRow).ToFailureMechanismSectionCategory()),
                TailorMadeAssessmentResult = cellHValueAsString.ToEAssessmentResultTypeT3(false),
                TailorMadeAssessmentResultCategory = RetrieveTailorMadeAssessmentResultCategory(cellHValueAsString),
                ExpectedTailorMadeAssessmentAssemblyResult =
                    new FmSectionAssemblyDirectResult(GetCellValueAsString("L", iRow).ToFailureMechanismSectionCategory()),
                ExpectedCombinedResult = GetCellValueAsString("M", iRow).ToFailureMechanismSectionCategory()
            };
        }

        private EFmSectionCategory RetrieveTailorMadeAssessmentResultCategory(string str)
        {
            try
            {
                return str.ToFailureMechanismSectionCategory();
            }
            catch (Exception)
            {
                return EFmSectionCategory.Gr;
            }
        }
    }
}