using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Dhgms.AspNetCoreContrib.Abstractions;
using Dhgms.AspNetCoreContrib.Controllers.Extensions;
using Dhgms.AspNetCoreContrib.Example.WebSite.Features.MediaTypeHeaders;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;

namespace Dhgms.AspNetCoreContrib.Example.WebSite.Controllers
{
    public sealed class ExcelController : Controller
    {
        private IAuthorizationService _authorizationService;

        private ILogger<ExcelController> _logger;

        private IMediator _mediator;

        public ExcelController(
            IAuthorizationService authorizationService,
            ILogger<ExcelController> logger,
            IMediator mediator)
        {
            this._authorizationService = authorizationService ??
                                        throw new ArgumentNullException(nameof(authorizationService));
            this._logger = logger ??
                          throw new ArgumentNullException(nameof(logger));

            this._mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        }

        public async Task<IActionResult> GetAsync()
        {
            int viewRequestDto = 1;
            string viewPolicyName = "viewExcelSpreadSheet";

            return await this.GetViewActionAsync<int, FileNameAndStream, IAuditableRequest<int, FileNameAndStream>>(
                this._logger,
                _mediator,
                _authorizationService,
                viewRequestDto,
                new EventId(1, "View Spreadsheet"),
                viewPolicyName,
                getViewActionResultAsync,
                viewCommandFactoryAsync,
                CancellationToken.None).ConfigureAwait(false);
        }

        private Task<IAuditableRequest<int, FileNameAndStream>> viewCommandFactoryAsync(
            int id,
            ClaimsPrincipal claimsPrincipal,
            CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        private async Task<IActionResult> getViewActionResultAsync(FileNameAndStream file)
        {
            var contentType = MediaTypeHeaderStringHelpers.ApplicationVndOpenXmlFormatsOfficeDocumentSpreadsheetMlSheet;
            return this.File(file.FileStream, contentType, file.FileName);
        }

        public sealed class FileNameAndStream
        {
            public string FileName { get; set; }
            public Stream FileStream { get; set; }
        }
    }
}
