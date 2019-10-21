// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Blazor.Hosting;

namespace Dhgms.AspNetCoreContrib.Example.WebBlazorWasm
{
    public static class Program
    {
        public static void Main()
        {
            CreateHostBuilder().Build().Run();
        }

        public static IWebAssemblyHostBuilder CreateHostBuilder() =>
            BlazorWebAssemblyHost.CreateDefaultBuilder()
                .UseBlazorStartup<Startup>();
    }
}
