// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;

namespace Whipstaff.Benchmarks
{
    /// <summary>
    /// Represents the application entry.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// Program entry point.
        /// </summary>
        public static void Main()
        {
            // This is a workaround for https://github.com/dotnet/BenchmarkDotNet/issues/2719
            // where you can't pass semicolons in the NoWarn property via MSBuildArgument.
            var noWarnArgs = "\"/p:NoWarn=$(NoWarn),CA1515,GR0027,GR0051,QA0002\"";
            noWarnArgs = noWarnArgs.Replace(",", "%3B", StringComparison.Ordinal);

            // Get the solution directory relative to the assembly location
            var assemblyLocation = Assembly.GetExecutingAssembly().Location;
#pragma warning disable IO0006 // Replace Path class with IFileSystem.Path for improved testability
            var assemblyDirectory = Path.GetDirectoryName(assemblyLocation)!;
            var solutionDirectory = Path.GetFullPath(Path.Combine(assemblyDirectory, "..", "..", "..", ".."));
#pragma warning restore IO0006 // Replace Path class with IFileSystem.Path for improved testability

            var job = Job.Default
                .WithArguments(new List<Argument>
                {
                    new MsBuildArgument(noWarnArgs),
                    new MsBuildArgument("/p:SolutionName=Whipstaff"),
                    new MsBuildArgument($"/p:SolutionDir={solutionDirectory}\\")
                });

            _ = BenchmarkRunner.Run(
                typeof(Program).Assembly,
                DefaultConfig.Instance.AddJob(job));
        }
    }
}
