using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Dhgms.AspNetCoreContrib.Controllers
{
    using System;
    using Abstractions;
    using MediatR;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.Extensions.Logging;

    public abstract class CrudController<TInheritingClass, TAddRequestDto, TAddResponseDto, TDeleteResponse, TListRequestDto, TListQueryResponse, TListResponse, TUpdateRequestDto, TUpdateResponseDto, TViewQueryResponse, TViewResponse> : QueryOnlyController<TInheritingClass, TListRequestDto, TListQueryResponse, TListResponse, TViewQueryResponse, TViewResponse>
        where TInheritingClass : CrudController<TInheritingClass, TAddRequestDto, TAddResponseDto, TDeleteResponse, TListRequestDto, TListQueryResponse, TListResponse, TUpdateRequestDto, TUpdateResponseDto, TViewQueryResponse, TViewResponse>
    {
        private readonly IAuditableCommandFactory<TAddRequestDto, TAddResponseDto, TDeleteResponse, TUpdateRequestDto, TUpdateResponseDto> _commandFactory;

        protected CrudController(
            IAuthorizationService authorizationService,
            ILogger<TInheritingClass> logger,
            IMediator mediator,
            IAuditableCommandFactory<TAddRequestDto, TAddResponseDto, TDeleteResponse, TUpdateRequestDto, TUpdateResponseDto> commandFactory,
            IAuditableQueryFactory<TListRequestDto, TListQueryResponse, TViewQueryResponse> queryFactory)
            : base(
                  authorizationService,
                  logger,
                  mediator,
                  queryFactory)
        {
            _commandFactory = commandFactory ?? throw new ArgumentNullException(nameof(commandFactory));
        }

        public async Task<IActionResult> AddAsync(
            TAddRequestDto addRequestDto,
            CancellationToken cancellationToken)
        {
            var eventId = await GetAddEventIdAsync();
            Logger.LogDebug(eventId, "Entered AddAsync");

            var user = HttpContext.User;

            var methodAuthorization = await AuthorizationService.AuthorizeAsync(user, await GetAddPolicyAsync());
            if (!methodAuthorization.Succeeded)
            {
                return Forbid();
            }

            var query = await _commandFactory.GetAddCommandAsync(addRequestDto, user, cancellationToken).ConfigureAwait(false);
            var result = await Mediator.Send(query, cancellationToken).ConfigureAwait(false);

            var viewResult = await GetAddActionResultAsync(result);
            Logger.LogDebug(eventId, "Finished AddAsync");

            return viewResult;
        }


        public async Task<IActionResult> DeleteAsync(
            int id,
            CancellationToken cancellationToken)
        {
            var eventId = await GetDeleteEventIdAsync();
            Logger.LogDebug(eventId, "Entered DeleteAsync");

            var user = HttpContext.User;

            var methodAuthorization = await AuthorizationService.AuthorizeAsync(user, await GetDeletePolicyAsync());
            if (!methodAuthorization.Succeeded)
            {
                return Forbid();
            }

            var query = await _commandFactory.GetDeleteCommandAsync(id, user, cancellationToken).ConfigureAwait(false);
            var result = await Mediator.Send(query, cancellationToken).ConfigureAwait(false);

            var viewResult = await GetDeleteActionResultAsync(result);
            Logger.LogDebug(eventId, "Finished DeleteAsync");

            return viewResult;

        }

        public async Task<IActionResult> UpdateAsync(
            TUpdateRequestDto updateRequestDto,
            CancellationToken cancellationToken)
        {
            var eventId = await GetUpdateEventIdAsync();
            Logger.LogDebug(eventId, "Entered UpdateAsync");

            var user = HttpContext.User;

            var methodAuthorization = await AuthorizationService.AuthorizeAsync(user, await GetUpdatePolicyAsync());
            if (!methodAuthorization.Succeeded)
            {
                return Forbid();
            }

            var query = await _commandFactory.GetUpdateCommandAsync(updateRequestDto, user, cancellationToken).ConfigureAwait(false);
            var result = await Mediator.Send(query, cancellationToken).ConfigureAwait(false);

            var viewResult = await GetUpdateActionResultAsync(result);
            Logger.LogDebug(eventId, "Finished UpdateAsync");

            return viewResult;
        }

        protected abstract Task<IActionResult> GetAddActionResultAsync(TAddResponseDto result);

        protected abstract Task<EventId> GetAddEventIdAsync();

        protected abstract Task<AuthorizationPolicy> GetAddPolicyAsync();


        protected abstract Task<EventId> GetDeleteEventIdAsync();

        protected abstract Task<IActionResult> GetDeleteActionResultAsync(TDeleteResponse result);

        protected abstract Task<AuthorizationPolicy> GetDeletePolicyAsync();

        protected abstract Task<EventId> GetUpdateEventIdAsync();

        protected abstract Task<AuthorizationPolicy> GetUpdatePolicyAsync();

        protected abstract Task<IActionResult> GetUpdateActionResultAsync(TUpdateResponseDto result);
    }
}
