// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.IO.Abstractions;
using System.Threading.Tasks;
using Whipstaff.CommandLine.Hosting;
using Whipstaff.CommandLine.MarkdownGen.DotNetTool.CommandLine;

namespace Whipstaff.CommandLine.MarkdownGen.DotNetTool
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
            return HostRunner.RunSimpleCliJobAsync<
                    CommandLineJob,
                    CommandLineArgModel,
                    CommandLineArgModelBinder,
                    CommandLineHandlerFactory>(
                    args,
                    (fileSystem, logger) => new CommandLineJob(
                        fileSystem,
                        new CommandLineJobLogMessageActionsWrapper(
                            new CommandLineJobLogMessageActions(),
                            logger)),
                    new FileSystem());
        }
    }
}
