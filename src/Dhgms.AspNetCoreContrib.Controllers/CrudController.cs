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
            this._commandFactory = commandFactory ?? throw new ArgumentNullException(nameof(commandFactory));
        }

        public abstract Task<IActionResult> AddAsync(TAddRequestDto addRequestDto);

        public abstract Task<IActionResult> DeleteAsync(int id);

        public abstract Task<IActionResult> UpdateAsync(TUpdateRequestDto updateRequestDto);
    }
}
