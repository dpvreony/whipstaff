using System.Diagnostics.CodeAnalysis;
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

    [SuppressMessage("csharpsquid", "S2436: Classes and methods should not have too many generic parameters", Justification = "By design, need large number of generics to make this powerful enough for ru-use in pattern")]
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
            _commandFactory = commandFactory ?? throw new ArgumentNullException(nameof(commandFactory));
        }

        [HttpPost]
        public async Task<IActionResult> AddAsync(
            [FromBody]TAddRequestDto addRequestDto,
            CancellationToken cancellationToken)
        {
            await new SynchronizationContextRemover();

            var eventId = await GetAddEventIdAsync();
            Logger.LogDebug(
                eventId,
                "Entered AddAsync");

            if (!Request.IsHttps)
            {
                return BadRequest();
            }

            var user = HttpContext.User;

            var addPolicyName = await GetAddPolicyAsync();

            // someone needs to have permission to do a general add
            // but there is a chance the request dto also has details such as a parent id
            // so while someone may have a generic add permission
            // they may not be able to add to a specific parent item
            var methodAuthorization = await AuthorizationService.AuthorizeAsync(
                user,
                addRequestDto,
                addPolicyName);

            if (!methodAuthorization.Succeeded)
            {
                return Forbid();
            }

            var query = await _commandFactory.GetAddCommandAsync(
                addRequestDto,
                user,
                cancellationToken);

            var result = await Mediator.Send(
                query,
                cancellationToken);

            var viewResult = await GetAddActionResultAsync(result);
            Logger.LogDebug(
                eventId,
                "Finished AddAsync");

            return viewResult;
        }


        [HttpDelete]
        public async Task<IActionResult> DeleteAsync(
            [FromRoute]int id,
            CancellationToken cancellationToken)
        {
            await new SynchronizationContextRemover();

            var eventId = await GetDeleteEventIdAsync();
            Logger.LogDebug(
                eventId,
                "Entered DeleteAsync");

            if (!Request.IsHttps)
            {
                return BadRequest();
            }

            var user = HttpContext.User;

            var deletePolicyName = await GetDeletePolicyAsync();

            var methodAuthorization = await AuthorizationService.AuthorizeAsync(
                user,
                id,
                deletePolicyName);
            if (!methodAuthorization.Succeeded)
            {
                return Forbid();
            }

            var query = await _commandFactory.GetDeleteCommandAsync(
                id,
                user,
                cancellationToken);

            var result = await Mediator.Send(
                query,
                cancellationToken);

            var viewResult = await GetDeleteActionResultAsync(result);
            Logger.LogDebug(
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
            await new SynchronizationContextRemover();

            var eventId = await GetUpdateEventIdAsync();
            Logger.LogDebug(eventId, "Entered UpdateAsync");

            if (!Request.IsHttps)
            {
                return BadRequest();
            }

            var user = HttpContext.User;

            var updatePolicyName = await GetUpdatePolicyAsync();

            var methodAuthorization = await AuthorizationService.AuthorizeAsync(
                user,
                updateRequestDto,
                updatePolicyName);
            if (!methodAuthorization.Succeeded)
            {
                return Forbid();
            }

            var query = await _commandFactory.GetUpdateCommandAsync(
                updateRequestDto,
                user,
                cancellationToken);

            var result = await Mediator.Send(
                query,
                cancellationToken);

            var viewResult = await GetUpdateActionResultAsync(result);
            Logger.LogDebug(
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
