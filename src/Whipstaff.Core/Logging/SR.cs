#pragma warning disable SA1636 // File header copyright text should match

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#pragma warning restore SA1636 // File header copyright text should match
using System.Diagnostics.CodeAnalysis;
using System.Resources;

namespace System
{
    [ExcludeFromCodeCoverage]
    internal static partial class SR
    {
        internal static string Format(IFormatProvider? provider, string resourceFormat, params object?[]? args)
        {
            if (args != null)
            {
                return string.Format(provider, resourceFormat, args);
            }

            return resourceFormat;
        }
    }
}
