// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
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
        /// <param name="serviceProvider">Service provider set up by the App builder.</param>
        public MermaidHttpServer(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }
    }
}
