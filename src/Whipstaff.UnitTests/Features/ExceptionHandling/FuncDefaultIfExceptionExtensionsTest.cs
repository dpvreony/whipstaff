// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using NetTestRegimentation;
using Whipstaff.Core.ExceptionHandling;
using Xunit;

namespace Whipstaff.UnitTests.Features.ExceptionHandling
{
    /// <summary>
    /// Unit Tests for the Functional Default If Exception Helper.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public static class FuncDefaultIfExceptionExtensionsTest
    {
        /// <summary>
        /// Unit tests for the default if exception handler that executes a func that takes no parameters.
        /// </summary>
        public sealed class DefaultIfExceptionAsyncNoArgsMethod : ITestAsyncMethodWithNullableParameters<Func<Task<int>>>
        {
            /// <inheritdoc/>
            [Theory]
            [ClassData(typeof(ThrowsArgumentNullExceptionAsyncTestData))]
            public async Task ThrowsArgumentNullExceptionAsync(
                Func<Task<int>> arg,
                string expectedParameterNameForException)
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => arg.DefaultIfExceptionAsync(-1))
;
                Assert.Equal(expectedParameterNameForException, exception.ParamName);
            }

            /// <summary>
            /// Test to ensure a value is returned, if the execution succeeds.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsValueAsync()
            {
                await ReturnsFromTask(ReturnsValue, -1, 1);
            }

            /// <summary>
            /// Test to ensure a default value is returned, if the execution fails.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsDefaultAsync()
            {
                await ReturnsFromTask(Throws, -1, -1);
            }

            private static async Task ReturnsFromTask(
                Func<Task<int>> func,
                int defaultResult,
                int expected)
            {
                var result = await func.DefaultIfExceptionAsync(defaultResult)
                    .ConfigureAwait(false);
                Assert.Equal(expected, result);
            }

            private static Task<int> ReturnsValue() => Task.FromResult(1);

            private static Task<int> Throws() => throw new NotImplementedException();

