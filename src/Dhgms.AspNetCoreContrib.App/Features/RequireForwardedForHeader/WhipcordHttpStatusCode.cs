// Copyright (c) 2019 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Dhgms.AspNetCoreContrib.App.Features.RequireForwardedForHeader
{
    /// <summary>
    /// HTTP Status Codes that are specific to Whipcord.
    /// </summary>
    public enum WhipcordHttpStatusCode
    {
        /// <summary>
        /// Server side error for when an X-Forwarded-For header is required but not
        /// received. It's a 5xx error as the client wouldn't be aware they should
        /// be going through a load balancer.
        /// </summary>
        ExpectedXForwardedFor = 599,

        /// <summary>
        /// Server side error for when an X-Forwarded-Proto header is required but not
        /// received. It's a 5xx error as the client wouldn't be aware they should
        /// be going through a load balancer.
        /// </summary>
        ExpectedXForwardedProto = 598,
    }
}
