﻿// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Threading.Tasks;

namespace Whipstaff.CommandLine
{
    /// <summary>
    /// Represents a command line handler.
    /// </summary>
    /// <typeparam name="TCommandLineArgModel">Model representing the command line arguments.</typeparam>
    public interface ICommandLineHandler<in TCommandLineArgModel>
    {
        /// <summary>
        /// Handles the execution of the command line job.
        /// </summary>
        /// <param name="commandLineArgModel">Command Line Arguments model.</param>
        /// <returns>0 for success, non-zero for error.</returns>
        Task<int> HandleCommand(TCommandLineArgModel commandLineArgModel);
    }
}
