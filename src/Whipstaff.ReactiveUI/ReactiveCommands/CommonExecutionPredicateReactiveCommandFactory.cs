// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive.Linq;
using ReactiveUI;

#if ARGUMENT_NULL_EXCEPTION_SHIM 
using ArgumentNullException = Whipstaff.Runtime.Exceptions.ArgumentNullException;
#endif

namespace Whipstaff.ReactiveUI.ReactiveCommands
{
    /// <summary>
    /// Factory for producing Reactive Commands that utilize a common predicate for controlling execution.
    /// </summary>
    public static class CommonExecutionPredicateReactiveCommandFactory
    {
        /// <summary>
        /// Creates 2 commands that have a common execution predicate.
        /// </summary>
        /// <remarks>
        /// A common example for this could be buttons that can't execute if an upstream is executing, or a client is offline, etc.
        /// </remarks>
        /// <typeparam name="TParam1">The input type for the first command.</typeparam>
        /// <typeparam name="TResult1">The result type for the first command.</typeparam>
        /// <typeparam name="TParam2">The input type for the second command.</typeparam>
        /// <typeparam name="TResult2">The result type for the second command.</typeparam>
        /// <param name="canExecuteObservable">Predicate observable for if the commands can execute.</param>
        /// <param name="commandFunc1">Function to execute in the first command.</param>
        /// <param name="commandFunc2">Function to execute in the second command.</param>
        /// <returns>A tuple representing the created commands.</returns>
        public static (
            ReactiveCommand<TParam1, TResult1> Command1,
            ReactiveCommand<TParam2, TResult2> Command2) GetCommandsWithCommonExecutionPredicate<
                TParam1,
                TResult1,
                TParam2,
                TResult2>(
                IObservable<bool> canExecuteObservable,
                Func<TParam1, TResult1> commandFunc1,
                Func<TParam2, TResult2> commandFunc2)
        {
            ArgumentNullException.ThrowIfNull(canExecuteObservable);
            ArgumentNullException.ThrowIfNull(commandFunc1);
            ArgumentNullException.ThrowIfNull(commandFunc2);

            var cmd1 = ReactiveCommand.Create(
                commandFunc1,
                canExecuteObservable);

            var cmd2 = ReactiveCommand.Create(
                commandFunc2,
                canExecuteObservable);

            return (
                cmd1,
                cmd2);
        }

        /// <summary>
        /// Creates 2 commands that have a common execution predicate, as well as their own predicate.
        /// </summary>
        /// <remarks>
        /// A common example for this could be buttons that can't execute if an upstream is executing, or a client is offline, etc.
        /// </remarks>
        /// <typeparam name="TParam1">The input type for the first command.</typeparam>
        /// <typeparam name="TResult1">The result type for the first command.</typeparam>
        /// <typeparam name="TParam2">The input type for the second command.</typeparam>
        /// <typeparam name="TResult2">The result type for the second command.</typeparam>
        /// <param name="canExecuteObservable">Predicate observable for if the commands can execute.</param>
        /// <param name="commandFunc1">Function to execute in the first command.</param>
        /// <param name="commandCanExecute1">Observable for whether the first command can execute. Is merged with canExecuteObservable.</param>
        /// <param name="commandFunc2">Function to execute in the second command.</param>
        /// <param name="commandCanExecute2">Observable for whether the second command can execute. Is merged with canExecuteObservable.</param>
        /// <returns>A tuple representing the created commands.</returns>
        public static (
            ReactiveCommand<TParam1, TResult1> Command1,
            ReactiveCommand<TParam2, TResult2> Command2) GetCommandsWithCommonExecutionPredicate<
                TParam1,
                TResult1,
                TParam2,
                TResult2>(
                IObservable<bool> canExecuteObservable,
                Func<TParam1, TResult1> commandFunc1,
                IObservable<bool> commandCanExecute1,
                Func<TParam2, TResult2> commandFunc2,
                IObservable<bool> commandCanExecute2)
        {
            ArgumentNullException.ThrowIfNull(canExecuteObservable);
            ArgumentNullException.ThrowIfNull(commandFunc1);
            ArgumentNullException.ThrowIfNull(commandCanExecute1);
            ArgumentNullException.ThrowIfNull(commandFunc2);
            ArgumentNullException.ThrowIfNull(commandCanExecute2);

            var cmd1 = ReactiveCommand.Create(
                commandFunc1,
                commandCanExecute1.Merge(canExecuteObservable));

            var cmd2 = ReactiveCommand.Create(
                commandFunc2,
                commandCanExecute2.Merge(canExecuteObservable));

            return (
                cmd1,
                cmd2);
        }

