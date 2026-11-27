// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.CommandLine;
using System.Linq;
using TextCopy;

namespace Whipstaff.CommandLine.VersionWithCopyToClipboard
{
    /// <summary>
    /// Extension Methods for <see cref="RootCommand"/>.
    /// </summary>
    public static class RootCommandExtensions
    {
        /// <summary>
        /// Enable the version option with the ability to copy to clipboard.
        /// </summary>
        /// <param name="rootCommand">The root command to enable the version option on.</param>
        /// <param name="clipboard">The clipboard instance to use for copying version information.</param>
        public static void EnableVersionWithClipboardCopy(this RootCommand rootCommand, IClipboard clipboard)
        {
            // Remove the built-in --version so ours can take --copy
            var options = rootCommand.Options;

            var defaultVersion = options.OfType<VersionOption>().FirstOrDefault();

            if (defaultVersion is not null)
            {
                _ = options.Remove(defaultVersion);
            }

            if (options.OfType<VersionWithCopyToClipboardOption>().Any())
            {
                return;
            }

            var versionOpt = new VersionWithCopyToClipboardOption(clipboard);
            var copyOpt = new VersionCopyOption();

            options.Add(versionOpt);

            options.Add(copyOpt);
        }
    }
}
