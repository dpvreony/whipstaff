// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Whipstaff.Core.Logging;

#if ARGUMENT_NULL_EXCEPTION_SHIM
using ArgumentNullException = Whipstaff.Runtime.Exceptions.ArgumentNullException;
#endif

namespace Whipstaff.Core.ExceptionHandling
{
    /// <summary>
    /// Helpers for managing task execution flows.
    /// </summary>
    public static class TaskHelpers
    {
        private static readonly Action<ILogger, Exception?> _defaultingResultDueToExceptionLogMessageAction =
            LoggerMessage.Define(
                LogLevel.Warning,
                WhipstaffEventIdFactory.DefaultIfException(),
                "Defaulting Result due to exception");

        /// <summary>
        /// Encapsulates a method that has no parameters, and in case of any exception it catches it and returns a default value.
        /// </summary>
        /// <typeparam name="TResult">The type of the return value of the method that this delegate encapsulates.</typeparam>
        /// <param name="func">The method to encapsulate.</param>
        /// <param name="defaultResult">The default value to return if an exception is thrown.</param>
        /// <param name="logger">Logging framework instance.</param>
        /// <returns>The return value of the method that this delegate encapsulates.</returns>
        public static async Task<TResult> DefaultIfExceptionAsync<TResult>(
            this Func<Task<TResult>> func,
            TResult defaultResult,
            ILogger? logger = null)
        {
            ArgumentNullException.ThrowIfNull(func);

            try
            {
                return await func().ConfigureAwait(false);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                if (logger != null)
                {
                    _defaultingResultDueToExceptionLogMessageAction(logger, e);
                }

                return defaultResult;
            }
        }

        /// <summary>
        /// Encapsulates a method that has 1 parameters, and in case of any exception it catches it and returns a default value.
        /// </summary>
        /// <typeparam name="T">The type of the parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the method that this delegate encapsulates.</typeparam>
        /// <param name="func">The method to encapsulate.</param>
        /// <param name="arg">The parameter the encapsulated method takes.</param>
        /// <param name="defaultResult">The default value to return if an exception is thrown.</param>
        /// <param name="logger">Logging framework instance.</param>
        /// <returns>The return value of the method that this delegate encapsulates.</returns>
        public static async Task<TResult> DefaultIfExceptionAsync<T, TResult>(
            this Func<T, Task<TResult>> func,
            T arg,
            TResult defaultResult,
            ILogger? logger = null)
        {
            ArgumentNullException.ThrowIfNull(func);

            try
            {
                return await func(arg).ConfigureAwait(false);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                if (logger != null)
                {
                    _defaultingResultDueToExceptionLogMessageAction(logger, e);
                }

                return defaultResult;
            }
        }

        /// <summary>
        /// Encapsulates a method that has 2 parameters, and in case of any exception it catches it and returns a default value.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T2">The type of the second parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the method that this delegate encapsulates.</typeparam>
        /// <param name="func">The method to encapsulate.</param>
        /// <param name="arg1">The first parameter the encapsulated method takes.</param>
        /// <param name="arg2">The second parameter the encapsulated method takes.</param>
        /// <param name="defaultResult">The default value to return if an exception is thrown.</param>
        /// <param name="logger">Logging framework instance.</param>
        /// <returns>The return value of the method that this delegate encapsulates.</returns>
        public static async Task<TResult> DefaultIfExceptionAsync<T1, T2, TResult>(
            this Func<T1, T2, Task<TResult>> func,
            T1 arg1,
            T2 arg2,
            TResult defaultResult,
            ILogger? logger = null)
        {
            ArgumentNullException.ThrowIfNull(func);

            try
            {
                return await func(
                    arg1,
                    arg2).ConfigureAwait(false);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                if (logger != null)
                {
                    _defaultingResultDueToExceptionLogMessageAction(logger, e);
                }

                return defaultResult;
            }
        }

