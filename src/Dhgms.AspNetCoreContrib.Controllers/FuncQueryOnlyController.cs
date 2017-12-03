using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dhgms.AspNetCoreContrib.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dhgms.AspNetCoreContrib.Controllers
{
    public sealed class FuncQueryOnlyController<TListRequestDto, TListResponse, TViewResponse>
        : QueryOnlyController<FuncQueryOnlyController<TListRequestDto, TListResponse, TViewResponse>, TListRequestDto, TListResponse, TViewResponse>
    {
        private readonly Func<TListResponse, IActionResult> _getListActionResultFunc;
        private readonly Func<TViewResponse, IActionResult> _getViewActionResultFunc;
        private readonly AuthorizationPolicy _viewAuthorizationPolicy;

        public FuncQueryOnlyController(
            IAuthorizationService authorizationService,
            ILogger<FuncQueryOnlyController<TListRequestDto, TListResponse, TViewResponse>> logger,
            IMediator mediator,
            IAuditableQueryFactory<TListRequestDto, TListResponse, TViewResponse> queryFactory,
            Func<TListResponse, IActionResult> getListActionResultFunc,
            AuthorizationPolicy viewAuthorizationPolicy,
            Func<TViewResponse, IActionResult> getViewActionResultFunc)
            : base(authorizationService, logger, mediator, queryFactory)
        {
            this._getListActionResultFunc = getListActionResultFunc ??
                                            throw new ArgumentNullException(nameof(getListActionResultFunc));
            this._viewAuthorizationPolicy = viewAuthorizationPolicy ??
                                            throw new ArgumentNullException(nameof(viewAuthorizationPolicy));
            this._getViewActionResultFunc = getViewActionResultFunc ??
                                            throw new ArgumentNullException(nameof(getViewActionResultFunc));

        }

        protected override async Task<AuthorizationPolicy> GetViewPolicy()
            => await Task.FromResult(this._viewAuthorizationPolicy);

        protected override IActionResult GetListActionResult(TListResponse listResponse)
            => this._getListActionResultFunc(listResponse);

        protected override IActionResult GetViewActionResult(TViewResponse viewResponse)
            => this._getViewActionResultFunc(viewResponse);
    }
}
