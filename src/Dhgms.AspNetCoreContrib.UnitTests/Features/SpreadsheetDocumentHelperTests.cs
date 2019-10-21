using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Dhgms.AspNetCoreContrib.App.Features.Excel;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace Dhgms.AspNetCoreContrib.UnitTests.Features
{
    public static class SpreadsheetDocumentHelperTests
    {
        public sealed class GetWorkbookSpreadSheetDocumentMethod : Foundatio.Logging.Xunit.TestWithLoggingBase
        {
            public GetWorkbookSpreadSheetDocumentMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            [Fact]
            public void ReturnsSpreadSheet()
            {
                using (var stream = new MemoryStream())
                {
                    var sheetActors = new List<(string Name, Action<Sheet, WorksheetPart> Actor)>();
                    sheetActors.Add(("Sheet1", CreateSheet1));
                    var workbook = SpreadsheetDocumentHelper.GetWorkbookSpreadSheetDocument(stream, sheetActors);

                    Assert.NotNull(workbook);

                    workbook.Save();
                    workbook.Close();

                    var buffer = stream.ToArray();
                    var stringOutput = Encoding.UTF8.GetString(buffer);
                    _logger.LogDebug(stringOutput);
                }
            }

            private void CreateSheet1(Sheet sheet, WorksheetPart worksheetPart)
            {
                uint currentRow = 1;
                var titleCell = InsertCellInWorksheet("A", currentRow, worksheetPart);
                titleCell.CellValue = new CellValue("Title");
                currentRow++;
            }

        private void WriteStringToCell(
            string s,
            uint currentRow,
            WorksheetPart worksheetPart,
            string value,
            int columnSpan = 1,
            int rowSpan = 1)
        {
            var cell = InsertCellInWorksheet("A", currentRow, worksheetPart);
            cell.CellValue = new CellValue(value);
        }

        // Given a column name, a row index, and a WorksheetPart, inserts a cell into the worksheet.
        // If the cell already exists, returns it.
        private static Cell InsertCellInWorksheet(string columnName, uint rowIndex, WorksheetPart worksheetPart)
        {
            Worksheet worksheet = worksheetPart.Worksheet;
            SheetData sheetData = worksheet.GetFirstChild<SheetData>();
            string cellReference = columnName + rowIndex;

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
}
