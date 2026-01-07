// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.Extensions.Logging;
using Xunit;

namespace Whipstaff.Testing.Logging
{
    /// <summary>
    /// Base class for tests that require logging.
    /// </summary>
    public abstract class AbstractTestWithLogging
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractTestWithLogging"/> class.
        /// </summary>
        /// <param name="output">XUnit Test Output Helper.</param>
        protected AbstractTestWithLogging(ITestOutputHelper output)
        {
            LoggerFactory = new TestLogger(output);
#pragma warning disable GR0012 // Constructors should minimise work and not execute methods
            Logger = LoggerFactory.CreateLogger(GetType());
#pragma warning restore GR0012 // Constructors should minimise work and not execute methods
        }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        protected ILogger Logger { get; }

        /// <summary>
        /// Gets the test logger factory.
        /// </summary>
        protected TestLogger LoggerFactory { get; }
    }
}
