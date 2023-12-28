// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

#if IS_EXTERNAL_INIT_SHIM
using System.ComponentModel;

namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// Enables support for C# 9/10 records on older frameworks.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    internal static class IsExternalInit
    {
    }
}
#endif
