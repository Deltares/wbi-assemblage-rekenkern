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
using assembly.kernel.benchmark.tests.data.Input.FailureMechanismSections;
using DocumentFormat.OpenXml.Packaging;

namespace assembly.kernel.benchmark.tests.io.Readers.FailureMechanismSection
{
    /// <summary>
    /// Factory to create instances of <see cref="ISectionReader{TFailureMechanismSection}"/>.
    /// </summary>
    public class SectionReaderFactory
    {
        private readonly WorksheetPart worksheetPart;
        private readonly WorkbookPart workbookPart;

        /// <summary>
        /// Creates a new instance of <see cref="SectionReaderFactory"/>.
        /// </summary>
        /// <param name="worksheetPart">The worksheet for which to create a dictionary</param>
        /// <param name="workbookPart">Thw workbook part of the workbook that contains this worksheet</param>
        public SectionReaderFactory(WorksheetPart worksheetPart, WorkbookPart workbookPart)
        {
            this.worksheetPart = worksheetPart;
            this.workbookPart = workbookPart;
        }

        /// <summary>
        /// Creates a reader for the specified mechanism type.
        /// </summary>
        /// <param name="mechanismType">The <see cref="MechanismType"/> to get the reader for.</param>
        /// <returns>The created <see cref="ISectionReader{TFailureMechanismSection}"/>.</returns>
        public ISectionReader<IFailureMechanismSection> CreateReader(MechanismType mechanismType)
        {
            switch (mechanismType)
            {
                case MechanismType.STBI:
                case MechanismType.STPH:
                case MechanismType.HTKW:
                case MechanismType.BSKW:
                case MechanismType.STKWp:
                case MechanismType.GEKB:
                case MechanismType.AGK:
                case MechanismType.GEBU:
                case MechanismType.ZST:
                case MechanismType.DA:
                case MechanismType.GABI:
                case MechanismType.GABU:
                case MechanismType.STMI:
                case MechanismType.PKW:
                case MechanismType.AWO:
                case MechanismType.STKWl:
                case MechanismType.INN:
                case MechanismType.STBU:
                case MechanismType.HAV:
                case MechanismType.NWOkl:
                case MechanismType.VLZV:
                case MechanismType.VLAF:
                case MechanismType.NWOoc:
                case MechanismType.NWObe:
                case MechanismType.NWObo:
                case MechanismType.VLGA:
                default:
                    throw new InvalidEnumArgumentException();
            }
        }
    }
}