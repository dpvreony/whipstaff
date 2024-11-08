// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Aspire.Hosting;
using Whipstaff.Aspire.Hosting.ZedAttackProxy;

var builder = DistributedApplication.CreateBuilder(args);

var apiSite = builder.AddProject<Projects.Dhgms_AspNetCoreContrib_Example_WebApiApp>("api-site")
    .WithExternalHttpEndpoints();

var mvcSite = builder.AddProject<Projects.Dhgms_AspNetCoreContrib_Example_WebMvcApp>("mvc-site")
    .WithExternalHttpEndpoints();

if (builder.ExecutionContext.IsRunMode)
{
    var zapApiKey = "ZAPROXY-API-SECRET";

    _ = builder.AddZedAttackProxyContainerAsDaemon(
            60080,
            zapApiKey)
        .WithReference(apiSite)
        .WithReference(mvcSite);
}

await builder.Build().RunAsync();
