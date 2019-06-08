using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Dhgms.AspNetCoreContrib.Example.WebSite.Features.MediaTypeHeaders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace Dhgms.AspNetCoreContrib.Example.WebSite.Controllers
{
    public class ExcelController : Controller
    {
        public async Task<IActionResult> GetAsync()
        {
            var filestream = Stream.Null;
            if (filestream == null)
            {
                return this.NotFound();
            }

            var contentType = MediaTypeHeaderStringHelpers.ApplicationVndOpenXmlFormatsOfficeDocumentSpreadsheetMlSheet;
            return this.File(filestream, contentType);
        }
    }
}
