// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using Markdig.Syntax;

namespace Dhgms.DocFx.MermaidJs.Plugin.Markdig
{
    /// <summary>
    /// Represents a mermaid code block.
    /// </summary>
    public sealed class MermaidCodeBlock : FencedCodeBlock
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MermaidCodeBlock"/> class.
        /// </summary>
        /// <param name="parser">Block parser to use.</param>
        public MermaidCodeBlock(MermaidJsBlockParser parser)
            : base(parser)
        {
        }
    }
}
