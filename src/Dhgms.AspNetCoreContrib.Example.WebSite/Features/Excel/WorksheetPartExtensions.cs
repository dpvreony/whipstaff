using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Dhgms.AspNetCoreContrib.Example.WebSite.Features.Excel
{
    public static class WorksheetPartExtensions
    {
        public static Cell InsertCellInWorksheet(
            this WorksheetPart worksheetPart,
            string columnName,
            uint rowIndex,
            string value)
        {
            var cell = worksheetPart.InsertCellInWorksheet(columnName, rowIndex);
            cell.CellValue = new CellValue(value);
            cell.DataType = new EnumValue<CellValues>(CellValues.String);

            return cell;
        }

        public static Cell InsertCellInWorksheet(
            this WorksheetPart worksheetPart,
            string columnName,
            uint rowIndex,
            bool value)
        {
            var cell = worksheetPart.InsertCellInWorksheet(columnName, rowIndex);
            cell.CellValue = new CellValue(value ? "1" : "0");
            cell.DataType = new EnumValue<CellValues>(CellValues.Boolean);

            return cell;
        }

        public static Cell InsertCellInWorksheet(
            this WorksheetPart worksheetPart,
            string columnName,
            uint rowIndex,
            int value)
        {
            var cell = worksheetPart.InsertCellInWorksheet(columnName, rowIndex);
            cell.CellValue = new CellValue(value.ToString(NumberFormatInfo.InvariantInfo));
            cell.DataType = new EnumValue<CellValues>(CellValues.Number);

            return cell;
        }

        public static Cell InsertCellInWorksheet(
            this WorksheetPart worksheetPart,
            string columnName,
            uint rowIndex,
            DateTimeOffset value)
        {
            var cell = worksheetPart.InsertCellInWorksheet(columnName, rowIndex);
            cell.CellValue = new CellValue(value.ToString(NumberFormatInfo.InvariantInfo));
            cell.DataType = new EnumValue<CellValues>(CellValues.Date);

            return cell;
        }

        // Given a column name, a row index, and a WorksheetPart, inserts a cell into the worksheet. 
        // If the cell already exists, returns it. 
        public static Cell InsertCellInWorksheet(
            this WorksheetPart worksheetPart,
            string columnName,
            uint rowIndex)
        {
            var worksheet = worksheetPart.Worksheet;
            var sheetData = worksheet.GetFirstChild<SheetData>();
            var cellReference = columnName + rowIndex;

            // If the worksheet does not contain a row with the specified row index, insert one.
            var row = sheetData.Elements<Row>().FirstOrDefault(r => r.RowIndex == rowIndex);
            if (row != null)
            {
                var cell = row.Elements<Cell>().FirstOrDefault(c => c.CellReference.Value == cellReference);
                if (cell != null)
                {
                    return cell;
                }
            }
            else
            {
                row = new Row { RowIndex = rowIndex };
                sheetData.Append(row);
            }

            Cell refCell = null;
            foreach (var cell in row.Elements<Cell>())
            {
                if (cell.CellReference.Value.Length == cellReference.Length)
                {
                    if (string.Compare(cell.CellReference.Value, cellReference, true) > 0)
                    {
                        refCell = cell;
                        break;
                    }
                }
            }

            var newCell = new Cell { CellReference = cellReference };
            row.InsertBefore(newCell, refCell);

            worksheet.Save();
            return newCell;
        }
    }
}