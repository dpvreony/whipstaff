using System;
using System.Collections.Generic;
using System.Text;

namespace Dhgms.AspNetCoreContrib.Fakes.EntityFramework.DbSets
{
    /// <summary>
    /// Represents a fake audit of an add command. This is not indicative of how and what to do in an audit.
    /// It is purely a mechanism for testing the library calls.
    /// </summary>
    public class FakeAddAuditDbSet
    {
        /// <summary>
        /// Gets or sets the unique id of the audit.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        public int Value { get; set; }

        /// <summary>
        /// Gets or sets the timestamp for when the record was created.
        /// </summary>
        public DateTimeOffset Created { get; set; }
    }
}
