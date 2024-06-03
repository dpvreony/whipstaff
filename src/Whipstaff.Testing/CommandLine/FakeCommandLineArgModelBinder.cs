﻿// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.CommandLine;
using System.CommandLine.Binding;
using System.IO;

namespace Whipstaff.Testing.CommandLine
{
    /// <summary>
    /// Command Line Argument Model Binder for <see cref="FakeCommandLineArgModel"/>.
    /// </summary>
    public sealed class FakeCommandLineArgModelBinder : BinderBase<FakeCommandLineArgModel>
    {
        private readonly Argument<FileInfo> _fileArgument;
        private readonly Argument<string?> _nameArgument;

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeCommandLineArgModelBinder"/> class.
        /// </summary>
        /// <param name="fileArgument">file argument to parse and bind against.</param>
        /// <param name="nameArgument">name argument to parse and bind against.</param>
        public FakeCommandLineArgModelBinder(
            Argument<FileInfo> fileArgument,
            Argument<string?> nameArgument)
        {
            _fileArgument = fileArgument;
            _nameArgument = nameArgument;
        }

        /// <inheritdoc/>
        protected override FakeCommandLineArgModel GetBoundValue(BindingContext bindingContext)
        {
            ArgumentNullException.ThrowIfNull(bindingContext);
            var fileName = bindingContext.ParseResult.GetValueForArgument(_fileArgument);
            var name = bindingContext.ParseResult.GetValueForArgument(_nameArgument);
            return new FakeCommandLineArgModel(fileName, name, null);
        }
    }
}
