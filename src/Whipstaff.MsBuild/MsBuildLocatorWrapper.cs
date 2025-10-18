using System;
using System.Linq;
using Microsoft.Build.Locator;

namespace Whipstaff.MsBuild
{
    /// <summary>
    /// Hack wrapper for <see cref="MSBuildLocator"/> as you have to initialise MSBuild in a method not actually using it.
    /// https://learn.microsoft.com/en-gb/visualstudio/msbuild/find-and-use-msbuild-versions?view=vs-2022#use-microsoftbuildlocator for more information.
    /// </summary>
    public sealed class MsBuildLocatorWrapper
    {
        private MsBuildLocatorWrapper(VisualStudioInstance visualStudioInstance)
        {
            VisualStudioInstance = visualStudioInstance;
        }

        /// <summary>
        /// Gets the registered Visual Studio instance.
        /// </summary>
        public VisualStudioInstance VisualStudioInstance { get; }

        /// <summary>
        /// Gets the latest version of Visual Studio if any version is installed.
        /// </summary>
        /// <returns>Visual Studio Instance for the latest version installed, or null if no instance is installed.</returns>
        public static VisualStudioInstance? GetLatestVersionOfVisualStudio()
        {
            return MSBuildLocator.QueryVisualStudioInstances()
                .MaxBy(instance => instance.Version);
        }

        /// <summary>
        /// Registers latest version of Visual Studio if any version is installed.
        /// </summary>
        /// <returns>Wrapper instance for <see cref="MSBuildLocator"/>.</returns>
        public static MsBuildLocatorWrapper RegisterLatestVersionOfVisualStudio()
        {
            var visualStudioInstance = GetLatestVersionOfVisualStudio();

            if (visualStudioInstance == null)
            {
                throw new InvalidOperationException("No instance of Visual Studio detected");
            }

            MSBuildLocator.RegisterInstance(visualStudioInstance);

            return new MsBuildLocatorWrapper(visualStudioInstance);
        }
    }
}
