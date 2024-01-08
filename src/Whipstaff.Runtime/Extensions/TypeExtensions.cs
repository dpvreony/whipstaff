// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Linq;
using System.Reflection;

namespace Whipstaff.Runtime.Extensions
{
    /// <summary>
    /// Extensions for the <see cref="Type"/> class.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Checks if a type contains a parameterless constructor.
        /// </summary>
        /// <param name="type">Type to check.</param>
        /// <returns>Whether the type contains a parameterless constructor.</returns>
        public static bool ContainsParameterlessConstructor(this Type type)
        {
            var constructors = type.GetConstructors();

            return constructors.Any(x => x.GetParameters().Length == 0);
        }

        /// <summary>
        /// Checks if a type contains a parameterless constructor.
        /// </summary>
        /// <param name="type">Type to check.</param>
        /// <returns>Whether the type contains a parameterless constructor.</returns>
        public static ConstructorInfo? GetParameterlessConstructor(this Type type)
        {
            var constructors = type.GetConstructors();

            return constructors.FirstOrDefault(x => x.GetParameters().Length == 0);
        }
    }
}
