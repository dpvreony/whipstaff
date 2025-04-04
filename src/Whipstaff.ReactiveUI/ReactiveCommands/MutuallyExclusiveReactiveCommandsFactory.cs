// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Threading.Tasks;
using ReactiveUI;
using Whipstaff.Rx;
using Whipstaff.Rx.Observables;

#if ARGUMENT_NULL_EXCEPTION_SHIM
using ArgumentNullException = Whipstaff.Runtime.Exceptions.ArgumentNullException;
#endif

namespace Whipstaff.ReactiveUI.ReactiveCommands
{
    /// <summary>
    /// Factory to produce commands that have an exclusive lock against each other.
    /// </summary>
    public static class MutuallyExclusiveReactiveCommandsFactory
    {
        /// <summary>
        /// Creates a set of exclusive commands that can be used to ensure that only one command is executing at a time.
        /// </summary>
        /// <typeparam name="T1Param">
        /// For the first command, the type of the parameter passed through to command execution.
        /// </typeparam>
        /// <typeparam name="T1CommandResult">
        /// For the first command, the type of the command's result.
        /// </typeparam>
        /// <typeparam name="T1ExecuteResult">
        /// For the first command, the type of the execute argument. Typically <typeparamref name="T1CommandResult"/> itself or wrapped in <see cref="Task{TResult}"/> or <see cref="IObservable{TResult}"/> depending on the type of command.
        /// </typeparam>
        /// <typeparam name="T2Param">
        /// For the second command, the type of the parameter passed through to command execution.
        /// </typeparam>
        /// <typeparam name="T2CommandResult">
        /// For the second command, the type of the command's result.
        /// </typeparam>
        /// <typeparam name="T2ExecuteResult">
        /// For the second command, the type of the execute argument. Typically <typeparamref name="T1CommandResult"/> itself or wrapped in <see cref="Task{TResult}"/> or <see cref="IObservable{TResult}"/> depending on the type of command.
        /// </typeparam>
        /// <param name="first">Factory arguments for the first command.</param>
        /// <param name="second">Factory arguments for the second command.</param>
        /// <returns>A tuple for the mutually exclusive lock, and the relevant commands.</returns>
        public static (
            BehaviorSubject<bool> NobodyIsExecuting,
            IDisposable ExclusiveLock,
            ReactiveCommand<T1Param, T1CommandResult> FirstCommand,
            ReactiveCommand<T2Param, T2CommandResult> SecondCommand) Create<
                T1Param,
                T1CommandResult,
                T1ExecuteResult,
                T2Param,
                T2CommandResult,
                T2ExecuteResult>(
            ReactiveCommandFactoryArgument<T1Param, T1CommandResult, T1ExecuteResult> first,
            ReactiveCommandFactoryArgument<T2Param, T2CommandResult, T2ExecuteResult> second)
        {
            ArgumentNullException.ThrowIfNull(first);
            ArgumentNullException.ThrowIfNull(second);

            var nobodyIsExecuting = new BehaviorSubject<bool>(false);

            var firstCommand = GetCommandWithCombinedCanExecute(
                nobodyIsExecuting,
                first);

            var secondCommand = GetCommandWithCombinedCanExecute(
                nobodyIsExecuting,
                second);

            var exclusiveLock = firstCommand.IsExecuting.CombineLatest(secondCommand.IsExecuting)
                .AllFalse()
                .Subscribe(nobodyIsExecuting);

            return (nobodyIsExecuting, exclusiveLock, firstCommand, secondCommand);
        }

        /// <summary>
        /// Creates a set of exclusive commands that can be used to ensure that only one command is executing at a time.
        /// </summary>
        /// <typeparam name="T1Param">
        /// For the first command, the type of the parameter passed through to command execution.
        /// </typeparam>
        /// <typeparam name="T1CommandResult">
        /// For the first command, the type of the command's result.
        /// </typeparam>
        /// <typeparam name="T1ExecuteResult">
        /// For the first command, the type of the execute argument. Typically <typeparamref name="T1CommandResult"/> itself or wrapped in <see cref="Task{TResult}"/> or <see cref="IObservable{TResult}"/> depending on the type of command.
        /// </typeparam>
        /// <typeparam name="T2Param">
        /// For the second command, the type of the parameter passed through to command execution.
        /// </typeparam>
        /// <typeparam name="T2CommandResult">
        /// For the second command, the type of the command's result.
        /// </typeparam>
        /// <typeparam name="T2ExecuteResult">
        /// For the second command, the type of the execute argument. Typically <typeparamref name="T1CommandResult"/> itself or wrapped in <see cref="Task{TResult}"/> or <see cref="IObservable{TResult}"/> depending on the type of command.
        /// </typeparam>
        /// <typeparam name="T3Param">
        /// For the third command, the type of the parameter passed through to command execution.
        /// </typeparam>
        /// <typeparam name="T3CommandResult">
        /// For the third command, the type of the command's result.
        /// </typeparam>
        /// <typeparam name="T3ExecuteResult">
        /// For the third command, the type of the execute argument. Typically <typeparamref name="T1CommandResult"/> itself or wrapped in <see cref="Task{TResult}"/> or <see cref="IObservable{TResult}"/> depending on the type of command.
        /// </typeparam>
        /// <param name="first">Factory arguments for the first command.</param>
        /// <param name="second">Factory arguments for the second command.</param>
        /// <param name="third">Factory arguments for the third command.</param>
        /// <returns>A tuple for the mutually exclusive lock, and the relevant commands.</returns>
        public static (
            BehaviorSubject<bool> NobodyIsExecuting,
            IDisposable ExclusiveLock,
            ReactiveCommand<T1Param, T1CommandResult> FirstCommand,
            ReactiveCommand<T2Param, T2CommandResult> SecondCommand,
            ReactiveCommand<T3Param, T3CommandResult> ThirdCommand) Create<
                T1Param,
                T1CommandResult,
                T1ExecuteResult,
                T2Param,
                T2CommandResult,
                T2ExecuteResult,
                T3Param,
                T3CommandResult,
                T3ExecuteResult>(
                ReactiveCommandFactoryArgument<T1Param, T1CommandResult, T1ExecuteResult> first,
                ReactiveCommandFactoryArgument<T2Param, T2CommandResult, T2ExecuteResult> second,
                ReactiveCommandFactoryArgument<T3Param, T3CommandResult, T3ExecuteResult> third)
        {
            ArgumentNullException.ThrowIfNull(first);
            ArgumentNullException.ThrowIfNull(second);
            ArgumentNullException.ThrowIfNull(third);

            var nobodyIsExecuting = new BehaviorSubject<bool>(false);

            var firstCommand = GetCommandWithCombinedCanExecute(
                nobodyIsExecuting,
                first);

            var secondCommand = GetCommandWithCombinedCanExecute(
                nobodyIsExecuting,
                second);

            var thirdCommand = GetCommandWithCombinedCanExecute(
                nobodyIsExecuting,
                third);

            var exclusiveLock = firstCommand.IsExecuting.CombineLatest(
                    secondCommand.IsExecuting,
                    thirdCommand.IsExecuting)
                .AllFalse()
                .Subscribe(nobodyIsExecuting);

            return (nobodyIsExecuting, exclusiveLock, firstCommand, secondCommand, thirdCommand);
        }

