// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Dhgms.AspNetCoreContrib.Abstractions;
using Dhgms.AspNetCoreContrib.App.Features.Swagger;
using Dhgms.AspNetCoreContrib.Controllers;
using Dhgms.AspNetCoreContrib.Fakes.Cqrs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dhgms.AspNetCoreContrib.Fakes
{
    /// <summary>
    /// Fake CRUD Web Controller, for use in examples and unit tests.
    /// </summary>
    [SwaggerClassMetaData(typeof(FakeCrudControllerSwaggerMetaData))]
    [ExcludeFromCodeCoverage]
    public sealed class FakeCrudController : CrudController<FakeCrudController, FakeCrudListQuery, FakeCrudListRequest, IList<int>, FakeCrudViewQuery, FakeCrudViewResponse, FakeCrudAddCommand, int, int, FakeCrudDeleteCommand, long, FakeCrudUpdateCommand, int, FakeCrudUpdateResponse>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FakeCrudController"/> class.
        /// </summary>
        /// <param name="authorizationService">The authorization service for validating access.</param>
        /// <param name="logger">The logger object.</param>
        /// <param name="mediator">The mediatr object to publish CQRS messages to.</param>
        public FakeCrudController(
            IAuthorizationService authorizationService,
            ILogger<FakeCrudController> logger,
            IMediator mediator)
            : base(
                authorizationService,
                logger,
                mediator)
        {
        }

        /// <inheritdoc />
        protected override Task<FakeCrudListQuery> GetListQueryAsync(FakeCrudListRequest listRequestDto, ClaimsPrincipal claimsPrincipal,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(new FakeCrudListQuery(listRequestDto, claimsPrincipal));
        }

        /// <inheritdoc />
        protected override async Task<EventId> GetListEventIdAsync()
        {
            return await Task.Run(() => new EventId(1)).ConfigureAwait(false);
        }

        /// <inheritdoc />
        protected override async Task<EventId> GetViewEventIdAsync()
        {
            return await Task.Run(() => new EventId(2)).ConfigureAwait(false);
        }

        /// <inheritdoc />
        protected override async Task<string> GetListPolicyAsync()
        {
            return await Task.FromResult("listPolicyName").ConfigureAwait(false);
        }

        /// <inheritdoc />
        protected override async Task<string> GetViewPolicyAsync()
        {
            return await Task.FromResult("viewPolicyName").ConfigureAwait(false);
        }

        /// <inheritdoc />
        protected override async Task<IActionResult> GetListActionResultAsync(IList<int> listResponse)
        {
            return await Task.FromResult(Ok(listResponse)).ConfigureAwait(false);
        }

        /// <inheritdoc />
        protected override async Task<IActionResult> GetViewActionResultAsync(FakeCrudViewResponse viewResponse)
        {
            return await Task.FromResult(Ok(viewResponse)).ConfigureAwait(false);
        }

        /// <inheritdoc />
        protected override Task<FakeCrudViewQuery> GetViewQueryAsync(long id, ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
        {
            return Task.FromResult(new FakeCrudViewQuery(id, claimsPrincipal));
        }

        /// <inheritdoc />
        protected override async Task<IActionResult> GetAddActionResultAsync(int result)
        {
            return await Task.FromResult(Ok(result)).ConfigureAwait(false);
        }

        /// <inheritdoc />
        protected override Task<FakeCrudAddCommand> GetAddCommandAsync(int addRequestDto, ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
        {
            return Task.FromResult(new FakeCrudAddCommand(addRequestDto, claimsPrincipal));
        }

        /// <inheritdoc />
        protected override async Task<EventId> GetAddEventIdAsync()
        {
            return await Task.FromResult(new EventId(3)).ConfigureAwait(false);
        }

        /// <inheritdoc />
        protected override async Task<string> GetAddPolicyAsync()
        {
            return await Task.FromResult("addPolicyName").ConfigureAwait(false);
        }

        /// <inheritdoc />
        protected override Task<FakeCrudDeleteCommand> GetDeleteCommand(
            long id,
            ClaimsPrincipal claimsPrincipal,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(new FakeCrudDeleteCommand(id, claimsPrincipal));
        }

        /// <inheritdoc />
        protected override async Task<EventId> GetDeleteEventIdAsync()
        {
            return await Task.FromResult(new EventId(4)).ConfigureAwait(false);
        }

        /// <inheritdoc />
        protected override async Task<IActionResult> GetDeleteActionResultAsync(long result)
        {
            return await Task.FromResult(Ok(result)).ConfigureAwait(false);
        }

        /// <inheritdoc />
        protected override async Task<string> GetDeletePolicyAsync()
        {
            return await Task.FromResult("deletePolicyName").ConfigureAwait(false);
        }

        /// <inheritdoc />
        protected override Task<FakeCrudUpdateCommand> GetUpdateCommandAsync(
            int updateRequestDto,
            ClaimsPrincipal claimsPrincipal,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(new FakeCrudUpdateCommand(updateRequestDto, claimsPrincipal));
        }

        /// <inheritdoc />
        protected override async Task<EventId> GetUpdateEventIdAsync()
        {
            return await Task.Run(() => new EventId(5)).ConfigureAwait(false);
        }

        /// <inheritdoc />
        protected override async Task<string> GetUpdatePolicyAsync()
        {
            return await Task.FromResult("updatePolicyName").ConfigureAwait(false);
        }

        /// <inheritdoc />
        protected override async Task<IActionResult> GetUpdateActionResultAsync(FakeCrudUpdateResponse result)
        {
            return await Task.FromResult(Ok(result)).ConfigureAwait(false);
        }
    }
}
