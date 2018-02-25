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
    public class SwaggerClassMetaDataAttribute : Attribute
    {
        public SwaggerClassMetaDataAttribute(Type metadataClass)
        {
            // todo: use a roslyn analyzer to ensure the type passed in is correct.
        }
    }

    public sealed class FakeCrudControllerSwaggerMetaData
    {

    }

    [SwaggerClassMetaData(typeof(FakeCrudControllerSwaggerMetaData))]
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

        protected override async Task<IActionResult> GetListActionResultAsync(int listResponse)
        {
        }

        protected override async Task<IActionResult> GetViewActionResultAsync(int listResponse)
        {
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
