﻿// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.CommandLine;
using System.CommandLine.Binding;
using System.IO.Abstractions;
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
        /// <param name="console">The console to which output is written during invocation.</param>
        /// <returns>0 for success, non 0 for failure.</returns>
        public static Task<int> GetResultFromRootCommand<TCommandLineArg, TCommandLineArgModelBinder, TRootCommandAndBinderFactory>(
            string[] args,
            Func<TCommandLineArg, Task<int>> rootCommandHandlerFunc,
            IFileSystem fileSystem,
            IConsole? console = null)
            where TCommandLineArgModelBinder : BinderBase<TCommandLineArg>
            where TRootCommandAndBinderFactory : IRootCommandAndBinderFactory<TCommandLineArgModelBinder>, new()
        {
            ArgumentNullException.ThrowIfNull(args);
            ArgumentNullException.ThrowIfNull(rootCommandHandlerFunc);
            ArgumentNullException.ThrowIfNull(fileSystem);

            return GetResultFromRootCommand(
                args,
                new TRootCommandAndBinderFactory().GetRootCommandAndBinder,
                rootCommandHandlerFunc,
                fileSystem,
                console);
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
        /// <param name="console">The console to which output is written during invocation.</param>
        /// <returns>0 for success, non 0 for failure.</returns>
        public static async Task<int> GetResultFromRootCommand<TCommandLineArg, TCommandLineArgModelBinder>(
            string[] args,
            Func<IFileSystem, RootCommandAndBinderModel<TCommandLineArgModelBinder>> rootCommandAndBinderModelFunc,
            Func<TCommandLineArg, Task<int>> rootCommandHandlerFunc,
            IFileSystem fileSystem,
            IConsole? console = null)
            where TCommandLineArgModelBinder : BinderBase<TCommandLineArg>
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
            rootCommand.SetHandler(
                commandLineArgModel => rootCommandHandlerFunc(commandLineArgModel),
                binder);
#pragma warning restore RCS1207 // Convert anonymous function to method group (or vice versa).

            return await rootCommand.InvokeAsync(args, console)
                .ConfigureAwait(false);
        }
    }
}
