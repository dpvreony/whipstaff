// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Whipstaff.AspNetCore;
using Whipstaff.AspNetCore.Features.Swagger;
using Whipstaff.Core;
using Whipstaff.Testing.Cqrs;

namespace Whipstaff.Testing
{
    /// <summary>
    /// Fake CRUD Web Controller, for use in examples and unit tests.
    /// </summary>
    [ExcludeFromCodeCoverage]
    [ApiController]
    [Route("api/fakecrud")]
    public sealed class FakeCrudController : AbstractCrudApiController<
        FakeCrudListQuery,
        FakeCrudListRequest,
        IList<int>,
        FakeCrudViewQuery,
        FakeCrudViewResponse,
        FakeCrudAddCommand,
        int,
        int?,
        FakeCrudDeleteCommand,
        long?,
        FakeCrudUpdateCommand,
        int,
        FakeCrudUpdateResponse?,
        FakeCrudControllerLogMessageActions>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FakeCrudController"/> class.
        /// </summary>
        /// <param name="authorizationService">The authorization service for validating access.</param>
        /// <param name="mediator">The mediator object to publish CQRS messages to.</param>
        /// <param name="commandFactory">Factory for Commands.</param>
        /// <param name="queryFactory">Factory for Queries.</param>
        /// <param name="logMessageActions">Logging Framework Message Actions Factory.</param>
        /// <param name="logger">The logger object.</param>
        public FakeCrudController(
            IAuthorizationService authorizationService,
            IMediator mediator,
            FakeAuditableCommandFactory commandFactory,
            FakeAuditableQueryFactory queryFactory,
            FakeCrudControllerLogMessageActions logMessageActions,
            ILogger<FakeCrudController> logger)
            : base(
                authorizationService,
                mediator,
                commandFactory,
                queryFactory,
                logMessageActions,
                logger)
        {
        }

        /// <inheritdoc />
        protected override Task<FakeCrudListQuery> GetListQueryAsync(
            FakeCrudListRequest listRequestDto,
            ClaimsPrincipal claimsPrincipal,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(new FakeCrudListQuery(listRequestDto, claimsPrincipal));
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
        protected override async Task<ActionResult<IList<int>>> GetListActionResultAsync(IList<int> listResponse)
        {
            return await Task.FromResult(Ok(listResponse)).ConfigureAwait(false);
        }

        /// <inheritdoc />
        protected override async Task<ActionResult<FakeCrudViewResponse>> GetViewActionResultAsync(FakeCrudViewResponse? viewResponse)
        {
            return await Task.FromResult(Ok(viewResponse)).ConfigureAwait(false);
        }

        /// <inheritdoc />
        protected override Task<FakeCrudViewQuery> GetViewQueryAsync(long id, ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
        {
            return Task.FromResult(new FakeCrudViewQuery(id, claimsPrincipal));
        }

        /// <inheritdoc />
        protected override async Task<ActionResult<int?>> GetAddActionResultAsync(int? result)
        {
            return await Task.FromResult(Ok(result)).ConfigureAwait(false);
        }

        /// <inheritdoc />
        protected override Task<FakeCrudAddCommand> GetAddCommandAsync(int addRequestDto, ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
        {
            return Task.FromResult(new FakeCrudAddCommand(addRequestDto, claimsPrincipal));
        }

        /// <inheritdoc />
        protected override async Task<string> GetAddPolicyAsync()
        {
            return await Task.FromResult("addPolicyName").ConfigureAwait(false);
        }

        /// <inheritdoc />
        protected override Task<FakeCrudDeleteCommand> GetDeleteCommandAsync(
            long id,
            ClaimsPrincipal claimsPrincipal,
            CancellationToken cancellationToken)
        {
            return Task.FromResult(new FakeCrudDeleteCommand(id, claimsPrincipal));
        }

        /// <inheritdoc />
        protected override async Task<ActionResult<long?>> GetDeleteActionResultAsync(long? result)
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
        protected override async Task<string> GetUpdatePolicyAsync()
        {
            return await Task.FromResult("updatePolicyName").ConfigureAwait(false);
        }

        /// <inheritdoc />
        protected override async Task<ActionResult<FakeCrudUpdateResponse?>> GetUpdateActionResultAsync(FakeCrudUpdateResponse? result)
        {
            return await Task.FromResult(Ok(result)).ConfigureAwait(false);
        }
    }
}
