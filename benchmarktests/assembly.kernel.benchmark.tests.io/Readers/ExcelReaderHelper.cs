using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using static System.Double;
using static System.Int32;
using static System.String;

namespace assembly.kernel.benchmark.tests.io.Readers
{
    public static class ExcelReaderHelper
    {
        /// <summary>
        /// Creates a dictionary of keywords and row numbers based on column A.
        /// </summary>
        /// <param name="worksheetPart">The worksheet for which to create a dictionary</param>
        /// <param name="workbookPart">Thw workbookpart of the workbook that contains this worksheet</param>
        /// <param name="maxRow">The last row to include in the dictionary.</param>
        /// <returns></returns>
        public static Dictionary<string, int> ReadKeywordsDictionary(WorksheetPart worksheetPart, WorkbookPart workbookPart, int maxRow)
        {
            var dict = new Dictionary<string, int>();

            int iRow = 1;
            while (iRow <= maxRow)
            {
                var keyword = GetCellValueAsString(worksheetPart.Worksheet, "A" + iRow, workbookPart);
                if (!(IsNullOrWhiteSpace(keyword) || dict.ContainsKey(keyword)))
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
        /// <param name="worksheet"></param>
        /// <param name="cellReference"></param>
        /// <param name="workbookPart"></param>
        /// <returns></returns>
        public static double GetCellValueAsDouble(Worksheet worksheet, string cellReference, WorkbookPart workbookPart)
        {
            var cellValue = GetCellValueAsString(worksheet, cellReference, workbookPart);
            if (IsNullOrWhiteSpace(cellValue))
            {
                return NaN;
            }


            var culture = cellValue.Contains(",") ? CultureInfo.CurrentCulture : CultureInfo.InvariantCulture;
            double cellValueAsDouble;
            if (!TryParse(cellValue, NumberStyles.Any, culture, out cellValueAsDouble))
            {
                return NaN;
            }

            return cellValueAsDouble;
        }

        /// <summary>
        /// Reads cell contain and translates the result to an int
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="cellReference"></param>
        /// <param name="workbookPart"></param>
        /// <returns></returns>
        public static int GetCellValueAsInt(Worksheet worksheet, string cellReference, WorkbookPart workbookPart)
        {
            var cellValue = GetCellValueAsString(worksheet, cellReference, workbookPart);
            if (IsNullOrWhiteSpace(cellValue))
            {
                return default(int);
            }

            int cellValueAsInt;
            if (!TryParse(cellValue, NumberStyles.Any, CultureInfo.InvariantCulture, out cellValueAsInt))
            {
                return default(int);
            }

            return cellValueAsInt;
        }

        /// <summary>
        /// Reads cell contain and translates the result to a string
        /// </summary>
        /// <param name="worksheet"></param>
        /// <param name="cellReference"></param>
        /// <param name="workbookPart"></param>
        /// <returns></returns>
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
        /// Gets te number of columns in an Excel-sheet
        /// </summary>
        /// <param name="worksheetPart"></param>
        /// <returns></returns>
        public static int GetMaxColumn(WorksheetPart worksheetPart)
        {
            return worksheetPart.Worksheet.Descendants<Column>().Count();
        }

        /// <summary>
        /// Gets te number of rows in an Excel-sheet
        /// </summary>
        /// <param name="worksheetPart"></param>
        /// <returns></returns>
        public static int GetMaxRow(WorksheetPart worksheetPart)
        {
            return worksheetPart.Worksheet.Descendants<Row>().Count();
        }

        /// <summary>
        /// Gets the row number associated to a certain keyword (column A).
        /// </summary>
        /// <param name="worksheetPart"></param>
        /// <param name="key"></param>
        /// <param name="keywords"></param>
        /// <returns></returns>
        public static int GetRowId(string key, Dictionary<string, int> keywords)
        {
            if (keywords.ContainsKey(key))
            {
                return keywords[key];
            }

            return -1;
        }

        /// <summary>
        /// Creates a dictionary of all worksheetparts associated with the name of the tab in Excel.
        /// </summary>
        /// <param name="workbookPart"></param>
        /// <returns></returns>
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
            string cellValue = Empty;

            if (cell.DataType != null && cell.DataType == CellValues.SharedString && workbookPart != null)
            {
                int id;
                if (TryParse(cell.InnerText, out id))
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
