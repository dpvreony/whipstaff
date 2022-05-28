// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Whipstaff.Runtime.Exceptions
{
    /// <summary>
    /// The number passed in was higher than the allowed maximum.
    /// </summary>
    public class NumberTooLowClrDateTimeException
        : System.ArgumentOutOfRangeException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NumberTooLowClrDateTimeException" /> class.
        /// </summary>
        public NumberTooLowClrDateTimeException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberTooLowClrDateTimeException" /> class.
        /// </summary>
        /// <param name="message">Exception Message.</param>
        public NumberTooLowClrDateTimeException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberTooLowClrDateTimeException" /> class.
        /// </summary>
        /// <param name="message">Exception Message.</param>
        /// <param name="innerException">Inner Exception.</param>
        public NumberTooLowClrDateTimeException(
            string message,
            System.Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberTooLowClrDateTimeException"/> class.
        /// </summary>
        /// <param name="parameterName">The name of the parameter causing the exception.</param>
        /// <param name="minimumValid">The minimum valid value.</param>
        /// <param name="actual">The actual value received.</param>
        public NumberTooLowClrDateTimeException(
            string parameterName,
            System.DateTime minimumValid,
            System.DateTime actual)
            : base(parameterName, "The number specified is too low. Minimum: " + minimumValid + ", Actual: " + actual)
        {
            Actual = actual;
            MinimumValid = minimumValid;
        }

        /// <summary>
        /// Gets the actual value that cause the exception.
        /// </summary>
        public System.DateTime Actual { get; private set; }

        /// <summary>
        /// Gets the minimum valid value.
        /// </summary>
        public System.DateTime MinimumValid { get; private set; }
    }
}