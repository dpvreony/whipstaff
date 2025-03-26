// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Runtime.CompilerServices;

#if ARGUMENT_NULL_EXCEPTION_SHIM
using ArgumentNullException = Whipstaff.Runtime.Exceptions.ArgumentNullException;
#endif

namespace Whipstaff.Runtime.Maths
{
    /// <summary>
    /// Math helpers to build upon the dotnet runtime.
    /// </summary>
    public static class MathHelpers
    {
        /// <summary>
        /// Gets the item that has the highest value, based on a selector.
        /// </summary>
        /// <typeparam name="T">The type being compared.</typeparam>
        /// <param name="arg1">The first argument to compare.</param>
        /// <param name="arg2">The second argument to compare.</param>
        /// <param name="selector">The function to use to get the value to compare on each argument.</param>
        /// <returns>The item with the highest value.</returns>
        public static T ItemWithMax<T>(
            T arg1,
            T arg2,
            Func<T, long> selector)
        {
            ArgumentNullException.ThrowIfNull(selector);

            var arg1Number = selector(arg1);
            var arg2Number = selector(arg2);

            return ItemWithMaxInternal(
                arg1,
                arg1Number,
                arg2,
                arg2Number);
        }

        /// <summary>
        /// Gets the item that has the highest value, based on a selector.
        /// </summary>
        /// <typeparam name="T">The type being compared.</typeparam>
        /// <param name="arg1">The first argument to compare.</param>
        /// <param name="arg2">The second argument to compare.</param>
        /// <param name="arg3">The third argument to compare.</param>
        /// <param name="selector">The function to use to get the value to compare on each argument.</param>
        /// <returns>The item with the highest value.</returns>
        public static T ItemWithMax<T>(
            T arg1,
            T arg2,
            T arg3,
            Func<T, long> selector)
        {
            ArgumentNullException.ThrowIfNull(selector);

            var arg1Number = selector(arg1);
            var arg2Number = selector(arg2);
            var arg3Number = selector(arg3);

            return ItemWithMaxInternal(
                arg1,
                arg1Number,
                arg2,
                arg2Number,
                arg3,
                arg3Number);
        }

        /// <summary>
        /// Gets the item that has the highest value, based on a selector.
        /// </summary>
        /// <typeparam name="T">The type being compared.</typeparam>
        /// <param name="arg1">The first argument to compare.</param>
        /// <param name="arg2">The second argument to compare.</param>
        /// <param name="arg3">The third argument to compare.</param>
        /// <param name="arg4">The forth argument to compare.</param>
        /// <param name="selector">The function to use to get the value to compare on each argument.</param>
        /// <returns>The item with the highest value.</returns>
        public static T ItemWithMax<T>(
            T arg1,
            T arg2,
            T arg3,
            T arg4,
            Func<T, long> selector)
        {
            ArgumentNullException.ThrowIfNull(selector);

            var arg1Number = selector(arg1);
            var arg2Number = selector(arg2);
            var arg3Number = selector(arg3);
            var arg4Number = selector(arg4);

            return ItemWithMaxInternal(
                arg1,
                arg1Number,
                arg2,
                arg2Number,
                arg3,
                arg3Number,
                arg4,
                arg4Number);
        }

        /// <summary>
        /// Gets the item that has the highest value, based on a selector.
        /// </summary>
        /// <typeparam name="T">The type being compared.</typeparam>
        /// <param name="arg1">The first argument to compare.</param>
        /// <param name="arg2">The second argument to compare.</param>
        /// <param name="arg3">The third argument to compare.</param>
        /// <param name="arg4">The forth argument to compare.</param>
        /// <param name="arg5">The fifth argument to compare.</param>
        /// <param name="selector">The function to use to get the value to compare on each argument.</param>
        /// <returns>The item with the highest value.</returns>
        public static T ItemWithMax<T>(
            T arg1,
            T arg2,
            T arg3,
            T arg4,
            T arg5,
            Func<T, long> selector)
        {
            ArgumentNullException.ThrowIfNull(selector);

            var arg1Number = selector(arg1);
            var arg2Number = selector(arg2);
            var arg3Number = selector(arg3);
            var arg4Number = selector(arg4);
            var arg5Number = selector(arg5);

            return ItemWithMaxInternal(
                arg1,
                arg1Number,
                arg2,
                arg2Number,
                arg3,
                arg3Number,
                arg4,
                arg4Number,
                arg5,
                arg5Number);
        }

        /// <summary>
        /// Gets the item that has the highest value, based on a selector.
        /// </summary>
        /// <typeparam name="T">The type being compared.</typeparam>
        /// <param name="arg1">The first argument to compare.</param>
        /// <param name="arg2">The second argument to compare.</param>
        /// <param name="arg3">The third argument to compare.</param>
        /// <param name="arg4">The forth argument to compare.</param>
        /// <param name="arg5">The fifth argument to compare.</param>
        /// <param name="arg6">The sixth argument to compare.</param>
        /// <param name="selector">The function to use to get the value to compare on each argument.</param>
        /// <returns>The item with the highest value.</returns>
        public static T ItemWithMax<T>(
            T arg1,
            T arg2,
            T arg3,
            T arg4,
            T arg5,
            T arg6,
            Func<T, long> selector)
        {
            ArgumentNullException.ThrowIfNull(selector);

            var arg1Number = selector(arg1);
            var arg2Number = selector(arg2);
            var arg3Number = selector(arg3);
            var arg4Number = selector(arg4);
            var arg5Number = selector(arg5);
            var arg6Number = selector(arg6);

            return ItemWithMaxInternal(
                arg1,
                arg1Number,
                arg2,
                arg2Number,
                arg3,
                arg3Number,
                arg4,
                arg4Number,
                arg5,
                arg5Number,
                arg6,
                arg6Number);
        }

