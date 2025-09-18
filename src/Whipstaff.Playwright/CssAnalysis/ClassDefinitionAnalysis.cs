// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;

namespace Whipstaff.Playwright.CssAnalysis
{
    /// <summary>
    /// Model for the results of a CSS class definition analysis.
    /// </summary>
    /// <param name="Used">The classes used in a page.</param>
    /// <param name="Defined">The classes that are defined in stylesheets.</param>
    /// <param name="Undefined">The classes that are not defined in stylesheets.</param>
    public record ClassDefinitionAnalysis(HashSet<string> Used, HashSet<string> Defined, HashSet<string> Undefined);
}
