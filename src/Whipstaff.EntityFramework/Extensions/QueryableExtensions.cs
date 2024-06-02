// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Linq;
using System.Runtime.CompilerServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace Whipstaff.EntityFramework.Extensions
{
    /// <summary>
    /// Extension methods for <see cref="IQueryable{T}"/>.
    /// </summary>
    public static class QueryableExtensions
    {
        /// <summary>
        ///     Adds a tag providing the caller member name to the collection of tags associated with an EF LINQ query. Tags are query annotations
        ///     that can provide contextual tracing information at different points in the query pipeline.
        /// </summary>
        /// <remarks>
        ///     See <see href="https://aka.ms/efcore-docs-query-tags">Tagging queries in EF Core</see> for more information and examples.
        /// </remarks>
        /// <typeparam name="T">The type of entity being queried.</typeparam>
        /// <param name="source">The source query.</param>
        /// <param name="callerMemberName">The tag.</param>
        /// <param name="filePath">The file name where the method was called.</param>
        /// <param name="lineNumber">The file line number where the method was called.</param>
        /// <returns>A new query annotated with the given tag.</returns>
        public static IQueryable<T> TagWithCallerMemberAndCallSite<T>(
            this IQueryable<T> source,
            [CallerMemberName] string callerMemberName = "",
            [NotParameterized, CallerFilePath] string? filePath = null,
            [NotParameterized, CallerLineNumber] int lineNumber = 0)
        {
            // ReSharper disable ExplicitCallerInfoArgument
            return source.TagWith(callerMemberName).TagWithCallSite(
                filePath,
                lineNumber);

            // ReSharper enable ExplicitCallerInfoArgument
        }
    }
}
