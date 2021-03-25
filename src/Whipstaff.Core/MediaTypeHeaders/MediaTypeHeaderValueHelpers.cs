// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Net.Http.Headers;

namespace Whipstaff.Core.MediaTypeHeaders
{
    /// <summary>
    /// Helpers for getting mime types as header values.
    /// </summary>
    public static class MediaTypeHeaderValueHelpers
    {
        /// <summary>
        /// Gets the Mime Type for an Open XML Office Spreadsheet.
        /// </summary>
        /// <returns>Mime Type.</returns>
        public static MediaTypeHeaderValue ApplicationVndOpenXmlFormatsOfficeDocumentSpreadsheetMlSheet()
            => new MediaTypeHeaderValue(MediaTypeHeaderStringHelpers.ApplicationVndOpenXmlFormatsOfficeDocumentSpreadsheetMlSheet);
    }
}