        /// <summary>
        /// Encapsulates a method that has 3 parameters, and in case of any exception it catches it and returns a default value.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T2">The type of the second parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T3">The type of the third parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the method that this delegate encapsulates.</typeparam>
        /// <param name="func">The method to encapsulate.</param>
        /// <param name="arg1">The first parameter the encapsulated method takes.</param>
        /// <param name="arg2">The second parameter the encapsulated method takes.</param>
        /// <param name="arg3">The third parameter the encapsulated method takes.</param>
        /// <param name="defaultResult">The default value to return if an exception is thrown.</param>
        /// <param name="logger">Logging framework instance.</param>
        /// <returns>The return value of the method that this delegate encapsulates.</returns>
        public static async Task<TResult> DefaultIfExceptionAsync<T1, T2, T3, TResult>(
            this Func<T1, T2, T3, Task<TResult>> func,
            T1 arg1,
            T2 arg2,
            T3 arg3,
            TResult defaultResult,
            ILogger? logger = null)
        {
            ArgumentNullException.ThrowIfNull(func);

            try
            {
                return await func(
                    arg1,
                    arg2,
                    arg3).ConfigureAwait(false);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                if (logger != null)
                {
                    _defaultingResultDueToExceptionLogMessageAction(logger, e);
                }

                return defaultResult;
            }
        }

        /// <summary>
        /// Encapsulates a method that has 4 parameters, and in case of any exception it catches it and returns a default value.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T2">The type of the second parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T3">The type of the third parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the method that this delegate encapsulates.</typeparam>
        /// <param name="func">The method to encapsulate.</param>
        /// <param name="arg1">The first parameter the encapsulated method takes.</param>
        /// <param name="arg2">The second parameter the encapsulated method takes.</param>
        /// <param name="arg3">The third parameter the encapsulated method takes.</param>
        /// <param name="arg4">The fourth parameter the encapsulated method takes.</param>
        /// <param name="defaultResult">The default value to return if an exception is thrown.</param>
        /// <param name="logger">Logging framework instance.</param>
        /// <returns>The return value of the method that this delegate encapsulates.</returns>
        public static async Task<TResult> DefaultIfExceptionAsync<T1, T2, T3, T4, TResult>(
            this Func<T1, T2, T3, T4, Task<TResult>> func,
            T1 arg1,
            T2 arg2,
            T3 arg3,
            T4 arg4,
            TResult defaultResult,
            ILogger? logger = null)
        {
            ArgumentNullException.ThrowIfNull(func);

            try
            {
                return await func(
                    arg1,
                    arg2,
                    arg3,
                    arg4).ConfigureAwait(false);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                if (logger != null)
                {
                    _defaultingResultDueToExceptionLogMessageAction(logger, e);
                }

                return defaultResult;
            }
        }

        /// <summary>
        /// Encapsulates a method that has 5 parameters, and in case of any exception it catches it and returns a default value.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T2">The type of the second parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T3">The type of the third parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the method that this delegate encapsulates.</typeparam>
        /// <param name="func">The method to encapsulate.</param>
        /// <param name="arg1">The first parameter the encapsulated method takes.</param>
        /// <param name="arg2">The second parameter the encapsulated method takes.</param>
        /// <param name="arg3">The third parameter the encapsulated method takes.</param>
        /// <param name="arg4">The fourth parameter the encapsulated method takes.</param>
        /// <param name="arg5">The fifth parameter the encapsulated method takes.</param>
        /// <param name="defaultResult">The default value to return if an exception is thrown.</param>
        /// <param name="logger">Logging framework instance.</param>
        /// <returns>The return value of the method that this delegate encapsulates.</returns>
        public static async Task<TResult> DefaultIfExceptionAsync<T1, T2, T3, T4, T5, TResult>(
            this Func<T1, T2, T3, T4, T5, Task<TResult>> func,
            T1 arg1,
            T2 arg2,
            T3 arg3,
            T4 arg4,
            T5 arg5,
            TResult defaultResult,
            ILogger? logger = null)
        {
            ArgumentNullException.ThrowIfNull(func);

            try
            {
                return await func(
                    arg1,
                    arg2,
                    arg3,
                    arg4,
                    arg5).ConfigureAwait(false);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                if (logger != null)
                {
                    _defaultingResultDueToExceptionLogMessageAction(logger, e);
                }

                return defaultResult;
            }
        }

