// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Whipstaff.AspNetCore.Extensions;
using Whipstaff.Core;

namespace Whipstaff.AspNetCore
{
    /// <summary>
    /// A generic controller supporting CRUD operations. Pre-defines CQRS activities along with Authorization and logging.
    /// </summary>
    /// <typeparam name="TInheritingClass">The type of the inheriting class. Used for compile time validation of objects passed in such as the logger.</typeparam>
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
    [SuppressMessage("csharpsquid", "S2436: Classes and methods should not have too many generic parameters", Justification = "By design, need large number of generics to make this powerful enough for re-use in pattern")]
    public abstract class CrudController<TInheritingClass, TListQuery, TListRequestDto, TListQueryResponse, TViewQuery, TViewQueryResponse, TAddCommand, TAddRequestDto, TAddResponseDto, TDeleteCommand, TDeleteResponseDto, TUpdateCommand, TUpdateRequestDto, TUpdateResponseDto>
        : QueryOnlyController<TInheritingClass, TListQuery, TListRequestDto, TListQueryResponse, TViewQuery, TViewQueryResponse>
        where TInheritingClass : CrudController<TInheritingClass, TListQuery, TListRequestDto, TListQueryResponse, TViewQuery, TViewQueryResponse, TAddCommand, TAddRequestDto, TAddResponseDto, TDeleteCommand, TDeleteResponseDto, TUpdateCommand, TUpdateRequestDto, TUpdateResponseDto>
        where TAddCommand : IAuditableRequest<TAddRequestDto, TAddResponseDto>
        where TDeleteCommand : IAuditableRequest<long, TDeleteResponseDto>
        where TListQuery : IAuditableRequest<TListRequestDto, TListQueryResponse>
        where TListQueryResponse : class
        where TListRequestDto : class, new()
        where TViewQuery : IAuditableRequest<long, TViewQueryResponse>
        where TViewQueryResponse : class
        where TUpdateCommand : IAuditableRequest<TUpdateRequestDto, TUpdateResponseDto>
        where TUpdateResponseDto : class
    {
        private readonly Action<ILogger, string, Exception?> _addLogAction;
        private readonly Action<ILogger, string, Exception?> _deleteLogAction;
        private readonly Action<ILogger, string, Exception?> _updateLogAction;

        /// <summary>
        /// Initializes a new instance of the <see cref="CrudController{TInheritingClass, TListQuery, TListRequestDto, TListQueryResponse, TViewQuery, TViewQueryResponse, TAddCommand, TAddRequestDto, TAddResponseDto, TDeleteCommand, TDeleteResponseDto, TUpdateCommand, TUpdateRequestDto, TUpdateResponseDto}"/> class.
        /// </summary>
        /// <param name="authorizationService">The authorization service for validating access.</param>
        /// <param name="logger">The logger object.</param>
        /// <param name="mediator">The mediatr object to publish CQRS messages to.</param>
        /// <param name="commandFactory">The factory for generating Command messages.</param>
        /// <param name="queryFactory">The factory for generating Query messages.</param>
        /// <param name="addLogAction"></param>
        /// <param name="deleteLogAction"></param>
        /// <param name="listLogAction"></param>
        /// <param name="updateLogAction"></param>
        /// <param name="viewLogAction"></param>
        protected CrudController(
            IAuthorizationService authorizationService,
            ILogger<TInheritingClass> logger,
            IMediator mediator,
            IAuditableCommandFactory<TAddCommand, TAddRequestDto, TAddResponseDto?, TDeleteCommand, TDeleteResponseDto?, TUpdateCommand, TUpdateRequestDto, TUpdateResponseDto?> commandFactory,
            IAuditableQueryFactory<TListQuery, TListRequestDto, TListQueryResponse?, TViewQuery, TViewQueryResponse?> queryFactory,
            Action<ILogger, string, Exception?> addLogAction,
            Action<ILogger, string, Exception?> deleteLogAction,
            Action<ILogger, string, Exception?> listLogAction,
            Action<ILogger, string, Exception?> updateLogAction,
            Action<ILogger, string, Exception?> viewLogAction)
            : base(
                  authorizationService,
                  logger,
                  mediator,
                  queryFactory,
                  listLogAction,
                  viewLogAction)
        {
            CommandFactory = commandFactory ?? throw new ArgumentNullException(nameof(commandFactory));
            _addLogAction = addLogAction ?? throw new ArgumentNullException(nameof(addLogAction));
            _deleteLogAction = deleteLogAction ?? throw new ArgumentNullException(nameof(deleteLogAction));
            _updateLogAction = updateLogAction ?? throw new ArgumentNullException(nameof(updateLogAction));
        }

        /// <summary>
        /// Gets the Command Factory used for creating Commands to pass through the mediator.
        /// </summary>
        protected IAuditableCommandFactory<
            TAddCommand,
            TAddRequestDto,
            TAddResponseDto?,
            TDeleteCommand,
            TDeleteResponseDto?,
            TUpdateCommand,
            TUpdateRequestDto,
            TUpdateResponseDto?> CommandFactory
        {
            get;
        }

        /// <summary>
        /// Operation to Delete an entity.
        /// </summary>
        /// <param name="id">Unique ID of the entity to be deleted.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<IActionResult> Delete(
            int id,
            CancellationToken cancellationToken)
        {
            var deletePolicyName = await GetDeletePolicyAsync().ConfigureAwait(false);

            return await this.GetDeleteActionAsync<TDeleteResponseDto, TDeleteCommand>(
                Logger,
                Mediator,
                AuthorizationService,
                id,
                _deleteLogAction,
                deletePolicyName,
                GetDeleteActionResultAsync,
                GetDeleteCommandAsync,
                cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Operation to create an Entity.
        /// </summary>
        /// <param name="addRequestDto">The Request DTO for the Add Operation.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<IActionResult> Post(
            TAddRequestDto addRequestDto,
            CancellationToken cancellationToken)
        {
            var addPolicyName = await GetAddPolicyAsync().ConfigureAwait(false);

            return await this.GetAddActionAsync<TAddRequestDto, TAddResponseDto, TAddCommand>(
                Logger,
                Mediator,
                AuthorizationService,
                addRequestDto,
                _addLogAction,
                addPolicyName,
                GetAddActionResultAsync,
                GetAddCommandAsync,
                cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Operation to update an entity.
        /// </summary>
        /// <param name="id">Unique Id of the entity.</param>
        /// <param name="updateRequestDto">The Request DTO of the Update operation.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        public async Task<IActionResult> Put(
            long id,
            TUpdateRequestDto updateRequestDto,
            CancellationToken cancellationToken)
        {
            var updatePolicyName = await GetUpdatePolicyAsync().ConfigureAwait(false);

            return await this.GetUpdateActionAsync<TUpdateRequestDto, TUpdateResponseDto, TUpdateCommand>(
                Logger,
                Mediator,
                AuthorizationService,
                id,
                updateRequestDto,
                _updateLogAction,
                updatePolicyName,
                GetUpdateActionResultAsync,
                GetUpdateCommandAsync,
                cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        /// Gets the Action Result for the Add operation.
        /// </summary>
        /// <param name="result">The Response DTO from the CQRS operation.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        protected abstract Task<IActionResult> GetAddActionResultAsync(TAddResponseDto result);

        /// <summary>
        /// Gets the CQRS Command for the Add operation.
        /// </summary>
        /// <param name="addRequestDto">The Add Request DTO.</param>
        /// <param name="claimsPrincipal">The claims principal of the user requesting the add.</param>
        /// <param name="cancellationToken">The cancellation token for the request.</param>
        /// <returns></returns>
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
        protected abstract Task<IActionResult> GetDeleteActionResultAsync(TDeleteResponseDto result);

        /// <summary>
        /// Gets the CQRS Delete Command
        /// </summary>
        /// <param name="id">Unique Id for the entity to delete.</param>
        /// <param name="claimsPrincipal">Claims Principal of the user requesting deletion.</param>
        /// <param name="cancellationToken">Cancellation token for the request.</param>
        /// <returns></returns>
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
        protected abstract Task<IActionResult> GetUpdateActionResultAsync(TUpdateResponseDto result);

        /// <summary>
        /// Gets the CQRS update command for the request.
        /// </summary>
        /// <param name="updateRequestDto">The Update request DTO</param>
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
