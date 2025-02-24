// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Runtime.Serialization;

namespace Whipstaff.Runtime.Exceptions
{
    /// <summary>
    /// We failed to get the resource stream. Used when loading a resource file.
    /// </summary>
    public class FailedToGetResourceStreamException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FailedToGetResourceStreamException"/> class.
        /// Constructor.
        /// </summary>
        public FailedToGetResourceStreamException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FailedToGetResourceStreamException"/> class.
        /// Constructor.
        /// </summary>
        /// <param name="message">
        /// Exception Message.
        /// </param>
        public FailedToGetResourceStreamException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FailedToGetResourceStreamException"/> class.
        /// Constructor.
        /// </summary>
        /// <param name="message">
        /// Exception Message.
        /// </param>
        /// <param name="innerException">
        /// Inner Exception.
        /// </param>
        public FailedToGetResourceStreamException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
