// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Threading.Tasks;
using Whipstaff.CommandLine;

namespace Whipstaff.Testing.CommandLine
{
    /// <summary>
    /// Fake command line handler for testing.
    /// </summary>
    public sealed class FakeCommandLineHandler : ICommandLineHandler<FakeCommandLineArgModel>
    {
        /// <inheritdoc/>
        public Task<int> HandleCommand(FakeCommandLineArgModel commandLineArgModel)
        {
            return Task.FromResult(0);
        }
    }
}
