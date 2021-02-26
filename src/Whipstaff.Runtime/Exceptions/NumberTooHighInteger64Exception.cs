// Copyright (c) 2020 DHGMS Solutions and Contributors. All rights reserved.
// DHGMS Solutions and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Whipstaff.Runtime.Exceptions
{
    /// <summary>
    /// The number passed in was higher than the allowed maximum.
    /// </summary>
    [System.Serializable]
    public class NumberTooHighInteger64Exception
        : System.Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NumberTooHighInteger64Exception" /> class.
        /// </summary>
        public NumberTooHighInteger64Exception()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberTooHighInteger64Exception" /> class.
        /// </summary>
        /// <param name="message">Exception Message.</param>
        public NumberTooHighInteger64Exception(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberTooHighInteger64Exception" /> class.
        /// </summary>
        /// <param name="message">Exception Message.</param>
        /// <param name="innerException">Inner Exception.</param>
        public NumberTooHighInteger64Exception(
            string message,
            System.Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberTooHighInteger64Exception" /> class.
        /// </summary>
        /// <param name="parameterName">The name of the parameter causing the exception.</param>
        /// <param name="maximumValid">The maximum valid value.</param>
        /// <param name="actual">The actual value received.</param>
        public NumberTooHighInteger64Exception(
            string parameterName,
            long maximumValid,
            long actual)
            : base(parameterName + ": The number specified is too high. Maximum: " + maximumValid + ", Actual: " + actual)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberTooHighInteger64Exception" /> class.
        /// </summary>
        /// <param name="info">The serialization information.</param>
        /// <param name="context">The streaming context.</param>
        protected NumberTooHighInteger64Exception(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
}