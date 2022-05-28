// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Whipstaff.Runtime.Exceptions
{
    /// <summary>
    /// The number passed in was higher than the allowed maximum.
    /// </summary>
    [System.Serializable]
    public class NumberTooLowClrCharException
        : System.ArgumentOutOfRangeException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NumberTooLowClrCharException" /> class.
        /// </summary>
        public NumberTooLowClrCharException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberTooLowClrCharException" /> class.
        /// </summary>
        /// <param name="message">Exception Message.</param>
        public NumberTooLowClrCharException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberTooLowClrCharException" /> class.
        /// </summary>
        /// <param name="message">Exception Message.</param>
        /// <param name="innerException">Inner Exception.</param>
        public NumberTooLowClrCharException(
            string message,
            System.Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberTooLowClrCharException"/> class.
        /// </summary>
        /// <param name="parameterName">The name of the parameter causing the exception.</param>
        /// <param name="minimumValid">The minimum valid value.</param>
        /// <param name="actual">The actual value received.</param>
        public NumberTooLowClrCharException(
            string parameterName,
            char minimumValid,
            char actual)
            : base(parameterName, "The number specified is too low. Minimum: " + minimumValid + ", Actual: " + actual)
        {
            Actual = actual;
            MinimumValid = minimumValid;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberTooLowClrCharException"/> class with serialized data.
        /// </summary>
        /// <param name="serializationInfo">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="streamingContext">The StreamingContext that contains contextual information about the source or destination.</param>
        protected NumberTooLowClrCharException(
            System.Runtime.Serialization.SerializationInfo serializationInfo,
            System.Runtime.Serialization.StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }

        /// <summary>
        /// Gets the actual value that cause the exception.
        /// </summary>
        public char Actual { get; }

        /// <summary>
        /// Gets the minimum valid value.
        /// </summary>
        public char MinimumValid { get; }
    }
}