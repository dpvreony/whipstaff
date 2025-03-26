// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using Rocks;

namespace Whipstaff.UnitTests
{
    /// <summary>
    /// Expectations for the Authorization Service.
    /// </summary>
    [RockPartial(typeof(IAuthorizationService), BuildType.Create)]
#pragma warning disable SA1402 // File may only contain a single type
    public sealed partial class IAuthorizationServiceCreateExpectations;

    /// <summary>
    /// Expectations for the Mediator.
    /// </summary>
    [RockPartial(typeof(IMediator), BuildType.Create)]
    public sealed partial class IMediatorCreateExpectations;

    /// <summary>
    /// Expectations for the Mediator.
    /// </summary>
    /// <typeparam name="TCategoryName">The type whose name is used for the logger category name.</typeparam>
    [RockPartial(typeof(ILogger<>), BuildType.Create)]
    public sealed partial class ILoggerCreateExpectations<TCategoryName>;
#pragma warning restore SA1402 // File may only contain a single type
}
