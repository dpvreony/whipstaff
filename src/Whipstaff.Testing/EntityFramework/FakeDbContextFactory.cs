// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Whipstaff.Testing.EntityFramework
{
    /// <summary>
    /// DB Context Factory for the Fake DB Context.
    /// </summary>
    public sealed class FakeDbContextFactory : IDbContextFactory<FakeDbContext>
    {
        private readonly string _databaseName;
        private readonly ILoggerFactory _loggerFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeDbContextFactory"/> class.
        /// </summary>
        /// <param name="loggerFactory">Instance of the logger factory.</param>
        public FakeDbContextFactory(ILoggerFactory loggerFactory)
        {
            ArgumentNullException.ThrowIfNull(loggerFactory);
            _databaseName = Guid.NewGuid().ToString();
            _loggerFactory = loggerFactory;
        }

        /// <inheritdoc/>
        public FakeDbContext CreateDbContext()
        {
            var dbContextOptions = new DbContextOptionsBuilder<FakeDbContext>()
                .UseInMemoryDatabase(_databaseName)
                .UseLoggerFactory(_loggerFactory)
                .Options;

            return new FakeDbContext(dbContextOptions);
        }
    }
}
