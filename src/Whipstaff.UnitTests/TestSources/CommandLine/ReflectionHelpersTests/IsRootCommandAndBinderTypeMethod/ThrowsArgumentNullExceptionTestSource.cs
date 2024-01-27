﻿// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using NetTestRegimentation.XUnit.Theories.ArgumentNullException;

namespace Whipstaff.UnitTests.TestSources.CommandLine.ReflectionHelpersTests.IsRootCommandAndBinderTypeMethod
{
    /// <summary>
    /// Test Source for <see cref="Whipstaff.UnitTests.CommandLine.ReflectionHelpersTests.IsRootCommandAndBinderTypeMethod.ThrowsArgumentNullException"/>.
    /// </summary>
    public sealed class ThrowsArgumentNullExceptionTestSource : ArgumentNullExceptionTheoryData<Type>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThrowsArgumentNullExceptionTestSource"/> class.
        /// </summary>
        public ThrowsArgumentNullExceptionTestSource()
            : base("type")
        {
        }
    }
}