        /// <summary>
        /// Gets the item that has the highest value, based on a selector.
        /// </summary>
        /// <typeparam name="T">The type being compared.</typeparam>
        /// <param name="arg1">The first argument to compare.</param>
        /// <param name="arg2">The second argument to compare.</param>
        /// <param name="arg3">The third argument to compare.</param>
        /// <param name="arg4">The forth argument to compare.</param>
        /// <param name="arg5">The fifth argument to compare.</param>
        /// <param name="arg6">The sixth argument to compare.</param>
        /// <param name="arg7">The seventh argument to compare.</param>
        /// <param name="selector">The function to use to get the value to compare on each argument.</param>
        /// <returns>The item with the highest value.</returns>
        public static T ItemWithMax<T>(
            T arg1,
            T arg2,
            T arg3,
            T arg4,
            T arg5,
            T arg6,
            T arg7,
            Func<T, long> selector)
        {
            ArgumentNullException.ThrowIfNull(selector);

            var arg1Number = selector(arg1);
            var arg2Number = selector(arg2);
            var arg3Number = selector(arg3);
            var arg4Number = selector(arg4);
            var arg5Number = selector(arg5);
            var arg6Number = selector(arg6);
            var arg7Number = selector(arg7);

            return ItemWithMaxInternal(
                arg1,
                arg1Number,
                arg2,
                arg2Number,
                arg3,
                arg3Number,
                arg4,
                arg4Number,
                arg5,
                arg5Number,
                arg6,
                arg6Number,
                arg7,
                arg7Number);
        }

        /// <summary>
        /// Gets the item that has the highest value, based on a selector.
        /// </summary>
        /// <typeparam name="T">The type being compared.</typeparam>
        /// <param name="arg1">The first argument to compare.</param>
        /// <param name="arg2">The second argument to compare.</param>
        /// <param name="arg3">The third argument to compare.</param>
        /// <param name="arg4">The forth argument to compare.</param>
        /// <param name="arg5">The fifth argument to compare.</param>
        /// <param name="arg6">The sixth argument to compare.</param>
        /// <param name="arg7">The seventh argument to compare.</param>
        /// <param name="arg8">The eighth argument to compare.</param>
        /// <param name="selector">The function to use to get the value to compare on each argument.</param>
        /// <returns>The item with the highest value.</returns>
        public static T ItemWithMax<T>(
            T arg1,
            T arg2,
            T arg3,
            T arg4,
            T arg5,
            T arg6,
            T arg7,
            T arg8,
            Func<T, long> selector)
        {
            ArgumentNullException.ThrowIfNull(selector);

            var arg1Number = selector(arg1);
            var arg2Number = selector(arg2);
            var arg3Number = selector(arg3);
            var arg4Number = selector(arg4);
            var arg5Number = selector(arg5);
            var arg6Number = selector(arg6);
            var arg7Number = selector(arg7);
            var arg8Number = selector(arg8);
            return ItemWithMaxInternal(
                arg1,
                arg1Number,
                arg2,
                arg2Number,
                arg3,
                arg3Number,
                arg4,
                arg4Number,
                arg5,
                arg5Number,
                arg6,
                arg6Number,
                arg7,
                arg7Number,
                arg8,
                arg8Number);
        }

        /// <summary>
        /// Gets the item that has the highest value, based on a selector.
        /// </summary>
        /// <typeparam name="T">The type being compared.</typeparam>
        /// <param name="arg1">The first argument to compare.</param>
        /// <param name="arg2">The second argument to compare.</param>
        /// <param name="arg3">The third argument to compare.</param>
        /// <param name="arg4">The forth argument to compare.</param>
        /// <param name="arg5">The fifth argument to compare.</param>
        /// <param name="arg6">The sixth argument to compare.</param>
        /// <param name="arg7">The seventh argument to compare.</param>
        /// <param name="arg8">The eighth argument to compare.</param>
        /// <param name="arg9">The ninth argument to compare.</param>
        /// <param name="selector">The function to use to get the value to compare on each argument.</param>
        /// <returns>The item with the highest value.</returns>
        public static T ItemWithMax<T>(
            T arg1,
            T arg2,
            T arg3,
            T arg4,
            T arg5,
            T arg6,
            T arg7,
            T arg8,
            T arg9,
            Func<T, long> selector)
        {
            ArgumentNullException.ThrowIfNull(selector);

            var arg1Number = selector(arg1);
            var arg2Number = selector(arg2);
            var arg3Number = selector(arg3);
            var arg4Number = selector(arg4);
            var arg5Number = selector(arg5);
            var arg6Number = selector(arg6);
            var arg7Number = selector(arg7);
            var arg8Number = selector(arg8);
            var arg9Number = selector(arg9);

            return ItemWithMaxInternal(
                arg1,
                arg1Number,
                arg2,
                arg2Number,
                arg3,
                arg3Number,
                arg4,
                arg4Number,
                arg5,
                arg5Number,
                arg6,
                arg6Number,
                arg7,
                arg7Number,
                arg8,
                arg8Number,
                arg9,
                arg9Number);
        }

