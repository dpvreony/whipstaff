using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Dhgms.AspNetCoreContrib.Example.WebSite.Features.Excel
{
    public static class SpreadsheetDocumentHelper
    {
        public static SpreadsheetDocument GetWorkbookSpreadSheetDocument(
            Stream stream,
            IList<(string Name, Action<Sheet, WorksheetPart> Actor)> sheetActors)
        {
            var spreadsheetDocument = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook);

            // Add a WorkbookPart to the document.
            var workbookPart = spreadsheetDocument.AddWorkbookPart();
            var workbook = new Workbook();
            workbookPart.Workbook = workbook;

            // Add a WorksheetPart to the WorkbookPart.
            var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();

            var sheetData = new SheetData();
            var worksheet = new Worksheet(sheetData);
            worksheetPart.Worksheet = worksheet;

            var sheets = workbook.AppendChild(new Sheets());

            var workSheetPartId = workbookPart.GetIdOfPart(worksheetPart);

            if (sheetActors?.Count > 0)
            {
                var sheetsToAdd = new List<Sheet>(sheetActors.Count);
                foreach (var (name, actor) in sheetActors)
                {
                    var sheet = new Sheet
                    {
                        Id = workSheetPartId,
                        SheetId = 1,
                        Name = name,
                    };

                    actor(sheet, worksheetPart);
                    sheetsToAdd.Add(sheet);
                }

                sheets.Append(sheetsToAdd);
            }

            return spreadsheetDocument;
        }
    }
}
