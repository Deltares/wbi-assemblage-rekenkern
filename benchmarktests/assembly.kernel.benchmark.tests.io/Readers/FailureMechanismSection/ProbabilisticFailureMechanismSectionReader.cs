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
    public class ProbabilisticFailureMechanismSectionReader : ExcelSheetReaderBase, ISectionReader<ProbabilisticFailureMechanismSection>
    {
        private readonly bool lengthEffectPresent;

        /// <summary>
        /// Creates an instance of the ProbabilisticFailureMechanismSectionReader.
        /// </summary>
        /// <param name="worksheetPart">The WorksheetPart that contains information on this failure mechanism</param>
        /// <param name="workbookPart">The workbook containing the specified worksheet</param>
        public ProbabilisticFailureMechanismSectionReader(WorksheetPart worksheetPart, WorkbookPart workbookPart, bool lengthEffectPresent)
            : base(worksheetPart, workbookPart)
        {
            this.lengthEffectPresent = lengthEffectPresent;
        }

        public ProbabilisticFailureMechanismSection ReadSection(int iRow, double startMeters, double endMeters)
        {
            var cellFValueAsString = GetCellValueAsString("F", iRow);
            var lengthEffectFactor = lengthEffectPresent ? GetCellValueAsDouble("P", iRow) : 1.0;
            var detailedAssessmentResultProbability = GetCellValueAsDouble("G", iRow);
            var cellHValueAsString = GetCellValueAsString("H", iRow);
            var tailorMadeAssessmentResultProbability = cellHValueAsString.ToLower() == "fv" ? 0.0 : GetCellValueAsDouble("H", iRow);

            return new ProbabilisticFailureMechanismSection
            {
                SectionName = GetCellValueAsString("E", iRow),
                Start = startMeters,
                End = endMeters,
                LengthEffectFactor = lengthEffectFactor,
                SimpleAssessmentResult = cellFValueAsString.ToEAssessmentResultTypeE1(),
                ExpectedSimpleAssessmentAssemblyResult = new FmSectionAssemblyDirectResultWithProbability(
                    GetCellValueAsString("J", iRow).ToFailureMechanismSectionCategory(),
                    cellFValueAsString.ToLower() == "fv" || cellFValueAsString.ToLower() == "nvt"
                        ? 0.0
                        : double.NaN),
                DetailedAssessmentResult = GetCellValueAsString("G", iRow).ToEAssessmentResultTypeG2(true),
                DetailedAssessmentResultProbability = detailedAssessmentResultProbability,
                ExpectedDetailedAssessmentAssemblyResult = new FmSectionAssemblyDirectResultWithProbability(
                    GetCellValueAsString("K", iRow).ToFailureMechanismSectionCategory(),
                    Math.Min(1, detailedAssessmentResultProbability * lengthEffectFactor)),
                TailorMadeAssessmentResult = cellHValueAsString.ToEAssessmentResultTypeT3(true),
                TailorMadeAssessmentResultProbability = tailorMadeAssessmentResultProbability,
                ExpectedTailorMadeAssessmentAssemblyResult = new FmSectionAssemblyDirectResultWithProbability(
                    GetCellValueAsString("L", iRow).ToFailureMechanismSectionCategory(),
                    Math.Min(1, tailorMadeAssessmentResultProbability * lengthEffectFactor)),
                ExpectedCombinedResult = GetCellValueAsString("M", iRow).ToFailureMechanismSectionCategory(),
                ExpectedCombinedResultProbability = GetCellValueAsDouble("N", iRow)
            };
        }
    }
}