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
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using Xunit;

namespace Dhgms.AspNetCoreContrib.UnitTests.Controllers
{
    public static class CrudControllerTests
    {
        public sealed class ConstructorMethod
        {
            public void ThrowsArgumentNullException()
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
