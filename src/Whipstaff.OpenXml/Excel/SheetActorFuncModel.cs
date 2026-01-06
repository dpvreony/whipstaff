// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;

namespace Whipstaff.OpenXml.Excel
{
    /// <summary>
    /// Represents a model for an actor that can manipulate a worksheet in a spreadsheet.
    /// </summary>
    /// <param name="Name">Name of the work sheet.</param>
    /// <param name="Actor">Action to carry out on the sheet.</param>
    public sealed record SheetActorFuncModel(string Name, Action<Sheet, WorksheetPart> Actor);
}
