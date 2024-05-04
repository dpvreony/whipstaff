﻿// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.CommandLine;

#if ARGUMENT_NULL_EXCEPTION_SHIM
using ArgumentNullException = Whipstaff.Runtime.Exceptions.ArgumentNullException;
#endif

namespace Whipstaff.CommandLine
{
    /// <summary>
    /// Extensions for Command manipulation.
    /// </summary>
    public static class CommandExtensions
    {
        /// <summary>
        /// Make two options exclusive.
        /// </summary>
        /// <param name="command">Command object to modify.</param>
        /// <param name="option1">First option.</param>
        /// <param name="option2">Second option.</param>
        public static void MakeOptionsExclusive(
            this Command command,
            Option option1,
            Option option2)
        {
            ArgumentNullException.ThrowIfNull(command);
            ArgumentNullException.ThrowIfNull(option1);
            ArgumentNullException.ThrowIfNull(option2);

            command.AddValidator(result =>
            {
                if (result.FindResultFor(option1) is not null &&
                    result.FindResultFor(option2) is not null)
                {
                    result.ErrorMessage = $"You cannot use options {option1.Name} and {option2.Name} together";
                }
            });
        }
    }
}
