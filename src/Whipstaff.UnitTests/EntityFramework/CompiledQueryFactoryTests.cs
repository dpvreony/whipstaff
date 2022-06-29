// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Whipstaff.EntityFramework;
using Xunit.Abstractions;

namespace Whipstaff.UnitTests.EntityFramework
{
    /// <summary>
    /// Unit Tests for <see cref="CompiledQueryFactory"/>.
    /// </summary>
    public static class CompiledQueryFactoryTests
    {
        /// <summary>
        /// Unit Tests for <see cref="CompiledQueryFactory.GetMaxRowVersionCompiledAsyncQuery{TDbContext, TDbSet}"/>.
        /// </summary>
        public sealed class GetMaxRowVersionCompiledAsyncQueryMethod : Foundatio.Xunit.TestWithLoggingBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="GetMaxRowVersionCompiledAsyncQueryMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit logging output helper.</param>
            public GetMaxRowVersionCompiledAsyncQueryMethod(ITestOutputHelper output)
                : base(output)
            {
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="CompiledQueryFactory.GetMaxRowVersionCompiledQuery{TDbContext, TDbSet}"/>.
        /// </summary>
        public sealed class GetMaxRowVersionCompiledQueryMethod
        {
        }

        /// <summary>
        /// Unit Tests for <see cref="CompiledQueryFactory.GetWhereRowVersionBetweenCompiledAsyncQuery{TDbContext, TDbSet}"/>.
        /// </summary>
        public sealed class GetWhereRowVersionBetweenCompiledAsyncQueryMethod
        {
        }

        /// <summary>
        /// Unit Tests for <see cref="CompiledQueryFactory.GetWhereRowVersionGreaterThanCompiledAsyncQuery{TDbContext, TDbSet}"/>.
        /// </summary>
        public sealed class GetWhereRowVersionGreaterThanCompiledAsyncQueryMethod
        {
        }

        /// <summary>
        /// Unit Tests for <see cref="CompiledQueryFactory.GetWhereUniqueIntIdEqualsCompiledAsyncQuery{TDbContext, TDbSet}"/>.
        /// </summary>
        public sealed class GetWhereUniqueIntIdEqualsCompiledAsyncQueryMethod
        {
        }

        /// <summary>
        /// Unit Tests for <see cref="CompiledQueryFactory.GetWhereUniqueIntIdEqualsCompiledQuery{TDbContext, TDbSet}"/>.
        /// </summary>
        public sealed class GetWhereUniqueIntIdEqualsCompiledQueryMethod
        {
        }

        /// <summary>
        /// Unit Tests for <see cref="CompiledQueryFactory.GetWhereUniqueLongIdEqualsCompiledAsyncQuery{TDbContext, TDbSet}"/>.
        /// </summary>
        public sealed class GetWhereUniqueLongIdEqualsCompiledAsyncQueryMethod
        {
        }

        /// <summary>
        /// Unit Tests for <see cref="CompiledQueryFactory.GetWhereUniqueLongIdEqualsCompiledQuery{TDbContext, TDbSet}"/>.
        /// </summary>
        public sealed class GetWhereUniqueLongIdEqualsCompiledQueryMethod
        {
        }
    }
}
