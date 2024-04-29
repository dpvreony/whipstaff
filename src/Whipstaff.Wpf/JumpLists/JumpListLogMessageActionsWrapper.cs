// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.Extensions.Logging;
using Whipstaff.Core.Logging;

namespace Whipstaff.Wpf.JumpLists
{
    /// <summary>
    /// Log message actions wrapper for <see cref="JumpListHelper"/>.
    /// </summary>
    public sealed class JumpListLogMessageActionsWrapper : AbstractLogMessageActionsWrapper<JumpListHelper, JumpListLogMessageActions>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JumpListLogMessageActionsWrapper"/> class.
        /// </summary>
        /// <param name="logMessageActions">Log Message Actions instance.</param>
        /// <param name="logger">Logging framework instance.</param>
        public JumpListLogMessageActionsWrapper(
            JumpListLogMessageActions logMessageActions,
#pragma warning disable S6672
            ILogger<JumpListHelper> logger)
#pragma warning restore S6672
            : base(
                logMessageActions,
                logger)
        {
        }

        /// <summary>
        /// Log event when no jump list is registered and creating a new one.
        /// </summary>
        public void NoJumpListRegisteredCreatingNew()
        {
            LogMessageActions.NoJumpListRegisteredCreatingNew(Logger);
        }

        /// <summary>
        /// Log event when adding a jump path to the recent category.
        /// </summary>
        public void AddingJumpPathToRecentCategory()
        {
            LogMessageActions.AddingJumpPathToRecentCategory(Logger);
        }

        /// <summary>
        /// Log event when adding a jump task to the recent category.
        /// </summary>
        public void AddingJumpTaskToRecentCategory()
        {
            LogMessageActions.AddingJumpTaskToRecentCategory(Logger);
        }

        /// <summary>
        /// Log event when clearing the jump list.
        /// </summary>
        public void ClearingJumpList()
        {
            LogMessageActions.ClearingJumpList(Logger);
        }
    }
}
