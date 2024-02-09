// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Whipstaff.EntityFramework.Reflection;
using Whipstaff.Testing.EntityFramework;
using Xunit;

namespace Whipstaff.UnitTests.EntityFramework.Reflection
{
    /// <summary>
    /// Unit tests for the <see cref="ReflectedDbContextFactory"/> class.
    /// </summary>
    public static class ReflectedDbContextFactoryTests
    {
        /// <summary>
        /// Unit tests for the <see cref="ReflectedDbContextFactory.GetDesignTimeDbContextFactoryFromAssembly(System.Reflection.Assembly, string)"/> method.
        /// </summary>
        public sealed class GetDesignTimeDbContextFactoryFromAssemblyMethod
        {
            /// <summary>
            /// Test to ensure the method returns a DbContext when the factory is found in an assembly.
            /// </summary>
            [Fact]
            public void ReturnsDbContext()
            {
                var assembly = typeof(FakeDesignTimeDbContextFactory).Assembly;
                var dbContext = ReflectedDbContextFactory.GetDesignTimeDbContextFactoryFromAssembly(assembly, typeof(FakeDbContext).FullName!);

                Assert.NotNull(dbContext);
            }
        }
    }
}