        /// <summary>
        /// Encapsulates a method that has 6 parameters, and in case of any exception it catches it and returns a default value.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T2">The type of the second parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T3">The type of the third parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the method that this delegate encapsulates.</typeparam>
        /// <param name="func">The method to encapsulate.</param>
        /// <param name="arg1">The first parameter the encapsulated method takes.</param>
        /// <param name="arg2">The second parameter the encapsulated method takes.</param>
        /// <param name="arg3">The third parameter the encapsulated method takes.</param>
        /// <param name="arg4">The fourth parameter the encapsulated method takes.</param>
        /// <param name="arg5">The fifth parameter the encapsulated method takes.</param>
        /// <param name="arg6">The sixth parameter the encapsulated method takes.</param>
        /// <param name="defaultResult">The default value to return if an exception is thrown.</param>
        /// <param name="logger">Logging framework instance.</param>
        /// <returns>The return value of the method that this delegate encapsulates.</returns>
        public static async Task<TResult> DefaultIfExceptionAsync<T1, T2, T3, T4, T5, T6, TResult>(
            this Func<T1, T2, T3, T4, T5, T6, Task<TResult>> func,
            T1 arg1,
            T2 arg2,
            T3 arg3,
            T4 arg4,
            T5 arg5,
            T6 arg6,
            TResult defaultResult,
            ILogger? logger = null)
        {
            ArgumentNullException.ThrowIfNull(func);

            try
            {
                return await func(
                    arg1,
                    arg2,
                    arg3,
                    arg4,
                    arg5,
                    arg6).ConfigureAwait(false);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                if (logger != null)
                {
                    _defaultingResultDueToExceptionLogMessageAction(logger, e);
                }

                return defaultResult;
            }
        }

        /// <summary>
        /// Encapsulates a method that has 7 parameters, and in case of any exception it catches it and returns a default value.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T2">The type of the second parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T3">The type of the third parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the method that this delegate encapsulates.</typeparam>
        /// <param name="func">The method to encapsulate.</param>
        /// <param name="arg1">The first parameter the encapsulated method takes.</param>
        /// <param name="arg2">The second parameter the encapsulated method takes.</param>
        /// <param name="arg3">The third parameter the encapsulated method takes.</param>
        /// <param name="arg4">The fourth parameter the encapsulated method takes.</param>
        /// <param name="arg5">The fifth parameter the encapsulated method takes.</param>
        /// <param name="arg6">The sixth parameter the encapsulated method takes.</param>
        /// <param name="arg7">The seventh parameter the encapsulated method takes.</param>
        /// <param name="defaultResult">The default value to return if an exception is thrown.</param>
        /// <param name="logger">Logging framework instance.</param>
        /// <returns>The return value of the method that this delegate encapsulates.</returns>
        public static async Task<TResult> DefaultIfExceptionAsync<T1, T2, T3, T4, T5, T6, T7, TResult>(
            this Func<T1, T2, T3, T4, T5, T6, T7, Task<TResult>> func,
            T1 arg1,
            T2 arg2,
            T3 arg3,
            T4 arg4,
            T5 arg5,
            T6 arg6,
            T7 arg7,
            TResult defaultResult,
            ILogger? logger = null)
        {
            ArgumentNullException.ThrowIfNull(func);

            try
            {
                return await func(
                    arg1,
                    arg2,
                    arg3,
                    arg4,
                    arg5,
                    arg6,
                    arg7).ConfigureAwait(false);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                if (logger != null)
                {
                    _defaultingResultDueToExceptionLogMessageAction(logger, e);
                }

                return defaultResult;
            }
        }

        /// <summary>
        /// Encapsulates a method that has 8 parameters, and in case of any exception it catches it and returns a default value.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T2">The type of the second parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T3">The type of the third parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the method that this delegate encapsulates.</typeparam>
        /// <param name="func">The method to encapsulate.</param>
        /// <param name="arg1">The first parameter the encapsulated method takes.</param>
        /// <param name="arg2">The second parameter the encapsulated method takes.</param>
        /// <param name="arg3">The third parameter the encapsulated method takes.</param>
        /// <param name="arg4">The fourth parameter the encapsulated method takes.</param>
        /// <param name="arg5">The fifth parameter the encapsulated method takes.</param>
        /// <param name="arg6">The sixth parameter the encapsulated method takes.</param>
        /// <param name="arg7">The seventh parameter the encapsulated method takes.</param>
        /// <param name="arg8">The eighth parameter the encapsulated method takes.</param>
        /// <param name="defaultResult">The default value to return if an exception is thrown.</param>
        /// <param name="logger">Logging framework instance.</param>
        /// <returns>The return value of the method that this delegate encapsulates.</returns>
        public static async Task<TResult> DefaultIfExceptionAsync<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, Task<TResult>> func,
            T1 arg1,
            T2 arg2,
            T3 arg3,
            T4 arg4,
            T5 arg5,
            T6 arg6,
            T7 arg7,
            T8 arg8,
            TResult defaultResult,
            ILogger? logger = null)
        {
            ArgumentNullException.ThrowIfNull(func);

            try
            {
                return await func(
                    arg1,
                    arg2,
                    arg3,
                    arg4,
                    arg5,
                    arg6,
                    arg7,
                    arg8).ConfigureAwait(false);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                if (logger != null)
                {
                    _defaultingResultDueToExceptionLogMessageAction(logger, e);
                }

                return defaultResult;
            }
        }

