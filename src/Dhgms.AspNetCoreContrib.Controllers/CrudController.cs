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

        protected abstract Task<EventId> GetDeleteEventIdAsync();

        protected abstract Task<IActionResult> GetDeleteActionResultAsync(TDeleteResponseDto result);

        protected abstract Task<string> GetDeletePolicyAsync();

        protected abstract Task<EventId> GetUpdateEventIdAsync();

        protected abstract Task<string> GetUpdatePolicyAsync();

        protected abstract Task<IActionResult> GetUpdateActionResultAsync(TUpdateResponseDto result);
    }
}
