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
