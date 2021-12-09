using Microsoft.Extensions.Logging;
using Microsoft.Win32;

namespace Dhgms.Whipstaff.Desktop.Features.GroupPolicyMonitoring
{
    public sealed class GroupPolicyMonitoringProcessManager : IDisposable
    {
        private readonly ILogger<GroupPolicyMonitoringProcessManager> _logger;

        public GroupPolicyMonitoringProcessManager(ILogger<GroupPolicyMonitoringProcessManager> logger)
        {
            this._logger = logger;
        }

        public void Run()
        {
            SystemEvents.UserPreferenceChanging += this.SystemEventsOnUserPreferenceChanging;
        }

        public void Stop()
        {
            SystemEvents.UserPreferenceChanging -= this.SystemEventsOnUserPreferenceChanging;
        }

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
