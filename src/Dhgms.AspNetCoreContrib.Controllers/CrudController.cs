// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using Dhgms.AspNetCoreContrib.Abstractions;
using Dhgms.AspNetCoreContrib.Controllers.Extensions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dhgms.AspNetCoreContrib.Controllers
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
        private readonly IAuditableCommandFactory<TAddCommand, TAddRequestDto, TAddResponseDto, TDeleteCommand, TDeleteResponseDto, TUpdateCommand, TUpdateRequestDto, TUpdateResponseDto> _commandFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="CrudController{TInheritingClass, TListQuery, TListRequestDto, TListQueryResponse, TViewQuery, TViewQueryResponse, TAddCommand, TAddRequestDto, TAddResponseDto, TDeleteCommand, TDeleteResponseDto, TUpdateCommand, TUpdateRequestDto, TUpdateResponseDto}"/> class.
        /// </summary>
        /// <param name="authorizationService">The authorization service for validating access.</param>
        /// <param name="logger">The logger object.</param>
        /// <param name="mediator">The mediatr object to publish CQRS messages to.</param>
        /// <param name="commandFactory">The factory for generating Command messages.</param>
        /// <param name="queryFactory">The factory for generating Query messages.</param>
        protected CrudController(
            IAuthorizationService authorizationService,
            ILogger<TInheritingClass> logger,
            IMediator mediator,
            IAuditableCommandFactory<TAddCommand, TAddRequestDto, TAddResponseDto, TDeleteCommand, TDeleteResponseDto, TUpdateCommand, TUpdateRequestDto, TUpdateResponseDto> commandFactory,
            IAuditableQueryFactory<TListQuery, TListRequestDto, TListQueryResponse, TViewQuery, TViewQueryResponse> queryFactory)
            : base(
                  authorizationService,
                  logger,
                  mediator,
                  queryFactory)
        {
            this._commandFactory = commandFactory ?? throw new ArgumentNullException(nameof(commandFactory));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="addRequestDto"></param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpPost]
        public async Task<IActionResult> AddAsync(
            [FromBody]TAddRequestDto addRequestDto,
            CancellationToken cancellationToken)
        {
            var eventId = await this.GetAddEventIdAsync().ConfigureAwait(false);
            var addPolicyName = await this.GetAddPolicyAsync().ConfigureAwait(false);

            return await this.GetAddActionAsync<TAddRequestDto, TAddResponseDto, TAddCommand>(
                this.Logger,
                this.Mediator,
                this.AuthorizationService,
                addRequestDto,
                eventId,
                addPolicyName,
                this.GetAddActionResultAsync,
                this._commandFactory.GetAddCommandAsync,
                cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(
            [FromRoute]int id,
            CancellationToken cancellationToken)
        {
            var eventId = await this.GetDeleteEventIdAsync().ConfigureAwait(false);
            var deletePolicyName = await this.GetDeletePolicyAsync().ConfigureAwait(false);

            return await this.GetDeleteActionAsync<TDeleteResponseDto, TDeleteCommand>(
                this.Logger,
                this.Mediator,
                this.AuthorizationService,
                id,
                eventId,
                deletePolicyName,
                this.GetDeleteActionResultAsync,
                this._commandFactory.GetDeleteCommandAsync,
                cancellationToken).ConfigureAwait(false);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        /// <param name="updateRequestDto"></param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="Task{TResult}"/> representing the result of the asynchronous operation.</returns>
        [HttpPut]
        public async Task<IActionResult> UpdateAsync(
            [FromRoute]long id,
            [FromBody]TUpdateRequestDto updateRequestDto,
            CancellationToken cancellationToken)
        {
            var eventId = await this.GetUpdateEventIdAsync().ConfigureAwait(false);
            var updatePolicyName = await this.GetUpdatePolicyAsync().ConfigureAwait(false);

            return await this.GetUpdateActionAsync<TUpdateRequestDto, TUpdateResponseDto, TUpdateCommand>(
                this.Logger,
                this.Mediator,
                this.AuthorizationService,
                updateRequestDto,
                eventId,
                updatePolicyName,
                this.GetUpdateActionResultAsync,
                this._commandFactory.GetUpdateCommandAsync,
                cancellationToken).ConfigureAwait(false);
        }

        protected abstract Task<IActionResult> GetAddActionResultAsync(TAddResponseDto result);

        protected abstract Task<EventId> GetAddEventIdAsync();

        protected abstract Task<string> GetAddPolicyAsync();

        protected abstract Task<IActionResult> GetDeleteActionResultAsync(TDeleteResponseDto result);

        protected abstract Task<EventId> GetDeleteEventIdAsync();

        protected abstract Task<string> GetDeletePolicyAsync();

        protected abstract Task<IActionResult> GetUpdateActionResultAsync(TUpdateResponseDto result);

        protected abstract Task<EventId> GetUpdateEventIdAsync();

        protected abstract Task<string> GetUpdatePolicyAsync();
    }
}
