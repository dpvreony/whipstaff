// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Whipstaff.Wpf.InteractionFlows
{
    /// <summary>
    /// Represents an interaction request where the user can acknowledge an error.
    /// </summary>
    /// <param name="Message">The message to show in the dialog.</param>
    public sealed record ErrorOnlyInteractionRequest(string Message);
}
