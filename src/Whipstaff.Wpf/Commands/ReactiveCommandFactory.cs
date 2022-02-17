using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive;
using System.Reactive.Concurrency;
using System.Text;
using System.Threading.Tasks;
using ReactiveUI;

namespace Whipstaff.Wpf.Commands
{
    public static class ReactiveCommandFactory
    {
        public static ReactiveCommand<Unit> GetOpenLogFileLocationCommand()
        {

        }

        public static ReactiveCommand<Unit> GetShellExecuteCommand(
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
