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
    public sealed class SecuredWebsiteTests : BaseWebApplicationTest<Startup>
    {
        public SecuredWebsiteTests(
            ITestOutputHelper output,
            WebApplicationFactory<Startup> factory)
            : base(output, factory)
        {
        }

        public static IEnumerable<object[]> GetReturnsSuccessAndCorrectContentTypetestSource =>
            GetGetReturnsSuccessAndCorrectContentTypeTestSource();

        [Theory]
        [MemberData(nameof(GetReturnsSuccessAndCorrectContentTypetestSource))]
        public async Task GetReturnsSuccessAndCorrectContentType(string url)
        {
            var client = this.Factory.CreateClient();

            var response = await client.GetAsync(url).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();
            Assert.Equal(
                "text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());

            await this.LogResponse(response).ConfigureAwait(false);
        }

        private static IEnumerable<object[]> GetGetReturnsSuccessAndCorrectContentTypeTestSource()
        {
            return new[]
            {
                new object[]
                {
                    "/",
                },
            };
        }
    }
}
