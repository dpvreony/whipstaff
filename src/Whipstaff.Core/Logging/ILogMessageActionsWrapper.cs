// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.Extensions.Logging;

namespace Whipstaff.Core.Logging
{
    /// <summary>
    /// Represents a wrapper for log message actions and logging framework instance so they can be tightly coupled and passed in as a single argument to a consuming constructor.
    /// </summary>
    /// <typeparam name="TCategoryName">The type whose name is used for the logger category name.</typeparam>
    public interface ILogMessageActionsWrapper<out TCategoryName>
    {
        /// <summary>
        /// Gets the logging framework instance.
        /// </summary>
        ILogger<TCategoryName> Logger { get; }
    }
}
