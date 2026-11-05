// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.CommandLine;
using System.CommandLine.Invocation;
using System.Linq;
using System.Reflection;
using TextCopy;

namespace Whipstaff.CommandLine.VersionWithCopyToClipboard
{
    /// <summary>
    /// Provides a command-line action that writes the application version to output and optionally copies it to the
    /// clipboard when the copy option is specified.
    /// </summary>
    /// <remarks>
    /// You can pass a custom function to retrieve the version string, or it will default to:
    /// * use the entry assembly, or the CALLING assembly if the entry assembly is null
    /// * if the assembly has an <see cref="AssemblyInformationalVersionAttribute"/>, use that value
    /// * otherwise, use the assembly version.
    /// </remarks>
    public sealed class VersionWithCopyToClipboardOptionAction : SynchronousCommandLineAction
    {
        private readonly Func<string> _getVersionFunc;

        /// <summary>
        /// Initializes a new instance of the <see cref="VersionWithCopyToClipboardOptionAction"/> class.
        /// </summary>
        public VersionWithCopyToClipboardOptionAction()
            : this(GetExecutableVersion)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="VersionWithCopyToClipboardOptionAction"/> class.
        /// </summary>
        /// <param name="getVersionFunc">A function that returns the version string.</param>
        public VersionWithCopyToClipboardOptionAction(Func<string> getVersionFunc)
        {
            _getVersionFunc = getVersionFunc ?? throw new ArgumentNullException(nameof(getVersionFunc));
        }

        /// <inheritdoc />
        public override bool ClearsParseErrors => true;

        /// <inheritdoc />
        public override int Invoke(ParseResult parseResult)
        {
            var hasCopy = parseResult.Tokens.Any(t => t.Value.Equals("--copy", StringComparison.OrdinalIgnoreCase));
            var executableVersion = _getVersionFunc();
            parseResult.InvocationConfiguration.Output.WriteLine(executableVersion);

            if (hasCopy)
            {
                ClipboardService.SetText(executableVersion);
            }

            return 0;
        }

        private static string GetExecutableVersion()
        {
            var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetCallingAssembly();

            var assemblyVersionAttribute = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();

            if (assemblyVersionAttribute is null)
            {
                return assembly.GetName().Version?.ToString() ?? string.Empty;
            }

            return assemblyVersionAttribute.InformationalVersion;
        }
    }
}
