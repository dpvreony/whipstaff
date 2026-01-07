// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reflection.Emit;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using NuGet.Common;
using NuGet.Protocol.Plugins;
using static System.Runtime.InteropServices.JavaScript.JSType;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Whipstaff.Nuget
{
    /// <summary>
    /// Nuget logger that forwards to a .NET Core logger.
    /// </summary>
    /// <remarks>
    /// This is based upon https://raw.githubusercontent.com/NuGet/NuGet.Jobs/943076ea59f3bad50b019c27e511bf82808575aa/src/NuGet.Services.Metadata.Catalog.Monitoring/Utility/CommonLogger.cs
    /// Copyright (c) .NET Foundation. All rights reserved.
    /// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
    /// </remarks>
    public sealed class NugetForwardingToNetCoreLogger : NuGet.Common.ILogger
    {
        // This event ID is believed to be unused anywhere else but is otherwise arbitrary.
        private const int DefaultLogEventId = 23847;
        private static readonly EventId _defaultClientLogEvent = new EventId(DefaultLogEventId);
        private readonly Microsoft.Extensions.Logging.ILogger _internalLogger;

        /// <summary>
        /// Initializes a new instance of the <see cref="NugetForwardingToNetCoreLogger"/> class.
        /// </summary>
        /// <param name="logger">Net Core logging framework instance.</param>
#pragma warning disable GR0027 // Constructor should have a logging framework instance as the final parameter.
        public NugetForwardingToNetCoreLogger(Microsoft.Extensions.Logging.ILogger logger)
        {
            _internalLogger = logger ?? throw new ArgumentNullException(nameof(logger));
        }
#pragma warning restore GR0027 // Constructor should have a logging framework instance as the final parameter.

        /// <inheritdoc/>
        public void Log(NuGet.Common.LogLevel level, string data)
        {
            var internalLevel = GetLogLevel(level);

            if (_internalLogger.IsEnabled(internalLevel))
            {
                _internalLogger.Log(internalLevel, _defaultClientLogEvent, data, null, (str, ex) => str);
            }
        }

        /// <inheritdoc/>
        public void Log(ILogMessage message)
        {
            if (message == null)
            {
                return;
            }

            var internalLevel = GetLogLevel(message.Level);

            if (_internalLogger.IsEnabled(internalLevel))
            {
                _internalLogger.Log(internalLevel, new EventId((int)message.Code), message.Message, null, (str, ex) => str);
            }
        }

        /// <inheritdoc/>
        public Task LogAsync(NuGet.Common.LogLevel level, string data)
        {
            var internalLevel = GetLogLevel(level);

            if (_internalLogger.IsEnabled(internalLevel))
            {
                _internalLogger.Log(internalLevel, _defaultClientLogEvent, data, null, (str, _) => str);
            }

            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public Task LogAsync(ILogMessage message)
        {
            if (message == null)
            {
                return Task.CompletedTask;
            }

            var internalLevel = GetLogLevel(message.Level);

            if (_internalLogger.IsEnabled(internalLevel))
            {
                _internalLogger.Log(internalLevel, new EventId((int)message.Code), message.Message, null, (str, ex) => str);
            }

            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public void LogDebug(string data)
        {
            if (!_internalLogger.IsEnabled(LogLevel.Debug))
            {
                return;
            }

#pragma warning disable CA1848 // Use the LoggerMessage delegates
            _internalLogger.LogDebug("{Data}", data);
#pragma warning restore CA1848 // Use the LoggerMessage delegates
        }

        /// <inheritdoc/>
        public void LogVerbose(string data)
        {
            if (!_internalLogger.IsEnabled(LogLevel.Trace))
            {
                return;
            }

#pragma warning disable CA1848 // Use the LoggerMessage delegates
            _internalLogger.LogTrace("{Data}", data);
#pragma warning restore CA1848 // Use the LoggerMessage delegates
        }

        /// <inheritdoc/>
        public void LogInformation(string data)
        {
            if (!_internalLogger.IsEnabled(LogLevel.Information))
            {
                return;
            }

#pragma warning disable CA1848 // Use the LoggerMessage delegates
            _internalLogger.LogInformation("{Data}", data);
#pragma warning restore CA1848 // Use the LoggerMessage delegates
        }

        /// <inheritdoc/>
        public void LogMinimal(string data)
        {
            LogInformation(data);
        }

        /// <inheritdoc/>
        public void LogWarning(string data)
        {
            if (!_internalLogger.IsEnabled(LogLevel.Warning))
            {
                return;
            }

#pragma warning disable CA1848 // Use the LoggerMessage delegates
            _internalLogger.LogWarning("{Data}", data);
#pragma warning restore CA1848 // Use the LoggerMessage delegates
        }

        /// <inheritdoc/>
        public void LogError(string data)
        {
            if (!_internalLogger.IsEnabled(LogLevel.Error))
            {
                return;
            }

#pragma warning disable CA1848 // Use the LoggerMessage delegates
            _internalLogger.LogError("{Data}", data);
#pragma warning restore CA1848 // Use the LoggerMessage delegates
        }

        /// <inheritdoc/>
        public void LogInformationSummary(string data)
        {
            LogInformation(data);
        }

        private static Microsoft.Extensions.Logging.LogLevel GetLogLevel(NuGet.Common.LogLevel logLevel)
        {
            return logLevel switch
            {
                NuGet.Common.LogLevel.Debug => Microsoft.Extensions.Logging.LogLevel.Debug,
                NuGet.Common.LogLevel.Verbose => Microsoft.Extensions.Logging.LogLevel.Information,
                NuGet.Common.LogLevel.Information => Microsoft.Extensions.Logging.LogLevel.Information,
                NuGet.Common.LogLevel.Minimal => Microsoft.Extensions.Logging.LogLevel.Information,
                NuGet.Common.LogLevel.Warning => Microsoft.Extensions.Logging.LogLevel.Warning,
                NuGet.Common.LogLevel.Error => Microsoft.Extensions.Logging.LogLevel.Error,
                _ => Microsoft.Extensions.Logging.LogLevel.None,
            };
        }
    }
}
