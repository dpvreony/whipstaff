// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using ReactiveUI;

#if ARGUMENT_NULL_EXCEPTION_SHIM
using ArgumentNullException = Whipstaff.Runtime.Exceptions.ArgumentNullException;
#endif

namespace Whipstaff.ReactiveUI.ReactiveCommands
{
    /// <summary>
    /// This class is a factory argument for handling creating a ReactiveCommand in helpers such as the ExclusiveCommands and ChainedCommand Factories.
    /// This was done to remove the need for lots of overloads as this class handles 4 permutations of ReactiveCommand creation.
    /// Where it would require a high number of overloads per command arg.
    /// For simple command creation still use the ReactiveCommand.Create overloads.
    /// note: this doesn't create an actual ReactiveCommand, it provides the methods and arguments to create one to the helper logic.
    /// </summary>
    /// <typeparam name="TParam">
    /// The type of the parameter passed through to command execution.
    /// </typeparam>
    /// <typeparam name="TCommandResult">
    /// The type of the command's result.
    /// </typeparam>
    /// <typeparam name="TExecuteResult">
    /// The type of the execute argument. Typically <typeparamref name="TCommandResult"/> itself or wrapped in <see cref="Task{TResult}"/> or <see cref="IObservable{TResult}"/> depending on the type of command.
    /// </typeparam>
    public sealed class ReactiveCommandFactoryArgument<TParam, TCommandResult, TExecuteResult>
    {
        private ReactiveCommandFactoryArgument(
            Func<Func<TParam, TExecuteResult>, IObservable<bool>?, IScheduler?, ReactiveCommand<TParam, TCommandResult>> factoryFunc,
            Func<TParam, TExecuteResult> execute,
            IObservable<bool>? canExecute = null,
            IScheduler? scheduler = null)
        {
            ArgumentNullException.ThrowIfNull(factoryFunc);
            ArgumentNullException.ThrowIfNull(execute);

            Execute = execute;
            FactoryFunc = factoryFunc;
            CanExecute = canExecute;
            Scheduler = scheduler;
        }

        /// <summary>
        /// Gets the factory function to create the ReactiveCommand.
        /// </summary>
        public Func<Func<TParam, TExecuteResult>, IObservable<bool>?, IScheduler?, ReactiveCommand<TParam, TCommandResult>> FactoryFunc { get; }

        /// <summary>
        /// Gets the observable to determine if the command can execute.
        /// </summary>
        public IObservable<bool>? CanExecute { get; }

        /// <summary>
        /// Gets the scheduler to run the command on.
        /// </summary>
        public IScheduler? Scheduler { get; }

        /// <summary>
        /// Gets the function to execute when the command is triggered.
        /// </summary>
        public Func<TParam, TExecuteResult> Execute { get; }

        /// <summary>
        /// Creates a ReactiveCommandFactoryArgument for a simple command.
        /// </summary>
        /// <param name="execute">The logic to execute when the command is triggered.</param>
        /// <param name="canExecute">Logic to use for whether the command can execute.</param>
        /// <param name="scheduler">The scheduler to run the command on.</param>
        /// <returns>Factory argument model representing the ReactiveCommand.</returns>
        public static ReactiveCommandFactoryArgument<TParam, TCommandResult, TCommandResult> Create(
            Func<TParam, TCommandResult> execute,
            IObservable<bool>? canExecute = null,
            IScheduler? scheduler = null)
        {
            return new ReactiveCommandFactoryArgument<TParam, TCommandResult, TCommandResult>(
                static (lambdaExecute, lambdaCanExecute, lambdaScheduler) => ReactiveCommand.Create(
                    lambdaExecute,
                    lambdaCanExecute,
                    lambdaScheduler),
                execute,
                canExecute,
                scheduler);
        }

        /// <summary>
        /// Creates a ReactiveCommandFactoryArgument for a command that returns an observable.
        /// </summary>
        /// <param name="execute">The logic to execute when the command is triggered.</param>
        /// <param name="canExecute">Logic to use for whether the command can execute.</param>
        /// <param name="scheduler">The scheduler to run the command on.</param>
        /// <returns>Factory argument model representing the ReactiveCommand.</returns>
        public static ReactiveCommandFactoryArgument<TParam, TCommandResult, IObservable<TCommandResult>> CreateFromObservable(
            Func<TParam, IObservable<TCommandResult>> execute,
            IObservable<bool>? canExecute = null,
            IScheduler? scheduler = null)
        {
            return new ReactiveCommandFactoryArgument<TParam, TCommandResult, IObservable<TCommandResult>>(
                static (lambdaExecute, lambdaCanExecute, lambdaScheduler) => ReactiveCommand.CreateFromObservable(
                    lambdaExecute,
                    lambdaCanExecute,
                    lambdaScheduler),
                execute,
                canExecute,
                scheduler);
        }

        /// <summary>
        /// Creates a ReactiveCommandFactoryArgument for a command that returns a task.
        /// </summary>
        /// <param name="execute">The logic to execute when the command is triggered.</param>
        /// <param name="canExecute">Logic to use for whether the command can execute.</param>
        /// <param name="scheduler">The scheduler to run the command on.</param>
        /// <returns>Factory argument model representing the ReactiveCommand.</returns>
        public static ReactiveCommandFactoryArgument<TParam, TCommandResult, Task<TCommandResult>> CreateFromTask(
            Func<TParam, Task<TCommandResult>> execute,
            IObservable<bool>? canExecute = null,
            IScheduler? scheduler = null)
        {
            return new ReactiveCommandFactoryArgument<TParam, TCommandResult, Task<TCommandResult>>(
                static (lambdaExecute, lambdaCanExecute, lambdaScheduler) => ReactiveCommand.CreateFromTask(
                    lambdaExecute,
                    lambdaCanExecute,
                    lambdaScheduler),
                execute,
                canExecute,
                scheduler);
        }

        /// <summary>
        /// Creates a ReactiveCommandFactoryArgument for a command that runs in the background.
        /// </summary>
        /// <param name="execute">The logic to execute when the command is triggered.</param>
        /// <param name="canExecute">Logic to use for whether the command can execute.</param>
        /// <param name="scheduler">The scheduler to run the command on.</param>
        /// <returns>Factory argument model representing the ReactiveCommand.</returns>
        public static ReactiveCommandFactoryArgument<TParam, TCommandResult, TCommandResult> CreateRunInBackground(
            Func<TParam, TCommandResult> execute,
            IObservable<bool>? canExecute = null,
            IScheduler? scheduler = null)
        {
            return new ReactiveCommandFactoryArgument<TParam, TCommandResult, TCommandResult>(
                static (lambdaExecute, lambdaCanExecute, lambdaScheduler) => ReactiveCommand.CreateRunInBackground(
                    lambdaExecute,
                    lambdaCanExecute,
                    lambdaScheduler),
                execute,
                canExecute,
                scheduler);
        }
    }
}
