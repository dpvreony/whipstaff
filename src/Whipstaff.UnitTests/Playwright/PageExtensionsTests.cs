// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Playwright;
using NetTestRegimentation;
using NetTestRegimentation.XUnit.Theories.ArgumentNullException;
using Whipstaff.Playwright;
using Xunit;
using Xunit.Abstractions;

namespace Whipstaff.UnitTests.Playwright
{
    /// <summary>
    /// Unit tests for <see cref="PageExtensions"/>.
    /// </summary>
    public static class PageExtensionsTests
    {
        /// <summary>
        /// Unit tests for <see cref="PageExtensions.EnumerateImgTagsWithIncompleteAltAttributeAsync"/>.
        /// </summary>
        public sealed class EnumerateImgTagsWithIncompleteAltAttributeMethod : Foundatio.Xunit.TestWithLoggingBase, ITestMethodWithNullableParameters<IPage>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="EnumerateImgTagsWithIncompleteAltAttributeMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output instance.</param>
            public EnumerateImgTagsWithIncompleteAltAttributeMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <inheritdoc />
            [Theory]
            [ClassData(typeof(ThrowsArgumentNullExceptionTestSource))]
            public void ThrowsArgumentNullException(
                IPage arg,
                string expectedParameterNameForException)
            {
                _ = Assert.Throws<ArgumentNullException>(
                    expectedParameterNameForException,
                    () => arg.EnumerateImgTagsWithIncompleteAltAttributeAsync());
            }

            /// <summary>
            /// Test that the method returns a populated collection.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsPopulatedCollection()
            {
                // Start Playwright and launch a browser
                using (var playwright = await Microsoft.Playwright.Playwright.CreateAsync())
                await using (var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true }))
                {
                    var page = await browser.NewPageAsync();

                    // Navigate to a page
                    var response = await page.GotoAsync("https://localhost");

                    Assert.NotNull(response);
                    Assert.True(response.Ok);

                    // Get all img elements
                    var images = await page.EnumerateImgTagsWithIncompleteAltAttributeAsync().ToArrayAsync();

                    Assert.NotEmpty(images);

                    // Close the browser
                    await browser.CloseAsync();
                }
            }

            /// <summary>
            /// Test that the method returns an empty collection.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsEmptyCollection()
            {
                // Start Playwright and launch a browser
                using (var playwright = await Microsoft.Playwright.Playwright.CreateAsync())
                await using (var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true }))
                {
                    var page = await browser.NewPageAsync();

                    // Navigate to a page
                    var response = await page.GotoAsync("https://localhost");

                    Assert.NotNull(response);
                    Assert.True(response.Ok);

                    // Get all img elements
                    var images = await page.EnumerateImgTagsWithIncompleteAltAttributeAsync().ToArrayAsync();

                    Assert.NotEmpty(images);

                    // Close the browser
                    await browser.CloseAsync();
                }
            }

            /// <summary>
            /// Test source for <see cref="ThrowsArgumentNullException"/>.
            /// </summary>
            public sealed class ThrowsArgumentNullExceptionTestSource : ArgumentNullExceptionTheoryData<IPlaywright>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="ThrowsArgumentNullExceptionTestSource"/> class.
                /// </summary>
                public ThrowsArgumentNullExceptionTestSource()
                    : base("page")
                {
                }
            }
        }
    }
}
