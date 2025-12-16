// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Whipstaff.AspNetCore.Extensions;
using Whipstaff.Mediator;

namespace Whipstaff.AspNetCore.FileTransfer
{
    /// <summary>
    /// Base class for a controller that serves a file with a mime\type.
    /// </summary>
    /// <typeparam name="TGetRequestDto">The type for the api request dto.</typeparam>
    /// <typeparam name="TQueryDto">The type for the CQRS query dto.</typeparam>
    public abstract class BaseFileDownloadController<TGetRequestDto, TQueryDto> : Controller
        where TQueryDto : IAuditableQuery<TGetRequestDto, FileNameAndStreamModel?>
    {
        private readonly IAuthorizationService _authorizationService;

        private readonly ILogger<BaseFileDownloadController<TGetRequestDto, TQueryDto>> _logger;

        private readonly IMediator _mediator;
        private readonly Action<ILogger, string, Exception?> _viewLogAction;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseFileDownloadController{TGetRequestDto, TQueryDto}"/> class.
        /// </summary>
        /// <param name="authorizationService">Authorization service instance for verifying requests.</param>
        /// <param name="logger">Logging framework instance.</param>
        /// <param name="mediator">CQRS handler.</param>
        /// <param name="viewLogAction">Log Message Action for the View event.</param>
        protected BaseFileDownloadController(
            IAuthorizationService authorizationService,
            ILogger<BaseFileDownloadController<TGetRequestDto, TQueryDto>> logger,
            IMediator mediator,
            Action<ILogger, string, Exception?> viewLogAction)
        {
            ArgumentNullException.ThrowIfNull(authorizationService);
            ArgumentNullException.ThrowIfNull(logger);
            ArgumentNullException.ThrowIfNull(mediator);
            ArgumentNullException.ThrowIfNull(viewLogAction);

            _authorizationService = authorizationService;
            _logger = logger;
            _mediator = mediator;
            _viewLogAction = viewLogAction;
        }

        /// <summary>
        /// Allows for retrieval of a file.
        /// </summary>
        /// <param name="request">The request dto.</param>
        /// <param name="cancellationToken">Cancellation token for the process.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
#pragma warning disable S6967
        public async Task<IActionResult> GetAsync(
            TGetRequestDto request,
            CancellationToken cancellationToken)
#pragma warning restore S6967
        {
            var viewPolicyName = GetViewPolicyName();

            return await this.GetViewActionAsync<TGetRequestDto, FileNameAndStreamModel, TQueryDto>(
                _logger,
                _mediator,
                _authorizationService,
                request,
                _viewLogAction,
                viewPolicyName,
                GetViewActionResultAsync,
                ViewCommandFactoryAsync,
                cancellationToken).ConfigureAwait(false);
        }

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

        private async Task<IActionResult> GetViewActionResultAsync(FileNameAndStreamModel? file)
        {
            if (file == null)
            {
                return NotFound();
            }

            return await Task.Run(() =>
            {
                var contentType = GetMediaTypeHeaderString();
                _ = file.FileStream.Seek(0, SeekOrigin.Begin);
                return File(file.FileStream, contentType, file.FileName);
            }).ConfigureAwait(false);
        }
    }
}
