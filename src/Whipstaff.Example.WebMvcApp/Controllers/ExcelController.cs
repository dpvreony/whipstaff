// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Whipstaff.AspNetCore.FileTransfer;
using Whipstaff.Core.MediaTypeHeaders;
using Whipstaff.OpenXml.Excel;

namespace Dhgms.AspNetCoreContrib.Example.WebMvcApp.Controllers
{
    /// <summary>
    /// Sample web controller for an XLSX download.
    /// </summary>
    public sealed class ExcelController : AbstractFileDownloadController<int, DownloadSpreadsheetRequestDto>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelController"/> class.
        /// </summary>
        /// <param name="authorizationService">Instance of the authorization service for use in requests.</param>
        /// <param name="logger">Instance of logging framework.</param>
        /// <param name="mediator">Instance of the CQRS mediator.</param>
        public ExcelController(
            IAuthorizationService authorizationService,
            IMediator mediator,
            ILogger<ExcelController> logger)
            : base(
                authorizationService,
                mediator,
#pragma warning disable GR0012 // Constructors should minimise work and not execute methods
                LoggerMessage.Define<string>(LogLevel.Debug, new EventId(1), "{Message}"),
#pragma warning restore GR0012 // Constructors should minimise work and not execute methods
                logger)
        {
        }

        /// <inheritdoc/>
        protected override string GetViewPolicyName() => "ViewSpreadSheet";

        /// <inheritdoc/>
        protected override Task<DownloadSpreadsheetRequestDto> ViewCommandFactoryAsync(
            int request,
            ClaimsPrincipal claimsPrincipal,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(new DownloadSpreadsheetRequestDto(request, claimsPrincipal));
        }

        /// <inheritdoc/>
        protected override string GetMediaTypeHeaderString() => MediaTypeHeaderStringHelpers.ApplicationVndOpenXmlFormatsOfficeDocumentSpreadsheetMlSheet;
    }
}
