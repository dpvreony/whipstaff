// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.CommandLine;

#if ARGUMENT_NULL_EXCEPTION_SHIM
using ArgumentNullException = Whipstaff.Runtime.Exceptions.ArgumentNullException;
#endif

namespace Whipstaff.CommandLine
{
    /// <summary>
    /// A base class for command line binders that convert parsed command line arguments into a specific type.
    /// </summary>
    /// <typeparam name="T">The type for the command line model to bind to.</typeparam>
    public interface IBinderBase<out T>
    {
        /// <summary>
        /// Gets the bound value from the parse result.
        /// </summary>
        /// <param name="parseResult">The parse result to bind from.</param>
        /// <returns>Bound command line model.</returns>
        T GetBoundValue(ParseResult parseResult);
    }
}
