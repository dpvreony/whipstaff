// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.IO.Abstractions;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Whipstaff.CommandLine.Hosting;
using Whipstaff.DocFxGen.DotNetTool.CommandLine;

namespace Whipstaff.DocFxGen.DotNetTool
{
    /// <summary>
    /// Hosts the program entry point.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Program entry point.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        /// <returns>0 for success, non 0 for failure.</returns>
        public static Task<int> Main(string[] args)
        {
            return HostRunner.RunJobWithFullDependencyInjectionAsync<
                CommandLineJob,
                CommandLineArgModel,
                CommandLineArgModelBinder,
                CommandLineHandlerFactory>(
                args,
                new FileSystem(),
                static serviceCollection => OnConfigureServiceCollection(serviceCollection));
        }

        private static void OnConfigureServiceCollection(IServiceCollection serviceCollection)
        {
            _ = serviceCollection.AddSingleton<CommandLineJobLogMessageActionsWrapper>(static serviceProvider =>
            {
                var logger = serviceProvider.GetRequiredService<Microsoft.Extensions.Logging.ILogger<CommandLineJob>>();
                var commandLineJobLogMessageActions =
                    serviceProvider.GetRequiredService<CommandLineJobLogMessageActions>();
                return new CommandLineJobLogMessageActionsWrapper(
                    commandLineJobLogMessageActions,
                    logger);
            });
        }
    }
}
