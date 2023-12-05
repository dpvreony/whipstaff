// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.EntityFrameworkCore;
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
    }
}
