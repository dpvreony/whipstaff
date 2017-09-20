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

    public class ReadOnlyMvcController<TInheritingClass, TListRequestDto, TEntity> : Microsoft.AspNetCore.Mvc.Controller
        where TInheritingClass : ReadOnlyMvcController<TInheritingClass, TListRequestDto, TEntity>
    {
        private readonly IAuthorizationService _authorizationService;
        private readonly ILogger<TInheritingClass> _logger;
        private readonly IMediator _mediator;
        private readonly IAuditableQueryFactory<TListRequestDto, TEntity> _queryFactory;

        public ReadOnlyMvcController(
            Microsoft.AspNetCore.Authorization.IAuthorizationService authorizationService,
            Microsoft.Extensions.Logging.ILogger<TInheritingClass> logger,
            IMediator mediator,
            IAuditableQueryFactory<TListRequestDto, TEntity> queryFactory)
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
            var result = await _mediator.Send(query, cancellationToken).ConfigureAwait(false);
            return this.View("List", result);
        }

        [Microsoft​.AspNetCore​.Mvc.HttpGet]
        public async System.Threading.Tasks.Task<Microsoft.AspNetCore.Mvc.IActionResult> ViewAsync(
            [FromQuery]TListRequestDto requestDto,
            CancellationToken cancellationToken)
        {
            var user = this.HttpContext.User;
            var query = await this._queryFactory.GetViewQueryAsync(requestDto, user, cancellationToken).ConfigureAwait(false);
            var result = await _mediator.Send(query, cancellationToken).ConfigureAwait(false);
            return this.View("View", result);
        }
    }
}
