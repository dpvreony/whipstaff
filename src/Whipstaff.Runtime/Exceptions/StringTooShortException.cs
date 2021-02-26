// Copyright (c) 2020 DHGMS Solutions and Contributors. All rights reserved.
// DHGMS Solutions and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Runtime.Serialization;

namespace Whipstaff.Runtime.Exceptions
{
    /// <summary>
    /// The string passed in was longer than the allowed maximum.
    /// </summary>
    [Serializable]
    public class StringTooShortException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringTooShortException"/> class.
        /// Constructor.
        /// </summary>
        public StringTooShortException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringTooShortException"/> class.
        /// Constructor.
        /// </summary>
        /// <param name="message">
        /// Exception Message.
        /// </param>
        public StringTooShortException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringTooShortException"/> class.
        /// Constructor.
        /// </summary>
        /// <param name="message">
        /// Exception Message.
        /// </param>
        /// <param name="innerException">
        /// Inner Exception.
        /// </param>
        public StringTooShortException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringTooShortException"/> class.
        /// Constructor.
        /// </summary>
        /// <param name="expected">
        /// The expected.
        /// </param>
        /// <param name="actual">
        /// The actual.
        /// </param>
        public StringTooShortException(int expected, int actual)
            : base("The string specified is too short. Minimum Length: " + expected + ", Actual: " + actual)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringTooShortException"/> class.
        /// Constructor.
        /// </summary>
        /// <param name="info">
        /// Serialization Info.
        /// </param>
        /// <param name="context">
        /// Context.
        /// </param>
        protected StringTooShortException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}