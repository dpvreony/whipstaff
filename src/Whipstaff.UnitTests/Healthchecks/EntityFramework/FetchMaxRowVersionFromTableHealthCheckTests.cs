// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Microsoft.EntityFrameworkCore;
using NetTestRegimentation;
using Whipstaff.Healthchecks.EntityFramework;
using Whipstaff.Testing.EntityFramework;
using Whipstaff.Testing.EntityFramework.DbSets;
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
        public sealed class ConstructorMethod : ITestConstructorMethodWithNullableParameters<IDbContextFactory<FakeDbContext>>
        {
            /// <inheritdoc/>
            [Fact]
            public void ReturnsInstance()
            {
                Assert.NotNull(new FetchMaxRowVersionFromTableHealthCheck<FakeDbContext, FakeAddAuditDbSet>(new FakeDbContextFactory()));
            }

            /// <inheritdoc/>
            [Theory]
            [ClassData(typeof(ThrowsArgumentNullExceptionTestSource))]
            public void ThrowsArgumentNullException(IDbContextFactory<FakeDbContext> arg, string expectedParameterNameForException)
            {
                var exception = Assert.Throws<ArgumentNullException>(() => new FetchMaxRowVersionFromTableHealthCheck<FakeDbContext, FakeAddAuditDbSet>(arg));
                Assert.Equal(expectedParameterNameForException, exception.ParamName);
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
