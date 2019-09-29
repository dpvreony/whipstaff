// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Dhgms.AspNetCoreContrib.App.Features.Excel
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

            var sheets = workbook.AppendChild(new Sheets());

            if (sheetActors?.Count > 0)
            {
                AddWorkSheets(sheetActors, workbookPart, sheets);
            }

            return spreadsheetDocument;
        }

        private static void AddWorkSheets(
            IList<(string Name, Action<Sheet, WorksheetPart> Actor)> sheetActors,
            WorkbookPart workbookPart,
            Sheets sheets)
        {
            var sheetsToAdd = new List<Sheet>(sheetActors.Count);
            for (int i = 0; i < sheetActors.Count; i++)
            {
                var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                var sheetData = new SheetData();
                var worksheet = new Worksheet(sheetData);

                worksheetPart.Worksheet = worksheet;
                var workSheetPartId = workbookPart.GetIdOfPart(worksheetPart);

                var current = sheetActors[i];
                var name = current.Name;
                var actor = current.Actor;

                var sheet = new Sheet
                {
                    Id = workSheetPartId,
                    SheetId = (uint)i + 1,
                    Name = name,
                };

                actor(sheet, worksheetPart);
                sheetsToAdd.Add(sheet);
            }

            sheets.Append(sheetsToAdd);
        }
    }
}
