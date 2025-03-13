// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Couchbase;
using Couchbase.Extensions.DependencyInjection;
using Couchbase.KeyValue;
using NetTestRegimentation;
using Whipstaff.Couchbase;
using Whipstaff.Testing.Logging;
using Xunit;

namespace Whipstaff.UnitTests.Couchbase
{
    /// <summary>
    /// Unit tests for the <see cref="CouchbaseLockProvider"/> class.
    /// </summary>
    public static class CouchbaseLockProviderTests
    {
        /// <summary>
        /// Unit test for the <see cref="CouchbaseLockProvider"/> constructor.
        /// </summary>
        public sealed class ConstructorMethod
            : TestWithLoggingBase,
                ITestMethodWithNullableParameters<ICouchbaseCollection, CouchbaseLockProviderLogMessageActionsWrapper>
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
            public void ThrowsArgumentNullException(
                ICouchbaseCollection arg1,
                CouchbaseLockProviderLogMessageActionsWrapper arg2,
                string expectedParameterNameForException)
            {
                _ = Assert.Throws<ArgumentNullException>(
                    expectedParameterNameForException,
                    () => new CouchbaseLockProvider(
                        arg1,
                        arg2));
            }
        }

        /// <summary>
        /// Unit test for the <see cref="CouchbaseLockProvider.GetInstanceAsync(IBucketProvider, CouchbaseLockProviderLogMessageActionsWrapper)"/> method.
        /// </summary>
        public sealed class GetInstanceAsyncMethod
            : TestWithLoggingBase,
                ITestAsyncMethodWithNullableParameters<IBucketProvider, CouchbaseLockProviderLogMessageActionsWrapper>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="GetInstanceAsyncMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output helper instance.</param>
            public GetInstanceAsyncMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <inheritdoc/>
            public async Task ThrowsArgumentNullExceptionAsync(
                IBucketProvider arg1,
                CouchbaseLockProviderLogMessageActionsWrapper arg2,
                string expectedParameterNameForException)
            {
                _ = await Assert.ThrowsAsync<ArgumentNullException>(
                    expectedParameterNameForException,
                    () => CouchbaseLockProvider.GetInstanceAsync(
                        arg1,
                        arg2));
            }
        }

        /// <summary>
        /// Unit test for the <see cref="CouchbaseLockProvider.GetInstance(IBucket, CouchbaseLockProviderLogMessageActionsWrapper)"/> method.
        /// </summary>
        public sealed class GetInstanceMethod
            : TestWithLoggingBase,
                ITestMethodWithNullableParameters<IBucket, CouchbaseLockProviderLogMessageActionsWrapper>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="GetInstanceMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output helper instance.</param>
            public GetInstanceMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <inheritdoc/>
            public void ThrowsArgumentNullException(
                IBucket arg1,
                CouchbaseLockProviderLogMessageActionsWrapper arg2,
                string expectedParameterNameForException)
            {
                _ = Assert.Throws<ArgumentNullException>(
                    expectedParameterNameForException,
                    () => CouchbaseLockProvider.GetInstance(
                        arg1,
                        arg2));
            }
        }

        /// <summary>
        /// Unit test for the <see cref="CouchbaseLockProvider.AcquireAsync(string, TimeSpan?, bool, CancellationToken)"/> method.
        /// </summary>
        public sealed class AcquireAsyncMethod
            : TestWithLoggingBase,
                ITestMethodWithNullableParameters<string>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="AcquireAsyncMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output helper instance.</param>
            public AcquireAsyncMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <inheritdoc/>
            public void ThrowsArgumentNullException(string arg, string expectedParameterNameForException)
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Unit test for the <see cref="CouchbaseLockProvider.IsLockedAsync(string)"/> method.
        /// </summary>
        public sealed class IsLockedAsyncMethod
            : TestWithLoggingBase,
                ITestMethodWithNullableParameters<string>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="IsLockedAsyncMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output helper instance.</param>
            public IsLockedAsyncMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <inheritdoc/>
            public void ThrowsArgumentNullException(string arg, string expectedParameterNameForException)
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Unit test for the <see cref="CouchbaseLockProvider.ReleaseAsync(string, string)"/> method.
        /// </summary>
        public sealed class ReleaseAsyncMethod
            : TestWithLoggingBase,
                ITestMethodWithNullableParameters<string, string>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ReleaseAsyncMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output helper instance.</param>
            public ReleaseAsyncMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <inheritdoc/>
            public void ThrowsArgumentNullException(string arg1, string arg2, string expectedParameterNameForException)
            {
                throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Unit test for the <see cref="CouchbaseLockProvider.RenewAsync(string, string, TimeSpan?)"/> method.
        /// </summary>
        public sealed class RenewAsyncMethod
            : TestWithLoggingBase,
                ITestMethodWithNullableParameters<string, string>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="RenewAsyncMethod"/> class.
            /// </summary>
            /// <param name="output">XUnit test output helper instance.</param>
            public RenewAsyncMethod(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <inheritdoc/>
            public void ThrowsArgumentNullException(string arg1, string arg2, string expectedParameterNameForException)
            {
                throw new NotImplementedException();
            }
        }
    }
}
