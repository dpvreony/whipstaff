// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using ReactiveUI;
using Whipstaff.Rx;

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
        /// <summary>
        /// Creates 2 commands that have mutually exclusive execution.
        /// </summary>
        /// <typeparam name="TParam1">The input type for the first command.</typeparam>
        /// <typeparam name="TResult1">The result type for the first command.</typeparam>
        /// <typeparam name="TParam2">The input type for the second command.</typeparam>
        /// <typeparam name="TResult2">The result type for the second command.</typeparam>
        /// <param name="commandFunc1">Function to execute in the first command.</param>
        /// <param name="commandFunc2">Function to execute in the second command.</param>
        /// <returns>A tuple representing the exclusive lock subject and the created commands.</returns>
        public static (
            BehaviorSubject<int> ExclusiveLockSubject,
            IDisposable ExclusiveLockSubscription,
            ReactiveCommand<TParam1, TResult1> Command1,
            ReactiveCommand<TParam2, TResult2> Command2) GetExclusiveCommands<
                TParam1,
                TResult1,
                TParam2,
                TResult2>(
                    Func<TParam1, TResult1> commandFunc1,
                    Func<TParam2, TResult2> commandFunc2)
        {
            ArgumentNullException.ThrowIfNull(commandFunc1);
            ArgumentNullException.ThrowIfNull(commandFunc2);

            var numberExecuting = new BehaviorSubject<int>(0);
            var nobodyIsExecuting = numberExecuting.Select(x => x < 1);

            var (
                command1,
                command2) = CommonExecutionPredicateReactiveCommandFactory.GetCommandsWithCommonExecutionPredicate(
                nobodyIsExecuting,
                commandFunc1,
                commandFunc2);

            var exclusiveLockSubscription = Observable.Merge(
                    command1.IsExecuting,
                    command2.IsExecuting)
                .CountThatAreTrue()
                .Subscribe(numberExecuting);

            return (
                numberExecuting,
                exclusiveLockSubscription,
                command1,
                command2);
        }

        /// <summary>
        /// Creates 2 commands that have mutually exclusive execution.
        /// </summary>
        /// <typeparam name="TParam1">The input type for the first command.</typeparam>
        /// <typeparam name="TResult1">The result type for the first command.</typeparam>
        /// <typeparam name="TParam2">The input type for the second command.</typeparam>
        /// <typeparam name="TResult2">The result type for the second command.</typeparam>
        /// <param name="commandFunc1">Function to execute in the first command.</param>
        /// <param name="commandCanExecute1">Observable indicating whether the first command can execute.</param>
        /// <param name="commandFunc2">Function to execute in the second command.</param>
        /// <param name="commandCanExecute2">Observable indicating whether the second command can execute.</param>
        /// <returns>A tuple representing the exclusive lock subject and the created commands.</returns>
        public static (
            BehaviorSubject<int> ExclusiveLockSubject,
            IDisposable ExclusiveLockSubscription,
            ReactiveCommand<TParam1, TResult1> Command1,
            ReactiveCommand<TParam2, TResult2> Command2) GetExclusiveCommands<
                TParam1,
                TResult1,
                TParam2,
                TResult2>(
                Func<TParam1, TResult1> commandFunc1,
                IObservable<bool> commandCanExecute1,
                Func<TParam2, TResult2> commandFunc2,
                IObservable<bool> commandCanExecute2)
        {
            ArgumentNullException.ThrowIfNull(commandFunc1);
            ArgumentNullException.ThrowIfNull(commandCanExecute1);
            ArgumentNullException.ThrowIfNull(commandFunc2);
            ArgumentNullException.ThrowIfNull(commandCanExecute2);

            var numberExecuting = new BehaviorSubject<int>(0);
            var nobodyIsExecuting = numberExecuting.Select(x => x < 1);

            var cmd1 = ReactiveCommand.Create(
                commandFunc1,
                commandCanExecute1.Merge(nobodyIsExecuting));

            var cmd2 = ReactiveCommand.Create(
                commandFunc2,
                commandCanExecute2.Merge(nobodyIsExecuting));

            var exclusiveLockSubscription = Observable.Merge(
                    cmd1.IsExecuting,
                    cmd2.IsExecuting)
                .CountThatAreTrue()
                .Subscribe(numberExecuting);

            return (
                numberExecuting,
                exclusiveLockSubscription,
                cmd1,
                cmd2);
        }

        /// <summary>
        /// Creates 3 commands that have mutually exclusive execution.
        /// </summary>
        /// <typeparam name="TParam1">The input type for the first command.</typeparam>
        /// <typeparam name="TResult1">The result type for the first command.</typeparam>
        /// <typeparam name="TParam2">The input type for the second command.</typeparam>
        /// <typeparam name="TResult2">The result type for the second command.</typeparam>
        /// <typeparam name="TParam3">The input type for the third command.</typeparam>
        /// <typeparam name="TResult3">The result type for the third command.</typeparam>
        /// <param name="commandFunc1">Function to execute in the first command.</param>
        /// <param name="commandFunc2">Function to execute in the second command.</param>
        /// <param name="commandFunc3">Function to execute in the third command.</param>
        /// <returns>A tuple representing the exclusive lock subject and the created commands.</returns>
        public static (
            BehaviorSubject<int> ExclusiveLockSubject,
            IDisposable ExclusiveLockSubscription,
            ReactiveCommand<TParam1, TResult1> Command1,
            ReactiveCommand<TParam2, TResult2> Command2,
            ReactiveCommand<TParam3, TResult3> Command3) GetExclusiveCommands<
                TParam1,
                TResult1,
                TParam2,
                TResult2,
                TParam3,
                TResult3>(
                Func<TParam1, TResult1> commandFunc1,
                Func<TParam2, TResult2> commandFunc2,
                Func<TParam3, TResult3> commandFunc3)
        {
            ArgumentNullException.ThrowIfNull(commandFunc1);
            ArgumentNullException.ThrowIfNull(commandFunc2);
            ArgumentNullException.ThrowIfNull(commandFunc3);

            var numberExecuting = new BehaviorSubject<int>(0);
            var nobodyIsExecuting = numberExecuting.Select(x => x < 1);

            var (
                command1,
                command2,
                command3) = CommonExecutionPredicateReactiveCommandFactory.GetCommandsWithCommonExecutionPredicate(
                nobodyIsExecuting,
                commandFunc1,
                commandFunc2,
                commandFunc3);

            var exclusiveLockSubscription = Observable.Merge(
                    command1.IsExecuting,
                    command2.IsExecuting,
                    command3.IsExecuting)
                .CountThatAreTrue()
                .Subscribe(numberExecuting);

            return (
                numberExecuting,
                exclusiveLockSubscription,
                command1,
                command2,
                command3);
        }

        /// <summary>
        /// Creates 4 commands that have mutually exclusive execution.
        /// </summary>
        /// <typeparam name="TParam1">The input type for the first command.</typeparam>
        /// <typeparam name="TResult1">The result type for the first command.</typeparam>
        /// <typeparam name="TParam2">The input type for the second command.</typeparam>
        /// <typeparam name="TResult2">The result type for the second command.</typeparam>
        /// <typeparam name="TParam3">The input type for the third command.</typeparam>
        /// <typeparam name="TResult3">The result type for the third command.</typeparam>
        /// <typeparam name="TParam4">The input type for the fourth command.</typeparam>
        /// <typeparam name="TResult4">The result type for the fourth command.</typeparam>
        /// <param name="commandFunc1">Function to execute in the first command.</param>
        /// <param name="commandFunc2">Function to execute in the second command.</param>
        /// <param name="commandFunc3">Function to execute in the third command.</param>
        /// <param name="commandFunc4">Function to execute in the fourth command.</param>
        /// <returns>A tuple representing the exclusive lock subject and the created commands.</returns>
        public static (
            BehaviorSubject<int> ExclusiveLockSubject,
            IDisposable ExclusiveLockSubscription,
            ReactiveCommand<TParam1, TResult1> Command1,
            ReactiveCommand<TParam2, TResult2> Command2,
            ReactiveCommand<TParam3, TResult3> Command3,
            ReactiveCommand<TParam4, TResult4> Command4) GetExclusiveCommands<
                TParam1,
                TResult1,
                TParam2,
                TResult2,
                TParam3,
                TResult3,
                TParam4,
                TResult4>(
                Func<TParam1, TResult1> commandFunc1,
                Func<TParam2, TResult2> commandFunc2,
                Func<TParam3, TResult3> commandFunc3,
                Func<TParam4, TResult4> commandFunc4)
        {
            ArgumentNullException.ThrowIfNull(commandFunc1);
            ArgumentNullException.ThrowIfNull(commandFunc2);
            ArgumentNullException.ThrowIfNull(commandFunc3);
            ArgumentNullException.ThrowIfNull(commandFunc4);

            var numberExecuting = new BehaviorSubject<int>(0);
            var nobodyIsExecuting = numberExecuting.Select(x => x < 1);

            var (
                command1,
                command2,
                command3,
                command4) = CommonExecutionPredicateReactiveCommandFactory.GetCommandsWithCommonExecutionPredicate(
                nobodyIsExecuting,
                commandFunc1,
                commandFunc2,
                commandFunc3,
                commandFunc4);

            var exclusiveLockSubscription = Observable.Merge(
                    command1.IsExecuting,
                    command2.IsExecuting,
                    command3.IsExecuting,
                    command4.IsExecuting)
                .CountThatAreTrue()
                .Subscribe(numberExecuting);

            return (
                numberExecuting,
                exclusiveLockSubscription,
                command1,
                command2,
                command3,
                command4);
        }

        /// <summary>
        /// Creates 5 commands that have mutually exclusive execution.
        /// </summary>
        /// <typeparam name="TParam1">The input type for the first command.</typeparam>
        /// <typeparam name="TResult1">The result type for the first command.</typeparam>
        /// <typeparam name="TParam2">The input type for the second command.</typeparam>
        /// <typeparam name="TResult2">The result type for the second command.</typeparam>
        /// <typeparam name="TParam3">The input type for the third command.</typeparam>
        /// <typeparam name="TResult3">The result type for the third command.</typeparam>
        /// <typeparam name="TParam4">The input type for the fourth command.</typeparam>
        /// <typeparam name="TResult4">The result type for the fourth command.</typeparam>
        /// <typeparam name="TParam5">The input type for the fifth command.</typeparam>
        /// <typeparam name="TResult5">The result type for the fifth command.</typeparam>
        /// <param name="commandFunc1">Function to execute in the first command.</param>
        /// <param name="commandFunc2">Function to execute in the second command.</param>
        /// <param name="commandFunc3">Function to execute in the third command.</param>
        /// <param name="commandFunc4">Function to execute in the fourth command.</param>
        /// <param name="commandFunc5">Function to execute in the fifth command.</param>
        /// <returns>A tuple representing the exclusive lock subject and the created commands.</returns>
        public static (
            BehaviorSubject<int> ExclusiveLockSubject,
            IDisposable ExclusiveLockSubscription,
            ReactiveCommand<TParam1, TResult1> Command1,
            ReactiveCommand<TParam2, TResult2> Command2,
            ReactiveCommand<TParam3, TResult3> Command3,
            ReactiveCommand<TParam4, TResult4> Command4,
            ReactiveCommand<TParam5, TResult5> Command5) GetExclusiveCommands<
                TParam1,
                TResult1,
                TParam2,
                TResult2,
                TParam3,
                TResult3,
                TParam4,
                TResult4,
                TParam5,
                TResult5>(
                Func<TParam1, TResult1> commandFunc1,
                Func<TParam2, TResult2> commandFunc2,
                Func<TParam3, TResult3> commandFunc3,
                Func<TParam4, TResult4> commandFunc4,
                Func<TParam5, TResult5> commandFunc5)
        {
            ArgumentNullException.ThrowIfNull(commandFunc1);
            ArgumentNullException.ThrowIfNull(commandFunc2);
            ArgumentNullException.ThrowIfNull(commandFunc3);
            ArgumentNullException.ThrowIfNull(commandFunc4);
            ArgumentNullException.ThrowIfNull(commandFunc5);

            var numberExecuting = new BehaviorSubject<int>(0);
            var nobodyIsExecuting = numberExecuting.Select(x => x < 1);

            var (
                command1,
                command2,
                command3,
                command4,
                command5) = CommonExecutionPredicateReactiveCommandFactory.GetCommandsWithCommonExecutionPredicate(
                nobodyIsExecuting,
                commandFunc1,
                commandFunc2,
                commandFunc3,
                commandFunc4,
                commandFunc5);

            var exclusiveLockSubscription = Observable.Merge(
                    command1.IsExecuting,
                    command2.IsExecuting,
                    command3.IsExecuting,
                    command4.IsExecuting,
                    command5.IsExecuting)
                .CountThatAreTrue()
                .Subscribe(numberExecuting);

            return (
                numberExecuting,
                exclusiveLockSubscription,
                command1,
                command2,
                command3,
                command4,
                command5);
        }

        /// <summary>
        /// Creates 6 commands that have mutually exclusive execution.
        /// </summary>
        /// <typeparam name="TParam1">The input type for the first command.</typeparam>
        /// <typeparam name="TResult1">The result type for the first command.</typeparam>
        /// <typeparam name="TParam2">The input type for the second command.</typeparam>
        /// <typeparam name="TResult2">The result type for the second command.</typeparam>
        /// <typeparam name="TParam3">The input type for the third command.</typeparam>
        /// <typeparam name="TResult3">The result type for the third command.</typeparam>
        /// <typeparam name="TParam4">The input type for the fourth command.</typeparam>
        /// <typeparam name="TResult4">The result type for the fourth command.</typeparam>
        /// <typeparam name="TParam5">The input type for the fifth command.</typeparam>
        /// <typeparam name="TResult5">The result type for the fifth command.</typeparam>
        /// <typeparam name="TParam6">The input type for the sixth command.</typeparam>
        /// <typeparam name="TResult6">The result type for the sixth command.</typeparam>
        /// <param name="commandFunc1">Function to execute in the first command.</param>
        /// <param name="commandFunc2">Function to execute in the second command.</param>
        /// <param name="commandFunc3">Function to execute in the third command.</param>
        /// <param name="commandFunc4">Function to execute in the fourth command.</param>
        /// <param name="commandFunc5">Function to execute in the fifth command.</param>
        /// <param name="commandFunc6">Function to execute in the sixth command.</param>
        /// <returns>A tuple representing the exclusive lock subject and the created commands.</returns>
        public static (
            BehaviorSubject<int> ExclusiveLockSubject,
            IDisposable ExclusiveLockSubscription,
            ReactiveCommand<TParam1, TResult1> Command1,
            ReactiveCommand<TParam2, TResult2> Command2,
            ReactiveCommand<TParam3, TResult3> Command3,
            ReactiveCommand<TParam4, TResult4> Command4,
            ReactiveCommand<TParam5, TResult5> Command5,
            ReactiveCommand<TParam6, TResult6> Command6) GetExclusiveCommands<
                TParam1,
                TResult1,
                TParam2,
                TResult2,
                TParam3,
                TResult3,
                TParam4,
                TResult4,
                TParam5,
                TResult5,
                TParam6,
                TResult6>(
                Func<TParam1, TResult1> commandFunc1,
                Func<TParam2, TResult2> commandFunc2,
                Func<TParam3, TResult3> commandFunc3,
                Func<TParam4, TResult4> commandFunc4,
                Func<TParam5, TResult5> commandFunc5,
                Func<TParam6, TResult6> commandFunc6)
        {
            ArgumentNullException.ThrowIfNull(commandFunc1);
            ArgumentNullException.ThrowIfNull(commandFunc2);
            ArgumentNullException.ThrowIfNull(commandFunc3);
            ArgumentNullException.ThrowIfNull(commandFunc4);
            ArgumentNullException.ThrowIfNull(commandFunc5);
            ArgumentNullException.ThrowIfNull(commandFunc6);

            var numberExecuting = new BehaviorSubject<int>(0);
            var nobodyIsExecuting = numberExecuting.Select(x => x < 1);

            var (
                command1,
                command2,
                command3,
                command4,
                command5,
                command6) = CommonExecutionPredicateReactiveCommandFactory.GetCommandsWithCommonExecutionPredicate(
                nobodyIsExecuting,
                commandFunc1,
                commandFunc2,
                commandFunc3,
                commandFunc4,
                commandFunc5,
                commandFunc6);

            var exclusiveLockSubscription = Observable.Merge(
                    command1.IsExecuting,
                    command2.IsExecuting,
                    command3.IsExecuting,
                    command4.IsExecuting,
                    command5.IsExecuting,
                    command6.IsExecuting)
                .CountThatAreTrue()
                .Subscribe(numberExecuting);

            return (
                numberExecuting,
                exclusiveLockSubscription,
                command1,
                command2,
                command3,
                command4,
                command5,
                command6);
        }
    }
}
