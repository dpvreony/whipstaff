using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dhgms.AspNetCoreContrib.Abstractions;
using Dhgms.AspNetCoreContrib.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Dhgms.AspNetCoreContrib.UnitTests.Controllers
{
    public sealed class FakeCrudController : CrudController<FakeCrudController, int, int, int, int, int, int, int, int, int, int>
    {
        public FakeCrudController(
            IAuthorizationService authorizationService,
            ILogger<FakeCrudController> logger,
            IMediator mediator,
            IAuditableCommandFactory<int, int, int, int, int> commandFactory,
            IAuditableQueryFactory<int, int, int> queryFactory)
            : base(
                authorizationService,
                logger,
                mediator,
                commandFactory,
                queryFactory)
        {
        }

        public override async Task<IActionResult> ListAsync(int requestDto, CancellationToken cancellationToken)
        {
            return await OnListAsync(requestDto, cancellationToken);
        }

        public override async Task<IActionResult> ViewAsync(long id, CancellationToken cancellationToken)
        {
            return await OnViewAsync(id, cancellationToken);
        }

        protected override async Task<EventId> GetOnListEventIdAsync()
        {
        }

        protected override async Task<EventId> GetOnViewEventIdAsync()
        {
        }

        protected override async Task<AuthorizationPolicy> GetListPolicyAsync()
        {
        }

        protected override async Task<AuthorizationPolicy> GetViewPolicyAsync()
        {
        }

        protected override IActionResult GetListActionResult(int listResponse)
        {
        }

        protected override IActionResult GetViewActionResult(int listResponse)
        {
        }

        public override async Task<IActionResult> AddAsync(int addRequestDto)
        {
            return await OnAddAsync(addRequestDto);
        }

        public override async Task<IActionResult> DeleteAsync(int id)
        {
            return await OnDeleteAsync(id);
        }

        public override async Task<IActionResult> UpdateAsync(int updateRequestDto)
        {
            return await OnUpdateAsync(updateRequestDto);
        }
    }

    public static class CrudControllerTests
    {
        public sealed class ConstructorMethod
        {
            public async Task ThrowsArgumentNullException()
            {
                Assert.Throws<ArgumentNullException>(() => new FakeCrudController());
            }
        }

        public sealed class AddAsyncMethod
        {
            public async Task ThrowsArgumentNullException()
            {

            }
        }

        public sealed class DeleteAsyncMethod
        {
            public async Task ThrowsArgumentNullException()
            {

            }
        }

        public sealed class UpdateAsyncMethod
        {
            public async Task ThrowsArgumentNullException()
            {

            }
        }
    }
}
