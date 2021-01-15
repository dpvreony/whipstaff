using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dhgms.AspNetCoreContrib.App.Features.Bitwise
{
    /// <summary>
    /// A single set latch, used for where you need to flag something like an update flag where updates
    /// can be caused by multiple sources.
    /// This is designed to reduce the risk of using a boolean flag and manually applying the logic.
    /// </summary>
    public sealed class BooleanLatch
    {
        /// <summary>
        /// A flag indicating whether the latch has been set to true.
        /// </summary>
        private bool _internalValue;

        /// <summary>
        /// Gets a value indicating whether the flag has been set.
        /// </summary>
        public bool Value
        {
            get => _internalValue;
            private set => _internalValue = value;
        }

        /// <summary>
        /// Triggers the latch.
        /// </summary>
        public void Set()
        {
            Value = true;
        }
    }
}
