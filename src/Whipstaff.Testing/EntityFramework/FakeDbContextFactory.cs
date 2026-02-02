// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Whipstaff.Testing.EntityFramework
{
    /// <summary>
    /// DB Context Factory for the Fake DB Context.
    /// </summary>
    public sealed class FakeDbContextFactory : AbstractDbContextFactory<FakeDbContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FakeDbContextFactory"/> class.
        /// </summary>
        /// <param name="loggerFactory">Instance of the logger factory.</param>
#pragma warning disable GR0027 // Constructor should have a logging framework instance as the final parameter.
        public FakeDbContextFactory(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
        }
#pragma warning restore GR0027 // Constructor should have a logging framework instance as the final parameter.

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeDbContextFactory"/> class.
        /// </summary>
        /// <param name="dbConnection">Database connection instance.</param>
        /// <param name="loggerFactory">Instance of the logger factory.</param>
#pragma warning disable GR0027 // Constructor should have a logging framework instance as the final parameter.
        public FakeDbContextFactory(DbConnection dbConnection, ILoggerFactory loggerFactory)
            : base(
                dbConnection,
                loggerFactory)
        {
        }
#pragma warning restore GR0027 // Constructor should have a logging framework instance as the final parameter.

        /// <inheritdoc/>
        protected override FakeDbContext GetDbContext(DbContextOptions<FakeDbContext> dbContextOptions)
        {
            return new FakeDbContext(
                dbContextOptions,
                () => new SqliteFakeDbContextModelCreator<FakeDbContext>());
        }
    }
}
