// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using NetTestRegimentation.XUnit.Theories.ArgumentNullException;
using Whipstaff.Testing.CommandLine;

namespace Whipstaff.UnitTests.TestSources.CommandLine.AbstractCommandLineHandlerTests.HandleCommand
{
    /// <summary>
    /// Test Source for <see cref="ArgumentNullException"/> test scenario of <see cref="Whipstaff.CommandLine.AbstractCommandLineHandler{TCommandLineArgModel, TLogMessageActionsWrapper}.HandleCommandAsync"/>.
    /// </summary>
    public sealed class ThrowsArgumentNullExceptionAsyncTestSource : ArgumentNullExceptionTheoryData<FakeCommandLineArgModel>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThrowsArgumentNullExceptionAsyncTestSource"/> class.
        /// </summary>
        public ThrowsArgumentNullExceptionAsyncTestSource()
            : base("commandLineArgModel")
        {
        }
    }
}
