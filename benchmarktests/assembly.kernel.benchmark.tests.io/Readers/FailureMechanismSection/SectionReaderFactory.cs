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

using System.ComponentModel;
using assembly.kernel.benchmark.tests.data.Input.FailureMechanisms;
using DocumentFormat.OpenXml.Packaging;

namespace assembly.kernel.benchmark.tests.io.Readers.FailureMechanismSection
{
    public class SectionReaderFactory
    {
        private readonly WorksheetPart worksheetPart;
        private readonly WorkbookPart workbookPart;

        /// <summary>
        /// Creates an instance of the SectionReaderFactory for a specific worksheet.
        /// </summary>
        /// <param name="worksheetPart"></param>
        /// <param name="workbookPart"></param>
        public SectionReaderFactory(WorksheetPart worksheetPart, WorkbookPart workbookPart)
        {
            this.worksheetPart = worksheetPart;
            this.workbookPart = workbookPart;
        }

        /// <summary>
        /// Creates a reader for the specified mechanism type.
        /// </summary>
        /// <param name="mechanismType"></param>
        /// <returns></returns>
        public ISectionReader CreateReader(MechanismType mechanismType)
        {
            switch (mechanismType)
            {
                case MechanismType.STBI:
                case MechanismType.STPH:
                case MechanismType.HTKW:
                case MechanismType.BSKW:
                    return new ProbabilisticFailureMechanismSectionReader(worksheetPart, workbookPart, mechanismType == MechanismType.STBI || mechanismType == MechanismType.STPH);
                case MechanismType.STKWp:
                case MechanismType.GEKB:
                    return new Group1NoSimpleAssessmentFailureMechanismSectionReader(worksheetPart, workbookPart);
                case MechanismType.AGK:
                case MechanismType.GEBU:
                    return new Group3FailureMechanismSectionReader(worksheetPart, workbookPart);
                case MechanismType.ZST:
                case MechanismType.DA:
                    return new Group3NoSimpleAssessmentFailureMechanismSectionReader(worksheetPart, workbookPart);
                case MechanismType.GABI:
                case MechanismType.GABU:
                case MechanismType.STMI:
                case MechanismType.PKW:
                    return new Group4FailureMechanismSectionReader(worksheetPart, workbookPart);
                case MechanismType.AWO:
                case MechanismType.STKWl:
                case MechanismType.INN:
                    return new Group4NoDetailedAssessmentFailureMechanismSectionReader(worksheetPart, workbookPart);
                case MechanismType.STBU:
                    return new STBUFailureMechanismSectionReader(worksheetPart, workbookPart);
                case MechanismType.HAV:
                case MechanismType.NWOkl:
                case MechanismType.VLZV:
                case MechanismType.VLAF:
                    return new Group5FailureMechanismSectionReader(worksheetPart, workbookPart);
                case MechanismType.NWOoc:
                    return new NWOocFailureMechanismSectionReader(worksheetPart, workbookPart);
                case MechanismType.NWObe:
                case MechanismType.NWObo:
                case MechanismType.VLGA:
                    return new Group5NoDetailedAssessmentFailureMechanismSectionReader(worksheetPart, workbookPart);
                default:
                    throw new InvalidEnumArgumentException();
            }
        }
    }
}