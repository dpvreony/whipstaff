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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Whipstaff.Core;
using Whipstaff.Core.Mediatr;
using Whipstaff.Testing;
using Whipstaff.Testing.Cqrs;
using Whipstaff.Testing.MediatR;
using Xunit;
using Xunit.Abstractions;

namespace Whipstaff.UnitTests.Controllers
{
#pragma warning disable CA1034
    /// <summary>
    /// Unit Tests for the CRUD controller.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class CrudControllerTests
    {
        /// <summary>
        /// Authorization Function for validating the request has the expected policy name.
        /// </summary>
        /// <param name="expectedPolicyName">Expected Policy Name for the request.</param>
        /// <returns>Function to execute in Authorization Manager.</returns>
        public static Func<ClaimsPrincipal, object?, string, Task<AuthorizationResult>> GetSimplePolicyNameFunc(string expectedPolicyName) =>
            (principal, o, policyName) =>
            {
                // TODO: move to a helper method.
                ArgumentNullException.ThrowIfNull(principal);
                ArgumentNullException.ThrowIfNull(o);
                ArgumentNullException.ThrowIfNull(policyName);

                _ = Assert.IsType<int>(o);

                if (policyName.Equals(expectedPolicyName, StringComparison.Ordinal))
                {
                    return Task.FromResult(AuthorizationResult.Success());
                }

                return Task.FromResult(AuthorizationResult.Failed());
            };

        /// <summary>
        /// Gets an empty mediator.
        /// </summary>
        /// <returns>Empty mediator.</returns>
        public static IMediator MockMediatorFactory()
        {
            return new Mediator(null!);
        }

        /// <summary>
        /// Gets a mediator with a mocked request handler.
        /// </summary>
        /// <typeparam name="TRequest">Type for the mediator request.</typeparam>
        /// <typeparam name="TResponse">Type for the mediator response.</typeparam>
        /// <param name="func">Test function to handle the request.</param>
        /// <returns>Mediator with simple registration.</returns>
        public static IMediator MockMediatorFactory<TRequest, TResponse>(Func<TRequest, CancellationToken, Task<TResponse>> func)
            where TRequest : IRequest<TResponse>
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a mock command factory.
        /// </summary>
        /// <returns>Command Factory.</returns>
        public static IAuditableCommandFactory<FakeCrudAddCommand, int, int?, FakeCrudDeleteCommand, long?, FakeCrudUpdateCommand, int, FakeCrudUpdateResponse> MockCommandFactory()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets a mock query factory.
        /// </summary>
        /// <returns>Query Factory.</returns>
        public static IAuditableQueryFactory<FakeCrudListQuery, FakeCrudListRequest, IList<int>, FakeCrudViewQuery, FakeCrudViewResponse> MockQueryFactory()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Unit tests for the constructor.
        /// </summary>
        public sealed class ConstructorMethod : Foundatio.Xunit.TestWithLoggingBase
        {
            /// <summary>
            /// Test Data for checking an Argument Null Exception is thrown.
            /// </summary>
            public static readonly TheoryData<
                    IAuthorizationService?,
                    NullLogger<FakeCrudController>?,
                    IMediator?,
                    IAuditableCommandFactory<
                        FakeCrudAddCommand,
                        int,
                        int?,
                        FakeCrudDeleteCommand,
                        long?,
                        FakeCrudUpdateCommand,
                        int,
                        FakeCrudUpdateResponse>?,
                    IAuditableQueryFactory<
                        FakeCrudListQuery,
                        FakeCrudListRequest,
                        IList<int>,
                        FakeCrudViewQuery,
                        FakeCrudViewResponse>?,
                    FakeCrudControllerLogMessageActions?,
                    string> ThrowsArgumentNullExceptionTestData = new()
            {
                {
                    null,
                    new NullLogger<FakeCrudController>(),
                    MockMediatorFactory(),
                    MockCommandFactory(),
                    MockQueryFactory(),
                    new FakeCrudControllerLogMessageActions(),
                    "authorizationService"
                },
                {
                    new FakeAuthorizationService(),
                    null,
                    MockMediatorFactory(),
                    MockCommandFactory(),
                    MockQueryFactory(),
                    new FakeCrudControllerLogMessageActions(),
                    "logger"
                },
                {
                    new FakeAuthorizationService(),
                    new NullLogger<FakeCrudController>(),
                    null,
                    MockCommandFactory(),
                    MockQueryFactory(),
                    new FakeCrudControllerLogMessageActions(),
                    "mediator"
                },
                {
                    new FakeAuthorizationService(),
                    new NullLogger<FakeCrudController>(),
                    MockMediatorFactory(),
                    null,
                    MockQueryFactory(),
                    new FakeCrudControllerLogMessageActions(),
                    "commandFactory"
                },
                {
                    new FakeAuthorizationService(),
                    new NullLogger<FakeCrudController>(),
                    MockMediatorFactory(),
                    MockCommandFactory(),
                    null,
                    new FakeCrudControllerLogMessageActions(),
                    "queryFactory"
                },
                {
                    new FakeAuthorizationService(),
                    new NullLogger<FakeCrudController>(),
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
            /// <param name="logMessageActionMappings">Log Message Action Mapping.</param>
            /// <param name="argumentNullExceptionParameterName">Name of the parameter expected to cause the exception.</param>
            [Theory]
            [MemberData(nameof(ThrowsArgumentNullExceptionTestData))]
            public void ThrowsArgumentNullException(
                IAuthorizationService? authorizationService,
                ILogger<FakeCrudController>? logger,
                IMediator? mediator,
                IAuditableCommandFactory<
                    FakeCrudAddCommand,
                    int,
                    int?,
                    FakeCrudDeleteCommand,
                    long?,
                    FakeCrudUpdateCommand,
                    int,
                    FakeCrudUpdateResponse?>? commandFactory,
                IAuditableQueryFactory<
                    FakeCrudListQuery,
                    FakeCrudListRequest,
                    IList<int>,
                    FakeCrudViewQuery,
                    FakeCrudViewResponse?>? queryFactory,
                FakeCrudControllerLogMessageActions? logMessageActionMappings,
                string argumentNullExceptionParameterName)
            {
                var ex = Assert.Throws<ArgumentNullException>(() => new FakeCrudController(
                    authorizationService!,
                    logger!,
                    mediator!,
                    commandFactory!,
                    queryFactory!,
                    logMessageActionMappings!));

                Assert.Equal(argumentNullExceptionParameterName, ex.ParamName);
            }

            /// <summary>
            /// Unit test to ensure an instance is created.
            /// </summary>
            [Fact]
            public void ReturnsInstance()
            {
                var mockAuthorizationService = new FakeAuthorizationService();
                var mockLogger = new NullLogger<FakeCrudController>();
                var mockMediator = MockMediatorFactory();
                var commandFactory = MockCommandFactory();
                var queryFactory = MockQueryFactory();

                using (var instance = new FakeCrudController(
                    mockAuthorizationService,
                    mockLogger,
                    mockMediator,
                    commandFactory,
                    queryFactory,
                    new FakeCrudControllerLogMessageActions()))
                {
                    Assert.NotNull(instance);
                }
            }
        }

        /// <summary>
        /// Unit Tests for the Post call.
        /// </summary>
        public sealed class PostAsyncMethod : Foundatio.Xunit.TestWithLoggingBase
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
                FakeAuthorizationService mockAuthorizationService = new()
                {
                    AuthorizeAsyncUserResourcePolicyNameFunc = GetSimplePolicyNameFunc("addPolicyName"),
                };

                var mediator = MockMediatorFactory<IAuditableRequest<int, int?>, int?>(MockAddMediatorHandlerAsync);

                var commandFactory = MockCommandFactory();
                var queryFactory = MockQueryFactory();

                using (var instance = new FakeCrudController(
                    mockAuthorizationService,
                    Log.CreateLogger<FakeCrudController>(),
                    mediator,
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
                })
                {
                    var result = await instance.Post(addRequestDto, CancellationToken.None).ConfigureAwait(false);
                    Assert.NotNull(result);
                    _ = Assert.IsType<OkObjectResult>(result);
                }
            }

            /// <summary>
            /// Test to ensure a HTTP BAD REQUEST is returned.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsBadRequestAsync()
            {
                FakeAuthorizationService authorizationService = new()
                {
                    AuthorizeAsyncUserResourcePolicyNameFunc = GetSimplePolicyNameFunc("addPolicyName"),
                };

                var mediator = MockMediatorFactory<IAuditableRequest<int, int?>, int?>(MockAddMediatorHandlerAsync);

                var commandFactory = MockCommandFactory();
                var queryFactory = MockQueryFactory();

                using (var instance = new FakeCrudController(
                    authorizationService,
                    Log.CreateLogger<FakeCrudController>(),
                    mediator,
                    commandFactory,
                    queryFactory,
                    new FakeCrudControllerLogMessageActions())
                {
                    ControllerContext = new ControllerContext
                    {
                        HttpContext = new DefaultHttpContext
                        {
                            Request = { IsHttps = false },
                            User = new ClaimsPrincipal(new ClaimsIdentity()),
                        },
                    },
                })
                {
                    var result = await instance.Post(1, CancellationToken.None).ConfigureAwait(false);
                    Assert.NotNull(result);
                    _ = Assert.IsType<BadRequestResult>(result);
                }
            }

            private static async Task<int?> MockAddMediatorHandlerAsync(IAuditableRequest<int, int?> auditableRequest, CancellationToken cancellationToken)
            {
                return await Task.FromResult(auditableRequest.RequestDto).ConfigureAwait(false);
            }

            private static async Task<FakeCrudAddCommand> MockAddCommandAsync(int requestDto, ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
            {
                return await Task.FromResult(new FakeCrudAddCommand(requestDto, claimsPrincipal)).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Unit Tests for the Delete call.
        /// </summary>
        public sealed class DeleteAsyncMethod : Foundatio.Xunit.TestWithLoggingBase
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
                FakeAuthorizationService authorizationService = new()
                {
                    AuthorizeAsyncUserResourcePolicyNameFunc = GetSimplePolicyNameFunc("deletePolicyName"),
                };

                var mediator = MockMediatorFactory<IAuditableRequest<long, long?>, long?>(MockDeleteMediatorHandlerAsync);

                var commandFactory = MockCommandFactory();
                var queryFactory = MockQueryFactory();

                using (var instance = new FakeCrudController(
                    authorizationService,
                    Log.CreateLogger<FakeCrudController>(),
                    mediator,
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
                })
                {
                    var result = await instance.Delete(id, CancellationToken.None).ConfigureAwait(false);
                    Assert.NotNull(result);
                    _ = Assert.IsType<OkObjectResult>(result);
                }
            }

            /// <summary>
            /// Tests to make sure a HTTP BAD REQUEST is returned.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsBadRequestAsync()
            {
                FakeAuthorizationService authorizationService = new()
                {
                    AuthorizeAsyncUserResourcePolicyNameFunc = GetSimplePolicyNameFunc("deletePolicyName"),
                };

                var mediator = MockMediatorFactory<IAuditableRequest<long, long?>, long?>(MockDeleteMediatorHandlerAsync);

                var commandFactory = MockCommandFactory();
                var queryFactory = MockQueryFactory();

                using (var instance = new FakeCrudController(
                    authorizationService,
                    Log.CreateLogger<FakeCrudController>(),
                    mediator,
                    commandFactory,
                    queryFactory,
                    new FakeCrudControllerLogMessageActions())
                {
                    ControllerContext = new ControllerContext
                    {
                        HttpContext = new DefaultHttpContext
                        {
                            Request = { IsHttps = false },
                            User = new ClaimsPrincipal(new HttpListenerBasicIdentity("user", "pass")),
                        },
                    },
                })
                {
                    var result = await instance.Delete(1, CancellationToken.None).ConfigureAwait(false);
                    Assert.NotNull(result);
                    _ = Assert.IsType<BadRequestResult>(result);
                }
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
                FakeAuthorizationService authorizationService = new()
                {
                    AuthorizeAsyncUserResourcePolicyNameFunc = GetSimplePolicyNameFunc("deletePolicyName"),
                };

                var mediator = MockMediatorFactory<IAuditableRequest<long, long?>, long?>(MockDeleteMediatorHandlerAsync);

                var commandFactory = MockCommandFactory();
                var queryFactory = MockQueryFactory();

                using (var instance = new FakeCrudController(
                    authorizationService,
                    Log.CreateLogger<FakeCrudController>(),
                    mediator,
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
                })
                {
                    var result = await instance.Delete(id, CancellationToken.None).ConfigureAwait(false);
                    Assert.NotNull(result);
                    _ = Assert.IsType<NotFoundResult>(result);
                }
            }

            private static async Task<long?> MockDeleteMediatorHandlerAsync(IAuditableRequest<long, long?> arg1, CancellationToken arg2)
            {
                return await Task.FromResult(arg1.RequestDto).ConfigureAwait(false);
            }

            private static async Task<FakeCrudDeleteCommand> MockDeleteCommandAsync(long requestDto, ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
            {
                return await Task.FromResult(new FakeCrudDeleteCommand(requestDto, claimsPrincipal)).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Unit Tests for the List call.
        /// </summary>
        public sealed class ListAsyncMethod : Foundatio.Xunit.TestWithLoggingBase
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

                FakeAuthorizationService authorizationService = new()
                {
                    AuthorizeAsyncUserResourcePolicyNameFunc = GetSimplePolicyNameFunc("listPolicyName"),
                };

                var mediator = MockMediatorFactory<FakeCrudListQuery, IList<int>?>(MockListMediatorHandlerAsync);

                var commandFactory = MockCommandFactory();
                var queryFactory = MockQueryFactory();

                using (var instance = new FakeCrudController(
                    authorizationService,
                    Log.CreateLogger<FakeCrudController>(),
                    mediator,
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
                })
                {
                    var result = await instance.Get(null, CancellationToken.None).ConfigureAwait(false);
                    Assert.NotNull(result);
                    _ = Assert.IsType<OkObjectResult>(result);
                }
            }

            /// <summary>
            /// Unit tests to ensure HTTP BAD REQUEST.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsBadRequestAsync()
            {
                FakeAuthorizationService authorizationService = new()
                {
                    AuthorizeAsyncUserResourcePolicyNameFunc = GetSimplePolicyNameFunc("listPolicyName"),
                };

                var mediator = MockMediatorFactory<FakeCrudListQuery, IList<int>?>(MockListMediatorHandlerAsync);

                var commandFactory = MockCommandFactory();
                var queryFactory = MockQueryFactory();

                using (var instance = new FakeCrudController(
                    authorizationService,
                    Log.CreateLogger<FakeCrudController>(),
                    mediator,
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
                                IsHttps = false,
                                Query = new QueryCollection(),
                            },
                        },
                    },
                })
                {
                    var result = await instance.Get(null, CancellationToken.None).ConfigureAwait(false);
                    Assert.NotNull(result);
                    _ = Assert.IsType<BadRequestResult>(result);
                }
            }

#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
            private static async Task<IList<int>?> MockListMediatorHandlerAsync(FakeCrudListQuery auditableRequest, CancellationToken cancellationToken)
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
            {
                return new List<int> { 1, 2, 3 };
            }

            private static Task<FakeCrudListQuery> MockListQueryAsync(FakeCrudListRequest requestDto, ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
            {
                return Task.FromResult(new FakeCrudListQuery(requestDto, claimsPrincipal));
            }
        }

        /// <summary>
        /// Unit Tests for the PUT request.
        /// </summary>
        public sealed class PutAsyncMethod : Foundatio.Xunit.TestWithLoggingBase
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
                FakeAuthorizationService authorizationService = new()
                {
                    AuthorizeAsyncUserResourcePolicyNameFunc = GetSimplePolicyNameFunc("updatePolicyName"),
                };

                var mediator = MockMediatorFactory<FakeCrudUpdateCommand, FakeCrudUpdateResponse?>(MockUpdateMediatorHandlerAsync);

                var commandFactory = MockCommandFactory();
                var queryFactory = MockQueryFactory();

                using (var instance = new FakeCrudController(
                    authorizationService,
                    Log.CreateLogger<FakeCrudController>(),
                    mediator,
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
                })
                {
                    var result = await instance.Put(1, updateRequestDto, CancellationToken.None).ConfigureAwait(false);
                    Assert.NotNull(result);
                    _ = Assert.IsType<OkObjectResult>(result);
                }
            }