        /// <summary>
        /// Creates a set of exclusive commands that can be used to ensure that only one command is executing at a time.
        /// </summary>
        /// <typeparam name="T1Param">
        /// For the first command, the type of the parameter passed through to command execution.
        /// </typeparam>
        /// <typeparam name="T1CommandResult">
        /// For the first command, the type of the command's result.
        /// </typeparam>
        /// <typeparam name="T1ExecuteResult">
        /// For the first command, the type of the execute argument. Typically <typeparamref name="T1CommandResult"/> itself or wrapped in <see cref="Task{TResult}"/> or <see cref="IObservable{TResult}"/> depending on the type of command.
        /// </typeparam>
        /// <typeparam name="T2Param">
        /// For the second command, the type of the parameter passed through to command execution.
        /// </typeparam>
        /// <typeparam name="T2CommandResult">
        /// For the second command, the type of the command's result.
        /// </typeparam>
        /// <typeparam name="T2ExecuteResult">
        /// For the second command, the type of the execute argument. Typically <typeparamref name="T1CommandResult"/> itself or wrapped in <see cref="Task{TResult}"/> or <see cref="IObservable{TResult}"/> depending on the type of command.
        /// </typeparam>
        /// <typeparam name="T3Param">
        /// For the third command, the type of the parameter passed through to command execution.
        /// </typeparam>
        /// <typeparam name="T3CommandResult">
        /// For the third command, the type of the command's result.
        /// </typeparam>
        /// <typeparam name="T3ExecuteResult">
        /// For the third command, the type of the execute argument. Typically <typeparamref name="T1CommandResult"/> itself or wrapped in <see cref="Task{TResult}"/> or <see cref="IObservable{TResult}"/> depending on the type of command.
        /// </typeparam>
        /// <typeparam name="T4Param">
        /// For the fourth command, the type of the parameter passed through to command execution.
        /// </typeparam>
        /// <typeparam name="T4CommandResult">
        /// For the fourth command, the type of the command's result.
        /// </typeparam>
        /// <typeparam name="T4ExecuteResult">
        /// For the fourth command, the type of the execute argument. Typically <typeparamref name="T1CommandResult"/> itself or wrapped in <see cref="Task{TResult}"/> or <see cref="IObservable{TResult}"/> depending on the type of command.
        /// </typeparam>
        /// <param name="first">Factory arguments for the first command.</param>
        /// <param name="second">Factory arguments for the second command.</param>
        /// <param name="third">Factory arguments for the third command.</param>
        /// <param name="fourth">Factory arguments for the fourth command.</param>
        /// <returns>A tuple for the mutually exclusive lock, and the relevant commands.</returns>
        public static (
            BehaviorSubject<bool> NobodyIsExecuting,
            IDisposable ExclusiveLock,
            ReactiveCommand<T1Param, T1CommandResult> FirstCommand,
            ReactiveCommand<T2Param, T2CommandResult> SecondCommand,
            ReactiveCommand<T3Param, T3CommandResult> ThirdCommand,
            ReactiveCommand<T4Param, T4CommandResult> FourthCommand) Create<
                T1Param,
                T1CommandResult,
                T1ExecuteResult,
                T2Param,
                T2CommandResult,
                T2ExecuteResult,
                T3Param,
                T3CommandResult,
                T3ExecuteResult,
                T4Param,
                T4CommandResult,
                T4ExecuteResult>(
                ReactiveCommandFactoryArgument<T1Param, T1CommandResult, T1ExecuteResult> first,
                ReactiveCommandFactoryArgument<T2Param, T2CommandResult, T2ExecuteResult> second,
                ReactiveCommandFactoryArgument<T3Param, T3CommandResult, T3ExecuteResult> third,
                ReactiveCommandFactoryArgument<T4Param, T4CommandResult, T4ExecuteResult> fourth)
        {
            ArgumentNullException.ThrowIfNull(first);
            ArgumentNullException.ThrowIfNull(second);
            ArgumentNullException.ThrowIfNull(third);
            ArgumentNullException.ThrowIfNull(fourth);

            var nobodyIsExecuting = new BehaviorSubject<bool>(false);

            var firstCommand = GetCommandWithCombinedCanExecute(
                nobodyIsExecuting,
                first);

            var secondCommand = GetCommandWithCombinedCanExecute(
                nobodyIsExecuting,
                second);

            var thirdCommand = GetCommandWithCombinedCanExecute(
                nobodyIsExecuting,
                third);

            var fourthCommand = GetCommandWithCombinedCanExecute(
                nobodyIsExecuting,
                fourth);

            var exclusiveLock = firstCommand.IsExecuting.CombineLatest(
                    secondCommand.IsExecuting,
                    thirdCommand.IsExecuting,
                    fourthCommand.IsExecuting)
                .AllFalse()
                .Subscribe(nobodyIsExecuting);

            return (nobodyIsExecuting, exclusiveLock, firstCommand, secondCommand, thirdCommand, fourthCommand);
        }

