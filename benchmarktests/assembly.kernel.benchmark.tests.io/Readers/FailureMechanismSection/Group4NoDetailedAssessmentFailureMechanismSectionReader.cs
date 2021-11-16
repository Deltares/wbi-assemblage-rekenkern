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

using assembly.kernel.benchmark.tests.data.Input.FailureMechanismSections;
using Assembly.Kernel.Model.FmSectionTypes;
using DocumentFormat.OpenXml.Packaging;

namespace assembly.kernel.benchmark.tests.io.Readers.FailureMechanismSection
{
    /// <summary>
    /// Reader for failure mechanism sections in group 4 without detailed assessment.
    /// </summary>
    public class Group4NoDetailedAssessmentFailureMechanismSectionReader : ExcelSheetReaderBase, ISectionReader<Group4NoDetailedAssessmentFailureMechanismSection>
    {
        /// <summary>
        /// Creates a new instance of <see cref="Group4NoDetailedAssessmentFailureMechanismSectionReader"/>.
        /// </summary>
        /// <param name="worksheetPart">The WorksheetPart that contains information on this failure mechanism</param>
        /// <param name="workbookPart">The workbook containing the specified worksheet</param>
        public Group4NoDetailedAssessmentFailureMechanismSectionReader(WorksheetPart worksheetPart, WorkbookPart workbookPart)
            : base(worksheetPart, workbookPart) {}

        public Group4NoDetailedAssessmentFailureMechanismSection ReadSection(int iRow, double startMeters, double endMeters)
        {
            return new Group4NoDetailedAssessmentFailureMechanismSection
            {
                SectionName = GetCellValueAsString("E", iRow),
                Start = startMeters,
                End = endMeters,
                ExpectedDetailedAssessmentAssemblyResult =
                    new FmSectionAssemblyDirectResult(GetCellValueAsString("K", iRow)
                                                          .ToFailureMechanismSectionCategory()),
                TailorMadeAssessmentResult = GetCellValueAsString("H", iRow).ToEAssessmentResultTypeT1(),
                ExpectedTailorMadeAssessmentAssemblyResult =
                    new FmSectionAssemblyDirectResult(GetCellValueAsString("L", iRow)
                                                          .ToFailureMechanismSectionCategory()),
                ExpectedCombinedResult = GetCellValueAsString("M", iRow).ToFailureMechanismSectionCategory()
            };
        }
    }
}