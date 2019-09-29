using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Dhgms.AspNetCoreContrib.Abstractions;
using Dhgms.AspNetCoreContrib.App.Features.Excel;
using Dhgms.AspNetCoreContrib.App.Features.FileTransfer;
using Dhgms.AspNetCoreContrib.App.Features.MediaTypeHeaders;
using Dhgms.AspNetCoreContrib.Controllers;
using Dhgms.AspNetCoreContrib.Controllers.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

namespace Dhgms.AspNetCoreContrib.Example.WebSite.Controllers
{
    public sealed class ExcelController : BaseFileDownloadController<int, DownloadSpreadsheetRequestDto>
    {
        public ExcelController(
            IAuthorizationService authorizationService,
            ILogger<ExcelController> logger,
            IMediator mediator)
            : base(authorizationService, logger, mediator)
        {
        }

        protected override EventId GetViewEventId() => new EventId(1, "View Spreadsheet");

        protected override string GetViewPolicyName() => "ViewSpreadSheet";

        protected override async Task<DownloadSpreadsheetRequestDto> ViewCommandFactoryAsync(
            int id,
            ClaimsPrincipal claimsPrincipal,
            CancellationToken cancellationToken)
        {
            var result = new DownloadSpreadsheetRequestDto(id, claimsPrincipal);
            return result;
        }

        protected override string GetMediaTypeHeaderString() => MediaTypeHeaderStringHelpers.ApplicationVndOpenXmlFormatsOfficeDocumentSpreadsheetMlSheet;
    }
}
