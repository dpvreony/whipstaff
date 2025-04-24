// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Whipstaff.Core.Logging;

namespace Whipstaff.EntityFramework.SmokeTest
{
    /// <summary>
    /// Log Message Actions for <see cref="SmokeTestBackgroundWorker{TDbContext,TDbSetChecker}"/>.
    /// </summary>
    /// <typeparam name="TDbContext">The type for the DbContext.</typeparam>
    /// <typeparam name="TDbSetChecker">The type for the DbSet checker.</typeparam>
    public sealed class SmokeTestBackgroundWorkerLogMessageActions<TDbContext, TDbSetChecker> : ILogMessageActions<SmokeTestBackgroundWorker<TDbContext, TDbSetChecker>>
        where TDbContext : DbContext
        where TDbSetChecker : AbstractDbSetChecker<TDbContext>
    {
        private readonly Action<ILogger, Exception?> _startingTestOfDbSet;
        private readonly Action<ILogger, Exception?> _completedTestOfDbSet;
        private readonly Action<ILogger, Exception?> _testOfDbSetFailed;

        /// <summary>
        /// Initializes a new instance of the <see cref="SmokeTestBackgroundWorkerLogMessageActions{TDbContext, TDbSetChecker}"/> class.
        /// </summary>
        public SmokeTestBackgroundWorkerLogMessageActions()
        {
            _startingTestOfDbSet = LoggerMessage.Define(
                LogLevel.Information,
                WhipstaffEventIdFactory.TestOfDbSetStarting(),
                "Starting test of DBSet");

            _completedTestOfDbSet = LoggerMessage.Define(
                LogLevel.Information,
                WhipstaffEventIdFactory.TestOfDbSetCompleted(),
                "Completed test of DBSet");

            _testOfDbSetFailed = LoggerMessage.Define(
                LogLevel.Information,
                WhipstaffEventIdFactory.TestOfDbSetFailed(),
                "Starting test of DBSet");
        }

        /// <summary>
        /// Logging event for when the test of DB Sets is starting.
        /// </summary>
        /// <param name="logger">Logging framework instance.</param>
        public void StartingTestOfDbSet(ILogger<SmokeTestBackgroundWorker<TDbContext, TDbSetChecker>> logger)
        {
            _startingTestOfDbSet(logger, null);
        }

        /// <summary>
        /// Logging event for when the test of DB Sets has completed.
        /// </summary>
        /// <param name="logger">Logging framework instance.</param>
        public void CompletedTestOfDbSet(ILogger<SmokeTestBackgroundWorker<TDbContext, TDbSetChecker>> logger)
        {
            _completedTestOfDbSet(logger, null);
        }

        /// <summary>
        /// Logging event for when the test of DB Sets has failed.
        /// </summary>
        /// <param name="logger">Logging framework instance.</param>
        /// <param name="exception">The exception that occurred.</param>
        public void TestOfDbSetFailed(ILogger<SmokeTestBackgroundWorker<TDbContext, TDbSetChecker>> logger, Exception exception)
        {
            _testOfDbSetFailed(logger, exception);
        }
    }
}
