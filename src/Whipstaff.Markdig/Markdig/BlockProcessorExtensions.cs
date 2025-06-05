// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;

namespace Dhgms.DocFx.MermaidJs.Plugin.Markdig
{
    /// <summary>
    /// Extensions for <see cref="BlockProcessor"/>.
    /// </summary>
    public static class BlockProcessorExtensions
    {
        /// <summary>
        /// Returns the current stack of <see cref="BlockProcessor.LinesBefore"/> to assign it to a <see cref="Block"/>.
        /// Afterwards, the <see cref="BlockProcessor.LinesBefore"/> is set to null.
        /// </summary>
        /// <remarks>
        /// This is a copy of an internal markdig extension method, which we need to replicate the block behaviour.
        /// </remarks>
        /// <param name="blockProcessor">The block processor instance.</param>
        /// <returns>Lines before the code block.</returns>
#pragma warning disable CA1002 // Do not expose generic lists
        public static List<StringSlice> UseLinesBefore(this BlockProcessor blockProcessor)
#pragma warning restore CA1002 // Do not expose generic lists
        {
            ArgumentNullException.ThrowIfNull(blockProcessor);

            var linesBefore = blockProcessor.LinesBefore;
            blockProcessor.LinesBefore = null;
            return linesBefore!;
        }
    }
}
