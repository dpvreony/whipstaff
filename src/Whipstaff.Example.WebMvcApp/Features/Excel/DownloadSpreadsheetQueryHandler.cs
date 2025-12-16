// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Mediator;
using Whipstaff.AspNetCore.FileTransfer;

namespace Whipstaff.OpenXml.Excel
{
    /// <summary>
    /// Sample handler for generating and\or serving spreadsheets.
    /// </summary>
    internal sealed class DownloadSpreadsheetQueryHandler : IQueryHandler<DownloadSpreadsheetRequestDto, FileNameAndStreamModel?>
    {
        /// <inheritdoc/>
        public async ValueTask<FileNameAndStreamModel?> Handle(DownloadSpreadsheetRequestDto request, CancellationToken cancellationToken)
        {
            return await Task.Run(() =>
            {
                if (request.RequestDto != 0 && request.RequestDto % 2 == 0)
                {
                    // crude test for checking the 404 logic on even request ids.
                    return null;
                }

                var stream = new MemoryStream();
                var worksheetActors = new List<(string Name, Action<Sheet, WorksheetPart> Actor)>
                {
                    ("Sheet1", CreateSheetOne),
                    ("Sheet2", CreateSheetTwo),
                };

                using (var spreadsheet =
                       SpreadsheetDocumentHelper.GetWorkbookSpreadSheetDocument(stream, worksheetActors))
                {
                    spreadsheet.Save();
                }

                var fileName = $"{Guid.NewGuid()}.xlsx";

                return new FileNameAndStreamModel(fileName, stream);
            });
        }

        private static void CreateSheetOne(Sheet sheet, WorksheetPart worksheetPart)
        {
            uint currentRow = 1;
            _ = worksheetPart.InsertCellInWorksheet("A", currentRow, "Title");

            _ = sheet.GetFirstChild<SheetData>();
        }

        private static void CreateSheetTwo(Sheet sheet, WorksheetPart worksheetPart)
        {
            var worksheet = worksheetPart.Worksheet;
            var sheetData = worksheet.GetFirstChild<SheetData>();
            if (sheetData == null)
            {
                throw new InvalidOperationException("No sheet data in worksheet");
            }

            var row = new Row();

            row.Append(
                ConstructCell("Title"),
                ConstructCell("Forename"),
                ConstructCell("Surname"),
                ConstructCell("Date Of Birth"));

            _ = sheetData.AppendChild(row);
            worksheet.Save();
        }

        /// <summary>
        /// Creates a cell with a string value.
        /// </summary>
        /// <remarks>
        /// Taken from: http://www.dispatchertimer.com/tutorial/how-to-create-an-excel-file-in-net-using-openxml-part-2-export-a-collection-to-spreadsheet/
        /// stripped down for example.
        /// </remarks>
        /// <param name="value">Value to place in cell.</param>
        /// <returns>A worksheet cell.</returns>
        private static Cell ConstructCell(string value)
        {
            return new Cell
            {
                CellValue = new CellValue(value),
                DataType = new EnumValue<CellValues>(CellValues.String),
            };
        }
    }
}