            /// <summary>
            /// Unit tests to ensure HTTP BAD REQUEST.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsBadRequestAsync()
            {
                FakeAuthorizationService authorizationService = new()
                {
                    AuthorizeAsyncUserResourcePolicyNameFunc = GetSimplePolicyNameFunc("updatePolicyName"),
                };

                var mediator = MockMediatorFactory<FakeCrudUpdateCommand, FakeCrudUpdateResponse?>(MockUpdateMediatorHandlerAsync);

                var commandFactory = MockCommandFactory();
                var queryFactory = MockQueryFactory();

                using (var instance = new FakeCrudController(
                    authorizationService,
                    Log.CreateLogger<FakeCrudController>(),
                    mediator,
                    commandFactory,
                    queryFactory,
                    new FakeCrudControllerLogMessageActions())
                {
                    ControllerContext = new ControllerContext
                    {
                        HttpContext = new DefaultHttpContext
                        {
                            Request = { IsHttps = false },
                            User = new ClaimsPrincipal(new HttpListenerBasicIdentity("user", "pass")),
                        },
                    },
                })
                {
                    var result = await instance.Put(1, 1, CancellationToken.None).ConfigureAwait(false);
                    Assert.NotNull(result);
                    _ = Assert.IsType<BadRequestResult>(result);
                }
            }

