namespace Dhgms.AspNetCoreContrib.Controllers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Threading;
    using System.Threading.Tasks;
    using Abstractions;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [SuppressMessage("csharpsquid", "S2436: Classes and methods should not have too many generic parameters", Justification = "By design, need large number of generics to make this powerful enough for re-use in pattern")]
    public abstract class CrudController<TInheritingClass, TListQuery, TListRequestDto, TListQueryResponse, TViewQuery, TViewQueryResponse, TAddCommand, TAddRequestDto, TAddResponseDto, TDeleteCommand, TDeleteResponseDto, TUpdateCommand, TUpdateRequestDto, TUpdateResponseDto>
        : QueryOnlyController<TInheritingClass, TListQuery, TListRequestDto, TListQueryResponse, TViewQuery, TViewQueryResponse>
        where TInheritingClass : CrudController<TInheritingClass, TListQuery, TListRequestDto, TListQueryResponse, TViewQuery, TViewQueryResponse, TAddCommand, TAddRequestDto, TAddResponseDto, TDeleteCommand, TDeleteResponseDto, TUpdateCommand, TUpdateRequestDto, TUpdateResponseDto>
        where TAddCommand : IAuditableRequest<TAddRequestDto, TAddResponseDto>
        where TDeleteCommand : IAuditableRequest<long, TDeleteResponseDto>
        where TListQuery : IAuditableRequest<TListRequestDto, TListQueryResponse>
        where TListRequestDto : class, new()
        where TViewQuery : IAuditableRequest<long, TViewQueryResponse>
        where TUpdateCommand : IAuditableRequest<TUpdateRequestDto, TUpdateResponseDto>
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
            this.Logger.LogDebug(
                eventId,
                "Entered AddAsync");

            if (!this.Request.IsHttps)
            {
                return this.BadRequest();
            }

            var user = this.HttpContext.User;

            var addPolicyName = await this.GetAddPolicyAsync().ConfigureAwait(false);

            // someone needs to have permission to do a general add
            // but there is a chance the request dto also has details such as a parent id
            // so while someone may have a generic add permission
            // they may not be able to add to a specific parent item
            var methodAuthorization = await this.AuthorizationService.AuthorizeAsync(
                user,
                addRequestDto,
                addPolicyName).ConfigureAwait(false);

            if (!methodAuthorization.Succeeded)
            {
                return this.Forbid();
            }

            var query = await this._commandFactory.GetAddCommandAsync(
                addRequestDto,
                user,
                cancellationToken).ConfigureAwait(false);

            var result = await this.Mediator.Send(
                query,
                cancellationToken).ConfigureAwait(false);

            var viewResult = await this.GetAddActionResultAsync(result).ConfigureAwait(false);
            this.Logger.LogDebug(
                eventId,
                "Finished AddAsync");

            return viewResult;
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(
            [FromRoute]int id,
            CancellationToken cancellationToken)
        {
            var eventId = await this.GetDeleteEventIdAsync().ConfigureAwait(false);
            this.Logger.LogDebug(
                eventId,
                "Entered DeleteAsync");

            if (!this.Request.IsHttps)
            {
                return this.BadRequest();
            }

            var user = this.HttpContext.User;

            var deletePolicyName = await this.GetDeletePolicyAsync().ConfigureAwait(false);

            var methodAuthorization = await this.AuthorizationService.AuthorizeAsync(
                user,
                id,
                deletePolicyName).ConfigureAwait(false);
            if (!methodAuthorization.Succeeded)
            {
                return this.Forbid();
            }

            var query = await this._commandFactory.GetDeleteCommandAsync(
                id,
                user,
                cancellationToken).ConfigureAwait(false);

            var result = await this.Mediator.Send(
                query,
                cancellationToken).ConfigureAwait(false);

            var viewResult = await this.GetDeleteActionResultAsync(result).ConfigureAwait(false);
            this.Logger.LogDebug(
                eventId,
                "Finished DeleteAsync");

            return viewResult;
        }

        [HttpPut]
        public async Task<IActionResult> UpdateAsync(
            [FromRoute]long id,
            [FromBody]TUpdateRequestDto updateRequestDto,
            CancellationToken cancellationToken)
        {
            var eventId = await this.GetUpdateEventIdAsync().ConfigureAwait(false);
            this.Logger.LogDebug(eventId, "Entered UpdateAsync");

            if (!this.Request.IsHttps)
            {
                return this.BadRequest();
            }

            var user = this.HttpContext.User;

            var updatePolicyName = await this.GetUpdatePolicyAsync().ConfigureAwait(false);

            var methodAuthorization = await this.AuthorizationService.AuthorizeAsync(
                user,
                updateRequestDto,
                updatePolicyName).ConfigureAwait(false);
            if (!methodAuthorization.Succeeded)
            {
                return this.Forbid();
            }

            var query = await this._commandFactory.GetUpdateCommandAsync(
                updateRequestDto,
                user,
                cancellationToken).ConfigureAwait(false);

            var result = await this.Mediator.Send(
                query,
                cancellationToken).ConfigureAwait(false);

            var viewResult = await this.GetUpdateActionResultAsync(result).ConfigureAwait(false);
            this.Logger.LogDebug(
                eventId,
                "Finished UpdateAsync");

            return viewResult;
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
