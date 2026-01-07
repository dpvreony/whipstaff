// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reactive.Disposables;
using System.Reflection;
using System.Windows;
using System.Windows.Shell;
using Microsoft.Extensions.Logging;
using ReactiveMarbles.ObservableEvents;

namespace Whipstaff.Wpf.JumpLists
{
    /// <summary>
    /// Jump List Process Manager.
    /// </summary>
    public sealed class JumpListHelper : IDisposable
    {
        private readonly JumpList _jumpList;
        private readonly JumpListLogMessageActionsWrapper _logMessageActionsWrapper;
        private readonly CompositeDisposable _compositeDisposable;

        /// <summary>
        /// Initializes a new instance of the <see cref="JumpListHelper"/> class.
        /// </summary>
        /// <param name="jumpList">Jump list associated with the application.</param>
        /// <param name="jumpItemsRemovedByUserSubscription">Subscription action for handling the notification for when an item is removed from a jump list by a user.</param>
        /// <param name="jumpItemsRejectedSubscription">Subscription action for handling the notification for when a jump item is rejected.</param>
        /// <param name="logMessageActionsWrapper">Logging framework message actions wrapper instance.</param>
        public JumpListHelper(
            JumpList jumpList,
            Action<JumpItemsRemovedEventArgs>? jumpItemsRemovedByUserSubscription,
            Action<JumpItemsRejectedEventArgs>? jumpItemsRejectedSubscription,
            JumpListLogMessageActionsWrapper logMessageActionsWrapper)
        {
            ArgumentNullException.ThrowIfNull(jumpList);
            ArgumentNullException.ThrowIfNull(logMessageActionsWrapper);
            _jumpList = jumpList;
            _logMessageActionsWrapper = logMessageActionsWrapper;

            _compositeDisposable = new CompositeDisposable();
            var jumpListEvents = _jumpList.Events();

#pragma warning disable GR0012 // Constructors should minimise work and not execute methods
            if (jumpItemsRemovedByUserSubscription != null)
            {
                _compositeDisposable.Add(jumpListEvents.JumpItemsRemovedByUser.Subscribe(jumpItemsRemovedByUserSubscription));
            }

            if (jumpItemsRejectedSubscription != null)
            {
                _compositeDisposable.Add(jumpListEvents.JumpItemsRejected.Subscribe(jumpItemsRejectedSubscription));
            }
#pragma warning restore GR0012 // Constructors should minimise work and not execute methods
        }

        /// <summary>
        /// Helper to set up an instance of the Jump List Helper.
        /// </summary>
        /// <typeparam name="TApplication">The type for the application.</typeparam>
        /// <param name="applicationContext">Instance of the application context.</param>
        /// <param name="logMessageActionsWrapper">Logging framework message actions wrapper instance.</param>
        /// <param name="jumpItemsFunc">Function to produce a set of Jump List items.</param>
        /// <param name="jumpItemsRemovedByUserSubscription">Handler for the event fired off when a jump item is removed by a user.</param>
        /// <param name="jumpItemsRejectedSubscription">Handler for the event fired off when when the jump items are rejected.</param>
        /// <returns>Instance of <see cref="JumpListHelper"/>.</returns>
        public static JumpListHelper GetInstance<TApplication>(
            TApplication applicationContext,
            JumpListLogMessageActionsWrapper logMessageActionsWrapper,
            Func<string, IEnumerable<JumpItem>> jumpItemsFunc,
            Action<JumpItemsRemovedEventArgs>? jumpItemsRemovedByUserSubscription,
            Action<JumpItemsRejectedEventArgs>? jumpItemsRejectedSubscription)
            where TApplication : Application
        {
            var assembly = typeof(TApplication).Assembly;

            return GetInstance(
                applicationContext,
                logMessageActionsWrapper,
                assembly,
                jumpItemsFunc,
                jumpItemsRemovedByUserSubscription,
                jumpItemsRejectedSubscription);
        }

        /// <summary>
        /// Helper to set up an instance of the Jump List Helper.
        /// </summary>
        /// <param name="applicationContext">Instance of the application context.</param>
        /// <param name="logMessageActionsWrapper">Logging framework message actions wrapper instance.</param>
        /// <param name="assembly">Assembly to associate the jump list to.</param>
        /// <param name="jumpItemsFunc">Function to produce a set of Jump List items.</param>
        /// <param name="jumpItemsRemovedByUserSubscription">Handler for the event fired off when a jump item is removed by a user.</param>
        /// <param name="jumpItemsRejectedSubscription">Handler for the event fired off when when the jump items are rejected.</param>
        /// <returns>Instance of <see cref="JumpListHelper"/>.</returns>
        public static JumpListHelper GetInstance(
            Application applicationContext,
            JumpListLogMessageActionsWrapper logMessageActionsWrapper,
            Assembly assembly,
            Func<string, IEnumerable<JumpItem>> jumpItemsFunc,
            Action<JumpItemsRemovedEventArgs>? jumpItemsRemovedByUserSubscription,
            Action<JumpItemsRejectedEventArgs>? jumpItemsRejectedSubscription)
        {
            ArgumentNullException.ThrowIfNull(applicationContext);
            ArgumentNullException.ThrowIfNull(logMessageActionsWrapper);
            ArgumentNullException.ThrowIfNull(assembly);
            ArgumentNullException.ThrowIfNull(jumpItemsFunc);

            var cmdPath = assembly.Location;
            var jumpItems = jumpItemsFunc(cmdPath);

            var jumpList = JumpList.GetJumpList(applicationContext);
            if (jumpList == null)
            {
#pragma warning disable CA1848 // Use the LoggerMessage delegates
                logMessageActionsWrapper.NoJumpListRegisteredCreatingNew();
#pragma warning restore CA1848 // Use the LoggerMessage delegates
                jumpList = new JumpList(jumpItems, true, true);

                JumpList.SetJumpList(applicationContext, jumpList);
            }

            return new JumpListHelper(
                jumpList,
                jumpItemsRemovedByUserSubscription,
                jumpItemsRejectedSubscription,
                logMessageActionsWrapper);
        }

        /// <summary>
        /// Adds a Jump Path to the recent category.
        /// </summary>
        /// <param name="jumpPath">Jump path to add.</param>
        public void AddToRecentCategory(JumpPath jumpPath)
        {
            ArgumentNullException.ThrowIfNull(jumpPath);

            _logMessageActionsWrapper.AddingJumpPathToRecentCategory();
            JumpList.AddToRecentCategory(jumpPath);
        }

        /// <summary>
        /// Adds a Jump Task to the recent category.
        /// </summary>
        /// <param name="jumpTask">Jump Task to add.</param>
        public void AddToRecentCategory(JumpTask jumpTask)
        {
            ArgumentNullException.ThrowIfNull(jumpTask);

            _logMessageActionsWrapper.AddingJumpTaskToRecentCategory();
            JumpList.AddToRecentCategory(jumpTask);
        }

        /// <summary>
        /// Clears the jump list.
        /// </summary>
        public void Clear()
        {
            _logMessageActionsWrapper.ClearingJumpList();
            _jumpList.JumpItems.Clear();
            _jumpList.Apply();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _compositeDisposable.Dispose();
        }
    }
}
