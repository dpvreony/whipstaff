using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Dhgms.AspNetCoreContrib.Example.WebSite;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace Dhgms.AspNetCoreContrib.IntegrationTests
{
    public sealed class SecuredWebsiteTests : Foundatio.Logging.Xunit.TestWithLoggingBase, IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public SecuredWebsiteTests(ITestOutputHelper output, WebApplicationFactory<Startup> factory)
            : base(output)
        {
            this._factory = factory;
        }

        public static IEnumerable<object[]> GetReturnsSuccessAndCorrectContentTypetestSource =>
            GetGetReturnsSuccessAndCorrectContentTypetestSource();

        [Theory]
        [MemberData(nameof(GetReturnsSuccessAndCorrectContentTypetestSource))]
        public async Task GetReturnsSuccessAndCorrectContentType(string url)
        {
            var client = this._factory.CreateClient();

            var response = await client.GetAsync(url).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();
            Assert.Equal(
                "text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());

            await this.LogResponse(response).ConfigureAwait(false);
        }

        private static IEnumerable<object[]> GetGetReturnsSuccessAndCorrectContentTypetestSource()
        {
            return new[]
            {
                new object[]
                {
                    "/",
                },
            };
        }

        private async Task LogResponse(HttpResponseMessage response)
        {
            foreach (var httpResponseHeader in response.Headers)
            {
                this._logger.LogInformation($"{httpResponseHeader.Key}: {string.Join(",", httpResponseHeader.Value)}");
            }

            var result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            this._logger.LogInformation(result);
        }
    }
}
