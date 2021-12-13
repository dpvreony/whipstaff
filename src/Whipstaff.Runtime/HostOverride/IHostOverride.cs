// Copyright (c) 2020 DHGMS Solutions and Contributors. All rights reserved.
// DHGMS Solutions and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Whipstaff.Runtime.HostOverride
{
    /// <summary>
    /// A mechanism for allowing the overriding of DNS\hosts within webservice clients.
    /// This has come from having an infrastructure that uses global messaging
    /// designs but we don't to direct traffic out and back into the network
    /// when there is no point.
    /// This can be done via a hosts file on the system. but it adds a level of
    /// complication in managing systems. And a hosts file is global to the
    /// whole operating system.
    /// </summary>
    public interface IHostOverride
    {
        /// <summary>
        /// Returns an overridable hostname.
        /// </summary>
        /// <param name="host">The host to check.</param>
        /// <returns>A target host, or null if no override.</returns>
        string? Resolve(string host);
    }
}
