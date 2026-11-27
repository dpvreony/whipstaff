// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.CommandLine;
using System.CommandLine.Invocation;

namespace Whipstaff.CommandLine.VersionWithCopyToClipboard
{
    /// <summary>
    /// Represents the `--copy` command-line option that copies version information to the clipboard.
    /// </summary>
    /// <remarks>This option is a zero-arity switch and does not accept a value.</remarks>
    public sealed class VersionCopyOption : Option<bool>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="VersionCopyOption"/> class.
        /// </summary>
        public VersionCopyOption()
            : base("--copy", [])
        {
            Description = "Show version information to the clipboard";
            Arity = ArgumentArity.Zero;
        }

        /// <inheritdoc/>
        public override CommandLineAction? Action => null;

        /// <inheritdoc/>
        public override Type ValueType => typeof(void);
    }
}
