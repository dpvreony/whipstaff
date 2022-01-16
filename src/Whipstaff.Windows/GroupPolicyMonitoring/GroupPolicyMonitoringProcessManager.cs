using Microsoft.Extensions.Logging;
using Microsoft.Win32;

// using ReactiveMarbles.ObservableEvents;

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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="onUserPreferenceChangingGroupPolicy"></param>
        public GroupPolicyMonitoringProcessManager(
            ILogger<GroupPolicyMonitoringProcessManager> logger,
            Action onUserPreferenceChangingGroupPolicy)
        {
            this._logger = logger;
            _onUserPreferenceChangingGroupPolicy = onUserPreferenceChangingGroupPolicy;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Run()
        {
            SystemEvents.UserPreferenceChanging += this.SystemEventsOnUserPreferenceChanging;
        }

        /// <summary>
        /// 
        /// </summary>
        public void Stop()
        {
            SystemEvents.UserPreferenceChanging -= this.SystemEventsOnUserPreferenceChanging;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            SystemEvents.UserPreferenceChanging -= this.SystemEventsOnUserPreferenceChanging;
        }

        private void SystemEventsOnUserPreferenceChanging(object sender, UserPreferenceChangingEventArgs userPreferenceChangingEventArgs)
        {
            switch (userPreferenceChangingEventArgs.Category)
            {
                case UserPreferenceCategory.Policy:
                    this._groupPolicyRefreshDetectedLogMessage(_logger, null);

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
