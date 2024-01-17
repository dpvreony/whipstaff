// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.CommandLine;
using System.Linq;
using System.Reflection;
using Whipstaff.Runtime.Extensions;

namespace Whipstaff.CommandLine
{
    /// <summary>
    /// Reflection helpers for the command line objects.
    /// </summary>
    public static class ReflectionHelpers
    {
        /// <summary>
        /// Determines if the type is a root command and binder type.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns>Whether the type is a root command and binder type.</returns>
        public static bool IsRootCommandAndBinderType(Type type)
        {
            if (!type.IsPublicClosedClass())
            {
                return false;
            }

            var matchingInterface = type.GetInterface("IRootCommandAndBinderFactory`1");
            if (matchingInterface == null)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Gets the root command from the assembly.
        /// </summary>
        /// <param name="assembly">The assembly to scan.</param>
        /// <returns>The discovered root command, if any.</returns>
        public static RootCommand? GetRootCommand(Assembly assembly)
        {
            var allTypes = assembly.GetTypes();

            // ReSharper disable once ConvertClosureToMethodGroup
            var matchingType = allTypes.AsParallel().FirstOrDefault(type => IsRootCommandAndBinderType(type));
            if (matchingType == null)
            {
                return null;
            }

            var instance = Activator.CreateInstance(matchingType);
            if (instance == null)
            {
                return null;
            }

            var rootCommandProperty = instance.GetType().GetProperty("RootCommand");
            var accessor = rootCommandProperty!.GetGetMethod();
            var rootCommand = accessor!.Invoke(instance, null);

            // ReSharper disable once MergeConditionalExpression
            return rootCommand != null ? (RootCommand)rootCommand : null;
        }
    }
}
