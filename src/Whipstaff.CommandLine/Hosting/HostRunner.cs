// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.CommandLine;
using System.CommandLine.Binding;
using System.IO.Abstractions;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

#if ARGUMENT_NULL_EXCEPTION_SHIM
using ArgumentNullException = Whipstaff.Runtime.Exceptions.ArgumentNullException;
#endif

namespace Whipstaff.CommandLine.Hosting
{
    /// <summary>
    /// Wrapper for hosting a command line job.
    /// </summary>
    public static class HostRunner
    {
        /// <summary>
        /// Runs a command line job that requires additional configuration or injection.
        /// </summary>
        /// <typeparam name="TCommandLineHandler">The type of the command line handler.</typeparam>
        /// <typeparam name="TCommandLineArgModel">The type of the command line argument model.</typeparam>
        /// <typeparam name="TCommandLineArgModelBinder">The type of the command line argument model binder.</typeparam>
        /// <typeparam name="TRootCommandAndBinderFactory">The type of the RootCommand and Argument Binder factory.</typeparam>
        /// <param name="args">Command line arguments to parse.</param>
        /// <param name="fileSystem">File system wrapper.</param>
        /// <param name="additionalServiceRegistrationsAction">Action to carry out additional service registrations, if any.</param>
        /// <param name="parserConfigurationFunc">Function for passing in a parser configuration to override the default behaviour of the command line parser.</param>
        /// <param name="invocationConfigurationFunc">Function for passing in a configuration to override the default invocation behaviour of the command line runner. Useful for testing and redirecting the console.</param>
        /// <returns>0 for success, non 0 for failure.</returns>
        public static async Task<int> RunJobWithFullDependencyInjectionAsync<TCommandLineHandler, TCommandLineArgModel, TCommandLineArgModelBinder, TRootCommandAndBinderFactory>(
            string[] args,
            IFileSystem fileSystem,
            Action<IServiceCollection>? additionalServiceRegistrationsAction,
            Func<ParserConfiguration>? parserConfigurationFunc = null,
            Func<InvocationConfiguration>? invocationConfigurationFunc = null)
            where TCommandLineHandler : class, ICommandLineHandler<TCommandLineArgModel>
            where TCommandLineArgModelBinder : IBinderBase<TCommandLineArgModel>
            where TRootCommandAndBinderFactory : IRootCommandAndBinderFactory<TCommandLineArgModelBinder>, new()
        {
            ArgumentNullException.ThrowIfNull(args);
            ArgumentNullException.ThrowIfNull(fileSystem);

            try
            {
                var serviceCollection = new ServiceCollection()
                    .AddLogging(loggingBuilder => loggingBuilder
                        .SetMinimumLevel(LogLevel.Information)
                        .AddConsole());

                _ = serviceCollection.AddSingleton(fileSystem);
                _ = serviceCollection.AddSingleton<TCommandLineHandler>();
                additionalServiceRegistrationsAction?.Invoke(serviceCollection);

                var serviceProvider = serviceCollection
                    .BuildServiceProvider();

                var commandLineHandler = serviceProvider.GetRequiredService<TCommandLineHandler>();

                return await CommandLineArgumentHelpers.GetResultFromRootCommandAsync<TCommandLineArgModel, TCommandLineArgModelBinder, TRootCommandAndBinderFactory>(
                        args,
                        (commandLineArgModel, cancellationToken) => commandLineHandler.HandleCommandAsync(commandLineArgModel, cancellationToken),
                        fileSystem,
                        parserConfigurationFunc,
                        invocationConfigurationFunc)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
#pragma warning disable GR0015 // Do not use System.Console.
                await Console.Error.WriteLineAsync(ex.ToString()).ConfigureAwait(false);
#pragma warning restore GR0015 // Do not use System.Console.
                return int.MaxValue;
            }
        }

        /// <summary>
        /// Runs a command line job that requires no additional configuration or injection.
        /// </summary>
        /// <typeparam name="TCommandLineHandler">The type of the command line handler.</typeparam>
        /// <typeparam name="TCommandLineArgModel">The type of the command line argument model.</typeparam>
        /// <typeparam name="TCommandLineArgModelBinder">The type of the command line argument model binder.</typeparam>
        /// <typeparam name="TRootCommandAndBinderFactory">The type of the RootCommand and Argument Binder factory.</typeparam>
        /// <param name="args">Command line arguments to parse.</param>
        /// <param name="commandLineHandlerFactoryFunc">Factory method for the command line handler.</param>
        /// <param name="fileSystem">File system wrapper.</param>
        /// <param name="parserConfigurationFunc">Function for passing in a parser configuration to override the default behaviour of the command line parser.</param>
        /// <param name="invocationConfigurationFunc">Function for passing in a configuration to override the default invocation behaviour of the command line runner. Useful for testing and redirecting the console.</param>
        /// <returns>0 for success, non 0 for failure.</returns>
        public static async Task<int> RunSimpleCliJobAsync<TCommandLineHandler, TCommandLineArgModel, TCommandLineArgModelBinder, TRootCommandAndBinderFactory>(
            string[] args,
            Func<IFileSystem, ILogger<TCommandLineHandler>, TCommandLineHandler> commandLineHandlerFactoryFunc,
            IFileSystem fileSystem,
            Func<ParserConfiguration>? parserConfigurationFunc = null,
            Func<InvocationConfiguration>? invocationConfigurationFunc = null)
            where TCommandLineHandler : ICommandLineHandler<TCommandLineArgModel>
            where TCommandLineArgModelBinder : IBinderBase<TCommandLineArgModel>
            where TRootCommandAndBinderFactory : IRootCommandAndBinderFactory<TCommandLineArgModelBinder>, new()
        {
            ArgumentNullException.ThrowIfNull(args);
            ArgumentNullException.ThrowIfNull(commandLineHandlerFactoryFunc);
            ArgumentNullException.ThrowIfNull(fileSystem);

            try
            {
                var serviceProvider = new ServiceCollection()
                    .AddLogging(loggingBuilder => loggingBuilder
                        .SetMinimumLevel(LogLevel.Information)
                        .AddConsole())
                    .BuildServiceProvider();

                var logger = serviceProvider.GetRequiredService<ILoggerFactory>()
                    .CreateLogger<TCommandLineHandler>();

                var commandLineHandler = commandLineHandlerFactoryFunc(fileSystem, logger);

                return await CommandLineArgumentHelpers.GetResultFromRootCommandAsync<TCommandLineArgModel, TCommandLineArgModelBinder, TRootCommandAndBinderFactory>(
                        args,
                        commandLineHandler.HandleCommandAsync,
                        fileSystem,
                        parserConfigurationFunc,
                        invocationConfigurationFunc)
                    .ConfigureAwait(false);
            }
            catch (Exception ex)
            {
#pragma warning disable GR0015 // Do not use System.Console.
                await Console.Error.WriteLineAsync(ex.ToString()).ConfigureAwait(false);
#pragma warning restore GR0015 // Do not use System.Console.
                return int.MaxValue;
            }
        }
    }
}
