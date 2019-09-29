using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Dhgms.AspNetCoreContrib.Abstractions;
using Dhgms.AspNetCoreContrib.App.Features.Swagger;
using Dhgms.AspNetCoreContrib.Controllers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Dhgms.AspNetCoreContrib.Fakes
{
    [Route("/fakecrud")]
    [SwaggerClassMetaData(typeof(FakeCrudControllerSwaggerMetaData))]
    [ExcludeFromCodeCoverage]
    public sealed class FakeCrudController : CrudController<FakeCrudController, FakeCrudListQuery, FakeCrudListRequest, IList<int>, FakeCrudViewQuery, FakeCrudViewResponse, FakeCrudAddCommand, int, int, FakeCrudDeleteCommand, long, FakeCrudUpdateCommand, int, FakeCrudUpdateResponse>
    {
        public FakeCrudController(
            IAuthorizationService authorizationService,
            ILogger<FakeCrudController> logger,
            IMediator mediator,
            IAuditableCommandFactory<FakeCrudAddCommand, int, int, FakeCrudDeleteCommand, long, FakeCrudUpdateCommand, int, FakeCrudUpdateResponse> commandFactory,
            IAuditableQueryFactory<FakeCrudListQuery, FakeCrudListRequest, IList<int>, FakeCrudViewQuery, FakeCrudViewResponse> queryFactory)
            : base(
                authorizationService,
                logger,
                mediator,
                commandFactory,
                queryFactory)
        {
        }

        protected override async Task<EventId> GetListEventIdAsync()
        {
            return await Task.Run(() => new EventId(1)).ConfigureAwait(false);
        }

        protected override async Task<EventId> GetViewEventIdAsync()
        {
            return await Task.Run(() => new EventId(2)).ConfigureAwait(false);
        }

        protected override async Task<string> GetListPolicyAsync()
        {
            return await Task.FromResult("listPolicyName").ConfigureAwait(false);
        }

        protected override async Task<string> GetViewPolicyAsync()
        {
            return await Task.FromResult("viewPolicyName").ConfigureAwait(false);
        }

        protected override async Task<IActionResult> GetListActionResultAsync(IList<int> listResponse)
        {
            return await Task.FromResult(this.Ok(listResponse)).ConfigureAwait(false);
        }

        protected override async Task<IActionResult> GetViewActionResultAsync(FakeCrudViewResponse viewResult)
        {
            return await Task.FromResult(this.Ok(viewResult)).ConfigureAwait(false);
        }

        protected override async Task<IActionResult> GetAddActionResultAsync(int addResult)
        {
            return await Task.FromResult(this.Ok(addResult)).ConfigureAwait(false);
        }

        protected override async Task<EventId> GetAddEventIdAsync()
        {
            return await Task.FromResult(new EventId(3)).ConfigureAwait(false);
        }

        protected override async Task<string> GetAddPolicyAsync()
        {
            return await Task.FromResult("addPolicyName").ConfigureAwait(false);
        }

        protected override async Task<EventId> GetDeleteEventIdAsync()
        {
            return await Task.FromResult(new EventId(4)).ConfigureAwait(false);
        }

        protected override async Task<IActionResult> GetDeleteActionResultAsync(long result)
        {
            return await Task.FromResult(this.Ok(result)).ConfigureAwait(false);
        }

        protected override async Task<string> GetDeletePolicyAsync()
        {
            return await Task.FromResult("deletePolicyName").ConfigureAwait(false);
        }

        protected override async Task<EventId> GetUpdateEventIdAsync()
        {
            return await Task.Run(() => new EventId(5)).ConfigureAwait(false);
        }

        protected override async Task<string> GetUpdatePolicyAsync()
        {
            return await Task.FromResult("updatePolicyName").ConfigureAwait(false);
        }

        protected override async Task<IActionResult> GetUpdateActionResultAsync(FakeCrudUpdateResponse result)
        {
            return await Task.FromResult(this.Ok(result)).ConfigureAwait(false);
        }
    }
}
