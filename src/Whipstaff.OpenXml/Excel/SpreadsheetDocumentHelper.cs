// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Whipstaff.OpenXml.Excel
{
    /// <summary>
    /// Helpers for working with Open XML Spreadsheet Documents.
    /// </summary>
    public static class SpreadsheetDocumentHelper
    {
        /// <summary>
        /// Gets a workbook spreadsheet document.
        /// </summary>
        /// <param name="stream">Underlying stream for the file.</param>
        /// <param name="sheetActors">A collection of actors used to build up worksheets inside the workbook.</param>
        /// <returns>A spreadsheet document.</returns>
        public static SpreadsheetDocument GetWorkbookSpreadSheetDocument(
            Stream stream,
            IList<SheetActorFuncModel> sheetActors)
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
            IList<SheetActorFuncModel> sheetActors,
            WorkbookPart workbookPart,
            Sheets sheets)
        {
            var sheetsToAdd = new List<Sheet>(sheetActors.Count);
            for (int i = 0; i < sheetActors.Count; i++)
            {
                var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                var sheetData = new SheetData();
                var sheetElements = new OpenXmlElement[]
                {
                    sheetData,
                };

                worksheetPart.Worksheet = new Worksheet(sheetElements);
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
