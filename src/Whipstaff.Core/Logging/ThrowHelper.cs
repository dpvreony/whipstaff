#pragma warning disable SA1636 // File header copyright text should match

// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#pragma warning restore SA1636 // File header copyright text should match
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

// This file is intended to be used by components that don't have access to ArgumentNullException.ThrowIfNull.
#pragma warning disable CS0436 // Type conflicts with imported type

namespace System
{
    [ExcludeFromCodeCoverage]
    internal static partial class ThrowHelper
    {
        /// <summary>Throws an <see cref="ArgumentNullException"/> if <paramref name="argument"/> is null.</summary>
        /// <param name="argument">The reference type argument to validate as non-null.</param>
        /// <param name="paramName">The name of the parameter with which <paramref name="argument"/> corresponds.</param>
        internal static void ThrowIfNull(
#if NETCOREAPP3_0_OR_GREATER
            [NotNull]
#endif
            object? argument,
            [CallerArgumentExpression("argument")] string? paramName = null)
        {
            if (argument is null)
            {
                Throw(paramName);
            }
        }

#if NETCOREAPP3_0_OR_GREATER
        [DoesNotReturn]
#endif
        private static void Throw(string? paramName) => throw new ArgumentNullException(paramName);
    }
}

#if !NETCOREAPP3_0_OR_GREATER
#pragma warning disable SA1403 // File may only contain a single namespace
namespace System.Runtime.CompilerServices
#pragma warning restore SA1403 // File may only contain a single namespace
{
    [ExcludeFromCodeCoverage]
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
#pragma warning disable SA1402 // File may only contain a single type
    internal sealed class CallerArgumentExpressionAttribute : Attribute
#pragma warning restore SA1402 // File may only contain a single type
    {
        public CallerArgumentExpressionAttribute(string parameterName)
        {
            ParameterName = parameterName;
        }

        public string ParameterName { get; }
    }
}
#endif
