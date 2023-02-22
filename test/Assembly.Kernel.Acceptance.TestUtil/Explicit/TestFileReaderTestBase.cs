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

using System.Collections.Generic;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Assembly.Kernel.Acceptance.TestUtil.Explicit
{
    /// <summary>
    /// Base class for al tests that need to read files.
    /// </summary>
    public class TestFileReaderTestBase
    {
        /// <summary>
        /// Read all <see cref="WorksheetPart"/> from a <see cref="WorkbookPart"/>.
        /// </summary>
        /// <param name="workbookPart">The <see cref="WorkbookPart"/> that needs to be read.</param>
        /// <returns>A dictionary of worksheet parts (values) stored by their name (key).</returns>
        protected static Dictionary<string, WorksheetPart> ReadWorkSheetParts(WorkbookPart workbookPart)
        {
            var workSheetParts = new Dictionary<string, WorksheetPart>();

            foreach (WorksheetPart worksheetPart in workbookPart.WorksheetParts)
            {
                Sheet sheet = GetSheetFromWorkSheet(workbookPart, worksheetPart);
                workSheetParts[sheet.Name] = worksheetPart;
            }

            return workSheetParts;
        }

        private static Sheet GetSheetFromWorkSheet(WorkbookPart workbookPart, OpenXmlPart worksheetPart)
        {
            string relationshipId = workbookPart.GetIdOfPart(worksheetPart);
            IEnumerable<Sheet> sheets = workbookPart.Workbook.Sheets.Elements<Sheet>();
            return sheets.FirstOrDefault(s => s.Id.HasValue && s.Id.Value == relationshipId);
        }
    }
}