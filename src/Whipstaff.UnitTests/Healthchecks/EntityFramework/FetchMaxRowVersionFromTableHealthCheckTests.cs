// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.VisualBasic.Logging;
using NetTestRegimentation;
using Whipstaff.Healthchecks.EntityFramework;
using Whipstaff.Testing.EntityFramework;
using Whipstaff.Testing.EntityFramework.DbSets;
using Whipstaff.Testing.Logging;
using Xunit;

namespace Whipstaff.UnitTests.Healthchecks.EntityFramework
{
    /// <summary>
    /// Unit tests for the <see ceef="FetchMaxRowVersionFromTableHealthCheck{TDbContext, TEntity}" /> class.
    /// </summary>
    public static class FetchMaxRowVersionFromTableHealthCheckTests
    {
        /// <summary>
        /// Unit tests for the <see cref="FetchMaxRowVersionFromTableHealthCheck{TDbContext, TEntity}" /> constructor.
        /// </summary>
        public sealed class ConstructorMethod : TestWithLoggingBase, ITestConstructorMethodWithNullableParameters<IDbContextFactory<FakeDbContext>>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ConstructorMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output helper instance.</param>
            public ConstructorMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <inheritdoc/>
            [Fact]
            public void ReturnsInstance()
            {
                Assert.NotNull(new FetchMaxRowVersionFromTableHealthCheck<FakeDbContext, FakeAddAuditDbSet>(new FakeDbContextFactory(LoggerFactory)));
            }

            /// <inheritdoc/>
            [Theory]
            [ClassData(typeof(ThrowsArgumentNullExceptionTestSource))]
            public void ThrowsArgumentNullException(IDbContextFactory<FakeDbContext>? arg, string expectedParameterNameForException)
            {
                var exception = Assert.Throws<ArgumentNullException>(() => new FetchMaxRowVersionFromTableHealthCheck<FakeDbContext, FakeAddAuditDbSet>(arg!));
                Assert.Equal(expectedParameterNameForException, exception.ParamName);
            }
        }

        /// <summary>
        /// Unit tests for the <see cref="FetchMaxRowVersionFromTableHealthCheck{TDbContext, TEntity}.CheckHealthAsync" /> method.
        /// </summary>
        public sealed class CheckHealthAsyncMethod : TestWithLoggingBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="CheckHealthAsyncMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output helper instance.</param>
            public CheckHealthAsyncMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <summary>
            /// Check that the health check returns healthy when the table is not empty.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsHealthyAsync()
            {
                var dbContextFactory = new FakeDbContextFactory(LoggerFactory);
                using (var dbContext = dbContextFactory.CreateDbContext())
                {
                    _ = await dbContext.FakeAddAudit.AddAsync(new FakeAddAuditDbSet { Value = 1, RowVersion = 1 }, TestContext.Current.CancellationToken);

                    _ = await dbContext.SaveChangesAsync(TestContext.Current.CancellationToken);
                }

                var instance = new FetchMaxRowVersionFromTableHealthCheck<FakeDbContext, FakeAddAuditDbSet>(dbContextFactory);

                var context = new HealthCheckContext();

                var result = await instance.CheckHealthAsync(context, TestContext.Current.CancellationToken);

                Assert.Equal(HealthStatus.Healthy, result.Status);
            }

            /// <summary>
            /// Check that the health check returns degraded when the table is empty.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsDegradedAsync()
            {
                var instance = new FetchMaxRowVersionFromTableHealthCheck<FakeDbContext, FakeAddAuditDbSet>(new FakeDbContextFactory(LoggerFactory));

                var context = new HealthCheckContext();

                var result = await instance.CheckHealthAsync(context, TestContext.Current.CancellationToken);

                Assert.Equal(HealthStatus.Degraded, result.Status);
            }
        }

        /// <summary>
        /// Test Source for <see cref="ConstructorMethod.ThrowsArgumentNullException(IDbContextFactory{FakeDbContext}, string)"/>.
        /// </summary>
        public sealed class ThrowsArgumentNullExceptionTestSource : TheoryData<IDbContextFactory<FakeDbContext>?, string>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ThrowsArgumentNullExceptionTestSource"/> class.
            /// </summary>
            public ThrowsArgumentNullExceptionTestSource()
            {
                Add(null, "dbContextFactory");
            }
        }
    }
}