        /// <summary>
        /// Creates a set of exclusive commands that can be used to ensure that only one command is executing at a time.
        /// </summary>
        /// <typeparam name="T1Param">
        /// For the first command, the type of the parameter passed through to command execution.
        /// </typeparam>
        /// <typeparam name="T1CommandResult">
        /// For the first command, the type of the command's result.
        /// </typeparam>
        /// <typeparam name="T1ExecuteResult">
        /// For the first command, the type of the execute argument. Typically <typeparamref name="T1CommandResult"/> itself or wrapped in <see cref="Task{TResult}"/> or <see cref="IObservable{TResult}"/> depending on the type of command.
        /// </typeparam>
        /// <typeparam name="T2Param">
        /// For the second command, the type of the parameter passed through to command execution.
        /// </typeparam>
        /// <typeparam name="T2CommandResult">
        /// For the second command, the type of the command's result.
        /// </typeparam>
        /// <typeparam name="T2ExecuteResult">
        /// For the second command, the type of the execute argument. Typically <typeparamref name="T1CommandResult"/> itself or wrapped in <see cref="Task{TResult}"/> or <see cref="IObservable{TResult}"/> depending on the type of command.
        /// </typeparam>
        /// <typeparam name="T3Param">
        /// For the third command, the type of the parameter passed through to command execution.
        /// </typeparam>
        /// <typeparam name="T3CommandResult">
        /// For the third command, the type of the command's result.
        /// </typeparam>
        /// <typeparam name="T3ExecuteResult">
        /// For the third command, the type of the execute argument. Typically <typeparamref name="T1CommandResult"/> itself or wrapped in <see cref="Task{TResult}"/> or <see cref="IObservable{TResult}"/> depending on the type of command.
        /// </typeparam>
        /// <typeparam name="T4Param">
        /// For the fourth command, the type of the parameter passed through to command execution.
        /// </typeparam>
        /// <typeparam name="T4CommandResult">
        /// For the fourth command, the type of the command's result.
        /// </typeparam>
        /// <typeparam name="T4ExecuteResult">
        /// For the fourth command, the type of the execute argument. Typically <typeparamref name="T1CommandResult"/> itself or wrapped in <see cref="Task{TResult}"/> or <see cref="IObservable{TResult}"/> depending on the type of command.
        /// </typeparam>
        /// <typeparam name="T5Param">
        /// For the fifth command, the type of the parameter passed through to command execution.
        /// </typeparam>
        /// <typeparam name="T5CommandResult">
        /// For the fifth command, the type of the command's result.
        /// </typeparam>
        /// <typeparam name="T5ExecuteResult">
        /// For the fifth command, the type of the execute argument. Typically <typeparamref name="T1CommandResult"/> itself or wrapped in <see cref="Task{TResult}"/> or <see cref="IObservable{TResult}"/> depending on the type of command.
        /// </typeparam>
        /// <param name="first">Factory arguments for the first command.</param>
        /// <param name="second">Factory arguments for the second command.</param>
        /// <param name="third">Factory arguments for the third command.</param>
        /// <param name="fourth">Factory arguments for the fourth command.</param>
        /// <param name="fifth">Factory arguments for the fifth command.</param>
        /// <returns>A tuple for the mutually exclusive lock, and the relevant commands.</returns>
        public static (
            BehaviorSubject<bool> NobodyIsExecuting,
            IDisposable ExclusiveLock,
            ReactiveCommand<T1Param, T1CommandResult> FirstCommand,
            ReactiveCommand<T2Param, T2CommandResult> SecondCommand,
            ReactiveCommand<T3Param, T3CommandResult> ThirdCommand,
            ReactiveCommand<T4Param, T4CommandResult> FourthCommand,
            ReactiveCommand<T5Param, T5CommandResult> FifthCommand) Create<
                T1Param,
                T1CommandResult,
                T1ExecuteResult,
                T2Param,
                T2CommandResult,
                T2ExecuteResult,
                T3Param,
                T3CommandResult,
                T3ExecuteResult,
                T4Param,
                T4CommandResult,
                T4ExecuteResult,
                T5Param,
                T5CommandResult,
                T5ExecuteResult>(
                ReactiveCommandFactoryArgument<T1Param, T1CommandResult, T1ExecuteResult> first,
                ReactiveCommandFactoryArgument<T2Param, T2CommandResult, T2ExecuteResult> second,
                ReactiveCommandFactoryArgument<T3Param, T3CommandResult, T3ExecuteResult> third,
                ReactiveCommandFactoryArgument<T4Param, T4CommandResult, T4ExecuteResult> fourth,
                ReactiveCommandFactoryArgument<T5Param, T5CommandResult, T5ExecuteResult> fifth)
        {
            ArgumentNullException.ThrowIfNull(first);
            ArgumentNullException.ThrowIfNull(second);
            ArgumentNullException.ThrowIfNull(third);
            ArgumentNullException.ThrowIfNull(fourth);
            ArgumentNullException.ThrowIfNull(fifth);

            var nobodyIsExecuting = new BehaviorSubject<bool>(false);

            var firstCommand = GetCommandWithCombinedCanExecute(
                nobodyIsExecuting,
                first);

            var secondCommand = GetCommandWithCombinedCanExecute(
                nobodyIsExecuting,
                second);

            var thirdCommand = GetCommandWithCombinedCanExecute(
                nobodyIsExecuting,
                third);

            var fourthCommand = GetCommandWithCombinedCanExecute(
                nobodyIsExecuting,
                fourth);

            var fifthCommand = GetCommandWithCombinedCanExecute(
                nobodyIsExecuting,
                fifth);

            var exclusiveLock = firstCommand.IsExecuting.CombineLatest(
                    secondCommand.IsExecuting,
                    thirdCommand.IsExecuting,
                    fourthCommand.IsExecuting,
                    fifthCommand.IsExecuting)
                .AllFalse()
                .Subscribe(nobodyIsExecuting);

            return (nobodyIsExecuting, exclusiveLock, firstCommand, secondCommand, thirdCommand, fourthCommand, fifthCommand);
        }

