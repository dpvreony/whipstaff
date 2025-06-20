// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.CommandLine;
using NetTestRegimentation.XUnit.Theories.ArgumentNullException;

namespace Whipstaff.UnitTests.TestSources.CommandLine.CommandExtensions.MakeOptionsMutuallyExclusiveMethod
{
    /// <summary>
    /// Test Source for <see cref="Whipstaff.UnitTests.CommandLine.CommandExtensionTests.MakeOptionsMutuallyExclusiveMethod.ThrowsArgumentNullException"/>.
    /// </summary>
    public sealed class ThrowsArgumentNullExceptionTestSource : ArgumentNullExceptionTheoryData<Command, Option, Option>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThrowsArgumentNullExceptionTestSource"/> class.
        /// </summary>
        public ThrowsArgumentNullExceptionTestSource()
            : base(
                new NamedParameterInput<Command>(
                    "command",
                    () => new RootCommand("Test Root Command")),
                new NamedParameterInput<Option>(
                    "option1",
                    () =>
                    new Option<bool>("--option1")
                    {
                        Description = "The first exclusive option"
                    }),
                new NamedParameterInput<Option>(
                    "option2",
                    () =>
                    new Option<bool>("--option2")
                    {
                        Description = "The second exclusive option"
                    }))
        {
        }
    }
}
