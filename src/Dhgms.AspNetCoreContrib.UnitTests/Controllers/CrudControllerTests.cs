using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dhgms.AspNetCoreContrib.Abstractions;
using Dhgms.AspNetCoreContrib.Controllers;
using Dhgms.AspNetCoreContrib.Fakes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Logging;
using Moq;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Xunit;

namespace Dhgms.AspNetCoreContrib.UnitTests.Controllers
{
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
        private static Mock<IAuditableCommandFactory<FakeCrudAddCommand,int, int, FakeCrudDeleteCommand, long, FakeCrudUpdateCommand, int, int>> MockCommandFactory() => new Mock<IAuditableCommandFactory<FakeCrudAddCommand,int, int, FakeCrudDeleteCommand, long, FakeCrudUpdateCommand, int, int>>(MockBehavior.Strict);
        private static Mock<IAuditableQueryFactory<FakeCrudListQuery, FakeCrudListRequest, IList<int>, FakeCrudViewQuery, long>> MockQueryFactory() => new Mock<IAuditableQueryFactory<FakeCrudListQuery, FakeCrudListRequest, IList<int>, FakeCrudViewQuery, long>>(MockBehavior.Strict);

        public sealed class ConstructorMethod
        {
            public static readonly IEnumerable<object[]> ThrowsArgumentNullExceptionTestData = new []
            {
                GetAuthorizationServiceNullTestData(),
                GetLoggerNullTestData(),
                GetMediatorNullTestData(),
                GetAuditableCommandFactoryNullTestData(),
                GetAuditableQueryFactoryNullTestData(),
            };

            private static object[] GetAuthorizationServiceNullTestData()
            {
                return new object[]
                {
                    (Mock<IAuthorizationService>)null,
                    MockLoggerFactory(),
                    MockMediatorFactory(),
                    MockCommandFactory(),
                    MockQueryFactory(),
                    "authorizationService"
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
                    "logger"
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
                    "mediator"
                };
            }

            private static object[] GetAuditableCommandFactoryNullTestData()
            {
                return new object[]
                {
                    MockAuthorizationServiceFactory(),
                    MockLoggerFactory(),
                    MockMediatorFactory(),
                    (Mock<IAuditableCommandFactory<FakeCrudAddCommand,int, int, FakeCrudDeleteCommand, long, FakeCrudUpdateCommand, int, int>>)null,
                    MockQueryFactory(),
                    "commandFactory"
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
                    (Mock<IAuditableQueryFactory<FakeCrudListQuery, FakeCrudListRequest, IList<int>, FakeCrudViewQuery, long>>)null,
                    "queryFactory"
                };
            }

            [Theory]
            [MemberData(nameof(ThrowsArgumentNullExceptionTestData))]
            public void ThrowsArgumentNullException(
                Mock<IAuthorizationService> authorizationService,
                Mock<ILogger<FakeCrudController>> logger,
                Mock<IMediator> mediator,
                Mock<IAuditableCommandFactory<FakeCrudAddCommand,int, int, FakeCrudDeleteCommand, long, FakeCrudUpdateCommand, int, int>> auditableCommandFactory,
                Mock<IAuditableQueryFactory<FakeCrudListQuery, FakeCrudListRequest, IList<int>, FakeCrudViewQuery, long>> auditableQueryFactory,
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

            [Fact]
            public void ReturnsInstance()
            {
                var mockAuthorizationService = MockAuthorizationServiceFactory();
                var mockLogger = MockLoggerFactory();
                var mockMediator = MockMediatorFactory();
                var mockCommandFactory = MockCommandFactory();
                var mockQueryFactory = MockQueryFactory();

                var instance = new FakeCrudController(
                    mockAuthorizationService.Object,
                    mockLogger.Object,
                    mockMediator.Object,
                    mockCommandFactory.Object,
                    mockQueryFactory.Object);

                Assert.NotNull(instance);
                mockAuthorizationService.VerifyNoOtherCalls();
                mockLogger.VerifyNoOtherCalls();
                mockMediator.VerifyNoOtherCalls();
                mockCommandFactory.VerifyNoOtherCalls();
                mockQueryFactory.VerifyNoOtherCalls();
            }
        }

        public sealed class AddAsyncMethod
        {
            public static readonly IEnumerable<object[]> ShouldSucceedTestData = new []
            {
                new object[] { 0, },
                new object[] { -1, },
            };

            [Theory]
            [MemberData(nameof(ShouldSucceedTestData))]
            public async Task ShouldSucceed(int addRequestDto)
            {
                var authorizationService = MockAuthorizationServiceFactory();
                authorizationService.Setup(s =>
                    s.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>(), "addPolicyName"))
                    .Returns(async () => await Task.FromResult(AuthorizationResult.Success()));

