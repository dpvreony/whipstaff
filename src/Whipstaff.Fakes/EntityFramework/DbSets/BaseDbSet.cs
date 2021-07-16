using System;
using Whipstaff.Core.Entities;

namespace Dhgms.AspNetCoreContrib.Fakes.EntityFramework.DbSets
{
    /// <summary>
    /// Represents a base db set.
    /// </summary>
    public class BaseDbSet : IIntId
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
