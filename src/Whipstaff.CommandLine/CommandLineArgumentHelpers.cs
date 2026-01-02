// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.CommandLine;
using System.CommandLine.Binding;
using System.IO.Abstractions;
using System.Threading;
using System.Threading.Tasks;

#if ARGUMENT_NULL_EXCEPTION_SHIM
using ArgumentNullException = Whipstaff.Runtime.Exceptions.ArgumentNullException;
#endif

namespace Whipstaff.CommandLine
{
    /// <summary>
    /// Helpers for working with command line arguments.
    /// </summary>
    public static class CommandLineArgumentHelpers
    {
        /// <summary>
        /// Gets the result from the invoking root command.
        /// </summary>
        /// <typeparam name="TCommandLineArg">The type for the command line argument model.</typeparam>
        /// <typeparam name="TCommandLineArgModelBinder">The type for the command line argument model binder.</typeparam>
        /// <typeparam name="TRootCommandAndBinderFactory">The type of the RootCommand and Argument Binder factory.</typeparam>
        /// <param name="args">Command line arguments to parse.</param>
        /// <param name="rootCommandHandlerFunc">Function to execute for handling the invocation of the root command.</param>
        /// <param name="fileSystem">File System abstraction.</param>
        /// <param name="parserConfigurationFunc">Function for passing in a parser configuration to override the default behaviour of the command line parser.</param>
        /// <param name="invocationConfigurationFunc">Function for passing in a configuration to override the default invocation behaviour of the command line runner. Useful for testing and redirecting the console.</param>
        /// <param name="cancellationToken">The cancellation token for the operation.</param>
        /// <returns>0 for success, non 0 for failure.</returns>
        public static Task<int> GetResultFromRootCommandAsync<TCommandLineArg, TCommandLineArgModelBinder, TRootCommandAndBinderFactory>(
            string[] args,
            Func<TCommandLineArg, CancellationToken, Task<int>> rootCommandHandlerFunc,
            IFileSystem fileSystem,
            Func<ParserConfiguration>? parserConfigurationFunc = null,
            Func<InvocationConfiguration>? invocationConfigurationFunc = null,
            System.Threading.CancellationToken cancellationToken = default)
            where TCommandLineArgModelBinder : IBinderBase<TCommandLineArg>
            where TRootCommandAndBinderFactory : IRootCommandAndBinderFactory<TCommandLineArgModelBinder>, new()
        {
            ArgumentNullException.ThrowIfNull(args);
            ArgumentNullException.ThrowIfNull(rootCommandHandlerFunc);
            ArgumentNullException.ThrowIfNull(fileSystem);

            return GetResultFromRootCommandAsync(
                args,
                new TRootCommandAndBinderFactory().GetRootCommandAndBinder,
                rootCommandHandlerFunc,
                fileSystem,
                parserConfigurationFunc,
                invocationConfigurationFunc,
                cancellationToken);
        }

        /// <summary>
        /// Gets the result from the invoking root command.
        /// </summary>
        /// <typeparam name="TCommandLineArg">The type for the command line argument model.</typeparam>
        /// <typeparam name="TCommandLineArgModelBinder">The type for the command line argument model binder.</typeparam>
        /// <param name="args">Command line arguments to parse.</param>
        /// <param name="rootCommandAndBinderModelFunc">Function to execute for handling the binding of the command line model.</param>
        /// <param name="rootCommandHandlerFunc">Function to execute for handling the invocation of the root command.</param>
        /// <param name="fileSystem">File System abstraction.</param>
        /// <param name="parserConfigurationFunc">Function for passing in a parser configuration to override the default behaviour of the command line parser.</param>
        /// <param name="invocationConfigurationFunc">Function for passing in a configuration to override the default invocation behaviour of the command line runner. Useful for testing and redirecting the console.</param>
        /// <param name="cancellationToken">The cancellation token for the operation.</param>
        /// <returns>0 for success, non 0 for failure.</returns>
        public static async Task<int> GetResultFromRootCommandAsync<TCommandLineArg, TCommandLineArgModelBinder>(
            string[] args,
            Func<IFileSystem, RootCommandAndBinderModel<TCommandLineArgModelBinder>> rootCommandAndBinderModelFunc,
            Func<TCommandLineArg, CancellationToken, Task<int>> rootCommandHandlerFunc,
            IFileSystem fileSystem,
            Func<ParserConfiguration>? parserConfigurationFunc = null,
            Func<InvocationConfiguration>? invocationConfigurationFunc = null,
            System.Threading.CancellationToken cancellationToken = default)
            where TCommandLineArgModelBinder : IBinderBase<TCommandLineArg>
        {
            ArgumentNullException.ThrowIfNull(args);
            ArgumentNullException.ThrowIfNull(rootCommandAndBinderModelFunc);
            ArgumentNullException.ThrowIfNull(rootCommandHandlerFunc);
            ArgumentNullException.ThrowIfNull(fileSystem);

            var rootCommandAndBinderModel = rootCommandAndBinderModelFunc(fileSystem);

            var rootCommand = rootCommandAndBinderModel.RootCommand;

            var binder = rootCommandAndBinderModel.CommandLineBinder;

#pragma warning disable RCS1207 // Convert anonymous function to method group (or vice versa).
            // ReSharper disable once ConvertClosureToMethodGroup
            rootCommand.SetAction(
                (parseResult, cxt) =>
                {
                    var commandLineArgModel = binder.GetBoundValue(parseResult);
                    return rootCommandHandlerFunc(
                        commandLineArgModel,
                        cxt);
                });
#pragma warning restore RCS1207 // Convert anonymous function to method group (or vice versa).

            var parseConfiguration = parserConfigurationFunc?.Invoke();
            var parseResult = rootCommand.Parse(
                args,
                parseConfiguration);

            var invocationConfiguration = invocationConfigurationFunc?.Invoke();

            return await parseResult.InvokeAsync(
                    invocationConfiguration,
                    cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
