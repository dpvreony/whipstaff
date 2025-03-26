// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Whipstaff.EntityFramework;
using Whipstaff.Testing.Logging;
using Xunit;

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
        public sealed class GetMaxRowVersionCompiledAsyncQueryMethod : TestWithLoggingBase
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
        public sealed class GetMaxRowVersionCompiledQueryMethod : TestWithLoggingBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="GetMaxRowVersionCompiledQueryMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit logging output helper.</param>
            public GetMaxRowVersionCompiledQueryMethod(ITestOutputHelper output)
                : base(output)
            {
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="CompiledQueryFactory.GetWhereRowVersionBetweenCompiledAsyncQuery{TDbContext, TDbSet}"/>.
        /// </summary>
        public sealed class GetWhereRowVersionBetweenCompiledAsyncQueryMethod : TestWithLoggingBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="GetWhereRowVersionBetweenCompiledAsyncQueryMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit logging output helper.</param>
            public GetWhereRowVersionBetweenCompiledAsyncQueryMethod(ITestOutputHelper output)
                : base(output)
            {
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="CompiledQueryFactory.GetWhereRowVersionGreaterThanCompiledAsyncQuery{TDbContext, TDbSet}"/>.
        /// </summary>
        public sealed class GetWhereRowVersionGreaterThanCompiledAsyncQueryMethod : TestWithLoggingBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="GetWhereRowVersionGreaterThanCompiledAsyncQueryMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit logging output helper.</param>
            public GetWhereRowVersionGreaterThanCompiledAsyncQueryMethod(ITestOutputHelper output)
                : base(output)
            {
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="CompiledQueryFactory.GetWhereUniqueIntIdEqualsCompiledAsyncQuery{TDbContext, TDbSet}"/>.
        /// </summary>
        public sealed class GetWhereUniqueIntIdEqualsCompiledAsyncQueryMethod : TestWithLoggingBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="GetWhereUniqueIntIdEqualsCompiledAsyncQueryMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit logging output helper.</param>
            public GetWhereUniqueIntIdEqualsCompiledAsyncQueryMethod(ITestOutputHelper output)
                : base(output)
            {
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="CompiledQueryFactory.GetWhereUniqueIntIdEqualsCompiledQuery{TDbContext, TDbSet}"/>.
        /// </summary>
        public sealed class GetWhereUniqueIntIdEqualsCompiledQueryMethod : TestWithLoggingBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="GetWhereUniqueIntIdEqualsCompiledQueryMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit logging output helper.</param>
            public GetWhereUniqueIntIdEqualsCompiledQueryMethod(ITestOutputHelper output)
                : base(output)
            {
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="CompiledQueryFactory.GetWhereUniqueLongIdEqualsCompiledAsyncQuery{TDbContext, TDbSet}"/>.
        /// </summary>
        public sealed class GetWhereUniqueLongIdEqualsCompiledAsyncQueryMethod : TestWithLoggingBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="GetWhereUniqueLongIdEqualsCompiledAsyncQueryMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit logging output helper.</param>
            public GetWhereUniqueLongIdEqualsCompiledAsyncQueryMethod(ITestOutputHelper output)
                : base(output)
            {
            }
        }

        /// <summary>
        /// Unit Tests for <see cref="CompiledQueryFactory.GetWhereUniqueLongIdEqualsCompiledQuery{TDbContext, TDbSet}"/>.
        /// </summary>
        public sealed class GetWhereUniqueLongIdEqualsCompiledQueryMethod : TestWithLoggingBase
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="GetWhereUniqueLongIdEqualsCompiledQueryMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit logging output helper.</param>
            public GetWhereUniqueLongIdEqualsCompiledQueryMethod(ITestOutputHelper output)
                : base(output)
            {
            }
        }
    }
}
