// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Whipstaff.CommandLine;

namespace Whipstaff.EntityFramework.Diagram.DotNetTool
{
    /// <summary>
    /// Hosts the program entry point.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Program entry point.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        /// <returns>0 for success, non 0 for failure.</returns>
        public static async Task<int> Main(string[] args)
        {
            try
            {
                var serviceProvider = new ServiceCollection()
                    .AddLogging((loggingBuilder) => loggingBuilder
                        .SetMinimumLevel(LogLevel.Information)
                        .AddConsole())
                    .BuildServiceProvider();

                var logger = serviceProvider.GetRequiredService<ILoggerFactory>().CreateLogger<Job>();

                var job = new Job(new JobLogMessageActionsWrapper(logger, new JobLogMessageActions()));

                return await CommandLineArgumentHelpers.GetResultFromRootCommand<CommandLineArgModel, CommandLineArgModelBinder>(
                        args,
                        GetRootCommandAndBinder,
                        job.HandleCommand)
                    .ConfigureAwait(false);
            }
#pragma warning disable CA1031
            catch
#pragma warning restore CA1031
            {
                return int.MaxValue;
            }
        }
    }
}
