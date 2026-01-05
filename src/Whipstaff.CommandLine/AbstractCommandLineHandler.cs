// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Whipstaff.Core.Logging;
using Whipstaff.Core.Logging.MessageActionWrappers;

#if ARGUMENT_NULL_EXCEPTION_SHIM
using ArgumentNullException = Whipstaff.Runtime.Exceptions.ArgumentNullException;
#endif

namespace Whipstaff.CommandLine
{
    /// <summary>
    /// Abstract base class for handling command line jobs. This class will catch unhandled exceptions and log them to
    /// reduce the length of the callstack in comparison to the global catch logic inside System.Commandline itself
    /// that includes a lot of calls developers will have no interest in.
    /// </summary>
    /// <typeparam name="TCommandLineArgModel">Type representing the command line arguments model.</typeparam>
    /// <typeparam name="TLogMessageActionsWrapper">Type representing the Log Message Actions Wrapper.</typeparam>
    public abstract class AbstractCommandLineHandler<TCommandLineArgModel, TLogMessageActionsWrapper> : ICommandLineHandler<TCommandLineArgModel>
        where TLogMessageActionsWrapper : IWrapLogMessageActionUnhandledException, ILogMessageActionsWrapper<AbstractCommandLineHandler<TCommandLineArgModel, TLogMessageActionsWrapper>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AbstractCommandLineHandler{TCommandLineArgModel, TLogMessageActionsWrapper}"/> class.
        /// </summary>
        /// <param name="logMessageActionsWrapper">Logging framework Message Actions Wrapper instance.</param>
        protected AbstractCommandLineHandler(TLogMessageActionsWrapper logMessageActionsWrapper)
        {
            ArgumentNullException.ThrowIfNull(logMessageActionsWrapper);

            LogMessageActionsWrapper = logMessageActionsWrapper;
        }

        /// <summary>
        /// Gets the logging framework Message Actions Wrapper instance.
        /// </summary>
        protected TLogMessageActionsWrapper LogMessageActionsWrapper { get; }

        /// <inheritdoc/>
        public System.Threading.Tasks.Task<int> HandleCommandAsync(TCommandLineArgModel commandLineArgModel, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(commandLineArgModel);
            try
            {
                return OnHandleCommand(commandLineArgModel, cancellationToken);
            }
            catch (Exception e)
            {
                LogMessageActionsWrapper.UnhandledException(e);
                return Task.FromResult(int.MaxValue);
            }
        }

        /// <summary>
        /// Handles the execution of the command line job.
        /// </summary>
        /// <param name="commandLineArgModel">Command Line Arguments model.</param>
        /// <param name="cancellationToken">The cancellation token for the operation.</param>
        /// <returns>0 for success, non-zero for error.</returns>
        protected abstract Task<int> OnHandleCommand(TCommandLineArgModel commandLineArgModel, CancellationToken cancellationToken);
    }
}
