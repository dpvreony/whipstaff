// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

#if CALLER_ARGUMENT_EXPRESSION_SHIM
using System;

namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// Allows capturing of the expressions passed to a method.
    /// Shims for CallerArgumentExpressionAttribute to provide compatability with NET48
    /// Taken from: https://weblogs.asp.net/dixin/csharp-10-new-feature-callerargumentexpression-argument-check-and-more on 20230904.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
    internal sealed class CallerArgumentExpressionAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CallerArgumentExpressionAttribute"/> class.
        /// </summary>
        /// <param name="parameterName">The name of the targeted parameter.</param>
        public CallerArgumentExpressionAttribute(string parameterName) => ParameterName = parameterName;

        /// <summary>
        /// Gets the target parameter name of the <c>CallerArgumentExpression</c>.
        /// </summary>
        /// <returns>
        /// The name of the targeted parameter of the <c>CallerArgumentExpression</c>.
        /// </returns>
        public string ParameterName { get; }
    }
}
#endif
