using System;
using System.Collections.Generic;
using System.Globalization;
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
            var titleCell = worksheetPart.InsertCellInWorksheet("A", currentRow, "Title");
        }
    }
}