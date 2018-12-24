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
            this.AuthorizationService = authorizationService ??
                                         throw new ArgumentNullException(nameof(authorizationService));
            this.Logger = logger ??
                                         throw new ArgumentNullException(nameof(logger));

            this.Mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

            this._queryFactory = queryFactory ??
                                            throw new ArgumentNullException(nameof(queryFactory));
        }

        protected IAuthorizationService AuthorizationService { get; }

        protected ILogger<TInheritingClass> Logger { get; }

        protected IMediator Mediator { get; }

        [HttpGet]
        public async Task<IActionResult> IndexAsync(
            [FromRoute]long? id,
            // [FromQuery]TListRequestDto requestDto,
            CancellationToken cancellationToken)
        {
            if (!this.Request.IsHttps)
            {
                return this.BadRequest();
            }

            if (!id.HasValue)
            {
                return await this.ListAsync(cancellationToken).ConfigureAwait(false);
            }

            return await this.ViewAsync(id.Value, cancellationToken).ConfigureAwait(false);
        }

        private async Task<IActionResult> ListAsync(CancellationToken cancellationToken)
        {
            // removes need for ConfigureAwait(false)
            var eventId = await this.GetListEventIdAsync().ConfigureAwait(false);
            this.Logger.LogDebug(eventId, "Entered ListAsync");

            var user = this.HttpContext.User;

            var listPolicy = await this.GetListPolicyAsync().ConfigureAwait(false);
            var methodAuthorization = await this.AuthorizationService.AuthorizeAsync(user, listPolicy).ConfigureAwait(false);
            if (!methodAuthorization.Succeeded)
            {
                // not found rather than forbidden
                return this.NotFound();
            }

            var requestDto = new TListRequestDto();
            /*
            var queryCollection = this.Request.Query;
            var bindingSource = BindingSource.Query;
            IValueProvider provider = new QueryStringValueProvider(bindingSource, queryCollection, CultureInfo.InvariantCulture);
            await this.TryUpdateModelAsync(requestDto, string.Empty, provider);
            */

            var query = await this._queryFactory.GetListQueryAsync(requestDto, user, cancellationToken).ConfigureAwait(false);
            var result = await this.Mediator.Send(query, cancellationToken).ConfigureAwait(false);

            var viewResult = await this.GetListActionResultAsync(result).ConfigureAwait(false);
            this.Logger.LogDebug(eventId, "Finished ListAsync");

            return viewResult;
        }

        private async Task<IActionResult> ViewAsync(
            long id,
            CancellationToken cancellationToken)
        {
            var eventId = await this.GetViewEventIdAsync().ConfigureAwait(false);
            this.Logger.LogDebug(eventId, "Entered ViewAsync");

            var user = this.HttpContext.User;

            if (id < 1)
            {
                return this.NotFound();
            }

            var methodAuthorization = await this.AuthorizationService.AuthorizeAsync(user, await this.GetViewPolicyAsync().ConfigureAwait(false)).ConfigureAwait(false);
            if (!methodAuthorization.Succeeded)
            {
                // not found rather than forbidden
                return this.NotFound();
            }

            var query = await this._queryFactory.GetViewQueryAsync(id, user, cancellationToken).ConfigureAwait(false);
            var result = await this.Mediator.Send(query, cancellationToken).ConfigureAwait(false);

            if (result == null)
            {
                return this.NotFound();
            }

            var resourceAuthorization = await this.AuthorizationService.AuthorizeAsync(user, result, await this.GetViewPolicyAsync().ConfigureAwait(false)).ConfigureAwait(false);
            if (!resourceAuthorization.Succeeded)
            {
                // not found rather than forbidden
                return this.NotFound();
            }

            var viewResult = await this.GetViewActionResultAsync(result).ConfigureAwait(false);
            this.Logger.LogDebug(eventId, "Finished ViewAsync");

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
