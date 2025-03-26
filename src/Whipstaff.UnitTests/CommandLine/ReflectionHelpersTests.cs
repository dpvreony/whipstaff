// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reflection;
using NetTestRegimentation;
using Whipstaff.CommandLine;
using Xunit;

namespace Whipstaff.UnitTests.CommandLine
{
    /// <summary>
    /// Unit tests for the <see cref="ReflectionHelpers"/> class.
    /// </summary>
    public static class ReflectionHelpersTests
    {
        /// <summary>
        /// Unit tests for the <see cref="ReflectionHelpers.GetRootCommand(Assembly)"/> method.
        /// </summary>
        public sealed class GetRootCommandMethod : ITestMethodWithNullableParameters<Assembly>
        {
            /// <summary>
            /// Tests that the method returns the root command when the assembly contains one.
            /// </summary>
            /// <param name="arg">Assembly to check.</param>
            [Theory]
            [ClassData(typeof(Whipstaff.UnitTests.TestSources.CommandLine.ReflectionHelpersTests.GetRootCommandMethod.ReturnsRootCommandTestSource))]
            public void ReturnsRootCommand(Assembly arg)
            {
                var rootCommand = ReflectionHelpers.GetRootCommand(arg);
                Assert.NotNull(rootCommand);
            }

            /// <summary>
            /// Tests that the method returns null when the assembly does not contain a root command.
            /// </summary>
            /// <param name="arg">Assembly to check.</param>
            [Theory]
            [ClassData(typeof(Whipstaff.UnitTests.TestSources.CommandLine.ReflectionHelpersTests.GetRootCommandMethod.ReturnsNullTestSource))]
            public void ReturnsNull(Assembly arg)
            {
                var rootCommand = ReflectionHelpers.GetRootCommand(arg);
                Assert.Null(rootCommand);
            }

            /// <inheritdoc/>
            [Theory]
            [ClassData(typeof(Whipstaff.UnitTests.TestSources.CommandLine.ReflectionHelpersTests.GetRootCommandMethod.ThrowsArgumentNullExceptionTestSource))]
            public void ThrowsArgumentNullException(Assembly? arg, string expectedParameterNameForException)
            {
                _ = Assert.Throws<ArgumentNullException>(
                    expectedParameterNameForException,
                    () => ReflectionHelpers.GetRootCommand(arg!));
            }
        }

        /// <summary>
        /// Unit tests for the <see cref="ReflectionHelpers.IsRootCommandAndBinderType(Type)"/> method.
        /// </summary>
        public sealed class IsRootCommandAndBinderTypeMethod : ITestMethodWithNullableParameters<Type>
        {
            /// <summary>
            /// Tests that the method returns true when the type is a root command and binder type.
            /// </summary>
            /// <param name="type">The type to check.</param>
            [Theory]
            [ClassData(typeof(Whipstaff.UnitTests.TestSources.CommandLine.ReflectionHelpersTests.IsRootCommandAndBinderTypeMethod.ReturnsTrueTestSource))]
            public void ReturnsTrue(Type type)
            {
                Assert.True(ReflectionHelpers.IsRootCommandAndBinderType(type));
            }

            /// <inheritdoc/>
            [Theory]
            [ClassData(typeof(Whipstaff.UnitTests.TestSources.CommandLine.ReflectionHelpersTests.IsRootCommandAndBinderTypeMethod.ThrowsArgumentNullExceptionTestSource))]
            public void ThrowsArgumentNullException(Type? arg, string expectedParameterNameForException)
            {
                _ = Assert.Throws<ArgumentNullException>(
                    expectedParameterNameForException,
                    () => ReflectionHelpers.IsRootCommandAndBinderType(arg!));
            }
        }
    }
}
