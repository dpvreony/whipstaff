// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.Logging;
using Whipstaff.Core.Logging;

namespace Whipstaff.EntityFramework.SmokeTest
{
    /// <summary>
    /// Log Message Actions for DBSet checks.
    /// </summary>
    /// <typeparam name="TDbContext">The type for the DBContext being tested.</typeparam>
    public sealed class DbSetCheckerLogMessageActions<TDbContext> : ILogMessageActions<AbstractDbSetChecker<TDbContext>>
    {
        private Action<ILogger, Type, Exception?> _startingTestOfDbSet;
        private Action<ILogger, Type, Exception?> _testOfDbSetFailed;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbSetCheckerLogMessageActions{TDbContext}"/> class.
        /// </summary>
        public DbSetCheckerLogMessageActions()
        {
            _startingTestOfDbSet = LoggerMessage.Define<Type>(
                Microsoft.Extensions.Logging.LogLevel.Information,
                EventIdFactory.StartingTestOfDbSet(),
                "Starting Test of DBSet for {EntityType}");

            _testOfDbSetFailed = LoggerMessage.Define<Type>(
                Microsoft.Extensions.Logging.LogLevel.Error,
                EventIdFactory.TestOfDbSetFailed(),
                "Failed during test of DBSet for {EntityType}");
        }

        public void StartingTestOfDbSet(ILogger logger, Type type)
        {
            _startingTestOfDbSet(logger, type, null);
        }

        public void TestOfDbSetFailed(ILogger logger, Type type, Exception exception)
        {
            _testOfDbSetFailed(logger, type, exception);
        }
    }
}
