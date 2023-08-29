// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Microsoft.EntityFrameworkCore;

namespace Whipstaff.Testing.EntityFramework
{
    /// <summary>
    /// DB Context Factory for the Fake DB Context.
    /// </summary>
    public sealed class FakeDbContextFactory : IDbContextFactory<FakeDbContext>
    {
        private readonly string _databaseName;

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeDbContextFactory"/> class.
        /// </summary>
        public FakeDbContextFactory()
        {
            _databaseName = Guid.NewGuid().ToString();
        }

        /// <inheritdoc/>
        public FakeDbContext CreateDbContext()
        {
            var dbContextOptions = new DbContextOptionsBuilder<FakeDbContext>()
                .UseInMemoryDatabase(_databaseName)
                .Options;

            return new FakeDbContext(dbContextOptions);
        }
    }
}
