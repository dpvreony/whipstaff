// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Whipstaff.Runtime.Exceptions
{
    /// <summary>
    /// The number passed in was higher than the allowed maximum.
    /// </summary>
    public class NumberTooHighUnsignedInteger8Exception
        : System.Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NumberTooHighUnsignedInteger8Exception" /> class.
        /// </summary>
        public NumberTooHighUnsignedInteger8Exception()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberTooHighUnsignedInteger8Exception" /> class.
        /// </summary>
        /// <param name="message">Exception Message.</param>
        public NumberTooHighUnsignedInteger8Exception(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberTooHighUnsignedInteger8Exception" /> class.
        /// </summary>
        /// <param name="message">Exception Message.</param>
        /// <param name="innerException">Inner Exception.</param>
        public NumberTooHighUnsignedInteger8Exception(
            string message,
            System.Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberTooHighUnsignedInteger8Exception" /> class.
        /// </summary>
        /// <param name="parameterName">The name of the parameter causing the exception.</param>
        /// <param name="maximumValid">The maximum valid value.</param>
        /// <param name="actual">The actual value received.</param>
        public NumberTooHighUnsignedInteger8Exception(
            string parameterName,
            byte maximumValid,
            byte actual)
            : base(parameterName + ": The number specified is too high. Maximum: " + maximumValid + ", Actual: " + actual)
        {
        }
    }
}
