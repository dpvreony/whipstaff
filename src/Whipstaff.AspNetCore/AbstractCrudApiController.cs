// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Mediator;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Whipstaff.AspNetCore.Extensions;
using Whipstaff.AspNetCore.Features.Logging;
using Whipstaff.Mediator;

namespace Whipstaff.AspNetCore
{
    /// <summary>
    /// A generic controller supporting CRUD operations. Pre-defines CQRS activities along with Authorization and logging.
    /// </summary>
    /// <typeparam name="TListQuery">The type for the List Query.</typeparam>
    /// <typeparam name="TListRequestDto">The type for the Request DTO for the List Operation.</typeparam>
    /// <typeparam name="TListQueryResponse">The type for the Response DTO for the List Operation.</typeparam>
    /// <typeparam name="TViewQuery">The type for the View Query.</typeparam>
    /// <typeparam name="TViewQueryResponse">The type for the Response DTO for the View Operation.</typeparam>
    /// <typeparam name="TAddCommand">The type for the Add Command.</typeparam>
    /// <typeparam name="TAddRequestDto">The type for the Request DTO for the Add Operation.</typeparam>
    /// <typeparam name="TAddResponseDto">The type for the Response DTO for the Add Operation.</typeparam>
    /// <typeparam name="TDeleteCommand">The type for the Delete Command.</typeparam>
    /// <typeparam name="TDeleteResponseDto">The type for the Request DTO for the Delete Operation.</typeparam>
    /// <typeparam name="TUpdateCommand">The type for the Update Command.</typeparam>
    /// <typeparam name="TUpdateRequestDto">The type for the Request DTO for the Update Operation.</typeparam>
    /// <typeparam name="TUpdateResponseDto">The type for the Response DTO for the Update Operation.</typeparam>
    /// <typeparam name="TCrudControllerLogMessageActions">The type for the log message actions mapping class.</typeparam>
#pragma warning disable S6934
    public abstract class AbstractCrudApiController<
#pragma warning restore S6934
            TListQuery,
            TListRequestDto,
            TListQueryResponse,
            TViewQuery,
            TViewQueryResponse,
            TAddCommand,
            TAddRequestDto,
            TAddResponseDto,
            TDeleteCommand,
            TDeleteResponseDto,
            TUpdateCommand,
            TUpdateRequestDto,
            TUpdateResponseDto,
            TCrudControllerLogMessageActions>
        : AbstractQueryOnlyApiController<TListQuery, TListRequestDto, TListQueryResponse, TViewQuery, TViewQueryResponse, TCrudControllerLogMessageActions>
        where TListQuery : IAuditableQuery<TListRequestDto, TListQueryResponse>
        where TListRequestDto : class, new()
        where TListQueryResponse : class
        where TViewQuery : IAuditableQuery<long, TViewQueryResponse?>
        where TViewQueryResponse : class
        where TAddCommand : IAuditableCommand<TAddRequestDto, TAddResponseDto?>
        where TDeleteCommand : IAuditableCommand<long, TDeleteResponseDto?>
        where TUpdateCommand : IAuditableCommand<TUpdateRequestDto, TUpdateResponseDto?>
        where TUpdateResponseDto : class?
        where TCrudControllerLogMessageActions : ICrudControllerLogMessageActions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractCrudApiController{TListQuery, TListRequestDto, TListQueryResponse, TViewQuery, TViewQueryResponse, TAddCommand, TAddRequestDto, TAddResponseDto, TDeleteCommand, TDeleteResponseDto, TUpdateCommand, TUpdateRequestDto, TUpdateResponseDto, TCrudControllerLogMessageActions}"/> class.
        /// </summary>
        /// <param name="authorizationService">The authorization service for validating access.</param>
        /// <param name="logger">The logger object.</param>
        /// <param name="mediator">The mediator object to publish CQRS messages to.</param>
        /// <param name="commandFactory">The factory for generating Command messages.</param>
        /// <param name="queryFactory">The factory for generating Query messages.</param>
        /// <param name="logMessageActions">Log Message Actions for the logging events in the controller.</param>
        protected AbstractCrudApiController(
            IAuthorizationService authorizationService,
            ILogger<AbstractCrudApiController<
                TListQuery,
                TListRequestDto,
                TListQueryResponse,
                TViewQuery,
                TViewQueryResponse,
                TAddCommand,
                TAddRequestDto,
                TAddResponseDto,
                TDeleteCommand,
                TDeleteResponseDto,
                TUpdateCommand,
                TUpdateRequestDto,
                TUpdateResponseDto,
                TCrudControllerLogMessageActions>> logger,
            IMediator mediator,
            IAuditableCommandFactory<TAddCommand, TAddRequestDto, TAddResponseDto, TDeleteCommand, TDeleteResponseDto, TUpdateCommand, TUpdateRequestDto, TUpdateResponseDto> commandFactory,
            IAuditableQueryFactory<TListQuery, TListRequestDto, TListQueryResponse, TViewQuery, TViewQueryResponse> queryFactory,
            TCrudControllerLogMessageActions logMessageActions)
            : base(
                  authorizationService,
                  logger,
                  mediator,
                  queryFactory,
                  logMessageActions)
        {
            ArgumentNullException.ThrowIfNull(commandFactory);
            CommandFactory = commandFactory;
        }

        /// <summary>
        /// Gets the Command Factory used for creating Commands to pass through the mediator.
        /// </summary>
        protected IAuditableCommandFactory<
            TAddCommand,
            TAddRequestDto,
            TAddResponseDto,
            TDeleteCommand,
            TDeleteResponseDto,
            TUpdateCommand,
            TUpdateRequestDto,
            TUpdateResponseDto> CommandFactory
        {
            get;
        }

