using System.ComponentModel;
using System.Reactive.Linq;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
// using ReactiveMarbles.ObservableEvents;

namespace Dhgms.Whipstaff.Desktop.Features.GroupPolicyMonitoring
{
    /// <summary>
    /// Process Manager for handling Group Policy changes.
    /// </summary>
    public sealed class GroupPolicyMonitoringProcessManager : IDisposable
    {
        private readonly ILogger<GroupPolicyMonitoringProcessManager> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public GroupPolicyMonitoringProcessManager(ILogger<GroupPolicyMonitoringProcessManager> logger)
        {
            this._logger = logger;
        }

        private static void HandledEvent<T>(
            object sender,
            T userPreferenceChangingEventArgs,
            Action<T> action)
        {
            action(userPreferenceChangingEventArgs);
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
                    this._logger.LogInformation("Group Policy Refresh Detected");
                    this.OnGroupPolicyRefresh();
                    break;
            }
        }

        private void OnGroupPolicyRefresh()
        {
        }
    }
}
