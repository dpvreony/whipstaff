﻿// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.IO;

namespace Whipstaff.Testing.CommandLine
{
    /// <summary>
    /// Model that represents the fake command line arguments for unit testing.
    /// </summary>
    /// <param name="FileName">The parsed test filename argument.</param>
    /// <param name="Name">The parsed test name argument.</param>
    public sealed record FakeCommandLineArgModel(FileInfo FileName, string? Name)
    {
    }
}
