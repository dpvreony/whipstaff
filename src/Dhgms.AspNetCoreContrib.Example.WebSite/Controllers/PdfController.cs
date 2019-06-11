using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Dhgms.AspNetCoreContrib.Abstractions;
using Dhgms.AspNetCoreContrib.Controllers.Extensions;
using Dhgms.AspNetCoreContrib.Example.WebSite.Features.FileTransfer;
using Dhgms.AspNetCoreContrib.Example.WebSite.Features.MediaTypeHeaders;
using Dhgms.AspNetCoreContrib.Example.WebSite.Features.Pdf;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dhgms.AspNetCoreContrib.Example.WebSite.Controllers
{
    public sealed class PdfController : BaseFileDownloadController<int, DownloadPdfRequestDto>
    {
        public PdfController(
            IAuthorizationService authorizationService,
            ILogger<ExcelController> logger,
            IMediator mediator)
            : base(authorizationService, logger, mediator)
        {
        }

        protected override EventId GetViewEventId() => new EventId(1, "View PDF");

        protected override string GetViewPolicyName() => "View PDF";

        protected override async Task<DownloadPdfRequestDto> ViewCommandFactoryAsync(
            int id,
            ClaimsPrincipal claimsPrincipal,
            CancellationToken cancellationToken)
        {
            var result = new DownloadPdfRequestDto(id, claimsPrincipal);
            return result;
        }

        protected override string GetMediaTypeHeaderString() => MediaTypeHeaderStringHelpers.ApplicationPdf;
    }
}
