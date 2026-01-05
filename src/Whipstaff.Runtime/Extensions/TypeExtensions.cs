// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reflection;

#if ARGUMENT_NULL_EXCEPTION_SHIM
using ArgumentNullException = Whipstaff.Runtime.Exceptions.ArgumentNullException;
#endif

namespace Whipstaff.Runtime.Extensions
{
    /// <summary>
    /// Extensions for the <see cref="Type"/> class.
    /// </summary>
    public static class TypeExtensions
    {
#if !IOS && !ANDROID && !TVOS && !MACOS
        /// <summary>
        /// Checks if a type contains a parameterless constructor.
        /// </summary>
        /// <param name="type">Type to check.</param>
        /// <returns>Whether the type contains a parameterless constructor.</returns>
        public static bool ContainsParameterlessConstructor(this Type type)
        {
            ArgumentNullException.ThrowIfNull(type);

            var constructors = type.GetConstructors();

            return Array.Exists(
                constructors,
                x => x.GetParameters().Length == 0);
        }

        /// <summary>
        /// Checks if a type contains a parameterless constructor.
        /// </summary>
        /// <param name="type">Type to check.</param>
        /// <returns>Whether the type contains a parameterless constructor.</returns>
        public static ConstructorInfo? GetParameterlessConstructor(this Type type)
        {
            ArgumentNullException.ThrowIfNull(type);

            var constructors = type.GetConstructors();

            return Array.Find(constructors, x => x.GetParameters().Length == 0);
        }
#endif

        /// <summary>
        /// Checks if a type is a public closed type.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>Whether the type is a public closed type.</returns>
        public static bool IsPublicClosedClass(this Type type)
        {
            ArgumentNullException.ThrowIfNull(type);

            if (!type.IsClass)
            {
                return false;
            }

            if (!type.IsPublic)
            {
                return false;
            }

            if (type.IsAbstract)
            {
                return false;
            }

            if (type.ContainsGenericParameters)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if a type is a closed type.
        /// </summary>
        /// <remarks>
        /// This is used where an executable is now meant to make types internal to prevent CA1515.
        /// </remarks>
        /// <param name="type">The type to check.</param>
        /// <returns>Whether the type is a closed type.</returns>
        public static bool IsClosedClass(this Type type)
        {
            ArgumentNullException.ThrowIfNull(type);

            if (!type.IsClass)
            {
                return false;
            }

            if (type.IsAbstract)
            {
                return false;
            }

            if (type.ContainsGenericParameters)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Checks if a type is a public closed type.
        /// </summary>
        /// <typeparam name="T">The type of class that the type should inherit from.</typeparam>
        /// <param name="type">The type to check.</param>
        /// <returns>Whether the type is a public closed type.</returns>
        public static bool IsPublicClosedSubclass<T>(this Type type)
            where T : class
        {
            ArgumentNullException.ThrowIfNull(type);

            return type.IsPublicClosedClass()
                   && type.IsSubclassOf(typeof(T));
        }
    }
}
