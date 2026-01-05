// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NetTestRegimentation;
using NetTestRegimentation.XUnit.Logging;
using NetTestRegimentation.XUnit.Theories.ArgumentNullException;
using Whipstaff.Mermaid.HttpServer;
using Whipstaff.Mermaid.Playwright;
using Whipstaff.Playwright;
using Xunit;

namespace Whipstaff.UnitTests.Mermaid.Playwright
{
    /// <summary>
    /// Unit Tests for <see cref="PlaywrightRendererBrowserInstance"/>.
    /// </summary>
    public static class PlaywrightRendererBrowserInstanceTests
    {
        /// <summary>
        /// Unit Tests for <see cref="PlaywrightRendererBrowserInstance.CreateDiagramAndWriteToFileAsync(IFileInfo, IFileInfo)"/>.
        /// </summary>
        public sealed class CreateDiagramAndWriteToFileAsyncMethod : TestWithLoggingBase, ITestAsyncMethodWithNullableParameters<IFileInfo, IFileInfo>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="CreateDiagramAndWriteToFileAsyncMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output instance.</param>
            public CreateDiagramAndWriteToFileAsyncMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <inheritdoc/>
            [Theory]
            [ClassData(typeof(ThrowsArgumentNullExceptionAsyncTestSource))]
            public async Task ThrowsArgumentNullExceptionAsync(
                IFileInfo? arg1,
                IFileInfo? arg2,
                string expectedParameterNameForException)
            {
                var mermaidHttpServer = MermaidHttpServerFactory.GetTestServer(LoggerFactory, new FileSystem());
                var logMessageActionsWrapper = new PlaywrightRendererLogMessageActionsWrapper(
                    new PlaywrightRendererLogMessageActions(),
                    LoggerFactory.CreateLogger<PlaywrightRenderer>());

                var instance = new PlaywrightRenderer(
                    mermaidHttpServer,
                    logMessageActionsWrapper);

                var browserSession = await instance.GetBrowserSessionAsync(PlaywrightBrowserTypeAndChannel.Chrome());

                _ = await Assert.ThrowsAsync<ArgumentNullException>(
                    expectedParameterNameForException,
                    () => browserSession.CreateDiagramAndWriteToFileAsync(
                        arg1!,
                        arg2!));
            }

            /// <summary>
            /// Test to ensure the SVG generator creates a file.
            /// </summary>
            /// <returns>
            /// A <see cref="Task" /> representing the asynchronous operation.
            /// </returns>
            [Fact]
            public async Task ReturnsResult()
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

                var sourceFile = fileSystem.FileInfo.New(fileSystem.AllFiles.First());

                var mermaidHttpServer = MermaidHttpServerFactory.GetTestServer(LoggerFactory, new FileSystem());
                var logMessageActionsWrapper = new PlaywrightRendererLogMessageActionsWrapper(
                    new PlaywrightRendererLogMessageActions(),
                    LoggerFactory.CreateLogger<PlaywrightRenderer>());

                var instance = new PlaywrightRenderer(
                    mermaidHttpServer,
                    logMessageActionsWrapper);

                var browserSession = await instance.GetBrowserSessionAsync(PlaywrightBrowserTypeAndChannel.Chrome());

                for (int i = 1; i <= 40; i++)
                {
                    var targetFile = fileSystem.FileInfo.New(fileSystem.Path.Combine(fileSystem.Directory.GetCurrentDirectory(), i + "output.mmd"));

                    await browserSession.CreateDiagramAndWriteToFileAsync(
                        sourceFile,
                        targetFile);

                    Assert.True(targetFile.Exists);
                    var content = await targetFile.OpenText().ReadToEndAsync(TestContext.Current.CancellationToken);
                    Assert.StartsWith(
                        "<svg id=\"mermaid-graph\" width=\"100%\" xmlns=\"http://www.w3.org/2000/svg\" class=\"flowchart\" style=\"max-width: 204.640625px;\" viewBox=\"0 0 204.640625 278\" role=\"graphics-document document\" aria-roledescription=\"flowchart-v2\">",
                        content,
                        StringComparison.Ordinal);
                }
            }

