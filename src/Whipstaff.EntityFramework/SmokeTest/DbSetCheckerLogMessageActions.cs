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
    /// <typeparam name="TDbContext">The type for the DBContext being tested.</typeparam>
    public sealed class DbSetCheckerLogMessageActions<TDbContext> : ILogMessageActions<AbstractDbSetChecker<TDbContext>>
        where TDbContext : DbContext
    {
        private readonly Action<ILogger, Type, Exception?> _startingTestOfDbSet;
        private readonly Action<ILogger, Type, Exception?> _testOfDbSetFailed;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbSetCheckerLogMessageActions{TDbContext}"/> class.
        /// </summary>
        public DbSetCheckerLogMessageActions()
        {
            _startingTestOfDbSet = LoggerMessage.Define<Type>(
                Microsoft.Extensions.Logging.LogLevel.Information,
                WhipstaffEventIdFactory.TestOfDbSetStarting(),
                "Starting Test of DBSet for {EntityType}");

            _testOfDbSetFailed = LoggerMessage.Define<Type>(
                Microsoft.Extensions.Logging.LogLevel.Error,
                WhipstaffEventIdFactory.TestOfDbSetFailed(),
                "Failed during test of DBSet for {EntityType}");
        }

        /// <summary>
        /// Logs the start of a test of a DBSet.
        /// </summary>
        /// <param name="logger">Logging framework instance.</param>
        /// <param name="type">The type of the db set being tested.</param>
        public void StartingTestOfDbSet(ILogger logger, Type type)
        {
            _startingTestOfDbSet(logger, type, null);
        }

        /// <summary>
        /// Logs a failure during a test of a DBSet.
        /// </summary>
        /// <param name="logger">Logging framework instance.</param>
        /// <param name="type">The type of the db set being tested.</param>
        /// <param name="exception">The exception that occurred.</param>
        public void TestOfDbSetFailed(ILogger logger, Type type, Exception exception)
        {
            _testOfDbSetFailed(logger, type, exception);
        }
    }
}
