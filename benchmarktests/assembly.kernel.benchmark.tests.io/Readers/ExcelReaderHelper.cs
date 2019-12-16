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

using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace assembly.kernel.benchmark.tests.io.Readers
{
    /// <summary>
    /// Helper class for reading excel data.
    /// </summary>
    public static class ExcelReaderHelper
    {
        /// <summary>
        /// Creates a dictionary of keywords and row numbers based on column A.
        /// </summary>
        /// <param name="worksheetPart">The worksheet for which to create a dictionary</param>
        /// <param name="workbookPart">Thw workbook part of the workbook that contains this worksheet</param>
        /// <param name="maxRow">The last row to include in the dictionary.</param>
        /// <returns>The created dictionary.</returns>
        public static Dictionary<string, int> ReadKeywordsDictionary(WorksheetPart worksheetPart, WorkbookPart workbookPart,
                                                                     int maxRow)
        {
            var dict = new Dictionary<string, int>();

            int iRow = 1;
            while (iRow <= maxRow)
            {
                var keyword = GetCellValueAsString(worksheetPart.Worksheet, "A" + iRow, workbookPart);
                if (!(string.IsNullOrWhiteSpace(keyword) || dict.ContainsKey(keyword)))
                {
                    dict[keyword] = iRow;
                }

                iRow++;
            }

            return dict;
        }

        /// <summary>
        /// Reads cell contain and translates the result to a double
        /// </summary>
        /// <param name="worksheet">The worksheet.</param>
        /// <param name="cellReference">the cell reference.</param>
        /// <param name="workbookPart">the workbook part.</param>
        /// <returns>The cell value as <see cref="double"/>.</returns>
        public static double GetCellValueAsDouble(Worksheet worksheet, string cellReference, WorkbookPart workbookPart)
        {
            var cellValue = GetCellValueAsString(worksheet, cellReference, workbookPart);
            if (string.IsNullOrWhiteSpace(cellValue))
            {
                return double.NaN;
            }

            var culture = cellValue.Contains(",") ? CultureInfo.CurrentCulture : CultureInfo.InvariantCulture;
            double cellValueAsDouble;
            if (!double.TryParse(cellValue, NumberStyles.Any, culture, out cellValueAsDouble))
            {
                return double.NaN;
            }

            return cellValueAsDouble;
        }

        /// <summary>
        /// Reads cell contain and translates the result to a string
        /// </summary>
        /// <param name="worksheet">The worksheet.</param>
        /// <param name="cellReference">the cell reference.</param>
        /// <param name="workbookPart">the workbook part.</param>
        /// <returns>The cell value as <see cref="string"/>.</returns>
        public static string GetCellValueAsString(Worksheet worksheet, string cellReference, WorkbookPart workbookPart)
        {
            var cell = GetCell(worksheet, cellReference);
            if (cell == null)
            {
                return "";
            }

            return CellValueAsStringFromCell(cell, workbookPart);
        }

        /// <summary>
        /// Gets te number of rows in an Excel-sheet
        /// </summary>
        /// <param name="worksheetPart">The worksheet part.</param>
        /// <returns>The row count.</returns>
        public static int GetMaxRow(WorksheetPart worksheetPart)
        {
            return worksheetPart.Worksheet.Descendants<Row>().Count();
        }

        /// <summary>
        /// Gets the row number associated to a certain keyword (column A).
        /// </summary>
        /// <param name="key">The key to get the row for.</param>
        /// <param name="keywords">All the keywords.</param>
        /// <returns>The row number that belongs to the key;
        /// or -1 when the key does not exist.</returns>
        public static int GetRowId(string key, Dictionary<string, int> keywords)
        {
            if (keywords.ContainsKey(key))
            {
                return keywords[key];
            }

            return -1;
        }

        /// <summary>
        /// Creates a dictionary of all worksheet parts associated with the name of the tab in Excel.
        /// </summary>
        /// <param name="workbookPart">The workbook part.</param>
        /// <returns>A dictionary with all worksheet parts
        /// associated with the name of the tab in Excel</returns>
        public static Dictionary<string, WorksheetPart> ReadWorkSheetParts(WorkbookPart workbookPart)
        {
            var workSheetParts = new Dictionary<string, WorksheetPart>();

            foreach (var worksheetPart in workbookPart.WorksheetParts)
            {
                var sheet = GetSheetFromWorkSheet(workbookPart, worksheetPart);
                workSheetParts[sheet.Name] = worksheetPart;
            }

            return workSheetParts;
        }

        private static Cell GetCell(Worksheet worksheet, string addressName)
        {
            return worksheet.Descendants<Cell>().FirstOrDefault(c => c.CellReference == addressName);
        }

        private static string CellValueAsStringFromCell(Cell cell, WorkbookPart workbookPart)
        {
            string cellValue = string.Empty;

            if (cell.DataType != null && cell.DataType == CellValues.SharedString && workbookPart != null)
            {
                int id;
                if (int.TryParse(cell.InnerText, out id))
                {
                    SharedStringItem item = GetSharedStringItemById(workbookPart, id);

                    if (item.Text != null)
                    {
                        cellValue = item.Text.Text;
                    }
                    else if (item.InnerText != null)
                    {
                        cellValue = item.InnerText;
                    }
                    else if (item.InnerXml != null)
                    {
                        cellValue = item.InnerXml;
                    }
                }
            }
            else if (cell.CellValue == null)
            {
                cellValue = cell.InnerText;
            }
            else
            {
                cellValue = cell.CellValue.InnerText;
            }

            return cellValue;
        }

        private static Sheet GetSheetFromWorkSheet
            (WorkbookPart workbookPart, WorksheetPart worksheetPart)
        {
            string relationshipId = workbookPart.GetIdOfPart(worksheetPart);
            IEnumerable<Sheet> sheets = workbookPart.Workbook.Sheets.Elements<Sheet>();
            return sheets.FirstOrDefault(s => s.Id.HasValue && s.Id.Value == relationshipId);
        }

        private static SharedStringItem GetSharedStringItemById(WorkbookPart workbookPart, int id)
        {
            return workbookPart.SharedStringTablePart.SharedStringTable.Elements<SharedStringItem>().ElementAt(id);
        }
    }
}