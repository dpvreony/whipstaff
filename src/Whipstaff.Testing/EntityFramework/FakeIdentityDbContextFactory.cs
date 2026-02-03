// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Whipstaff.Testing.EntityFramework
{
    /// <summary>
    /// DB Context Factory for the Fake Identity DB Context.
    /// </summary>
    public sealed class FakeIdentityDbContextFactory : AbstractDbContextFactory<FakeIdentityDbContext>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FakeIdentityDbContextFactory"/> class.
        /// </summary>
        /// <param name="loggerFactory">Instance of the logger factory.</param>
#pragma warning disable GR0027 // Constructor should have a logging framework instance as the final parameter.
        public FakeIdentityDbContextFactory(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
        }
#pragma warning restore GR0027 // Constructor should have a logging framework instance as the final parameter.

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeIdentityDbContextFactory"/> class.
        /// </summary>
        /// <param name="dbConnection">Database connection instance.</param>
        /// <param name="loggerFactory">Instance of the logger factory.</param>
#pragma warning disable GR0027 // Constructor should have a logging framework instance as the final parameter.
        public FakeIdentityDbContextFactory(DbConnection dbConnection, ILoggerFactory loggerFactory)
            : base(
                dbConnection,
                loggerFactory)
        {
        }
#pragma warning restore GR0027 // Constructor should have a logging framework instance as the final parameter.

        /// <inheritdoc/>
        protected override FakeIdentityDbContext GetDbContext(DbContextOptions<FakeIdentityDbContext> dbContextOptions)
        {
            return new FakeIdentityDbContext(
                dbContextOptions,
                () => new SqliteFakeDbContextModelCreator<FakeIdentityDbContext>());
        }
    }
}
