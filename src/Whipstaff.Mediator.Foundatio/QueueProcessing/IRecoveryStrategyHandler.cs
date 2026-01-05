// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;

namespace Whipstaff.Mediator.Foundatio.QueueProcessing
{
    /// <summary>
    /// Handler for determining recovery strategy based on exception type.
    /// </summary>
    public interface IRecoveryStrategyHandler
    {
        /// <summary>
        /// Gets the recovery strategy for the given exception.
        /// </summary>
        /// <param name="exception">The exception to check.</param>
        /// <returns>The recovery strategy for the exception.</returns>
        Task<QueueMessageRecoveryStrategy> GetRecoveryStrategyAsync(Exception exception);
    }
}
