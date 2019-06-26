using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dhgms.AspNetCoreContrib.Example.WebSite;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace Dhgms.AspNetCoreContrib.IntegrationTests
{
    public sealed class SecuredWebsiteTests : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> _factory;

        public SecuredWebsiteTests(WebApplicationFactory<Startup> factory)
        {
            this._factory = factory;
        }

        public static IEnumerable<object[]> GetReturnsSuccessAndCorrectContentTypetestSource =>
            GetGetReturnsSuccessAndCorrectContentTypetestSource();

        [Theory]
        [MemberData(nameof(GetReturnsSuccessAndCorrectContentTypetestSource))]
        public async Task GetReturnsSuccessAndCorrectContentType(string url)
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync(url);

            response.EnsureSuccessStatusCode();
            Assert.Equal(
                "text/html; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
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
    }
}
