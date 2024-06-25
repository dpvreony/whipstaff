// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NetTestRegimentation;
using NetTestRegimentation.XUnit.Theories.ArgumentNullException;
using Whipstaff.Mermaid.HttpServer;
using Whipstaff.Mermaid.Playwright;
using Xunit;
using Xunit.Abstractions;

namespace Whipstaff.UnitTests.Mermaid.Playwright
{
    /// <summary>
    /// Unit Tests for <see cref="PlaywrightRenderer"/>.
    /// </summary>
    public static class PlaywrightRendererTests
    {
        /// <summary>
        /// Unit tests for the constructor.
        /// </summary>
        public sealed class ConstructorMethod : Foundatio.Xunit.TestWithLoggingBase, ITestConstructorMethodWithNullableParameters<MermaidHttpServer, PlaywrightRendererLogMessageActionsWrapper>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ConstructorMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output instance.</param>
            public ConstructorMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <inheritdoc/>
            [Fact]
            public void ReturnsInstance()
            {
                var mermaidHttpServer = MermaidHttpServerFactory.GetTestServer(Log);
                var logMessageActionsWrapper = new PlaywrightRendererLogMessageActionsWrapper(
                    new PlaywrightRendererLogMessageActions(),
                    Log.CreateLogger<PlaywrightRenderer>());

                var instance = new PlaywrightRenderer(
                    mermaidHttpServer,
                    logMessageActionsWrapper);
                Assert.NotNull(instance);
            }

            /// <inheritdoc/>
            [Theory]
            [ClassData(typeof(ThrowsArgumentNullExceptionTestSource))]
            public void ThrowsArgumentNullException(
                MermaidHttpServer arg1,
                PlaywrightRendererLogMessageActionsWrapper arg2,
                string expectedParameterNameForException)
            {
                var exception = Assert.Throws<ArgumentNullException>(() => new PlaywrightRenderer(arg1, arg2));

                Assert.Equal(
                    expectedParameterNameForException,
                    exception.ParamName);
            }

            /// <summary>
            /// Test source for <see cref="ThrowsArgumentNullException"/>.
            /// </summary>
            public sealed class ThrowsArgumentNullExceptionTestSource : ArgumentNullExceptionTheoryData<MermaidHttpServer, PlaywrightRendererLogMessageActionsWrapper>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="ThrowsArgumentNullExceptionTestSource"/> class.
                /// </summary>
                public ThrowsArgumentNullExceptionTestSource()
                    : base(
                        new NamedParameterInput<MermaidHttpServer>(
                            "mermaidHttpServer",
                            () => MermaidHttpServerFactory.GetTestServer(new NullLoggerFactory())),
                        new NamedParameterInput<PlaywrightRendererLogMessageActionsWrapper>(
                            "logMessageActionsWrapper",
                            () => new PlaywrightRendererLogMessageActionsWrapper(
                                new PlaywrightRendererLogMessageActions(),
                                new NullLoggerFactory().CreateLogger<PlaywrightRenderer>())))
                {
                }
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="PlaywrightRenderer.GetDiagram"/>.
        /// </summary>
        public sealed class GetDiagramMethod : Foundatio.Xunit.TestWithLoggingBase, ITestAsyncMethodWithNullableParameters<string>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="GetDiagramMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output instance.</param>
            public GetDiagramMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <inheritdoc/>
            [Theory]
            [ClassData(typeof(ThrowsArgumentNullExceptionAsyncTestSource))]
            public async Task ThrowsArgumentNullExceptionAsync(string arg, string expectedParameterNameForException)
            {
                var mermaidHttpServer = MermaidHttpServerFactory.GetTestServer(Log);
                var logMessageActionsWrapper = new PlaywrightRendererLogMessageActionsWrapper(
                    new PlaywrightRendererLogMessageActions(),
                    Log.CreateLogger<PlaywrightRenderer>());

                var instance = new PlaywrightRenderer(
                    mermaidHttpServer,
                    logMessageActionsWrapper);
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => instance.GetDiagram(arg, PlaywrightBrowserType.Chromium, null));

                Assert.Equal(expectedParameterNameForException, exception.ParamName);
            }

            /// <summary>
            /// Test to ensure the SVG generator returns specific results.
            /// </summary>
            /// <param name="diagram">Mermaid diagram to parse.</param>
            /// <param name="expectedStart">The expected result.</param>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Theory]
            [ClassData(typeof(ReturnsResultTestSource))]
            public async Task ReturnsResult(string diagram, string expectedStart)
            {
                var mermaidHttpServer = MermaidHttpServerFactory.GetTestServer(Log);
                var logMessageActionsWrapper = new PlaywrightRendererLogMessageActionsWrapper(
                    new PlaywrightRendererLogMessageActions(),
                    Log.CreateLogger<PlaywrightRenderer>());

                var instance = new PlaywrightRenderer(
                    mermaidHttpServer,
                    logMessageActionsWrapper);
                var diagramResponseModel = await instance.GetDiagram(diagram, PlaywrightBrowserType.Chromium, "chrome");

                Assert.NotNull(diagramResponseModel);

                _logger.LogInformation(diagramResponseModel.Svg);

                Assert.NotNull(diagramResponseModel.Svg);
                Assert.StartsWith(expectedStart, diagramResponseModel.Svg, StringComparison.Ordinal);
            }

            /// <summary>
            /// Test source <see cref="PlaywrightRenderer.GetDiagram"/>.
            /// </summary>
            public sealed class ReturnsResultTestSource : TheoryData<string, string>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="ReturnsResultTestSource"/> class.
                /// </summary>
                public ReturnsResultTestSource()
                {
                    var graph = "graph TD;" + Environment.NewLine +
                        "    A-->B;" + Environment.NewLine +
                        "    A-->C;" + Environment.NewLine +
                        "    B-->D;" + Environment.NewLine +
                        "    C-->D;";

                    Add(graph, "<svg aria-roledescription=\"flowchart-v2\" role=\"graphics-document document\"");
                }
            }

            /// <summary>
            /// Test source for <see cref="ThrowsArgumentNullExceptionAsync"/>.
            /// </summary>
            public sealed class ThrowsArgumentNullExceptionAsyncTestSource : TheoryData<string?, string>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="ThrowsArgumentNullExceptionAsyncTestSource"/> class.
                /// </summary>
                public ThrowsArgumentNullExceptionAsyncTestSource()
                {
                    Add(null, "markdown");
                }
            }
        }
    }
}
