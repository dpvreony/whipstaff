// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.IO.Abstractions;

namespace Whipstaff.Testing.CommandLine
{
    /// <summary>
    /// Model that represents the fake command line arguments for unit testing.
    /// </summary>
    /// <param name="FileName">The parsed test filename argument.</param>
    /// <param name="Name">The parsed test name argument.</param>
    /// <param name="TestExceptionFunc">Function to generate an exception the test harness is expecting.</param>
    public sealed record FakeCommandLineArgModel(IFileInfo FileName, string? Name, Func<Exception>? TestExceptionFunc)
    {
    }
}
