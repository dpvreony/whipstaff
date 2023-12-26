// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.Configuration;
using NetTestRegimentation;
using Whipstaff.Core.Configuration;
using Whipstaff.Testing.Configuration;
using Xunit;
using Xunit.Abstractions;

namespace Whipstaff.UnitTests.Core.Configuration
{
    /// <summary>
    /// Unit tests for <see cref="ServiceCollectionExtensions"/>.
    /// </summary>
    public static class ServiceCollectionExtensionsTests
    {
        /// <summary>
        /// Unit Tests for the <see cref="ServiceCollectionExtensions.AddStrictConfigurationBinding{TOptions}(Microsoft.Extensions.DependencyInjection.IServiceCollection,string,System.Func{TOptions,bool})"/> method.
        /// </summary>
        public sealed class AddStrictConfigurationBindingT1Method
            : Foundatio.Xunit.TestWithLoggingBase,
                ITestMethodWithNullableParameters<string, Func<FakeOptions, bool>>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="AddStrictConfigurationBindingT1Method"/> class.
            /// </summary>
            /// <param name="output">XUnit test output helper instance.</param>
            public AddStrictConfigurationBindingT1Method(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <inheritdoc/>
            [ClassData(typeof(Whipstaff.UnitTests.TestSources.Core.Configuration.ServiceCollectionExtensionsTests.AddStrictConfigurationBindingT1Method.ThrowsArgumentNullExceptionTestSource))]
            [Theory]
            public void ThrowsArgumentNullException(
                string arg1,
                Func<FakeOptions, bool> arg2,
                string expectedParameterNameForException)
            {
                var services = new Microsoft.Extensions.DependencyInjection.ServiceCollection();

                _ = Assert.Throws<ArgumentNullException>(
                    expectedParameterNameForException,
                    () => services.AddStrictConfigurationBinding(arg1, arg2));
            }
        }

        /// <summary>
        /// Unit Tests for the <see cref="ServiceCollectionExtensions.AddStrictConfigurationBinding{TOptions,TOptionsValidator}(Microsoft.Extensions.DependencyInjection.IServiceCollection,string)"/> method.
        /// </summary>
        public sealed class AddStrictConfigurationBindingT2Method
            : Foundatio.Xunit.TestWithLoggingBase,
                ITestMethodWithNullableParameters<string>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="AddStrictConfigurationBindingT2Method"/> class.
            /// </summary>
            /// <param name="output">XUnit test output helper instance.</param>
            public AddStrictConfigurationBindingT2Method(ITestOutputHelper output)
                : base(output)
            {
            }

            /// <inheritdoc/>
            [ClassData(typeof(Whipstaff.UnitTests.TestSources.Core.Configuration.ServiceCollectionExtensionsTests.AddStrictConfigurationBindingT2Method.ThrowsArgumentNullExceptionTestSource))]
            [Theory]
            public void ThrowsArgumentNullException(string arg, string expectedParameterNameForException)
            {
                var services = new Microsoft.Extensions.DependencyInjection.ServiceCollection();

                _ = Assert.Throws<ArgumentNullException>(
                    expectedParameterNameForException,
                    () => services.AddStrictConfigurationBinding<FakeOptions, FakeOptionsValidator>(arg));
            }
        }
    }
}
