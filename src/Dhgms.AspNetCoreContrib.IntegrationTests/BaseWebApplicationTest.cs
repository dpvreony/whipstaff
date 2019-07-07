using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Dhgms.AspNetCoreContrib.Example.WebSite;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace Dhgms.AspNetCoreContrib.IntegrationTests
{
    public class BaseWebApplicationTest<TStartup>
        : Foundatio.Logging.Xunit.TestWithLoggingBase, IClassFixture<WebApplicationFactory<TStartup>>
        where TStartup : class, IStartup
    {
        public BaseWebApplicationTest(
            ITestOutputHelper output,
            WebApplicationFactory<TStartup> factory)
            : base(output)
        {
            this.Factory = factory;
        }

        protected WebApplicationFactory<TStartup> Factory { get; }

        protected async Task LogResponse(HttpResponseMessage response)
        {
            foreach (var (key, value) in response.Headers)
            {
                this._logger.LogInformation($"{key}: {string.Join(",", value)}");
            }

            var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            this._logger.LogInformation(result);
        }
    }
}
