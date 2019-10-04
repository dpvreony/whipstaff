// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Dhgms.AspNetCoreContrib.Abstractions;
using Dhgms.AspNetCoreContrib.App.Features.FileTransfer;
using Dhgms.AspNetCoreContrib.App.Features.MediaTypeHeaders;
using Dhgms.AspNetCoreContrib.App.Features.Pdf;
using Dhgms.AspNetCoreContrib.Controllers.Extensions;
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
            ILogger<PdfController> logger,
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
