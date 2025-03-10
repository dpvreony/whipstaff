using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Whipstaff.Testing.Logging
{
    /// <summary>
    /// Represents a log entry.
    /// </summary>
    public class LogEntry
    {
        /// <summary>
        /// Gets or sets the date and time of the log entry.
        /// </summary>
        public required DateTimeOffset Date { get; set; }

        /// <summary>
        /// Gets or sets the category name of the log entry.
        /// </summary>
        public required string CategoryName { get; set; }

        /// <summary>
        /// Gets or sets the log level of the log entry.
        /// </summary>
        public required LogLevel LogLevel { get; set; }

        /// <summary>
        /// Gets or sets the scopes of the log entry.
        /// </summary>
        public object[]? Scopes { get; set; }

        /// <summary>
        /// Gets or sets the event id of the log entry.
        /// </summary>
        public EventId? EventId { get; set; }

        /// <summary>
        /// Gets or sets the state of the log entry.
        /// </summary>
        public required object State { get; set; }

        /// <summary>
        /// Gets or sets the exception of the log entry.
        /// </summary>
        public Exception? Exception { get; set; }

        /// <summary>
        /// Gets or sets the formatter of the log entry.
        /// </summary>
        public required Func<object, Exception?, string> Formatter { get; set; }

        /// <summary>
        /// Gets the properties of the log entry.
        /// </summary>
        public IDictionary<string, object> Properties { get; } = new Dictionary<string, object>();

        /// <summary>
        /// Gets the message of the log entry.
        /// </summary>
        public string Message => Formatter(State, Exception);

        /// <summary>Returns a string that represents the current object.</summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
#pragma warning disable CA1305 // Specify IFormatProvider
#pragma warning disable CA1304 // Specify CultureInfo
#pragma warning disable CA1311 // Specify a culture or use an invariant version
            return string.Concat(string.Empty, Date.ToString("mm:ss.fff"), " ", LogLevel.ToString().Substring(0, 1).ToUpper(), ": ", Message, " - ", CategoryName);
#pragma warning restore CA1311 // Specify a culture or use an invariant version
#pragma warning restore CA1304 // Specify CultureInfo
#pragma warning restore CA1305 // Specify IFormatProvider
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <param name="useFullCategory">
        /// Whether to use the full category name or just the last part.
        /// </param>
        /// <returns>A string that represents the current object.</returns>
        public string ToString(bool useFullCategory)
        {
            string category = CategoryName;
            if (!useFullCategory)
            {
                int lastDot = category.LastIndexOf('.');
                if (lastDot >= 0)
                {
                    category = category.Substring(lastDot + 1);
                }
            }

#pragma warning disable CA1305 // Specify IFormatProvider
#pragma warning disable CA1304 // Specify CultureInfo
#pragma warning disable CA1311 // Specify a culture or use an invariant version
            return string.Concat(string.Empty, Date.ToString("mm:ss.fff"), " ", LogLevel.ToString().Substring(0, 1).ToUpper(), ": ", Message, " - ", category);
#pragma warning restore CA1311 // Specify a culture or use an invariant version
#pragma warning restore CA1304 // Specify CultureInfo
#pragma warning restore CA1305 // Specify IFormatProvider
        }
    }
}
