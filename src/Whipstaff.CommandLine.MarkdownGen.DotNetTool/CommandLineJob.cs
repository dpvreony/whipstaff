// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.CommandLine;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Whipstaff.CommandLine.MarkdownGen.DotNetTool.CommandLine;

namespace Whipstaff.CommandLine.MarkdownGen.DotNetTool
{
    /// <summary>
    /// Command line job for handling the creation of the Entity Framework Diagram.
    /// </summary>
    public sealed class CommandLineJob : ICommandLineHandler<CommandLineArgModel>
    {
        private readonly CommandLineJobLogMessageActionsWrapper _commandLineJobLogMessageActionsWrapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineJob"/> class.
        /// </summary>
        /// <param name="commandLineJobLogMessageActionsWrapper">Wrapper for logging framework messages.</param>
        public CommandLineJob(CommandLineJobLogMessageActionsWrapper commandLineJobLogMessageActionsWrapper)
        {
            ArgumentNullException.ThrowIfNull(commandLineJobLogMessageActionsWrapper);

            _commandLineJobLogMessageActionsWrapper = commandLineJobLogMessageActionsWrapper;
        }

        /// <inheritdoc/>
        public Task<int> HandleCommand(CommandLineArgModel commandLineArgModel)
        {
            return Task.Run(() =>
            {
                _commandLineJobLogMessageActionsWrapper.StartingHandleCommand();
                var assembly = Assembly.LoadFrom(commandLineArgModel.AssemblyPath.FullName);
                var outputFilePath = commandLineArgModel.OutputFilePath;

                var rootCommand = GetRootCommand(assembly);

                return 0;
            });
        }

        private static bool IsRootCommandAndBinderType(Type type)
        {
            if (!type.IsClass)
            {
                return false;
            }

            if (!type.IsPublic)
            {
                return false;
            }

            if (type.IsAbstract)
            {
                return false;
            }

            if (type.ContainsGenericParameters)
            {
                return false;
            }

            var matchingInterface = type.GetInterface("IRootCommandAndBinderFactory`1");
            if (matchingInterface == null)
            {
                return false;
            }

            return true;
        }

        private static RootCommand? GetRootCommand(Assembly assembly)
        {
            var allTypes = assembly.GetTypes();

            // ReSharper disable once ConvertClosureToMethodGroup
            var matchingType = allTypes.AsParallel().FirstOrDefault(type => IsRootCommandAndBinderType(type));
            if (matchingType == null)
            {
                return null;
            }

            var instance = Activator.CreateInstance(matchingType);
            if (instance == null)
            {
                return null;
            }

            var rootCommandProperty = instance.GetType().GetProperty("RootCommand");
            var accessor = rootCommandProperty!.GetGetMethod();
            var rootCommand = accessor!.Invoke(instance, null);

            // ReSharper disable once MergeConditionalExpression
            return rootCommand != null ? (RootCommand)rootCommand : null;
        }
    }
}
