// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.Logging;
using Whipstaff.Core.Logging;

namespace Whipstaff.Wpf.JumpLists
{
    /// <summary>
    /// Log message actions for <see cref="JumpListHelper"/>.
    /// </summary>
    public sealed class JumpListLogMessageActions : ILogMessageActions<JumpListHelper>
    {
        private readonly Action<ILogger, Exception?> _noJumpListRegisteredCreatingNew;
        private readonly Action<ILogger, Exception?> _addingJumpPathToRecentCategory;
        private readonly Action<ILogger, Exception?> _addingJumpTaskToRecentCategory;
        private readonly Action<ILogger, Exception?> _clearingJumpList;

        /// <summary>
        /// Initializes a new instance of the <see cref="JumpListLogMessageActions"/> class.
        /// </summary>
        public JumpListLogMessageActions()
        {
            _noJumpListRegisteredCreatingNew = LoggerMessage.Define(
                LogLevel.Information,
                WhipstaffEventIdFactory.NoJumpListRegisteredCreatingNew(),
                "No Jump List Registered for application, creating a new registration");

            _addingJumpPathToRecentCategory = LoggerMessage.Define(
                LogLevel.Information,
                WhipstaffEventIdFactory.AddingJumpPathToRecentCategory(),
                "Adding Jump Path to Recent");

            _addingJumpTaskToRecentCategory = LoggerMessage.Define(
                LogLevel.Information,
                WhipstaffEventIdFactory.AddingJumpTaskToRecentCategory(),
                "Adding Jump Task to Recent");

            _clearingJumpList = LoggerMessage.Define(
                LogLevel.Information,
                WhipstaffEventIdFactory.ClearingJumpList(),
                "Clearing Jump List");
        }

        /// <summary>
        /// Log event when no jump list is registered and creating a new one.
        /// </summary>
        /// <param name="logger">Logging framework instance.</param>
        public void NoJumpListRegisteredCreatingNew(ILogger<JumpListHelper> logger)
        {
            _noJumpListRegisteredCreatingNew(logger, null);
        }

        /// <summary>
        /// Log event when adding a jump path to the recent category.
        /// </summary>
        /// <param name="logger">Logging framework instance.</param>
        public void AddingJumpPathToRecentCategory(ILogger<JumpListHelper> logger)
        {
            _addingJumpPathToRecentCategory(logger, null);
        }

        /// <summary>
        /// Log event when adding a jump task to the recent category.
        /// </summary>
        /// <param name="logger">Logging framework instance.</param>
        public void AddingJumpTaskToRecentCategory(ILogger<JumpListHelper> logger)
        {
            _addingJumpTaskToRecentCategory(logger, null);
        }

        /// <summary>
        /// Log event when clearing the jump list.
        /// </summary>
        /// <param name="logger">Logging framework instance.</param>
        public void ClearingJumpList(ILogger<JumpListHelper> logger)
        {
            _clearingJumpList(logger, null);
        }
    }
}
