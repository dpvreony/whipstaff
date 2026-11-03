// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.IO.Abstractions;
using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Runtime.Loader;
using System.Threading;
using System.Threading.Tasks;
using Whipstaff.CommandLine;
using Whipstaff.EntityFramework.Diagram.DotNetTool.CommandLine;

namespace Whipstaff.EntityFramework.Diagram.DotNetTool
{
    /// <summary>
    /// Command line job for handling the creation of the Entity Framework Diagram.
    /// </summary>
    internal sealed class CommandLineJob : AbstractCommandLineHandler<CommandLineArgModel, CommandLineJobLogMessageActionsWrapper>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineJob"/> class.
        /// </summary>
        /// <param name="fileSystem">File System abstraction.</param>
        /// <param name="commandLineJobLogMessageActionsWrapper">Wrapper for logging framework messages.</param>
        public CommandLineJob(
            IFileSystem fileSystem,
            CommandLineJobLogMessageActionsWrapper commandLineJobLogMessageActionsWrapper)
            : base(commandLineJobLogMessageActionsWrapper)
        {
            ArgumentNullException.ThrowIfNull(fileSystem);
        }

        /// <inheritdoc/>
        protected override Task<int> OnHandleCommandAsync(CommandLineArgModel commandLineArgModel, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(ExecuteInIsolatedContext(commandLineArgModel));
        }

        private int ExecuteInIsolatedContext(CommandLineArgModel commandLineArgModel)
        {
            LogMessageActionsWrapper.StartingHandleCommand();

            var isolatedLoadContext = new IsolatedToolLoadContext(commandLineArgModel.AssemblyPath.FullName);
            var unloadTrackingReference = new WeakReference(isolatedLoadContext);

            try
            {
                var toolAssembly = isolatedLoadContext.LoadFromAssemblyPath(typeof(CommandLineJob).Assembly.Location);
                var isolatedGeneratorType = toolAssembly.GetType(typeof(IsolatedDiagramGenerator).FullName!, throwOnError: true)!;
                var generateMethod = isolatedGeneratorType.GetMethod(
                    nameof(IsolatedDiagramGenerator.Generate),
                    BindingFlags.Public | BindingFlags.Static);

                object? generationResult;
                try
                {
                    generationResult = generateMethod!.Invoke(
                        null,
                        [
                            commandLineArgModel.AssemblyPath.FullName,
                            commandLineArgModel.DbContextName,
                            commandLineArgModel.OutputFilePath.FullName,
                        ]);
                }
                catch (TargetInvocationException exception) when (exception.InnerException != null)
                {
                    ExceptionDispatchInfo.Capture(exception.InnerException).Throw();
                    throw;
                }

                return generationResult is int intResult
                    ? intResult
                    : throw new InvalidOperationException("Isolated diagram generation did not return a valid status code.");
            }
            finally
            {
                isolatedLoadContext.Unload();
                UnloadHelper.WaitForUnload(unloadTrackingReference);
            }
        }

        private static class UnloadHelper
        {
            public static void WaitForUnload(WeakReference unloadTrackingReference)
            {
                ArgumentNullException.ThrowIfNull(unloadTrackingReference);

                for (var attempt = 0; unloadTrackingReference.IsAlive && attempt < 10; attempt++)
                {
#pragma warning disable S1215 // "GC.Collect" calls should not be used
                    GC.Collect();
#pragma warning restore S1215 // "GC.Collect" calls should not be used
                    GC.WaitForPendingFinalizers();
                }
            }
        }

        private sealed class IsolatedToolLoadContext : AssemblyLoadContext
        {
            private readonly AssemblyDependencyResolver _assemblyDependencyResolver;

            public IsolatedToolLoadContext(string rootAssemblyPath)
                : base(isCollectible: true)
            {
                _assemblyDependencyResolver = new AssemblyDependencyResolver(rootAssemblyPath);
            }

            protected override Assembly? Load(AssemblyName assemblyName)
            {
                var assemblyPath = _assemblyDependencyResolver.ResolveAssemblyToPath(assemblyName);
                if (assemblyPath == null)
                {
                    return null;
                }

                return LoadFromAssemblyPath(assemblyPath);
            }

            protected override IntPtr LoadUnmanagedDll(string unmanagedDllName)
            {
                var unmanagedDllPath = _assemblyDependencyResolver.ResolveUnmanagedDllToPath(unmanagedDllName);
                if (string.IsNullOrWhiteSpace(unmanagedDllPath))
                {
                    return IntPtr.Zero;
                }

                return LoadUnmanagedDllFromPath(unmanagedDllPath);
            }
        }
    }
}
