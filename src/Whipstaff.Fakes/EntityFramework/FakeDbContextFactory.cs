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
        /// <inheritdoc/>
        public FakeDbContext CreateDbContext()
        {
            var dbContextOptions = new DbContextOptionsBuilder<FakeDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new FakeDbContext(dbContextOptions);
        }
    }
}
