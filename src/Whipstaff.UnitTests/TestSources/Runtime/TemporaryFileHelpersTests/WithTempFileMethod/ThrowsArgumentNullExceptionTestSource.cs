// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Xunit;

namespace Whipstaff.UnitTests.TestSources.Runtime.TemporaryFileHelpersTests.WithTempFileMethod
{
    /// <summary>
    /// Test Source for <see cref="Whipstaff.UnitTests.Runtime.TemporaryFileHelpersTests.WithTempFileMethod.ThrowsArgumentNullException"/>.
    /// </summary>
    public sealed class ThrowsArgumentNullExceptionTestSource : TheoryData<byte[]?, string?, Action<string>?, string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThrowsArgumentNullExceptionTestSource"/> class.
        /// </summary>
        public ThrowsArgumentNullExceptionTestSource()
        {
            var arg1 = new byte[] { 1 };
            const string arg2 = ".txt";
            Action<string> arg3 = _ =>
            {
            };

            Add(null, arg2, arg3, "fileAsBytes");
            Add(arg1, null, arg3, "fileExtension");
            Add(arg1, arg2, null, "fileAction");
        }
    }
}
