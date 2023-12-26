// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.CommandLine;

namespace Whipstaff.CommandLine
{
    /// <summary>
    /// Model that represents the root command and command line binder logic.
    /// </summary>
    /// <typeparam name="TCommandLineBinder">The type for the command line binding logic.</typeparam>
    /// <remarks>
    /// Initializes a new instance of the <see cref="RootCommandAndBinderModel{TCommandLineBinder}"/> class.
    /// </remarks>
    /// <param name="RootCommand">The root command.</param>
    /// <param name="CommandLineBinder">The command line binding logic.</param>
    public sealed record RootCommandAndBinderModel<TCommandLineBinder>(RootCommand RootCommand, TCommandLineBinder CommandLineBinder)
    {
    }
}
