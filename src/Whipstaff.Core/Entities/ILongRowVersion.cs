namespace Whipstaff.Core.Entities
{
    /// <summary>
    /// Represents an entity that has a row version for concurrency checks.
    /// </summary>
    public interface ILongRowVersion
    {
        /// <summary>
        /// Gets or sets the row version.
        /// </summary>
        long RowVersion { get; set; }
    }

}
