// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Whipstaff.Core.MediaTypeHeaders
{
    /// <summary>
    /// Helpers for getting mime types as strings.
    /// </summary>
    public static class MediaTypeHeaderStringHelpers
    {
        /*
         * use roslyn to generate these from Microsoft.AspNet.StaticFiles.FileExtensionContentTypeProvider
         */

        /// <summary>
        /// Gets the mime type for a PDF.
        /// </summary>
        public static string ApplicationPdf => "application/pdf";

        /// <summary>
        /// Gets the mime type for an Open XML Office Spreadsheet.
        /// </summary>
        public static string ApplicationVndOpenXmlFormatsOfficeDocumentSpreadsheetMlSheet =>
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    }
}