                var logger = MockLoggerFactory();

                var mediator = MockMediatorFactory();
                mediator.Setup(s => s.Send(It.IsAny<IAuditableRequest<int, int>>(), It.IsAny<CancellationToken>())).Returns<IAuditableRequest<int, int>, CancellationToken>(MockAddMediatorHandler);

                var auditableCommandFactory = MockCommandFactory();
                auditableCommandFactory.Setup(s =>
                    s.GetAddCommandAsync(
                        It.IsAny<int>(),
                        It.IsAny<ClaimsPrincipal>(),
                        It.IsAny<CancellationToken>()))
                    .Returns<int, ClaimsPrincipal, CancellationToken>(MockAddCommand);

                var auditableQueryFactory = MockQueryFactory();

                var instance = new FakeCrudController(
                    authorizationService.Object,
                    logger.Object,
                    mediator.Object,
                    auditableCommandFactory.Object,
                    auditableQueryFactory.Object)
                {
                    ControllerContext = new ControllerContext
                    {
                        HttpContext = new DefaultHttpContext()
                        {
                            Request = { IsHttps = true },
                            User = new ClaimsPrincipal(new HttpListenerBasicIdentity("user", "pass"))
                        }
                    }
                };


                var result = await instance.AddAsync(addRequestDto, CancellationToken.None);
                Assert.NotNull(result);
            }

            [Fact]
            public async Task ReturnsBadRequest()
            {
                var authorizationService = MockAuthorizationServiceFactory();
                authorizationService.Setup(s =>
                    s.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>(), "addPolicyName"))
                    .Returns(async () => await Task.FromResult(AuthorizationResult.Success()));

                var logger = MockLoggerFactory();

                var mediator = MockMediatorFactory();
                mediator.Setup(s => s.Send(It.IsAny<IAuditableRequest<int, int>>(), It.IsAny<CancellationToken>())).Returns<IAuditableRequest<int, int>, CancellationToken>(MockAddMediatorHandler);

                var auditableCommandFactory = MockCommandFactory();
                auditableCommandFactory.Setup(s =>
                    s.GetAddCommandAsync(
                        It.IsAny<int>(),
                        It.IsAny<ClaimsPrincipal>(),
                        It.IsAny<CancellationToken>()))
                    .Returns<int, ClaimsPrincipal, CancellationToken>(MockAddCommand);

                var auditableQueryFactory = MockQueryFactory();

                var instance = new FakeCrudController(
                    authorizationService.Object,
                    logger.Object,
                    mediator.Object,
                    auditableCommandFactory.Object,
                    auditableQueryFactory.Object)
                {
                    ControllerContext = new ControllerContext
                    {
                        HttpContext = new DefaultHttpContext()
                        {
                            Request = { IsHttps = false },
                            User = new ClaimsPrincipal(new HttpListenerBasicIdentity("user", "pass"))
                        }
                    }
                };


                var result = await instance.AddAsync(1, CancellationToken.None);
                Assert.NotNull(result);
                Assert.IsType<BadRequestResult>(result);
            }


            private async Task<int> MockAddMediatorHandler(IAuditableRequest<int, int> auditableRequest, CancellationToken cancellationToken)
            {
                return await Task.FromResult(auditableRequest.RequestDto);
            }

            private async Task<FakeCrudAddCommand> MockAddCommand(int requestDto, ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
            {
                return await Task.FromResult(new FakeCrudAddCommand(requestDto, claimsPrincipal));
            }
        }

        public sealed class DeleteAsyncMethod
        {
            public static readonly IEnumerable<object[]> ShouldSucceedTestData = new []
            {
                new object[] { 0, },
                new object[] { -1, },
            };

            [Theory]
            [MemberData(nameof(ShouldSucceedTestData))]
            public async Task ShouldSucceed(int id)
            {
                var authorizationService = MockAuthorizationServiceFactory();
                authorizationService.Setup(s =>
                    s.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>(), "deletePolicyName"))
                    .Returns(async () => await Task.FromResult(AuthorizationResult.Success()));

                var logger = MockLoggerFactory();

                var mediator = MockMediatorFactory();
                mediator.Setup(s => s.Send(It.IsAny<IAuditableRequest<long, long>>(), It.IsAny<CancellationToken>())).Returns<IAuditableRequest<long, long>, CancellationToken>(MockDeleteMediatorHandler);

