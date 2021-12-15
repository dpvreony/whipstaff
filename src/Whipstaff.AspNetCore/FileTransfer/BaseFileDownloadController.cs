// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Whipstaff.AspNetCore.Extensions;
using Whipstaff.Core;

namespace Whipstaff.AspNetCore.FileTransfer
{
    /// <summary>
    /// Base class for a controller that serves a file with a mime\type.
    /// </summary>
    /// <typeparam name="TGetRequestDto">The type for the api request dto.</typeparam>
    /// <typeparam name="TQueryDto">The type for the CQRS query dto.</typeparam>
    public abstract class BaseFileDownloadController<TGetRequestDto, TQueryDto> : Controller
        where TQueryDto : IAuditableRequest<TGetRequestDto, FileNameAndStreamModel>
    {
        private readonly IAuthorizationService _authorizationService;

        private readonly ILogger<BaseFileDownloadController<TGetRequestDto, TQueryDto>> _logger;

        private readonly IMediator _mediator;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseFileDownloadController{TGetRequestDto, TQueryDto}"/> class.
        /// </summary>
        /// <param name="authorizationService">Authorization service instance for verifying requests.</param>
        /// <param name="logger">Logging framework instance.</param>
        /// <param name="mediator">CQRS handler.</param>
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

        /// <summary>
        /// Allows for retrieval of a file.
        /// </summary>
        /// <param name="request">The request dto.</param>
        /// <param name="cancellationToken">Cancellation token for the process.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<IActionResult> GetAsync(
            TGetRequestDto request,
            CancellationToken cancellationToken)
        {
            var viewPolicyName = GetViewPolicyName();
            var eventId = GetViewEventId();

            return await this.GetViewActionAsync<TGetRequestDto, FileNameAndStreamModel, TQueryDto>(
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

        /// <summary>
        /// Gets the view event id. used for auditing, apm and logging.
        /// </summary>
        /// <returns>The event id.</returns>
        protected abstract EventId GetViewEventId();

        /// <summary>
        /// Gets the policy name for authorizing viewing of the file.
        /// </summary>
        /// <returns>Policy name.</returns>
        protected abstract string GetViewPolicyName();

        /// <summary>
        /// Gets the Command Factory for the View Command.
        /// </summary>
        /// <param name="request">The request dto for the api.</param>
        /// <param name="claimsPrincipal">Authentication principal associated with the request.</param>
        /// <param name="cancellationToken">Cancellation token for the process.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        protected abstract Task<TQueryDto> ViewCommandFactoryAsync(
            TGetRequestDto request,
            ClaimsPrincipal claimsPrincipal,
            CancellationToken cancellationToken);

        /// <summary>
        /// Gets the MIME type for the file.
        /// </summary>
        /// <returns>The mime type.</returns>
        protected abstract string GetMediaTypeHeaderString();

        private async Task<IActionResult> GetViewActionResultAsync(FileNameAndStreamModel file)
        {
            return await Task.Run(() =>
            {
                var contentType = GetMediaTypeHeaderString();
                file.FileStream.Seek(0, SeekOrigin.Begin);
                return File(file.FileStream, contentType, file.FileName);
            }).ConfigureAwait(false);
        }
    }
}
