// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Whipstaff.Wpf.InteractionFlows
{
    /// <summary>
    /// Represents an interaction request where the user can only acknowledge the message.
    /// </summary>
    /// <param name="Title">The title of the dialog.</param>
    /// <param name="Message">The message to display in the dialog.</param>
    /// <param name="OkButtonText">The text to show on the ok button.</param>
    public record OkOnlyInteractionRequest(
        string Title,
        string Message,
        string OkButtonText);
}