            /// <summary>
            /// Data Source for the Unit Test ThrowsArgumentNullExceptionAsync.
            /// </summary>
            public sealed class ThrowsArgumentNullExceptionAsyncTestData
                : TheoryData<Func<Task<int>>?, string>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="ThrowsArgumentNullExceptionAsyncTestData"/> class.
                /// </summary>
                public ThrowsArgumentNullExceptionAsyncTestData()
                {
                    Add(null, "func");
                }
            }
        }

        /// <summary>
        /// Unit tests for the default if exception handler that executes a func that takes 1 parameter.
        /// </summary>
        public sealed class DefaultIfExceptionAsync1ArgMethod : ITestAsyncMethodWithNullableParameters<Func<int, Task<int>>>
        {
            /// <inheritdoc/>
            [Theory]
            [ClassData(typeof(ThrowsArgumentNullExceptionAsyncTestData))]
            public async Task ThrowsArgumentNullExceptionAsync(
                Func<int, Task<int>> arg,
                string expectedParameterNameForException)
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => arg.DefaultIfExceptionAsync(1, -1))
;
                Assert.Equal(expectedParameterNameForException, exception.ParamName);
            }

            /// <summary>
            /// Test to ensure a value is returned, if the execution succeeds.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsValueAsync()
            {
                await ReturnsFromTask(ReturnsValue, -1, 1);
            }

            /// <summary>
            /// Test to ensure a default value is returned, if the execution fails.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsDefaultAsync()
            {
                await ReturnsFromTask(Throws, -1, -1);
            }

            private static async Task ReturnsFromTask(
                Func<int, Task<int>> func,
                int defaultResult,
                int expected)
            {
                var result = await func.DefaultIfExceptionAsync(1, defaultResult)
                    .ConfigureAwait(false);
                Assert.Equal(expected, result);
            }

            private static Task<int> ReturnsValue(int arg) => Task.FromResult(arg);

            private static Task<int> Throws(int arg) => throw new NotImplementedException();

            /// <summary>
            /// Data Source for the Unit Test ThrowsArgumentNullExceptionAsync.
            /// </summary>
            public sealed class ThrowsArgumentNullExceptionAsyncTestData
                : TheoryData<Func<int, Task<int>>?, string>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="ThrowsArgumentNullExceptionAsyncTestData"/> class.
                /// </summary>
                public ThrowsArgumentNullExceptionAsyncTestData()
                {
                    Add(null, "func");
                }
            }
        }

        /// <summary>
        /// Unit tests for the default if exception handler that executes a func that takes 2 parameters.
        /// </summary>
        public sealed class DefaultIfExceptionAsync2ArgsMethod : ITestAsyncMethodWithNullableParameters<Func<int, int, Task<int>>>
        {
            /// <inheritdoc/>
            [Theory]
            [ClassData(typeof(ThrowsArgumentNullExceptionAsyncTestData))]
            public async Task ThrowsArgumentNullExceptionAsync(
                Func<int, int, Task<int>> arg,
                string expectedParameterNameForException)
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => arg.DefaultIfExceptionAsync(1, 2, -1))
;
                Assert.Equal(expectedParameterNameForException, exception.ParamName);
            }

            /// <summary>
            /// Test to ensure a value is returned, if the execution succeeds.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsValueAsync()
            {
                await ReturnsFromTask(ReturnsValue, -1, 1);
            }

            /// <summary>
            /// Test to ensure a default value is returned, if the execution fails.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsDefaultAsync()
            {
                await ReturnsFromTask(Throws, -1, -1);
            }

            private static async Task ReturnsFromTask(
                Func<int, int, Task<int>> func,
                int defaultResult,
                int expected)
            {
                var result = await func.DefaultIfExceptionAsync(1, 2, defaultResult)
                    .ConfigureAwait(false);
                Assert.Equal(expected, result);
            }

            private static Task<int> ReturnsValue(
                int arg1,
                int arg2) => Task.FromResult(arg1);

            private static Task<int> Throws(
                int arg1,
                int arg2) => throw new NotImplementedException();

            /// <summary>
            /// Data Source for the Unit Test ThrowsArgumentNullExceptionAsync.
            /// </summary>
            public sealed class ThrowsArgumentNullExceptionAsyncTestData
                : TheoryData<Func<int, int, Task<int>>?, string>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="ThrowsArgumentNullExceptionAsyncTestData"/> class.
                /// </summary>
                public ThrowsArgumentNullExceptionAsyncTestData()
                {
                    Add(null, "func");
                }
            }
        }

        /// <summary>
        /// Unit tests for the default if exception handler that executes a func that takes 3 parameters.
        /// </summary>
        public sealed class DefaultIfExceptionAsync3ArgsMethod : ITestAsyncMethodWithNullableParameters<Func<int, int, int, Task<int>>>
        {
            /// <inheritdoc/>
            [Theory]
            [ClassData(typeof(ThrowsArgumentNullExceptionAsyncTestData))]
            public async Task ThrowsArgumentNullExceptionAsync(
                Func<int, int, int, Task<int>> arg,
                string expectedParameterNameForException)
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => arg.DefaultIfExceptionAsync(1, 2, 3, -1))
;
                Assert.Equal(expectedParameterNameForException, exception.ParamName);
            }

            /// <summary>
            /// Test to ensure a value is returned, if the execution succeeds.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsValueAsync()
            {
                await ReturnsFromTask(ReturnsValue, -1, 1);
            }

            /// <summary>
            /// Test to ensure a default value is returned, if the execution fails.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsDefaultAsync()
            {
                await ReturnsFromTask(Throws, -1, -1);
            }

            private static async Task ReturnsFromTask(
                Func<int, int, int, Task<int>> func,
                int defaultResult,
                int expected)
            {
                var result = await func.DefaultIfExceptionAsync(1, 2, 3, defaultResult)
                    .ConfigureAwait(false);
                Assert.Equal(expected, result);
            }

            private static Task<int> ReturnsValue(
                int arg1,
                int arg2,
                int arg3) => Task.FromResult(arg1);

            private static Task<int> Throws(
                int arg1,
                int arg2,
                int arg3) => throw new NotImplementedException();

            /// <summary>
            /// Data Source for the Unit Test ThrowsArgumentNullExceptionAsync.
            /// </summary>
            public sealed class ThrowsArgumentNullExceptionAsyncTestData
                : TheoryData<Func<int, int, int, Task<int>>?, string>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="ThrowsArgumentNullExceptionAsyncTestData"/> class.
                /// </summary>
                public ThrowsArgumentNullExceptionAsyncTestData()
                {
                    Add(null, "func");
                }
            }
        }

        /// <summary>
        /// Unit tests for the default if exception handler that executes a func that takes 4 parameters.
        /// </summary>
        public sealed class DefaultIfExceptionAsync4ArgsMethod : ITestAsyncMethodWithNullableParameters<Func<int, int, int, int, Task<int>>>
        {
            /// <inheritdoc/>
            [Theory]
            [ClassData(typeof(ThrowsArgumentNullExceptionAsyncTestData))]
            public async Task ThrowsArgumentNullExceptionAsync(
                Func<int, int, int, int, Task<int>> arg,
                string expectedParameterNameForException)
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => arg.DefaultIfExceptionAsync(1, 2, 3, 4, -1))
;
                Assert.Equal(expectedParameterNameForException, exception.ParamName);
            }

            /// <summary>
            /// Test to ensure a value is returned, if the execution succeeds.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsValueAsync()
            {
                await ReturnsFromTask(ReturnsValue, -1, 1);
            }

            /// <summary>
            /// Test to ensure a default value is returned, if the execution fails.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsDefaultAsync()
            {
                await ReturnsFromTask(Throws, -1, -1);
            }

            private static async Task ReturnsFromTask(
                Func<int, int, int, int, Task<int>> func,
                int defaultResult,
                int expected)
            {
                var result = await func.DefaultIfExceptionAsync(1, 2, 3, 4, defaultResult)
                    .ConfigureAwait(false);
                Assert.Equal(expected, result);
            }

            private static Task<int> ReturnsValue(
                int arg1,
                int arg2,
                int arg3,
                int arg4) => Task.FromResult(arg1);

            private static Task<int> Throws(
                int arg1,
                int arg2,
                int arg3,
                int arg4) => throw new NotImplementedException();

            /// <summary>
            /// Data Source for the Unit Test ThrowsArgumentNullExceptionAsync.
            /// </summary>
            public sealed class ThrowsArgumentNullExceptionAsyncTestData
                : TheoryData<Func<int, int, int, int, Task<int>>?, string>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="ThrowsArgumentNullExceptionAsyncTestData"/> class.
                /// </summary>
                public ThrowsArgumentNullExceptionAsyncTestData()
                {
                    Add(null, "func");
                }
            }
        }

        /// <summary>
        /// Unit tests for the default if exception handler that executes a func that takes 5 parameters.
        /// </summary>
        public sealed class DefaultIfExceptionAsync5ArgsMethod : ITestAsyncMethodWithNullableParameters<Func<int, int, int, int, int, Task<int>>>
        {
            /// <inheritdoc/>
            [Theory]
            [ClassData(typeof(ThrowsArgumentNullExceptionAsyncTestData))]
            public async Task ThrowsArgumentNullExceptionAsync(
                Func<int, int, int, int, int, Task<int>> arg,
                string expectedParameterNameForException)
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => arg.DefaultIfExceptionAsync(1, 2, 3, 4, 5, -1))
;
                Assert.Equal(expectedParameterNameForException, exception.ParamName);
            }

            /// <summary>
            /// Test to ensure a value is returned, if the execution succeeds.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsValueAsync()
            {
                await ReturnsFromTask(ReturnsValue, -1, 1);
            }

            /// <summary>
            /// Test to ensure a default value is returned, if the execution fails.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsDefaultAsync()
            {
                await ReturnsFromTask(Throws, -1, -1);
            }

            private static async Task ReturnsFromTask(
                Func<int, int, int, int, int, Task<int>> func,
                int defaultResult,
                int expected)
            {
                var result = await func.DefaultIfExceptionAsync(1, 2, 3, 4, 5, defaultResult)
                    .ConfigureAwait(false);
                Assert.Equal(expected, result);
            }

            private static Task<int> ReturnsValue(
                int arg1,
                int arg2,
                int arg3,
                int arg4,
                int arg5) => Task.FromResult(arg1);

            private static Task<int> Throws(
                int arg1,
                int arg2,
                int arg3,
                int arg4,
                int arg5) => throw new NotImplementedException();

            /// <summary>
            /// Data Source for the Unit Test ThrowsArgumentNullExceptionAsync.
            /// </summary>
            public sealed class ThrowsArgumentNullExceptionAsyncTestData
                : TheoryData<Func<int, int, int, int, int, Task<int>>?, string>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="ThrowsArgumentNullExceptionAsyncTestData"/> class.
                /// </summary>
                public ThrowsArgumentNullExceptionAsyncTestData()
                {
                    Add(null, "func");
                }
            }
        }

        /// <summary>
        /// Unit tests for the default if exception handler that executes a func that takes 6 parameters.
        /// </summary>
        public sealed class DefaultIfExceptionAsync6ArgsMethod : ITestAsyncMethodWithNullableParameters<Func<int, int, int, int, int, int, Task<int>>>
        {
            /// <inheritdoc/>
            [Theory]
            [ClassData(typeof(ThrowsArgumentNullExceptionAsyncTestData))]
            public async Task ThrowsArgumentNullExceptionAsync(
                Func<int, int, int, int, int, int, Task<int>> arg,
                string expectedParameterNameForException)
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => arg.DefaultIfExceptionAsync(1, 2, 3, 4, 5, 6, -1))
;
                Assert.Equal(expectedParameterNameForException, exception.ParamName);
            }

            /// <summary>
            /// Test to ensure a value is returned, if the execution succeeds.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsValueAsync()
            {
                await ReturnsFromTask(ReturnsValue, -1, 1);
            }

            /// <summary>
            /// Test to ensure a default value is returned, if the execution fails.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsDefaultAsync()
            {
                await ReturnsFromTask(Throws, -1, -1);
            }

            private static async Task ReturnsFromTask(
                Func<int, int, int, int, int, int, Task<int>> func,
                int defaultResult,
                int expected)
            {
                var result = await func.DefaultIfExceptionAsync(1, 2, 3, 4, 5, 6, defaultResult)
                    .ConfigureAwait(false);
                Assert.Equal(expected, result);
            }

            private static Task<int> ReturnsValue(
                int arg1,
                int arg2,
                int arg3,
                int arg4,
                int arg5,
                int arg6) => Task.FromResult(arg1);

            private static Task<int> Throws(
                int arg1,
                int arg2,
                int arg3,
                int arg4,
                int arg5,
                int arg6) => throw new NotImplementedException();

            /// <summary>
            /// Data Source for the Unit Test ThrowsArgumentNullExceptionAsync.
            /// </summary>
            public sealed class ThrowsArgumentNullExceptionAsyncTestData
                : TheoryData<Func<int, int, int, int, int, int, Task<int>>?, string>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="ThrowsArgumentNullExceptionAsyncTestData"/> class.
                /// </summary>
                public ThrowsArgumentNullExceptionAsyncTestData()
                {
                    Add(null, "func");
                }
            }
        }

        /// <summary>
        /// Unit tests for the default if exception handler that executes a func that takes 7 parameters.
        /// </summary>
        public sealed class DefaultIfExceptionAsync7ArgsMethod : ITestAsyncMethodWithNullableParameters<Func<int, int, int, int, int, int, int, Task<int>>>
        {
            /// <inheritdoc/>
            [Theory]
            [ClassData(typeof(ThrowsArgumentNullExceptionAsyncTestData))]
            public async Task ThrowsArgumentNullExceptionAsync(
                Func<int, int, int, int, int, int, int, Task<int>> arg,
                string expectedParameterNameForException)
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => arg.DefaultIfExceptionAsync(1, 2, 3, 4, 5, 6, 7, -1))
;
                Assert.Equal(expectedParameterNameForException, exception.ParamName);
            }

            /// <summary>
            /// Test to ensure a value is returned, if the execution succeeds.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsValueAsync()
            {
                await ReturnsFromTask(ReturnsValue, -1, 1);
            }

            /// <summary>
            /// Test to ensure a default value is returned, if the execution fails.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsDefaultAsync()
            {
                await ReturnsFromTask(Throws, -1, -1);
            }

            private static async Task ReturnsFromTask(
                Func<int, int, int, int, int, int, int, Task<int>> func,
                int defaultResult,
                int expected)
            {
                var result = await func.DefaultIfExceptionAsync(1, 2, 3, 4, 5, 6, 7, defaultResult)
                    .ConfigureAwait(false);
                Assert.Equal(expected, result);
            }

            private static Task<int> ReturnsValue(
                int arg1,
                int arg2,
                int arg3,
                int arg4,
                int arg5,
                int arg6,
                int arg7) => Task.FromResult(arg1);

            private static Task<int> Throws(
                int arg1,
                int arg2,
                int arg3,
                int arg4,
                int arg5,
                int arg6,
                int arg7) => throw new NotImplementedException();

            /// <summary>
            /// Data Source for the Unit Test ThrowsArgumentNullExceptionAsync.
            /// </summary>
            public sealed class ThrowsArgumentNullExceptionAsyncTestData
                : TheoryData<Func<int, int, int, int, int, int, int, Task<int>>?, string>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="ThrowsArgumentNullExceptionAsyncTestData"/> class.
                /// </summary>
                public ThrowsArgumentNullExceptionAsyncTestData()
                {
                    Add(null, "func");
                }
            }
        }

        /// <summary>
        /// Unit tests for the default if exception handler that executes a func that takes 8 parameters.
        /// </summary>
        public sealed class DefaultIfExceptionAsync8ArgsMethod : ITestAsyncMethodWithNullableParameters<Func<int, int, int, int, int, int, int, int, Task<int>>>
        {
            /// <inheritdoc/>
            [Theory]
            [ClassData(typeof(ThrowsArgumentNullExceptionAsyncTestData))]
            public async Task ThrowsArgumentNullExceptionAsync(
                Func<int, int, int, int, int, int, int, int, Task<int>> arg,
                string expectedParameterNameForException)
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => arg.DefaultIfExceptionAsync(1, 2, 3, 4, 5, 6, 7, 8, -1))
;
                Assert.Equal(expectedParameterNameForException, exception.ParamName);
            }

            /// <summary>
            /// Test to ensure a value is returned, if the execution succeeds.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsValueAsync()
            {
                await ReturnsFromTask(ReturnsValue, -1, 1);
            }

            /// <summary>
            /// Test to ensure a default value is returned, if the execution fails.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsDefaultAsync()
            {
                await ReturnsFromTask(Throws, -1, -1);
            }

            private static async Task ReturnsFromTask(
                Func<int, int, int, int, int, int, int, int, Task<int>> func,
                int defaultResult,
                int expected)
            {
                var result = await func.DefaultIfExceptionAsync(1, 2, 3, 4, 5, 6, 7, 8, defaultResult)
                    .ConfigureAwait(false);
                Assert.Equal(expected, result);
            }

            private static Task<int> ReturnsValue(
                int arg1,
                int arg2,
                int arg3,
                int arg4,
                int arg5,
                int arg6,
                int arg7,
                int arg8) => Task.FromResult(arg1);

            private static Task<int> Throws(
                int arg1,
                int arg2,
                int arg3,
                int arg4,
                int arg5,
                int arg6,
                int arg7,
                int arg8) => throw new NotImplementedException();

            /// <summary>
            /// Data Source for the Unit Test ThrowsArgumentNullExceptionAsync.
            /// </summary>
            public sealed class ThrowsArgumentNullExceptionAsyncTestData
                : TheoryData<Func<int, int, int, int, int, int, int, int, Task<int>>?, string>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="ThrowsArgumentNullExceptionAsyncTestData"/> class.
                /// </summary>
                public ThrowsArgumentNullExceptionAsyncTestData()
                {
                    Add(null, "func");
                }
            }
        }

        /// <summary>
        /// Unit tests for the default if exception handler that executes a func that takes 9 parameters.
        /// </summary>
        public sealed class DefaultIfExceptionAsync9ArgsMethod : ITestAsyncMethodWithNullableParameters<Func<int, int, int, int, int, int, int, int, int, Task<int>>>
        {
            /// <inheritdoc/>
            [Theory]
            [ClassData(typeof(ThrowsArgumentNullExceptionAsyncTestData))]
            public async Task ThrowsArgumentNullExceptionAsync(
                Func<int, int, int, int, int, int, int, int, int, Task<int>> arg,
                string expectedParameterNameForException)
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => arg.DefaultIfExceptionAsync(1, 2, 3, 4, 5, 6, 7, 8, 9, -1))
;
                Assert.Equal(expectedParameterNameForException, exception.ParamName);
            }

            /// <summary>
            /// Test to ensure a value is returned, if the execution succeeds.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsValueAsync()
            {
                await ReturnsFromTask(ReturnsValue, -1, 1);
            }

            /// <summary>
            /// Test to ensure a default value is returned, if the execution fails.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsDefaultAsync()
            {
                await ReturnsFromTask(Throws, -1, -1);
            }

            private static async Task ReturnsFromTask(
                Func<int, int, int, int, int, int, int, int, int, Task<int>> func,
                int defaultResult,
                int expected)
            {
                var result = await func.DefaultIfExceptionAsync(1, 2, 3, 4, 5, 6, 7, 8, 9, defaultResult)
                    .ConfigureAwait(false);
                Assert.Equal(expected, result);
            }

            private static Task<int> ReturnsValue(
                int arg1,
                int arg2,
                int arg3,
                int arg4,
                int arg5,
                int arg6,
                int arg7,
                int arg8,
                int arg9) => Task.FromResult(arg1);

            private static Task<int> Throws(
                int arg1,
                int arg2,
                int arg3,
                int arg4,
                int arg5,
                int arg6,
                int arg7,
                int arg8,
                int arg9) => throw new NotImplementedException();

            /// <summary>
            /// Data Source for the Unit Test ThrowsArgumentNullExceptionAsync.
            /// </summary>
            public sealed class ThrowsArgumentNullExceptionAsyncTestData
                : TheoryData<Func<int, int, int, int, int, int, int, int, int, Task<int>>?, string>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="ThrowsArgumentNullExceptionAsyncTestData"/> class.
                /// </summary>
                public ThrowsArgumentNullExceptionAsyncTestData()
                {
                    Add(null, "func");
                }
            }
        }

        /// <summary>
        /// Unit tests for the default if exception handler that executes a func that takes 10 parameters.
        /// </summary>
        public sealed class DefaultIfExceptionAsync10ArgsMethod : ITestAsyncMethodWithNullableParameters<Func<int, int, int, int, int, int, int, int, int, int, Task<int>>>
        {
            /// <inheritdoc/>
            [Theory]
            [ClassData(typeof(ThrowsArgumentNullExceptionAsyncTestData))]
            public async Task ThrowsArgumentNullExceptionAsync(
                Func<int, int, int, int, int, int, int, int, int, int, Task<int>> arg,
                string expectedParameterNameForException)
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => arg.DefaultIfExceptionAsync(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, -1))
;
                Assert.Equal(expectedParameterNameForException, exception.ParamName);
            }

            /// <summary>
            /// Test to ensure a value is returned, if the execution succeeds.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsValueAsync()
            {
                await ReturnsFromTask(ReturnsValue, -1, 1);
            }

            /// <summary>
            /// Test to ensure a default value is returned, if the execution fails.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsDefaultAsync()
            {
                await ReturnsFromTask(Throws, -1, -1);
            }

            private static async Task ReturnsFromTask(
                Func<int, int, int, int, int, int, int, int, int, int, Task<int>> func,
                int defaultResult,
                int expected)
            {
                var result = await func.DefaultIfExceptionAsync(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, defaultResult)
                    .ConfigureAwait(false);
                Assert.Equal(expected, result);
            }

            private static Task<int> ReturnsValue(
                int arg1,
                int arg2,
                int arg3,
                int arg4,
                int arg5,
                int arg6,
                int arg7,
                int arg8,
                int arg9,
                int arg10) => Task.FromResult(arg1);

            private static Task<int> Throws(
                int arg1,
                int arg2,
                int arg3,
                int arg4,
                int arg5,
                int arg6,
                int arg7,
                int arg8,
                int arg9,
                int arg10) => throw new NotImplementedException();

            /// <summary>
            /// Data Source for the Unit Test ThrowsArgumentNullExceptionAsync.
            /// </summary>
            public sealed class ThrowsArgumentNullExceptionAsyncTestData
                : TheoryData<Func<int, int, int, int, int, int, int, int, int, int, Task<int>>?, string>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="ThrowsArgumentNullExceptionAsyncTestData"/> class.
                /// </summary>
                public ThrowsArgumentNullExceptionAsyncTestData()
                {
                    Add(null, "func");
                }
            }
        }

        /// <summary>
        /// Unit tests for the default if exception handler that executes a func that takes 11 parameters.
        /// </summary>
        public sealed class DefaultIfExceptionAsync11ArgsMethod : ITestAsyncMethodWithNullableParameters<Func<int, int, int, int, int, int, int, int, int, int, int, Task<int>>>
        {
            /// <inheritdoc/>
            [Theory]
            [ClassData(typeof(ThrowsArgumentNullExceptionAsyncTestData))]
            public async Task ThrowsArgumentNullExceptionAsync(
                Func<int, int, int, int, int, int, int, int, int, int, int, Task<int>> arg,
                string expectedParameterNameForException)
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => arg.DefaultIfExceptionAsync(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, -1))
;
                Assert.Equal(expectedParameterNameForException, exception.ParamName);
            }

            /// <summary>
            /// Test to ensure a value is returned, if the execution succeeds.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsValueAsync()
            {
                await ReturnsFromTask(ReturnsValue, -1, 1);
            }

            /// <summary>
            /// Test to ensure a default value is returned, if the execution fails.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsDefaultAsync()
            {
                await ReturnsFromTask(Throws, -1, -1);
            }

            private static async Task ReturnsFromTask(
                Func<int, int, int, int, int, int, int, int, int, int, int, Task<int>> func,
                int defaultResult,
                int expected)
            {
                var result = await func.DefaultIfExceptionAsync(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, defaultResult)
                    .ConfigureAwait(false);
                Assert.Equal(expected, result);
            }

            private static Task<int> ReturnsValue(
                int arg1,
                int arg2,
                int arg3,
                int arg4,
                int arg5,
                int arg6,
                int arg7,
                int arg8,
                int arg9,
                int arg10,
                int arg11) => Task.FromResult(arg1);

            private static Task<int> Throws(
                int arg1,
                int arg2,
                int arg3,
                int arg4,
                int arg5,
                int arg6,
                int arg7,
                int arg8,
                int arg9,
                int arg10,
                int arg11) => throw new NotImplementedException();

            /// <summary>
            /// Data Source for the Unit Test ThrowsArgumentNullExceptionAsync.
            /// </summary>
            public sealed class ThrowsArgumentNullExceptionAsyncTestData
                : TheoryData<Func<int, int, int, int, int, int, int, int, int, int, int, Task<int>>?, string>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="ThrowsArgumentNullExceptionAsyncTestData"/> class.
                /// </summary>
                public ThrowsArgumentNullExceptionAsyncTestData()
                {
                    Add(null, "func");
                }
            }
        }

        /// <summary>
        /// Unit tests for the default if exception handler that executes a func that takes 12 parameters.
        /// </summary>
        public sealed class DefaultIfExceptionAsync12ArgsMethod : ITestAsyncMethodWithNullableParameters<Func<int, int, int, int, int, int, int, int, int, int, int, int, Task<int>>>
        {
            /// <inheritdoc/>
            [Theory]
            [ClassData(typeof(ThrowsArgumentNullExceptionAsyncTestData))]
            public async Task ThrowsArgumentNullExceptionAsync(
                Func<int, int, int, int, int, int, int, int, int, int, int, int, Task<int>> arg,
                string expectedParameterNameForException)
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => arg.DefaultIfExceptionAsync(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, -1))
;
                Assert.Equal(expectedParameterNameForException, exception.ParamName);
            }

            /// <summary>
            /// Test to ensure a value is returned, if the execution succeeds.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsValueAsync()
            {
                await ReturnsFromTask(ReturnsValue, -1, 1);
            }

            /// <summary>
            /// Test to ensure a default value is returned, if the execution fails.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsDefaultAsync()
            {
                await ReturnsFromTask(Throws, -1, -1);
            }

            private static async Task ReturnsFromTask(
                Func<int, int, int, int, int, int, int, int, int, int, int, int, Task<int>> func,
                int defaultResult,
                int expected)
            {
                var result = await func.DefaultIfExceptionAsync(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, defaultResult)
                    .ConfigureAwait(false);
                Assert.Equal(expected, result);
            }

            private static Task<int> ReturnsValue(
                int arg1,
                int arg2,
                int arg3,
                int arg4,
                int arg5,
                int arg6,
                int arg7,
                int arg8,
                int arg9,
                int arg10,
                int arg11,
                int arg12) => Task.FromResult(arg1);

            private static Task<int> Throws(
                int arg1,
                int arg2,
                int arg3,
                int arg4,
                int arg5,
                int arg6,
                int arg7,
                int arg8,
                int arg9,
                int arg10,
                int arg11,
                int arg12) => throw new NotImplementedException();

            /// <summary>
            /// Data Source for the Unit Test ThrowsArgumentNullExceptionAsync.
            /// </summary>
            public sealed class ThrowsArgumentNullExceptionAsyncTestData
                : TheoryData<Func<int, int, int, int, int, int, int, int, int, int, int, int, Task<int>>?, string>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="ThrowsArgumentNullExceptionAsyncTestData"/> class.
                /// </summary>
                public ThrowsArgumentNullExceptionAsyncTestData()
                {
                    Add(null, "func");
                }
            }
        }

        /// <summary>
        /// Unit tests for the default if exception handler that executes a func that takes 13 parameters.
        /// </summary>
        public sealed class DefaultIfExceptionAsync13ArgsMethod : ITestAsyncMethodWithNullableParameters<Func<int, int, int, int, int, int, int, int, int, int, int, int, int, Task<int>>>
        {
            /// <inheritdoc/>
            [Theory]
            [ClassData(typeof(ThrowsArgumentNullExceptionAsyncTestData))]
            public async Task ThrowsArgumentNullExceptionAsync(
                Func<int, int, int, int, int, int, int, int, int, int, int, int, int, Task<int>> arg,
                string expectedParameterNameForException)
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => arg.DefaultIfExceptionAsync(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, -1))
;
                Assert.Equal(expectedParameterNameForException, exception.ParamName);
            }

            /// <summary>
            /// Test to ensure a value is returned, if the execution succeeds.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsValueAsync()
            {
                await ReturnsFromTask(ReturnsValue, -1, 1);
            }

            /// <summary>
            /// Test to ensure a default value is returned, if the execution fails.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsDefaultAsync()
            {
                await ReturnsFromTask(Throws, -1, -1);
            }

            private static async Task ReturnsFromTask(
                Func<int, int, int, int, int, int, int, int, int, int, int, int, int, Task<int>> func,
                int defaultResult,
                int expected)
            {
                var result = await func.DefaultIfExceptionAsync(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, defaultResult)
                    .ConfigureAwait(false);
                Assert.Equal(expected, result);
            }

            private static Task<int> ReturnsValue(
                int arg1,
                int arg2,
                int arg3,
                int arg4,
                int arg5,
                int arg6,
                int arg7,
                int arg8,
                int arg9,
                int arg10,
                int arg11,
                int arg12,
                int arg13) => Task.FromResult(arg1);

            private static Task<int> Throws(
                int arg1,
                int arg2,
                int arg3,
                int arg4,
                int arg5,
                int arg6,
                int arg7,
                int arg8,
                int arg9,
                int arg10,
                int arg11,
                int arg12,
                int arg13) => throw new NotImplementedException();

            /// <summary>
            /// Data Source for the Unit Test ThrowsArgumentNullExceptionAsync.
            /// </summary>
            public sealed class ThrowsArgumentNullExceptionAsyncTestData
                : TheoryData<Func<int, int, int, int, int, int, int, int, int, int, int, int, int, Task<int>>?, string>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="ThrowsArgumentNullExceptionAsyncTestData"/> class.
                /// </summary>
                public ThrowsArgumentNullExceptionAsyncTestData()
                {
                    Add(null, "func");
                }
            }
        }

        /// <summary>
        /// Unit tests for the default if exception handler that executes a func that takes 14 parameters.
        /// </summary>
        public sealed class DefaultIfExceptionAsync14ArgsMethod : ITestAsyncMethodWithNullableParameters<Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, Task<int>>>
        {
            /// <inheritdoc/>
            [Theory]
            [ClassData(typeof(ThrowsArgumentNullExceptionAsyncTestData))]
            public async Task ThrowsArgumentNullExceptionAsync(
                Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, Task<int>> arg,
                string expectedParameterNameForException)
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => arg.DefaultIfExceptionAsync(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, -1))
;
                Assert.Equal(expectedParameterNameForException, exception.ParamName);
            }

            /// <summary>
            /// Test to ensure a value is returned, if the execution succeeds.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsValueAsync()
            {
                await ReturnsFromTask(ReturnsValue, -1, 1);
            }

            /// <summary>
            /// Test to ensure a default value is returned, if the execution fails.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsDefaultAsync()
            {
                await ReturnsFromTask(Throws, -1, -1);
            }

            private static async Task ReturnsFromTask(
                Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, Task<int>> func,
                int defaultResult,
                int expected)
            {
                var result = await func.DefaultIfExceptionAsync(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, defaultResult)
                    .ConfigureAwait(false);
                Assert.Equal(expected, result);
            }

            private static Task<int> ReturnsValue(
                int arg1,
                int arg2,
                int arg3,
                int arg4,
                int arg5,
                int arg6,
                int arg7,
                int arg8,
                int arg9,
                int arg10,
                int arg11,
                int arg12,
                int arg13,
                int arg14) => Task.FromResult(arg1);

            private static Task<int> Throws(
                int arg1,
                int arg2,
                int arg3,
                int arg4,
                int arg5,
                int arg6,
                int arg7,
                int arg8,
                int arg9,
                int arg10,
                int arg11,
                int arg12,
                int arg13,
                int arg14) => throw new NotImplementedException();

            /// <summary>
            /// Data Source for the Unit Test ThrowsArgumentNullExceptionAsync.
            /// </summary>
            public sealed class ThrowsArgumentNullExceptionAsyncTestData
                : TheoryData<Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, Task<int>>?, string>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="ThrowsArgumentNullExceptionAsyncTestData"/> class.
                /// </summary>
                public ThrowsArgumentNullExceptionAsyncTestData()
                {
                    Add(null, "func");
                }
            }
        }

        /// <summary>
        /// Unit tests for the default if exception handler that executes a func that takes 15 parameters.
        /// </summary>
        public sealed class DefaultIfExceptionAsync15ArgsMethod : ITestAsyncMethodWithNullableParameters<Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Task<int>>>
        {
            /// <inheritdoc/>
            [Theory]
            [ClassData(typeof(ThrowsArgumentNullExceptionAsyncTestData))]
            public async Task ThrowsArgumentNullExceptionAsync(
                Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Task<int>> arg,
                string expectedParameterNameForException)
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => arg.DefaultIfExceptionAsync(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, -1))
;
                Assert.Equal(expectedParameterNameForException, exception.ParamName);
            }

            /// <summary>
            /// Test to ensure a value is returned, if the execution succeeds.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsValueAsync()
            {
                await ReturnsFromTask(ReturnsValue, -1, 1);
            }

            /// <summary>
            /// Test to ensure a default value is returned, if the execution fails.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsDefaultAsync()
            {
                await ReturnsFromTask(Throws, -1, -1);
            }

            private static async Task ReturnsFromTask(
                Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Task<int>> func,
                int defaultResult,
                int expected)
            {
                var result = await func.DefaultIfExceptionAsync(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, defaultResult)
                    .ConfigureAwait(false);
                Assert.Equal(expected, result);
            }

            private static Task<int> ReturnsValue(
                int arg1,
                int arg2,
                int arg3,
                int arg4,
                int arg5,
                int arg6,
                int arg7,
                int arg8,
                int arg9,
                int arg10,
                int arg11,
                int arg12,
                int arg13,
                int arg14,
                int arg15) => Task.FromResult(arg1);

            private static Task<int> Throws(
                int arg1,
                int arg2,
                int arg3,
                int arg4,
                int arg5,
                int arg6,
                int arg7,
                int arg8,
                int arg9,
                int arg10,
                int arg11,
                int arg12,
                int arg13,
                int arg14,
                int arg15) => throw new NotImplementedException();

            /// <summary>
            /// Data Source for the Unit Test ThrowsArgumentNullExceptionAsync.
            /// </summary>
            public sealed class ThrowsArgumentNullExceptionAsyncTestData
                : TheoryData<Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Task<int>>?, string>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="ThrowsArgumentNullExceptionAsyncTestData"/> class.
                /// </summary>
                public ThrowsArgumentNullExceptionAsyncTestData()
                {
                    Add(null, "func");
                }
            }
        }

        /// <summary>
        /// Unit tests for the default if exception handler that executes a func that takes 16 parameters.
        /// </summary>
        public sealed class DefaultIfExceptionAsync16ArgsMethod : ITestAsyncMethodWithNullableParameters<Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Task<int>>>
        {
            /// <inheritdoc/>
            [Theory]
            [ClassData(typeof(ThrowsArgumentNullExceptionAsyncTestData))]
            public async Task ThrowsArgumentNullExceptionAsync(
                Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Task<int>>? arg,
                string expectedParameterNameForException)
            {
                var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => arg!.DefaultIfExceptionAsync(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, -1))
;
                Assert.Equal(expectedParameterNameForException, exception.ParamName);
            }

            /// <summary>
            /// Test to ensure a value is returned, if the execution succeeds.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsValueAsync()
            {
                await ReturnsFromTask(ReturnsValue, -1, 1);
            }

            /// <summary>
            /// Test to ensure a default value is returned, if the execution fails.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task ReturnsDefaultAsync()
            {
                await ReturnsFromTask(Throws, -1, -1);
            }

            private static async Task ReturnsFromTask(
                Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Task<int>> func,
                int defaultResult,
                int expected)
            {
                var result = await func.DefaultIfExceptionAsync(1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, defaultResult)
                    .ConfigureAwait(false);
                Assert.Equal(expected, result);
            }

            private static Task<int> ReturnsValue(
                int arg1,
                int arg2,
                int arg3,
                int arg4,
                int arg5,
                int arg6,
                int arg7,
                int arg8,
                int arg9,
                int arg10,
                int arg11,
                int arg12,
                int arg13,
                int arg14,
                int arg15,
                int arg16) => Task.FromResult(arg1);

            private static Task<int> Throws(
                int arg1,
                int arg2,
                int arg3,
                int arg4,
                int arg5,
                int arg6,
                int arg7,
                int arg8,
                int arg9,
                int arg10,
                int arg11,
                int arg12,
                int arg13,
                int arg14,
                int arg15,
                int arg16) => throw new NotImplementedException();

            /// <summary>
            /// Data Source for the Unit Test ThrowsArgumentNullExceptionAsync.
            /// </summary>
            public sealed class ThrowsArgumentNullExceptionAsyncTestData
                : TheoryData<Func<int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, int, Task<int>>?, string>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="ThrowsArgumentNullExceptionAsyncTestData"/> class.
                /// </summary>
                public ThrowsArgumentNullExceptionAsyncTestData()
                {
                    Add(null, "func");
                }
            }
        }
    }
}
