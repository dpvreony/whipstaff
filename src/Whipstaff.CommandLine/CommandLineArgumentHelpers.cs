// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.CommandLine;
using System.CommandLine.Binding;
using System.Threading.Tasks;

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
        /// <param name="args">Command line arguments to parse.</param>
        /// <param name="rootCommandAndBinderModelFunc">Function to execute for handling the binding of the command line model.</param>
        /// <param name="rootCommandHandlerFunc">Function to execute for handling the invocation of the root command.</param>
        /// <returns>0 for success, non 0 for failure.</returns>
        public static async Task<int> GetResultFromRootCommand<TCommandLineArg, TCommandLineArgModelBinder>(
            string[] args,
            Func<RootCommandAndBinderModel<TCommandLineArgModelBinder>> rootCommandAndBinderModelFunc,
            Func<TCommandLineArg, Task<int>> rootCommandHandlerFunc)
            where TCommandLineArgModelBinder : BinderBase<TCommandLineArg>
        {
            var rootCommandAndBinderModel = rootCommandAndBinderModelFunc();

            var rootCommand = rootCommandAndBinderModel.RootCommand;
            var binder = rootCommandAndBinderModel.CommandLineBinder;

#pragma warning disable RCS1207 // Convert anonymous function to method group (or vice versa).
            rootCommand.SetHandler(
                commandLineArgModel => rootCommandHandlerFunc(commandLineArgModel),
                binder);
#pragma warning restore RCS1207 // Convert anonymous function to method group (or vice versa).

            return await rootCommand.InvokeAsync(args)
                .ConfigureAwait(false);
        }
    }
}
