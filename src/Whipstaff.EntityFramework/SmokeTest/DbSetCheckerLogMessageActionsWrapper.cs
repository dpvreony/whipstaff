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
    /// Log Message Actions for DBSet checks.
    /// </summary>
    /// <typeparam name="TDbContext">The type for the DbContext.</typeparam>
    public sealed class DbSetCheckerLogMessageActionsWrapper<TDbContext> : AbstractLogMessageActionsWrapper<AbstractDbSetChecker<TDbContext>, DbSetCheckerLogMessageActions<TDbContext>>
        where TDbContext : DbContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DbSetCheckerLogMessageActionsWrapper{TDbContext}"/> class.
        /// </summary>
        /// <param name="logMessageActions">Log Message Actions instance.</param>
        /// <param name="logger">Logging framework instance.</param>
        public DbSetCheckerLogMessageActionsWrapper(
            DbSetCheckerLogMessageActions<TDbContext> logMessageActions,
#pragma warning disable S6672
            ILogger<AbstractDbSetChecker<TDbContext>> logger)
#pragma warning restore S6672
            : base(logMessageActions, logger)
        {
        }

        /// <summary>
        /// Logs the start of a test of a DBSet.
        /// </summary>
        /// <param name="type">The type of the DbSet being tested.</param>
        public void StartingTestOfDbSet(Type type)
        {
            LogMessageActions.StartingTestOfDbSet(Logger, type);
        }

        /// <summary>
        /// Logs a failure during a test of a DBSet.
        /// </summary>
        /// <param name="exception">The exception that occurred.</param>
        /// <param name="type">The type of the DbSet being tested.</param>
        public void FailedToTestDbSet(Exception exception, Type type)
        {
            LogMessageActions.TestOfDbSetFailed(Logger, type, exception);
        }
    }
}
