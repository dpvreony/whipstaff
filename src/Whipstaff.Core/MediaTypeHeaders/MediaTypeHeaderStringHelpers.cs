// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
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
        /// <example>
        /// <code>
        /// var mimeType = MediaTypeHeaderStringHelpers.ApplicationPdf;
        /// Console.WriteLine(mimeType); // application/pdf
        /// </code>
        /// </example>
        public static string ApplicationPdf => "application/pdf";

        /// <summary>
        /// Gets the mime type for an Open XML Office Spreadsheet.
        /// </summary>
        /// <example>
        /// <code>
        /// var mimeType = MediaTypeHeaderStringHelpers.ApplicationVndOpenXmlFormatsOfficeDocumentSpreadsheetMlSheet;
        /// Console.WriteLine(mimeType); // application/vnd.openxmlformats-officedocument.spreadsheetml.sheet
        /// </code>
        /// </example>
        public static string ApplicationVndOpenXmlFormatsOfficeDocumentSpreadsheetMlSheet =>
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    }
}
