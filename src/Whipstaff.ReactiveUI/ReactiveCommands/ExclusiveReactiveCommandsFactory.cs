// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
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
    public static class ExclusiveReactiveCommandsFactory
    {
        public static (
            BehaviorSubject<bool> NobodyIsExecuting,
            IDisposable ExclusiveLock,
            ReactiveCommand<T1Param, T1CommandResult> FirstCommand,
            ReactiveCommand<T2Param, T2CommandResult> SecondCommand) GetExclusiveCommands<
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

        public static (
            BehaviorSubject<bool> NobodyIsExecuting,
            IDisposable ExclusiveLock,
            ReactiveCommand<T1Param, T1CommandResult> FirstCommand,
            ReactiveCommand<T2Param, T2CommandResult> SecondCommand,
            ReactiveCommand<T3Param, T3CommandResult> ThirdCommand) GetExclusiveCommands<
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

            var exclusiveLock = firstCommand.IsExecuting.CombineLatest(secondCommand.IsExecuting)
                .CombineLatest(thirdCommand.IsExecuting)
                .FlattenCombination()
                .AllFalse()
                .Subscribe(nobodyIsExecuting);

            return (nobodyIsExecuting, exclusiveLock, firstCommand, secondCommand, thirdCommand);
        }

        public static (
            BehaviorSubject<bool> NobodyIsExecuting,
            IDisposable ExclusiveLock,
            ReactiveCommand<T1Param, T1CommandResult> FirstCommand,
            ReactiveCommand<T2Param, T2CommandResult> SecondCommand,
            ReactiveCommand<T3Param, T3CommandResult> ThirdCommand,
            ReactiveCommand<T4Param, T4CommandResult> FourthCommand) GetExclusiveCommands<
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

            var exclusiveLock = firstCommand.IsExecuting.CombineLatest(secondCommand.IsExecuting)
                .CombineLatest(thirdCommand.IsExecuting)
                .CombineLatest(fourthCommand.IsExecuting)
                .FlattenCombination()
                .AllFalse()
                .Subscribe(nobodyIsExecuting);

            return (nobodyIsExecuting, exclusiveLock, firstCommand, secondCommand, thirdCommand, fourthCommand);
        }

        public static (
            BehaviorSubject<bool> NobodyIsExecuting,
            IDisposable ExclusiveLock,
            ReactiveCommand<T1Param, T1CommandResult> FirstCommand,
            ReactiveCommand<T2Param, T2CommandResult> SecondCommand,
            ReactiveCommand<T3Param, T3CommandResult> ThirdCommand,
            ReactiveCommand<T4Param, T4CommandResult> FourthCommand,
            ReactiveCommand<T5Param, T5CommandResult> FifthCommand) GetExclusiveCommands<
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

            var exclusiveLock = firstCommand.IsExecuting.CombineLatest(secondCommand.IsExecuting)
                .CombineLatest(thirdCommand.IsExecuting)
                .CombineLatest(fourthCommand.IsExecuting)
                .CombineLatest(fifthCommand.IsExecuting)
                .FlattenCombination()
                .AllFalse()
                .Subscribe(nobodyIsExecuting);

            return (nobodyIsExecuting, exclusiveLock, firstCommand, secondCommand, thirdCommand, fourthCommand, fifthCommand);
        }

        public static (
            BehaviorSubject<bool> NobodyIsExecuting,
            IDisposable ExclusiveLock,
            ReactiveCommand<T1Param, T1CommandResult> FirstCommand,
            ReactiveCommand<T2Param, T2CommandResult> SecondCommand,
            ReactiveCommand<T3Param, T3CommandResult> ThirdCommand,
            ReactiveCommand<T4Param, T4CommandResult> FourthCommand,
            ReactiveCommand<T5Param, T5CommandResult> FifthCommand,
            ReactiveCommand<T6Param, T6CommandResult> SixthCommand) GetExclusiveCommands<
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

            var exclusiveLock = firstCommand.IsExecuting.CombineLatest(secondCommand.IsExecuting)
                .CombineLatest(thirdCommand.IsExecuting)
                .CombineLatest(fourthCommand.IsExecuting)
                .CombineLatest(fifthCommand.IsExecuting)
                .CombineLatest(sixthCommand.IsExecuting)
                .FlattenCombination()
                .AllFalse()
                .Subscribe(nobodyIsExecuting);

            return (nobodyIsExecuting, exclusiveLock, firstCommand, secondCommand, thirdCommand, fourthCommand, fifthCommand, sixthCommand);
        }

        public static (
            BehaviorSubject<bool> NobodyIsExecuting,
            IDisposable ExclusiveLock,
            ReactiveCommand<T1Param, T1CommandResult> FirstCommand,
            ReactiveCommand<T2Param, T2CommandResult> SecondCommand,
            ReactiveCommand<T3Param, T3CommandResult> ThirdCommand,
            ReactiveCommand<T4Param, T4CommandResult> FourthCommand,
            ReactiveCommand<T5Param, T5CommandResult> FifthCommand,
            ReactiveCommand<T6Param, T6CommandResult> SixthCommand,
            ReactiveCommand<T7Param, T7CommandResult> SeventhCommand) GetExclusiveCommands<
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

            var exclusiveLock = firstCommand.IsExecuting.CombineLatest(secondCommand.IsExecuting)
                .CombineLatest(thirdCommand.IsExecuting)
                .CombineLatest(fourthCommand.IsExecuting)
                .CombineLatest(fifthCommand.IsExecuting)
                .CombineLatest(sixthCommand.IsExecuting)
                .CombineLatest(seventhCommand.IsExecuting)
                .FlattenCombination()
                .AllFalse()
                .Subscribe(nobodyIsExecuting);

            return (nobodyIsExecuting, exclusiveLock, firstCommand, secondCommand, thirdCommand, fourthCommand, fifthCommand, sixthCommand, seventhCommand);
        }

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
            ReactiveCommand<T8Param, T8CommandResult> EighthCommand) GetExclusiveCommands<
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
