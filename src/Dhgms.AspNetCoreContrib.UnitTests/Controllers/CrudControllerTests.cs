// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Dhgms.AspNetCoreContrib.Abstractions;
using Dhgms.AspNetCoreContrib.Fakes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace Dhgms.AspNetCoreContrib.UnitTests.Controllers
{
#pragma warning disable CA1034
    /// <summary>
    /// Unit Tests for the CRUD controller.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class CrudControllerTests
    {
        private static Mock<IAuthorizationService> MockAuthorizationServiceFactory() => new Mock<IAuthorizationService>(MockBehavior.Strict);

        private static Mock<ILogger<FakeCrudController>> MockLoggerFactory()
        {
            var logger = new Mock<ILogger<FakeCrudController>>(MockBehavior.Strict);

            logger.Setup(s => s.Log(
                LogLevel.Debug,
                It.IsAny<EventId>(),
                It.IsAny<object>(),
                It.IsAny<Exception>(),
                It.IsAny<Func<object, Exception, string>>()));

            return logger;
        }

        private static Mock<IMediator> MockMediatorFactory() => new Mock<IMediator>(MockBehavior.Strict);

        private static Mock<IAuditableCommandFactory<FakeCrudAddCommand, int, int, FakeCrudDeleteCommand, long, FakeCrudUpdateCommand, int, FakeCrudUpdateResponse>> MockCommandFactory() => new Mock<IAuditableCommandFactory<FakeCrudAddCommand, int, int, FakeCrudDeleteCommand, long, FakeCrudUpdateCommand, int, FakeCrudUpdateResponse>>(MockBehavior.Strict);

        private static Mock<IAuditableQueryFactory<FakeCrudListQuery, FakeCrudListRequest, IList<int>, FakeCrudViewQuery, FakeCrudViewResponse>> MockQueryFactory() => new Mock<IAuditableQueryFactory<FakeCrudListQuery, FakeCrudListRequest, IList<int>, FakeCrudViewQuery, FakeCrudViewResponse>>(MockBehavior.Strict);

        /// <summary>
        /// Unit tests for the constructor.
        /// </summary>
        public sealed class ConstructorMethod : Foundatio.Logging.Xunit.TestWithLoggingBase
        {
            /// <summary>
            /// Test Data for checking an Argument Null Exception is thrown.
            /// </summary>
            public static readonly IEnumerable<object[]> ThrowsArgumentNullExceptionTestData = new[]
            {
                GetAuthorizationServiceNullTestData(),
                GetLoggerNullTestData(),
                GetMediatorNullTestData(),
                GetAuditableCommandFactoryNullTestData(),
                GetAuditableQueryFactoryNullTestData(),
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
            /// <param name="auditableCommandFactory">Instance of the Auditable Command Factory.</param>
            /// <param name="auditableQueryFactory">Instance of the Auditable Query Factory.</param>
            /// <param name="argumentNullExceptionParameterName">Name of the parameter expected to cause the exception.</param>
            [Theory]
            [MemberData(nameof(ThrowsArgumentNullExceptionTestData))]
            public void ThrowsArgumentNullException(
                Mock<IAuthorizationService> authorizationService,
                Mock<ILogger<FakeCrudController>> logger,
                Mock<IMediator> mediator,
                Mock<IAuditableCommandFactory<FakeCrudAddCommand, int, int, FakeCrudDeleteCommand, long, FakeCrudUpdateCommand, int, FakeCrudUpdateResponse>> auditableCommandFactory,
                Mock<IAuditableQueryFactory<FakeCrudListQuery, FakeCrudListRequest, IList<int>, FakeCrudViewQuery, FakeCrudViewResponse>> auditableQueryFactory,
                string argumentNullExceptionParameterName)
            {
                var ex = Assert.Throws<ArgumentNullException>(() => new FakeCrudController(
                    authorizationService?.Object,
                    logger?.Object,
                    mediator?.Object,
                    auditableCommandFactory?.Object,
                    auditableQueryFactory?.Object));

                Assert.Equal(argumentNullExceptionParameterName, ex.ParamName);

                authorizationService?.VerifyNoOtherCalls();
                logger?.VerifyNoOtherCalls();
                mediator?.VerifyNoOtherCalls();
                auditableCommandFactory?.VerifyNoOtherCalls();
                auditableQueryFactory?.VerifyNoOtherCalls();
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
                var mockCommandFactory = MockCommandFactory();
                var mockQueryFactory = MockQueryFactory();

                using (var instance = new FakeCrudController(
                    mockAuthorizationService.Object,
                    mockLogger.Object,
                    mockMediator.Object,
                    mockCommandFactory.Object,
                    mockQueryFactory.Object))
                {
                    Assert.NotNull(instance);
                }

                mockAuthorizationService.VerifyNoOtherCalls();
                mockLogger.VerifyNoOtherCalls();
                mockMediator.VerifyNoOtherCalls();
                mockCommandFactory.VerifyNoOtherCalls();
                mockQueryFactory.VerifyNoOtherCalls();
            }

            private static object[] GetAuthorizationServiceNullTestData()
            {
                return new object[]
                {
                    (Mock<IAuthorizationService>)null,
                    MockLoggerFactory(),
                    MockMediatorFactory(),
                    MockCommandFactory(),
                    MockQueryFactory(),
                    "authorizationService",
                };
            }

            private static object[] GetLoggerNullTestData()
            {
                return new object[]
                {
                    MockAuthorizationServiceFactory(),
                    (Mock<ILogger<FakeCrudController>>)null,
                    MockMediatorFactory(),
                    MockCommandFactory(),
                    MockQueryFactory(),
                    "logger",
                };
            }

            private static object[] GetMediatorNullTestData()
            {
                return new object[]
                {
                    MockAuthorizationServiceFactory(),
                    MockLoggerFactory(),
                    (Mock<IMediator>)null,
                    MockCommandFactory(),
                    MockQueryFactory(),
                    "mediator",
                };
            }

            private static object[] GetAuditableCommandFactoryNullTestData()
            {
                return new object[]
                {
                    MockAuthorizationServiceFactory(),
                    MockLoggerFactory(),
                    MockMediatorFactory(),
                    null,
                    MockQueryFactory(),
                    "commandFactory",
                };
            }

            private static object[] GetAuditableQueryFactoryNullTestData()
            {
                return new object[]
                {
                    MockAuthorizationServiceFactory(),
                    MockLoggerFactory(),
                    MockMediatorFactory(),
                    MockCommandFactory(),
                    null,
                    "queryFactory",
                };
            }
        }

        /// <summary>
        /// Unit Tests for the Post call.
        /// </summary>
        public sealed class PostAsyncMethod : Foundatio.Logging.Xunit.TestWithLoggingBase
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
                authorizationService.Setup(s =>
                    s.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>(), "addPolicyName"))
                    .Returns(async () => await Task.FromResult(AuthorizationResult.Success()).ConfigureAwait(false));

                var logger = MockLoggerFactory();

                var mediator = MockMediatorFactory();
                mediator.Setup(s => s.Send(It.IsAny<IAuditableRequest<int, int>>(), It.IsAny<CancellationToken>())).Returns<IAuditableRequest<int, int>, CancellationToken>(MockAddMediatorHandlerAsync);

                var auditableCommandFactory = MockCommandFactory();
                auditableCommandFactory.Setup(s =>
                    s.GetAddCommandAsync(
                        It.IsAny<int>(),
                        It.IsAny<ClaimsPrincipal>(),
                        It.IsAny<CancellationToken>()))
                    .Returns<int, ClaimsPrincipal, CancellationToken>(MockAddCommandAsync);

                var auditableQueryFactory = MockQueryFactory();

                using (var instance = new FakeCrudController(
                    authorizationService.Object,
                    logger.Object,
                    mediator.Object,
                    auditableCommandFactory.Object,
                    auditableQueryFactory.Object)
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
                    var result = await instance.PostAsync(addRequestDto, CancellationToken.None).ConfigureAwait(false);
                    Assert.NotNull(result);
                    Assert.IsType<OkObjectResult>(result);
                }
            }

            /// <summary>
            /// Test to ensure a HTTP BAD REQUEST is returned.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsBadRequestAsync()
            {
                var authorizationService = MockAuthorizationServiceFactory();
                authorizationService.Setup(s =>
                    s.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>(), "addPolicyName"))
                    .Returns(async () => await Task.FromResult(AuthorizationResult.Success()).ConfigureAwait(false));

                var logger = MockLoggerFactory();

                var mediator = MockMediatorFactory();
                mediator.Setup(s => s.Send(It.IsAny<IAuditableRequest<int, int>>(), It.IsAny<CancellationToken>())).Returns<IAuditableRequest<int, int>, CancellationToken>(MockAddMediatorHandlerAsync);

                var auditableCommandFactory = MockCommandFactory();
                auditableCommandFactory.Setup(s =>
                    s.GetAddCommandAsync(
                        It.IsAny<int>(),
                        It.IsAny<ClaimsPrincipal>(),
                        It.IsAny<CancellationToken>()))
                    .Returns<int, ClaimsPrincipal, CancellationToken>(MockAddCommandAsync);

                var auditableQueryFactory = MockQueryFactory();

                using (var instance = new FakeCrudController(
                    authorizationService.Object,
                    logger.Object,
                    mediator.Object,
                    auditableCommandFactory.Object,
                    auditableQueryFactory.Object)
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
                    var result = await instance.PostAsync(1, CancellationToken.None).ConfigureAwait(false);
                    Assert.NotNull(result);
                    Assert.IsType<BadRequestResult>(result);
                }
            }

            private static async Task<int> MockAddMediatorHandlerAsync(IAuditableRequest<int, int> auditableRequest, CancellationToken cancellationToken)
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
        public sealed class DeleteAsyncMethod : Foundatio.Logging.Xunit.TestWithLoggingBase
        {
            /// <summary>
            /// Gets the XUnit test source for testing DELETE methods succeed.
            /// </summary>
            public static readonly IEnumerable<object[]> ShouldSucceedTestData = new[]
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
                authorizationService.Setup(s =>
                    s.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<object>(), "deletePolicyName"))
                    .Returns(async () => await Task.FromResult(AuthorizationResult.Success()).ConfigureAwait(false));

                var logger = MockLoggerFactory();

                var mediator = MockMediatorFactory();
                mediator.Setup(s => s.Send(It.IsAny<IAuditableRequest<long, long>>(), It.IsAny<CancellationToken>())).Returns<IAuditableRequest<long, long>, CancellationToken>(MockDeleteMediatorHandlerAsync);

                var auditableCommandFactory = MockCommandFactory();
                auditableCommandFactory.Setup(s =>
                    s.GetDeleteCommandAsync(
                        It.IsAny<long>(),
                        It.IsAny<ClaimsPrincipal>(),
                        It.IsAny<CancellationToken>()))
                    .Returns<long, ClaimsPrincipal, CancellationToken>(MockDeleteCommandAsync);

                var auditableQueryFactory = MockQueryFactory();

                using (var instance = new FakeCrudController(
                    authorizationService.Object,
                    logger.Object,
                    mediator.Object,
                    auditableCommandFactory.Object,
                    auditableQueryFactory.Object)
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
                    var result = await instance.DeleteAsync(id, CancellationToken.None).ConfigureAwait(false);
                    Assert.NotNull(result);
                    Assert.IsType<OkObjectResult>(result);
                }
            }

            /// <summary>
            /// Tests to make sure a HTTP BAD REQUEST is returned.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsBadRequestAsync()
            {
                var authorizationService = MockAuthorizationServiceFactory();
                authorizationService.Setup(s =>
                    s.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>(), "deletePolicyName"))
                    .Returns(async () => await Task.FromResult(AuthorizationResult.Success()).ConfigureAwait(false));

                var logger = MockLoggerFactory();

                var mediator = MockMediatorFactory();
                mediator.Setup(s => s.Send(It.IsAny<IAuditableRequest<long, long>>(), It.IsAny<CancellationToken>())).Returns<IAuditableRequest<long, long>, CancellationToken>(MockDeleteMediatorHandlerAsync);

                var auditableCommandFactory = MockCommandFactory();
                auditableCommandFactory.Setup(s =>
                    s.GetDeleteCommandAsync(
                        It.IsAny<long>(),
                        It.IsAny<ClaimsPrincipal>(),
                        It.IsAny<CancellationToken>()))
                    .Returns<long, ClaimsPrincipal, CancellationToken>(MockDeleteCommandAsync);

                var auditableQueryFactory = MockQueryFactory();

                using (var instance = new FakeCrudController(
                    authorizationService.Object,
                    logger.Object,
                    mediator.Object,
                    auditableCommandFactory.Object,
                    auditableQueryFactory.Object)
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
                    var result = await instance.DeleteAsync(1, CancellationToken.None).ConfigureAwait(false);
                    Assert.NotNull(result);
                    Assert.IsType<BadRequestResult>(result);
                }
            }

            private static async Task<long> MockDeleteMediatorHandlerAsync(IAuditableRequest<long, long> arg1, CancellationToken arg2)
            {
                return await Task.FromResult(arg1.RequestDto).ConfigureAwait(false);
            }

            private static async Task<FakeCrudDeleteCommand> MockDeleteCommandAsync(long requestDto, ClaimsPrincipal claimsPrincipal, CancellationToken arg3)
            {
                return await Task.FromResult(new FakeCrudDeleteCommand(requestDto, claimsPrincipal)).ConfigureAwait(false);
            }
        }

        /// <summary>
        /// Unit Tests for the List call.
        /// </summary>
        public sealed class ListAsyncMethod : Foundatio.Logging.Xunit.TestWithLoggingBase
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
                authorizationService.Setup(s =>
                    s.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<object>(), "listPolicyName"))
                    .Returns(async () => await Task.FromResult(AuthorizationResult.Success()).ConfigureAwait(false));

                var logger = MockLoggerFactory();

                var mediator = MockMediatorFactory();
                mediator.Setup(s => s.Send(It.IsAny<FakeCrudListQuery>(), It.IsAny<CancellationToken>())).Returns<FakeCrudListQuery, CancellationToken>(MockListMediatorHandlerAsync);

                var auditableCommandFactory = MockCommandFactory();

                var auditableQueryFactory = MockQueryFactory();
                auditableQueryFactory.Setup(s =>
                        s.GetListQueryAsync(
                            It.IsAny<FakeCrudListRequest>(),
                            It.IsAny<ClaimsPrincipal>(),
                            It.IsAny<CancellationToken>()))
                    .Returns<FakeCrudListRequest, ClaimsPrincipal, CancellationToken>(MockListQueryAsync);

                using (var instance = new FakeCrudController(
                    authorizationService.Object,
                    logger.Object,
                    mediator.Object,
                    auditableCommandFactory.Object,
                    auditableQueryFactory.Object)
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
                    var result = await instance.GetAsync(null, CancellationToken.None).ConfigureAwait(false);
                    Assert.NotNull(result);
                    Assert.IsType<OkObjectResult>(result);
                }
            }

            /// <summary>
            /// Unit tests to ensure HTTP BAD REQUEST.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsBadRequestAsync()
            {
                var authorizationService = MockAuthorizationServiceFactory();
                authorizationService.Setup(s =>
                    s.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), null, "listPolicyName"))
                    .Returns(async () => await Task.FromResult(AuthorizationResult.Success()).ConfigureAwait(false));

                var logger = MockLoggerFactory();

                var mediator = MockMediatorFactory();
                mediator.Setup(s => s.Send(It.IsAny<FakeCrudListQuery>(), It.IsAny<CancellationToken>())).Returns<FakeCrudListQuery, CancellationToken>(MockListMediatorHandlerAsync);

                var auditableCommandFactory = MockCommandFactory();

                var auditableQueryFactory = MockQueryFactory();
                auditableQueryFactory.Setup(s =>
                        s.GetListQueryAsync(
                            It.IsAny<FakeCrudListRequest>(),
                            It.IsAny<ClaimsPrincipal>(),
                            It.IsAny<CancellationToken>()))
                    .Returns<FakeCrudListRequest, ClaimsPrincipal, CancellationToken>(MockListQueryAsync);

                using (var instance = new FakeCrudController(
                    authorizationService.Object,
                    logger.Object,
                    mediator.Object,
                    auditableCommandFactory.Object,
                    auditableQueryFactory.Object)
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
                    var result = await instance.GetAsync(null, CancellationToken.None).ConfigureAwait(false);
                    Assert.NotNull(result);
                    Assert.IsType<BadRequestResult>(result);
                }
            }

            private static Task<IList<int>> MockListMediatorHandlerAsync(FakeCrudListQuery auditableRequest, CancellationToken cancellationToken)
            {
                return Task.FromResult(new List<int> { 1, 2, 3 } as IList<int>);
            }

            private static Task<FakeCrudListQuery> MockListQueryAsync(FakeCrudListRequest requestDto, ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
            {
                return Task.FromResult(new FakeCrudListQuery(requestDto, claimsPrincipal));
            }
        }

        /// <summary>
        /// Unit Tests for the PUT request.
        /// </summary>
        public sealed class PutAsyncMethod : Foundatio.Logging.Xunit.TestWithLoggingBase
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
                authorizationService.Setup(s =>
                    s.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>(), "updatePolicyName"))
                    .Returns(async () => await Task.FromResult(AuthorizationResult.Success()).ConfigureAwait(false));

                var logger = MockLoggerFactory();

                var mediator = MockMediatorFactory();
                mediator.Setup(s => s.Send(It.IsAny<FakeCrudUpdateCommand>(), It.IsAny<CancellationToken>())).Returns<FakeCrudUpdateCommand, CancellationToken>(MockUpdateMediatorHandlerAsync);

                var auditableCommandFactory = MockCommandFactory();
                auditableCommandFactory.Setup(s =>
                    s.GetUpdateCommandAsync(
                        It.IsAny<int>(),
                        It.IsAny<ClaimsPrincipal>(),
                        It.IsAny<CancellationToken>()))
                    .Returns<int, ClaimsPrincipal, CancellationToken>(MockUpdateCommandAsync);

                var auditableQueryFactory = MockQueryFactory();

                using (var instance = new FakeCrudController(
                    authorizationService.Object,
                    logger.Object,
                    mediator.Object,
                    auditableCommandFactory.Object,
                    auditableQueryFactory.Object)
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
                    var result = await instance.PutAsync(1, updateRequestDto, CancellationToken.None).ConfigureAwait(false);
                    Assert.NotNull(result);
                    Assert.IsType<OkObjectResult>(result);
                }
            }

            /// <summary>
            /// Unit tests to ensure HTTP BAD REQUEST.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsBadRequestAsync()
            {
                var authorizationService = MockAuthorizationServiceFactory();
                authorizationService.Setup(s =>
                    s.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>(), "updatePolicyName"))
                    .Returns(async () => await Task.FromResult(AuthorizationResult.Success()).ConfigureAwait(false));

                var logger = MockLoggerFactory();

                var mediator = MockMediatorFactory();
                mediator.Setup(s => s.Send(It.IsAny<FakeCrudUpdateCommand>(), It.IsAny<CancellationToken>())).Returns<FakeCrudUpdateCommand, CancellationToken>(MockUpdateMediatorHandlerAsync);

                var auditableCommandFactory = MockCommandFactory();
                auditableCommandFactory.Setup(s =>
                    s.GetUpdateCommandAsync(
                        It.IsAny<int>(),
                        It.IsAny<ClaimsPrincipal>(),
                        It.IsAny<CancellationToken>()))
                    .Returns<int, ClaimsPrincipal, CancellationToken>(MockUpdateCommandAsync);

                var auditableQueryFactory = MockQueryFactory();

                using (var instance = new FakeCrudController(
                    authorizationService.Object,
                    logger.Object,
                    mediator.Object,
                    auditableCommandFactory.Object,
                    auditableQueryFactory.Object)
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
                    var result = await instance.PutAsync(1, 1, CancellationToken.None).ConfigureAwait(false);
                    Assert.NotNull(result);
                    Assert.IsType<BadRequestResult>(result);
                }
            }

            private static Task<FakeCrudUpdateResponse> MockUpdateMediatorHandlerAsync(FakeCrudUpdateCommand arg1, CancellationToken arg2)
            {
                return Task.FromResult(new FakeCrudUpdateResponse());
            }

            private static Task<FakeCrudUpdateCommand> MockUpdateCommandAsync(int requestDto, ClaimsPrincipal claimsPrincipal, CancellationToken arg3)
            {
                return Task.FromResult(new FakeCrudUpdateCommand(requestDto, claimsPrincipal));
            }
        }

        /// <summary>
        /// Unit Tests for the View call.
        /// </summary>
        public sealed class ViewAsyncMethod : Foundatio.Logging.Xunit.TestWithLoggingBase
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
                authorizationService.Setup(s =>
                    s.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<object>(), "viewPolicyName"))
                    .Returns(async () => await Task.FromResult(AuthorizationResult.Success()).ConfigureAwait(false));
                authorizationService.Setup(s =>
                        s.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), null, "viewPolicyName"))
                    .Returns(async () => await Task.FromResult(AuthorizationResult.Success()).ConfigureAwait(false));

                var logger = MockLoggerFactory();

                var mediator = MockMediatorFactory();
                mediator.Setup(s => s.Send(It.IsAny<FakeCrudViewQuery>(), It.IsAny<CancellationToken>())).Returns<FakeCrudViewQuery, CancellationToken>(MockViewMediatorHandlerAsync);

                var auditableCommandFactory = MockCommandFactory();

                var auditableQueryFactory = MockQueryFactory();
                auditableQueryFactory.Setup(s =>
                        s.GetViewQueryAsync(
                            It.IsAny<long>(),
                            It.IsAny<ClaimsPrincipal>(),
                            It.IsAny<CancellationToken>()))
                    .Returns<long, ClaimsPrincipal, CancellationToken>(MockViewQueryAsync);

                using (var instance = new FakeCrudController(
                    authorizationService.Object,
                    logger.Object,
                    mediator.Object,
                    auditableCommandFactory.Object,
                    auditableQueryFactory.Object)
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
                    var result = await instance.GetAsync(listRequest, CancellationToken.None).ConfigureAwait(false);
                    Assert.NotNull(result);
                    Assert.IsType(expectedResultType, result);
                }
            }

            private static Task<FakeCrudViewResponse> MockViewMediatorHandlerAsync(FakeCrudViewQuery auditableRequest, CancellationToken cancellationToken)
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
