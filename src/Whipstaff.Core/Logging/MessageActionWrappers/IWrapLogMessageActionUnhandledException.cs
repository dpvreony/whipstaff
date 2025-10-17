// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace Whipstaff.Core.Logging.MessageActionWrappers
{
    /// <summary>
    /// Interface for wrapping the logging of unhandled exceptions.
    /// </summary>
    public interface IWrapLogMessageActionUnhandledException
    {
        /// <summary>
        /// Log an unhandled exception.
        /// </summary>
        /// <param name="exception">Exception that occurred.</param>
        void UnhandledException(Exception exception);
    }
}
