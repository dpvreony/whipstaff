// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;

namespace Whipstaff.Runtime.HostOverride
{
    /// <summary>
    /// An in memory implementation of a host override.
    /// </summary>
    public sealed class InMemoryHostOverride : IHostOverride
    {
        private readonly IDictionary<string, string> _mappings;
        private readonly ILogger<InMemoryHostOverride> _logger;
        private readonly Action<ILogger, string, Exception?> _logNoOverrideFoundForHost;
        private readonly Action<ILogger, string, string, Exception?> _logFoundOverrideFoundForHost;

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryHostOverride"/> class.
        /// </summary>
        /// <param name="mappings">Dictionary of mappings.</param>
        /// <param name="logger">Logging Framework instance.</param>
        public InMemoryHostOverride(IDictionary<string, string> mappings, ILogger<InMemoryHostOverride> logger)
        {
            _logNoOverrideFoundForHost =
                LoggerMessage.Define<string>(LogLevel.Debug, new EventId(1), "No override found for host \"{Host}\"");
            _logFoundOverrideFoundForHost =
                LoggerMessage.Define<string, string>(LogLevel.Debug, new EventId(1), "Found for host \"{Host}\" to \"{Value}\"");
            _mappings = mappings ?? throw new ArgumentNullException(nameof(mappings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>
        public string? Resolve(string host)
        {
            if (string.IsNullOrWhiteSpace(host))
            {
                throw new ArgumentNullException(nameof(host));
            }

            var value = _mappings.FirstOrDefault(x => x.Key.Equals(host, StringComparison.Ordinal)).Value;

            if (value == null)
            {
                _logNoOverrideFoundForHost(_logger, host, null);
            }
            else
            {
                _logFoundOverrideFoundForHost(_logger, host, value, null);
            }

            return value;
        }
    }
}
