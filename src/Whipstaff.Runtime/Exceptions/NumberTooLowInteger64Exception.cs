// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Whipstaff.Runtime.Exceptions
{
    /// <summary>
    /// The number passed in was higher than the allowed maximum.
    /// </summary>
    [System.Serializable]
    public class NumberTooLowInteger64Exception
        : System.ArgumentOutOfRangeException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NumberTooLowInteger64Exception" /> class.
        /// </summary>
        public NumberTooLowInteger64Exception()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberTooLowInteger64Exception" /> class.
        /// </summary>
        /// <param name="message">Exception Message.</param>
        public NumberTooLowInteger64Exception(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberTooLowInteger64Exception" /> class.
        /// </summary>
        /// <param name="message">Exception Message.</param>
        /// <param name="innerException">Inner Exception.</param>
        public NumberTooLowInteger64Exception(
            string message,
            System.Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberTooLowInteger64Exception"/> class.
        /// </summary>
        /// <param name="parameterName">The name of the parameter causing the exception.</param>
        /// <param name="minimumValid">The minimum valid value.</param>
        /// <param name="actual">The actual value received.</param>
        public NumberTooLowInteger64Exception(
            string parameterName,
            long minimumValid,
            long actual)
            : base(parameterName, "The number specified is too low. Minimum: " + minimumValid + ", Actual: " + actual)
        {
            Actual = actual;
            MinimumValid = minimumValid;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberTooLowInteger64Exception"/> class with serialized data.
        /// </summary>
        /// <param name="serializationInfo">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="streamingContext">The StreamingContext that contains contextual information about the source or destination.</param>
        protected NumberTooLowInteger64Exception(
            System.Runtime.Serialization.SerializationInfo serializationInfo,
            System.Runtime.Serialization.StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }

        /// <summary>
        /// Gets the actual value that cause the exception.
        /// </summary>
        public long Actual { get; private set; }

        /// <summary>
        /// Gets the minimum valid value.
        /// </summary>
        public long MinimumValid { get; private set; }
    }
}