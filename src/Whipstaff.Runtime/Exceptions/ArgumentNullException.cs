// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

#if NET48 || NETSTANDARD2_1
using System.Runtime.CompilerServices;
using Whipstaff.Runtime.CompilerServices;

namespace Whipstaff.Runtime.Exceptions
{
    /// <summary>
    /// Shims for ArgumentNullException to provide compatability with NET48.
    /// </summary>
    public static class ArgumentNullException
    {
        /// <summary>Throws an <see cref="T:System.ArgumentNullException" /> if <paramref name="argument" /> is <see langword="null" />.</summary>
        /// <param name="argument">The reference type argument to validate as non-null.</param>
        /// <param name="paramName">The name of the parameter with which <paramref name="argument" /> corresponds.</param>
        /// <exception cref="T:System.ArgumentNullException">
        /// <paramref name="argument" /> is <see langword="null" />.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfNull(object? argument, [CallerArgumentExpression("argument")] string? paramName = null)
        {
            if (argument != null)
            {
                return;
            }

            throw new global::System.ArgumentNullException(paramName);
        }
    }
}
#endif
