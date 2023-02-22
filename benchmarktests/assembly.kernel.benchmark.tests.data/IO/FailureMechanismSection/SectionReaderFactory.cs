﻿// Copyright (C) Rijkswaterstaat 2022. All rights reserved.
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
using DocumentFormat.OpenXml.Packaging;

namespace assembly.kernel.benchmark.tests.data.IO.FailureMechanismSection
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
        /// <param name="worksheetPart">The worksheet for which to create a dictionary.</param>
        /// <param name="workbookPart">The workbook part of the workbook that contains this worksheet.</param>
        public SectionReaderFactory(WorksheetPart worksheetPart, WorkbookPart workbookPart)
        {
            this.worksheetPart = worksheetPart;
            this.workbookPart = workbookPart;
        }

        /// <summary>
        /// Creates a reader for the failure mechanism based on the length effect within a section being present.
        /// </summary>
        /// <param name="hasLengthEffectWithinSection">Indicates whether the sections that need to be read have length effect.</param>
        /// <returns>The created <see cref="ISectionReader{TFailureMechanismSection}"/>.</returns>
        public ISectionReader<IExpectedFailureMechanismSection> CreateReader(bool hasLengthEffectWithinSection)
        {
            return hasLengthEffectWithinSection
                       ? (ISectionReader<IExpectedFailureMechanismSection>) new SectionReaderWithLengthEffect(worksheetPart, workbookPart)
                       : new SectionReaderWithoutLengthEffect(worksheetPart, workbookPart);
        }
    }
}