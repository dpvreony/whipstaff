﻿// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NetTestRegimentation;
using Whipstaff.CommandLine;
using Whipstaff.Testing.CommandLine;
using Xunit;
using Xunit.Abstractions;

namespace Whipstaff.UnitTests.CommandLine
{
    /// <summary>
    /// Unit tests for the <see cref="AbstractCommandLineHandler{TCommandLineArgModel,TLogMessageActionsWrapper}"/> class.
    /// </summary>
    public static class AbstractCommandLineHandlerTests
    {
        /// <summary>
        /// Unit test for <see cref="AbstractCommandLineHandler{TCommandLineArgModel,TLogMessageActionsWrapper}"/> constructor.
        /// </summary>
        public sealed class ConstructorMethod : Foundatio.Xunit.TestWithLoggingBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ConstructorMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output helper instance.</param>
            public ConstructorMethod(ITestOutputHelper output)
                : base(output)
            {
            }
        }

        /// <summary>
        /// Unit test for <see cref="AbstractCommandLineHandler{TCommandLineArgModel,TLogMessageActionsWrapper}.OnHandleCommandAsync(TCommandLineArgModel)"/> method.
        /// </summary>
        public sealed class HandleCommandMethod
            : Foundatio.Xunit.TestWithLoggingBase,
                ITestAsyncMethodWithNullableParameters<FakeCommandLineArgModel>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="HandleCommandMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output helper instance.</param>
            public HandleCommandMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <inheritdoc/>
            [ClassData(typeof(Whipstaff.UnitTests.TestSources.CommandLine.AbstractCommandLineHandlerTests.HandleCommand.ThrowsArgumentNullExceptionAsyncTestSource))]
            [Theory]
            public async Task ThrowsArgumentNullExceptionAsync(
                FakeCommandLineArgModel arg,
                string expectedParameterNameForException)
            {
                var logger = Log.CreateLogger<FakeCommandLineHandler>();
                var instance = new FakeCommandLineHandler(
                    new FakeCommandLineHandlerLogMessageActionsWrapper(logger));

                _ = await Assert.ThrowsAsync<ArgumentNullException>(expectedParameterNameForException, () => instance.HandleCommandAsync(arg));
            }
        }
    }
}
