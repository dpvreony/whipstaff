// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Runtime.Serialization;

namespace Whipstaff.Runtime.Exceptions
{
    /// <summary>
    /// The string passed in was longer than the allowed maximum.
    /// </summary>
    public class StringTooLongException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StringTooLongException"/> class.
        /// Constructor.
        /// </summary>
        public StringTooLongException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringTooLongException"/> class.
        /// Constructor.
        /// </summary>
        /// <param name="message">
        /// Exception Message.
        /// </param>
        public StringTooLongException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringTooLongException"/> class.
        /// Constructor.
        /// </summary>
        /// <param name="message">
        /// Exception Message.
        /// </param>
        /// <param name="innerException">
        /// Inner Exception.
        /// </param>
        public StringTooLongException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StringTooLongException"/> class.
        /// Constructor.
        /// </summary>
        /// <param name="expected">
        /// The expected.
        /// </param>
        /// <param name="actual">
        /// The actual.
        /// </param>
        public StringTooLongException(int expected, int actual)
            : base("The string specified is too long. Maximum Length: " + expected + ", Actual: " + actual)
        {
        }
    }
}
