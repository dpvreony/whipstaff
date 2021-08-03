﻿// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Whipstaff.AspNetCore.Features.Pdf;
using Whipstaff.AspNetCore.FileTransfer;
using Whipstaff.Core.MediaTypeHeaders;

namespace Dhgms.AspNetCoreContrib.Example.WebSite.Controllers
{
    /// <summary>
    /// Example controller for serving pdf files.
    /// </summary>
    public sealed class PdfController : BaseFileDownloadController<int, DownloadPdfRequestDto>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PdfController"/> class.
        /// </summary>
        /// <param name="authorizationService">Authorization service for verifying requests.</param>
        /// <param name="logger">Logging framework.</param>
        /// <param name="mediator">CQRS mediatr.</param>
        public PdfController(
            IAuthorizationService authorizationService,
            ILogger<PdfController> logger,
            IMediator mediator)
            : base(authorizationService, logger, mediator)
        {
        }

        /// <inheritdoc />
        protected override EventId GetViewEventId() => new EventId(1, "View PDF");

        /// <inheritdoc />
        protected override string GetViewPolicyName() => "View PDF";

        /// <inheritdoc />
        protected override Task<DownloadPdfRequestDto> ViewCommandFactoryAsync(
            int request,
            ClaimsPrincipal claimsPrincipal,
            CancellationToken cancellationToken)
        {
            return Task.Run(
                () =>
                {
                    var result = new DownloadPdfRequestDto(request, claimsPrincipal);
                    return result;
                },
                cancellationToken);
        }

        /// <inheritdoc />
        protected override string GetMediaTypeHeaderString() => MediaTypeHeaderStringHelpers.ApplicationPdf;
    }
}
