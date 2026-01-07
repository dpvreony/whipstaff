// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using ReactiveUI;

#pragma warning disable CA2000 // Dispose objects before losing scope
#pragma warning disable VSTHRD101 // Avoid unsupported async delegates - need new version of System.Reactive to fix
namespace Whipstaff.ReactiveUI.ReactiveCommands
{
    /// <summary>
    /// Factory for creating reactive commands with subscriptions.
    /// </summary>
    public static class ReactiveCommandFactory
    {
        /// <summary>
        /// Creates a reactive command from a task with a complete and exception action.
        /// Checks if the command can be executed via control observable.
        /// </summary>
        /// <typeparam name="TResult">The type for the result passed around.</typeparam>
        /// <param name="commandFunc">The command function to execute.</param>
        /// <param name="canExecute">Observable indicating whether the command can execute.</param>
        /// <param name="onExecutionResultAvailable">Action to carry out when command completes. Used to update the UI.</param>
        /// <param name="onExceptionAction">Action to carry out on an exception. Used to notify the user of an error etc.</param>
        /// <param name="scheduler">Schedule to use for carrying out the command. Typically used for time travel in unit tests.</param>
        /// <returns>Reactive command, along with subscriptions to the execution result and possibly the thrown exception handler, if one was passed.</returns>
        public static ReactiveCommandFromTaskWithSubscriptionsResult<Unit, TResult> CreateFromTaskWithSubscriptions<TResult>(
            Func<Task<TResult>> commandFunc,
            IObservable<bool> canExecute,
            Action<TResult> onExecutionResultAvailable,
            Action<Exception>? onExceptionAction,
            IScheduler scheduler)
        {
            var command = ReactiveCommand.CreateFromTask(
                commandFunc,
                canExecute,
                scheduler);
            var onExecutionResultAvailableSubscription = command.Subscribe(onExecutionResultAvailable);
            var onThrownExceptionSubscription = onExceptionAction != null
                ? command.ThrownExceptions.Subscribe(onExceptionAction)
                : null;

            return new(
                command,
                onExecutionResultAvailableSubscription,
                onThrownExceptionSubscription);
        }

        /// <summary>
        /// Creates a reactive command from a task with a complete and exception action.
        /// Assumes the command can always execute.
        /// </summary>
        /// <typeparam name="TResult">The type for the result passed around.</typeparam>
        /// <param name="commandFunc">The command function to execute.</param>
        /// <param name="onExecutionResultAvailable">Action to carry out when command completes. Used to update the UI.</param>
        /// <param name="onExceptionAction">Action to carry out on an exception. Used to notify the user of an error etc.</param>
        /// <param name="scheduler">Schedule to use for carrying out the command. Typically used for time travel in unit tests.</param>
        /// <returns>Reactive command, along with subscriptions to the execution result and possibly the thrown exception handler, if one was passed.</returns>
        public static ReactiveCommandFromTaskWithSubscriptionsResult<Unit, TResult> CreateFromTaskWithSubscriptions<TResult>(
            Func<Task<TResult>> commandFunc,
            Action<TResult> onExecutionResultAvailable,
            Action<Exception>? onExceptionAction,
            IScheduler scheduler)
        {
            var command = ReactiveCommand.CreateFromTask(
                commandFunc,
                outputScheduler: scheduler);
            var onExecutionResultAvailableSubscription = command.Subscribe(onExecutionResultAvailable);
            var onThrownExceptionSubscription = onExceptionAction != null
                ? command.ThrownExceptions.Subscribe(onExceptionAction)
                : null;

            return new(
                command,
                onExecutionResultAvailableSubscription,
                onThrownExceptionSubscription);
        }

        /// <summary>
        /// Creates a reactive command from a task with a complete and exception action.
        /// Assumes the command can always execute.
        /// </summary>
        /// <typeparam name="TResult">The type for the result passed around.</typeparam>
        /// <param name="commandFunc">The command function to execute.</param>
        /// <param name="canExecute">Observable indicating whether the command can execute.</param>
        /// <param name="onExecutionResultAvailable">Action to carry out when command completes. Used to update the UI.</param>
        /// <param name="onExceptionAction">Action to carry out on an exception. Used to notify the user of an error etc.</param>
        /// <param name="scheduler">Schedule to use for carrying out the command. Typically used for time travel in unit tests.</param>
        /// <returns>Reactive command, along with subscriptions to the execution result and possibly the thrown exception handler, if one was passed.</returns>
        public static ReactiveCommandFromTaskWithSubscriptionsResult<Unit, TResult> CreateFromTaskWithSubscriptions<TResult>(
            Func<Task<TResult>> commandFunc,
            IObservable<bool> canExecute,
            Func<TResult, Task> onExecutionResultAvailable,
            Func<Exception, Task>? onExceptionAction,
            IScheduler scheduler)
        {
            var command = ReactiveCommand.CreateFromTask(
                commandFunc,
                canExecute,
                outputScheduler: scheduler);
            var onExecutionResultAvailableSubscription = command.Subscribe(async s => await onExecutionResultAvailable(s));
            var onThrownExceptionSubscription = onExceptionAction != null
                ? command.ThrownExceptions.Subscribe(async e => await onExceptionAction(e))
                : null;

            return new(
                command,
                onExecutionResultAvailableSubscription,
                onThrownExceptionSubscription);
        }

