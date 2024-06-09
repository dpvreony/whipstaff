// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Whipstaff.Wpf.InteractionFlows
{
    /// <summary>
    /// Represents an interaction request where the user can acknowledge or dismiss the message.
    /// </summary>
    /// <param name="Title">The title of the dialog.</param>
    /// <param name="Message">The message to display in the dialog.</param>
    /// <param name="OkButtonText">The text to show on the ok button.</param>
    /// <param name="CancelButtonText">The text to show on the cancel button.</param>
    /// <param name="DefaultAffirmative">The affirmative action is the default action of the dialog.</param>
    public sealed record OkCancelInteractionRequest(
        string Title,
        string Message,
        string OkButtonText,
        string CancelButtonText,
        bool DefaultAffirmative)
        : OkOnlyInteractionRequest(
            Title,
            Message,
            OkButtonText);
}
