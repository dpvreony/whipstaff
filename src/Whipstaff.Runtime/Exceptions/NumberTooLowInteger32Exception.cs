// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Whipstaff.Runtime.Exceptions
{
    /// <summary>
    /// The number passed in was higher than the allowed maximum.
    /// </summary>
    [System.Serializable]
#pragma warning disable RCS1194 // Implement exception constructors.
    public class NumberTooLowInteger32Exception
#pragma warning restore RCS1194 // Implement exception constructors.
        : System.ArgumentOutOfRangeException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NumberTooLowInteger32Exception" /> class.
        /// </summary>
        public NumberTooLowInteger32Exception()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberTooLowInteger32Exception" /> class.
        /// </summary>
        /// <param name="message">Exception Message.</param>
        public NumberTooLowInteger32Exception(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberTooLowInteger32Exception" /> class.
        /// </summary>
        /// <param name="message">Exception Message.</param>
        /// <param name="innerException">Inner Exception.</param>
        public NumberTooLowInteger32Exception(
            string message,
            System.Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberTooLowInteger32Exception"/> class.
        /// </summary>
        /// <param name="parameterName">The name of the parameter causing the exception.</param>
        /// <param name="minimumValid">The minimum valid value.</param>
        /// <param name="actual">The actual value received.</param>
        public NumberTooLowInteger32Exception(
            string parameterName,
            int minimumValid,
            int actual)
            : base(parameterName, "The number specified is too low. Minimum: " + minimumValid + ", Actual: " + actual)
        {
            Actual = actual;
            MinimumValid = minimumValid;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberTooLowInteger32Exception"/> class with serialized data.
        /// </summary>
        /// <param name="serializationInfo">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="streamingContext">The StreamingContext that contains contextual information about the source or destination.</param>
        protected NumberTooLowInteger32Exception(
            System.Runtime.Serialization.SerializationInfo serializationInfo,
            System.Runtime.Serialization.StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
        }

        /// <summary>
        /// Gets the actual value that cause the exception.
        /// </summary>
        public int Actual { get; private set; }

        /// <summary>
        /// Gets the minimum valid value.
        /// </summary>
        public int MinimumValid { get; private set; }
    }
}