        /// <summary>
        /// Operation to Delete an entity.
        /// </summary>
        /// <param name="id">Unique ID of the entity to be deleted.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpDelete("{id:long}")]
#pragma warning disable S6967
        public async Task<ActionResult<TDeleteResponseDto>> DeleteAsync(
            long id,
            CancellationToken cancellationToken)
#pragma warning restore S6967
        {
            var deletePolicyName = await GetDeletePolicyAsync().ConfigureAwait(false);

            return await this.GetDeleteActionAsync<TDeleteResponseDto, TDeleteCommand>(
                Logger,
                Mediator,
                AuthorizationService,
                id,
                LogMessageActionMappings.DeleteEventLogMessageAction,
                deletePolicyName,
                r => GetDeleteActionResultAsync(r),
                (a, cp, cx) => GetDeleteCommandAsync(a, cp, cx),
                cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Operation to create an Entity.
        /// </summary>
        /// <param name="addRequestDto">The Request DTO for the Add Operation.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpPost]
#pragma warning disable S6967
        public async Task<ActionResult<TAddResponseDto>> PostAsync(
            TAddRequestDto addRequestDto,
            CancellationToken cancellationToken)
#pragma warning restore S6967
        {
            var addPolicyName = await GetAddPolicyAsync().ConfigureAwait(false);

            return await this.GetAddActionAsync<TAddRequestDto, TAddResponseDto, TAddCommand>(
                Logger,
                Mediator,
                AuthorizationService,
                addRequestDto,
                LogMessageActionMappings.AddEventLogMessageAction,
                addPolicyName,
                r => GetAddActionResultAsync(r),
                (r, cp, cx) => GetAddCommandAsync(r, cp, cx),
                cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Operation to update an entity.
        /// </summary>
        /// <param name="id">Unique Id of the entity.</param>
        /// <param name="updateRequestDto">The Request DTO of the Update operation.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpPut("{id:long}")]
#pragma warning disable S6967
        public async Task<ActionResult<TUpdateResponseDto>> PutAsync(
            long id,
            TUpdateRequestDto updateRequestDto,
            CancellationToken cancellationToken)
#pragma warning restore S6967
        {
            var updatePolicyName = await GetUpdatePolicyAsync().ConfigureAwait(false);

            return await this.GetUpdateActionAsync<TUpdateRequestDto, TUpdateResponseDto, TUpdateCommand>(
                Logger,
                Mediator,
                AuthorizationService,
                id,
                updateRequestDto,
                LogMessageActionMappings.UpdateEventLogMessageAction,
                updatePolicyName,
                r => GetUpdateActionResultAsync(r),
                (r, cp, cx) => GetUpdateCommandAsync(r, cp, cx),
                cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the Action Result for the Add operation.
        /// </summary>
        /// <param name="result">The Response DTO from the CQRS operation.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        protected abstract Task<ActionResult<TAddResponseDto>> GetAddActionResultAsync(TAddResponseDto result);

        /// <summary>
        /// Gets the CQRS Command for the Add operation.
        /// </summary>
        /// <param name="addRequestDto">The Add Request DTO.</param>
        /// <param name="claimsPrincipal">The claims principal of the user requesting the add.</param>
        /// <param name="cancellationToken">The cancellation token for the request.</param>
        /// <returns>Command for performing an Add.</returns>
        protected abstract Task<TAddCommand> GetAddCommandAsync(
            TAddRequestDto addRequestDto,
            ClaimsPrincipal claimsPrincipal,
            CancellationToken cancellationToken);

        /// <summary>
        /// Gets the authorization policy for the Add operation.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        protected abstract Task<string> GetAddPolicyAsync();

        /// <summary>
        /// Gets the Action Result for the Delete operation.
        /// </summary>
        /// <param name="result">The Response DTO from the CQRS operation.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        protected abstract Task<ActionResult<TDeleteResponseDto>> GetDeleteActionResultAsync(TDeleteResponseDto result);

        /// <summary>
        /// Gets the CQRS Delete Command.
        /// </summary>
        /// <param name="id">Unique Id for the entity to delete.</param>
        /// <param name="claimsPrincipal">Claims Principal of the user requesting deletion.</param>
        /// <param name="cancellationToken">Cancellation token for the request.</param>
        /// <returns>Command for performing a delete.</returns>
        protected abstract Task<TDeleteCommand> GetDeleteCommandAsync(
            long id,
            ClaimsPrincipal claimsPrincipal,
            CancellationToken cancellationToken);

        /// <summary>
        /// Gets the authorization policy for the Delete operation.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        protected abstract Task<string> GetDeletePolicyAsync();

        /// <summary>
        /// Gets the Action Result for the Update operation.
        /// </summary>
        /// <param name="result">The Response DTO from the CQRS operation.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        protected abstract Task<ActionResult<TUpdateResponseDto>> GetUpdateActionResultAsync(TUpdateResponseDto? result);

        /// <summary>
        /// Gets the CQRS update command for the request.
        /// </summary>
        /// <param name="updateRequestDto">The Update request DTO.</param>
        /// <param name="claimsPrincipal">The claims principal of the user requesting the update.</param>
        /// <param name="cancellationToken">The cancellation token for the request.</param>
        /// <returns>CQRS Update Command.</returns>
        protected abstract Task<TUpdateCommand> GetUpdateCommandAsync(
            TUpdateRequestDto updateRequestDto,
            ClaimsPrincipal claimsPrincipal,
            CancellationToken cancellationToken);

        /// <summary>
        /// Gets the authorization policy for the Update operation.
        /// </summary>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        protected abstract Task<string> GetUpdatePolicyAsync();
    }
}
