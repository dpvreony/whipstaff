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
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dhgms.AspNetCoreContrib.Example.WebSite.Controllers
{
    public sealed class PdfController : BaseFileDownloadController<int>
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

        protected override Task<IAuditableRequest<int, FileNameAndStream>> ViewCommandFactoryAsync(
            int id,
            ClaimsPrincipal claimsPrincipal,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        protected override string GetMediaTypeHeaderString() => MediaTypeHeaderStringHelpers.ApplicationPdf;
    }
}
