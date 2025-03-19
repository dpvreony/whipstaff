// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Rocks;
using Whipstaff.Core;
using Whipstaff.MediatR;
using Whipstaff.Testing;
using Whipstaff.Testing.Cqrs;
using Whipstaff.Testing.Logging;
using Xunit;

namespace Whipstaff.UnitTests.Controllers
{
    /// <summary>
    /// Expectations for the Authorization Service.
    /// </summary>
    [RockPartial(typeof(IAuthorizationService), BuildType.Create)]
#pragma warning disable SA1402 // File may only contain a single type
    public sealed partial class IAuthorizationServiceCreateExpectations;

    /// <summary>
    /// Expectations for the Mediator.
    /// </summary>
    [RockPartial(typeof(IMediator), BuildType.Create)]
    public sealed partial class IMediatorCreateExpectations;

    /// <summary>
    /// Expectations for the Mediator.
    /// </summary>
    /// <typeparam name="TCategoryName">The type whose name is used for the logger category name.</typeparam>
    [RockPartial(typeof(ILogger<>), BuildType.Create)]
    public sealed partial class ILoggerCreateExpectations<TCategoryName>;
#pragma warning restore SA1402 // File may only contain a single type

#pragma warning disable CA1034
    /// <summary>
    /// Unit Tests for the CRUD controller.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class CrudControllerTests
    {
        private static IAuthorizationServiceCreateExpectations MockAuthorizationServiceFactory() => new();

        private static ILoggerCreateExpectations<FakeCrudController> MockLoggerFactory() => new();

        private static IMediatorCreateExpectations MockMediatorFactory() => new();

        private static FakeAuditableCommandFactory MockCommandFactory() => new();

        private static FakeAuditableQueryFactory MockQueryFactory() => new();

        /// <summary>
        /// Unit tests for the constructor.
        /// </summary>
        public sealed class ConstructorMethod : TestWithLoggingBase
        {
            /// <summary>
            /// Test Data for checking an Argument Null Exception is thrown.
            /// </summary>
            public static readonly TheoryData<
                    IAuthorizationServiceCreateExpectations?,
                    ILoggerCreateExpectations<FakeCrudController>?,
                    IMediatorCreateExpectations?,
                    FakeAuditableCommandFactory?,
                    FakeAuditableQueryFactory?,
                    FakeCrudControllerLogMessageActions?,
                    string> ThrowsArgumentNullExceptionTestData = new()
            {
                {
                    null,
                    MockLoggerFactory(),
                    MockMediatorFactory(),
                    MockCommandFactory(),
                    MockQueryFactory(),
                    new FakeCrudControllerLogMessageActions(),
                    "authorizationService"
                },
                {
                    MockAuthorizationServiceFactory(),
                    null,
                    MockMediatorFactory(),
                    MockCommandFactory(),
                    MockQueryFactory(),
                    new FakeCrudControllerLogMessageActions(),
                    "logger"
                },
                {
                    MockAuthorizationServiceFactory(),
                    MockLoggerFactory(),
                    null,
                    MockCommandFactory(),
                    MockQueryFactory(),
                    new FakeCrudControllerLogMessageActions(),
                    "mediator"
                },
                {
                    MockAuthorizationServiceFactory(),
                    MockLoggerFactory(),
                    MockMediatorFactory(),
                    null,
                    MockQueryFactory(),
                    new FakeCrudControllerLogMessageActions(),
                    "commandFactory"
                },
                {
                    MockAuthorizationServiceFactory(),
                    MockLoggerFactory(),
                    MockMediatorFactory(),
                    MockCommandFactory(),
                    null,
                    new FakeCrudControllerLogMessageActions(),
                    "queryFactory"
                },
                {
                    MockAuthorizationServiceFactory(),
                    MockLoggerFactory(),
                    MockMediatorFactory(),
                    MockCommandFactory(),
                    MockQueryFactory(),
                    null,
                    "logMessageActionMappings"
                },
            };

            /// <summary>
            /// Initializes a new instance of the <see cref="ConstructorMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit Test Output helper.</param>
            public ConstructorMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <summary>
            /// Unit tests for ensuring an Argument Null Exception is thrown.
            /// </summary>
            /// <param name="authorizationService">Instance of the authorization service.</param>
            /// <param name="logger">Instance of the logging framework.</param>
            /// <param name="mediator">Instance of the CQRS mediator instance.</param>
            /// <param name="commandFactory">Instance of the Command factory used for creating commands to pass to the mediator.</param>
            /// <param name="queryFactory">Instance of the Query factory used for creating queries to pass to the mediator.</param>
            /// <param name="logMessageActionMappings">LoggerFactory Message Action Mapping.</param>
            /// <param name="argumentNullExceptionParameterName">Name of the parameter expected to cause the exception.</param>
            [Theory]
            [MemberData(nameof(ThrowsArgumentNullExceptionTestData))]
            public void ThrowsArgumentNullException(
                IAuthorizationServiceCreateExpectations? authorizationService,
                ILoggerCreateExpectations<FakeCrudController>? logger,
                IMediatorCreateExpectations? mediator,
                FakeAuditableCommandFactory? commandFactory,
                FakeAuditableQueryFactory? queryFactory,
                FakeCrudControllerLogMessageActions? logMessageActionMappings,
                string argumentNullExceptionParameterName)
            {
                var ex = Assert.Throws<ArgumentNullException>(
                    argumentNullExceptionParameterName,
                    () => new FakeCrudController(
                    authorizationService?.Instance()!,
                    logger?.Instance()!,
                    mediator?.Instance()!,
                    commandFactory!,
                    queryFactory!,
                    logMessageActionMappings!));

                Assert.Equal(argumentNullExceptionParameterName, ex.ParamName);

                authorizationService?.Verify();
                logger?.Verify();
                mediator?.Verify();
            }

            /// <summary>
            /// Unit test to ensure an instance is created.
            /// </summary>
            [Fact]
            public void ReturnsInstance()
            {
                var mockAuthorizationService = MockAuthorizationServiceFactory();
                var mockLogger = MockLoggerFactory();
                var mockMediator = MockMediatorFactory();
                var commandFactory = MockCommandFactory();
                var queryFactory = MockQueryFactory();

                var instance = new FakeCrudController(
                    mockAuthorizationService.Instance(),
                    mockLogger.Instance(),
                    mockMediator.Instance(),
                    commandFactory,
                    queryFactory,
                    new FakeCrudControllerLogMessageActions());
                Assert.NotNull(instance);

                mockAuthorizationService.Verify();
                mockLogger.Verify();
                mockMediator.Verify();
            }
        }

        /// <summary>
        /// Unit Tests for the Post call.
        /// </summary>
        public sealed class PostAsyncMethod : TestWithLoggingBase
        {
            /// <summary>
            /// Gets the XUnit test source for testing POST methods succeed.
            /// </summary>
            public static readonly IEnumerable<object[]> ShouldSucceedTestData = new[]
            {
                new object[] { 0, },
                new object[] { -1, },
            };

            /// <summary>
            /// Initializes a new instance of the <see cref="PostAsyncMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit Test Output helper.</param>
            public PostAsyncMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <summary>
            /// Unit Tests to ensure POST requests succeed.
            /// </summary>
            /// <param name="addRequestDto">request DTO.</param>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Theory]
            [MemberData(nameof(ShouldSucceedTestData))]
            public async Task ShouldSucceedAsync(int addRequestDto)
            {
                var authorizationService = MockAuthorizationServiceFactory();
                authorizationService.Methods.AuthorizeAsync(
                        Arg.Any<ClaimsPrincipal>(),
                        Arg.Any<int>(),
                        Arg.Is("addPolicyName"))
                    .ReturnValue(Task.FromResult(AuthorizationResult.Success()));

                var logger = MockLoggerFactory();

                var mediator = MockMediatorFactory();
                _ = mediator.Methods.Send<int?>(
                        Arg.Validate<IRequest<int?>>(query => true),
                        Arg.Any<CancellationToken>())
                    .Callback(static (a, b) =>
                    {
                        var fakeCrudViewQuery = a as IAuditableRequest<int, int?>;
                        return MockAddMediatorHandlerAsync(fakeCrudViewQuery!, b);
                    });

                var commandFactory = MockCommandFactory();
                var queryFactory = MockQueryFactory();

                var instance = new FakeCrudController(
                    authorizationService.Instance(),
                    logger.Instance(),
                    mediator.Instance(),
                    commandFactory,
                    queryFactory,
                    new FakeCrudControllerLogMessageActions())
                {
                    ControllerContext = new ControllerContext
                    {
                        HttpContext = new DefaultHttpContext
                        {
                            Request = { IsHttps = true },
                            User = new ClaimsPrincipal(new HttpListenerBasicIdentity("user", "pass")),
                        },
                    },
                };

                var result = await instance.PostAsync(addRequestDto, CancellationToken.None);
                Assert.NotNull(result);
                _ = Assert.IsType<OkObjectResult>(result.Result);
            }

            private static async Task<int?> MockAddMediatorHandlerAsync(IAuditableRequest<int, int?> auditableRequest, CancellationToken cancellationToken)
            {
                return await Task.FromResult(auditableRequest.RequestDto);
            }
        }

        /// <summary>
        /// Unit Tests for the Delete call.
        /// </summary>
        public sealed class DeleteAsyncMethod : TestWithLoggingBase
        {
            /// <summary>
            /// Gets the XUnit test source for testing DELETE methods succeed.
            /// </summary>
            public static readonly IEnumerable<object[]> ShouldSucceedTestData = new[]
            {
                new object[] { 1, },
                new object[] { 2, },
            };

            /// <summary>
            /// Gets the XUnit test source for testing DELETE methods succeed.
            /// </summary>
            public static readonly IEnumerable<object[]> ReturnsHttpNotFoundTestData = new[]
            {
                new object[] { 0, },
                new object[] { -1, },
            };

            /// <summary>
            /// Initializes a new instance of the <see cref="DeleteAsyncMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit Test Output helper.</param>
            public DeleteAsyncMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <summary>
            /// Unit Tests to ensure DELETE requests succeed.
            /// </summary>
            /// <param name="id">unique id of the entity.</param>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Theory]
            [MemberData(nameof(ShouldSucceedTestData))]
            public async Task ShouldSucceedAsync(int id)
            {
                var authorizationService = MockAuthorizationServiceFactory();
                _ = authorizationService.Setup(s =>
                    s.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<object>(), "deletePolicyName"))
                    .Returns(async () => await Task.FromResult(AuthorizationResult.Success()));

                var logger = MockLoggerFactory();

                var mediator = MockMediatorFactory();
                _ = mediator.Setup(s => s.Send(It.IsAny<IAuditableRequest<long, long?>>(), It.IsAny<CancellationToken>())).Returns<IAuditableRequest<long, long?>, CancellationToken>(MockDeleteMediatorHandlerAsync);

                var commandFactory = MockCommandFactory();
                var queryFactory = MockQueryFactory();

                var instance = new FakeCrudController(
                    authorizationService.Instance(),
                    logger.Instance(),
                    mediator.Instance(),
                    commandFactory,
                    queryFactory,
                    new FakeCrudControllerLogMessageActions())
                {
                    ControllerContext = new ControllerContext
                    {
                        HttpContext = new DefaultHttpContext
                        {
                            Request = { IsHttps = true },
                            User = new ClaimsPrincipal(new ClaimsIdentity()),
                        },
                    },
                };

                var result = await instance.DeleteAsync(id, CancellationToken.None);
                Assert.NotNull(result);
                _ = Assert.IsType<OkObjectResult>(result.Result);
            }

            /// <summary>
            /// Unit Tests to ensure DELETE requests succeed.
            /// </summary>
            /// <param name="id">unique id of the entity.</param>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Theory]
            [MemberData(nameof(ReturnsHttpNotFoundTestData))]
            public async Task ReturnsHttpNotFoundAsync(int id)
            {
                var authorizationService = MockAuthorizationServiceFactory();

                _ = authorizationService.Methods.AuthorizeAsync(
                        Arg.Any<ClaimsPrincipal>(),
                        Arg.Any<int>(),
                        Arg.Is("deletePolicyName"))
                    .ReturnValue(Task.FromResult(AuthorizationResult.Success()));

                var logger = MockLoggerFactory();

                var mediator = MockMediatorFactory();
                _ = mediator.Setup(s => s.Send(It.IsAny<IAuditableRequest<long, long?>>(), It.IsAny<CancellationToken>())).Returns<IAuditableRequest<long, long?>, CancellationToken>(MockDeleteMediatorHandlerAsync);

                var commandFactory = MockCommandFactory();
                var queryFactory = MockQueryFactory();

                var instance = new FakeCrudController(
                    authorizationService.Instance(),
                    logger.Instance(),
                    mediator.Instance(),
                    commandFactory,
                    queryFactory,
                    new FakeCrudControllerLogMessageActions())
                {
                    ControllerContext = new ControllerContext
                    {
                        HttpContext = new DefaultHttpContext
                        {
                            Request = { IsHttps = true },
                            User = new ClaimsPrincipal(new HttpListenerBasicIdentity("user", "pass")),
                        },
                    },
                };

                var result = await instance.DeleteAsync(id, CancellationToken.None);
                Assert.NotNull(result);
                _ = Assert.IsType<NotFoundResult>(result.Result);
            }

            private static async Task<long?> MockDeleteMediatorHandlerAsync(IAuditableRequest<long, long?> arg1, CancellationToken arg2)
            {
                return await Task.FromResult(arg1.RequestDto).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Unit Tests for the List call.
        /// </summary>
        public sealed class ListAsyncMethod : TestWithLoggingBase
        {
            /// <summary>
            /// Gets the XUnit Test Source for ensuring the list request succeeds.
            /// </summary>
            public static readonly IEnumerable<object[]> ShouldSucceedTestData = new[]
            {
                new object[]
                {
                    new FakeCrudListRequest(),
                },
            };

            /// <summary>
            /// Initializes a new instance of the <see cref="ListAsyncMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit Test Output helper.</param>
            public ListAsyncMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <summary>
            /// Unit tests to ensure list requests succeed.
            /// </summary>
            /// <param name="listRequest">request DTO.</param>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Theory]
            [MemberData(nameof(ShouldSucceedTestData))]
            public async Task ShouldSucceedAsync(FakeCrudListRequest listRequest)
            {
                Assert.NotNull(listRequest);

                var authorizationService = MockAuthorizationServiceFactory();

                _ = authorizationService.Methods.AuthorizeAsync(
                        Arg.Any<ClaimsPrincipal>(),
                        Arg.Any<object?>(),
                        Arg.Is("listPolicyName"))
                    .ReturnValue(Task.FromResult(AuthorizationResult.Success()));

                var logger = MockLoggerFactory();

                var mediator = MockMediatorFactory();
                _ = mediator.Setup(s => s.Send(It.IsAny<FakeCrudListQuery>(), It.IsAny<CancellationToken>())).Returns<FakeCrudListQuery, CancellationToken>(MockListMediatorHandlerAsync);

                var commandFactory = MockCommandFactory();
                var queryFactory = MockQueryFactory();

                var instance = new FakeCrudController(
                    authorizationService.Instance(),
                    logger.Instance(),
                    mediator.Instance(),
                    commandFactory,
                    queryFactory,
                    new FakeCrudControllerLogMessageActions())
                {
                    ControllerContext = new ControllerContext
                    {
                        HttpContext = new DefaultHttpContext
                        {
                            User = new ClaimsPrincipal(new HttpListenerBasicIdentity("user", "pass")),
                            Request =
                            {
                                IsHttps = true,
                                Query = new QueryCollection(),
                            },
                        },
                    },
                };

                var result = await instance.ListAsync(CancellationToken.None);
                Assert.NotNull(result);
                _ = Assert.IsType<OkObjectResult>(result.Result);
            }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
            private static async Task<IList<int>?> MockListMediatorHandlerAsync(FakeCrudListQuery auditableRequest, CancellationToken cancellationToken)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
            {
                return new List<int> { 1, 2, 3 };
            }
        }

        /// <summary>
        /// Unit Tests for the PUT request.
        /// </summary>
        public sealed class PutAsyncMethod : TestWithLoggingBase
        {
            /// <summary>
            /// Gets the XUnit test source for making sure PUT requests succeed.
            /// </summary>
            public static readonly IEnumerable<object[]> ThrowsArgumentNullExceptionTestData = new[]
            {
                new object[] { 0, },
                new object[] { -1, },
            };

            /// <summary>
            /// Initializes a new instance of the <see cref="PutAsyncMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit Test Output helper.</param>
            public PutAsyncMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <summary>
            /// Unit tests to ensure PUT requests succeed.
            /// </summary>
            /// <param name="updateRequestDto">Unique Id of the entity.</param>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Theory]
            [MemberData(nameof(ThrowsArgumentNullExceptionTestData))]
            public async Task ShouldSucceedAsync(int updateRequestDto)
            {
                var authorizationService = MockAuthorizationServiceFactory();

                _ = authorizationService.Methods.AuthorizeAsync(
                        Arg.Any<ClaimsPrincipal>(),
                        Arg.Any<int>(),
                        Arg.Is("updatePolicyName"))
                    .ReturnValue(Task.FromResult(AuthorizationResult.Success()));

                var logger = MockLoggerFactory();

                var mediator = MockMediatorFactory();
                _ = mediator.Setup(s => s.Send(It.IsAny<FakeCrudUpdateCommand>(), It.IsAny<CancellationToken>())).Returns<FakeCrudUpdateCommand, CancellationToken>(MockUpdateMediatorHandlerAsync);

                var commandFactory = MockCommandFactory();
                var queryFactory = MockQueryFactory();

                var instance = new FakeCrudController(
                    authorizationService.Instance(),
                    logger.Instance(),
                    mediator.Instance(),
                    commandFactory,
                    queryFactory,
                    new FakeCrudControllerLogMessageActions())
                {
                    ControllerContext = new ControllerContext
                    {
                        HttpContext = new DefaultHttpContext
                        {
                            Request = { IsHttps = true },
                            User = new ClaimsPrincipal(new HttpListenerBasicIdentity("user", "pass")),
                        },
                    },
                };

                var result = await instance.PutAsync(1, updateRequestDto, CancellationToken.None);
                Assert.NotNull(result);
                _ = Assert.IsType<OkObjectResult>(result.Result);
            }

            private static Task<FakeCrudUpdateResponse?> MockUpdateMediatorHandlerAsync(FakeCrudUpdateCommand arg1, CancellationToken arg2)
            {
                return Task.FromResult<FakeCrudUpdateResponse?>(new FakeCrudUpdateResponse(arg1.RequestDto));
            }
        }

        /// <summary>
        /// Unit Tests for the View call.
        /// </summary>
        public sealed class ViewAsyncMethod : TestWithLoggingBase
        {
            /// <summary>
            /// Gets the XUnit test data source for making sure View requests succeed.
            /// </summary>
            public static readonly IEnumerable<object[]> ShouldSucceedTestData = new[]
            {
                new object[]
                {
                    -1,
                    typeof(NotFoundResult),
                },
                new object[]
                {
                    1,
                    typeof(OkObjectResult),
                },
                new object[]
                {
                    2,
                    typeof(NotFoundResult),
                },
            };

            /// <summary>
            /// Initializes a new instance of the <see cref="ViewAsyncMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit Test Output helper.</param>
            public ViewAsyncMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <summary>
            /// Unit tests for ensuring view requests succeed.
            /// </summary>
            /// <param name="listRequest">Unique Id of the entity.</param>
            /// <param name="expectedResultType">The expected result type.</param>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Theory]
            [MemberData(nameof(ShouldSucceedTestData))]
            public async Task ShouldSucceedAsync(long listRequest, Type expectedResultType)
            {
                var authorizationService = MockAuthorizationServiceFactory();

                _ = authorizationService.Methods.AuthorizeAsync(
                        Arg.Any<ClaimsPrincipal>(),
                        Arg.Any<object?>(),
                        Arg.Is("viewPolicyName"))
                    .ReturnValue(Task.FromResult(AuthorizationResult.Success()));

                _ = authorizationService.Methods.AuthorizeAsync(
                        Arg.Any<ClaimsPrincipal>(),
                        null as object,
                        Arg.Is("viewPolicyName"))
                    .ReturnValue(Task.FromResult(AuthorizationResult.Success()));

                var logger = MockLoggerFactory();

                var mediator = MockMediatorFactory();
                _ = mediator.Methods.Send<FakeCrudViewResponse?>(
                        Arg.Validate<IRequest<FakeCrudViewResponse?>>(query => true),
                        Arg.Any<CancellationToken>())
                    .Callback(static (a, b) =>
                    {
                        var fakeCrudViewQuery = a as FakeCrudViewQuery;
                        return MockViewMediatorHandlerAsync(fakeCrudViewQuery!, b);
                    });

                var commandFactory = MockCommandFactory();
                var queryFactory = MockQueryFactory();

                var instance = new FakeCrudController(
                    authorizationService.Instance(),
                    logger.Instance(),
                    mediator.Instance(),
                    commandFactory,
                    queryFactory,
                    new FakeCrudControllerLogMessageActions())
                {
                    ControllerContext = new ControllerContext
                    {
                        HttpContext = new DefaultHttpContext
                        {
                            Request = { IsHttps = true },
                            User = new ClaimsPrincipal(new HttpListenerBasicIdentity("user", "pass")),
                        },
                    },
                };

                var result = await instance.ViewAsync(listRequest, CancellationToken.None);
                Assert.NotNull(result);
                Assert.IsType(expectedResultType, result.Result);
            }

            private static Task<FakeCrudViewResponse?> MockViewMediatorHandlerAsync(FakeCrudViewQuery auditableRequest, CancellationToken cancellationToken)
            {
                return Task.FromResult(auditableRequest.RequestDto == 1 ? new FakeCrudViewResponse(auditableRequest.RequestDto) : null);
            }
        }
    }
}
