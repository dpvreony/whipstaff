// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Dhgms.AspNetCoreContrib.Abstractions;
using Dhgms.AspNetCoreContrib.Controllers.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dhgms.AspNetCoreContrib.App.Features.FileTransfer
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
            _authorizationService = authorizationService ??
                                         throw new ArgumentNullException(nameof(authorizationService));

            _logger = logger ??
                           throw new ArgumentNullException(nameof(logger));

            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<IActionResult> GetAsync(
            TGetRequestDto request,
            CancellationToken cancellationToken)
        {
            var viewPolicyName = GetViewPolicyName();
            var eventId = GetViewEventId();

            return await this.GetViewActionAsync<TGetRequestDto, FileNameAndStream, TQueryDto>(
                _logger,
                _mediator,
                _authorizationService,
                request,
                eventId,
                viewPolicyName,
                GetViewActionResultAsync,
                ViewCommandFactoryAsync,
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
            var contentType = GetMediaTypeHeaderString();
            file.FileStream.Seek(0, SeekOrigin.Begin);
            return File(file.FileStream, contentType, file.FileName);
        }
    }
}