        /// <summary>
        /// Encapsulates a method that has 9 parameters, and in case of any exception it catches it and returns a default value.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T2">The type of the second parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T3">The type of the third parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the method that this delegate encapsulates.</typeparam>
        /// <param name="func">The method to encapsulate.</param>
        /// <param name="arg1">The first parameter the encapsulated method takes.</param>
        /// <param name="arg2">The second parameter the encapsulated method takes.</param>
        /// <param name="arg3">The third parameter the encapsulated method takes.</param>
        /// <param name="arg4">The fourth parameter the encapsulated method takes.</param>
        /// <param name="arg5">The fifth parameter the encapsulated method takes.</param>
        /// <param name="arg6">The sixth parameter the encapsulated method takes.</param>
        /// <param name="arg7">The seventh parameter the encapsulated method takes.</param>
        /// <param name="arg8">The eighth parameter the encapsulated method takes.</param>
        /// <param name="arg9">The ninth parameter the encapsulated method takes.</param>
        /// <param name="defaultResult">The default value to return if an exception is thrown.</param>
        /// <param name="logger">Logging framework instance.</param>
        /// <returns>The return value of the method that this delegate encapsulates.</returns>
        public static async Task<TResult> DefaultIfExceptionAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Task<TResult>> func,
            T1 arg1,
            T2 arg2,
            T3 arg3,
            T4 arg4,
            T5 arg5,
            T6 arg6,
            T7 arg7,
            T8 arg8,
            T9 arg9,
            TResult defaultResult,
            ILogger? logger = null)
        {
            ArgumentNullException.ThrowIfNull(func);

            try
            {
                return await func(
                    arg1,
                    arg2,
                    arg3,
                    arg4,
                    arg5,
                    arg6,
                    arg7,
                    arg8,
                    arg9).ConfigureAwait(false);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                if (logger != null)
                {
                    _defaultingResultDueToExceptionLogMessageAction(logger, e);
                }

                return defaultResult;
            }
        }

        /// <summary>
        /// Encapsulates a method that has 10 parameters, and in case of any exception it catches it and returns a default value.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T2">The type of the second parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T3">The type of the third parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T10">The type of the tenth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the method that this delegate encapsulates.</typeparam>
        /// <param name="func">The method to encapsulate.</param>
        /// <param name="arg1">The first parameter the encapsulated method takes.</param>
        /// <param name="arg2">The second parameter the encapsulated method takes.</param>
        /// <param name="arg3">The third parameter the encapsulated method takes.</param>
        /// <param name="arg4">The fourth parameter the encapsulated method takes.</param>
        /// <param name="arg5">The fifth parameter the encapsulated method takes.</param>
        /// <param name="arg6">The sixth parameter the encapsulated method takes.</param>
        /// <param name="arg7">The seventh parameter the encapsulated method takes.</param>
        /// <param name="arg8">The eighth parameter the encapsulated method takes.</param>
        /// <param name="arg9">The ninth parameter the encapsulated method takes.</param>
        /// <param name="arg10">The tenth parameter the encapsulated method takes.</param>
        /// <param name="defaultResult">The default value to return if an exception is thrown.</param>
        /// <param name="logger">Logging framework instance.</param>
        /// <returns>The return value of the method that this delegate encapsulates.</returns>
        public static async Task<TResult> DefaultIfExceptionAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Task<TResult>> func,
            T1 arg1,
            T2 arg2,
            T3 arg3,
            T4 arg4,
            T5 arg5,
            T6 arg6,
            T7 arg7,
            T8 arg8,
            T9 arg9,
            T10 arg10,
            TResult defaultResult,
            ILogger? logger = null)
        {
            ArgumentNullException.ThrowIfNull(func);

            try
            {
                return await func(
                    arg1,
                    arg2,
                    arg3,
                    arg4,
                    arg5,
                    arg6,
                    arg7,
                    arg8,
                    arg9,
                    arg10).ConfigureAwait(false);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                if (logger != null)
                {
                    _defaultingResultDueToExceptionLogMessageAction(logger, e);
                }

                return defaultResult;
            }
        }

        /// <summary>
        /// Encapsulates a method that has 11 parameters, and in case of any exception it catches it and returns a default value.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T2">The type of the second parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T3">The type of the third parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T10">The type of the tenth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T11">The type of the eleventh parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the method that this delegate encapsulates.</typeparam>
        /// <param name="func">The method to encapsulate.</param>
        /// <param name="arg1">The first parameter the encapsulated method takes.</param>
        /// <param name="arg2">The second parameter the encapsulated method takes.</param>
        /// <param name="arg3">The third parameter the encapsulated method takes.</param>
        /// <param name="arg4">The fourth parameter the encapsulated method takes.</param>
        /// <param name="arg5">The fifth parameter the encapsulated method takes.</param>
        /// <param name="arg6">The sixth parameter the encapsulated method takes.</param>
        /// <param name="arg7">The seventh parameter the encapsulated method takes.</param>
        /// <param name="arg8">The eighth parameter the encapsulated method takes.</param>
        /// <param name="arg9">The ninth parameter the encapsulated method takes.</param>
        /// <param name="arg10">The tenth parameter the encapsulated method takes.</param>
        /// <param name="arg11">The eleventh parameter the encapsulated method takes.</param>
        /// <param name="defaultResult">The default value to return if an exception is thrown.</param>
        /// <param name="logger">Logging framework instance.</param>
        /// <returns>The return value of the method that this delegate encapsulates.</returns>
        public static async Task<TResult> DefaultIfExceptionAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, TResult>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, Task<TResult>> func,
            T1 arg1,
            T2 arg2,
            T3 arg3,
            T4 arg4,
            T5 arg5,
            T6 arg6,
            T7 arg7,
            T8 arg8,
            T9 arg9,
            T10 arg10,
            T11 arg11,
            TResult defaultResult,
            ILogger? logger = null)
        {
            ArgumentNullException.ThrowIfNull(func);

            try
            {
                return await func(
                    arg1,
                    arg2,
                    arg3,
                    arg4,
                    arg5,
                    arg6,
                    arg7,
                    arg8,
                    arg9,
                    arg10,
                    arg11).ConfigureAwait(false);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                if (logger != null)
                {
                    _defaultingResultDueToExceptionLogMessageAction(logger, e);
                }

                return defaultResult;
            }
        }

        /// <summary>
        /// Encapsulates a method that has 12 parameters, and in case of any exception it catches it and returns a default value.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T2">The type of the second parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T3">The type of the third parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T10">The type of the tenth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T11">The type of the eleventh parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T12">The type of the twelfth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the method that this delegate encapsulates.</typeparam>
        /// <param name="func">The method to encapsulate.</param>
        /// <param name="arg1">The first parameter the encapsulated method takes.</param>
        /// <param name="arg2">The second parameter the encapsulated method takes.</param>
        /// <param name="arg3">The third parameter the encapsulated method takes.</param>
        /// <param name="arg4">The fourth parameter the encapsulated method takes.</param>
        /// <param name="arg5">The fifth parameter the encapsulated method takes.</param>
        /// <param name="arg6">The sixth parameter the encapsulated method takes.</param>
        /// <param name="arg7">The seventh parameter the encapsulated method takes.</param>
        /// <param name="arg8">The eighth parameter the encapsulated method takes.</param>
        /// <param name="arg9">The ninth parameter the encapsulated method takes.</param>
        /// <param name="arg10">The tenth parameter the encapsulated method takes.</param>
        /// <param name="arg11">The eleventh parameter the encapsulated method takes.</param>
        /// <param name="arg12">The twelfth parameter the encapsulated method takes.</param>
        /// <param name="defaultResult">The default value to return if an exception is thrown.</param>
        /// <param name="logger">Logging framework instance.</param>
        /// <returns>The return value of the method that this delegate encapsulates.</returns>
        public static async Task<TResult> DefaultIfExceptionAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, TResult>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, Task<TResult>> func,
            T1 arg1,
            T2 arg2,
            T3 arg3,
            T4 arg4,
            T5 arg5,
            T6 arg6,
            T7 arg7,
            T8 arg8,
            T9 arg9,
            T10 arg10,
            T11 arg11,
            T12 arg12,
            TResult defaultResult,
            ILogger? logger = null)
        {
            ArgumentNullException.ThrowIfNull(func);

            try
            {
                return await func(
                    arg1,
                    arg2,
                    arg3,
                    arg4,
                    arg5,
                    arg6,
                    arg7,
                    arg8,
                    arg9,
                    arg10,
                    arg11,
                    arg12).ConfigureAwait(false);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                if (logger != null)
                {
                    _defaultingResultDueToExceptionLogMessageAction(logger, e);
                }

                return defaultResult;
            }
        }

        /// <summary>
        /// Encapsulates a method that has 13 parameters, and in case of any exception it catches it and returns a default value.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T2">The type of the second parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T3">The type of the third parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T10">The type of the tenth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T11">The type of the eleventh parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T12">The type of the twelfth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T13">The type of the thirteenth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the method that this delegate encapsulates.</typeparam>
        /// <param name="func">The method to encapsulate.</param>
        /// <param name="arg1">The first parameter the encapsulated method takes.</param>
        /// <param name="arg2">The second parameter the encapsulated method takes.</param>
        /// <param name="arg3">The third parameter the encapsulated method takes.</param>
        /// <param name="arg4">The fourth parameter the encapsulated method takes.</param>
        /// <param name="arg5">The fifth parameter the encapsulated method takes.</param>
        /// <param name="arg6">The sixth parameter the encapsulated method takes.</param>
        /// <param name="arg7">The seventh parameter the encapsulated method takes.</param>
        /// <param name="arg8">The eighth parameter the encapsulated method takes.</param>
        /// <param name="arg9">The ninth parameter the encapsulated method takes.</param>
        /// <param name="arg10">The tenth parameter the encapsulated method takes.</param>
        /// <param name="arg11">The eleventh parameter the encapsulated method takes.</param>
        /// <param name="arg12">The twelfth parameter the encapsulated method takes.</param>
        /// <param name="arg13">The thirteenth parameter the encapsulated method takes.</param>
        /// <param name="defaultResult">The default value to return if an exception is thrown.</param>
        /// <param name="logger">Logging framework instance.</param>
        /// <returns>The return value of the method that this delegate encapsulates.</returns>
        public static async Task<TResult> DefaultIfExceptionAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, TResult>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, Task<TResult>> func,
            T1 arg1,
            T2 arg2,
            T3 arg3,
            T4 arg4,
            T5 arg5,
            T6 arg6,
            T7 arg7,
            T8 arg8,
            T9 arg9,
            T10 arg10,
            T11 arg11,
            T12 arg12,
            T13 arg13,
            TResult defaultResult,
            ILogger? logger = null)
        {
            ArgumentNullException.ThrowIfNull(func);

            try
            {
                return await func(
                    arg1,
                    arg2,
                    arg3,
                    arg4,
                    arg5,
                    arg6,
                    arg7,
                    arg8,
                    arg9,
                    arg10,
                    arg11,
                    arg12,
                    arg13).ConfigureAwait(false);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                if (logger != null)
                {
                    _defaultingResultDueToExceptionLogMessageAction(logger, e);
                }

                return defaultResult;
            }
        }

        /// <summary>
        /// Encapsulates a method that has 14 parameters, and in case of any exception it catches it and returns a default value.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T2">The type of the second parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T3">The type of the third parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T10">The type of the tenth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T11">The type of the eleventh parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T12">The type of the twelfth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T13">The type of the thirteenth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T14">The type of the fourteenth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the method that this delegate encapsulates.</typeparam>
        /// <param name="func">The method to encapsulate.</param>
        /// <param name="arg1">The first parameter the encapsulated method takes.</param>
        /// <param name="arg2">The second parameter the encapsulated method takes.</param>
        /// <param name="arg3">The third parameter the encapsulated method takes.</param>
        /// <param name="arg4">The fourth parameter the encapsulated method takes.</param>
        /// <param name="arg5">The fifth parameter the encapsulated method takes.</param>
        /// <param name="arg6">The sixth parameter the encapsulated method takes.</param>
        /// <param name="arg7">The seventh parameter the encapsulated method takes.</param>
        /// <param name="arg8">The eighth parameter the encapsulated method takes.</param>
        /// <param name="arg9">The ninth parameter the encapsulated method takes.</param>
        /// <param name="arg10">The tenth parameter the encapsulated method takes.</param>
        /// <param name="arg11">The eleventh parameter the encapsulated method takes.</param>
        /// <param name="arg12">The twelfth parameter the encapsulated method takes.</param>
        /// <param name="arg13">The thirteenth parameter the encapsulated method takes.</param>
        /// <param name="arg14">The fourteenth parameter the encapsulated method takes.</param>
        /// <param name="defaultResult">The default value to return if an exception is thrown.</param>
        /// <param name="logger">Logging framework instance.</param>
        /// <returns>The return value of the method that this delegate encapsulates.</returns>
        public static async Task<TResult> DefaultIfExceptionAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, TResult>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, Task<TResult>> func,
            T1 arg1,
            T2 arg2,
            T3 arg3,
            T4 arg4,
            T5 arg5,
            T6 arg6,
            T7 arg7,
            T8 arg8,
            T9 arg9,
            T10 arg10,
            T11 arg11,
            T12 arg12,
            T13 arg13,
            T14 arg14,
            TResult defaultResult,
            ILogger? logger = null)
        {
            ArgumentNullException.ThrowIfNull(func);

            try
            {
                return await func(
                    arg1,
                    arg2,
                    arg3,
                    arg4,
                    arg5,
                    arg6,
                    arg7,
                    arg8,
                    arg9,
                    arg10,
                    arg11,
                    arg12,
                    arg13,
                    arg14).ConfigureAwait(false);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                if (logger != null)
                {
                    _defaultingResultDueToExceptionLogMessageAction(logger, e);
                }

                return defaultResult;
            }
        }

        /// <summary>
        /// Encapsulates a method that has 15 parameters, and in case of any exception it catches it and returns a default value.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T2">The type of the second parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T3">The type of the third parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T10">The type of the tenth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T11">The type of the eleventh parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T12">The type of the twelfth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T13">The type of the thirteenth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T14">The type of the fourteenth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T15">The type of the fifteenth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the method that this delegate encapsulates.</typeparam>
        /// <param name="func">The method to encapsulate.</param>
        /// <param name="arg1">The first parameter the encapsulated method takes.</param>
        /// <param name="arg2">The second parameter the encapsulated method takes.</param>
        /// <param name="arg3">The third parameter the encapsulated method takes.</param>
        /// <param name="arg4">The fourth parameter the encapsulated method takes.</param>
        /// <param name="arg5">The fifth parameter the encapsulated method takes.</param>
        /// <param name="arg6">The sixth parameter the encapsulated method takes.</param>
        /// <param name="arg7">The seventh parameter the encapsulated method takes.</param>
        /// <param name="arg8">The eighth parameter the encapsulated method takes.</param>
        /// <param name="arg9">The ninth parameter the encapsulated method takes.</param>
        /// <param name="arg10">The tenth parameter the encapsulated method takes.</param>
        /// <param name="arg11">The eleventh parameter the encapsulated method takes.</param>
        /// <param name="arg12">The twelfth parameter the encapsulated method takes.</param>
        /// <param name="arg13">The thirteenth parameter the encapsulated method takes.</param>
        /// <param name="arg14">The fourteenth parameter the encapsulated method takes.</param>
        /// <param name="arg15">The fifteenth parameter the encapsulated method takes.</param>
        /// <param name="defaultResult">The default value to return if an exception is thrown.</param>
        /// <param name="logger">Logging framework instance.</param>
        /// <returns>The return value of the method that this delegate encapsulates.</returns>
        public static async Task<TResult> DefaultIfExceptionAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, TResult>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, Task<TResult>> func,
            T1 arg1,
            T2 arg2,
            T3 arg3,
            T4 arg4,
            T5 arg5,
            T6 arg6,
            T7 arg7,
            T8 arg8,
            T9 arg9,
            T10 arg10,
            T11 arg11,
            T12 arg12,
            T13 arg13,
            T14 arg14,
            T15 arg15,
            TResult defaultResult,
            ILogger? logger = null)
        {
            ArgumentNullException.ThrowIfNull(func);

            try
            {
                return await func(
                    arg1,
                    arg2,
                    arg3,
                    arg4,
                    arg5,
                    arg6,
                    arg7,
                    arg8,
                    arg9,
                    arg10,
                    arg11,
                    arg12,
                    arg13,
                    arg14,
                    arg15).ConfigureAwait(false);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                if (logger != null)
                {
                    _defaultingResultDueToExceptionLogMessageAction(logger, e);
                }

                return defaultResult;
            }
        }

        /// <summary>
        /// Encapsulates a method that has 16 parameters, and in case of any exception it catches it and returns a default value.
        /// </summary>
        /// <typeparam name="T1">The type of the first parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T2">The type of the second parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T3">The type of the third parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T4">The type of the fourth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T5">The type of the fifth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T6">The type of the sixth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T7">The type of the seventh parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T8">The type of the eighth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T9">The type of the ninth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T10">The type of the tenth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T11">The type of the eleventh parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T12">The type of the twelfth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T13">The type of the thirteenth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T14">The type of the fourteenth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T15">The type of the fifteenth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="T16">The type of the sixteenth parameter the encapsulated method takes.</typeparam>
        /// <typeparam name="TResult">The type of the return value of the method that this delegate encapsulates.</typeparam>
        /// <param name="func">The method to encapsulate.</param>
        /// <param name="arg1">The first parameter the encapsulated method takes.</param>
        /// <param name="arg2">The second parameter the encapsulated method takes.</param>
        /// <param name="arg3">The third parameter the encapsulated method takes.</param>
        /// <param name="arg4">The fourth parameter the encapsulated method takes.</param>
        /// <param name="arg5">The fifth parameter the encapsulated method takes.</param>
        /// <param name="arg6">The sixth parameter the encapsulated method takes.</param>
        /// <param name="arg7">The seventh parameter the encapsulated method takes.</param>
        /// <param name="arg8">The eighth parameter the encapsulated method takes.</param>
        /// <param name="arg9">The ninth parameter the encapsulated method takes.</param>
        /// <param name="arg10">The tenth parameter the encapsulated method takes.</param>
        /// <param name="arg11">The eleventh parameter the encapsulated method takes.</param>
        /// <param name="arg12">The twelfth parameter the encapsulated method takes.</param>
        /// <param name="arg13">The thirteenth parameter the encapsulated method takes.</param>
        /// <param name="arg14">The fourteenth parameter the encapsulated method takes.</param>
        /// <param name="arg15">The fifteenth parameter the encapsulated method takes.</param>
        /// <param name="arg16">The sixteenth parameter the encapsulated method takes.</param>
        /// <param name="defaultResult">The default value to return if an exception is thrown.</param>
        /// <param name="logger">Logging framework instance.</param>
        /// <returns>The return value of the method that this delegate encapsulates.</returns>
        public static async Task<TResult> DefaultIfExceptionAsync<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, TResult>(
            this Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16, Task<TResult>> func,
            T1 arg1,
            T2 arg2,
            T3 arg3,
            T4 arg4,
            T5 arg5,
            T6 arg6,
            T7 arg7,
            T8 arg8,
            T9 arg9,
            T10 arg10,
            T11 arg11,
            T12 arg12,
            T13 arg13,
            T14 arg14,
            T15 arg15,
            T16 arg16,
            TResult defaultResult,
            ILogger? logger = null)
        {
            ArgumentNullException.ThrowIfNull(func);

            try
            {
                return await func(
                    arg1,
                    arg2,
                    arg3,
                    arg4,
                    arg5,
                    arg6,
                    arg7,
                    arg8,
                    arg9,
                    arg10,
                    arg11,
                    arg12,
                    arg13,
                    arg14,
                    arg15,
                    arg16).ConfigureAwait(false);
            }
#pragma warning disable CA1031 // Do not catch general exception types
            catch (Exception e)
#pragma warning restore CA1031 // Do not catch general exception types
            {
                if (logger != null)
                {
                    _defaultingResultDueToExceptionLogMessageAction(logger, e);
                }

                return defaultResult;
            }
        }
    }
}
