// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Runtime.Serialization;

namespace Whipstaff.Runtime.Exceptions
{
    /// <summary>
    /// The number passed in was higher than the allowed maximum.
    /// </summary>
#pragma warning disable RCS1194 // Implement exception constructors.
    [Serializable]
    public class NumberTooLowClrDateTimeException
#pragma warning restore RCS1194 // Implement exception constructors.
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
            : base(
                parameterName,
                actual,
                "The number specified is too low. Minimum: " + minimumValid + ", Actual: " + actual)
        {
            MinimumValid = minimumValid;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NumberTooLowClrDateTimeException"/> class with serialized data.
        /// </summary>
        /// <param name="serializationInfo">The SerializationInfo that holds the serialized object data about the exception being thrown.</param>
        /// <param name="streamingContext">The StreamingContext that contains contextual information about the source or destination.</param>
        protected NumberTooLowClrDateTimeException(
            System.Runtime.Serialization.SerializationInfo serializationInfo,
            System.Runtime.Serialization.StreamingContext streamingContext)
            : base(serializationInfo, streamingContext)
        {
            MinimumValid = (DateTime)serializationInfo.GetValue("MimimumValid", typeof(object));
        }

        /// <summary>
        /// Gets the minimum valid value.
        /// </summary>
        public System.DateTime MinimumValid { get; private set; }

        /// <inheritdoc/>
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("MimimumValid", MinimumValid, typeof(object));
        }
    }
}
