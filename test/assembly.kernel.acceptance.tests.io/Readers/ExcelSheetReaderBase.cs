using System.Collections.Generic;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace assembly.kernel.acceptance.tests.io.Readers
{
    public class ExcelSheetReaderBase
    {
        protected readonly Worksheet Worksheet;
        protected readonly int MaxRow;
        protected readonly int MaxColumn;
        protected readonly Dictionary<string, int> KeywordsDictionary;
        protected readonly WorkbookPart WorkbookPart;

        protected ExcelSheetReaderBase(WorksheetPart worksheetPart, WorkbookPart workbookPart)
        {
            this.WorkbookPart = workbookPart;
            this.Worksheet = worksheetPart.Worksheet;
            this.MaxRow = ExcelReaderHelper.GetMaxRow(worksheetPart);
            this.MaxColumn = ExcelReaderHelper.GetMaxColumn(worksheetPart);
            this.KeywordsDictionary = ExcelReaderHelper.ReadKeywordsDictionary(worksheetPart, workbookPart, MaxRow);
        }

        protected int GetRowId(string keyword)
        {
            return ExcelReaderHelper.GetRowId(keyword, KeywordsDictionary);
        }

        protected string GetCellValueAsString(string columnReference, string keyword)
        {
            return GetCellValueAsString(columnReference, GetRowId(keyword));
        }

        protected string GetCellValueAsString(string columnReference, int rowId)
        {
            return ExcelReaderHelper.GetCellValueAsString(Worksheet, columnReference + rowId, WorkbookPart);
        }

        protected double GetCellValueAsDouble(string columnReference, string keyword)
        {
            return GetCellValueAsDouble(columnReference, GetRowId(keyword));
        }

        protected double GetCellValueAsDouble(string columnReference, int rowId)
        {
            return ExcelReaderHelper.GetCellValueAsDouble(Worksheet, columnReference + rowId, WorkbookPart);
        }

        protected int GetCellValueAsInt(string columnReference, string keyword)
        {
            return GetCellValueAsInt(columnReference, GetRowId(keyword));
        }

        protected int GetCellValueAsInt(string columnReference, int rowId)
        {
            return ExcelReaderHelper.GetCellValueAsInt(Worksheet, columnReference + rowId, WorkbookPart);
        }
    }
}