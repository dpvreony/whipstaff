// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
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
            var noWarnArgs = "\"/p:NoWarn=$(NoWarn),QA0002,GR0027,GR0051\"";
            noWarnArgs = noWarnArgs.Replace(",", "%3B", StringComparison.Ordinal);

            var job = Job.Default
                .WithArguments(new List<Argument> { new MsBuildArgument(noWarnArgs) });

            _ = BenchmarkRunner.Run(
                typeof(Program).Assembly,
                DefaultConfig.Instance.AddJob(job));
        }
    }
}
