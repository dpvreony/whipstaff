// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

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
