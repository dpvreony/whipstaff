// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Diagnostics;
using System.IO;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Threading.Tasks;
using ReactiveUI;

namespace Whipstaff.Wpf.Commands
{
    /// <summary>
    /// WPF specific Reactive Commands for Windows interactions.
    /// </summary>
    public static class ReactiveCommandFactory
    {
        /// <summary>
        /// Gets a command for launching the log file folder via shell execute, which will typically launch windows explorer.
        /// </summary>
        /// <param name="directoryInfo">Directory representation.</param>
        /// <param name="canExecute">Observable for whether the command can execute.</param>
        /// <param name="outputScheduler">Schedule to run the command on.</param>
        /// <returns>UI Command.</returns>
        public static ReactiveCommand<Unit, int?> GetOpenLogFileLocationCommand(
            DirectoryInfo directoryInfo,
            IObservable<bool>? canExecute = null,
            IScheduler? outputScheduler = null)
        {
            ArgumentNullException.ThrowIfNull(directoryInfo);

            return GetShellExecuteCommand(
                directoryInfo.FullName,
                canExecute,
                outputScheduler);
        }

        /// <summary>
        /// Gets a command for using shell execute for windows interactions.
        /// </summary>
        /// <param name="fileName">File name to shell execute.</param>
        /// <param name="canExecute">Observable for whether the command can execute.</param>
        /// <param name="outputScheduler">Schedule to run the command on.</param>
        /// <returns>UI Command.</returns>
        public static ReactiveCommand<Unit, int?> GetShellExecuteCommand(
            string fileName,
            IObservable<bool>? canExecute = null,
            IScheduler? outputScheduler = null)
        {
#pragma warning disable CA2000 // Dispose objects before losing scope
            return ReactiveCommand.CreateFromTask(() => OnShellExecuteAsync(fileName), canExecute, outputScheduler);
#pragma warning restore CA2000 // Dispose objects before losing scope
        }

        private static Task<int?> OnShellExecuteAsync(string fileName)
        {
            var processStartInfo = new ProcessStartInfo(fileName)
            {
                UseShellExecute = true
            };

            var process = Process.Start(processStartInfo);

            return Task.FromResult(process?.Id);
        }
    }
}
