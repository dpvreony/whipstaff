// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.Logging;

namespace Whipstaff.Testing.Logging
{
    /// <summary>
    /// XUnit Test Logger Provider.
    /// </summary>
    [ProviderAlias("Test")]
    public class TestLoggerProvider : ILoggerProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TestLoggerProvider"/> class.
        /// </summary>
        /// <param name="options">Options to use for configuring the logging.</param>
        public TestLoggerProvider(TestLoggerOptions options)
        {
            Log = new TestLogger(options);
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="TestLoggerProvider"/> class.
        /// </summary>
        ~TestLoggerProvider()
        {
            Dispose(false);
        }

        /// <summary>
        /// Gets the test logger.
        /// </summary>
        public TestLogger Log { get; }

        /// <summary>
        /// Creates a logger.
        /// </summary>
        /// <param name="categoryName">Name of the category for the logger.</param>
        /// <returns>Logging framework instance.</returns>
        public virtual ILogger CreateLogger(string categoryName)
        {
            return Log.CreateLogger(categoryName);
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Disposes of the resources used by the logger provider.
        /// </summary>
        /// <param name="disposing">Whether the object is being disposed.</param>
        protected virtual void Dispose(bool disposing)
        {
        }
    }
}