        /// <summary>
        /// Gets the item that has the highest value, based on a selector.
        /// </summary>
        /// <typeparam name="T">The type being compared.</typeparam>
        /// <param name="arg1">The first argument to compare.</param>
        /// <param name="arg2">The second argument to compare.</param>
        /// <param name="arg3">The third argument to compare.</param>
        /// <param name="arg4">The forth argument to compare.</param>
        /// <param name="arg5">The fifth argument to compare.</param>
        /// <param name="arg6">The sixth argument to compare.</param>
        /// <param name="arg7">The seventh argument to compare.</param>
        /// <param name="arg8">The eighth argument to compare.</param>
        /// <param name="arg9">The ninth argument to compare.</param>
        /// <param name="arg10">The tenth argument to compare.</param>
        /// <param name="selector">The function to use to get the value to compare on each argument.</param>
        /// <returns>The item with the highest value.</returns>
        public static T ItemWithMax<T>(
            T arg1,
            T arg2,
            T arg3,
            T arg4,
            T arg5,
            T arg6,
            T arg7,
            T arg8,
            T arg9,
            T arg10,
            Func<T, long> selector)
        {
            ArgumentNullException.ThrowIfNull(selector);

            var arg1Number = selector(arg1);
            var arg2Number = selector(arg2);
            var arg3Number = selector(arg3);
            var arg4Number = selector(arg4);
            var arg5Number = selector(arg5);
            var arg6Number = selector(arg6);
            var arg7Number = selector(arg7);
            var arg8Number = selector(arg8);
            var arg9Number = selector(arg9);
            var arg10Number = selector(arg10);

            return ItemWithMaxInternal(
                arg1,
                arg1Number,
                arg2,
                arg2Number,
                arg3,
                arg3Number,
                arg4,
                arg4Number,
                arg5,
                arg5Number,
                arg6,
                arg6Number,
                arg7,
                arg7Number,
                arg8,
                arg8Number,
                arg9,
                arg9Number,
                arg10,
                arg10Number);
        }

        /// <summary>
        /// Gets the item that has the highest value, based on a selector.
        /// </summary>
        /// <typeparam name="T">The type being compared.</typeparam>
        /// <param name="arg1">The first argument to compare.</param>
        /// <param name="arg2">The second argument to compare.</param>
        /// <param name="arg3">The third argument to compare.</param>
        /// <param name="arg4">The forth argument to compare.</param>
        /// <param name="arg5">The fifth argument to compare.</param>
        /// <param name="arg6">The sixth argument to compare.</param>
        /// <param name="arg7">The seventh argument to compare.</param>
        /// <param name="arg8">The eighth argument to compare.</param>
        /// <param name="arg9">The ninth argument to compare.</param>
        /// <param name="arg10">The tenth argument to compare.</param>
        /// <param name="arg11">The eleventh argument to compare.</param>
        /// <param name="selector">The function to use to get the value to compare on each argument.</param>
        /// <returns>The item with the highest value.</returns>
        public static T ItemWithMax<T>(
            T arg1,
            T arg2,
            T arg3,
            T arg4,
            T arg5,
            T arg6,
            T arg7,
            T arg8,
            T arg9,
            T arg10,
            T arg11,
            Func<T, long> selector)
        {
            ArgumentNullException.ThrowIfNull(selector);

            var arg1Number = selector(arg1);
            var arg2Number = selector(arg2);
            var arg3Number = selector(arg3);
            var arg4Number = selector(arg4);
            var arg5Number = selector(arg5);
            var arg6Number = selector(arg6);
            var arg7Number = selector(arg7);
            var arg8Number = selector(arg8);
            var arg9Number = selector(arg9);
            var arg10Number = selector(arg10);
            var arg11Number = selector(arg11);
            return ItemWithMaxInternal(
                arg1,
                arg1Number,
                arg2,
                arg2Number,
                arg3,
                arg3Number,
                arg4,
                arg4Number,
                arg5,
                arg5Number,
                arg6,
                arg6Number,
                arg7,
                arg7Number,
                arg8,
                arg8Number,
                arg9,
                arg9Number,
                arg10,
                arg10Number,
                arg11,
                arg11Number);
        }

        /// <summary>
        /// Gets the item that has the highest value, based on a selector.
        /// </summary>
        /// <typeparam name="T">The type being compared.</typeparam>
        /// <param name="arg1">The first argument to compare.</param>
        /// <param name="arg2">The second argument to compare.</param>
        /// <param name="arg3">The third argument to compare.</param>
        /// <param name="arg4">The forth argument to compare.</param>
        /// <param name="arg5">The fifth argument to compare.</param>
        /// <param name="arg6">The sixth argument to compare.</param>
        /// <param name="arg7">The seventh argument to compare.</param>
        /// <param name="arg8">The eighth argument to compare.</param>
        /// <param name="arg9">The ninth argument to compare.</param>
        /// <param name="arg10">The tenth argument to compare.</param>
        /// <param name="arg11">The eleventh argument to compare.</param>
        /// <param name="arg12">The twelfth argument to compare.</param>
        /// <param name="selector">The function to use to get the value to compare on each argument.</param>
        /// <returns>The item with the highest value.</returns>
        public static T ItemWithMax<T>(
            T arg1,
            T arg2,
            T arg3,
            T arg4,
            T arg5,
            T arg6,
            T arg7,
            T arg8,
            T arg9,
            T arg10,
            T arg11,
            T arg12,
            Func<T, long> selector)
        {
            ArgumentNullException.ThrowIfNull(selector);

            var arg1Number = selector(arg1);
            var arg2Number = selector(arg2);
            var arg3Number = selector(arg3);
            var arg4Number = selector(arg4);
            var arg5Number = selector(arg5);
            var arg6Number = selector(arg6);
            var arg7Number = selector(arg7);
            var arg8Number = selector(arg8);
            var arg9Number = selector(arg9);
            var arg10Number = selector(arg10);
            var arg11Number = selector(arg11);
            var arg12Number = selector(arg12);

            return ItemWithMaxInternal(
                arg1,
                arg1Number,
                arg2,
                arg2Number,
                arg3,
                arg3Number,
                arg4,
                arg4Number,
                arg5,
                arg5Number,
                arg6,
                arg6Number,
                arg7,
                arg7Number,
                arg8,
                arg8Number,
                arg9,
                arg9Number,
                arg10,
                arg10Number,
                arg11,
                arg11Number,
                arg12,
                arg12Number);
        }

