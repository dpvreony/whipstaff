// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.Extensions.Logging;

namespace Whipstaff.EntityFramework.Diagram.DotNetTool
{
    internal static class JobEventIdFactory
    {
        internal static EventId StartingHandleCommand() => new(1, nameof(StartingHandleCommand));

        internal static EventId FailedToFindDbContext() => new(2, nameof(FailedToFindDbContext));

        internal static EventId UnhandledException() => new(3, nameof(UnhandledException));
    }
}
