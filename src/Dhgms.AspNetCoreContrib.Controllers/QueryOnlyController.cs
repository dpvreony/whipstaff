using Dhgms.AspNetCoreContrib.Controllers.Extensions;

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
        where TListQueryResponse : class
        where TViewQuery : IAuditableRequest<long, TViewQueryResponse>
        where TViewQueryResponse : class
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
            var eventId = await this.GetListEventIdAsync().ConfigureAwait(false);
            var listPolicyName = await this.GetListPolicyAsync().ConfigureAwait(false);
            var requestDto = new TListRequestDto();

            return await this.GetListActionAsync<TListRequestDto, TListQueryResponse, TListQuery>(
                this.Logger,
                this.Mediator,
                this.AuthorizationService,
                requestDto,
                eventId,
                listPolicyName,
                this.GetListActionResultAsync,
                this._queryFactory.GetListQueryAsync,
                cancellationToken).ConfigureAwait(false);
        }

        private async Task<IActionResult> ViewAsync(
            long id,
            CancellationToken cancellationToken)
        {
            var eventId = await this.GetViewEventIdAsync().ConfigureAwait(false);
            var viewPolicyName = await this.GetViewPolicyAsync().ConfigureAwait(false);

            return await this.GetViewActionAsync<long, TViewQueryResponse, TViewQuery>(
                this.Logger,
                this.Mediator,
                this.AuthorizationService,
                id,
                eventId,
                viewPolicyName,
                this.GetViewActionResultAsync,
                this._queryFactory.GetViewQueryAsync,
                cancellationToken).ConfigureAwait(false);
        }

        protected abstract Task<IActionResult> GetListActionResultAsync(TListQueryResponse listResponse);

        protected abstract Task<EventId> GetListEventIdAsync();

        protected abstract Task<string> GetListPolicyAsync();

        protected abstract Task<EventId> GetViewEventIdAsync();

        protected abstract Task<string> GetViewPolicyAsync();

        protected abstract Task<IActionResult> GetViewActionResultAsync(TViewQueryResponse listResponse);
    }
}
