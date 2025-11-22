// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Whipstaff.CommandLine;

namespace Whipstaff.Testing.CommandLine
{
    /// <summary>
    /// Fake command line handler for testing.
    /// </summary>
    public sealed class FakeCommandLineHandler : AbstractCommandLineHandler<FakeCommandLineArgModel, FakeCommandLineHandlerLogMessageActionsWrapper>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FakeCommandLineHandler"/> class.
        /// </summary>
        /// <param name="logMessageActionsWrapper">Logging framework Message Actions Wrapper instance.</param>
        public FakeCommandLineHandler(FakeCommandLineHandlerLogMessageActionsWrapper logMessageActionsWrapper)
            : base(logMessageActionsWrapper)
        {
        }

        /// <inheritdoc/>
        protected override Task<int> OnHandleCommand(FakeCommandLineArgModel commandLineArgModel, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(commandLineArgModel);

            if (commandLineArgModel.TestExceptionFunc != null)
            {
                throw commandLineArgModel.TestExceptionFunc();
            }

            return Task.FromResult(0);
        }
    }
}
