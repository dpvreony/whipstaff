// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.CommandLine;
using System.CommandLine.Parsing;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using NetTestRegimentation.XUnit.Theories.ArgumentNullException;

namespace Whipstaff.UnitTests.TestSources.CommandLine.SymbolResultHelpersTests.FileHasSupportedExtension2Method
{
    /// <summary>
    /// Test Source for <see cref="Whipstaff.UnitTests.CommandLine.SymbolResultHelpersTests.FileHasSupportedExtension2Method.ThrowsArgumentNullException"/>.
    /// </summary>
    public sealed class ThrowsArgumentNullExceptionTestSource : ArgumentNullExceptionTheoryData<SymbolResult, IFileSystem, string[]>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThrowsArgumentNullExceptionTestSource"/> class.
        /// </summary>
        public ThrowsArgumentNullExceptionTestSource()
            : base(
                new NamedParameterInput<SymbolResult>("result", () =>
                    {
                        var argument1Builder = new Argument<string>("--filename");
                        var rootCommand = new RootCommand
                        {
                            argument1Builder
                        };

                        var parseResults = rootCommand.Parse("--filename somefilename.txt");

                        return parseResults.GetResult(argument1Builder)!;
                    }),
                new NamedParameterInput<IFileSystem>(
                    "fileSystem",
                    () => new MockFileSystem()),
                new NamedParameterInput<string[]>(
                    "extensions",
                    () => [".txt"]))
        {
        }
    }
}
