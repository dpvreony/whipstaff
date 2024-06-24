// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;

namespace Whipstaff.Mermaid.HttpServer
{
    /// <summary>
    /// In memory HTTP server for Mermaid.
    /// </summary>
    public class MermaidHttpServer : TestServer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MermaidHttpServer"/> class.
        /// </summary>
        /// <param name="builder">Web host builder used to create a server instance.</param>
        public MermaidHttpServer(IWebHostBuilder builder)
            : base(builder)
        {
        }
    }
}
