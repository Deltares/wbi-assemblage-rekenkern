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
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace assembly.kernel.benchmark.tests.io.Readers
{
    /// <summary>
    /// Base class to read excel sheets.
    /// </summary>
    public class ExcelSheetReaderBase
    {
        protected readonly int MaxRow;
        private readonly Dictionary<string, int> keywordsDictionary;
        private readonly WorkbookPart workbookPart;
        private readonly Worksheet worksheet;

        /// <summary>
        /// Creates a new instance of <see cref="ExcelSheetReaderBase"/>.
        /// </summary>
        /// <param name="worksheetPart">The worksheet for which to create a dictionary.</param>
        /// <param name="workbookPart">The workbook part of the workbook that contains this worksheet.</param>
        /// <param name="column">"String indicating the column that contains keywords.</param>
        protected ExcelSheetReaderBase(WorksheetPart worksheetPart, WorkbookPart workbookPart, string column)
        {
            this.workbookPart = workbookPart;
            worksheet = worksheetPart.Worksheet;
            MaxRow = ExcelReaderHelper.GetMaxRow(worksheetPart);
            keywordsDictionary = ExcelReaderHelper.ReadKeywordsDictionary(worksheetPart, workbookPart, column, MaxRow);
        }

        /// <summary>
        /// Gets the row number given a specific keyword.
        /// </summary>
        /// <param name="keyword">The keyword to get te row number for.</param>
        /// <returns>The row number that belongs to the key; or -1 when
        /// the key does not exist.</returns>
        protected int GetRowId(string keyword)
        {
            return ExcelReaderHelper.GetRowId(keyword, keywordsDictionary);
        }

        /// <summary>
        /// Gets the string value of a cell in the specified column at the row that is associated with the specified keyword.
        /// </summary>
        /// <param name="columnReference">The column reference.</param>
        /// <param name="keyword">The keyword to get the cell value from.</param>
        /// <returns>The cell value as <see cref="string"/>.</returns>
        protected string GetCellValueAsString(string columnReference, string keyword)
        {
            return GetCellValueAsString(columnReference, GetRowId(keyword));
        }

        /// <summary>
        /// Gets the string value of a cell in the specified column at the specified row.
        /// </summary>
        /// <param name="columnReference">The column reference.</param>
        /// <param name="rowId">The row id to get the cell value from.</param>
        /// <returns>The cell value as <see cref="string"/>.</returns>
        protected string GetCellValueAsString(string columnReference, int rowId)
        {
            return ExcelReaderHelper.GetCellValueAsString(worksheet, columnReference + rowId, workbookPart);
        }

        /// <summary>
        /// Gets the double value of a cell in the specified column at the row that is associated with the specified keyword.
        /// </summary>
        /// <param name="columnReference">The column reference.</param>
        /// <param name="keyword">The keyword to get the cell value from.</param>
        /// <returns>The cell value as <see cref="double"/>.</returns>
        protected double GetCellValueAsDouble(string columnReference, string keyword)
        {
            return GetCellValueAsDouble(columnReference, GetRowId(keyword));
        }

        /// <summary>
        /// Gets the double value of a cell in the specified column at the specified row.
        /// </summary>
        /// <param name="columnReference">The column reference.</param>
        /// <param name="rowId">The row id to get the cell value from.</param>
        /// <returns>The cell value as <see cref="double"/>.</returns>
        protected double GetCellValueAsDouble(string columnReference, int rowId)
        {
            return ExcelReaderHelper.GetCellValueAsDouble(worksheet, columnReference + rowId, workbookPart);
        }
    }
}