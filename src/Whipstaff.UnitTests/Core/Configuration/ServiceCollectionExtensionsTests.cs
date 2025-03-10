// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NetTestRegimentation;
using Whipstaff.Core.Configuration;
using Whipstaff.Testing.Configuration;
using Whipstaff.Testing.Logging;
using Xunit;

namespace Whipstaff.UnitTests.Core.Configuration
{
    /// <summary>
    /// Unit tests for <see cref="Whipstaff.Core.Configuration.ServiceCollectionExtensions"/>.
    /// </summary>
    public static class ServiceCollectionExtensionsTests
    {
        /// <summary>
        /// Unit Tests for the <see cref="Whipstaff.Core.Configuration.ServiceCollectionExtensions.AddStrictConfigurationBinding{TOptions}(Microsoft.Extensions.DependencyInjection.IServiceCollection,string,System.Func{TOptions,bool})"/> method.
        /// </summary>
        public sealed class AddStrictConfigurationBindingT1Method
            : TestWithLoggingBase,
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
                var services = new ServiceCollection();

                var builder = new ConfigurationBuilder();
                var configValues = new Dictionary<string, string?>
                {
                    { "FakeOptions:FakeString", "FakeValue" },
                };
                var configuration = builder.AddInMemoryCollection(configValues).Build();

                _ = services.AddSingleton<IConfiguration>(configuration);

                _ = Assert.Throws<ArgumentNullException>(
                    expectedParameterNameForException,
                    () => services.AddStrictConfigurationBinding(arg1, arg2));
            }

            /// <summary>
            /// Test to ensure that the method returns an instance of the options.
            /// </summary>
            [Fact]
            public void ReturnsInstance()
            {
                var services = new ServiceCollection();

                var builder = new ConfigurationBuilder();
                var configValues = new Dictionary<string, string?>
                {
                    { "FakeOptions:FakeString", "FakeValue" },
                };
                var configuration = builder.AddInMemoryCollection(configValues).Build();
                _ = services.AddSingleton<IConfiguration>(configuration);

                int executionCount = 0;
                services.AddStrictConfigurationBinding<FakeOptions>(
                    "FakeOptions",
                    _ =>
                    {
                        executionCount++;
                        return true;
                    });

                var provider = services.BuildServiceProvider();
                var options = provider.GetRequiredService<IOptions<FakeOptions>>();
                var fakeOptionsValue = options.Value;

                Assert.NotNull(fakeOptionsValue);
                Assert.Equal("FakeValue", fakeOptionsValue.FakeString);
                Assert.Equal(1, executionCount);
            }
        }

        /// <summary>
        /// Unit Tests for the <see cref="Whipstaff.Core.Configuration.ServiceCollectionExtensions.AddStrictConfigurationBinding{TOptions,TOptionsValidator}(Microsoft.Extensions.DependencyInjection.IServiceCollection,string)"/> method.
        /// </summary>
        public sealed class AddStrictConfigurationBindingT2Method
            : TestWithLoggingBase,
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

            /// <summary>
            /// Test to ensure that the method returns an instance of the options.
            /// </summary>
            [Fact]
            public void ReturnsInstance()
            {
                var services = new ServiceCollection();

                var builder = new ConfigurationBuilder();
                var configValues = new Dictionary<string, string?>
                {
                    { "FakeOptions:FakeString", "FakeValue" },
                };
                var configuration = builder.AddInMemoryCollection(configValues).Build();
                _ = services.AddSingleton<IConfiguration>(configuration);

                services.AddStrictConfigurationBinding<FakeOptions, FakeOptionsValidator>("FakeOptions");

                var provider = services.BuildServiceProvider();
                var options = provider.GetRequiredService<IOptions<FakeOptions>>();
                var fakeOptionsValue = options.Value;

                Assert.NotNull(fakeOptionsValue);
                Assert.Equal("FakeValue", fakeOptionsValue.FakeString);
            }

            /// <summary>
            /// Test to ensure that the method returns an instance of the options.
            /// </summary>
            [Fact]
            public void ThrowsOptionsValidationException()
            {
                var services = new ServiceCollection();

                var builder = new ConfigurationBuilder();
                var configValues = new Dictionary<string, string?>
                {
                    { "FakeOptions:FakeString", "1" },
                };
                var configuration = builder.AddInMemoryCollection(configValues).Build();
                _ = services.AddSingleton<IConfiguration>(configuration);

                services.AddStrictConfigurationBinding<FakeOptions, FakeOptionsValidator>("FakeOptions");

                var provider = services.BuildServiceProvider();
                var options = provider.GetRequiredService<IOptions<FakeOptions>>();
                _ = Assert.Throws<OptionsValidationException>(() => options.Value);
            }
        }
    }
}
