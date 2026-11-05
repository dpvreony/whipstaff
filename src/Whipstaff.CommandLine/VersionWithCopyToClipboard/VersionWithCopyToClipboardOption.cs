// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.CommandLine;

namespace Whipstaff.CommandLine.VersionWithCopyToClipboard
{
    /// <summary>
    /// An option to show the version information with the ability to copy it to the clipboard.
    /// </summary>
    public sealed class VersionWithCopyToClipboardOption : Option<bool>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VersionWithCopyToClipboardOption"/> class.
        /// </summary>
        public VersionWithCopyToClipboardOption()
            : base("--version")
        {
            Description = "Show version information";
            Action = new VersionWithCopyToClipboardOptionAction();
        }
    }
}
