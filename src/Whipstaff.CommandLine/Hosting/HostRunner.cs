// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.CommandLine.Binding;
using System.IO.Abstractions;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Whipstaff.CommandLine.Hosting
{
    /// <summary>
    /// Wrapper for hosting a command line job.
    /// </summary>
    public static class HostRunner
    {
        /// <summary>
        /// Runs a command line job that requires no additional configuration or injection.
        /// </summary>
        /// <typeparam name="TJob">The type of the job.</typeparam>
        /// <typeparam name="TCommandLineArgModel">The type of the command line argument model.</typeparam>
        /// <typeparam name="TCommandLineArgModelBinder">The type of the command line argument model binder.</typeparam>
        /// <typeparam name="TRootCommandAndBinderFactory">The type of the RootCommand and Argument Binder factory.</typeparam>
        /// <param name="args">Command line arguments to parse.</param>
        /// <param name="jobFactoryFunc">Factory method for the job.</param>
        /// <param name="fileSystem">File system wrapper.</param>
        /// <returns>0 for success, non 0 for failure.</returns>
        public static async Task<int> RunSimpleCliJob<TJob, TCommandLineArgModel, TCommandLineArgModelBinder, TRootCommandAndBinderFactory>(
            string[] args,
            Func<IFileSystem, ILogger<TJob>, TJob> jobFactoryFunc,
            IFileSystem fileSystem)
            where TJob : ICommandLineHandler<TCommandLineArgModel>
            where TCommandLineArgModelBinder : BinderBase<TCommandLineArgModel>
            where TRootCommandAndBinderFactory : IRootCommandAndBinderFactory<TCommandLineArgModelBinder>, new()
        {
            try
            {
                var serviceProvider = new ServiceCollection()
                    .AddLogging(loggingBuilder => loggingBuilder
                        .SetMinimumLevel(LogLevel.Information)
                        .AddConsole())
                    .BuildServiceProvider();

                var logger = serviceProvider.GetRequiredService<ILoggerFactory>()
                    .CreateLogger<TJob>();

                var job = jobFactoryFunc(fileSystem, logger);

                return await CommandLineArgumentHelpers.GetResultFromRootCommand<TCommandLineArgModel, TCommandLineArgModelBinder>(
                        args,
                        new TRootCommandAndBinderFactory().GetRootCommandAndBinder,
                        job.HandleCommand,
                        fileSystem)
                    .ConfigureAwait(false);
            }
#pragma warning disable CA1031
            catch
#pragma warning restore CA1031
            {
                return int.MaxValue;
            }
        }

        /// <summary>
        /// Runs a command line job that requires no additional configuration or injection.
        /// </summary>
        /// <typeparam name="TJob">The type of the job.</typeparam>
        /// <typeparam name="TCommandLineArgModel">The type of the command line argument model.</typeparam>
        /// <typeparam name="TCommandLineArgModelBinder">The type of the command line argument model binder.</typeparam>
        /// <typeparam name="TRootCommandAndBinderFactory">The type of the RootCommand and Argument Binder factory.</typeparam>
        /// <param name="args">Command line arguments to parse.</param>
        /// <param name="jobFactoryFunc">Factory method for the job.</param>
        /// <returns>0 for success, non 0 for failure.</returns>
        public static async Task<int> RunSimpleCliJob<TJob, TCommandLineArgModel, TCommandLineArgModelBinder,
            TRootCommandAndBinderFactory>(
            string[] args,
            Func<IFileSystem, ILogger<TJob>, TJob> jobFactoryFunc)
            where TJob : ICommandLineHandler<TCommandLineArgModel>
            where TCommandLineArgModelBinder : BinderBase<TCommandLineArgModel>
            where TRootCommandAndBinderFactory : IRootCommandAndBinderFactory<TCommandLineArgModelBinder>, new()
        {
            return await RunSimpleCliJob<
                    TJob,
                    TCommandLineArgModel,
                    TCommandLineArgModelBinder,
                    TRootCommandAndBinderFactory>(
                    args,
                    jobFactoryFunc,
                    new FileSystem())
                .ConfigureAwait(false);
        }
    }
}
