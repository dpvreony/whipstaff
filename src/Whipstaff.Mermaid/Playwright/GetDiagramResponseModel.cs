// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

namespace Whipstaff.Mermaid.Playwright
{
    /// <summary>
    /// Response Model for PlaywrightRenderer GetDiagram calls.
    /// </summary>
    /// <param name="Svg">The diagram in SVG format.</param>
    /// <param name="Png">PNG image of the diagram as a byte array.</param>
#pragma warning disable CA1819 // Properties should not return arrays
    public sealed record GetDiagramResponseModel(string Svg, byte[] Png)
#pragma warning restore CA1819 // Properties should not return arrays
    {
    }
}
