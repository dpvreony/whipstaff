// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Whipstaff.Core.Logging
{
    /// <summary>
    /// Interface for identifying a class as a log message actions host.
    /// There is no implementation as the intention is a marker interface for Nucleotide Source Code Generation.
    /// </summary>
    /// <typeparam name="TCategoryName">The type whose name is used for the logger category name.</typeparam>
#pragma warning disable S2326
    public interface ILogMessageActions<out TCategoryName>
#pragma warning restore S2326
    {
    }
}
