using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Dhgms.AspNetCoreContrib.Abstractions;
using Dhgms.AspNetCoreContrib.Controllers;
using Dhgms.AspNetCoreContrib.Fakes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
        private static readonly Mock<IAuthorizationService> MockAuthorizationService = new Mock<IAuthorizationService>(MockBehavior.Strict);
        private static readonly Mock<ILogger<FakeCrudController>> MockLogger = new Mock<ILogger<FakeCrudController>>(MockBehavior.Strict);
        private static readonly Mock<IMediator> MockMediator = new Mock<IMediator>(MockBehavior.Strict);
        private static readonly Mock<IAuditableCommandFactory<int, int, int, int, int>> MockCommandFactory = new Mock<IAuditableCommandFactory<int, int, int, int, int>>(MockBehavior.Strict);
        private static readonly Mock<IAuditableQueryFactory<int, int, int>> MockQueryFactory = new Mock<IAuditableQueryFactory<int, int, int>>(MockBehavior.Strict);

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
                    MockLogger,
                    MockMediator,
                    MockCommandFactory,
                    MockQueryFactory,
                    "authorizationService"
                };
            }

            private static object[] GetLoggerNullTestData()
            {
                return new object[]
                {
                    MockAuthorizationService,
                    (Mock<ILogger<FakeCrudController>>)null,
                    MockMediator,
                    MockCommandFactory,
                    MockQueryFactory,
                    "logger"
                };
            }

            private static object[] GetMediatorNullTestData()
            {
                return new object[]
                {
                    MockAuthorizationService,
                    MockLogger,
                    (Mock<IMediator>)null,
                    MockCommandFactory,
                    MockQueryFactory,
                    "mediator"
                };
            }

            private static object[] GetAuditableCommandFactoryNullTestData()
            {
                return new object[]
                {
                    MockAuthorizationService,
                    MockLogger,
                    MockMediator,
                    (Mock<IAuditableCommandFactory<int, int, int, int, int>>)null,
                    MockQueryFactory,
                    "auditableCommandFactory"
                };
            }

            private static object[] GetAuditableQueryFactoryNullTestData()
            {
                return new object[]
                {
                    MockAuthorizationService.Object,
                    MockLogger.Object,
                    MockMediator.Object,
                    MockCommandFactory.Object,
                    (Mock<IAuditableQueryFactory<int, int, int>>)null,
                    "auditableQueryFactory"
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
                    authorizationService.Object,
                    logger.Object,
                    mediator.Object,
                    auditableCommandFactory.Object,
                    auditableQueryFactory.Object));

                Assert.Equal(argumentNullExceptionParameterName, ex.ParamName);

                authorizationService.VerifyNoOtherCalls();
                logger.VerifyNoOtherCalls();
                mediator.VerifyNoOtherCalls();
                auditableCommandFactory.VerifyNoOtherCalls();
                auditableQueryFactory.VerifyNoOtherCalls();
            }
        }

        public sealed class AddAsyncMethod
        {
            public static IEnumerable<object[]> ThrowsArgumentNullExceptionTestData = new []
            {
                new object[] {},
            };

            [Theory]
            [MemberData(nameof(ThrowsArgumentNullExceptionTestData))]
            public async Task ThrowsArgumentNullExceptionAsync(int addRequestDto)
            {
                Mock<IAuthorizationService> authorizationService = MockAuthorizationService;
                Mock<ILogger<FakeCrudController>> logger = MockLogger;
                Mock<IMediator> mediator = MockMediator;
                Mock<IAuditableCommandFactory<int, int, int, int, int>> auditableCommandFactory = MockCommandFactory;
                Mock<IAuditableQueryFactory<int, int, int>> auditableQueryFactory = MockQueryFactory;

                var instance = new FakeCrudController(
                    authorizationService.Object,
                    logger.Object,
                    mediator.Object,
                    auditableCommandFactory.Object,
                    auditableQueryFactory.Object);

                var ex = await Assert.ThrowsAsync<ArgumentNullException>(() => instance.AddAsync(addRequestDto, CancellationToken.None));
                Assert.Equal("addRequestDto", ex.ParamName);

                authorizationService.VerifyNoOtherCalls();
                logger.VerifyNoOtherCalls();
                mediator.VerifyNoOtherCalls();
                auditableCommandFactory.VerifyNoOtherCalls();
                auditableQueryFactory.VerifyNoOtherCalls();
            }
        }

        public sealed class DeleteAsyncMethod
        {
            public static IEnumerable<object[]> ThrowsArgumentOutOfRangeExceptionTestData = new []
            {
                new object[] {},
            };

            [Theory]
            [MemberData(nameof(ThrowsArgumentOutOfRangeExceptionTestData))]
            public async Task ThrowsArgumentOutOfRangeExceptionAsync(int id)
            {
                Mock<IAuthorizationService> authorizationService = MockAuthorizationService;
                Mock<ILogger<FakeCrudController>> logger = MockLogger;
                Mock<IMediator> mediator = MockMediator;
                Mock<IAuditableCommandFactory<int, int, int, int, int>> auditableCommandFactory = MockCommandFactory;
                Mock<IAuditableQueryFactory<int, int, int>> auditableQueryFactory = MockQueryFactory;

                var instance = new FakeCrudController(
                    authorizationService.Object,
                    logger.Object,
                    mediator.Object,
                    auditableCommandFactory.Object,
                    auditableQueryFactory.Object);

                var ex = await Assert.ThrowsAsync<ArgumentOutOfRangeException>(() => instance.DeleteAsync(id, CancellationToken.None));
                Assert.Equal("id", ex.ParamName);

                authorizationService.VerifyNoOtherCalls();
                logger.VerifyNoOtherCalls();
                mediator.VerifyNoOtherCalls();
                auditableCommandFactory.VerifyNoOtherCalls();
                auditableQueryFactory.VerifyNoOtherCalls();
            }
        }

        public sealed class UpdateAsyncMethod
        {
            public static IEnumerable<object[]> ThrowsArgumentNullExceptionTestData = new []
            {
                new object[] {},
            };

            [Theory]
            [MemberData(nameof(ThrowsArgumentNullExceptionTestData))]
            public async Task ThrowsArgumentNullExceptionAsync(int updateRequestDto)
            {
                Mock<IAuthorizationService> authorizationService = MockAuthorizationService;
                Mock<ILogger<FakeCrudController>> logger = MockLogger;
                Mock<IMediator> mediator = MockMediator;
                Mock<IAuditableCommandFactory<int, int, int, int, int>> auditableCommandFactory = MockCommandFactory;
                Mock<IAuditableQueryFactory<int, int, int>> auditableQueryFactory = MockQueryFactory;

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