        /// <summary>
        /// Creates a set of exclusive commands that can be used to ensure that only one command is executing at a time.
        /// </summary>
        /// <typeparam name="T1Param">
        /// For the first command, the type of the parameter passed through to command execution.
        /// </typeparam>
        /// <typeparam name="T1CommandResult">
        /// For the first command, the type of the command's result.
        /// </typeparam>
        /// <typeparam name="T1ExecuteResult">
        /// For the first command, the type of the execute argument. Typically <typeparamref name="T1CommandResult"/> itself or wrapped in <see cref="Task{TResult}"/> or <see cref="IObservable{TResult}"/> depending on the type of command.
        /// </typeparam>
        /// <typeparam name="T2Param">
        /// For the second command, the type of the parameter passed through to command execution.
        /// </typeparam>
        /// <typeparam name="T2CommandResult">
        /// For the second command, the type of the command's result.
        /// </typeparam>
        /// <typeparam name="T2ExecuteResult">
        /// For the second command, the type of the execute argument. Typically <typeparamref name="T1CommandResult"/> itself or wrapped in <see cref="Task{TResult}"/> or <see cref="IObservable{TResult}"/> depending on the type of command.
        /// </typeparam>
        /// <typeparam name="T3Param">
        /// For the third command, the type of the parameter passed through to command execution.
        /// </typeparam>
        /// <typeparam name="T3CommandResult">
        /// For the third command, the type of the command's result.
        /// </typeparam>
        /// <typeparam name="T3ExecuteResult">
        /// For the third command, the type of the execute argument. Typically <typeparamref name="T1CommandResult"/> itself or wrapped in <see cref="Task{TResult}"/> or <see cref="IObservable{TResult}"/> depending on the type of command.
        /// </typeparam>
        /// <typeparam name="T4Param">
        /// For the fourth command, the type of the parameter passed through to command execution.
        /// </typeparam>
        /// <typeparam name="T4CommandResult">
        /// For the fourth command, the type of the command's result.
        /// </typeparam>
        /// <typeparam name="T4ExecuteResult">
        /// For the fourth command, the type of the execute argument. Typically <typeparamref name="T1CommandResult"/> itself or wrapped in <see cref="Task{TResult}"/> or <see cref="IObservable{TResult}"/> depending on the type of command.
        /// </typeparam>
        /// <typeparam name="T5Param">
        /// For the fifth command, the type of the parameter passed through to command execution.
        /// </typeparam>
        /// <typeparam name="T5CommandResult">
        /// For the fifth command, the type of the command's result.
        /// </typeparam>
        /// <typeparam name="T5ExecuteResult">
        /// For the fifth command, the type of the execute argument. Typically <typeparamref name="T1CommandResult"/> itself or wrapped in <see cref="Task{TResult}"/> or <see cref="IObservable{TResult}"/> depending on the type of command.
        /// </typeparam>
        /// <typeparam name="T6Param">
        /// For the sixth command, the type of the parameter passed through to command execution.
        /// </typeparam>
        /// <typeparam name="T6CommandResult">
        /// For the sixth command, the type of the command's result.
        /// </typeparam>
        /// <typeparam name="T6ExecuteResult">
        /// For the sixth command, the type of the execute argument. Typically <typeparamref name="T1CommandResult"/> itself or wrapped in <see cref="Task{TResult}"/> or <see cref="IObservable{TResult}"/> depending on the type of command.
        /// </typeparam>
        /// <param name="first">Factory arguments for the first command.</param>
        /// <param name="second">Factory arguments for the second command.</param>
        /// <param name="third">Factory arguments for the third command.</param>
        /// <param name="fourth">Factory arguments for the fourth command.</param>
        /// <param name="fifth">Factory arguments for the fifth command.</param>
        /// <param name="sixth">Factory arguments for the sixth command.</param>
        /// <returns>A tuple for the mutually exclusive lock, and the relevant commands.</returns>
        public static (
            BehaviorSubject<bool> NobodyIsExecuting,
            IDisposable ExclusiveLock,
            ReactiveCommand<T1Param, T1CommandResult> FirstCommand,
            ReactiveCommand<T2Param, T2CommandResult> SecondCommand,
            ReactiveCommand<T3Param, T3CommandResult> ThirdCommand,
            ReactiveCommand<T4Param, T4CommandResult> FourthCommand,
            ReactiveCommand<T5Param, T5CommandResult> FifthCommand,
            ReactiveCommand<T6Param, T6CommandResult> SixthCommand) Create<
                T1Param,
                T1CommandResult,
                T1ExecuteResult,
                T2Param,
                T2CommandResult,
                T2ExecuteResult,
                T3Param,
                T3CommandResult,
                T3ExecuteResult,
                T4Param,
                T4CommandResult,
                T4ExecuteResult,
                T5Param,
                T5CommandResult,
                T5ExecuteResult,
                T6Param,
                T6CommandResult,
                T6ExecuteResult>(
                ReactiveCommandFactoryArgument<T1Param, T1CommandResult, T1ExecuteResult> first,
                ReactiveCommandFactoryArgument<T2Param, T2CommandResult, T2ExecuteResult> second,
                ReactiveCommandFactoryArgument<T3Param, T3CommandResult, T3ExecuteResult> third,
                ReactiveCommandFactoryArgument<T4Param, T4CommandResult, T4ExecuteResult> fourth,
                ReactiveCommandFactoryArgument<T5Param, T5CommandResult, T5ExecuteResult> fifth,
                ReactiveCommandFactoryArgument<T6Param, T6CommandResult, T6ExecuteResult> sixth)
        {
            ArgumentNullException.ThrowIfNull(first);
            ArgumentNullException.ThrowIfNull(second);
            ArgumentNullException.ThrowIfNull(third);
            ArgumentNullException.ThrowIfNull(fourth);
            ArgumentNullException.ThrowIfNull(fifth);
            ArgumentNullException.ThrowIfNull(sixth);

            var nobodyIsExecuting = new BehaviorSubject<bool>(false);

            var firstCommand = GetCommandWithCombinedCanExecute(
                nobodyIsExecuting,
                first);

            var secondCommand = GetCommandWithCombinedCanExecute(
                nobodyIsExecuting,
                second);

            var thirdCommand = GetCommandWithCombinedCanExecute(
                nobodyIsExecuting,
                third);

            var fourthCommand = GetCommandWithCombinedCanExecute(
                nobodyIsExecuting,
                fourth);

            var fifthCommand = GetCommandWithCombinedCanExecute(
                nobodyIsExecuting,
                fifth);

            var sixthCommand = GetCommandWithCombinedCanExecute(
                nobodyIsExecuting,
                sixth);

            var exclusiveLock = firstCommand.IsExecuting.CombineLatest(
                    secondCommand.IsExecuting,
                    thirdCommand.IsExecuting,
                    fourthCommand.IsExecuting,
                    fifthCommand.IsExecuting,
                    sixthCommand.IsExecuting)
                .AllFalse()
                .Subscribe(nobodyIsExecuting);

            return (nobodyIsExecuting, exclusiveLock, firstCommand, secondCommand, thirdCommand, fourthCommand, fifthCommand, sixthCommand);
        }