                var auditableCommandFactory = MockCommandFactory();
                auditableCommandFactory.Setup(s =>
                    s.GetDeleteCommandAsync(
                        It.IsAny<long>(),
                        It.IsAny<ClaimsPrincipal>(),
                        It.IsAny<CancellationToken>()))
                    .Returns<long, ClaimsPrincipal, CancellationToken>(MockDeleteCommand);

                var auditableQueryFactory = MockQueryFactory();

                var instance = new FakeCrudController(
                    authorizationService.Object,
                    logger.Object,
                    mediator.Object,
                    auditableCommandFactory.Object,
                    auditableQueryFactory.Object)
                {
                    ControllerContext = new ControllerContext
                    {
                        HttpContext = new DefaultHttpContext()
                        {
                            Request = { IsHttps = true },
                            User = new ClaimsPrincipal(new HttpListenerBasicIdentity("user", "pass"))
                        }
                    }
                };

                var result = await instance.DeleteAsync(id, CancellationToken.None);
                Assert.NotNull(result);
            }

            [Fact]
            public async Task ReturnsBadRequest()
            {
                var authorizationService = MockAuthorizationServiceFactory();
                authorizationService.Setup(s =>
                    s.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>(), "deletePolicyName"))
                    .Returns(async () => await Task.FromResult(AuthorizationResult.Success()));

                var logger = MockLoggerFactory();

                var mediator = MockMediatorFactory();
                mediator.Setup(s => s.Send(It.IsAny<IAuditableRequest<long, long>>(), It.IsAny<CancellationToken>())).Returns<IAuditableRequest<long, long>, CancellationToken>(MockDeleteMediatorHandler);

                var auditableCommandFactory = MockCommandFactory();
                auditableCommandFactory.Setup(s =>
                    s.GetDeleteCommandAsync(
                        It.IsAny<long>(),
                        It.IsAny<ClaimsPrincipal>(),
                        It.IsAny<CancellationToken>()))
                    .Returns<long, ClaimsPrincipal, CancellationToken>(MockDeleteCommand);

                var auditableQueryFactory = MockQueryFactory();

                var instance = new FakeCrudController(
                    authorizationService.Object,
                    logger.Object,
                    mediator.Object,
                    auditableCommandFactory.Object,
                    auditableQueryFactory.Object)
                {
                    ControllerContext = new ControllerContext
                    {
                        HttpContext = new DefaultHttpContext()
                        {
                            Request = { IsHttps = false },
                            User = new ClaimsPrincipal(new HttpListenerBasicIdentity("user", "pass"))
                        }
                    }
                };

