// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using Microsoft.Extensions.Logging.Abstractions;
using Whipstaff.Mermaid.Playwright;
using Whipstaff.Playwright;

namespace Whipstaff.Benchmarks.Mermaid.Playwright
{
    /// <summary>
    /// Benchmarks for <see cref="PlaywrightRendererBrowserInstance"/>.
    /// </summary>
    public class PlaywrightRendererBrowserInstanceBenchmarks
    {
        private const string Markdown = """
                                        graph TD;
                                        A-->B;
                                        A-->C;
                                        B-->D;
                                        C-->D;
                                        """;

#pragma warning disable S1450 // Private fields only used as local variables in methods should become local variables
        private PlaywrightRenderer? _playwrightRenderer;
#pragma warning restore S1450 // Private fields only used as local variables in methods should become local variables
        private PlaywrightRendererBrowserInstance? _browserInstance;

        /// <summary>
        /// Global setup for benchmarks.
        /// </summary>
        [GlobalSetup]
        public void GlobalSetup()
        {
#pragma warning disable CA2000 // Dispose objects before losing scope
            _playwrightRenderer = PlaywrightRenderer.Default(new NullLoggerFactory());
#pragma warning restore CA2000 // Dispose objects before losing scope
#pragma warning disable VSTHRD002 // Avoid problematic synchronous waits
            _browserInstance = _playwrightRenderer
                .GetBrowserSessionAsync(PlaywrightBrowserTypeAndChannel.ChromiumDefault()).Result;
#pragma warning restore VSTHRD002 // Avoid problematic synchronous waits
        }

        /// <summary>
        /// Global cleanup for benchmarks.
        /// </summary>
        [GlobalCleanup]
        public void GlobalCleanup()
        {
            _browserInstance?.Dispose();
        }

        /// <summary>
        /// Benchmark for rendering a simple diagram.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Benchmark]
        public async Task GetDiagramAsync()
        {
            _ = await _browserInstance!.GetDiagramAsync(Markdown).ConfigureAwait(false);
        }
    }
}
