using System.Threading;
using System.Threading.Tasks;
using Dhgms.AspNetCoreContrib.Abstractions;
using MediatR;

namespace Dhgms.AspNetCoreContrib.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;

    public abstract class QueryOnlyController<TInheritingClass, TListRequestDto, TListResponse, TViewResponse>
        : Microsoft.AspNetCore.Mvc.Controller
        where TInheritingClass : QueryOnlyController<TInheritingClass, TListRequestDto, TListResponse, TViewResponse>
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly ILogger<TInheritingClass> _logger;

        private readonly IMediator _mediator;

        private readonly IAuditableQueryFactory<TListRequestDto, TListResponse, TViewResponse> _queryFactory;

        protected QueryOnlyController(
            Microsoft.AspNetCore.Authorization.IAuthorizationService authorizationService,
            Microsoft.Extensions.Logging.ILogger<TInheritingClass> logger,
            IMediator mediator,
            IAuditableQueryFactory<TListRequestDto, TListResponse, TViewResponse> queryFactory)
        {
            this._authorizationService = authorizationService ??
                                         throw new ArgumentNullException(nameof(authorizationService));
            this._logger = logger ??
                                         throw new ArgumentNullException(nameof(logger));

            this._mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

            this._queryFactory = queryFactory ??
                                            throw new ArgumentNullException(nameof(queryFactory));
        }

        [Microsoft​.AspNetCore​.Mvc.HttpGet]
        public async System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> ListAsync(
            [FromQuery]TListRequestDto requestDto,
            CancellationToken cancellationToken)
        {
            var user = this.HttpContext.User;
            var query = await this._queryFactory.GetListQueryAsync(requestDto, user, cancellationToken).ConfigureAwait(false);
            var result = await this._mediator.Send(query, cancellationToken).ConfigureAwait(false);
            return GetListActionResult(result);
        }

        [Microsoft​.AspNetCore​.Mvc.HttpGet]
        public async System.Threading.Tasks.Task<IActionResult> ViewAsync(
            [FromQuery]long id,
            CancellationToken cancellationToken)
        {
            var user = this.HttpContext.User;
            var query = await this._queryFactory.GetViewQueryAsync(id, user, cancellationToken).ConfigureAwait(false);
            var result = await this._mediator.Send(query, cancellationToken).ConfigureAwait(false);
            return GetViewActionResult(result);
        }

        protected abstract IActionResult GetListActionResult(TListResponse listResponse);

        protected abstract IActionResult GetViewActionResult(TViewResponse listResponse);
    }
}