                var result = await instance.DeleteAsync(1, CancellationToken.None);
                Assert.NotNull(result);
            }

            private async Task<long> MockDeleteMediatorHandler(IAuditableRequest<long, long> arg1, CancellationToken arg2)
            {
                return await Task.FromResult(arg1.RequestDto);
            }

            private async Task<FakeCrudDeleteCommand> MockDeleteCommand(long requestDto, ClaimsPrincipal claimsPrincipal, CancellationToken arg3)
            {
                return await Task.FromResult(new FakeCrudDeleteCommand(requestDto, claimsPrincipal));
            }
        }

        public sealed class ListAsyncMethod
        {
            public static readonly IEnumerable<object[]> ShouldSucceedTestData = new []
            {
                new object[]
                {
                    new FakeCrudListRequest()
                }
            };

            [Theory]
            [MemberData(nameof(ShouldSucceedTestData))]
            public async Task ShouldSucceed(FakeCrudListRequest listRequest)
            {
                var authorizationService = MockAuthorizationServiceFactory();
                authorizationService.Setup(s =>
                    s.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), null, "listPolicyName"))
                    .Returns(async () => await Task.FromResult(AuthorizationResult.Success()));

                var logger = MockLoggerFactory();

                var mediator = MockMediatorFactory();
                mediator.Setup(s => s.Send(It.IsAny<FakeCrudListQuery>(), It.IsAny<CancellationToken>())).Returns<FakeCrudListQuery, CancellationToken>(MockListMediatorHandler);

                var auditableCommandFactory = MockCommandFactory();

                var auditableQueryFactory = MockQueryFactory();
                auditableQueryFactory.Setup(s =>
                        s.GetListQueryAsync(
                            It.IsAny<FakeCrudListRequest>(),
                            It.IsAny<ClaimsPrincipal>(),
                            It.IsAny<CancellationToken>()))
                    .Returns<FakeCrudListRequest, ClaimsPrincipal, CancellationToken>(MockListQuery);

                var instance = new FakeCrudController(
                    authorizationService.Object,
                    logger.Object,
                    mediator.Object,
                    auditableCommandFactory.Object,
                    auditableQueryFactory.Object)
                {
                    ControllerContext = new ControllerContext
                    {
                        HttpContext = new DefaultHttpContext()
                        {
                            User = new ClaimsPrincipal(new HttpListenerBasicIdentity("user", "pass")),
                            Request =
                            {
                                IsHttps = true,
                                Query = new QueryCollection()
                            }
                        }
                    }

                };

                var result = await instance.IndexAsync(null, CancellationToken.None);
                Assert.NotNull(result);
            }

            [Fact]
            public async Task ReturnsBadRequest()
            {
                var authorizationService = MockAuthorizationServiceFactory();
                authorizationService.Setup(s =>
                    s.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), null, "listPolicyName"))
                    .Returns(async () => await Task.FromResult(AuthorizationResult.Success()));

                var logger = MockLoggerFactory();

                var mediator = MockMediatorFactory();
                mediator.Setup(s => s.Send(It.IsAny<FakeCrudListQuery>(), It.IsAny<CancellationToken>())).Returns<FakeCrudListQuery, CancellationToken>(MockListMediatorHandler);

                var auditableCommandFactory = MockCommandFactory();

                var auditableQueryFactory = MockQueryFactory();
                auditableQueryFactory.Setup(s =>
                        s.GetListQueryAsync(
                            It.IsAny<FakeCrudListRequest>(),
                            It.IsAny<ClaimsPrincipal>(),
                            It.IsAny<CancellationToken>()))
                    .Returns<FakeCrudListRequest, ClaimsPrincipal, CancellationToken>(MockListQuery);

                var instance = new FakeCrudController(
                    authorizationService.Object,
                    logger.Object,
                    mediator.Object,
                    auditableCommandFactory.Object,
                    auditableQueryFactory.Object)
                {
                    ControllerContext = new ControllerContext
                    {
                        HttpContext = new DefaultHttpContext()
                        {
                            User = new ClaimsPrincipal(new HttpListenerBasicIdentity("user", "pass")),
                            Request =
                            {
                                IsHttps = false,
                                Query = new QueryCollection()
                            }
                        }
                    }

                };

                var result = await instance.IndexAsync(null, CancellationToken.None);
                Assert.NotNull(result);
            }

            private async Task<IList<int>> MockListMediatorHandler(FakeCrudListQuery auditableRequest, CancellationToken cancellationToken)
            {
                return await Task.FromResult(new int[] {1,2,3});
            }

            private async Task<FakeCrudListQuery> MockListQuery(FakeCrudListRequest requestDto, ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
            {
                return await Task.FromResult(new FakeCrudListQuery(requestDto, claimsPrincipal));
            }
        }

        public sealed class UpdateAsyncMethod
        {
            public static readonly IEnumerable<object[]> ThrowsArgumentNullExceptionTestData = new []
            {
                new object[] { 0, },
                new object[] { -1, },
            };

            [Theory]
            [MemberData(nameof(ThrowsArgumentNullExceptionTestData))]
            public async Task ShouldSucceed(int updateRequestDto)
            {
                var authorizationService = MockAuthorizationServiceFactory();
                authorizationService.Setup(s =>
                    s.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>(), "updatePolicyName"))
                    .Returns(async () => await Task.FromResult(AuthorizationResult.Success()));

                var logger = MockLoggerFactory();

                var mediator = MockMediatorFactory();
                mediator.Setup(s => s.Send(It.IsAny<FakeCrudUpdateCommand>(), It.IsAny<CancellationToken>())).Returns<FakeCrudUpdateCommand, CancellationToken>(MockUpdateMediatorHandler);

                var auditableCommandFactory = MockCommandFactory();
                auditableCommandFactory.Setup(s =>
                    s.GetUpdateCommandAsync(
                        It.IsAny<int>(),
                        It.IsAny<ClaimsPrincipal>(),
                        It.IsAny<CancellationToken>()))
                    .Returns<int, ClaimsPrincipal, CancellationToken>(MockUpdateCommand);

                var auditableQueryFactory = MockQueryFactory();

                var instance = new FakeCrudController(
                    authorizationService.Object,
                    logger.Object,
                    mediator.Object,
                    auditableCommandFactory.Object,
                    auditableQueryFactory.Object)
                {
                    ControllerContext = new ControllerContext
                    {
                        HttpContext = new DefaultHttpContext()
                        {
                            Request = { IsHttps = true },
                            User = new ClaimsPrincipal(new HttpListenerBasicIdentity("user", "pass"))
                        }
                    }
                };

                var result = await instance.UpdateAsync(1, updateRequestDto, CancellationToken.None);
                Assert.NotNull(result);
            }

            [Fact]
            public async Task ReturnsBadRequest()
            {
                var authorizationService = MockAuthorizationServiceFactory();
                authorizationService.Setup(s =>
                    s.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>(), "updatePolicyName"))
                    .Returns(async () => await Task.FromResult(AuthorizationResult.Success()));

                var logger = MockLoggerFactory();

                var mediator = MockMediatorFactory();
                mediator.Setup(s => s.Send(It.IsAny<FakeCrudUpdateCommand>(), It.IsAny<CancellationToken>())).Returns<FakeCrudUpdateCommand, CancellationToken>(MockUpdateMediatorHandler);

                var auditableCommandFactory = MockCommandFactory();
                auditableCommandFactory.Setup(s =>
                    s.GetUpdateCommandAsync(
                        It.IsAny<int>(),
                        It.IsAny<ClaimsPrincipal>(),
                        It.IsAny<CancellationToken>()))
                    .Returns<int, ClaimsPrincipal, CancellationToken>(MockUpdateCommand);

                var auditableQueryFactory = MockQueryFactory();

                var instance = new FakeCrudController(
                    authorizationService.Object,
                    logger.Object,
                    mediator.Object,
                    auditableCommandFactory.Object,
                    auditableQueryFactory.Object)
                {
                    ControllerContext = new ControllerContext
                    {
                        HttpContext = new DefaultHttpContext()
                        {
                            Request = { IsHttps = false },
                            User = new ClaimsPrincipal(new HttpListenerBasicIdentity("user", "pass"))
                        }
                    }
                };

                var result = await instance.UpdateAsync(1, 1, CancellationToken.None);
                Assert.NotNull(result);
            }

            private async Task<int> MockUpdateMediatorHandler(FakeCrudUpdateCommand arg1, CancellationToken arg2)
            {
                return await Task.FromResult(arg1.RequestDto);
            }

            private async Task<FakeCrudUpdateCommand> MockUpdateCommand(int requestDto, ClaimsPrincipal claimsPrincipal, CancellationToken arg3)
            {
                return await Task.FromResult(new FakeCrudUpdateCommand(requestDto, claimsPrincipal));
            }
        }

        public sealed class ViewAsyncMethod
        {
            public static readonly IEnumerable<object[]> ShouldSucceedTestData = new []
            {
                new object[]
                {
                    -1
                },
                new object[]
                {
                    1
                }
            };

            [Theory]
            [MemberData(nameof(ShouldSucceedTestData))]
            public async Task ShouldSucceed(long listRequest)
            {
                var authorizationService = MockAuthorizationServiceFactory();
                authorizationService.Setup(s =>
                    s.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<long>(), "viewPolicyName"))
                    .Returns(async () => await Task.FromResult(AuthorizationResult.Success()));
                authorizationService.Setup(s =>
                        s.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), null, "viewPolicyName"))
                    .Returns(async () => await Task.FromResult(AuthorizationResult.Success()));

                var logger = MockLoggerFactory();

                var mediator = MockMediatorFactory();
                mediator.Setup(s => s.Send(It.IsAny<FakeCrudViewQuery>(), It.IsAny<CancellationToken>())).Returns<FakeCrudViewQuery, CancellationToken>(MockViewMediatorHandler);

                var auditableCommandFactory = MockCommandFactory();

                var auditableQueryFactory = MockQueryFactory();
                auditableQueryFactory.Setup(s =>
                        s.GetViewQueryAsync(
                            It.IsAny<long>(),
                            It.IsAny<ClaimsPrincipal>(),
                            It.IsAny<CancellationToken>()))
                    .Returns<long, ClaimsPrincipal, CancellationToken>(MockViewQuery);

                var instance = new FakeCrudController(
                    authorizationService.Object,
                    logger.Object,
                    mediator.Object,
                    auditableCommandFactory.Object,
                    auditableQueryFactory.Object)
                {
                    ControllerContext = new ControllerContext
                    {
                        HttpContext = new DefaultHttpContext()
                        {
                            Request = { IsHttps = true},
                            User = new ClaimsPrincipal(new HttpListenerBasicIdentity("user", "pass"))
                        }
                    }
                };


                var result = await instance.IndexAsync(listRequest, CancellationToken.None);
                Assert.NotNull(result);
            }

            private async Task<long> MockViewMediatorHandler(FakeCrudViewQuery auditableRequest, CancellationToken cancellationToken)
            {
                return await Task.FromResult(auditableRequest.RequestDto);
            }

            private async Task<FakeCrudViewQuery> MockViewQuery(long requestDto, ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
            {
                return await Task.FromResult(new FakeCrudViewQuery(requestDto, claimsPrincipal));
            }
        }
    }
}