        /// <summary>
        /// Creates 3 commands that have a common execution predicate.
        /// </summary>
        /// <remarks>
        /// A common example for this could be buttons that can't execute if an upstream is executing, or a client is offline, etc.
        /// </remarks>
        /// <typeparam name="TParam1">The input type for the first command.</typeparam>
        /// <typeparam name="TResult1">The result type for the first command.</typeparam>
        /// <typeparam name="TParam2">The input type for the second command.</typeparam>
        /// <typeparam name="TResult2">The result type for the second command.</typeparam>
        /// <typeparam name="TParam3">The input type for the third command.</typeparam>
        /// <typeparam name="TResult3">The result type for the third command.</typeparam>
        /// <param name="canExecuteObservable">Predicate observable for if the commands can execute.</param>
        /// <param name="commandFunc1">Function to execute in the first command.</param>
        /// <param name="commandFunc2">Function to execute in the second command.</param>
        /// <param name="commandFunc3">Function to execute in the third command.</param>
        /// <returns>A tuple representing the created commands.</returns>
        public static (
            ReactiveCommand<TParam1, TResult1> Command1,
            ReactiveCommand<TParam2, TResult2> Command2,
            ReactiveCommand<TParam3, TResult3> Command3) GetCommandsWithCommonExecutionPredicate<
                TParam1,
                TResult1,
                TParam2,
                TResult2,
                TParam3,
                TResult3>(
                IObservable<bool> canExecuteObservable,
                Func<TParam1, TResult1> commandFunc1,
                Func<TParam2, TResult2> commandFunc2,
                Func<TParam3, TResult3> commandFunc3)
        {
            ArgumentNullException.ThrowIfNull(canExecuteObservable);
            ArgumentNullException.ThrowIfNull(commandFunc1);
            ArgumentNullException.ThrowIfNull(commandFunc2);
            ArgumentNullException.ThrowIfNull(commandFunc3);

            var cmd1 = ReactiveCommand.Create(
                commandFunc1,
                canExecuteObservable);

            var cmd2 = ReactiveCommand.Create(
                commandFunc2,
                canExecuteObservable);

            var cmd3 = ReactiveCommand.Create(
                commandFunc3,
                canExecuteObservable);

            return (
                cmd1,
                cmd2,
                cmd3);
        }

        /// <summary>
        /// Creates 4 commands that have a common execution predicate.
        /// </summary>
        /// <remarks>
        /// A common example for this could be buttons that can't execute if an upstream is executing, or a client is offline, etc.
        /// </remarks>
        /// <typeparam name="TParam1">The input type for the first command.</typeparam>
        /// <typeparam name="TResult1">The result type for the first command.</typeparam>
        /// <typeparam name="TParam2">The input type for the second command.</typeparam>
        /// <typeparam name="TResult2">The result type for the second command.</typeparam>
        /// <typeparam name="TParam3">The input type for the third command.</typeparam>
        /// <typeparam name="TResult3">The result type for the third command.</typeparam>
        /// <typeparam name="TParam4">The input type for the fourth command.</typeparam>
        /// <typeparam name="TResult4">The result type for the fourth command.</typeparam>
        /// <param name="canExecuteObservable">Predicate observable for if the commands can execute.</param>
        /// <param name="commandFunc1">Function to execute in the first command.</param>
        /// <param name="commandFunc2">Function to execute in the second command.</param>
        /// <param name="commandFunc3">Function to execute in the third command.</param>
        /// <param name="commandFunc4">Function to execute in the fourth command.</param>
        /// <returns>A tuple representing the created commands.</returns>
        public static (
            ReactiveCommand<TParam1, TResult1> Command1,
            ReactiveCommand<TParam2, TResult2> Command2,
            ReactiveCommand<TParam3, TResult3> Command3,
            ReactiveCommand<TParam4, TResult4> Command4) GetCommandsWithCommonExecutionPredicate<
                TParam1,
                TResult1,
                TParam2,
                TResult2,
                TParam3,
                TResult3,
                TParam4,
                TResult4>(
                IObservable<bool> canExecuteObservable,
                Func<TParam1, TResult1> commandFunc1,
                Func<TParam2, TResult2> commandFunc2,
                Func<TParam3, TResult3> commandFunc3,
                Func<TParam4, TResult4> commandFunc4)
        {
            ArgumentNullException.ThrowIfNull(canExecuteObservable);
            ArgumentNullException.ThrowIfNull(commandFunc1);
            ArgumentNullException.ThrowIfNull(commandFunc2);
            ArgumentNullException.ThrowIfNull(commandFunc3);
            ArgumentNullException.ThrowIfNull(commandFunc4);

            var cmd1 = ReactiveCommand.Create(
                commandFunc1,
                canExecuteObservable);

            var cmd2 = ReactiveCommand.Create(
                commandFunc2,
                canExecuteObservable);

            var cmd3 = ReactiveCommand.Create(
                commandFunc3,
                canExecuteObservable);

            var cmd4 = ReactiveCommand.Create(
                commandFunc4,
                canExecuteObservable);

            return (
                cmd1,
                cmd2,
                cmd3,
                cmd4);
        }

