// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Whipstaff.Mediator.Foundatio.QueueProcessing
{
    /// <summary>
    /// Recovery Strategy for a failure in a Queue Message.
    /// </summary>
    public enum QueueMessageRecoveryStrategy
    {
        /// <summary>
        /// None.
        /// </summary>
        None,

        /// <summary>
        /// Abandon the message.
        /// </summary>
        Abandon,

        /// <summary>
        /// Dead Letter the message.
        /// </summary>
        DeadLetter,

        /// <summary>
        /// Retry the message.
        /// </summary>
        Retry,

        /// <summary>
        /// Mark the message as complete.
        /// </summary>
        Complete,

        /// <summary>
        /// Requeue the message.
        /// </summary>
        Requeue
    }
}
