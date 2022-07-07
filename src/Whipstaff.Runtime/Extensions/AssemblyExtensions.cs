// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Whipstaff.Runtime.Exceptions;

namespace Whipstaff.Runtime.Extensions
{
    /// <summary>
    /// Assembly Extensions.
    /// </summary>
    public static class AssemblyExtensions
    {
        /// <summary>
        /// The get classes in namespace.
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
            if (string.IsNullOrWhiteSpace(fullyQualifiedNamespace))
            {
                throw new ArgumentNullException(nameof(fullyQualifiedNamespace));
            }

            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            return assembly.GetTypes()
                .AsParallel()
                .Where(
                    type =>
                        type.Namespace != null
                        && type.Namespace.Equals(fullyQualifiedNamespace, StringComparison.Ordinal))
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
            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            if (string.IsNullOrWhiteSpace(resourceNamespace))
            {
                throw new ArgumentNullException(nameof(resourceNamespace));
            }

            if (string.IsNullOrWhiteSpace(resourceFileName))
            {
                throw new ArgumentNullException(nameof(resourceFileName));
            }

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
    }
}
