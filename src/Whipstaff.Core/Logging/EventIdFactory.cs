// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.Logging;

namespace Whipstaff.Core.Logging
{
    /// <summary>
    /// Factory for creating <see cref="EventId"/> instances.
    /// </summary>
    public static class EventIdFactory
    {
        /// <summary>
        /// Creates an <see cref="EventId"/> with the specified ID and the name of the calling member.
        /// </summary>
        /// <param name="id">Unique ID for the event.</param>
        /// <param name="memberName">The calling member name to use to name the event.</param>
        /// <returns>Event ID instance.</returns>
        public static EventId NamedByCallerMemberName(
            int id,
            [System.Runtime.CompilerServices.CallerMemberName] string memberName = "") => new(id, memberName);
    }
}
