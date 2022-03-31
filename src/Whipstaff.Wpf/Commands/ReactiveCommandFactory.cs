// Copyright (c) 2019 dpvreony and Contributors. All rights reserved.
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
        /// <param name="directoryInfo"></param>
        /// <param name="canExecute"></param>
        /// <param name="outputScheduler"></param>
        /// <returns></returns>
        public static ReactiveCommand<Unit, Unit> GetOpenLogFileLocationCommand(
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
        /// <param name="fileName"></param>
        /// <param name="canExecute"></param>
        /// <param name="outputScheduler"></param>
        /// <returns></returns>
        public static ReactiveCommand<Unit, Unit> GetShellExecuteCommand(
            string fileName,
            IObservable<bool>? canExecute = null,
            IScheduler? outputScheduler = null)
        {
#pragma warning disable CA2000 // Dispose objects before losing scope
            return ReactiveCommand.CreateFromTask<Unit>(() => OnShellExecute(fileName), canExecute, outputScheduler);
#pragma warning restore CA2000 // Dispose objects before losing scope
        }

        private static Task<Unit> OnShellExecute(string fileName)
        {
            var processStartInfo = new ProcessStartInfo(fileName)
            {
                UseShellExecute = true
            };

            var process = Process.Start(processStartInfo);

            return Task.FromResult(Unit.Default);
        }
    }
}
