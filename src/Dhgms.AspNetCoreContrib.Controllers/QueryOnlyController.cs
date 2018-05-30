using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Dhgms.AspNetCoreContrib.Controllers
{
    using System.Threading;
    using System.Threading.Tasks;
    using Abstractions;
    using MediatR;
    using System;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    [SuppressMessage("csharpsquid", "S2436: Classes and methods should not have too many generic parameters", Justification = "By design, need large number of generics to make this powerful enough for ru-use in pattern")]
    public abstract class QueryOnlyController<TInheritingClass, TListQuery, TListRequestDto, TListQueryResponse, TViewQuery, TViewQueryResponse>
        : Controller
        where TInheritingClass : QueryOnlyController<TInheritingClass, TListQuery, TListRequestDto, TListQueryResponse, TViewQuery, TViewQueryResponse>
        where TListRequestDto : class, new()
        where TListQuery : IAuditableRequest<TListRequestDto, TListQueryResponse>
        where TViewQuery : IAuditableRequest<long, TViewQueryResponse>
    {
        private readonly IAuditableQueryFactory<TListQuery, TListRequestDto, TListQueryResponse, TViewQuery, TViewQueryResponse> _queryFactory;

        protected QueryOnlyController(
            IAuthorizationService authorizationService,
            ILogger<TInheritingClass> logger,
            IMediator mediator,
            IAuditableQueryFactory<TListQuery, TListRequestDto, TListQueryResponse, TViewQuery, TViewQueryResponse> queryFactory)
        {
            AuthorizationService = authorizationService ??
                                         throw new ArgumentNullException(nameof(authorizationService));
            Logger = logger ??
                                         throw new ArgumentNullException(nameof(logger));

            Mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

            _queryFactory = queryFactory ??
                                            throw new ArgumentNullException(nameof(queryFactory));
        }

        protected IAuthorizationService AuthorizationService { get; }

        protected ILogger<TInheritingClass> Logger { get; }

        protected IMediator Mediator { get; }

        [HttpGet]
        public async Task<IActionResult> IndexAsync(
            [FromRoute]long? id,
            //[FromQuery]TListRequestDto requestDto,
            CancellationToken cancellationToken)
        {
            if (!Request.IsHttps)
            {
                return BadRequest();
            }

            if (!id.HasValue)
            {
                return await ListAsync(cancellationToken).ConfigureAwait(false);
            }

            return await ViewAsync(id.Value, cancellationToken).ConfigureAwait(false);
        }

        private async Task<IActionResult> ListAsync(CancellationToken cancellationToken)
        {
            // removes need for ConfigureAwait(false)
            await new SynchronizationContextRemover();
            var eventId = await GetListEventIdAsync();
            Logger.LogDebug(eventId, "Entered ListAsync");

            if (!Request.IsHttps)
            {
                return BadRequest();
            }

            var user = HttpContext.User;

            var listPolicy = await GetListPolicyAsync();
            var methodAuthorization = await AuthorizationService.AuthorizeAsync(user, listPolicy);
            if (!methodAuthorization.Succeeded)
            {
                // not found rather than forbidden
                return NotFound();
            }

            var requestDto = new TListRequestDto();
            /*
            var queryCollection = this.Request.Query;
            var bindingSource = BindingSource.Query;
            IValueProvider provider = new QueryStringValueProvider(bindingSource, queryCollection, CultureInfo.InvariantCulture);
            await this.TryUpdateModelAsync(requestDto, string.Empty, provider);
            */

            var query = await _queryFactory.GetListQueryAsync(requestDto, user, cancellationToken);
            var result = await Mediator.Send(query, cancellationToken);

            var viewResult = await GetListActionResultAsync(result);
            Logger.LogDebug(eventId, "Finished ListAsync");

            return viewResult;
        }

        private async Task<IActionResult> ViewAsync(
            long id,
            CancellationToken cancellationToken)
        {
            await new SynchronizationContextRemover();

            var eventId = await GetViewEventIdAsync();
            Logger.LogDebug(eventId, "Entered ViewAsync");

            if (!Request.IsHttps)
            {
                return BadRequest();
            }

            var user = HttpContext.User;

            if (id < 1)
            {
                return NotFound();
            }

            var methodAuthorization = await AuthorizationService.AuthorizeAsync(user, await GetViewPolicyAsync());
            if (!methodAuthorization.Succeeded)
            {
                // not found rather than forbidden
                return NotFound();
            }

            var query = await _queryFactory.GetViewQueryAsync(id, user, cancellationToken);
            var result = await Mediator.Send(query, cancellationToken);

            if (result == null)
            {
                return NotFound();
            }

            var resourceAuthorization = await AuthorizationService.AuthorizeAsync(user, result, await GetViewPolicyAsync());
            if (!resourceAuthorization.Succeeded)
            {
                // not found rather than forbidden
                return NotFound();
            }

            var viewResult = await GetViewActionResultAsync(result);
            Logger.LogDebug(eventId, "Finished ViewAsync");

            return viewResult;
        }

        protected abstract Task<IActionResult> GetListActionResultAsync(TListQueryResponse listResponse);

        protected abstract Task<EventId> GetListEventIdAsync();

        protected abstract Task<string> GetListPolicyAsync();

        protected abstract Task<EventId> GetViewEventIdAsync();

        protected abstract Task<string> GetViewPolicyAsync();

        protected abstract Task<IActionResult> GetViewActionResultAsync(TViewQueryResponse listResponse);
    }
}
