// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using Whipstaff.Runtime.Extensions;

#if ARGUMENT_NULL_EXCEPTION_SHIM
using ArgumentNullException = Whipstaff.Runtime.Exceptions.ArgumentNullException;
#endif

namespace Whipstaff.Core.Configuration
{
    /// <summary>
    /// Extensions for <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds Strict configuration binding to the DI container with a custom validation function.
        /// </summary>
        /// <typeparam name="TOptions">The options type to be configured.</typeparam>
        /// <param name="services">ServiceCollection to update.</param>
        /// <param name="sectionName">The name of the section in the configuration.</param>
        /// <param name="validationFunc">Validation function to run.</param>
        /// <example>
        /// <code>
        /// services.AddStrictConfigurationBinding&lt;MyOptions&gt;(
        ///     "MySection",
        ///     options => !string.IsNullOrEmpty(options.ConnectionString));
        /// </code>
        /// </example>
        public static void AddStrictConfigurationBinding<TOptions>(
            this IServiceCollection services,
            string sectionName,
            Func<TOptions, bool> validationFunc)
            where TOptions : class
        {
            sectionName.ThrowIfNullOrWhitespace();
            ArgumentNullException.ThrowIfNull(validationFunc);

            _ = services.AddOptions<TOptions>()
                .BindConfiguration(
                    sectionName,
                    binderOptions => binderOptions.ErrorOnUnknownConfiguration = true)
                .Validate(settings => validationFunc(settings))
                .ValidateOnStart();
        }

        /// <summary>
        /// Adds Strict configuration binding to the DI container with a OptionsValidator class.
        /// </summary>
        /// <typeparam name="TOptions">The options type to be configured.</typeparam>
        /// <typeparam name="TOptionsValidator">The type for the <see cref="IValidateOptions{TOptions}"/> used to validate the configuration.</typeparam>
        /// <param name="services">ServiceCollection to update.</param>
        /// <param name="sectionName">The name of the section in the configuration.</param>
        /// <example>
        /// <code>
        /// services.AddStrictConfigurationBinding&lt;MyOptions, MyOptionsValidator&gt;("MySection");
        /// </code>
        /// </example>
        public static void AddStrictConfigurationBinding<TOptions, TOptionsValidator>(
            this IServiceCollection services,
            string sectionName)
            where TOptions : class
            where TOptionsValidator : class, IValidateOptions<TOptions>
        {
            sectionName.ThrowIfNullOrWhitespace();

            _ = services.AddOptions<TOptions>()
                .BindConfiguration(
                    sectionName,
                    binderOptions => binderOptions.ErrorOnUnknownConfiguration = true);

            services.TryAddSingleton<IValidateOptions<TOptions>, TOptionsValidator>();
        }
    }
}
