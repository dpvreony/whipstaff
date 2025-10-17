// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using MediatR;

namespace Whipstaff.MediatR
{
    /// <summary>
    /// Represents a MediatR Command.
    /// </summary>
    /// <typeparam name="TCommandResponse">Response of the command.</typeparam>
#pragma warning disable CA1040 // Avoid empty interfaces
    public interface ICommand<out TCommandResponse> : IRequest<TCommandResponse>
#pragma warning restore CA1040 // Avoid empty interfaces
    {
    }
}
