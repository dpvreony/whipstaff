using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Net.Http.Headers;

namespace Dhgms.AspNetCoreContrib.Example.WebSite.Features.MediaTypeHeaders
{
    public class MediaTypeHeaderValueHelpers
    {
        public static MediaTypeHeaderValue ApplicationVndOpenXmlFormatsOfficeDocumentSpreadsheetMlSheet()
            => new MediaTypeHeaderValue(MediaTypeHeaderStringHelpers.ApplicationVndOpenXmlFormatsOfficeDocumentSpreadsheetMlSheet);
    }
}
