// Copyright (c) 2020 DHGMS Solutions and Contributors. All rights reserved.
// DHGMS Solutions and Contributors licenses this file to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Whipstaff.Runtime.Exceptions
{
    /// <summary>
    /// The number passed in was higher than the allowed maximum.
    /// </summary>
    [System.Serializable]
    public class NumberTooLowClrSingleException
        : System.ArgumentOutOfRangeException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NumberTooLowClrSingleException" /> class.
        /// </summary>
        public NumberTooLowClrSingleException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberTooLowClrSingleException" /> class.
        /// </summary>
        /// <param name="message">Exception Message.</param>
        public NumberTooLowClrSingleException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberTooLowClrSingleException" /> class.
        /// </summary>
        /// <param name="message">Exception Message.</param>
        /// <param name="innerException">Inner Exception.</param>
        public NumberTooLowClrSingleException(
            string message,
            System.Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberTooLowClrSingleException"/> class.
        /// </summary>
        /// <param name="parameterName">The name of the parameter causing the exception.</param>
        /// <param name="minimumValid">The minimum valid value.</param>
        /// <param name="actual">The actual value received.</param>
        public NumberTooLowClrSingleException(
            string parameterName,
            float minimumValid,
            float actual)
            : base(parameterName, "The number specified is too low. Minimum: " + minimumValid + ", Actual: " + actual)
        {
            Actual = actual;
            MinimumValid = minimumValid;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberTooLowClrSingleException"/> class with serialized data.
        /// </summary>
        /// <param name="serializationInfo">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="streamingContext">The StreamingContext that contains contextual information about the source or destination.</param>
        protected NumberTooLowClrSingleException(
            System.Runtime.Serialization.SerializationInfo serializationInfo,
            System.Runtime.Serialization.StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }

        /// <summary>
        /// Gets the actual value that cause the exception.
        /// </summary>
        public float Actual { get; private set; }

        /// <summary>
        /// Gets the minimum valid value.
        /// </summary>
        public float MinimumValid { get; private set; }
    }
}