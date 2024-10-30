// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using ReactiveMarbles.ObservableEvents;

[assembly: GenerateStaticEventObservables(typeof(SystemEvents))]

namespace Whipstaff.Windows.GroupPolicyMonitoring
{
    /// <summary>
    /// Process Manager for handling Group Policy changes.
    /// </summary>
    public sealed class GroupPolicyMonitoringProcessManager : IDisposable
    {
        private readonly ILogger<GroupPolicyMonitoringProcessManager> _logger;
        private readonly Action _onUserPreferenceChangingGroupPolicy;

        private readonly Action<ILogger, Exception?> _groupPolicyRefreshDetectedLogMessage =
            LoggerMessage.Define(LogLevel.Information, new EventId(1001, "Group Policy Refresh Detected"), "Group Policy Refresh Detected");

        private readonly IDisposable _userPreferenceChangingSubscription;

        /// <summary>
        /// Initializes a new instance of the <see cref="GroupPolicyMonitoringProcessManager"/> class.
        /// </summary>
        /// <param name="logger">Logging framework instance.</param>
        /// <param name="onUserPreferenceChangingGroupPolicy">action to carry out when user preferences are changing via group policy.</param>
        public GroupPolicyMonitoringProcessManager(
            ILogger<GroupPolicyMonitoringProcessManager> logger,
            Action onUserPreferenceChangingGroupPolicy)
        {
            _logger = logger;
            _onUserPreferenceChangingGroupPolicy = onUserPreferenceChangingGroupPolicy;
            _userPreferenceChangingSubscription = RxEvents.SystemEventsUserPreferenceChanging.Subscribe(userPreferenceChangingEventArgs => OnSystemEventsOnUserPreferenceChanging(userPreferenceChangingEventArgs));
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _userPreferenceChangingSubscription.Dispose();
        }

        private void OnSystemEventsOnUserPreferenceChanging(UserPreferenceChangingEventArgs userPreferenceChangingEventArgs)
        {
            switch (userPreferenceChangingEventArgs.Category)
            {
                case UserPreferenceCategory.Policy:
                    _groupPolicyRefreshDetectedLogMessage(_logger, null);

                    try
                    {
                        _onUserPreferenceChangingGroupPolicy?.Invoke();
                    }
#pragma warning disable CA1031 // Do not catch general exception types
                    catch
#pragma warning restore CA1031 // Do not catch general exception types
                    {
                        // ignored
                    }

                    break;
            }
        }
    }
}
