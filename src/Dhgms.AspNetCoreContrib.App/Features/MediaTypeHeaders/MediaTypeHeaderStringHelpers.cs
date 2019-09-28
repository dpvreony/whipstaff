// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Dhgms.AspNetCoreContrib.Example.WebSite.Features.MediaTypeHeaders
{
    public static class MediaTypeHeaderStringHelpers
    {
        //TODO : use roslyn to generate these from Microsoft.AspNet.StaticFiles.FileExtensionContentTypeProvider 

        public static string ApplicationPdf => "application/pdf";

        public static string ApplicationVndOpenXmlFormatsOfficeDocumentSpreadsheetMlSheet =>
            "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    }
}
