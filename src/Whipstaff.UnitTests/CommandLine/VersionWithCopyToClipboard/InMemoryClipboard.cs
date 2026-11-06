// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Threading;
using System.Threading.Tasks;
using TextCopy;

namespace Whipstaff.UnitTests.CommandLine.VersionWithCopyToClipboard
{
    internal sealed class InMemoryClipboard : IClipboard
    {
        private string? _text;

        public Task<string?> GetTextAsync(CancellationToken cancellation = default)
        {
            return Task.FromResult(_text);
        }

        public string? GetText()
        {
            return _text;
        }

        public Task SetTextAsync(string text, CancellationToken cancellation = default)
        {
            _text = text;
            return Task.CompletedTask;
        }

        public void SetText(string text)
        {
            _text = text;
        }
    }
}
