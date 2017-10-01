using System;
using System.Collections.Generic;
using System.Text;
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

        public FuncQueryOnlyController(
            IAuthorizationService authorizationService,
            ILogger<FuncQueryOnlyController<TListRequestDto, TListResponse, TViewResponse>> logger,
            IMediator mediator,
            IAuditableQueryFactory<TListRequestDto, TListResponse, TViewResponse> queryFactory,
            Func<TListResponse, IActionResult> getListActionResultFunc,
            Func<TViewResponse, IActionResult> getViewActionResultFunc)
            : base(authorizationService, logger, mediator, queryFactory)
        {
            this._getListActionResultFunc = getListActionResultFunc ??
                                            throw new ArgumentNullException(nameof(getListActionResultFunc));
            this._getViewActionResultFunc = getViewActionResultFunc ??
                                            throw new ArgumentNullException(nameof(getViewActionResultFunc));
        }

        protected override IActionResult GetListActionResult(TListResponse listResponse)
        {
            return this._getListActionResultFunc(listResponse);
        }

        protected override IActionResult GetViewActionResult(TViewResponse viewResponse)
        {
            return this._getViewActionResultFunc(viewResponse);
        }
    }
}
