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
        public sealed class ConstructorMethod
        {
            public void ThrowsArgumentNullException()
            {
                var mockAuthorizationService = new Mock<IAuthorizationService>(MockBehavior.Strict);
                var mockLogger = new Mock<ILogger<FakeCrudController>>(MockBehavior.Strict);
                var mockMediator = new Mock<IMediator>(MockBehavior.Strict);
                var mockCommandFactory = new Mock<IAuditableCommandFactory<int, int, int, int, int>>(MockBehavior.Strict);
                var mockQueryFactory = new Mock<IAuditableQueryFactory<int, int, int>>(MockBehavior.Strict);

                Assert.Throws<ArgumentNullException>(() => new FakeCrudController(
                    mockAuthorizationService.Object,
                    mockLogger.Object,
                    mockMediator.Object,
                    mockCommandFactory.Object,
                    mockQueryFactory.Object));
            }
        }

        public sealed class AddAsyncMethod
        {
            public async Task ThrowsArgumentNullExceptionAsync()
            {

            }
        }

        public sealed class DeleteAsyncMethod
        {
            public async Task ThrowsArgumentNullExceptionAsync()
            {

            }
        }

        public sealed class UpdateAsyncMethod
        {
            public async Task ThrowsArgumentNullExceptionAsync()
            {

            }
        }
    }
}