        /// <summary>
        /// Gets the item that has the highest value, based on a selector.
        /// </summary>
        /// <typeparam name="T">The type being compared.</typeparam>
        /// <param name="arg1">The first argument to compare.</param>
        /// <param name="arg2">The second argument to compare.</param>
        /// <param name="arg3">The third argument to compare.</param>
        /// <param name="arg4">The forth argument to compare.</param>
        /// <param name="arg5">The fifth argument to compare.</param>
        /// <param name="arg6">The sixth argument to compare.</param>
        /// <param name="arg7">The seventh argument to compare.</param>
        /// <param name="arg8">The eighth argument to compare.</param>
        /// <param name="arg9">The ninth argument to compare.</param>
        /// <param name="arg10">The tenth argument to compare.</param>
        /// <param name="arg11">The eleventh argument to compare.</param>
        /// <param name="arg12">The twelfth argument to compare.</param>
        /// <param name="arg13">The thirteenth argument to compare.</param>
        /// <param name="selector">The function to use to get the value to compare on each argument.</param>
        /// <returns>The item with the highest value.</returns>
        public static T ItemWithMax<T>(
            T arg1,
            T arg2,
            T arg3,
            T arg4,
            T arg5,
            T arg6,
            T arg7,
            T arg8,
            T arg9,
            T arg10,
            T arg11,
            T arg12,
            T arg13,
            Func<T, long> selector)
        {
            ArgumentNullException.ThrowIfNull(selector);

            var arg1Number = selector(arg1);
            var arg2Number = selector(arg2);
            var arg3Number = selector(arg3);
            var arg4Number = selector(arg4);
            var arg5Number = selector(arg5);
            var arg6Number = selector(arg6);
            var arg7Number = selector(arg7);
            var arg8Number = selector(arg8);
            var arg9Number = selector(arg9);
            var arg10Number = selector(arg10);
            var arg11Number = selector(arg11);
            var arg12Number = selector(arg12);
            var arg13Number = selector(arg13);

            return ItemWithMaxInternal(
                arg1,
                arg1Number,
                arg2,
                arg2Number,
                arg3,
                arg3Number,
                arg4,
                arg4Number,
                arg5,
                arg5Number,
                arg6,
                arg6Number,
                arg7,
                arg7Number,
                arg8,
                arg8Number,
                arg9,
                arg9Number,
                arg10,
                arg10Number,
                arg11,
                arg11Number,
                arg12,
                arg12Number,
                arg13,
                arg13Number);
        }

        /// <summary>
        /// Gets the item that has the highest value, based on a selector.
        /// </summary>
        /// <typeparam name="T">The type being compared.</typeparam>
        /// <param name="arg1">The first argument to compare.</param>
        /// <param name="arg2">The second argument to compare.</param>
        /// <param name="arg3">The third argument to compare.</param>
        /// <param name="arg4">The forth argument to compare.</param>
        /// <param name="arg5">The fifth argument to compare.</param>
        /// <param name="arg6">The sixth argument to compare.</param>
        /// <param name="arg7">The seventh argument to compare.</param>
        /// <param name="arg8">The eighth argument to compare.</param>
        /// <param name="arg9">The ninth argument to compare.</param>
        /// <param name="arg10">The tenth argument to compare.</param>
        /// <param name="arg11">The eleventh argument to compare.</param>
        /// <param name="arg12">The twelfth argument to compare.</param>
        /// <param name="arg13">The thirteenth argument to compare.</param>
        /// <param name="arg14">The fourteenth argument to compare.</param>
        /// <param name="selector">The function to use to get the value to compare on each argument.</param>
        /// <returns>The item with the highest value.</returns>
        public static T ItemWithMax<T>(
            T arg1,
            T arg2,
            T arg3,
            T arg4,
            T arg5,
            T arg6,
            T arg7,
            T arg8,
            T arg9,
            T arg10,
            T arg11,
            T arg12,
            T arg13,
            T arg14,
            Func<T, long> selector)
        {
            ArgumentNullException.ThrowIfNull(selector);

            var arg1Number = selector(arg1);
            var arg2Number = selector(arg2);
            var arg3Number = selector(arg3);
            var arg4Number = selector(arg4);
            var arg5Number = selector(arg5);
            var arg6Number = selector(arg6);
            var arg7Number = selector(arg7);
            var arg8Number = selector(arg8);
            var arg9Number = selector(arg9);
            var arg10Number = selector(arg10);
            var arg11Number = selector(arg11);
            var arg12Number = selector(arg12);
            var arg13Number = selector(arg13);
            var arg14Number = selector(arg14);

            return ItemWithMaxInternal(
                arg1,
                arg1Number,
                arg2,
                arg2Number,
                arg3,
                arg3Number,
                arg4,
                arg4Number,
                arg5,
                arg5Number,
                arg6,
                arg6Number,
                arg7,
                arg7Number,
                arg8,
                arg8Number,
                arg9,
                arg9Number,
                arg10,
                arg10Number,
                arg11,
                arg11Number,
                arg12,
                arg12Number,
                arg13,
                arg13Number,
                arg14,
                arg14Number);
        }

