using System;
using System.Collections.Generic;
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
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Logging;
using Moq;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Xunit;

namespace Dhgms.AspNetCoreContrib.UnitTests.Controllers
{
    /*
    public abstract class UnitTestAsyncMethod<TArg1, TArg2, TArg3>
    {
        public abstract static object[][] ThrowsArgumentNullExceptionAsyncTestData { get; }

        protected abstract Func<TArg1, TArg2, TArg3, Task> GetMethod();

        [MemberData(nameof(ThrowsArgumentNullExceptionAsyncTestData))]
        [Theory]
        public async Task ThrowsArgumentNullExceptionAsync(
            TArg1 arg1,
            TArg2 arg2,
            TArg3 args3,
            string argumentNullExceptionParamName)
        {
            var method = GetMethod();
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => method(arg1, arg2, args3));
            Assert.Equal(argumentNullExceptionParamName, exception.ParamName);
        }
    }
    */

    public static class CrudControllerTests
    {
        private static Mock<IAuthorizationService> MockAuthorizationServiceFactory() => new Mock<IAuthorizationService>(MockBehavior.Strict);
        private static Mock<ILogger<FakeCrudController>> MockLoggerFactory() => new Mock<ILogger<FakeCrudController>>(MockBehavior.Strict);
        private static Mock<IMediator> MockMediatorFactory() => new Mock<IMediator>(MockBehavior.Strict);
        private static Mock<IAuditableCommandFactory<int, int, int, int, int>> MockCommandFactory() => new Mock<IAuditableCommandFactory<int, int, int, int, int>>(MockBehavior.Strict);
        private static Mock<IAuditableQueryFactory<int, int, int>> MockQueryFactory() => new Mock<IAuditableQueryFactory<int, int, int>>(MockBehavior.Strict);

        public sealed class ConstructorMethod
        {
            public static IEnumerable<object[]> ThrowsArgumentNullExceptionTestData = new []
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
                    (Mock<IAuditableCommandFactory<int, int, int, int, int>>)null,
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
                    (Mock<IAuditableQueryFactory<int, int, int>>)null,
                    "queryFactory"
                };
            }

            [Theory]
            [MemberData(nameof(ThrowsArgumentNullExceptionTestData))]
            public void ThrowsArgumentNullException(
                Mock<IAuthorizationService> authorizationService,
                Mock<ILogger<FakeCrudController>> logger,
                Mock<IMediator> mediator,
                Mock<IAuditableCommandFactory<int, int, int, int, int>> auditableCommandFactory,
                Mock<IAuditableQueryFactory<int, int, int>> auditableQueryFactory,
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
            public static IEnumerable<object[]> ShouldSucceedTestData = new []
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
                logger.Setup(s => s.Log(
                    LogLevel.Debug,
                    It.IsAny<EventId>(),
                    It.IsAny<object>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<object, Exception, string>>()));

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
                            User = new ClaimsPrincipal(new HttpListenerBasicIdentity("user", "pass"))
                        }
                    }
                };


                var result = await instance.AddAsync(addRequestDto, CancellationToken.None);
                Assert.NotNull(result);

                //authorizationService.VerifyNoOtherCalls();
                //logger.VerifyNoOtherCalls();
                //mediator.VerifyNoOtherCalls();
                //auditableCommandFactory.VerifyNoOtherCalls();
                //auditableQueryFactory.VerifyNoOtherCalls();
            }

            private async Task<int> MockAddMediatorHandler(IAuditableRequest<int, int> auditableRequest, CancellationToken cancellationToken)
            {
                return await Task.FromResult(auditableRequest.RequestDto);
            }

            private async Task<IAuditableRequest<int, int>> MockAddCommand(int requestDto, ClaimsPrincipal claimsPrincipal, CancellationToken cancellationToken)
            {
                return await Task.FromResult(new AuditableRequest<int, int>(requestDto, claimsPrincipal));
            }
        }

        public sealed class DeleteAsyncMethod
        {
            public static IEnumerable<object[]> ShouldSucceedTestData = new []
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
                    s.AuthorizeAsync(It.IsAny<ClaimsPrincipal>(), It.IsAny<int>(), "addPolicyName"))
                    .Returns(async () => await Task.FromResult(AuthorizationResult.Success()));

                var logger = MockLoggerFactory();
                logger.Setup(s => s.Log(
                    LogLevel.Debug,
                    It.IsAny<EventId>(),
                    It.IsAny<object>(),
                    It.IsAny<Exception>(),
                    It.IsAny<Func<object, Exception, string>>()));

                var mediator = MockMediatorFactory();
                mediator.Setup(s => s.Send(It.IsAny<IAuditableRequest<long, int>>(), It.IsAny<CancellationToken>())).Returns<IAuditableRequest<long, int>, CancellationToken>(MockDeleteMediatorHandler);

                var auditableCommandFactory = MockCommandFactory();
                auditableCommandFactory.Setup(s =>
                    s.GetDeleteCommandAsync(
                        It.IsAny<int>(),
                        It.IsAny<ClaimsPrincipal>(),
                        It.IsAny<CancellationToken>()))
                    .Returns<int, ClaimsPrincipal, CancellationToken>(MockDeleteCommand);

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
                            User = new ClaimsPrincipal(new HttpListenerBasicIdentity("user", "pass"))
                        }
                    }
                };

                var result = await instance.DeleteAsync(id, CancellationToken.None);
                Assert.NotNull(result);

                //authorizationService.VerifyNoOtherCalls();
                //logger.VerifyNoOtherCalls();
                //mediator.VerifyNoOtherCalls();
                //auditableCommandFactory.VerifyNoOtherCalls();
                //auditableQueryFactory.VerifyNoOtherCalls();
            }

            private Task<int> MockDeleteMediatorHandler(IAuditableRequest<long, int> arg1, CancellationToken arg2)
            {
                throw new NotImplementedException();
            }

            private async Task<IAuditableRequest<long, int>> MockDeleteCommand(int requestDto, ClaimsPrincipal claimsPrincipal, CancellationToken arg3)
            {
                return await Task.FromResult(new FakeCrudDeleteRequest(requestDto, claimsPrincipal));
            }
        }

        public sealed class UpdateAsyncMethod
        {
            public static IEnumerable<object[]> ThrowsArgumentNullExceptionTestData = new []
            {
                new object[] { 0, },
                new object[] { -1, },
            };

            [Theory]
            [MemberData(nameof(ThrowsArgumentNullExceptionTestData))]
            public async Task ThrowsArgumentNullExceptionAsync(int updateRequestDto)
            {
                var authorizationService = MockAuthorizationServiceFactory();
                var logger = MockLoggerFactory();
                var mediator = MockMediatorFactory();
                var auditableCommandFactory = MockCommandFactory();
                var auditableQueryFactory = MockQueryFactory();

                var instance = new FakeCrudController(
                    authorizationService.Object,
                    logger.Object,
                    mediator.Object,
                    auditableCommandFactory.Object,
                    auditableQueryFactory.Object);

                var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => instance.UpdateAsync(updateRequestDto, CancellationToken.None));
                Assert.Equal("updateRequestDto", ex.ParamName);

                authorizationService.VerifyNoOtherCalls();
                logger.VerifyNoOtherCalls();
                mediator.VerifyNoOtherCalls();
                auditableCommandFactory.VerifyNoOtherCalls();
                auditableQueryFactory.VerifyNoOtherCalls();
            }
        }
    }
}