        /// <summary>
        /// Creates a set of exclusive commands that can be used to ensure that only one command is executing at a time.
        /// </summary>
        /// <typeparam name="T1Param">
        /// For the first command, the type of the parameter passed through to command execution.
        /// </typeparam>
        /// <typeparam name="T1CommandResult">
        /// For the first command, the type of the command's result.
        /// </typeparam>
        /// <typeparam name="T1ExecuteResult">
        /// For the first command, the type of the execute argument. Typically <typeparamref name="T1CommandResult"/> itself or wrapped in <see cref="Task{TResult}"/> or <see cref="IObservable{TResult}"/> depending on the type of command.
        /// </typeparam>
        /// <typeparam name="T2Param">
        /// For the second command, the type of the parameter passed through to command execution.
        /// </typeparam>
        /// <typeparam name="T2CommandResult">
        /// For the second command, the type of the command's result.
        /// </typeparam>
        /// <typeparam name="T2ExecuteResult">
        /// For the second command, the type of the execute argument. Typically <typeparamref name="T1CommandResult"/> itself or wrapped in <see cref="Task{TResult}"/> or <see cref="IObservable{TResult}"/> depending on the type of command.
        /// </typeparam>
        /// <typeparam name="T3Param">
        /// For the third command, the type of the parameter passed through to command execution.
        /// </typeparam>
        /// <typeparam name="T3CommandResult">
        /// For the third command, the type of the command's result.
        /// </typeparam>
        /// <typeparam name="T3ExecuteResult">
        /// For the third command, the type of the execute argument. Typically <typeparamref name="T1CommandResult"/> itself or wrapped in <see cref="Task{TResult}"/> or <see cref="IObservable{TResult}"/> depending on the type of command.
        /// </typeparam>
        /// <typeparam name="T4Param">
        /// For the fourth command, the type of the parameter passed through to command execution.
        /// </typeparam>
        /// <typeparam name="T4CommandResult">
        /// For the fourth command, the type of the command's result.
        /// </typeparam>
        /// <typeparam name="T4ExecuteResult">
        /// For the fourth command, the type of the execute argument. Typically <typeparamref name="T1CommandResult"/> itself or wrapped in <see cref="Task{TResult}"/> or <see cref="IObservable{TResult}"/> depending on the type of command.
        /// </typeparam>
        /// <typeparam name="T5Param">
        /// For the fifth command, the type of the parameter passed through to command execution.
        /// </typeparam>
        /// <typeparam name="T5CommandResult">
        /// For the fifth command, the type of the command's result.
        /// </typeparam>
        /// <typeparam name="T5ExecuteResult">
        /// For the fifth command, the type of the execute argument. Typically <typeparamref name="T1CommandResult"/> itself or wrapped in <see cref="Task{TResult}"/> or <see cref="IObservable{TResult}"/> depending on the type of command.
        /// </typeparam>
        /// <typeparam name="T6Param">
        /// For the sixth command, the type of the parameter passed through to command execution.
        /// </typeparam>
        /// <typeparam name="T6CommandResult">
        /// For the sixth command, the type of the command's result.
        /// </typeparam>
        /// <typeparam name="T6ExecuteResult">
        /// For the sixth command, the type of the execute argument. Typically <typeparamref name="T1CommandResult"/> itself or wrapped in <see cref="Task{TResult}"/> or <see cref="IObservable{TResult}"/> depending on the type of command.
        /// </typeparam>
        /// <typeparam name="T7Param">
        /// For the seventh command, the type of the parameter passed through to command execution.
        /// </typeparam>
        /// <typeparam name="T7CommandResult">
        /// For the seventh command, the type of the command's result.
        /// </typeparam>
        /// <typeparam name="T7ExecuteResult">
        /// For the seventh command, the type of the execute argument. Typically <typeparamref name="T1CommandResult"/> itself or wrapped in <see cref="Task{TResult}"/> or <see cref="IObservable{TResult}"/> depending on the type of command.
        /// </typeparam>
        /// <param name="first">Factory arguments for the first command.</param>
        /// <param name="second">Factory arguments for the second command.</param>
        /// <param name="third">Factory arguments for the third command.</param>
        /// <param name="fourth">Factory arguments for the fourth command.</param>
        /// <param name="fifth">Factory arguments for the fifth command.</param>
        /// <param name="sixth">Factory arguments for the sixth command.</param>
        /// <param name="seventh">Factory arguments for the seventh command.</param>
        /// <returns>A tuple for the mutually exclusive lock, and the relevant commands.</returns>
        public static (
            BehaviorSubject<bool> NobodyIsExecuting,
            IDisposable ExclusiveLock,
            ReactiveCommand<T1Param, T1CommandResult> FirstCommand,
            ReactiveCommand<T2Param, T2CommandResult> SecondCommand,
            ReactiveCommand<T3Param, T3CommandResult> ThirdCommand,
            ReactiveCommand<T4Param, T4CommandResult> FourthCommand,
            ReactiveCommand<T5Param, T5CommandResult> FifthCommand,
            ReactiveCommand<T6Param, T6CommandResult> SixthCommand,
            ReactiveCommand<T7Param, T7CommandResult> SeventhCommand) Create<
                T1Param,
                T1CommandResult,
                T1ExecuteResult,
                T2Param,
                T2CommandResult,
                T2ExecuteResult,
                T3Param,
                T3CommandResult,
                T3ExecuteResult,
                T4Param,
                T4CommandResult,
                T4ExecuteResult,
                T5Param,
                T5CommandResult,
                T5ExecuteResult,
                T6Param,
                T6CommandResult,
                T6ExecuteResult,
                T7Param,
                T7CommandResult,
                T7ExecuteResult>(
                ReactiveCommandFactoryArgument<T1Param, T1CommandResult, T1ExecuteResult> first,
                ReactiveCommandFactoryArgument<T2Param, T2CommandResult, T2ExecuteResult> second,
                ReactiveCommandFactoryArgument<T3Param, T3CommandResult, T3ExecuteResult> third,
                ReactiveCommandFactoryArgument<T4Param, T4CommandResult, T4ExecuteResult> fourth,
                ReactiveCommandFactoryArgument<T5Param, T5CommandResult, T5ExecuteResult> fifth,
                ReactiveCommandFactoryArgument<T6Param, T6CommandResult, T6ExecuteResult> sixth,
                ReactiveCommandFactoryArgument<T7Param, T7CommandResult, T7ExecuteResult> seventh)
        {
            ArgumentNullException.ThrowIfNull(first);
            ArgumentNullException.ThrowIfNull(second);
            ArgumentNullException.ThrowIfNull(third);
            ArgumentNullException.ThrowIfNull(fourth);
            ArgumentNullException.ThrowIfNull(fifth);
            ArgumentNullException.ThrowIfNull(sixth);
            ArgumentNullException.ThrowIfNull(seventh);

            var nobodyIsExecuting = new BehaviorSubject<bool>(false);

            var firstCommand = GetCommandWithCombinedCanExecute(
                nobodyIsExecuting,
                first);

            var secondCommand = GetCommandWithCombinedCanExecute(
                nobodyIsExecuting,
                second);

            var thirdCommand = GetCommandWithCombinedCanExecute(
                nobodyIsExecuting,
                third);

            var fourthCommand = GetCommandWithCombinedCanExecute(
                nobodyIsExecuting,
                fourth);

            var fifthCommand = GetCommandWithCombinedCanExecute(
                nobodyIsExecuting,
                fifth);

            var sixthCommand = GetCommandWithCombinedCanExecute(
                nobodyIsExecuting,
                sixth);

            var seventhCommand = GetCommandWithCombinedCanExecute(
                nobodyIsExecuting,
                seventh);

            var exclusiveLock = firstCommand.IsExecuting.CombineLatest(
                    secondCommand.IsExecuting,
                    thirdCommand.IsExecuting,
                    fourthCommand.IsExecuting,
                    fifthCommand.IsExecuting,
                    sixthCommand.IsExecuting,
                    seventhCommand.IsExecuting)
                .AllFalse()
                .Subscribe(nobodyIsExecuting);

            return (nobodyIsExecuting, exclusiveLock, firstCommand, secondCommand, thirdCommand, fourthCommand, fifthCommand, sixthCommand, seventhCommand);
        }

