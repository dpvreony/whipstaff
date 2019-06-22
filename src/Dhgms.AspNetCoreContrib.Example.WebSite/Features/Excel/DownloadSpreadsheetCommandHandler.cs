using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dhgms.AspNetCoreContrib.Example.WebSite.Features.FileTransfer;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using MediatR;

namespace Dhgms.AspNetCoreContrib.Example.WebSite.Features.Excel
{
    public sealed class DownloadSpreadsheetCommandHandler : IRequestHandler<DownloadSpreadsheetRequestDto, FileNameAndStream>
    {
        public async Task<FileNameAndStream> Handle(DownloadSpreadsheetRequestDto request, CancellationToken cancellationToken)
        {
            if (request.RequestDto != 0 && request.RequestDto % 2 == 0)
            {
                // crude test for checking the 404 logic on even request ids.
                return null;
            }

            var stream = new MemoryStream();
            var worksheetActors = new List<(string Name, Action<Sheet, WorksheetPart> Actor)>
            {
                ("Sheet1", this.CreateSheetOne),
            };
            var spreadsheet = SpreadsheetDocumentHelper.GetWorkbookSpreadSheetDocument(stream, worksheetActors);
            spreadsheet.Save();
            spreadsheet.Close();

            var fileName = $"{Guid.NewGuid()}.xlsx";

            return new FileNameAndStream
            {
                FileName = fileName,
                FileStream = stream,
            };
        }

        private void CreateSheetOne(Sheet sheet, WorksheetPart worksheetPart)
        {
            uint currentRow = 1;
            var titleCell = InsertCellInWorksheet("A", currentRow, worksheetPart);
            titleCell.CellValue = new CellValue("Title");
            titleCell.DataType = new EnumValue<CellValues>(CellValues.String);
        }

        // Given a column name, a row index, and a WorksheetPart, inserts a cell into the worksheet. 
        // If the cell already exists, returns it. 
        private static Cell InsertCellInWorksheet(string columnName, uint rowIndex, WorksheetPart worksheetPart)
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
