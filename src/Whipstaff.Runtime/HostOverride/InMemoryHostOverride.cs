// Copyright (c) 2020 DHGMS Solutions and Contributors. All rights reserved.
// DHGMS Solutions and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Whipstaff.Runtime.HostOverride
{
    /// <summary>
    /// An in memory implementation of a host override.
    /// </summary>
    public sealed class InMemoryHostOverride : IHostOverride
    {
        private readonly IDictionary<string, string> _mappings;

        /// <summary>
        /// Initializes a new instance of the <see cref="InMemoryHostOverride"/> class.
        /// </summary>
        /// <param name="mappings">Dictionary of mappings.</param>
        public InMemoryHostOverride(IDictionary<string, string> mappings)
        {
            _mappings = mappings ?? throw new ArgumentNullException(nameof(mappings));
        }

        /// <inheritdoc/>
        public string Resolve(string host)
        {
            if (string.IsNullOrWhiteSpace(host))
            {
                throw new ArgumentNullException(nameof(host));
            }

            return _mappings.FirstOrDefault(x => x.Key.Equals(host, StringComparison.Ordinal)).Value;
        }
    }
}