        /// <summary>
        /// Gets the item that has the highest value, based on a selector.
        /// </summary>
        /// <typeparam name="T">The type being compared.</typeparam>
        /// <param name="arg1">The first argument to compare.</param>
        /// <param name="arg2">The second argument to compare.</param>
        /// <param name="arg3">The third argument to compare.</param>
        /// <param name="arg4">The forth argument to compare.</param>
        /// <param name="arg5">The fifth argument to compare.</param>
        /// <param name="arg6">The sixth argument to compare.</param>
        /// <param name="arg7">The seventh argument to compare.</param>
        /// <param name="arg8">The eighth argument to compare.</param>
        /// <param name="arg9">The ninth argument to compare.</param>
        /// <param name="arg10">The tenth argument to compare.</param>
        /// <param name="arg11">The eleventh argument to compare.</param>
        /// <param name="arg12">The twelfth argument to compare.</param>
        /// <param name="arg13">The thirteenth argument to compare.</param>
        /// <param name="arg14">The fourteenth argument to compare.</param>
        /// <param name="arg15">The fifteenth argument to compare.</param>
        /// <param name="selector">The function to use to get the value to compare on each argument.</param>
        /// <returns>The item with the highest value.</returns>
        public static T ItemWithMax<T>(
            T arg1,
            T arg2,
            T arg3,
            T arg4,
            T arg5,
            T arg6,
            T arg7,
            T arg8,
            T arg9,
            T arg10,
            T arg11,
            T arg12,
            T arg13,
            T arg14,
            T arg15,
            Func<T, long> selector)
        {
            ArgumentNullException.ThrowIfNull(selector);

            var arg1Number = selector(arg1);
            var arg2Number = selector(arg2);
            var arg3Number = selector(arg3);
            var arg4Number = selector(arg4);
            var arg5Number = selector(arg5);
            var arg6Number = selector(arg6);
            var arg7Number = selector(arg7);
            var arg8Number = selector(arg8);
            var arg9Number = selector(arg9);
            var arg10Number = selector(arg10);
            var arg11Number = selector(arg11);
            var arg12Number = selector(arg12);
            var arg13Number = selector(arg13);
            var arg14Number = selector(arg14);
            var arg15Number = selector(arg15);

            return ItemWithMaxInternal(
                arg1,
                arg1Number,
                arg2,
                arg2Number,
                arg3,
                arg3Number,
                arg4,
                arg4Number,
                arg5,
                arg5Number,
                arg6,
                arg6Number,
                arg7,
                arg7Number,
                arg8,
                arg8Number,
                arg9,
                arg9Number,
                arg10,
                arg10Number,
                arg11,
                arg11Number,
                arg12,
                arg12Number,
                arg13,
                arg13Number,
                arg14,
                arg14Number,
                arg15,
                arg15Number);
        }

