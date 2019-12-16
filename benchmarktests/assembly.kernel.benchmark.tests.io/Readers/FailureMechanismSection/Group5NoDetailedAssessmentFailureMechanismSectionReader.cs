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
    /// Reader for failure mechanism sections in group 5 without detailed assessment.
    /// </summary>
    public class Group5NoDetailedAssessmentFailureMechanismSectionReader : ExcelSheetReaderBase, ISectionReader<Group5NoDetailedAssessmentFailureMechanismSection>
    {
        /// <summary>
        /// Creates a new instance of <see cref="Group5NoDetailedAssessmentFailureMechanismSectionReader"/>.
        /// </summary>
        /// <param name="worksheetPart">The WorksheetPart that contains information on this failure mechanism</param>
        /// <param name="workbookPart">The workbook containing the specified worksheet</param>
        public Group5NoDetailedAssessmentFailureMechanismSectionReader(WorksheetPart worksheetPart, WorkbookPart workbookPart)
            : base(worksheetPart, workbookPart) {}

        public Group5NoDetailedAssessmentFailureMechanismSection ReadSection(int iRow, double startMeters, double endMeters)
        {
            return new Group5NoDetailedAssessmentFailureMechanismSection
            {
                SectionName = GetCellValueAsString("E", iRow),
                Start = startMeters,
                End = endMeters,
                SimpleAssessmentResult = GetCellValueAsString("F", iRow).ToEAssessmentResultTypeE1(),
                ExpectedSimpleAssessmentAssemblyResult =
                    new FmSectionAssemblyIndirectResult(GetCellValueAsString("J", iRow)
                                                            .ToIndirectFailureMechanismSectionCategory()),
                ExpectedDetailedAssessmentAssemblyResult =
                    new FmSectionAssemblyIndirectResult(GetCellValueAsString("K", iRow)
                                                            .ToIndirectFailureMechanismSectionCategory()),
                TailorMadeAssessmentResult = GetCellValueAsString("H", iRow).ToEAssessmentResultTypeT2(),
                ExpectedTailorMadeAssessmentAssemblyResult =
                    new FmSectionAssemblyIndirectResult(GetCellValueAsString("L", iRow)
                                                            .ToIndirectFailureMechanismSectionCategory()),
                ExpectedCombinedResult = GetCellValueAsString("M", iRow).ToIndirectFailureMechanismSectionCategory()
            };
        }
    }
}