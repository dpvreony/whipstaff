using System.Threading;

namespace Whipstaff.Runtime.Counters
{
    /// <summary>
    /// Wrapper for an integer that can only be incremented. Useful for operation counters.
    /// </summary>
    public sealed class IncrementOnlyInt
    {
        private int _value;

        /// <summary>
        /// Gets the current value of the counter.
        /// </summary>
        public int Value => _value;

        /// <summary>
        /// Increments the counter by one.
        /// </summary>
        /// <returns>
        /// The incremented value.
        /// </returns>
        public int Increment()
        {
            return Interlocked.Increment(ref _value);
        }
    }
}