        /// <summary>
        /// Creates 5 commands that have a common execution predicate.
        /// </summary>
        /// <remarks>
        /// A common example for this could be buttons that can't execute if an upstream is executing, or a client is offline, etc.
        /// </remarks>
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
        /// <param name="canExecuteObservable">Predicate observable for if the commands can execute.</param>
        /// <param name="commandFunc1">Function to execute in the first command.</param>
        /// <param name="commandFunc2">Function to execute in the second command.</param>
        /// <param name="commandFunc3">Function to execute in the third command.</param>
        /// <param name="commandFunc4">Function to execute in the fourth command.</param>
        /// <param name="commandFunc5">Function to execute in the fifth command.</param>
        /// <returns>A tuple representing the created commands.</returns>
        public static (
            ReactiveCommand<TParam1, TResult1> Command1,
            ReactiveCommand<TParam2, TResult2> Command2,
            ReactiveCommand<TParam3, TResult3> Command3,
            ReactiveCommand<TParam4, TResult4> Command4,
            ReactiveCommand<TParam5, TResult5> Command5) GetCommandsWithCommonExecutionPredicate<
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
                IObservable<bool> canExecuteObservable,
                Func<TParam1, TResult1> commandFunc1,
                Func<TParam2, TResult2> commandFunc2,
                Func<TParam3, TResult3> commandFunc3,
                Func<TParam4, TResult4> commandFunc4,
                Func<TParam5, TResult5> commandFunc5)
        {
            ArgumentNullException.ThrowIfNull(canExecuteObservable);
            ArgumentNullException.ThrowIfNull(commandFunc1);
            ArgumentNullException.ThrowIfNull(commandFunc2);
            ArgumentNullException.ThrowIfNull(commandFunc3);
            ArgumentNullException.ThrowIfNull(commandFunc4);
            ArgumentNullException.ThrowIfNull(commandFunc5);

            var cmd1 = ReactiveCommand.Create(
                commandFunc1,
                canExecuteObservable);

            var cmd2 = ReactiveCommand.Create(
                commandFunc2,
                canExecuteObservable);

            var cmd3 = ReactiveCommand.Create(
                commandFunc3,
                canExecuteObservable);

            var cmd4 = ReactiveCommand.Create(
                commandFunc4,
                canExecuteObservable);

            var cmd5 = ReactiveCommand.Create(
                commandFunc5,
                canExecuteObservable);

            return (
                cmd1,
                cmd2,
                cmd3,
                cmd4,
                cmd5);
        }

        /// <summary>
        /// Creates 6 commands that have a common execution predicate.
        /// </summary>
        /// <remarks>
        /// A common example for this could be buttons that can't execute if an upstream is executing, or a client is offline, etc.
        /// </remarks>
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
        /// <param name="canExecuteObservable">Predicate observable for if the commands can execute.</param>
        /// <param name="commandFunc1">Function to execute in the first command.</param>
        /// <param name="commandFunc2">Function to execute in the second command.</param>
        /// <param name="commandFunc3">Function to execute in the third command.</param>
        /// <param name="commandFunc4">Function to execute in the fourth command.</param>
        /// <param name="commandFunc5">Function to execute in the fifth command.</param>
        /// <param name="commandFunc6">Function to execute in the sixth command.</param>
        /// <returns>A tuple representing the created commands.</returns>
        public static (
            ReactiveCommand<TParam1, TResult1> Command1,
            ReactiveCommand<TParam2, TResult2> Command2,
            ReactiveCommand<TParam3, TResult3> Command3,
            ReactiveCommand<TParam4, TResult4> Command4,
            ReactiveCommand<TParam5, TResult5> Command5,
            ReactiveCommand<TParam6, TResult6> Command6) GetCommandsWithCommonExecutionPredicate<
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
                IObservable<bool> canExecuteObservable,
                Func<TParam1, TResult1> commandFunc1,
                Func<TParam2, TResult2> commandFunc2,
                Func<TParam3, TResult3> commandFunc3,
                Func<TParam4, TResult4> commandFunc4,
                Func<TParam5, TResult5> commandFunc5,
                Func<TParam6, TResult6> commandFunc6)
        {
            ArgumentNullException.ThrowIfNull(canExecuteObservable);
            ArgumentNullException.ThrowIfNull(commandFunc1);
            ArgumentNullException.ThrowIfNull(commandFunc2);
            ArgumentNullException.ThrowIfNull(commandFunc3);
            ArgumentNullException.ThrowIfNull(commandFunc4);
            ArgumentNullException.ThrowIfNull(commandFunc5);
            ArgumentNullException.ThrowIfNull(commandFunc6);

            var cmd1 = ReactiveCommand.Create(
                commandFunc1,
                canExecuteObservable);

            var cmd2 = ReactiveCommand.Create(
                commandFunc2,
                canExecuteObservable);

            var cmd3 = ReactiveCommand.Create(
                commandFunc3,
                canExecuteObservable);

            var cmd4 = ReactiveCommand.Create(
                commandFunc4,
                canExecuteObservable);

            var cmd5 = ReactiveCommand.Create(
                commandFunc5,
                canExecuteObservable);

            var cmd6 = ReactiveCommand.Create(
                commandFunc6,
                canExecuteObservable);

            return (
                cmd1,
                cmd2,
                cmd3,
                cmd4,
                cmd5,
                cmd6);
        }
    }
}
