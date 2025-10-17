// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Threading.Tasks;
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
        public sealed class ConstructorMethod : TestWithLoggingBase, ITestConstructorMethodWithNullableParameters<MermaidHttpServer, PlaywrightRendererLogMessageActionsWrapper>
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
                MermaidHttpServer? arg1,
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
            public sealed class ThrowsArgumentNullExceptionTestSource : ArgumentNullExceptionTheoryData<MermaidHttpServer, PlaywrightRendererLogMessageActionsWrapper>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="ThrowsArgumentNullExceptionTestSource"/> class.
                /// </summary>
                public ThrowsArgumentNullExceptionTestSource()
                    : base(
                        new NamedParameterInput<MermaidHttpServer>(
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
    }
}