        /// <summary>
        /// Creates a set of exclusive commands that can be used to ensure that only one command is executing at a time.
        /// </summary>
        /// <typeparam name="T1Param">
        /// For the first command, the type of the parameter passed through to command execution.
        /// </typeparam>
        /// <typeparam name="T1CommandResult">
        /// For the first command, the type of the command's result.
        /// </typeparam>
        /// <typeparam name="T1ExecuteResult">
        /// For the first command, the type of the execute argument. Typically <typeparamref name="T1CommandResult"/> itself or wrapped in <see cref="Task{TResult}"/> or <see cref="IObservable{TResult}"/> depending on the type of command.
        /// </typeparam>
        /// <typeparam name="T2Param">
        /// For the second command, the type of the parameter passed through to command execution.
        /// </typeparam>
        /// <typeparam name="T2CommandResult">
        /// For the second command, the type of the command's result.
        /// </typeparam>
        /// <typeparam name="T2ExecuteResult">
        /// For the second command, the type of the execute argument. Typically <typeparamref name="T1CommandResult"/> itself or wrapped in <see cref="Task{TResult}"/> or <see cref="IObservable{TResult}"/> depending on the type of command.
        /// </typeparam>
        /// <typeparam name="T3Param">
        /// For the third command, the type of the parameter passed through to command execution.
        /// </typeparam>
        /// <typeparam name="T3CommandResult">
        /// For the third command, the type of the command's result.
        /// </typeparam>
        /// <typeparam name="T3ExecuteResult">
        /// For the third command, the type of the execute argument. Typically <typeparamref name="T1CommandResult"/> itself or wrapped in <see cref="Task{TResult}"/> or <see cref="IObservable{TResult}"/> depending on the type of command.
        /// </typeparam>
        /// <typeparam name="T4Param">
        /// For the fourth command, the type of the parameter passed through to command execution.
        /// </typeparam>
        /// <typeparam name="T4CommandResult">
        /// For the fourth command, the type of the command's result.
        /// </typeparam>
        /// <typeparam name="T4ExecuteResult">
        /// For the fourth command, the type of the execute argument. Typically <typeparamref name="T1CommandResult"/> itself or wrapped in <see cref="Task{TResult}"/> or <see cref="IObservable{TResult}"/> depending on the type of command.
        /// </typeparam>
        /// <typeparam name="T5Param">
        /// For the fifth command, the type of the parameter passed through to command execution.
        /// </typeparam>
        /// <typeparam name="T5CommandResult">
        /// For the fifth command, the type of the command's result.
        /// </typeparam>
        /// <typeparam name="T5ExecuteResult">
        /// For the fifth command, the type of the execute argument. Typically <typeparamref name="T1CommandResult"/> itself or wrapped in <see cref="Task{TResult}"/> or <see cref="IObservable{TResult}"/> depending on the type of command.
        /// </typeparam>
        /// <typeparam name="T6Param">
        /// For the sixth command, the type of the parameter passed through to command execution.
        /// </typeparam>
        /// <typeparam name="T6CommandResult">
        /// For the sixth command, the type of the command's result.
        /// </typeparam>
        /// <typeparam name="T6ExecuteResult">
        /// For the sixth command, the type of the execute argument. Typically <typeparamref name="T1CommandResult"/> itself or wrapped in <see cref="Task{TResult}"/> or <see cref="IObservable{TResult}"/> depending on the type of command.
        /// </typeparam>
        /// <typeparam name="T7Param">
        /// For the seventh command, the type of the parameter passed through to command execution.
        /// </typeparam>
        /// <typeparam name="T7CommandResult">
        /// For the seventh command, the type of the command's result.
        /// </typeparam>
        /// <typeparam name="T7ExecuteResult">
        /// For the seventh command, the type of the execute argument. Typically <typeparamref name="T1CommandResult"/> itself or wrapped in <see cref="Task{TResult}"/> or <see cref="IObservable{TResult}"/> depending on the type of command.
        /// </typeparam>
        /// <typeparam name="T8Param">
        /// For the eighth command, the type of the parameter passed through to command execution.
        /// </typeparam>
        /// <typeparam name="T8CommandResult">
        /// For the eighth command, the type of the command's result.
        /// </typeparam>
        /// <typeparam name="T8ExecuteResult">
        /// For the eighth command, the type of the execute argument. Typically <typeparamref name="T1CommandResult"/> itself or wrapped in <see cref="Task{TResult}"/> or <see cref="IObservable{TResult}"/> depending on the type of command.
        /// </typeparam>
        /// <param name="first">Factory arguments for the first command.</param>
        /// <param name="second">Factory arguments for the second command.</param>
        /// <param name="third">Factory arguments for the third command.</param>
        /// <param name="fourth">Factory arguments for the fourth command.</param>
        /// <param name="fifth">Factory arguments for the fifth command.</param>
        /// <param name="sixth">Factory arguments for the sixth command.</param>
        /// <param name="seventh">Factory arguments for the seventh command.</param>
        /// <param name="eighth">Factory arguments for the eighth command.</param>
        /// <returns>A tuple for the mutually exclusive lock, and the relevant commands.</returns>
        public static (
            BehaviorSubject<bool> NobodyIsExecuting,
            IDisposable ExclusiveLock,
            ReactiveCommand<T1Param, T1CommandResult> FirstCommand,
            ReactiveCommand<T2Param, T2CommandResult> SecondCommand,
            ReactiveCommand<T3Param, T3CommandResult> ThirdCommand,
            ReactiveCommand<T4Param, T4CommandResult> FourthCommand,
            ReactiveCommand<T5Param, T5CommandResult> FifthCommand,
            ReactiveCommand<T6Param, T6CommandResult> SixthCommand,
            ReactiveCommand<T7Param, T7CommandResult> SeventhCommand,
            ReactiveCommand<T8Param, T8CommandResult> EighthCommand) Create<
                T1Param,
                T1CommandResult,
                T1ExecuteResult,
                T2Param,
                T2CommandResult,
                T2ExecuteResult,
                T3Param,
                T3CommandResult,
                T3ExecuteResult,
                T4Param,
                T4CommandResult,
                T4ExecuteResult,
                T5Param,
                T5CommandResult,
                T5ExecuteResult,
                T6Param,
                T6CommandResult,
                T6ExecuteResult,
                T7Param,
                T7CommandResult,
                T7ExecuteResult,
                T8Param,
                T8CommandResult,
                T8ExecuteResult>(
                ReactiveCommandFactoryArgument<T1Param, T1CommandResult, T1ExecuteResult> first,
                ReactiveCommandFactoryArgument<T2Param, T2CommandResult, T2ExecuteResult> second,
                ReactiveCommandFactoryArgument<T3Param, T3CommandResult, T3ExecuteResult> third,
                ReactiveCommandFactoryArgument<T4Param, T4CommandResult, T4ExecuteResult> fourth,
                ReactiveCommandFactoryArgument<T5Param, T5CommandResult, T5ExecuteResult> fifth,
                ReactiveCommandFactoryArgument<T6Param, T6CommandResult, T6ExecuteResult> sixth,
                ReactiveCommandFactoryArgument<T7Param, T7CommandResult, T7ExecuteResult> seventh,
                ReactiveCommandFactoryArgument<T8Param, T8CommandResult, T8ExecuteResult> eighth)
        {
            ArgumentNullException.ThrowIfNull(first);
            ArgumentNullException.ThrowIfNull(second);
            ArgumentNullException.ThrowIfNull(third);
            ArgumentNullException.ThrowIfNull(fourth);
            ArgumentNullException.ThrowIfNull(fifth);
            ArgumentNullException.ThrowIfNull(sixth);
            ArgumentNullException.ThrowIfNull(seventh);
            ArgumentNullException.ThrowIfNull(eighth);

            var nobodyIsExecuting = new BehaviorSubject<bool>(false);

            var firstCommand = GetCommandWithCombinedCanExecute(
                nobodyIsExecuting,
                first);

            var secondCommand = GetCommandWithCombinedCanExecute(
                nobodyIsExecuting,
                second);

            var thirdCommand = GetCommandWithCombinedCanExecute(
                nobodyIsExecuting,
                third);

            var fourthCommand = GetCommandWithCombinedCanExecute(
                nobodyIsExecuting,
                fourth);

            var fifthCommand = GetCommandWithCombinedCanExecute(
                nobodyIsExecuting,
                fifth);

            var sixthCommand = GetCommandWithCombinedCanExecute(
                nobodyIsExecuting,
                sixth);

            var seventhCommand = GetCommandWithCombinedCanExecute(
                nobodyIsExecuting,
                seventh);

            var eighthCommand = GetCommandWithCombinedCanExecute(
                nobodyIsExecuting,
                eighth);

            var exclusiveLock = firstCommand.IsExecuting.CombineLatest(
                    secondCommand.IsExecuting,
                    thirdCommand.IsExecuting,
                    fourthCommand.IsExecuting,
                    fifthCommand.IsExecuting,
                    sixthCommand.IsExecuting,
                    seventhCommand.IsExecuting,
                    eighthCommand.IsExecuting)
                .AllFalse()
                .Subscribe(nobodyIsExecuting);

            return (nobodyIsExecuting, exclusiveLock, firstCommand, secondCommand, thirdCommand, fourthCommand, fifthCommand, sixthCommand, seventhCommand, eighthCommand);
        }

        private static ReactiveCommand<TParam, TCommandResult> GetCommandWithCombinedCanExecute<TParam, TCommandResult, TExecuteResult>(
            BehaviorSubject<bool> nobodyIsExecuting,
            ReactiveCommandFactoryArgument<TParam, TCommandResult, TExecuteResult> reactiveCommandFactoryArgument)
        {
            var canExecute = reactiveCommandFactoryArgument.CanExecute != null
                ? reactiveCommandFactoryArgument.CanExecute.CombineLatest(nobodyIsExecuting).AllTrue()
                : nobodyIsExecuting;
            return reactiveCommandFactoryArgument.FactoryFunc(
                reactiveCommandFactoryArgument.Execute,
                canExecute,
                reactiveCommandFactoryArgument.Scheduler);
        }
    }
}
