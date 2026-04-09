// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.Extensions.Logging;

namespace Whipstaff.YamlTemplating.DotNetTool
{
    internal static class JobEventIdFactory
    {
        internal static EventId StartingHandleCommand() => new(1, nameof(StartingHandleCommand));

        internal static EventId ReadingTemplateFile() => new(2, nameof(ReadingTemplateFile));

        internal static EventId ReadingContentFile() => new(3, nameof(ReadingContentFile));

        internal static EventId MergingYamlContent() => new(4, nameof(MergingYamlContent));

        internal static EventId WritingOutputFile() => new(5, nameof(WritingOutputFile));

        internal static EventId FailedToReadTemplate() => new(6, nameof(FailedToReadTemplate));

        internal static EventId FailedToReadContent() => new(7, nameof(FailedToReadContent));

        internal static EventId FailedToMerge() => new(8, nameof(FailedToMerge));

        internal static EventId FailedToWriteOutput() => new(9, nameof(FailedToWriteOutput));

        internal static EventId UnhandledException() => new(10, nameof(UnhandledException));

        internal static EventId ValidatingYamlPath() => new(11, nameof(ValidatingYamlPath));

        internal static EventId InvalidYamlPath() => new(12, nameof(InvalidYamlPath));

        internal static EventId InjectingAtPath() => new(13, nameof(InjectingAtPath));
    }
}
