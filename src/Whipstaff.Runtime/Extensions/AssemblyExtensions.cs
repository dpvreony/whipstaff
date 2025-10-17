// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Whipstaff.Runtime.Exceptions;

#if ARGUMENT_NULL_EXCEPTION_SHIM
using ArgumentNullException = Whipstaff.Runtime.Exceptions.ArgumentNullException;
#endif

namespace Whipstaff.Runtime.Extensions
{
    /// <summary>
    /// Extensions methods for reflecting on an assembly.
    /// </summary>
    public static class AssemblyExtensions
    {
        /// <summary>
        /// Gets all classes in an assembly.
        /// </summary>
        /// <param name="assembly">
        /// The assembly to search.
        /// </param>
        /// <param name="fullyQualifiedNamespace">
        /// The fully qualified namespace to look in.
        /// </param>
        /// <returns>
        /// List of classes.
        /// </returns>
        public static Type[] GetClassesInNamespace(this Assembly assembly, string fullyQualifiedNamespace)
        {
            ArgumentNullException.ThrowIfNull(assembly);
            ArgumentNullException.ThrowIfNull(fullyQualifiedNamespace);

            return assembly.GetTypes()
                .AsParallel()
                .Where(type => type.Namespace?.Equals(fullyQualifiedNamespace, StringComparison.Ordinal) == true)
                .ToArray();
        }

        /// <summary>
        /// Loads a string from an embedded resource file.
        /// </summary>
        /// <param name="assembly">
        /// The assembly containing the resource.
        /// </param>
        /// <param name="resourceNamespace">
        /// Namespace where the resource file resides. Usually the application name.
        /// </param>
        /// <param name="resourceFileName">
        /// The filename of the embedded resource.
        /// </param>
        /// <returns>
        /// string from embedded resource file.
        /// </returns>
        /// <remarks>
        /// This code is based on an answer on stack overflow
        /// http://stackoverflow.com/questions/576571/where-do-you-put-sql-statements-in-your-c-sharp-projects
        /// answer by: http://stackoverflow.com/users/13302/marc-s.
        /// </remarks>
        public static string LoadStringFromResource(
            this Assembly assembly,
            string resourceNamespace,
            string resourceFileName)
        {
            ArgumentNullException.ThrowIfNull(assembly);
            ArgumentNullException.ThrowIfNull(resourceNamespace);
            ArgumentNullException.ThrowIfNull(resourceFileName);

            string result;
            var resourceName = resourceNamespace + "." + resourceFileName;
            using (var resourceStream = assembly.GetManifestResourceStream(resourceName))
            {
                if (resourceStream == null)
                {
                    throw new FailedToGetResourceStreamException(resourceName);
                }

                using (var streamReader = new StreamReader(resourceStream))
                {
                    result = streamReader.ReadToEnd();
                }
            }

            return result;
        }

        /// <summary>
        /// Loads a byte from an embedded resource file.
        /// </summary>
        /// <param name="assembly">
        /// The assembly containing the resource.
        /// </param>
        /// <param name="resourceNamespace">
        /// Namespace where the resource file resides. Usually the application name.
        /// </param>
        /// <param name="resourceFileName">
        /// The filename of the embedded resource.
        /// </param>
        /// <returns>
        /// string from embedded resource file.
        /// </returns>
        public static byte[] GetManifestResourceStreamAsByteArray(
            this Assembly assembly,
            string resourceNamespace,
            string resourceFileName)
        {
            ArgumentNullException.ThrowIfNull(assembly);

            return assembly.GetManifestResourceStreamAsByteArray($"{resourceNamespace}.{resourceFileName}");
        }

        /// <summary>
        /// Loads a byte from an embedded resource file.
        /// </summary>
        /// <param name="assembly">
        /// The assembly containing the resource.
        /// </param>
        /// <param name="fullyQualifiedResourceFileName">
        /// Fully qualified namespace where the resource file resides.
        /// </param>
        /// <returns>
        /// string from embedded resource file.
        /// </returns>
        public static byte[] GetManifestResourceStreamAsByteArray(
            this Assembly assembly,
            string fullyQualifiedResourceFileName)
        {
            ArgumentNullException.ThrowIfNull(assembly);

            using (var stream = assembly.GetManifestResourceStream(fullyQualifiedResourceFileName))
            {
                if (stream == null)
                {
                    throw new FailedToGetResourceStreamException(fullyQualifiedResourceFileName);
                }

                return stream.ToByteArray();
            }
        }
    }
}