        /// <summary>
        /// Creates a reactive command from a task with a complete and exception action.
        /// Assumes the command can always execute.
        /// </summary>
        /// <typeparam name="TInput">The type for the command input.</typeparam>
        /// <typeparam name="TResult">The type for the result passed around.</typeparam>
        /// <param name="commandFunc">The command function to execute.</param>
        /// <param name="canExecute">Observable indicating whether the command can execute.</param>
        /// <param name="onExecutionResultAvailable">Action to carry out when command completes. Used to update the UI.</param>
        /// <param name="onExceptionAction">Action to carry out on an exception. Used to notify the user of an error etc.</param>
        /// <param name="scheduler">Schedule to use for carrying out the command. Typically used for time travel in unit tests.</param>
        /// <returns>Reactive command, along with subscriptions to the execution result and possibly the thrown exception handler, if one was passed.</returns>
        public static ReactiveCommandFromTaskWithSubscriptionsResult<TInput, TResult> CreateFromTaskWithSubscriptions<TInput, TResult>(
            Func<TInput, Task<TResult>> commandFunc,
            IObservable<bool> canExecute,
            Func<TResult, Task> onExecutionResultAvailable,
            Func<Exception, Task>? onExceptionAction,
            IScheduler scheduler)
        {
            var command = ReactiveCommand.CreateFromTask(
                commandFunc,
                canExecute,
                outputScheduler: scheduler);
            var onExecutionResultAvailableSubscription = command.Subscribe(async s => await onExecutionResultAvailable(s));
            var onThrownExceptionSubscription = onExceptionAction != null
                ? command.ThrownExceptions.Subscribe(async e => await onExceptionAction(e))
                : null;

            return new(
                command,
                onExecutionResultAvailableSubscription,
                onThrownExceptionSubscription);
        }

        /// <summary>
        /// Creates a reactive command from a task with a complete and exception action.
        /// Assumes the command can always execute.
        /// </summary>
        /// <typeparam name="TInput">The type for the command input.</typeparam>
        /// <typeparam name="TResult">The type for the result passed around.</typeparam>
        /// <param name="commandFunc">The command function to execute.</param>
        /// <param name="onExecutionResultAvailable">Action to carry out when command completes. Used to update the UI.</param>
        /// <param name="onExceptionAction">Action to carry out on an exception. Used to notify the user of an error etc.</param>
        /// <param name="scheduler">Schedule to use for carrying out the command. Typically used for time travel in unit tests.</param>
        /// <returns>Reactive command, along with subscriptions to the execution result and possibly the thrown exception handler, if one was passed.</returns>
        public static ReactiveCommandFromTaskWithSubscriptionsResult<TInput, TResult> CreateFromTaskWithSubscriptions<TInput, TResult>(
            Func<TInput, Task<TResult>> commandFunc,
            Func<TResult, Task> onExecutionResultAvailable,
            Func<Exception, Task>? onExceptionAction,
            IScheduler scheduler)
        {
            var command = ReactiveCommand.CreateFromTask(
                commandFunc,
                outputScheduler: scheduler);
            var onExecutionResultAvailableSubscription = command.Subscribe(async s => await onExecutionResultAvailable(s));
            var onThrownExceptionSubscription = onExceptionAction != null
                ? command.ThrownExceptions.Subscribe(async e => await onExceptionAction(e))
                : null;

            return new(
                command,
                onExecutionResultAvailableSubscription,
                onThrownExceptionSubscription);
        }

        /// <summary>
        /// Creates a reactive command from a task with a complete and exception action.
        /// Assumes the command can always execute.
        /// </summary>
        /// <typeparam name="TResult">The type for the result passed around.</typeparam>
        /// <param name="commandFunc">The command function to execute.</param>
        /// <param name="onExecutionResultAvailable">Action to carry out when command completes. Used to update the UI.</param>
        /// <param name="onExceptionAction">Action to carry out on an exception. Used to notify the user of an error etc.</param>
        /// <param name="scheduler">Schedule to use for carrying out the command. Typically used for time travel in unit tests.</param>
        /// <returns>Reactive command, along with subscriptions to the execution result and possibly the thrown exception handler, if one was passed.</returns>
        public static ReactiveCommandFromTaskWithSubscriptionsResult<Unit, TResult> CreateFromTaskWithSubscriptions<TResult>(
            Func<Task<TResult>> commandFunc,
            Func<TResult, Task> onExecutionResultAvailable,
            Func<Exception, Task>? onExceptionAction,
            IScheduler scheduler)
        {
            var command = ReactiveCommand.CreateFromTask(
                commandFunc,
                outputScheduler: scheduler);
            var onExecutionResultAvailableSubscription = command.Subscribe(async s => await onExecutionResultAvailable(s));
            var onThrownExceptionSubscription = onExceptionAction != null
                ? command.ThrownExceptions.Subscribe(async e => await onExceptionAction(e))
                : null;

            return new(
                command,
                onExecutionResultAvailableSubscription,
                onThrownExceptionSubscription);
        }
    }
}
