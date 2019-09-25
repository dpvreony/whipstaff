using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Dhgms.AspNetCoreContrib.Abstractions;
using Dhgms.AspNetCoreContrib.Controllers.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dhgms.AspNetCoreContrib.Example.WebSite.Features.FileTransfer
{
    public abstract class BaseFileDownloadController<TGetRequestDto, TQueryDto> : Controller
        where TQueryDto : IAuditableRequest<TGetRequestDto, FileNameAndStream>
    {
        private readonly IAuthorizationService _authorizationService;

        private readonly ILogger<BaseFileDownloadController<TGetRequestDto, TQueryDto>> _logger;

        private readonly IMediator _mediator;

        protected BaseFileDownloadController(
            IAuthorizationService authorizationService,
            ILogger<BaseFileDownloadController<TGetRequestDto, TQueryDto>> logger,
            IMediator mediator)
        {
            this._authorizationService = authorizationService ??
                                         throw new ArgumentNullException(nameof(authorizationService));

            this._logger = logger ??
                           throw new ArgumentNullException(nameof(logger));

            this._mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IActionResult> GetAsync(
            TGetRequestDto request,
            CancellationToken cancellationToken)
        {
            var viewPolicyName = this.GetViewPolicyName();
            var eventId = this.GetViewEventId();

            return await this.GetViewActionAsync<TGetRequestDto, FileNameAndStream, TQueryDto>(
                this._logger,
                this._mediator,
                this._authorizationService,
                request,
                eventId,
                viewPolicyName,
                this.GetViewActionResultAsync,
                this.ViewCommandFactoryAsync,
                cancellationToken).ConfigureAwait(false);
        }

        protected abstract EventId GetViewEventId();

        protected abstract string GetViewPolicyName();

        protected abstract Task<TQueryDto> ViewCommandFactoryAsync(
            TGetRequestDto id,
            ClaimsPrincipal claimsPrincipal,
            CancellationToken cancellationToken);

        protected abstract string GetMediaTypeHeaderString();

        private async Task<IActionResult> GetViewActionResultAsync(FileNameAndStream file)
        {
            var contentType = this.GetMediaTypeHeaderString();
            file.FileStream.Seek(0, SeekOrigin.Begin);
            return this.File(file.FileStream, contentType, file.FileName);
        }
    }
}
