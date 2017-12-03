using Dhgms.AspNetCoreContrib.Controllers;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dhgms.AspNetCoreContrib.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace Dhgms.AspNetCoreContrib.UnitTests.Controllers
{
    [ExcludeFromCodeCoverage]
    public static class FuncQueryOnlyControllerTests
    {
        private static readonly Mock<IAuthorizationService> MockAuthorizationService
            = new Mock<IAuthorizationService>(MockBehavior.Strict);

        private static readonly Mock<ILogger<FuncQueryOnlyController<int, int, int>>> MockLogger
            = new Mock<ILogger<FuncQueryOnlyController<int, int, int>>>(MockBehavior.Strict);

        private static readonly Mock<IMediator> MockMediator
            = new Mock<IMediator>(MockBehavior.Strict);

        private static readonly Mock<IAuditableQueryFactory<int, int, int>> MockQueryFactory
            = new Mock<IAuditableQueryFactory<int, int, int>>(MockBehavior.Strict);

        private static readonly Mock<Func<int, IActionResult>> MockGetListActionResultFunc
            = new Mock<Func<int, IActionResult>>(MockBehavior.Strict);

        private static readonly Mock<Func<int, IActionResult>> MockGetViewActionResultFunc
            = new Mock<Func<int, IActionResult>>(MockBehavior.Strict);

        public sealed class ConstructorMethod
        {
            public static IEnumerable<object[]> ThrowsArgumentNullExceptionTestData => new object[][]
            {
                // authorization service null
                new object[]
                {
                    default(IAuthorizationService),
                    MockLogger.Object,
                    MockMediator.Object,
                    MockQueryFactory.Object,
                    MockGetListActionResultFunc.Object,
                    MockGetViewActionResultFunc.Object
                },

                // logger null
                new object[]
                {
                    MockAuthorizationService.Object,
                    default(ILogger<FuncQueryOnlyController<int, int, int>>),
                    MockMediator.Object,
                    MockQueryFactory.Object,
                    MockGetListActionResultFunc.Object,
                    MockGetViewActionResultFunc.Object
                },

                // mediator null
                new object[]
                {
                    MockAuthorizationService.Object,
                    MockLogger.Object,
                    default(IMediator),
                    MockQueryFactory.Object,
                    MockGetListActionResultFunc.Object,
                    MockGetViewActionResultFunc.Object
                },

                // query factory null
                new object[]
                {
                    MockAuthorizationService.Object,
                    MockLogger.Object,
                    MockMediator.Object,
                    default(IAuditableQueryFactory<int, int, int>),
                    MockGetListActionResultFunc.Object,
                    MockGetViewActionResultFunc.Object
                },

                // list action func null
                new object[]
                {
                    MockAuthorizationService.Object,
                    MockLogger.Object,
                    MockMediator.Object,
                    MockQueryFactory.Object,
                    default(Func<int, IActionResult>),
                    MockGetViewActionResultFunc.Object
                },

                // view action func null
                new object[]
                {
                    MockAuthorizationService.Object,
                    MockLogger.Object,
                    MockMediator.Object,
                    MockQueryFactory.Object,
                    MockGetListActionResultFunc.Object,
                    default(Func<int, IActionResult>)
                },
            };

            [Fact]
            public void ReturnsInstance()
            {
                var instance = new FuncQueryOnlyController<int, int, int>(
                    MockAuthorizationService.Object,
                    MockLogger.Object,
                    MockMediator.Object,
                    MockQueryFactory.Object,
                    MockGetListActionResultFunc.Object,
                    MockGetViewActionResultFunc.Object);

                Assert.NotNull(instance);
            }

            [Theory]
            [MemberData(nameof(ThrowsArgumentNullExceptionTestData))]
            public void ThrowsArgumentNullException(
                IAuthorizationService authorizationService,
                ILogger<FuncQueryOnlyController<int, int, int>> logger,
                IMediator mediator,
                IAuditableQueryFactory<int, int, int> queryFactory,
                Func<int, IActionResult> getListActionResultFunc,
                Func<int, IActionResult> getViewActionResultFunc)
            {
                Assert.Throws<ArgumentNullException>(() => new FuncQueryOnlyController<int, int, int>(
                    authorizationService,
                    logger,
                    mediator,
                    queryFactory,
                    getListActionResultFunc,
                    getViewActionResultFunc));
            }
        }

        public sealed class ListAsyncMethod
        {
            [Fact]
            public async Task ReturnsInstance()
            {
                var instance = new FuncQueryOnlyController<int, int, int>(
                    MockAuthorizationService.Object,
                    MockLogger.Object,
                    MockMediator.Object,
                    MockQueryFactory.Object,
                    MockGetListActionResultFunc.Object,
                    MockGetViewActionResultFunc.Object);

                Assert.NotNull(instance);

                var httpContext = new DefaultHttpContext {User = new ClaimsPrincipal()};
                var controllerContext = new ControllerContext {HttpContext = httpContext};
                instance.ControllerContext = controllerContext;

                var result = await instance.ListAsync(1, CancellationToken.None);
                Assert.NotNull(result);
            }
        }

        public sealed class ViewMethod
        {
            [Fact]
            public async Task ReturnsInstance()
            {
                var instance = new FuncQueryOnlyController<int, int, int>(
                    MockAuthorizationService.Object,
                    MockLogger.Object,
                    MockMediator.Object,
                    MockQueryFactory.Object,
                    MockGetListActionResultFunc.Object,
                    MockGetViewActionResultFunc.Object);

                Assert.NotNull(instance);

                var httpContext = new DefaultHttpContext { User = new ClaimsPrincipal() };
                var controllerContext = new ControllerContext { HttpContext = httpContext };
                instance.ControllerContext = controllerContext;

                var result = await instance.ViewAsync(1, CancellationToken.None);
                Assert.NotNull(result);
            }
        }
    }
}
