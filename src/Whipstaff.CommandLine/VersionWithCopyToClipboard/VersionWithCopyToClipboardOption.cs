// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.CommandLine.Parsing;
using System.Linq;

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
            : base("--version", [])
        {
            Description = "Show version information";
            AddValidators();
            Arity = ArgumentArity.Zero;
        }

        /// <inheritdoc />
        public override CommandLineAction? Action => new VersionWithCopyToClipboardOptionAction();

        /// <inheritdoc />
        public override Type ValueType => typeof(void);

        private void AddValidators()
        {
            Validators.Add(static result =>
            {
                if (result.Parent is CommandResult parent &&
                    parent.Children.Any(r =>
                        r is not OptionResult { Option: VersionWithCopyToClipboardOption } &&
                        r is not OptionResult { Option: VersionCopyOption } &&
                        r is not OptionResult { Implicit: true }))
                {
                    result.AddError("Version option can only be combined with --copy. It can not be used with other options.");
                }
            });
        }
    }
}