            private static Task<FakeCrudUpdateResponse?> MockUpdateMediatorHandlerAsync(FakeCrudUpdateCommand arg1, CancellationToken arg2)
            {
                return Task.FromResult<FakeCrudUpdateResponse?>(new FakeCrudUpdateResponse());
            }

            private static Task<FakeCrudUpdateCommand> MockUpdateCommandAsync(int requestDto, ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
            {
                return Task.FromResult(new FakeCrudUpdateCommand(requestDto, claimsPrincipal));
            }
        }

        /// <summary>
        /// Unit Tests for the View call.
        /// </summary>
        public sealed class ViewAsyncMethod : Foundatio.Xunit.TestWithLoggingBase
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
                FakeAuthorizationService authorizationService = new()
                {
                    AuthorizeAsyncUserResourcePolicyNameFunc = GetSimplePolicyNameFunc("viewPolicyName"),
                };

#if OLD
                var authorizationService = MockAuthorizationServiceFactory();
                _ = authorizationService.Setup(s =>
                    s.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<object>(), "viewPolicyName"))
                    .Returns(async () => await Task.FromResult(AuthorizationResult.Success()).ConfigureAwait(false));
                _ = authorizationService.Setup(s =>
                        s.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), null, "viewPolicyName"))
                    .Returns(async () => await Task.FromResult(AuthorizationResult.Success()).ConfigureAwait(false));
#endif

                var mediator = MockMediatorFactory<FakeCrudViewQuery, FakeCrudViewResponse?>(MockViewMediatorHandlerAsync);

                var commandFactory = MockCommandFactory();
                var queryFactory = MockQueryFactory();

                using (var instance = new FakeCrudController(
                    authorizationService,
                    Log.CreateLogger<FakeCrudController>(),
                    mediator,
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
                })
                {
                    var result = await instance.Get(listRequest, CancellationToken.None).ConfigureAwait(false);
                    Assert.NotNull(result);
                    Assert.IsType(expectedResultType, result);
                }
            }

            private static Task<FakeCrudViewResponse?> MockViewMediatorHandlerAsync(FakeCrudViewQuery auditableRequest, CancellationToken cancellationToken)
            {
                return Task.FromResult(auditableRequest.RequestDto == 1 ? new FakeCrudViewResponse() : null);
            }

            private static Task<FakeCrudViewQuery> MockViewQueryAsync(long requestDto, ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
            {
                return Task.FromResult(new FakeCrudViewQuery(requestDto, claimsPrincipal));
            }
        }
    }
}
