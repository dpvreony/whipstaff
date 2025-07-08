// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Markdig;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NetTestRegimentation;
using NetTestRegimentation.XUnit.Logging;
using NetTestRegimentation.XUnit.Theories.ArgumentNullException;
using Whipstaff.Markdig.Mermaid;
using Whipstaff.Markdig.Settings;
using Whipstaff.Mermaid.HttpServer;
using Whipstaff.Mermaid.Playwright;
using Whipstaff.Playwright;
using Xunit;

namespace Whipstaff.UnitTests.Markdig.Mermaid
{
    /// <summary>
    /// Unit Tests for <see cref="HtmlMermaidJsRendererTests"/>.
    /// </summary>
    public static class HtmlMermaidJsRendererTests
    {
        /// <summary>
        /// Unit tests for the <see cref="HtmlMermaidJsRenderer.CreateAsync"/> method.
        /// </summary>
        public sealed class CreateAsyncMethod : TestWithLoggingBase, ITestAsyncMethodWithNullableParameters<MarkdownJsExtensionSettings, ILoggerFactory>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="CreateAsyncMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output instance.</param>
            public CreateAsyncMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <summary>
            /// Test to ensure that the <see cref="HtmlMermaidJsRenderer"/> is created successfully.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsInstance()
            {
                var mermaidHttpServer = MermaidHttpServerFactory.GetTestServer(LoggerFactory);
                var logMessageActions = new PlaywrightRendererLogMessageActions();
                var logMessageActionsWrapper = new PlaywrightRendererLogMessageActionsWrapper(
                    logMessageActions,
                    LoggerFactory.CreateLogger<PlaywrightRenderer>());
                var playwrightRenderer = new PlaywrightRenderer(
                    mermaidHttpServer,
                    logMessageActionsWrapper);

                var browserSession = await playwrightRenderer.GetBrowserSessionAsync(PlaywrightBrowserTypeAndChannel.Chrome());

                var settings = new MarkdownJsExtensionSettings(browserSession, OutputMode.Png);

                var instance = await HtmlMermaidJsRenderer.CreateAsync(settings, LoggerFactory);

                Assert.NotNull(instance);
            }

            /// <inheritdoc/>
            [Theory]
            [ClassData(typeof(ThrowsArgumentNullExceptionTestSource))]
            public async Task ThrowsArgumentNullExceptionAsync(MarkdownJsExtensionSettings? arg1, ILoggerFactory? arg2, string expectedParameterNameForException)
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => HtmlMermaidJsRenderer.CreateAsync(arg1!, arg2!));
                Assert.Equal(expectedParameterNameForException, exception.ParamName);
            }

            /// <summary>
            /// Test source for <see cref="ThrowsArgumentNullExceptionAsync"/>.
            /// </summary>
            public sealed class ThrowsArgumentNullExceptionTestSource : ArgumentNullExceptionTheoryData<MarkdownJsExtensionSettings, ILoggerFactory>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="ThrowsArgumentNullExceptionTestSource"/> class.
                /// </summary>
                public ThrowsArgumentNullExceptionTestSource()
                    : base(
                        new NamedParameterInput<MarkdownJsExtensionSettings>("settings", () => new MarkdownJsExtensionSettings(new PlaywrightRendererBrowserInstanceCreateExpectations().Instance(), OutputMode.Png)),
                        new NamedParameterInput<ILoggerFactory>("loggerFactory", () => new NullLoggerFactory()))
                {
                }
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="HtmlMermaidJsRenderer.Write"/>.
        /// </summary>
        public sealed class WriteMethod : TestWithLoggingBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="WriteMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output instance.</param>
            public WriteMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <summary>
            /// Test to ensure markdown is formatted to mermaid.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task WritesMarkdown()
            {
                var markdown = "```mermaid" + Environment.NewLine +
                               "graph TD;" + Environment.NewLine +
                               "    A-->B;" + Environment.NewLine +
                               "    A-->C;" + Environment.NewLine +
                               "    B-->D;" + Environment.NewLine +
                               "    C-->D;" + Environment.NewLine +
                               "```" + Environment.NewLine +
                               Environment.NewLine +
                               "```csharp" + Environment.NewLine +
                               "   int i = 1;" + Environment.NewLine +
                               "```";

                var mermaidHttpServer = MermaidHttpServerFactory.GetTestServer(LoggerFactory);
                var logMessageActions = new PlaywrightRendererLogMessageActions();
                var logMessageActionsWrapper = new PlaywrightRendererLogMessageActionsWrapper(
                    logMessageActions,
                    LoggerFactory.CreateLogger<PlaywrightRenderer>());
                var playwrightRenderer = new PlaywrightRenderer(
                    mermaidHttpServer,
                    logMessageActionsWrapper);
                var browserSession = await playwrightRenderer.GetBrowserSessionAsync(PlaywrightBrowserTypeAndChannel.Chrome());

                var pipelineBuilder = new MarkdownPipelineBuilder().UseMermaidJsExtension(browserSession, LoggerFactory);

                var pipeline = pipelineBuilder.Build();
                var actualHtml = Markdown.ToHtml(markdown, pipeline);

                Logger.LogInformation(actualHtml);
            }
        }
    }
}
