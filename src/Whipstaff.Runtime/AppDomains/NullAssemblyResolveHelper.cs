// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reflection;

namespace Whipstaff.Runtime.AppDomains
{
    /// <summary>
    /// Null implementation of <see cref="IAssemblyResolveHelper"/>. This can be used when no assembly resolution is required, but an instance of <see cref="IAssemblyResolveHelper"/> is required for construction of an object.
    /// </summary>
    public sealed class NullAssemblyResolveHelper : IAssemblyResolveHelper
    {
        /// <inheritdoc/>
        public Assembly? OnAssemblyResolve(object? sender, ResolveEventArgs args) => null;
    }
}
