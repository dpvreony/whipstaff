using System;
using System.Collections.Generic;
using System.Text;
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
