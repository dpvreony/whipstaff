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
    /// Log Message Actions Wrapper for DBSet checks.
    /// </summary>
    /// <typeparam name="TDbContext">The type for the DbContext.</typeparam>
    /// <typeparam name="TDbSetChecker">The type for the DbSet checker.</typeparam>
    public sealed class SmokeTestBackgroundWorkerLogMessageActionsWrapper<TDbContext, TDbSetChecker> : AbstractLogMessageActionsWrapper<SmokeTestBackgroundWorker<TDbContext, TDbSetChecker>, SmokeTestBackgroundWorkerLogMessageActions<TDbContext, TDbSetChecker>>
        where TDbContext : DbContext
        where TDbSetChecker : AbstractDbSetChecker<TDbContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SmokeTestBackgroundWorkerLogMessageActionsWrapper{TDbContext, TDbSetChecker}"/> class.
        /// </summary>
        /// <param name="logMessageActions">Log Message Actions instance.</param>
        /// <param name="logger">Logging framework instance.</param>
        public SmokeTestBackgroundWorkerLogMessageActionsWrapper(
            SmokeTestBackgroundWorkerLogMessageActions<TDbContext, TDbSetChecker> logMessageActions,
#pragma warning disable S6672
            ILogger<SmokeTestBackgroundWorker<TDbContext, TDbSetChecker>> logger)
#pragma warning restore S6672
            : base(logMessageActions, logger)
        {
        }

        /// <summary>
        /// Logs the start of a test of a DBSet.
        /// </summary>
        public void StartingDbSetChecker()
        {
            BrowserInstanceLogMessageActions.StartingTestOfDbSet(Logger);
        }

        /// <summary>
        /// Logs the completion of a test of a DBSet.
        /// </summary>
        public void CompletedDbSetChecker()
        {
            BrowserInstanceLogMessageActions.CompletedTestOfDbSet(Logger);
        }

        /// <summary>
        /// Logs a failure during a test of a DBSet.
        /// </summary>
        /// <param name="exception">The exception that occurred.</param>
        public void FailureOfDbSetChecker(Exception exception)
        {
            BrowserInstanceLogMessageActions.TestOfDbSetFailed(Logger, exception);
        }
    }
}
