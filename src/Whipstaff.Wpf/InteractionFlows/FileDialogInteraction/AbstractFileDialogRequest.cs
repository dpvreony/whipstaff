// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;

namespace Whipstaff.Wpf.InteractionFlows.FileDialogInteraction
{
    /// <summary>
    /// Abstraction Request DTO for firing off a File Dialog interaction.
    /// </summary>
    /// <param name="AddExtension">Gets or sets a value indicating whether a file dialog automatically adds an extension to a file name if the user omits an extension.</param>
    /// <param name="AddToRecent">Gets or sets a value indicating whether the dialog box will add the item being opened or saved to the recent documents list.</param>
    /// <param name="CheckFileExists">Gets or sets a value indicating whether a file dialog displays a warning if the user specifies a file name that does not exist.</param>
    /// <param name="CheckPathExists">Gets or sets a value that specifies whether warnings are displayed if the user types invalid paths and file names.</param>
    /// <param name="ClientGuid">Gets or sets a GUID to associate with the dialog's persisted state.</param>
    /// <param name="CustomPlaces">Gets or sets the list of custom places for file dialog boxes.</param>
    /// <param name="DefaultDirectory">Gets or sets the directory displayed by the file dialog box if no recently used directory value is available.</param>
    /// <param name="DefaultExt">Gets or sets a value that specifies the default extension string to use to filter the list of files that are displayed.</param>
    /// <param name="DereferenceLinks">Gets or sets a value indicating whether a file dialog returns either the location of the file referenced by a shortcut or the location of the shortcut file (.lnk).</param>
    /// <param name="FileName">Gets or sets a string containing the full path of the file selected in a file dialog.</param>
    /// <param name="FileNames">Gets an array that contains one file name for each selected file.</param>
    /// <param name="Filter">Gets or sets the filter string that determines what types of files are displayed from either the OpenFileDialog or SaveFileDialog.</param>
    /// <param name="FilterIndex">Gets or sets the index of the filter currently selected in a file dialog.</param>
    /// <param name="InitialDirectory">Gets or sets the initial directory that is displayed by a file dialog.</param>
    /// <param name="RootDirectory">Gets or sets the directory displayed as the navigation root for the dialog.</param>
    /// <param name="ShowHiddenItems">Gets or sets a value indicating whether the dialog box will show hidden and system items regardless of user preferences.</param>
    /// <param name="Tag">Gets or sets an object associated with the dialog.This provides the ability to attach an arbitrary object to the dialog.</param>
    /// <param name="ValidateNames">Gets or sets a value indicating whether the dialog accepts only valid Win32 file names.</param>
    public abstract record AbstractFileDialogRequest(
        bool? AddExtension = null,
        bool? AddToRecent = null,
        bool? CheckFileExists = null,
        bool? CheckPathExists = null,
        Guid? ClientGuid = null,
        System.Collections.Generic.IList<Microsoft.Win32.FileDialogCustomPlace>? CustomPlaces = null,
        string? DefaultDirectory = null,
        string? DefaultExt = null,
        bool? DereferenceLinks = null,
        string? FileName = null,
#pragma warning disable CA1819 // Properties should not return arrays
        string[]? FileNames = null,
#pragma warning restore CA1819 // Properties should not return arrays
        string? Filter = null,
        int? FilterIndex = null,
        string? InitialDirectory = null,
        string? RootDirectory = null,
        bool? ShowHiddenItems = null,
        string? Tag = null,
        bool? ValidateNames = null);
}
