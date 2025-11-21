// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NetTestRegimentation;
using NetTestRegimentation.XUnit.Theories.ArgumentNullException;
using Whipstaff.Mermaid.HttpServer;
using Whipstaff.Mermaid.Playwright;
using Whipstaff.Playwright;
using Whipstaff.Testing.Logging;
using Xunit;

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
        public sealed class ConstructorMethod : TestWithLoggingBase, ITestConstructorMethodWithNullableParameters<TestServer, PlaywrightRendererLogMessageActionsWrapper>
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
                var mermaidHttpServer = MermaidHttpServerFactory.GetTestServer(LoggerFactory, new FileSystem());
                var logMessageActionsWrapper = new PlaywrightRendererLogMessageActionsWrapper(
                    new PlaywrightRendererLogMessageActions(),
                    LoggerFactory.CreateLogger<PlaywrightRenderer>());

                var instance = new PlaywrightRenderer(
                    mermaidHttpServer,
                    logMessageActionsWrapper);
                Assert.NotNull(instance);
            }

            /// <inheritdoc/>
            [Theory]
            [ClassData(typeof(ThrowsArgumentNullExceptionTestSource))]
            public void ThrowsArgumentNullException(
                TestServer? arg1,
                PlaywrightRendererLogMessageActionsWrapper? arg2,
                string expectedParameterNameForException)
            {
                var exception = Assert.Throws<ArgumentNullException>(() => new PlaywrightRenderer(arg1!, arg2!));

                Assert.Equal(
                    expectedParameterNameForException,
                    exception.ParamName);
            }

            /// <summary>
            /// Test source for <see cref="ThrowsArgumentNullException"/>.
            /// </summary>
            public sealed class ThrowsArgumentNullExceptionTestSource : ArgumentNullExceptionTheoryData<TestServer, PlaywrightRendererLogMessageActionsWrapper>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="ThrowsArgumentNullExceptionTestSource"/> class.
                /// </summary>
                public ThrowsArgumentNullExceptionTestSource()
                    : base(
                        new NamedParameterInput<TestServer>(
                            "mermaidHttpServer",
                            () => MermaidHttpServerFactory.GetTestServer(new NullLoggerFactory(), new FileSystem())),
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
        /// Unit Tests for <see cref="PlaywrightRenderer.Default(ILoggerFactory)"/>.
        /// </summary>
        public sealed class DefaultMethod : TestWithLoggingBase, ITestMethodWithNullableParameters<ILoggerFactory>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="DefaultMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output instance.</param>
            public DefaultMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <inheritdoc/>
            [Theory]
            [ClassData(typeof(ThrowsArgumentNullExceptionTestSource))]
            public void ThrowsArgumentNullException(ILoggerFactory? arg, string expectedParameterNameForException)
            {
                _ = Assert.Throws<ArgumentNullException>(
                    expectedParameterNameForException,
                    () => PlaywrightRenderer.Default(arg!));
            }

            /// <summary>
            /// Test to ensure the default instance is returned.
            /// </summary>
            [Fact]
            public void ReturnsInstance()
            {
                var instance = PlaywrightRenderer.Default(LoggerFactory);
                Assert.NotNull(instance);
            }

            /// <summary>
            /// Test source for <see cref="ThrowsArgumentNullException"/>.
            /// </summary>
            public sealed class ThrowsArgumentNullExceptionTestSource : ArgumentNullExceptionTheoryData<ILoggerFactory>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="ThrowsArgumentNullExceptionTestSource"/> class.
                /// </summary>
                public ThrowsArgumentNullExceptionTestSource()
                    : base("loggerFactory")
                {
                }
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="PlaywrightRenderer.GetDiagramAsync(IFileInfo, PlaywrightBrowserTypeAndChannel)"/>.
        /// </summary>
        public sealed class GetDiagramMethodWithIFileInfoPlaywrightBrowserTypeString : TestWithLoggingBase, ITestAsyncMethodWithNullableParameters<IFileInfo>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="GetDiagramMethodWithIFileInfoPlaywrightBrowserTypeString"/> class.
            /// </summary>
            /// <param name="output">XUnit test output instance.</param>
            public GetDiagramMethodWithIFileInfoPlaywrightBrowserTypeString(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <inheritdoc/>
            [Theory]
            [ClassData(typeof(ThrowsArgumentNullExceptionAsyncTestSource))]
            public async Task ThrowsArgumentNullExceptionAsync(IFileInfo? arg, string expectedParameterNameForException)
            {
                var mermaidHttpServer = MermaidHttpServerFactory.GetTestServer(LoggerFactory);
                var logMessageActionsWrapper = new PlaywrightRendererLogMessageActionsWrapper(
                    new PlaywrightRendererLogMessageActions(),
                    LoggerFactory.CreateLogger<PlaywrightRenderer>());

                var instance = new PlaywrightRenderer(
                    mermaidHttpServer,
                    logMessageActionsWrapper);
                _ = await Assert.ThrowsAsync<ArgumentNullException>(
                    expectedParameterNameForException,
                    () => instance.GetDiagram(arg!, PlaywrightBrowserTypeAndChannel.Chrome()));
            }

            /// <summary>
            /// Test to ensure the SVG generator returns specific results.
            /// </summary>
            /// <param name="sourceFileInfo">Source File to parse.</param>
            /// <param name="expectedStart">The expected result.</param>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Theory]
            [ClassData(typeof(ReturnsResultTestSource))]
            public async Task ReturnsResult(IFileInfo sourceFileInfo, string expectedStart)
            {
                var mermaidHttpServer = MermaidHttpServerFactory.GetTestServer(LoggerFactory);
                var logMessageActionsWrapper = new PlaywrightRendererLogMessageActionsWrapper(
                    new PlaywrightRendererLogMessageActions(),
                    LoggerFactory.CreateLogger<PlaywrightRenderer>());

                var instance = new PlaywrightRenderer(
                    mermaidHttpServer,
                    logMessageActionsWrapper);
                var diagramResponseModel = await instance.GetDiagramAsync(sourceFileInfo, PlaywrightBrowserTypeAndChannel.Chrome());

                Assert.NotNull(diagramResponseModel);

                Logger.LogInformation(diagramResponseModel.Svg);

                Assert.NotNull(diagramResponseModel.Svg);
                Assert.StartsWith(expectedStart, diagramResponseModel.Svg, StringComparison.Ordinal);
            }

            /// <summary>
            /// Test source <see cref="PlaywrightRenderer.GetDiagramAsync(string, PlaywrightBrowserTypeAndChannel)"/>.
            /// </summary>
            public sealed class ReturnsResultTestSource : TheoryData<IFileInfo, string>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="ReturnsResultTestSource"/> class.
                /// </summary>
                public ReturnsResultTestSource()
                {
                    var fileSystem = new MockFileSystem();
                    var graph = "graph TD;" + Environment.NewLine +
                                "    A-->B;" + Environment.NewLine +
                                "    A-->C;" + Environment.NewLine +
                                "    B-->D;" + Environment.NewLine +
                                "    C-->D;";

                    fileSystem.AddFile(
                        "test.mmd",
                        graph);

                    var fileInfo = fileSystem.FileInfo.New(fileSystem.AllFiles.First());

                    Add(fileInfo, "<svg aria-roledescription=\"flowchart-v2\" role=\"graphics-document document\"");
                }
            }

            /// <summary>
            /// Test source for <see cref="ThrowsArgumentNullExceptionAsync"/>.
            /// </summary>
            public sealed class ThrowsArgumentNullExceptionAsyncTestSource : ArgumentNullExceptionTheoryData<IFileInfo>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="ThrowsArgumentNullExceptionAsyncTestSource"/> class.
                /// </summary>
                public ThrowsArgumentNullExceptionAsyncTestSource()
                    : base("sourceFileInfo")
                {
                }
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="PlaywrightRenderer.GetDiagramAsync(string, PlaywrightBrowserTypeAndChannel)"/>.
        /// </summary>
        public sealed class GetDiagramMethodWithStringPlaywrightBrowserTypeString : TestWithLoggingBase, ITestAsyncMethodWithNullableParameters<string>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="GetDiagramMethodWithStringPlaywrightBrowserTypeString"/> class.
            /// </summary>
            /// <param name="output">XUnit test output instance.</param>
            public GetDiagramMethodWithStringPlaywrightBrowserTypeString(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <inheritdoc/>
            [Theory]
            [ClassData(typeof(ThrowsArgumentNullExceptionAsyncTestSource))]
            public async Task ThrowsArgumentNullExceptionAsync(string? arg, string expectedParameterNameForException)
            {
                var mermaidHttpServer = MermaidHttpServerFactory.GetTestServer(LoggerFactory);
                var logMessageActionsWrapper = new PlaywrightRendererLogMessageActionsWrapper(
                    new PlaywrightRendererLogMessageActions(),
                    LoggerFactory.CreateLogger<PlaywrightRenderer>());

                var instance = new PlaywrightRenderer(
                    mermaidHttpServer,
                    logMessageActionsWrapper);
                _ = await Assert.ThrowsAsync<ArgumentNullException>(
                    expectedParameterNameForException,
                    () => instance.GetDiagram(arg!, PlaywrightBrowserTypeAndChannel.Chrome()));
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
                var mermaidHttpServer = MermaidHttpServerFactory.GetTestServer(LoggerFactory);
                var logMessageActionsWrapper = new PlaywrightRendererLogMessageActionsWrapper(
                    new PlaywrightRendererLogMessageActions(),
                    LoggerFactory.CreateLogger<PlaywrightRenderer>());

                var instance = new PlaywrightRenderer(
                    mermaidHttpServer,
                    logMessageActionsWrapper);
                var diagramResponseModel = await instance.GetDiagramAsync(diagram, PlaywrightBrowserTypeAndChannel.Chrome());

                Assert.NotNull(diagramResponseModel);

                Logger.LogInformation(diagramResponseModel.Svg);

                Assert.NotNull(diagramResponseModel.Svg);
                Assert.StartsWith(expectedStart, diagramResponseModel.Svg, StringComparison.Ordinal);
            }

            /// <summary>
            /// Test source <see cref="PlaywrightRenderer.GetDiagramAsync(string, PlaywrightBrowserTypeAndChannel)"/>.
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