        /// <summary>
        /// Gets the item that has the highest value, based on a selector.
        /// </summary>
        /// <typeparam name="T">The type being compared.</typeparam>
        /// <param name="arg1">The first argument to compare.</param>
        /// <param name="arg2">The second argument to compare.</param>
        /// <param name="arg3">The third argument to compare.</param>
        /// <param name="arg4">The forth argument to compare.</param>
        /// <param name="arg5">The fifth argument to compare.</param>
        /// <param name="arg6">The sixth argument to compare.</param>
        /// <param name="arg7">The seventh argument to compare.</param>
        /// <param name="arg8">The eighth argument to compare.</param>
        /// <param name="arg9">The ninth argument to compare.</param>
        /// <param name="arg10">The tenth argument to compare.</param>
        /// <param name="arg11">The eleventh argument to compare.</param>
        /// <param name="arg12">The twelfth argument to compare.</param>
        /// <param name="arg13">The thirteenth argument to compare.</param>
        /// <param name="arg14">The fourteenth argument to compare.</param>
        /// <param name="arg15">The fifteenth argument to compare.</param>
        /// <param name="arg16">The sixteenth argument to compare.</param>
        /// <param name="selector">The function to use to get the value to compare on each argument.</param>
        /// <returns>The item with the highest value.</returns>
        public static T ItemWithMax<T>(
            T arg1,
            T arg2,
            T arg3,
            T arg4,
            T arg5,
            T arg6,
            T arg7,
            T arg8,
            T arg9,
            T arg10,
            T arg11,
            T arg12,
            T arg13,
            T arg14,
            T arg15,
            T arg16,
            Func<T, long> selector)
        {
            ArgumentNullException.ThrowIfNull(selector);

            var arg1Number = selector(arg1);
            var arg2Number = selector(arg2);
            var arg3Number = selector(arg3);
            var arg4Number = selector(arg4);
            var arg5Number = selector(arg5);
            var arg6Number = selector(arg6);
            var arg7Number = selector(arg7);
            var arg8Number = selector(arg8);
            var arg9Number = selector(arg9);
            var arg10Number = selector(arg10);
            var arg11Number = selector(arg11);
            var arg12Number = selector(arg12);
            var arg13Number = selector(arg13);
            var arg14Number = selector(arg14);
            var arg15Number = selector(arg15);
            var arg16Number = selector(arg16);

            return ItemWithMaxInternal(
                arg1,
                arg1Number,
                arg2,
                arg2Number,
                arg3,
                arg3Number,
                arg4,
                arg4Number,
                arg5,
                arg5Number,
                arg6,
                arg6Number,
                arg7,
                arg7Number,
                arg8,
                arg8Number,
                arg9,
                arg9Number,
                arg10,
                arg10Number,
                arg11,
                arg11Number,
                arg12,
                arg12Number,
                arg13,
                arg13Number,
                arg14,
                arg14Number,
                arg15,
                arg15Number,
                arg16,
                arg16Number);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T ItemWithMaxInternal<T>(
            T arg1,
            long arg1Number,
            T arg2,
            long arg2Number)
        {
            return arg2Number > arg1Number
                ? arg2
                : arg1;
        }

#pragma warning disable S2234 // Arguments should be passed in the same order as the method parameters
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T ItemWithMaxInternal<T>(
            T arg1,
            long arg1Number,
            T arg2,
            long arg2Number,
            T arg3,
            long arg3Number)
        {
            if (arg2Number > arg1Number)
            {
                return ItemWithMaxInternal(
                    arg2,
                    arg2Number,
                    arg3,
                    arg3Number);
            }

            return ItemWithMaxInternal(
                arg1,
                arg1Number,
                arg3,
                arg3Number);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T ItemWithMaxInternal<T>(
            T arg1,
            long arg1Number,
            T arg2,
            long arg2Number,
            T arg3,
            long arg3Number,
            T arg4,
            long arg4Number)
        {
            if (arg2Number > arg1Number)
            {
                return ItemWithMaxInternal(
                    arg2,
                    arg2Number,
                    arg3,
                    arg3Number,
                    arg4,
                    arg4Number);
            }

            return ItemWithMaxInternal(
                arg1,
                arg1Number,
                arg3,
                arg3Number,
                arg4,
                arg4Number);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T ItemWithMaxInternal<T>(
            T arg1,
            long arg1Number,
            T arg2,
            long arg2Number,
            T arg3,
            long arg3Number,
            T arg4,
            long arg4Number,
            T arg5,
            long arg5Number)
        {
            if (arg2Number > arg1Number)
            {
                return ItemWithMaxInternal(
                    arg2,
                    arg2Number,
                    arg3,
                    arg3Number,
                    arg4,
                    arg4Number,
                    arg5,
                    arg5Number);
            }

            return ItemWithMaxInternal(
                arg1,
                arg1Number,
                arg3,
                arg3Number,
                arg4,
                arg4Number,
                arg5,
                arg5Number);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T ItemWithMaxInternal<T>(
            T arg1,
            long arg1Number,
            T arg2,
            long arg2Number,
            T arg3,
            long arg3Number,
            T arg4,
            long arg4Number,
            T arg5,
            long arg5Number,
            T arg6,
            long arg6Number)
        {
            if (arg2Number > arg1Number)
            {
                return ItemWithMaxInternal(
                    arg2,
                    arg2Number,
                    arg3,
                    arg3Number,
                    arg4,
                    arg4Number,
                    arg5,
                    arg5Number,
                    arg6,
                    arg6Number);
            }

            return ItemWithMaxInternal(
                arg1,
                arg1Number,
                arg3,
                arg3Number,
                arg4,
                arg4Number,
                arg5,
                arg5Number,
                arg6,
                arg6Number);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T ItemWithMaxInternal<T>(
            T arg1,
            long arg1Number,
            T arg2,
            long arg2Number,
            T arg3,
            long arg3Number,
            T arg4,
            long arg4Number,
            T arg5,
            long arg5Number,
            T arg6,
            long arg6Number,
            T arg7,
            long arg7Number)
        {
            if (arg2Number > arg1Number)
            {
                return ItemWithMaxInternal(
                    arg2,
                    arg2Number,
                    arg3,
                    arg3Number,
                    arg4,
                    arg4Number,
                    arg5,
                    arg5Number,
                    arg6,
                    arg6Number,
                    arg7,
                    arg7Number);
            }

            return ItemWithMaxInternal(
                arg1,
                arg1Number,
                arg3,
                arg3Number,
                arg4,
                arg4Number,
                arg5,
                arg5Number,
                arg6,
                arg6Number,
                arg7,
                arg7Number);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T ItemWithMaxInternal<T>(
            T arg1,
            long arg1Number,
            T arg2,
            long arg2Number,
            T arg3,
            long arg3Number,
            T arg4,
            long arg4Number,
            T arg5,
            long arg5Number,
            T arg6,
            long arg6Number,
            T arg7,
            long arg7Number,
            T arg8,
            long arg8Number)
        {
            if (arg2Number > arg1Number)
            {
                return ItemWithMaxInternal(
                    arg2,
                    arg2Number,
                    arg3,
                    arg3Number,
                    arg4,
                    arg4Number,
                    arg5,
                    arg5Number,
                    arg6,
                    arg6Number,
                    arg7,
                    arg7Number,
                    arg8,
                    arg8Number);
            }

            return ItemWithMaxInternal(
                arg1,
                arg1Number,
                arg3,
                arg3Number,
                arg4,
                arg4Number,
                arg5,
                arg5Number,
                arg6,
                arg6Number,
                arg7,
                arg7Number,
                arg8,
                arg8Number);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T ItemWithMaxInternal<T>(
            T arg1,
            long arg1Number,
            T arg2,
            long arg2Number,
            T arg3,
            long arg3Number,
            T arg4,
            long arg4Number,
            T arg5,
            long arg5Number,
            T arg6,
            long arg6Number,
            T arg7,
            long arg7Number,
            T arg8,
            long arg8Number,
            T arg9,
            long arg9Number)
        {
            if (arg2Number > arg1Number)
            {
                return ItemWithMaxInternal(
                    arg2,
                    arg2Number,
                    arg3,
                    arg3Number,
                    arg4,
                    arg4Number,
                    arg5,
                    arg5Number,
                    arg6,
                    arg6Number,
                    arg7,
                    arg7Number,
                    arg8,
                    arg8Number,
                    arg9,
                    arg9Number);
            }

            return ItemWithMaxInternal(
                arg1,
                arg1Number,
                arg3,
                arg3Number,
                arg4,
                arg4Number,
                arg5,
                arg5Number,
                arg6,
                arg6Number,
                arg7,
                arg7Number,
                arg8,
                arg8Number,
                arg9,
                arg9Number);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T ItemWithMaxInternal<T>(
            T arg1,
            long arg1Number,
            T arg2,
            long arg2Number,
            T arg3,
            long arg3Number,
            T arg4,
            long arg4Number,
            T arg5,
            long arg5Number,
            T arg6,
            long arg6Number,
            T arg7,
            long arg7Number,
            T arg8,
            long arg8Number,
            T arg9,
            long arg9Number,
            T arg10,
            long arg10Number)
        {
            if (arg2Number > arg1Number)
            {
                return ItemWithMaxInternal(
                    arg2,
                    arg2Number,
                    arg3,
                    arg3Number,
                    arg4,
                    arg4Number,
                    arg5,
                    arg5Number,
                    arg6,
                    arg6Number,
                    arg7,
                    arg7Number,
                    arg8,
                    arg8Number,
                    arg9,
                    arg9Number,
                    arg10,
                    arg10Number);
            }

            return ItemWithMaxInternal(
                arg1,
                arg1Number,
                arg3,
                arg3Number,
                arg4,
                arg4Number,
                arg5,
                arg5Number,
                arg6,
                arg6Number,
                arg7,
                arg7Number,
                arg8,
                arg8Number,
                arg9,
                arg9Number,
                arg10,
                arg10Number);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T ItemWithMaxInternal<T>(
            T arg1,
            long arg1Number,
            T arg2,
            long arg2Number,
            T arg3,
            long arg3Number,
            T arg4,
            long arg4Number,
            T arg5,
            long arg5Number,
            T arg6,
            long arg6Number,
            T arg7,
            long arg7Number,
            T arg8,
            long arg8Number,
            T arg9,
            long arg9Number,
            T arg10,
            long arg10Number,
            T arg11,
            long arg11Number)
        {
            if (arg2Number > arg1Number)
            {
                return ItemWithMaxInternal(
                    arg2,
                    arg2Number,
                    arg3,
                    arg3Number,
                    arg4,
                    arg4Number,
                    arg5,
                    arg5Number,
                    arg6,
                    arg6Number,
                    arg7,
                    arg7Number,
                    arg8,
                    arg8Number,
                    arg9,
                    arg9Number,
                    arg10,
                    arg10Number,
                    arg11,
                    arg11Number);
            }

            return ItemWithMaxInternal(
                arg1,
                arg1Number,
                arg3,
                arg3Number,
                arg4,
                arg4Number,
                arg5,
                arg5Number,
                arg6,
                arg6Number,
                arg7,
                arg7Number,
                arg8,
                arg8Number,
                arg9,
                arg9Number,
                arg10,
                arg10Number,
                arg11,
                arg11Number);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T ItemWithMaxInternal<T>(
            T arg1,
            long arg1Number,
            T arg2,
            long arg2Number,
            T arg3,
            long arg3Number,
            T arg4,
            long arg4Number,
            T arg5,
            long arg5Number,
            T arg6,
            long arg6Number,
            T arg7,
            long arg7Number,
            T arg8,
            long arg8Number,
            T arg9,
            long arg9Number,
            T arg10,
            long arg10Number,
            T arg11,
            long arg11Number,
            T arg12,
            long arg12Number)
        {
            if (arg2Number > arg1Number)
            {
                return ItemWithMaxInternal(
                    arg2,
                    arg2Number,
                    arg3,
                    arg3Number,
                    arg4,
                    arg4Number,
                    arg5,
                    arg5Number,
                    arg6,
                    arg6Number,
                    arg7,
                    arg7Number,
                    arg8,
                    arg8Number,
                    arg9,
                    arg9Number,
                    arg10,
                    arg10Number,
                    arg11,
                    arg11Number,
                    arg12,
                    arg12Number);
            }

            return ItemWithMaxInternal(
                arg1,
                arg1Number,
                arg3,
                arg3Number,
                arg4,
                arg4Number,
                arg5,
                arg5Number,
                arg6,
                arg6Number,
                arg7,
                arg7Number,
                arg8,
                arg8Number,
                arg9,
                arg9Number,
                arg10,
                arg10Number,
                arg11,
                arg11Number,
                arg12,
                arg12Number);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T ItemWithMaxInternal<T>(
            T arg1,
            long arg1Number,
            T arg2,
            long arg2Number,
            T arg3,
            long arg3Number,
            T arg4,
            long arg4Number,
            T arg5,
            long arg5Number,
            T arg6,
            long arg6Number,
            T arg7,
            long arg7Number,
            T arg8,
            long arg8Number,
            T arg9,
            long arg9Number,
            T arg10,
            long arg10Number,
            T arg11,
            long arg11Number,
            T arg12,
            long arg12Number,
            T arg13,
            long arg13Number)
        {
            if (arg2Number > arg1Number)
            {
                return ItemWithMaxInternal(
                    arg2,
                    arg2Number,
                    arg3,
                    arg3Number,
                    arg4,
                    arg4Number,
                    arg5,
                    arg5Number,
                    arg6,
                    arg6Number,
                    arg7,
                    arg7Number,
                    arg8,
                    arg8Number,
                    arg9,
                    arg9Number,
                    arg10,
                    arg10Number,
                    arg11,
                    arg11Number,
                    arg12,
                    arg12Number,
                    arg13,
                    arg13Number);
            }

            return ItemWithMaxInternal(
                arg1,
                arg1Number,
                arg3,
                arg3Number,
                arg4,
                arg4Number,
                arg5,
                arg5Number,
                arg6,
                arg6Number,
                arg7,
                arg7Number,
                arg8,
                arg8Number,
                arg9,
                arg9Number,
                arg10,
                arg10Number,
                arg11,
                arg11Number,
                arg12,
                arg12Number,
                arg13,
                arg13Number);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T ItemWithMaxInternal<T>(
            T arg1,
            long arg1Number,
            T arg2,
            long arg2Number,
            T arg3,
            long arg3Number,
            T arg4,
            long arg4Number,
            T arg5,
            long arg5Number,
            T arg6,
            long arg6Number,
            T arg7,
            long arg7Number,
            T arg8,
            long arg8Number,
            T arg9,
            long arg9Number,
            T arg10,
            long arg10Number,
            T arg11,
            long arg11Number,
            T arg12,
            long arg12Number,
            T arg13,
            long arg13Number,
            T arg14,
            long arg14Number)
        {
            if (arg2Number > arg1Number)
            {
                return ItemWithMaxInternal(
                    arg2,
                    arg2Number,
                    arg3,
                    arg3Number,
                    arg4,
                    arg4Number,
                    arg5,
                    arg5Number,
                    arg6,
                    arg6Number,
                    arg7,
                    arg7Number,
                    arg8,
                    arg8Number,
                    arg9,
                    arg9Number,
                    arg10,
                    arg10Number,
                    arg11,
                    arg11Number,
                    arg12,
                    arg12Number,
                    arg13,
                    arg13Number,
                    arg14,
                    arg14Number);
            }

            return ItemWithMaxInternal(
                arg1,
                arg1Number,
                arg3,
                arg3Number,
                arg4,
                arg4Number,
                arg5,
                arg5Number,
                arg6,
                arg6Number,
                arg7,
                arg7Number,
                arg8,
                arg8Number,
                arg9,
                arg9Number,
                arg10,
                arg10Number,
                arg11,
                arg11Number,
                arg12,
                arg12Number,
                arg13,
                arg13Number,
                arg14,
                arg14Number);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T ItemWithMaxInternal<T>(
            T arg1,
            long arg1Number,
            T arg2,
            long arg2Number,
            T arg3,
            long arg3Number,
            T arg4,
            long arg4Number,
            T arg5,
            long arg5Number,
            T arg6,
            long arg6Number,
            T arg7,
            long arg7Number,
            T arg8,
            long arg8Number,
            T arg9,
            long arg9Number,
            T arg10,
            long arg10Number,
            T arg11,
            long arg11Number,
            T arg12,
            long arg12Number,
            T arg13,
            long arg13Number,
            T arg14,
            long arg14Number,
            T arg15,
            long arg15Number)
        {
            if (arg2Number > arg1Number)
            {
                return ItemWithMaxInternal(
                    arg2,
                    arg2Number,
                    arg3,
                    arg3Number,
                    arg4,
                    arg4Number,
                    arg5,
                    arg5Number,
                    arg6,
                    arg6Number,
                    arg7,
                    arg7Number,
                    arg8,
                    arg8Number,
                    arg9,
                    arg9Number,
                    arg10,
                    arg10Number,
                    arg11,
                    arg11Number,
                    arg12,
                    arg12Number,
                    arg13,
                    arg13Number,
                    arg14,
                    arg14Number,
                    arg15,
                    arg15Number);
            }

            return ItemWithMaxInternal(
                arg1,
                arg1Number,
                arg3,
                arg3Number,
                arg4,
                arg4Number,
                arg5,
                arg5Number,
                arg6,
                arg6Number,
                arg7,
                arg7Number,
                arg8,
                arg8Number,
                arg9,
                arg9Number,
                arg10,
                arg10Number,
                arg11,
                arg11Number,
                arg12,
                arg12Number,
                arg13,
                arg13Number,
                arg14,
                arg14Number,
                arg15,
                arg15Number);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static T ItemWithMaxInternal<T>(
            T arg1,
            long arg1Number,
            T arg2,
            long arg2Number,
            T arg3,
            long arg3Number,
            T arg4,
            long arg4Number,
            T arg5,
            long arg5Number,
            T arg6,
            long arg6Number,
            T arg7,
            long arg7Number,
            T arg8,
            long arg8Number,
            T arg9,
            long arg9Number,
            T arg10,
            long arg10Number,
            T arg11,
            long arg11Number,
            T arg12,
            long arg12Number,
            T arg13,
            long arg13Number,
            T arg14,
            long arg14Number,
            T arg15,
            long arg15Number,
            T arg16,
            long arg16Number)
        {
            if (arg2Number > arg1Number)
            {
                return ItemWithMaxInternal(
                    arg2,
                    arg2Number,
                    arg3,
                    arg3Number,
                    arg4,
                    arg4Number,
                    arg5,
                    arg5Number,
                    arg6,
                    arg6Number,
                    arg7,
                    arg7Number,
                    arg8,
                    arg8Number,
                    arg9,
                    arg9Number,
                    arg10,
                    arg10Number,
                    arg11,
                    arg11Number,
                    arg12,
                    arg12Number,
                    arg13,
                    arg13Number,
                    arg14,
                    arg14Number,
                    arg15,
                    arg15Number,
                    arg16,
                    arg16Number);
            }

            return ItemWithMaxInternal(
                arg1,
                arg1Number,
                arg3,
                arg3Number,
                arg4,
                arg4Number,
                arg5,
                arg5Number,
                arg6,
                arg6Number,
                arg7,
                arg7Number,
                arg8,
                arg8Number,
                arg9,
                arg9Number,
                arg10,
                arg10Number,
                arg11,
                arg11Number,
                arg12,
                arg12Number,
                arg13,
                arg13Number,
                arg14,
                arg14Number,
                arg15,
                arg15Number,
                arg16,
                arg16Number);
        }
#pragma warning restore S2234 // Arguments should be passed in the same order as the method parameters
    }
}
