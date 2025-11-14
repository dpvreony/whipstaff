// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Whipstaff.AspNetCore.Features.ApplicationStartup;

namespace Dhgms.AspNetCoreContrib.Example.WebApiApp
{
    /// <summary>
    /// Holds the entry point for the application.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// Entry point for the application.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        public static void Main(string[] args)
        {
            WebApplicationFactory.GetHostApplicationBuilder<Startup>(args, null).Run();
        }
    }
}
