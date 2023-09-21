// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
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
        private readonly ILogger<JumpListHelper> _logger;
        private readonly IDisposable? _jumpItemsRemovedByUserSubscription;
        private readonly IDisposable? _jumpItemsRejectedSubscription;

        /// <summary>
        /// Initializes a new instance of the <see cref="JumpListHelper"/> class.
        /// </summary>
        /// <param name="jumpList">Jump list associated with the application.</param>
        /// <param name="jumpItemsRemovedByUserSubscription">Subscription action for handling the notification for when an item is removed from a jump list by a user.</param>
        /// <param name="jumpItemsRejectedSubscription">Subscription action for handling the notification for when a jump item is rejected.</param>
        /// <param name="logger">Logging framework instance.</param>
        public JumpListHelper(
            JumpList jumpList,
            Action<JumpItemsRemovedEventArgs>? jumpItemsRemovedByUserSubscription,
            Action<JumpItemsRejectedEventArgs>? jumpItemsRejectedSubscription,
            ILogger<JumpListHelper> logger)
        {
            _jumpList = jumpList ?? throw new ArgumentNullException(nameof(jumpList));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));

            var jumpListEvents = _jumpList.Events();

            if (jumpItemsRemovedByUserSubscription != null)
            {
                _jumpItemsRemovedByUserSubscription = jumpListEvents.JumpItemsRemovedByUser.Subscribe(jumpItemsRemovedByUserSubscription);
            }

            if (jumpItemsRejectedSubscription != null)
            {
                _jumpItemsRejectedSubscription = jumpListEvents.JumpItemsRejected.Subscribe(jumpItemsRejectedSubscription);
            }
        }

        /// <summary>
        /// Helper to set up an instance of the Jump List Helper.
        /// </summary>
        /// <typeparam name="TApplication">The type for the application.</typeparam>
        /// <param name="applicationContext">Instance of the application context.</param>
        /// <param name="logger">Logging framework instance.</param>
        /// <param name="jumpItemsFunc">Function to produce a set of Jump List items.</param>
        /// <param name="jumpItemsRemovedByUserSubscription">Handler for the event fired off when a jump item is removed by a user.</param>
        /// <param name="jumpItemsRejectedSubscription">Handler for the event fired off when when the jump items are rejected.</param>
        /// <returns>Instance of <see cref="JumpListHelper"/>.</returns>
        public static JumpListHelper GetInstance<TApplication>(
            TApplication applicationContext,
            ILogger<JumpListHelper> logger,
            Func<string, IEnumerable<JumpItem>> jumpItemsFunc,
            Action<JumpItemsRemovedEventArgs> jumpItemsRemovedByUserSubscription,
            Action<JumpItemsRejectedEventArgs> jumpItemsRejectedSubscription)
            where TApplication : Application
        {
            var assembly = typeof(TApplication).Assembly;

            return GetInstance(
                applicationContext,
                logger,
                assembly,
                jumpItemsFunc,
                jumpItemsRemovedByUserSubscription,
                jumpItemsRejectedSubscription);
        }

        /// <summary>
        /// Helper to set up an instance of the Jump List Helper.
        /// </summary>
        /// <param name="applicationContext">Instance of the application context.</param>
        /// <param name="logger">Logging framework instance.</param>
        /// <param name="assembly">Assembly to associate the jump list to.</param>
        /// <param name="jumpItemsFunc">Function to produce a set of Jump List items.</param>
        /// <param name="jumpItemsRemovedByUserSubscription">Handler for the event fired off when a jump item is removed by a user.</param>
        /// <param name="jumpItemsRejectedSubscription">Handler for the event fired off when when the jump items are rejected.</param>
        /// <returns>Instance of <see cref="JumpListHelper"/>.</returns>
        public static JumpListHelper GetInstance(
            Application applicationContext,
            ILogger<JumpListHelper> logger,
            Assembly assembly,
            Func<string, IEnumerable<JumpItem>> jumpItemsFunc,
            Action<JumpItemsRemovedEventArgs> jumpItemsRemovedByUserSubscription,
            Action<JumpItemsRejectedEventArgs> jumpItemsRejectedSubscription)
        {
            if (applicationContext == null)
            {
                throw new ArgumentNullException(nameof(applicationContext));
            }

            if (logger == null)
            {
                throw new ArgumentNullException(nameof(logger));
            }

            if (assembly == null)
            {
                throw new ArgumentNullException(nameof(assembly));
            }

            if (jumpItemsFunc == null)
            {
                throw new ArgumentNullException(nameof(jumpItemsFunc));
            }

            var cmdPath = assembly.Location;
            var jumpItems = jumpItemsFunc(cmdPath);

            var jumpList = JumpList.GetJumpList(applicationContext);
            if (jumpList == null)
            {
#pragma warning disable CA1848 // Use the LoggerMessage delegates
                logger.LogInformation("No jump list registered. Creating a new one");
#pragma warning restore CA1848 // Use the LoggerMessage delegates
                jumpList = new JumpList(jumpItems, true, true);

                JumpList.SetJumpList(applicationContext, jumpList);
            }

            return new JumpListHelper(jumpList, jumpItemsRemovedByUserSubscription, jumpItemsRejectedSubscription, logger);
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _jumpItemsRemovedByUserSubscription?.Dispose();
            _jumpItemsRejectedSubscription?.Dispose();
        }
    }
}
