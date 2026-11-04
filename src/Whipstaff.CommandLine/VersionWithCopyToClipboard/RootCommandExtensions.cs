using System;
using System.Collections.Generic;
using System.CommandLine;
using System.CommandLine.Help;
using System.CommandLine.Invocation;
using System.Linq;
using System.Reflection;
using System.Text;
using TextCopy;

namespace Whipstaff.CommandLine.VersionWithCopyToClipboard
{
    public static class RootCommandExtensions
    {
        public static void EnableVersionWithClipboardCopy(this RootCommand rootCommand)
        {
            // Remove the built-in --version so ours can take --copy
            var options = rootCommand.Options;

            var defaultVersion = options.OfType<VersionOption>().FirstOrDefault();

            if (defaultVersion is not null)
            {
                options.Remove(defaultVersion);
            }

            var versionOpt = new Option<bool>("--version")
            {
                Description = "Show version information",
                Action = new VersionWithCopyToClipboardOptionAction()
            };

            var copyOpt = new Option<bool>("--copy") { Description = "Copy the version to the clipboard (use with --version)" };

            options.Add(versionOpt);

            options.Add(copyOpt);
        }

        public sealed class VersionWithCopyToClipboardOptionAction : SynchronousCommandLineAction
        {
            public override int Invoke(ParseResult parseResult)
            {
                var hasCopy = parseResult.GetValue<string>("--copy") != null;
                var executableVersion = GetExecutableVersion();
                parseResult.InvocationConfiguration.Output.WriteLine(executableVersion);

                if (hasCopy)
                {
                    ClipboardService.SetText(executableVersion);
                }

                return 0;
            }

            public override bool ClearsParseErrors => true;

            private static string GetExecutableVersion()
            {
                var assembly = Assembly.GetEntryAssembly() ?? Assembly.GetExecutingAssembly();

                var assemblyVersionAttribute = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();

                if (assemblyVersionAttribute is null)
                {
                    return assembly.GetName().Version?.ToString() ?? "";
                }
                else
                {
                    return assemblyVersionAttribute.InformationalVersion;
                }
            }
        }

    }
}
