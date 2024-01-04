// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.Logging;

namespace Whipstaff.EntityFramework.Diagram.DotNetTool
{
    /// <summary>
    /// Log Message actions wrapper for <see cref="CommandLineJob" />.
    /// </summary>
    public sealed class CommandLineJobLogMessageActionsWrapper
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineJobLogMessageActionsWrapper"/> class.
        /// </summary>
        /// <param name="logger">Logging framework instance.</param>
        /// <param name="commandLineJobLogMessageActions">Log Message actions for <see cref="CommandLineJob" />.</param>
        public CommandLineJobLogMessageActionsWrapper(
            ILogger<CommandLineJob> logger,
            CommandLineJobLogMessageActions commandLineJobLogMessageActions)
        {
            ArgumentNullException.ThrowIfNull(logger);
            ArgumentNullException.ThrowIfNull(commandLineJobLogMessageActions);
        }
    }
}
