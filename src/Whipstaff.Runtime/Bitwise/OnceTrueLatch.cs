// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Whipstaff.Runtime.Bitwise
{
    /// <summary>
    /// A single set latch, used for where you need to flag something like an update flag where updates
    /// can be caused by multiple sources.
    /// This is designed to reduce the maintenance overhead of using a boolean flag and manually applying the logic.
    /// </summary>
    public sealed class OnceTrueLatch
    {
        /// <summary>
        /// Gets a value indicating whether the flag has been set.
        /// </summary>
        public bool Value
        {
            get;
            private set;
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
