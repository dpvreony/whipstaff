using Dhgms.AspNetCoreContrib.Controllers;
using System;
using System.Collections.Generic;
using System.Text;
using Dhgms.AspNetCoreContrib.Abstractions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Dhgms.AspNetCoreContrib.UnitTests.Controllers
{
    public static class FuncQueryOnlyControllerTests
    {
        public sealed class ConstructorMethod
        {
            public static IEnumerable<object[]> ThrowsArgumentNullExceptionTestData => new object[][]
            {

            };

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
    }
}
