// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reflection;

namespace Whipstaff.Runtime.AppDomains
{
    /// <summary>
    /// Helper for dealing with unresolved assemblies triggered by <see cref="System.AppDomain.AssemblyResolve"/>.
    /// </summary>
    public interface IAssemblyResolveHelper
    {
        /// <summary>
        /// Handles the <see cref="System.AppDomain.AssemblyResolve"/> event.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="args">The event data.</param>
        /// <returns>The assembly that resolves the type, assembly, or resource; or null if the assembly cannot be resolved.</returns>
#pragma warning disable GR0033
        Assembly? OnAssemblyResolve(object? sender, ResolveEventArgs args);
#pragma warning restore GR0033
    }
}
