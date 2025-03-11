// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Extensions.Logging;
using Whipstaff.OpenXml.Excel;
using Whipstaff.Testing.Logging;
using Xunit;

namespace Whipstaff.UnitTests.Features
{
    /// <summary>
    /// Unit tests for the Excel Spreadsheet document helper.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class SpreadsheetDocumentHelperTests
    {
        /// <summary>
        /// Unit tests for workbook generation.
        /// </summary>
        public sealed class GetWorkbookSpreadSheetDocumentMethod : TestWithLoggingBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="GetWorkbookSpreadSheetDocumentMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit Test Output helper.</param>
            public GetWorkbookSpreadSheetDocumentMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <summary>
            /// Tests to ensure a spreadsheet is returned.
            /// </summary>
            [Fact]
            public void ReturnsSpreadSheet()
            {
                using (var stream = new MemoryStream())
                {
                    var sheetActors = new List<(string Name, Action<Sheet, WorksheetPart> Actor)>();
                    sheetActors.Add(("Sheet1", CreateSheet1));

                    using (var workbook = SpreadsheetDocumentHelper.GetWorkbookSpreadSheetDocument(stream, sheetActors))
                    {
                        Assert.NotNull(workbook);

                        workbook.Save();
                    }

                    var buffer = stream.ToArray();
                    var stringOutput = Encoding.UTF8.GetString(buffer);
                    Logger.LogDebug(stringOutput);
                }
            }

            private static void CreateSheet1(Sheet sheet, WorksheetPart worksheetPart)
            {
                uint currentRow = 1;
                _ = worksheetPart.InsertCellInWorksheet("A", currentRow, "title");
            }
        }
    }
}