            /// <summary>
            /// Test source for <see cref="ThrowsArgumentNullExceptionAsync"/>.
            /// </summary>
            public sealed class ThrowsArgumentNullExceptionAsyncTestSource : ArgumentNullExceptionTheoryData<IFileInfo, IFileInfo>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="ThrowsArgumentNullExceptionAsyncTestSource"/> class.
                /// </summary>
                public ThrowsArgumentNullExceptionAsyncTestSource()
                    : base(
                        new NamedParameterInput<IFileInfo>("sourceFile", () => new MockFileInfo(new MockFileSystem(), "input.mmd")),
                        new NamedParameterInput<IFileInfo>("targetFile", () => new MockFileInfo(new MockFileSystem(), "output.mmd")))
                {
                }
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="PlaywrightRendererBrowserInstance.GetDiagram(IFileInfo)"/>.
        /// </summary>
        public sealed class GetDiagramMethodWithIFileInfo : TestWithLoggingBase, ITestAsyncMethodWithNullableParameters<IFileInfo>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="GetDiagramMethodWithIFileInfo"/> class.
            /// </summary>
            /// <param name="output">XUnit test output instance.</param>
            public GetDiagramMethodWithIFileInfo(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <inheritdoc/>
            [Theory]
            [ClassData(typeof(ThrowsArgumentNullExceptionAsyncTestSource))]
            public async Task ThrowsArgumentNullExceptionAsync(IFileInfo? arg, string expectedParameterNameForException)
            {
                var mermaidHttpServer = MermaidHttpServerFactory.GetTestServer(LoggerFactory, new FileSystem());
                var logMessageActionsWrapper = new PlaywrightRendererLogMessageActionsWrapper(
                    new PlaywrightRendererLogMessageActions(),
                    LoggerFactory.CreateLogger<PlaywrightRenderer>());

                var instance = new PlaywrightRenderer(
                    mermaidHttpServer,
                    logMessageActionsWrapper);
                var browserSession = await instance.GetBrowserSessionAsync(PlaywrightBrowserTypeAndChannel.Chrome());

                _ = await Assert.ThrowsAsync<ArgumentNullException>(
                    expectedParameterNameForException,
                    () => browserSession.GetDiagram(arg!));
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
                var mermaidHttpServer = MermaidHttpServerFactory.GetTestServer(LoggerFactory, new FileSystem());
                var logMessageActionsWrapper = new PlaywrightRendererLogMessageActionsWrapper(
                    new PlaywrightRendererLogMessageActions(),
                    LoggerFactory.CreateLogger<PlaywrightRenderer>());

                var instance = new PlaywrightRenderer(
                    mermaidHttpServer,
                    logMessageActionsWrapper);
                var browserSession = await instance.GetBrowserSessionAsync(PlaywrightBrowserTypeAndChannel.Chrome());
                var diagramResponseModel = await browserSession.GetDiagram(sourceFileInfo);

                Assert.NotNull(diagramResponseModel);

#pragma warning disable CA2254
                Logger.LogInformation(diagramResponseModel.Svg);
#pragma warning restore CA2254

                Assert.NotNull(diagramResponseModel.Svg);
                Assert.StartsWith(expectedStart, diagramResponseModel.Svg, StringComparison.Ordinal);
            }

            /// <summary>
            /// Test source <see cref="PlaywrightRendererBrowserInstance.GetDiagram(string)"/>.
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

                    Add(
                        fileInfo,
                        "<svg id=\"mermaid-graph\" width=\"100%\" xmlns=\"http://www.w3.org/2000/svg\" class=\"flowchart\" style=\"max-width: 204.640625px;\" viewBox=\"0 0 204.640625 278\" role=\"graphics-document document\" aria-roledescription=\"flowchart-v2\">");
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
        /// Unit Tests for <see cref="PlaywrightRendererBrowserInstance.GetDiagram(string)"/>.
        /// </summary>
        public sealed class GetDiagramMethodWithString : TestWithLoggingBase, ITestAsyncMethodWithNullableParameters<string>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="GetDiagramMethodWithString"/> class.
            /// </summary>
            /// <param name="output">XUnit test output instance.</param>
            public GetDiagramMethodWithString(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <inheritdoc/>
            [Theory]
            [ClassData(typeof(ThrowsArgumentNullExceptionAsyncTestSource))]
            public async Task ThrowsArgumentNullExceptionAsync(string? arg, string expectedParameterNameForException)
            {
                var mermaidHttpServer = MermaidHttpServerFactory.GetTestServer(LoggerFactory, new FileSystem());
                var logMessageActionsWrapper = new PlaywrightRendererLogMessageActionsWrapper(
                    new PlaywrightRendererLogMessageActions(),
                    LoggerFactory.CreateLogger<PlaywrightRenderer>());

                var instance = new PlaywrightRenderer(
                    mermaidHttpServer,
                    logMessageActionsWrapper);
                var browserSession = await instance.GetBrowserSessionAsync(PlaywrightBrowserTypeAndChannel.Chrome());
                _ = await Assert.ThrowsAsync<ArgumentNullException>(
                    expectedParameterNameForException,
                    () => browserSession.GetDiagram(arg!));
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
                var mermaidHttpServer = MermaidHttpServerFactory.GetTestServer(LoggerFactory, new FileSystem());
                var logMessageActionsWrapper = new PlaywrightRendererLogMessageActionsWrapper(
                    new PlaywrightRendererLogMessageActions(),
                    LoggerFactory.CreateLogger<PlaywrightRenderer>());

                var instance = new PlaywrightRenderer(
                    mermaidHttpServer,
                    logMessageActionsWrapper);
                var browserSession = await instance.GetBrowserSessionAsync(PlaywrightBrowserTypeAndChannel.Chrome());
                var diagramResponseModel = await browserSession.GetDiagram(diagram);

                Assert.NotNull(diagramResponseModel);

#pragma warning disable CA2254
                Logger.LogInformation(diagramResponseModel.Svg);
#pragma warning restore CA2254

                Assert.NotNull(diagramResponseModel.Svg);
                Assert.StartsWith(expectedStart, diagramResponseModel.Svg, StringComparison.Ordinal);
            }

            /// <summary>
            /// Test source <see cref="PlaywrightRendererBrowserInstance.GetDiagram(string)"/>.
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

                    Add(
                        graph,
                        "<svg id=\"mermaid-graph\" width=\"100%\" xmlns=\"http://www.w3.org/2000/svg\" class=\"flowchart\" style=\"max-width: 204.640625px;\" viewBox=\"0 0 204.640625 278\" role=\"graphics-document document\" aria-roledescription=\"flowchart-v2\">");
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
