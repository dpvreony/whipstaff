// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Linq.Expressions;
using System.Threading.Tasks;
using ReactiveUI;
using ReactiveUI.Primitives;
using ReactiveUI.Primitives.Concurrency;
using ReactiveUI.Primitives.Extensions;

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
        /// Creates a reactive command from a task where the result is mapped to a property, and an exception action is mapped.
        /// Checks if the command can be executed via control observable.
        /// </summary>
        /// <typeparam name="TObj">The type of the source object containing the property to update.</typeparam>
        /// <typeparam name="TResult">The type for the result passed around.</typeparam>
        /// <param name="commandFunc">The command function to execute.</param>
        /// <param name="canExecute">Observable indicating whether the command can execute.</param>
        /// <param name="propertySource">The source object containing the property to update.</param>
        /// <param name="property">Expression representing the property to update on the source object.</param>
        /// <param name="onExceptionAction">Action to carry out on an exception. Used to notify the user of an error etc.</param>
        /// <param name="sequencer">Schedule to use for carrying out the command. Typically used for time travel in unit tests.</param>
        /// <returns>Reactive command, along with subscriptions to the execution result and possibly the thrown exception handler, if one was passed.</returns>
        public static ReactiveCommandFromTaskWithSubscriptionsResult<RxVoid, TResult> CreateFromTaskToProperty<TObj, TResult>(
            System.Func<Task<TResult>> commandFunc,
            System.IObservable<bool> canExecute,
            TObj propertySource,
            Expression<System.Func<TObj, TResult>> property,
            System.Action<System.Exception>? onExceptionAction,
            ISequencer sequencer)
            where TObj : class, IReactiveObject
            where TResult : notnull
        {
            var command = ReactiveCommand.CreateFromTask(
                commandFunc,
                canExecute,
                sequencer);
            var onExecutionResultAvailableSubscription = command.ToProperty(propertySource, property);
            var onThrownExceptionSubscription = onExceptionAction != null
                ? command.ThrownExceptions.Subscribe(onExceptionAction)
                : null;

            return new(
                command,
                onExecutionResultAvailableSubscription,
                onThrownExceptionSubscription);
        }

        /// <summary>
        /// Creates a reactive command from a task where the result is mapped to a property, and an exception action is mapped.
        /// Assumes the command can always execute.
        /// </summary>
        /// <typeparam name="TObj">The type of the source object containing the property to update.</typeparam>
        /// <typeparam name="TResult">The type for the result passed around.</typeparam>
        /// <param name="commandFunc">The command function to execute.</param>
        /// <param name="propertySource">The source object containing the property to update.</param>
        /// <param name="property">Expression representing the property to update on the source object.</param>
        /// <param name="onExceptionAction">Action to carry out on an exception. Used to notify the user of an error etc.</param>
        /// <param name="sequencer">Schedule to use for carrying out the command. Typically used for time travel in unit tests.</param>
        /// <returns>Reactive command, along with subscriptions to the execution result and possibly the thrown exception handler, if one was passed.</returns>
        public static ReactiveCommandFromTaskWithSubscriptionsResult<RxVoid, TResult> CreateFromTaskToProperty<TObj, TResult>(
            System.Func<Task<TResult>> commandFunc,
            TObj propertySource,
            Expression<System.Func<TObj, TResult>> property,
            System.Action<System.Exception>? onExceptionAction,
            ISequencer sequencer)
            where TObj : class, IReactiveObject
            where TResult : notnull
        {
            var command = ReactiveCommand.CreateFromTask(
                commandFunc,
                outputScheduler: sequencer);
            var onExecutionResultAvailableSubscription = command.ToProperty(propertySource, property);
            var onThrownExceptionSubscription = onExceptionAction != null
                ? command.ThrownExceptions.Subscribe(onExceptionAction)
                : null;

            return new(
                command,
                onExecutionResultAvailableSubscription,
                onThrownExceptionSubscription);
        }

        /// <summary>
        /// Creates a reactive command from a task where the result is mapped to a property, and an exception action is mapped.
        /// Checks if the command can be executed via control observable.
        /// </summary>
        /// <typeparam name="TObj">The type of the source object containing the property to update.</typeparam>
        /// <typeparam name="TResult">The type for the result passed around.</typeparam>
        /// <param name="commandFunc">The command function to execute.</param>
        /// <param name="canExecute">Observable indicating whether the command can execute.</param>
        /// <param name="propertySource">The source object containing the property to update.</param>
        /// <param name="property">Expression representing the property to update on the source object.</param>
        /// <param name="onExceptionAction">Action to carry out on an exception. Used to notify the user of an error etc.</param>
        /// <param name="sequencer">Schedule to use for carrying out the command. Typically used for time travel in unit tests.</param>
        /// <returns>Reactive command, along with subscriptions to the execution result and possibly the thrown exception handler, if one was passed.</returns>
        public static ReactiveCommandFromTaskWithSubscriptionsResult<RxVoid, TResult> CreateFromTaskToProperty<TObj, TResult>(
            System.Func<Task<TResult>> commandFunc,
            System.IObservable<bool> canExecute,
            TObj propertySource,
            Expression<System.Func<TObj, TResult>> property,
            System.Func<System.Exception, ValueTask>? onExceptionAction,
            ISequencer sequencer)
            where TObj : class, IReactiveObject
            where TResult : notnull
        {
            var command = ReactiveCommand.CreateFromTask(
                commandFunc,
                canExecute,
                outputScheduler: sequencer);
            var onExecutionResultAvailableSubscription = command.ToProperty(propertySource, property);
            var onThrownExceptionSubscription = onExceptionAction != null
                ? command.ThrownExceptions.SubscribeAsync(e => onExceptionAction(e))
                : null;

            return new(
                command,
                onExecutionResultAvailableSubscription,
                onThrownExceptionSubscription);
        }

        /// <summary>
        /// Creates a reactive command from a task where the result is mapped to a property, and an exception action is mapped.
        /// Assumes the command can always execute.
        /// </summary>
        /// <typeparam name="TObj">The type of the source object containing the property to update.</typeparam>
        /// <typeparam name="TResult">The type for the result passed around.</typeparam>
        /// <param name="commandFunc">The command function to execute.</param>
        /// <param name="propertySource">The source object containing the property to update.</param>
        /// <param name="property">Expression representing the property to update on the source object.</param>
        /// <param name="onExceptionAction">Action to carry out on an exception. Used to notify the user of an error etc.</param>
        /// <param name="sequencer">Schedule to use for carrying out the command. Typically used for time travel in unit tests.</param>
        /// <returns>Reactive command, along with subscriptions to the execution result and possibly the thrown exception handler, if one was passed.</returns>
        public static ReactiveCommandFromTaskWithSubscriptionsResult<RxVoid, TResult> CreateFromTaskToProperty<TObj, TResult>(
            System.Func<Task<TResult>> commandFunc,
            TObj propertySource,
            Expression<System.Func<TObj, TResult>> property,
            System.Func<System.Exception, ValueTask>? onExceptionAction,
            ISequencer sequencer)
            where TObj : class, IReactiveObject
            where TResult : notnull
        {
            var command = ReactiveCommand.CreateFromTask(
                commandFunc,
                outputScheduler: sequencer);
            var onExecutionResultAvailableSubscription = command.ToProperty(propertySource, property);
            var onThrownExceptionSubscription = onExceptionAction != null
                ? command.ThrownExceptions.SubscribeAsync(e => onExceptionAction(e))
                : null;

            return new(
                command,
                onExecutionResultAvailableSubscription,
                onThrownExceptionSubscription);
        }

        /// <summary>
        /// Creates a reactive command from a task where the result is mapped to a property, and an exception action is mapped.
        /// Checks if the command can be executed via control observable.
        /// </summary>
        /// <typeparam name="TInput">The type for the command input.</typeparam>
        /// <typeparam name="TObj">The type of the source object containing the property to update.</typeparam>
        /// <typeparam name="TResult">The type for the result passed around.</typeparam>
        /// <param name="commandFunc">The command function to execute.</param>
        /// <param name="canExecute">Observable indicating whether the command can execute.</param>
        /// <param name="propertySource">The source object containing the property to update.</param>
        /// <param name="property">Expression representing the property to update on the source object.</param>
        /// <param name="onExceptionAction">Action to carry out on an exception. Used to notify the user of an error etc.</param>
        /// <param name="sequencer">Schedule to use for carrying out the command. Typically used for time travel in unit tests.</param>
        /// <returns>Reactive command, along with subscriptions to the execution result and possibly the thrown exception handler, if one was passed.</returns>
        public static ReactiveCommandFromTaskWithSubscriptionsResult<TInput, TResult> CreateFromTaskToProperty<TInput, TObj, TResult>(
            System.Func<TInput, Task<TResult>> commandFunc,
            System.IObservable<bool> canExecute,
            TObj propertySource,
            Expression<System.Func<TObj, TResult>> property,
            System.Func<System.Exception, Task>? onExceptionAction,
            ISequencer sequencer)
            where TObj : class, IReactiveObject
            where TResult : notnull
        {
            var command = ReactiveCommand.CreateFromTask(
                commandFunc,
                canExecute,
                outputScheduler: sequencer);
            var onExecutionResultAvailableSubscription = command.ToProperty(propertySource, property);
            var onThrownExceptionSubscription = onExceptionAction != null
                ? command.ThrownExceptions.SubscribeAsync(async e => await onExceptionAction(e))
                : null;

            return new(
                command,
                onExecutionResultAvailableSubscription,
                onThrownExceptionSubscription);
        }

        /// <summary>
        /// Creates a reactive command from a task where the result is mapped to a property, and an exception action is mapped.
        /// Assumes the command can always execute.
        /// </summary>
        /// <typeparam name="TInput">The type for the command input.</typeparam>
        /// <typeparam name="TObj">The type of the source object containing the property to update.</typeparam>
        /// <typeparam name="TResult">The type for the result passed around.</typeparam>
        /// <param name="commandFunc">The command function to execute.</param>
        /// <param name="propertySource">The source object containing the property to update.</param>
        /// <param name="property">Expression representing the property to update on the source object.</param>
        /// <param name="onExceptionAction">Action to carry out on an exception. Used to notify the user of an error etc.</param>
        /// <param name="sequencer">Sequencer to use for carrying out the command. Typically used for time travel in unit tests.</param>
        /// <returns>Reactive command, along with subscriptions to the execution result and possibly the thrown exception handler, if one was passed.</returns>
        public static ReactiveCommandFromTaskWithSubscriptionsResult<TInput, TResult> CreateFromTaskToProperty<TInput, TObj, TResult>(
            System.Func<TInput, Task<TResult>> commandFunc,
            TObj propertySource,
            Expression<System.Func<TObj, TResult>> property,
            System.Func<System.Exception, ValueTask>? onExceptionAction,
            ISequencer sequencer)
            where TObj : class, IReactiveObject
            where TResult : notnull
        {
            var command = ReactiveCommand.CreateFromTask(
                commandFunc,
                outputScheduler: sequencer);
            var onExecutionResultAvailableSubscription = command.ToProperty(propertySource, property);
            var onThrownExceptionSubscription = onExceptionAction != null
                ? command.ThrownExceptions.SubscribeAsync(e => onExceptionAction(e))
                : null;

            return new(
                command,
                onExecutionResultAvailableSubscription,
                onThrownExceptionSubscription);
        }

        /// <summary>
        /// Creates a reactive command from a task with a complete and exception action.
        /// Checks if the command can be executed via control observable.
        /// </summary>
        /// <typeparam name="TResult">The type for the result passed around.</typeparam>
        /// <param name="commandFunc">The command function to execute.</param>
        /// <param name="canExecute">Observable indicating whether the command can execute.</param>
        /// <param name="onExecutionResultAvailable">Action to carry out when command completes. Used to update the UI.</param>
        /// <param name="onExceptionAction">Action to carry out on an exception. Used to notify the user of an error etc.</param>
        /// <param name="sequencer">Sequencer to use for carrying out the command. Typically used for time travel in unit tests.</param>
        /// <returns>Reactive command, along with subscriptions to the execution result and possibly the thrown exception handler, if one was passed.</returns>
        public static ReactiveCommandFromTaskWithSubscriptionsResult<RxVoid, TResult> CreateFromTaskWithSubscriptions<TResult>(
            System.Func<Task<TResult>> commandFunc,
            System.IObservable<bool> canExecute,
            System.Action<TResult> onExecutionResultAvailable,
            System.Action<System.Exception>? onExceptionAction,
            ISequencer sequencer)
            where TResult : notnull
        {
            var command = ReactiveCommand.CreateFromTask(
                commandFunc,
                canExecute,
                sequencer);
            var onExecutionResultAvailableSubscription = command.Subscribe(result => onExecutionResultAvailable(result));
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
        /// <param name="sequencer">Sequencer to use for carrying out the command. Typically used for time travel in unit tests.</param>
        /// <returns>Reactive command, along with subscriptions to the execution result and possibly the thrown exception handler, if one was passed.</returns>
        public static ReactiveCommandFromTaskWithSubscriptionsResult<RxVoid, TResult> CreateFromTaskWithSubscriptions<TResult>(
            System.Func<Task<TResult>> commandFunc,
            System.Action<TResult> onExecutionResultAvailable,
            System.Action<System.Exception>? onExceptionAction,
            ISequencer sequencer)
            where TResult : notnull
        {
            var command = ReactiveCommand.CreateFromTask(
                commandFunc,
                outputScheduler: sequencer);
            var onExecutionResultAvailableSubscription = command.Subscribe(result => onExecutionResultAvailable(result));
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
        /// <param name="sequencer">Sequencer to use for carrying out the command. Typically used for time travel in unit tests.</param>
        /// <returns>Reactive command, along with subscriptions to the execution result and possibly the thrown exception handler, if one was passed.</returns>
        public static ReactiveCommandFromTaskWithSubscriptionsResult<RxVoid, TResult> CreateFromTaskWithSubscriptions<TResult>(
            System.Func<Task<TResult>> commandFunc,
            System.IObservable<bool> canExecute,
            System.Func<TResult, ValueTask> onExecutionResultAvailable,
            System.Func<System.Exception, ValueTask>? onExceptionAction,
            ISequencer sequencer)
            where TResult : notnull
        {
            var command = ReactiveCommand.CreateFromTask(
                commandFunc,
                canExecute,
                outputScheduler: sequencer);
            var onExecutionResultAvailableSubscription = command.SubscribeAsync(s => onExecutionResultAvailable(s));
            var onThrownExceptionSubscription = onExceptionAction != null
                ? command.ThrownExceptions.SubscribeAsync(e => onExceptionAction(e))
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
        /// <param name="sequencer">Sequencer to use for carrying out the command. Typically used for time travel in unit tests.</param>
        /// <returns>Reactive command, along with subscriptions to the execution result and possibly the thrown exception handler, if one was passed.</returns>
        public static ReactiveCommandFromTaskWithSubscriptionsResult<TInput, TResult> CreateFromTaskWithSubscriptions<TInput, TResult>(
            System.Func<TInput, Task<TResult>> commandFunc,
            System.IObservable<bool> canExecute,
            System.Func<TResult, ValueTask> onExecutionResultAvailable,
            System.Func<System.Exception, ValueTask>? onExceptionAction,
            ISequencer sequencer)
            where TResult : notnull
        {
            var command = ReactiveCommand.CreateFromTask(
                commandFunc,
                canExecute,
                outputScheduler: sequencer);
            var onExecutionResultAvailableSubscription = command.SubscribeAsync(s => onExecutionResultAvailable(s));
            var onThrownExceptionSubscription = onExceptionAction != null
                ? command.ThrownExceptions.SubscribeAsync(e => onExceptionAction(e))
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
        /// <param name="sequencer">Sequencer to use for carrying out the command. Typically used for time travel in unit tests.</param>
        /// <returns>Reactive command, along with subscriptions to the execution result and possibly the thrown exception handler, if one was passed.</returns>
        public static ReactiveCommandFromTaskWithSubscriptionsResult<TInput, TResult> CreateFromTaskWithSubscriptions<TInput, TResult>(
            System.Func<TInput, Task<TResult>> commandFunc,
            System.Func<TResult, ValueTask> onExecutionResultAvailable,
            System.Func<System.Exception, ValueTask>? onExceptionAction,
            ISequencer sequencer)
            where TResult : notnull
        {
            var command = ReactiveCommand.CreateFromTask(
                commandFunc,
                outputScheduler: sequencer);
            var onExecutionResultAvailableSubscription = command.SubscribeAsync(s => onExecutionResultAvailable(s));
            var onThrownExceptionSubscription = onExceptionAction != null
                ? command.ThrownExceptions.SubscribeAsync(e => onExceptionAction(e))
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
        /// <param name="sequencer">Sequencer to use for carrying out the command. Typically used for time travel in unit tests.</param>
        /// <returns>Reactive command, along with subscriptions to the execution result and possibly the thrown exception handler, if one was passed.</returns>
        public static ReactiveCommandFromTaskWithSubscriptionsResult<RxVoid, TResult> CreateFromTaskWithSubscriptions<TResult>(
            System.Func<Task<TResult>> commandFunc,
            System.Func<TResult, ValueTask> onExecutionResultAvailable,
            System.Func<System.Exception, ValueTask>? onExceptionAction,
            ISequencer sequencer)
            where TResult : notnull
        {
            var command = ReactiveCommand.CreateFromTask(
                commandFunc,
                outputScheduler: sequencer);
            var onExecutionResultAvailableSubscription = command.SubscribeAsync(s => onExecutionResultAvailable(s));
            var onThrownExceptionSubscription = onExceptionAction != null
                ? command.ThrownExceptions.SubscribeAsync(e => onExceptionAction(e))
                : null;

            return new(
                command,
                onExecutionResultAvailableSubscription,
                onThrownExceptionSubscription);
        }

        /// <summary>
        /// Creates a reactive command that runs in the background from a task where the result is mapped to a property, and an exception action is mapped.
        /// Checks if the command can be executed via control observable.
        /// </summary>
        /// <typeparam name="TObj">The type of the source object containing the property to update.</typeparam>
        /// <typeparam name="TResult">The type for the result passed around.</typeparam>
        /// <param name="commandFunc">The command function to execute.</param>
        /// <param name="canExecute">Observable indicating whether the command can execute.</param>
        /// <param name="propertySource">The source object containing the property to update.</param>
        /// <param name="property">Expression representing the property to update on the source object.</param>
        /// <param name="onExceptionAction">Action to carry out on an exception. Used to notify the user of an error etc.</param>
        /// <param name="sequencer">Sequencer to use for carrying out the command. Typically used for time travel in unit tests.</param>
        /// <returns>Reactive command, along with subscriptions to the execution result and possibly the thrown exception handler, if one was passed.</returns>
        public static ReactiveCommandFromTaskWithSubscriptionsResult<RxVoid, TResult> CreateRunInBackgroundToProperty<TObj, TResult>(
            System.Func<TResult> commandFunc,
            System.IObservable<bool> canExecute,
            TObj propertySource,
            Expression<System.Func<TObj, TResult>> property,
            System.Action<System.Exception>? onExceptionAction,
            ISequencer sequencer)
            where TObj : class, IReactiveObject
            where TResult : notnull
        {
            var command = ReactiveCommand.CreateRunInBackground(
                commandFunc,
                canExecute,
                sequencer);
            var onExecutionResultAvailableSubscription = command.ToProperty(propertySource, property);
            var onThrownExceptionSubscription = onExceptionAction != null
                ? command.ThrownExceptions.Subscribe(onExceptionAction)
                : null;

            return new(
                command,
                onExecutionResultAvailableSubscription,
                onThrownExceptionSubscription);
        }

        /// <summary>
        /// Creates a reactive command that runs in the background from a task where the result is mapped to a property, and an exception action is mapped.
        /// Assumes the command can always execute.
        /// </summary>
        /// <typeparam name="TObj">The type of the source object containing the property to update.</typeparam>
        /// <typeparam name="TResult">The type for the result passed around.</typeparam>
        /// <param name="commandFunc">The command function to execute.</param>
        /// <param name="propertySource">The source object containing the property to update.</param>
        /// <param name="property">Expression representing the property to update on the source object.</param>
        /// <param name="onExceptionAction">Action to carry out on an exception. Used to notify the user of an error etc.</param>
        /// <param name="backgroundScheduler">Scheduler to use for carrying out the command.</param>
        /// <param name="outputScheduler">Scheduler to use for observing the command results.</param>
        /// <returns>Reactive command, along with subscriptions to the execution result and possibly the thrown exception handler, if one was passed.</returns>
        public static ReactiveCommandFromTaskWithSubscriptionsResult<RxVoid, TResult> CreateRunInBackgroundToProperty<TObj, TResult>(
            System.Func<TResult> commandFunc,
            TObj propertySource,
            Expression<System.Func<TObj, TResult>> property,
            System.Action<System.Exception>? onExceptionAction,
            ISequencer backgroundScheduler,
            ISequencer outputScheduler)
            where TObj : class, IReactiveObject
            where TResult : notnull
        {
            var command = ReactiveCommand.CreateRunInBackground(
                commandFunc,
                backgroundScheduler,
                outputScheduler);
            var onExecutionResultAvailableSubscription = command.ToProperty(propertySource, property);
            var onThrownExceptionSubscription = onExceptionAction != null
                ? command.ThrownExceptions.Subscribe(onExceptionAction)
                : null;

            return new(
                command,
                onExecutionResultAvailableSubscription,
                onThrownExceptionSubscription);
        }

        /// <summary>
        /// Creates a reactive command that runs in the background from a task where the result is mapped to a property, and an exception action is mapped.
        /// Checks if the command can be executed via control observable.
        /// </summary>
        /// <typeparam name="TObj">The type of the source object containing the property to update.</typeparam>
        /// <typeparam name="TResult">The type for the result passed around.</typeparam>
        /// <param name="commandFunc">The command function to execute.</param>
        /// <param name="canExecute">Observable indicating whether the command can execute.</param>
        /// <param name="propertySource">The source object containing the property to update.</param>
        /// <param name="property">Expression representing the property to update on the source object.</param>
        /// <param name="onExceptionAction">Action to carry out on an exception. Used to notify the user of an error etc.</param>
        /// <param name="sequencer">Sequencer to use for carrying out the command. Typically used for time travel in unit tests.</param>
        /// <returns>Reactive command, along with subscriptions to the execution result and possibly the thrown exception handler, if one was passed.</returns>
        public static ReactiveCommandFromTaskWithSubscriptionsResult<RxVoid, TResult> CreateRunInBackgroundToProperty<TObj, TResult>(
            System.Func<TResult> commandFunc,
            System.IObservable<bool> canExecute,
            TObj propertySource,
            Expression<System.Func<TObj, TResult>> property,
            System.Func<System.Exception, ValueTask>? onExceptionAction,
            ISequencer sequencer)
            where TObj : class, IReactiveObject
            where TResult : notnull
        {
            var command = ReactiveCommand.CreateRunInBackground(
                commandFunc,
                canExecute,
                sequencer);
            var onExecutionResultAvailableSubscription = command.ToProperty(propertySource, property);
            var onThrownExceptionSubscription = onExceptionAction != null
                ? command.ThrownExceptions.SubscribeAsync(e => onExceptionAction(e))
                : null;

            return new(
                command,
                onExecutionResultAvailableSubscription,
                onThrownExceptionSubscription);
        }

        /// <summary>
        /// Creates a reactive command that runs in the background from a task where the result is mapped to a property, and an exception action is mapped.
        /// Assumes the command can always execute.
        /// </summary>
        /// <typeparam name="TObj">The type of the source object containing the property to update.</typeparam>
        /// <typeparam name="TResult">The type for the result passed around.</typeparam>
        /// <param name="commandFunc">The command function to execute.</param>
        /// <param name="propertySource">The source object containing the property to update.</param>
        /// <param name="property">Expression representing the property to update on the source object.</param>
        /// <param name="onExceptionAction">Action to carry out on an exception. Used to notify the user of an error etc.</param>
        /// <param name="backgroundScheduler">Scheduler to use for carrying out the command.</param>
        /// <param name="outputScheduler">Scheduler to use for observing the command results.</param>
        /// <returns>Reactive command, along with subscriptions to the execution result and possibly the thrown exception handler, if one was passed.</returns>
        public static ReactiveCommandFromTaskWithSubscriptionsResult<RxVoid, TResult> CreateRunInBackgroundToProperty<TObj, TResult>(
            System.Func<TResult> commandFunc,
            TObj propertySource,
            Expression<System.Func<TObj, TResult>> property,
            System.Func<System.Exception, ValueTask>? onExceptionAction,
            ISequencer backgroundScheduler,
            ISequencer outputScheduler)
            where TObj : class, IReactiveObject
            where TResult : notnull
        {
            var command = ReactiveCommand.CreateRunInBackground(
                commandFunc,
                backgroundScheduler,
                outputScheduler);
            var onExecutionResultAvailableSubscription = command.ToProperty(propertySource, property);
            var onThrownExceptionSubscription = onExceptionAction != null
                ? command.ThrownExceptions.SubscribeAsync(e => onExceptionAction(e))
                : null;

            return new(
                command,
                onExecutionResultAvailableSubscription,
                onThrownExceptionSubscription);
        }

        /// <summary>
        /// Creates a reactive command that runs in the background from a task where the result is mapped to a property, and an exception action is mapped.
        /// Checks if the command can be executed via control observable.
        /// </summary>
        /// <typeparam name="TInput">The type for the command input.</typeparam>
        /// <typeparam name="TObj">The type of the source object containing the property to update.</typeparam>
        /// <typeparam name="TResult">The type for the result passed around.</typeparam>
        /// <param name="commandFunc">The command function to execute.</param>
        /// <param name="canExecute">Observable indicating whether the command can execute.</param>
        /// <param name="propertySource">The source object containing the property to update.</param>
        /// <param name="property">Expression representing the property to update on the source object.</param>
        /// <param name="onExceptionAction">Action to carry out on an exception. Used to notify the user of an error etc.</param>
        /// <param name="backgroundScheduler">Scheduler to use for carrying out the command.</param>
        /// <param name="outputScheduler">Scheduler to use for observing the command results.</param>
        /// <returns>Reactive command, along with subscriptions to the execution result and possibly the thrown exception handler, if one was passed.</returns>
        public static ReactiveCommandFromTaskWithSubscriptionsResult<TInput, TResult> CreateRunInBackgroundToProperty<TInput, TObj, TResult>(
            System.Func<TInput, TResult> commandFunc,
            System.IObservable<bool> canExecute,
            TObj propertySource,
            Expression<System.Func<TObj, TResult>> property,
            System.Func<System.Exception, Task>? onExceptionAction,
            ISequencer backgroundScheduler,
            ISequencer outputScheduler)
            where TObj : class, IReactiveObject
            where TResult : notnull
        {
            var command = ReactiveCommand.CreateRunInBackground(
                commandFunc,
                canExecute,
                backgroundScheduler,
                outputScheduler);
            var onExecutionResultAvailableSubscription = command.ToProperty(propertySource, property);
            var onThrownExceptionSubscription = onExceptionAction != null
                ? command.ThrownExceptions.SubscribeAsync(async e => await onExceptionAction(e))
                : null;

            return new(
                command,
                onExecutionResultAvailableSubscription,
                onThrownExceptionSubscription);
        }

        /// <summary>
        /// Creates a reactive command that runs in the background from a task where the result is mapped to a property, and an exception action is mapped.
        /// Assumes the command can always execute.
        /// </summary>
        /// <typeparam name="TInput">The type for the command input.</typeparam>
        /// <typeparam name="TObj">The type of the source object containing the property to update.</typeparam>
        /// <typeparam name="TResult">The type for the result passed around.</typeparam>
        /// <param name="commandFunc">The command function to execute.</param>
        /// <param name="propertySource">The source object containing the property to update.</param>
        /// <param name="property">Expression representing the property to update on the source object.</param>
        /// <param name="onExceptionAction">Action to carry out on an exception. Used to notify the user of an error etc.</param>
        /// <param name="backgroundScheduler">Scheduler to use for carrying out the command.</param>
        /// <param name="outputScheduler">Scheduler to use for observing the command results.</param>
        /// <returns>Reactive command, along with subscriptions to the execution result and possibly the thrown exception handler, if one was passed.</returns>
        public static ReactiveCommandFromTaskWithSubscriptionsResult<TInput, TResult> CreateRunInBackgroundToProperty<TInput, TObj, TResult>(
            System.Func<TInput, TResult> commandFunc,
            TObj propertySource,
            Expression<System.Func<TObj, TResult>> property,
            System.Func<System.Exception, ValueTask>? onExceptionAction,
            ISequencer backgroundScheduler,
            ISequencer outputScheduler)
            where TObj : class, IReactiveObject
            where TResult : notnull
        {
            var command = ReactiveCommand.CreateRunInBackground(
                commandFunc,
                backgroundScheduler,
                outputScheduler);
            var onExecutionResultAvailableSubscription = command.ToProperty(propertySource, property);
            var onThrownExceptionSubscription = onExceptionAction != null
                ? command.ThrownExceptions.SubscribeAsync(e => onExceptionAction(e))
                : null;

            return new(
                command,
                onExecutionResultAvailableSubscription,
                onThrownExceptionSubscription);
        }

        /// <summary>
        /// Creates a reactive command that runs in the background from a task with a complete and exception action.
        /// Checks if the command can be executed via control observable.
        /// </summary>
        /// <typeparam name="TResult">The type for the result passed around.</typeparam>
        /// <param name="commandFunc">The command function to execute.</param>
        /// <param name="canExecute">Observable indicating whether the command can execute.</param>
        /// <param name="onExecutionResultAvailable">Action to carry out when command completes. Used to update the UI.</param>
        /// <param name="onExceptionAction">Action to carry out on an exception. Used to notify the user of an error etc.</param>
        /// <param name="backgroundScheduler">Scheduler to use for carrying out the command.</param>
        /// <param name="outputScheduler">Scheduler to use for observing the command results.</param>
        /// <returns>Reactive command, along with subscriptions to the execution result and possibly the thrown exception handler, if one was passed.</returns>
        public static ReactiveCommandFromTaskWithSubscriptionsResult<RxVoid, TResult> CreateRunInBackgroundWithSubscriptions<TResult>(
            System.Func<TResult> commandFunc,
            System.IObservable<bool> canExecute,
            System.Action<TResult> onExecutionResultAvailable,
            System.Action<System.Exception>? onExceptionAction,
            ISequencer backgroundScheduler,
            ISequencer outputScheduler)
            where TResult : notnull
        {
            var command = ReactiveCommand.CreateRunInBackground(
                commandFunc,
                canExecute,
                backgroundScheduler,
                outputScheduler);
            var onExecutionResultAvailableSubscription = command.Subscribe(result => onExecutionResultAvailable(result));
            var onThrownExceptionSubscription = onExceptionAction != null
                ? command.ThrownExceptions.Subscribe(onExceptionAction)
                : null;

            return new(
                command,
                onExecutionResultAvailableSubscription,
                onThrownExceptionSubscription);
        }

        /// <summary>
        /// Creates a reactive command that runs in the background from a task with a complete and exception action.
        /// Assumes the command can always execute.
        /// </summary>
        /// <typeparam name="TResult">The type for the result passed around.</typeparam>
        /// <param name="commandFunc">The command function to execute.</param>
        /// <param name="onExecutionResultAvailable">Action to carry out when command completes. Used to update the UI.</param>
        /// <param name="onExceptionAction">Action to carry out on an exception. Used to notify the user of an error etc.</param>
        /// <param name="backgroundScheduler">Scheduler to use for carrying out the command.</param>
        /// <param name="outputScheduler">Scheduler to use for observing the command results.</param>
        /// <returns>Reactive command, along with subscriptions to the execution result and possibly the thrown exception handler, if one was passed.</returns>
        public static ReactiveCommandFromTaskWithSubscriptionsResult<RxVoid, TResult> CreateRunInBackgroundWithSubscriptions<TResult>(
            System.Func<TResult> commandFunc,
            System.Action<TResult> onExecutionResultAvailable,
            System.Action<System.Exception>? onExceptionAction,
            ISequencer backgroundScheduler,
            ISequencer outputScheduler)
            where TResult : notnull
        {
            var command = ReactiveCommand.CreateRunInBackground(
                commandFunc,
                backgroundScheduler,
                outputScheduler);
            var onExecutionResultAvailableSubscription = command.Subscribe(result => onExecutionResultAvailable(result));
            var onThrownExceptionSubscription = onExceptionAction != null
                ? command.ThrownExceptions.Subscribe(onExceptionAction)
                : null;

            return new(
                command,
                onExecutionResultAvailableSubscription,
                onThrownExceptionSubscription);
        }

        /// <summary>
        /// Creates a reactive command that runs in the background from a task with a complete and exception action.
        /// Checks if the command can be executed via control observable.
        /// </summary>
        /// <typeparam name="TResult">The type for the result passed around.</typeparam>
        /// <param name="commandFunc">The command function to execute.</param>
        /// <param name="canExecute">Observable indicating whether the command can execute.</param>
        /// <param name="onExecutionResultAvailable">Action to carry out when command completes. Used to update the UI.</param>
        /// <param name="onExceptionAction">Action to carry out on an exception. Used to notify the user of an error etc.</param>
        /// <param name="backgroundScheduler">Scheduler to use for carrying out the command.</param>
        /// <param name="outputScheduler">Scheduler to use for observing the command results.</param>
        /// <returns>Reactive command, along with subscriptions to the execution result and possibly the thrown exception handler, if one was passed.</returns>
        public static ReactiveCommandFromTaskWithSubscriptionsResult<RxVoid, TResult> CreateRunInBackgroundWithSubscriptions<TResult>(
            System.Func<TResult> commandFunc,
            System.IObservable<bool> canExecute,
            System.Func<TResult, ValueTask> onExecutionResultAvailable,
            System.Func<System.Exception, ValueTask>? onExceptionAction,
            ISequencer backgroundScheduler,
            ISequencer outputScheduler)
            where TResult : notnull
        {
            var command = ReactiveCommand.CreateRunInBackground(
                commandFunc,
                canExecute,
                backgroundScheduler,
                outputScheduler);
            var onExecutionResultAvailableSubscription = command.SubscribeAsync(s => onExecutionResultAvailable(s));
            var onThrownExceptionSubscription = onExceptionAction != null
                ? command.ThrownExceptions.SubscribeAsync(e => onExceptionAction(e))
                : null;

            return new(
                command,
                onExecutionResultAvailableSubscription,
                onThrownExceptionSubscription);
        }

        /// <summary>
        /// Creates a reactive command that runs in the background from a task with a complete and exception action.
        /// Checks if the command can be executed via control observable.
        /// </summary>
        /// <typeparam name="TInput">The type for the command input.</typeparam>
        /// <typeparam name="TResult">The type for the result passed around.</typeparam>
        /// <param name="commandFunc">The command function to execute.</param>
        /// <param name="canExecute">Observable indicating whether the command can execute.</param>
        /// <param name="onExecutionResultAvailable">Action to carry out when command completes. Used to update the UI.</param>
        /// <param name="onExceptionAction">Action to carry out on an exception. Used to notify the user of an error etc.</param>
        /// <param name="backgroundScheduler">Scheduler to use for carrying out the command.</param>
        /// <param name="outputScheduler">Scheduler to use for observing the command results.</param>
        /// <returns>Reactive command, along with subscriptions to the execution result and possibly the thrown exception handler, if one was passed.</returns>
        public static ReactiveCommandFromTaskWithSubscriptionsResult<TInput, TResult> CreateRunInBackgroundWithSubscriptions<TInput, TResult>(
            System.Func<TInput, TResult> commandFunc,
            System.IObservable<bool> canExecute,
            System.Func<TResult, ValueTask> onExecutionResultAvailable,
            System.Func<System.Exception, ValueTask>? onExceptionAction,
            ISequencer backgroundScheduler,
            ISequencer outputScheduler)
            where TResult : notnull
        {
            var command = ReactiveCommand.CreateRunInBackground(
                commandFunc,
                canExecute,
                backgroundScheduler,
                outputScheduler);
            var onExecutionResultAvailableSubscription = command.SubscribeAsync(s => onExecutionResultAvailable(s));
            var onThrownExceptionSubscription = onExceptionAction != null
                ? command.ThrownExceptions.SubscribeAsync(e => onExceptionAction(e))
                : null;

            return new(
                command,
                onExecutionResultAvailableSubscription,
                onThrownExceptionSubscription);
        }

        /// <summary>
        /// Creates a reactive command that runs in the background from a task with a complete and exception action.
        /// Assumes the command can always execute.
        /// </summary>
        /// <typeparam name="TInput">The type for the command input.</typeparam>
        /// <typeparam name="TResult">The type for the result passed around.</typeparam>
        /// <param name="commandFunc">The command function to execute.</param>
        /// <param name="onExecutionResultAvailable">Action to carry out when command completes. Used to update the UI.</param>
        /// <param name="onExceptionAction">Action to carry out on an exception. Used to notify the user of an error etc.</param>
        /// <param name="backgroundScheduler">Scheduler to use for carrying out the command.</param>
        /// <param name="outputScheduler">Scheduler to use for observing the command results.</param>
        /// <returns>Reactive command, along with subscriptions to the execution result and possibly the thrown exception handler, if one was passed.</returns>
        public static ReactiveCommandFromTaskWithSubscriptionsResult<TInput, TResult> CreateRunInBackgroundWithSubscriptions<TInput, TResult>(
            System.Func<TInput, TResult> commandFunc,
            System.Func<TResult, ValueTask> onExecutionResultAvailable,
            System.Func<System.Exception, ValueTask>? onExceptionAction,
            ISequencer backgroundScheduler,
            ISequencer outputScheduler)
            where TResult : notnull
        {
            var command = ReactiveCommand.CreateRunInBackground(
                commandFunc,
                backgroundScheduler,
                outputScheduler);
            var onExecutionResultAvailableSubscription = command.SubscribeAsync(s => onExecutionResultAvailable(s));
            var onThrownExceptionSubscription = onExceptionAction != null
                ? command.ThrownExceptions.SubscribeAsync(e => onExceptionAction(e))
                : null;

            return new(
                command,
                onExecutionResultAvailableSubscription,
                onThrownExceptionSubscription);
        }

        /// <summary>
        /// Creates a reactive command that runs in the background from a task with a complete and exception action.
        /// Assumes the command can always execute.
        /// </summary>
        /// <typeparam name="TResult">The type for the result passed around.</typeparam>
        /// <param name="commandFunc">The command function to execute.</param>
        /// <param name="onExecutionResultAvailable">Action to carry out when command completes. Used to update the UI.</param>
        /// <param name="onExceptionAction">Action to carry out on an exception. Used to notify the user of an error etc.</param>
        /// <param name="backgroundScheduler">Scheduler to use for carrying out the command.</param>
        /// <param name="outputScheduler">Scheduler to use for observing the command results.</param>
        /// <returns>Reactive command, along with subscriptions to the execution result and possibly the thrown exception handler, if one was passed.</returns>
        public static ReactiveCommandFromTaskWithSubscriptionsResult<RxVoid, TResult> CreateRunInBackgroundWithSubscriptions<TResult>(
            System.Func<TResult> commandFunc,
            System.Func<TResult, ValueTask> onExecutionResultAvailable,
            System.Func<System.Exception, ValueTask>? onExceptionAction,
            ISequencer backgroundScheduler,
            ISequencer outputScheduler)
            where TResult : notnull
        {
            var command = ReactiveCommand.CreateRunInBackground(
                commandFunc,
                backgroundScheduler,
                outputScheduler);
            var onExecutionResultAvailableSubscription = command.SubscribeAsync(s => onExecutionResultAvailable(s));
            var onThrownExceptionSubscription = onExceptionAction != null
                ? command.ThrownExceptions.SubscribeAsync(e => onExceptionAction(e))
                : null;

            return new(
                command,
                onExecutionResultAvailableSubscription,
                onThrownExceptionSubscription);
        }
    }
}
