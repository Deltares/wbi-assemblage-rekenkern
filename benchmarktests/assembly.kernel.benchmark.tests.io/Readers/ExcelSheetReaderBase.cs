using System.Collections.Generic;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace assembly.kernel.benchmark.tests.io.Readers
{
    public class ExcelSheetReaderBase
    {
        private readonly Worksheet worksheet;
        private readonly Dictionary<string, int> keywordsDictionary;
        private readonly WorkbookPart workbookPart;
        protected readonly int MaxRow;

        protected ExcelSheetReaderBase(WorksheetPart worksheetPart, WorkbookPart workbookPart)
        {
            this.workbookPart = workbookPart;
            worksheet = worksheetPart.Worksheet;
            MaxRow = ExcelReaderHelper.GetMaxRow(worksheetPart);
            keywordsDictionary = ExcelReaderHelper.ReadKeywordsDictionary(worksheetPart, workbookPart, MaxRow);
        }

        /// <summary>
        /// Gets the rownumber given a specific keyword (Column A)
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        protected int GetRowId(string keyword)
        {
            return ExcelReaderHelper.GetRowId(keyword, keywordsDictionary);
        }

        /// <summary>
        /// Gets the string value of a cell in the specified column at the row that is associated with the specified keyword (Column A)
        /// </summary>
        /// <param name="columnReference"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        protected string GetCellValueAsString(string columnReference, string keyword)
        {
            return GetCellValueAsString(columnReference, GetRowId(keyword));
        }

        /// <summary>
        /// Gets the string value of a cell in the specified column at the specified row
        /// </summary>
        /// <returns></returns>
        protected string GetCellValueAsString(string columnReference, int rowId)
        {
            return ExcelReaderHelper.GetCellValueAsString(worksheet, columnReference + rowId, workbookPart);
        }

        /// <summary>
        /// Gets the double value of a cell in the specified column at the row that is associated with the specified keyword (Column A)
        /// </summary>
        /// <param name="columnReference"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        protected double GetCellValueAsDouble(string columnReference, string keyword)
        {
            return GetCellValueAsDouble(columnReference, GetRowId(keyword));
        }

        /// <summary>
        /// Gets the double value of a cell in the specified column at the specified row
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="columnReference"></param>
        /// <returns></returns>
        protected double GetCellValueAsDouble(string columnReference, int rowId)
        {
            return ExcelReaderHelper.GetCellValueAsDouble(worksheet, columnReference + rowId, workbookPart);
        }
    }
}