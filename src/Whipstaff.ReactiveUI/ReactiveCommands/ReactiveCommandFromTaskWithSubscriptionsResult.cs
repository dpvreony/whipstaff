// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using ReactiveUI;

namespace Whipstaff.ReactiveUI.ReactiveCommands
{
    /// <summary>
    /// Result of creating a reactive command from a task, along with subscriptions to the execution result and possibly the thrown exception handler.
    /// </summary>
    /// <typeparam name="TRequest">The type for the command input.</typeparam>
    /// <typeparam name="TResult">The type for the result passed around.</typeparam>
    /// <param name="ReactiveCommand">The ReactiveCommand that was created.</param>
    /// <param name="OnExecutionResultAvailableSubscription">The registration for the subscription that was created for the command execution having a result available.</param>
    /// <param name="OnThrownExceptionSubscription">The registration for the subscription that was created for if the command throws an exception.</param>
    public sealed record ReactiveCommandFromTaskWithSubscriptionsResult<TRequest, TResult>(
        ReactiveCommand<TRequest, TResult> ReactiveCommand,
        IDisposable OnExecutionResultAvailableSubscription,
        IDisposable? OnThrownExceptionSubscription);
}
