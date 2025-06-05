// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;

namespace Dhgms.DocFx.MermaidJs.Plugin.Markdig
{
    /// <summary>
    /// Block parser for Mermaid diagrams.
    /// </summary>
    public sealed class MermaidJsBlockParser : FencedBlockParserBase<MermaidCodeBlock>
    {
        /// <summary>
        /// the default prefix.
        /// </summary>
        public const string DefaultInfoPrefix = "language-";

        /// <summary>
        /// Initializes a new instance of the <see cref="MermaidJsBlockParser"/> class.
        /// </summary>
        public MermaidJsBlockParser()
        {
            OpeningCharacters = new[] { '`', '~' };
            InfoPrefix = DefaultInfoPrefix;
            InfoParser = MermaidInfoParser;
        }

        /// <inheritdoc/>
        public override BlockState TryContinue(BlockProcessor processor, Block block)
        {
            ArgumentNullException.ThrowIfNull(processor);
            ArgumentNullException.ThrowIfNull(block);

            var result = base.TryContinue(processor, block);
            if (result == BlockState.Continue && !processor.TrackTrivia)
            {
                var fence = (MermaidCodeBlock)block;

                // Remove any indent spaces
                var c = processor.CurrentChar;
                var indentCount = fence.IndentCount;
                while (indentCount > 0 && c.IsSpace())
                {
                    indentCount--;
                    c = processor.NextChar();
                }
            }

            return result;
        }

        /// <inheritdoc/>
        protected override MermaidCodeBlock CreateFencedBlock(BlockProcessor processor)
        {
            ArgumentNullException.ThrowIfNull(processor);

            var codeBlock = new MermaidCodeBlock(this)
            {
                IndentCount = processor.Indent,
            };

            if (processor.TrackTrivia)
            {
                codeBlock.LinesBefore = processor.UseLinesBefore();
                codeBlock.TriviaBefore = processor.UseTrivia(processor.Start - 1);
                codeBlock.NewLine = processor.Line.NewLine;
            }

            return codeBlock;
        }

        private static bool MermaidInfoParser(BlockProcessor state, ref StringSlice line, IFencedBlock fenced, char openingcharacter)
        {
            if (!DefaultInfoParser(state, ref line, fenced, openingcharacter))
            {
                return false;
            }

            return fenced.Info?.Equals("mermaid", StringComparison.Ordinal) == true;
        }
    }
}
