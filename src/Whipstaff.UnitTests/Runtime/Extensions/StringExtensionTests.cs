// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Whipstaff.Runtime.Extensions;
using Xunit;

namespace Whipstaff.UnitTests.Runtime.Extensions
{
    /// <summary>
    /// Unit tests for the <see cref="StringExtensions"/> class.
    /// </summary>
    public static class StringExtensionTests
    {
        /// <summary>
        /// Unit tests for the <see cref="Whipstaff.Runtime.Extensions.StringExtensions.ActIfNotNullOrWhiteSpace"/> method.
        /// </summary>
        public sealed class ActIfNotNullOrWhiteSpaceMethodMethod
        {
            /// <summary>
            /// Tests that the invocation is carried out if the string is not null or whitespace.
            /// </summary>
            [Fact]
            public void DoesInvocation()
            {
                var result = false;

                "test".ActIfNotNullOrWhiteSpace(_ => result = true);

                Assert.True(result);
            }

            /// <summary>
            /// Tests that the invocation is carried out if the string is not null or whitespace.
            /// </summary>
            [Fact]
            public void NoInvocationInvocation()
            {
                var result = false;

                string.Empty.ActIfNotNullOrWhiteSpace(_ => result = true);

                Assert.False(result);
            }
        }

        /// <summary>
        /// Unit tests for the <see cref="Whipstaff.Runtime.Extensions.StringExtensions.IsHexadecimal"/> method.
        /// </summary>
        public sealed class IsHexadecimalMethodMethod
        {
            /// <summary>
            /// Tests that the method returns true when the string is hexadecimal.
            /// </summary>
            [Fact]
            public void ReturnsTrue()
            {
                var result = "0123456789ABCDEF".IsHexadecimal();

                Assert.True(result);
            }
        }

        /// <summary>
        /// Unit tests for the <see cref="Whipstaff.Runtime.Extensions.StringExtensions.Replace(string, Dictionary{char,char})"/> method.
        /// </summary>
        public sealed class ReplaceMethodWithCharDictionaryMethod
        {
            /// <summary>
            /// Tests that the method returns a string on success.
            /// </summary>
            [Fact]
            public void Succeeds()
            {
                var result = "test".Replace(new Dictionary<char, char>());

                Assert.NotNull(result);
            }
        }

        /// <summary>
        /// Unit tests for the <see cref="Whipstaff.Runtime.Extensions.StringExtensions.Replace(string, Dictionary{char,string})"/> method.
        /// </summary>
        public sealed class ReplaceMethodWithStringDictionaryMethod
        {
            /// <summary>
            /// Tests that the method returns a string on success.
            /// </summary>
            [Fact]
            public void Succeeds()
            {
                var result = "test".Replace(new Dictionary<char, string>
                {
                    { 't', "ll" }
                });

                Assert.NotNull(result);
                Assert.Equal("llesll", result);
            }
        }

        /// <summary>
        /// Unit tests for the <see cref="Whipstaff.Runtime.Extensions.StringExtensions.ReplaceMsWordSmartQuotesWithAscii"/> method.
        /// </summary>
        public sealed class ReplaceMsWordSmartQuotesWithAsciiMethod
        {
            /// <summary>
            /// Tests that the method returns a string on success.
            /// </summary>
            [Fact]
            public void Succeeds()
            {
                var result = "\u2026\u0092\u2014\u0085".ReplaceMsWordSmartQuotesWithAscii();

                Assert.NotNull(result);
                Assert.Equal("...'-...", result);
            }
        }

        /// <summary>
        /// Unit tests for the <see cref="Whipstaff.Runtime.Extensions.StringExtensions.Remove"/> method.
        /// </summary>
        public sealed class RemoveMethod
        {
            /// <summary>
            /// Tests that the method returns a string on success.
            /// </summary>
            [Fact]
            public void Succeeds()
            {
                var result = "test".Remove("est", StringComparison.Ordinal);

                Assert.NotNull(result);
            }
        }

        /// <summary>
        /// Unit tests for the <see cref="Whipstaff.Runtime.Extensions.StringExtensions.ToMemoryStream"/> method.
        /// </summary>
        public sealed class ToMemoryStreamMethod
        {
            /// <summary>
            /// Tests that the method returns a memory stream on success.
            /// </summary>
            [Fact]
            public void Succeeds()
            {
                var result = "test".ToMemoryStream();

                Assert.NotNull(result);
            }
        }

        /// <summary>
        /// Unit tests for the <see cref="Whipstaff.Runtime.Extensions.StringExtensions.ThrowIfNullOrWhitespace"/> method.
        /// </summary>
        public sealed class ThrowIfNullOrWhitespaceMethod
        {
            /// <summary>
            /// Tests that the method throws an <see cref="ArgumentNullException"/> when the string is null or whitespace.
            /// </summary>
            [Fact]
            public void Succeeds()
            {
                _ = Assert.Throws<ArgumentNullException>(() => ((string?)null).ThrowIfNullOrWhitespace());
            }
        }
    }
}
