// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Whipstaff.AspNetCore.Features.RequireForwardedForHeader
{
    /// <summary>
    /// HTTP Status Codes that are specific to Whipstaff.
    /// </summary>
    public static class WhipcordHttpStatusCode
    {
        /// <summary>
        /// Server side error for when an X-Forwarded-For header is required but not
        /// received. It's a 5xx error as the client wouldn't be aware they should
        /// be going through a load balancer.
        /// </summary>
        public const int ExpectedXForwardedFor = 599;

        /// <summary>
        /// Server side error for when an X-Forwarded-Proto header is required but not
        /// received. It's a 5xx error as the client wouldn't be aware they should
        /// be going through a load balancer.
        /// </summary>
        public const int ExpectedXForwardedProto = 598;

        /// <summary>
        /// Server side error for when an X-Forwarded-Host header is required but not
        /// received. It's a 5xx error as the client wouldn't be aware they should
        /// be going through a load balancer.
        /// </summary>
        public const int ExpectedXForwardedHost = 597;
    }
}
