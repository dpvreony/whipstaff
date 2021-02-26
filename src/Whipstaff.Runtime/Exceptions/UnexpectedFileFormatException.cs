// Copyright (c) 2020 DHGMS Solutions and Contributors. All rights reserved.
// DHGMS Solutions and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Runtime.Serialization;

namespace Whipstaff.Runtime.Exceptions
{
    /// <summary>
    /// Exception for when a file is not in the expected format.
    /// </summary>
    [Serializable]
    public class UnexpectedFileFormatException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnexpectedFileFormatException"/> class.
        /// Constructor.
        /// </summary>
        public UnexpectedFileFormatException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnexpectedFileFormatException"/> class.
        /// Constructor.
        /// </summary>
        /// <param name="message">
        /// Exception Message.
        /// </param>
        public UnexpectedFileFormatException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnexpectedFileFormatException"/> class.
        /// Constructor.
        /// </summary>
        /// <param name="message">
        /// Exception Message.
        /// </param>
        /// <param name="innerException">
        /// Inner Exception.
        /// </param>
        public UnexpectedFileFormatException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnexpectedFileFormatException"/> class.
        /// Constructor.
        /// </summary>
        /// <param name="info">
        /// Serialization Info.
        /// </param>
        /// <param name="context">
        /// Context.
        /// </param>
        protected UnexpectedFileFormatException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}