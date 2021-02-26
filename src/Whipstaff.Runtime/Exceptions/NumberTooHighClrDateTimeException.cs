// Copyright (c) 2020 DHGMS Solutions and Contributors. All rights reserved.
// DHGMS Solutions and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Whipstaff.Runtime.Exceptions
{
    /// <summary>
    /// The number passed in was higher than the allowed maximum.
    /// </summary>
    [System.Serializable]
    public class NumberTooHighClrDateTimeException
        : System.Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NumberTooHighClrDateTimeException" /> class.
        /// </summary>
        public NumberTooHighClrDateTimeException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberTooHighClrDateTimeException" /> class.
        /// </summary>
        /// <param name="message">Exception Message.</param>
        public NumberTooHighClrDateTimeException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberTooHighClrDateTimeException" /> class.
        /// </summary>
        /// <param name="message">Exception Message.</param>
        /// <param name="innerException">Inner Exception.</param>
        public NumberTooHighClrDateTimeException(
            string message,
            System.Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberTooHighClrDateTimeException" /> class.
        /// </summary>
        /// <param name="parameterName">The name of the parameter causing the exception.</param>
        /// <param name="maximumValid">The maximum valid value.</param>
        /// <param name="actual">The actual value received.</param>
        public NumberTooHighClrDateTimeException(
            string parameterName,
            System.DateTime maximumValid,
            System.DateTime actual)
            : base(parameterName + ": The number specified is too high. Maximum: " + maximumValid + ", Actual: " + actual)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberTooHighClrDateTimeException" /> class.
        /// </summary>
        /// <param name="info">The serialization information.</param>
        /// <param name="context">The streaming context.</param>
        protected NumberTooHighClrDateTimeException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context)
            : base(info, context)
        {
        }
    }
}
