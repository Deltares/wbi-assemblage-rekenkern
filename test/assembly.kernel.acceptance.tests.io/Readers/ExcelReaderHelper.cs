using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace assembly.kernel.acceptance.tests.io.Readers
{
    // TODO: Convert this to IExcelWorksheetReader and stop passing actual Workbook and Worksheet parts etc.
    public static class ExcelReaderHelper
    {
        public static Dictionary<string, int> ReadKeywordsDictionary(WorksheetPart worksheetPart, WorkbookPart workbookPart, int maxRow)
        {
            var dict = new Dictionary<string, int>();

            int iRow = 1;
            while (iRow <= maxRow)
            {
                var keyword = GetCellValueAsString(worksheetPart.Worksheet, "A" + iRow, workbookPart);
                if (!(String.IsNullOrWhiteSpace(keyword) || dict.ContainsKey(keyword)))
                {
                    dict[keyword] = iRow;
                }
                iRow++;
            }

            return dict;
        }

        public static double GetCellValueAsDouble(Worksheet worksheet, string cellReference, WorkbookPart workbookPart)
        {
            var cellValue = GetCellValueAsString(worksheet, cellReference, workbookPart);
            if (String.IsNullOrWhiteSpace(cellValue))
            {
                return Double.NaN;
            }


            var culture = cellValue.Contains(",") ? CultureInfo.CurrentCulture : CultureInfo.InvariantCulture;
            double cellValueAsDouble;
            if (!Double.TryParse(cellValue, NumberStyles.Any, culture, out cellValueAsDouble))
            {
                return Double.NaN;
            }

            return cellValueAsDouble;
        }

        public static int GetCellValueAsInt(Worksheet worksheet, string cellReference, WorkbookPart workbookPart)
        {
            var cellValue = GetCellValueAsString(worksheet, cellReference, workbookPart);
            if (String.IsNullOrWhiteSpace(cellValue))
            {
                return default(int);
            }

            int cellValueAsInt;
            if (!Int32.TryParse(cellValue, NumberStyles.Any, CultureInfo.InvariantCulture, out cellValueAsInt))
            {
                return default(int);
            }

            return cellValueAsInt;
        }

        public static string GetCellValueAsString(Worksheet worksheet, string cellReference, WorkbookPart workbookPart)
        {
            var cell = GetCell(worksheet, cellReference);
            if (cell == null)
            {
                return "";
            }

            return CellValueAsStringFromCell(cell, workbookPart);
        }

        public static Cell GetCell(Worksheet worksheet, string addressName)
        {
            return worksheet.Descendants<Cell>().FirstOrDefault(c => c.CellReference == addressName);
        }

        public static string CellValueAsStringFromCell(Cell cell, WorkbookPart workbookPart)
        {
            string cellValue = String.Empty;

            if (cell.DataType != null && cell.DataType == CellValues.SharedString && workbookPart != null)
            {
                int id;
                if (Int32.TryParse(cell.InnerText, out id))
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

        public static int GetMaxColumn(WorksheetPart worksheetPart)
        {
            return worksheetPart.Worksheet.Descendants<Column>().Count();
        }

        public static int GetMaxRow(WorksheetPart worksheetPart)
        {
            return worksheetPart.Worksheet.Descendants<Row>().Count();
        }

        public static int GetRowId(string key, Dictionary<string, int> keywords)
        {
            if (keywords.ContainsKey(key))
            {
                return keywords[key];
            }

            return -1;
        }

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
