// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Whipstaff.AspNetCore.Features.ApplicationStartup
{
    /// <summary>
    /// Represents a feature that configures logging as part of application startup.
    /// </summary>
    public interface IConfigureLogging
    {
        /// <summary>
        /// Configure logging integration for the feature.
        /// </summary>
        /// <param name="hostBuilderContext">Host Builder Context for the application.</param>
        /// <param name="loggingBuilder">Logging builder instance to extend configuration of.</param>
        /// <remarks>
        /// TODO: for net8 migrate to static abstract.
        /// </remarks>
        void ConfigureLogging(
            HostBuilderContext hostBuilderContext,
            ILoggingBuilder loggingBuilder);
    }
}
