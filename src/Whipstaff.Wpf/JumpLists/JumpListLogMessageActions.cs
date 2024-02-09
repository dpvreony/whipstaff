// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.Extensions.Logging;
using Whipstaff.Core.Logging;

namespace Whipstaff.Wpf.JumpLists
{
    /// <summary>
    /// Log message actions for <see cref="JumpListHelper"/>.
    /// </summary>
    public sealed class JumpListLogMessageActions : ILogMessageActions<JumpListHelper>
    {
        /// <summary>
        /// Log event when no jump list is registered and creating a new one.
        /// </summary>
        /// <param name="logger">Logging framework instance.</param>
        public void NoJumpListRegisteredCreatingNew(ILogger<JumpListHelper> logger)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Log event when adding a jump path to the recent category.
        /// </summary>
        /// <param name="logger">Logging framework instance.</param>
        public void AddingJumpPathToRecentCategory(ILogger<JumpListHelper> logger)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Log event when adding a jump task to the recent category.
        /// </summary>
        /// <param name="logger">Logging framework instance.</param>
        public void AddingJumpTaskToRecentCategory(ILogger<JumpListHelper> logger)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Log event when clearing the jump list.
        /// </summary>
        /// <param name="logger">Logging framework instance.</param>
        public void ClearingJumpList(ILogger<JumpListHelper> logger)
        {
            throw new System.NotImplementedException();
        }
    }
}
