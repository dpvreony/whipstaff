// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

#pragma warning disable CA1707 // Test naming convention uses underscores for readability

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Abstractions;
using System.IO.Abstractions.TestingHelpers;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging.Abstractions;
using Whipstaff.YamlTemplating.DotNetTool;
using Whipstaff.YamlTemplating.DotNetTool.CommandLine;
using Xunit;

namespace Whipstaff.UnitTests.YamlTemplating.DotNetTool
{
    /// <summary>
    /// Tests for <see cref="CommandLineJob"/> using <see cref="MockFileSystem"/>.
    /// </summary>
    public sealed class CommandLineJobTests
    {
        private const string TemplatePath = "/template.yaml";
        private const string ContentPath = "/content.yaml";
        private const string OutputPath = "/output/result.yaml";

        /// <summary>Verifies a basic merge writes output and returns exit code 0.</summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task MergeMode_BasicMerge_WritesOutputAndReturnsZero()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { TemplatePath, new MockFileData("name: MyApp\nversion: \"1.0\"\n") },
                { ContentPath, new MockFileData("version: \"2.0\"\n") },
            });

            var job = CreateJob(fileSystem);
            var result = await job.HandleCommandAsync(CreateModel(fileSystem), TestContext.Current.CancellationToken);

            Assert.Equal(0, result);
            Assert.True(fileSystem.File.Exists(OutputPath));
            var output = await fileSystem.File.ReadAllTextAsync(OutputPath, TestContext.Current.CancellationToken);
            Assert.Contains("name: MyApp", output, StringComparison.Ordinal);
            Assert.Contains("version: 2.0", output, StringComparison.Ordinal);
        }

        /// <summary>Verifies content key overrides template key with content value winning.</summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task MergeMode_ContentKeyOverridesTemplateKey_ContentValueWins()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { TemplatePath, new MockFileData("debug: false\n") },
                { ContentPath, new MockFileData("debug: true\n") },
            });

            var job = CreateJob(fileSystem);
            var result = await job.HandleCommandAsync(CreateModel(fileSystem), TestContext.Current.CancellationToken);

            Assert.Equal(0, result);
            var output = await fileSystem.File.ReadAllTextAsync(OutputPath, TestContext.Current.CancellationToken);
            Assert.Contains("debug: true", output, StringComparison.Ordinal);
        }

        /// <summary>Verifies nested dictionaries are recursively merged.</summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task MergeMode_NestedDictionaries_RecursivelyMerged()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                {
                    TemplatePath, new MockFileData(
                        "settings:\n  debug: false\n  maxConnections: 10\n")
                },
                {
                    ContentPath, new MockFileData(
                        "settings:\n  debug: true\n")
                },
            });

            var job = CreateJob(fileSystem);
            var result = await job.HandleCommandAsync(CreateModel(fileSystem), TestContext.Current.CancellationToken);

            Assert.Equal(0, result);
            var output = await fileSystem.File.ReadAllTextAsync(OutputPath, TestContext.Current.CancellationToken);
            Assert.Contains("debug: true", output, StringComparison.Ordinal);
            Assert.Contains("maxConnections: 10", output, StringComparison.Ordinal);
        }

        /// <summary>Verifies template-only keys are preserved in the output.</summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task MergeMode_TemplateOnlyKey_PreservedInOutput()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { TemplatePath, new MockFileData("name: MyApp\nport: 5432\n") },
                { ContentPath, new MockFileData("name: Override\n") },
            });

            var job = CreateJob(fileSystem);
            var result = await job.HandleCommandAsync(CreateModel(fileSystem), TestContext.Current.CancellationToken);

            Assert.Equal(0, result);
            var output = await fileSystem.File.ReadAllTextAsync(OutputPath, TestContext.Current.CancellationToken);
            Assert.Contains("port: 5432", output, StringComparison.Ordinal);
        }

        /// <summary>Verifies content-only keys are added to the output.</summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task MergeMode_ContentOnlyKey_AddedToOutput()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { TemplatePath, new MockFileData("name: MyApp\n") },
                { ContentPath, new MockFileData("newKey: newValue\n") },
            });

            var job = CreateJob(fileSystem);
            var result = await job.HandleCommandAsync(CreateModel(fileSystem), TestContext.Current.CancellationToken);

            Assert.Equal(0, result);
            var output = await fileSystem.File.ReadAllTextAsync(OutputPath, TestContext.Current.CancellationToken);
            Assert.Contains("newKey: newValue", output, StringComparison.Ordinal);
        }

        /// <summary>Verifies that an unreadable template file causes exit code 1.</summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task MergeMode_TemplateFileUnreadable_ReturnsOne()
        {
            // Template file is absent — MockFileSystem will throw FileNotFoundException (an IOException)
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { ContentPath, new MockFileData("key: value\n") },
            });

            var job = CreateJob(fileSystem);
            var result = await job.HandleCommandAsync(CreateModel(fileSystem), TestContext.Current.CancellationToken);

            Assert.Equal(1, result);
        }

        /// <summary>Verifies that an unreadable content file causes exit code 1.</summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task MergeMode_ContentFileUnreadable_ReturnsOne()
        {
            // Content file is absent
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { TemplatePath, new MockFileData("key: value\n") },
            });

            var job = CreateJob(fileSystem);
            var result = await job.HandleCommandAsync(CreateModel(fileSystem), TestContext.Current.CancellationToken);

            Assert.Equal(1, result);
        }

        /// <summary>Verifies path injection at a single top-level segment.</summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task PathInjectionMode_ValidSingleSegmentPath_InjectsAtTopLevel()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { TemplatePath, new MockFileData("- run: dotnet build\n- run: dotnet test\n") },
                { ContentPath, new MockFileData("name: MyWorkflow\ntasks: placeholder\n") },
            });

            var job = CreateJob(fileSystem);
            var result = await job.HandleCommandAsync(CreateModel(fileSystem, "tasks"), TestContext.Current.CancellationToken);

            Assert.Equal(0, result);
            var output = await fileSystem.File.ReadAllTextAsync(OutputPath, TestContext.Current.CancellationToken);
            Assert.Contains("name: MyWorkflow", output, StringComparison.Ordinal);
            Assert.Contains("dotnet build", output, StringComparison.Ordinal);
        }

        /// <summary>Verifies path injection at a three-level deep dot-notation path.</summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task PathInjectionMode_ValidDeepPath_InjectsAtCorrectLocation()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { TemplatePath, new MockFileData("- run: dotnet build\n") },
                {
                    ContentPath, new MockFileData(
                        "name: MyWorkflow\nsteps:\n  build:\n    name: Build Step\n    tasks: placeholder\n")
                },
            });

            var job = CreateJob(fileSystem);
            var result = await job.HandleCommandAsync(CreateModel(fileSystem, "steps.build.tasks"), TestContext.Current.CancellationToken);

            Assert.Equal(0, result);
            var output = await fileSystem.File.ReadAllTextAsync(OutputPath, TestContext.Current.CancellationToken);
            Assert.Contains("name: MyWorkflow", output, StringComparison.Ordinal);
            Assert.Contains("Build Step", output, StringComparison.Ordinal);
            Assert.Contains("dotnet build", output, StringComparison.Ordinal);
        }

        /// <summary>Verifies that a path not found in content causes exit code 1.</summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task PathInjectionMode_PathNotFoundInContent_ReturnsOne()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { TemplatePath, new MockFileData("value: x\n") },
                { ContentPath, new MockFileData("name: MyWorkflow\n") },
            });

            var job = CreateJob(fileSystem);
            var result = await job.HandleCommandAsync(CreateModel(fileSystem, "missing.path"), TestContext.Current.CancellationToken);

            Assert.Equal(1, result);
        }

        /// <summary>Verifies that an intermediate scalar segment causes exit code 1.</summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task PathInjectionMode_IntermediateSegmentIsScalar_ReturnsOne()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { TemplatePath, new MockFileData("value: x\n") },
                { ContentPath, new MockFileData("name: scalar\n") },
            });

            var job = CreateJob(fileSystem);

            // "name" exists but is a scalar, not a mapping — navigation into "name.tasks" fails
            var result = await job.HandleCommandAsync(CreateModel(fileSystem, "name.tasks"), TestContext.Current.CancellationToken);

            Assert.Equal(1, result);
        }

        /// <summary>Verifies that a scalar template replaces an existing value at the specified path.</summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task PathInjectionMode_ScalarTemplate_ReplacesExistingValue()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { TemplatePath, new MockFileData("\"2.0\"\n") },
                { ContentPath, new MockFileData("name: MyApp\nversion: \"1.0\"\n") },
            });

            var job = CreateJob(fileSystem);
            var result = await job.HandleCommandAsync(CreateModel(fileSystem, "version"), TestContext.Current.CancellationToken);

            Assert.Equal(0, result);
            var output = await fileSystem.File.ReadAllTextAsync(OutputPath, TestContext.Current.CancellationToken);
            Assert.Contains("name: MyApp", output, StringComparison.Ordinal);
            Assert.Contains("version: 2.0", output, StringComparison.Ordinal);
        }

        /// <summary>Verifies that the output directory is created when it does not exist.</summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task MergeMode_OutputDirectoryCreated_WhenParentDoesNotExist()
        {
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { TemplatePath, new MockFileData("name: MyApp\n") },
                { ContentPath, new MockFileData("name: Override\n") },
            });

            var job = CreateJob(fileSystem);
            var result = await job.HandleCommandAsync(CreateModel(fileSystem), TestContext.Current.CancellationToken);

            Assert.Equal(0, result);
            Assert.True(fileSystem.File.Exists(OutputPath));
        }

        /// <summary>Verifies that the output file starts with a generated-by header comment containing the tool name, version, and date.</summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [Fact]
        public async Task MergeMode_OutputFileHasGeneratedByHeaderComment()
        {
            var fixedUtcNow = new DateTimeOffset(2026, 3, 30, 12, 0, 0, TimeSpan.Zero);
            var fileSystem = new MockFileSystem(new Dictionary<string, MockFileData>
            {
                { TemplatePath, new MockFileData("name: MyApp\n") },
                { ContentPath, new MockFileData("env: production\n") },
            });

            var job = CreateJob(fileSystem, new FixedTimeProvider(fixedUtcNow));
            var result = await job.HandleCommandAsync(CreateModel(fileSystem), TestContext.Current.CancellationToken);

            Assert.Equal(0, result);
            var output = await fileSystem.File.ReadAllTextAsync(OutputPath, TestContext.Current.CancellationToken);
            var assembly = typeof(CommandLineJob).Assembly;
            var expectedToolName = assembly.GetName().Name;
            var expectedVersion = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? "unknown";
            var expectedDate = fixedUtcNow.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            var expectedHeader = $"# Generated by {expectedToolName} v{expectedVersion} on {expectedDate}";
            Assert.StartsWith(expectedHeader, output, StringComparison.Ordinal);
        }

        private static CommandLineJob CreateJob(MockFileSystem fileSystem, TimeProvider? timeProvider = null)
        {
            var logMessageActions = new CommandLineJobLogMessageActions();
            var logger = new NullLogger<CommandLineJob>();
            var wrapper = new CommandLineJobLogMessageActionsWrapper(logMessageActions, logger);
            return new CommandLineJob(fileSystem, timeProvider ?? TimeProvider.System, wrapper);
        }

        private static CommandLineArgModel CreateModel(MockFileSystem fileSystem, string? yamlPath = null)
        {
            return new CommandLineArgModel(
                fileSystem.FileInfo.New(TemplatePath),
                fileSystem.FileInfo.New(ContentPath),
                fileSystem.FileInfo.New(OutputPath),
                yamlPath);
        }

        private sealed class FixedTimeProvider(DateTimeOffset fixedUtcNow) : TimeProvider
        {
            public override DateTimeOffset GetUtcNow() => fixedUtcNow;
        }
    }
}
