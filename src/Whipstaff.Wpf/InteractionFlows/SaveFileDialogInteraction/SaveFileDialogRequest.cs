// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Whipstaff.Core.Entities;

namespace Whipstaff.Wpf.InteractionFlows.SaveFileDialogInteraction
{
    /// <summary>
    /// Request DTO for firing off a Save File Dialog interaction.
    /// </summary>
    /// <param name="Title">The title to set on the dialog.</param>
    public record SaveFileDialogRequest(string Title) : ITitle
    {
    }
}
