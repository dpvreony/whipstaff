// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive;
using Whipstaff.ReactiveUI.ReactiveCommands;
using Xunit;

namespace Whipstaff.UnitTests.ReactiveUI
{
    /// <summary>
    /// Unit Tests for <see cref="ExclusiveReactiveCommandsFactory"/>.
    /// </summary>
    public static class ExclusiveReactiveCommandsFactoryTests
    {
        /// <summary>
        /// Unit tests for <see cref="ExclusiveReactiveCommandsFactory.GetExclusiveCommands{T1Param, T1CommandResult, T1ExecuteResult, T2Param, T2CommandResult, T2ExecuteResult}(ReactiveCommandFactoryArgument{T1Param, T1CommandResult, T1ExecuteResult}, ReactiveCommandFactoryArgument{T2Param, T2CommandResult, T2ExecuteResult})"/>.
        /// </summary>
        public sealed class CreateT6Method : NetTestRegimentation.ITestMethodWithNullableParameters<
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>>
        {
            /// <inheritdoc/>
            [Theory]
            [ClassData(typeof(CreateT6MethodThrowsArgumentNullTestSource))]
            public void ThrowsArgumentNullException(
                ReactiveCommandFactoryArgument<Unit, Unit, Unit> arg1,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit> arg2,
                string expectedParameterNameForException)
            {
                _ = Assert.Throws<ArgumentNullException>(
                    expectedParameterNameForException,
                    () => ExclusiveReactiveCommandsFactory.GetExclusiveCommands(
                        arg1,
                        arg2));
            }
        }

        /// <summary>
        /// Unit tests for <see cref="ExclusiveReactiveCommandsFactory.GetExclusiveCommands{T1Param, T1CommandResult, T1ExecuteResult, T2Param, T2CommandResult, T2ExecuteResult, T3Param, T3CommandResult, T3ExecuteResult}(ReactiveCommandFactoryArgument{T1Param, T1CommandResult, T1ExecuteResult}, ReactiveCommandFactoryArgument{T2Param, T2CommandResult, T2ExecuteResult}, ReactiveCommandFactoryArgument{T3Param, T3CommandResult, T3ExecuteResult})"/>.
        /// </summary>
        public sealed class CreateT9Method : NetTestRegimentation.ITestMethodWithNullableParameters<
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>>
        {
            /// <inheritdoc/>
            [Theory]
            [ClassData(typeof(CreateT9MethodThrowsArgumentNullTestSource))]
            public void ThrowsArgumentNullException(
                ReactiveCommandFactoryArgument<Unit, Unit, Unit> arg1,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit> arg2,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit> arg3,
                string expectedParameterNameForException)
            {
                _ = Assert.Throws<ArgumentNullException>(
                    expectedParameterNameForException,
                    () => ExclusiveReactiveCommandsFactory.GetExclusiveCommands(
                        arg1,
                        arg2,
                        arg3));
            }
        }

        /// <summary>
        /// Unit tests for <see cref="ExclusiveReactiveCommandsFactory.GetExclusiveCommands{T1Param, T1CommandResult, T1ExecuteResult, T2Param, T2CommandResult, T2ExecuteResult, T3Param, T3CommandResult, T3ExecuteResult, T4Param, T4CommandResult, T4ExecuteResult}(ReactiveCommandFactoryArgument{T1Param, T1CommandResult, T1ExecuteResult}, ReactiveCommandFactoryArgument{T2Param, T2CommandResult, T2ExecuteResult}, ReactiveCommandFactoryArgument{T3Param, T3CommandResult, T3ExecuteResult}, ReactiveCommandFactoryArgument{T4Param, T4CommandResult, T4ExecuteResult})"/>.
        /// </summary>
        public sealed class CreateT12Method : NetTestRegimentation.ITestMethodWithNullableParameters<
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>>
        {
            /// <inheritdoc/>
            [Theory]
            [ClassData(typeof(CreateT12MethodThrowsArgumentNullTestSource))]
            public void ThrowsArgumentNullException(
                ReactiveCommandFactoryArgument<Unit, Unit, Unit> arg1,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit> arg2,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit> arg3,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit> arg4,
                string expectedParameterNameForException)
            {
                _ = Assert.Throws<ArgumentNullException>(
                    expectedParameterNameForException,
                    () => ExclusiveReactiveCommandsFactory.GetExclusiveCommands(
                        arg1,
                        arg2,
                        arg3,
                        arg4));
            }
        }

        /// <summary>
        /// Unit tests for <see cref="ExclusiveReactiveCommandsFactory.GetExclusiveCommands{T1Param, T1CommandResult, T1ExecuteResult, T2Param, T2CommandResult, T2ExecuteResult, T3Param, T3CommandResult, T3ExecuteResult, T4Param, T4CommandResult, T4ExecuteResult, T5Param, T5CommandResult, T5ExecuteResult}(ReactiveCommandFactoryArgument{T1Param, T1CommandResult, T1ExecuteResult}, ReactiveCommandFactoryArgument{T2Param, T2CommandResult, T2ExecuteResult}, ReactiveCommandFactoryArgument{T3Param, T3CommandResult, T3ExecuteResult}, ReactiveCommandFactoryArgument{T4Param, T4CommandResult, T4ExecuteResult}, ReactiveCommandFactoryArgument{T5Param, T5CommandResult, T5ExecuteResult})"/>.
        /// </summary>
        public sealed class CreateT15Method : NetTestRegimentation.ITestMethodWithNullableParameters<
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>>
        {
            /// <inheritdoc/>
            [Theory]
            [ClassData(typeof(CreateT15MethodThrowsArgumentNullTestSource))]
            public void ThrowsArgumentNullException(
                ReactiveCommandFactoryArgument<Unit, Unit, Unit> arg1,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit> arg2,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit> arg3,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit> arg4,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit> arg5,
                string expectedParameterNameForException)
            {
                _ = Assert.Throws<ArgumentNullException>(
                    expectedParameterNameForException,
                    () => ExclusiveReactiveCommandsFactory.GetExclusiveCommands(
                        arg1,
                        arg2,
                        arg3,
                        arg4,
                        arg5));
            }
        }

        /// <summary>
        /// Unit tests for <see cref="ExclusiveReactiveCommandsFactory.GetExclusiveCommands{T1Param, T1CommandResult, T1ExecuteResult, T2Param, T2CommandResult, T2ExecuteResult, T3Param, T3CommandResult, T3ExecuteResult, T4Param, T4CommandResult, T4ExecuteResult, T5Param, T5CommandResult, T5ExecuteResult, T6Param, T6CommandResult, T6ExecuteResult}(ReactiveCommandFactoryArgument{T1Param, T1CommandResult, T1ExecuteResult}, ReactiveCommandFactoryArgument{T2Param, T2CommandResult, T2ExecuteResult}, ReactiveCommandFactoryArgument{T3Param, T3CommandResult, T3ExecuteResult}, ReactiveCommandFactoryArgument{T4Param, T4CommandResult, T4ExecuteResult}, ReactiveCommandFactoryArgument{T5Param, T5CommandResult, T5ExecuteResult}, ReactiveCommandFactoryArgument{T6Param, T6CommandResult, T6ExecuteResult})"/>.
        /// </summary>
        public sealed class CreateT18Method : NetTestRegimentation.ITestMethodWithNullableParameters<
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>>
        {
            /// <inheritdoc/>
            [Theory]
            [ClassData(typeof(CreateT18MethodThrowsArgumentNullTestSource))]
            public void ThrowsArgumentNullException(
                ReactiveCommandFactoryArgument<Unit, Unit, Unit> arg1,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit> arg2,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit> arg3,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit> arg4,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit> arg5,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit> arg6,
                string expectedParameterNameForException)
            {
                _ = Assert.Throws<ArgumentNullException>(
                    expectedParameterNameForException,
                    () => ExclusiveReactiveCommandsFactory.GetExclusiveCommands(
                        arg1,
                        arg2,
                        arg3,
                        arg4,
                        arg5,
                        arg6));
            }
        }

        /// <summary>
        /// Unit tests for <see cref="ExclusiveReactiveCommandsFactory.GetExclusiveCommands{T1Param, T1CommandResult, T1ExecuteResult, T2Param, T2CommandResult, T2ExecuteResult, T3Param, T3CommandResult, T3ExecuteResult, T4Param, T4CommandResult, T4ExecuteResult, T5Param, T5CommandResult, T5ExecuteResult, T6Param, T6CommandResult, T6ExecuteResult, T7Param, T7CommandResult, T7ExecuteResult}(ReactiveCommandFactoryArgument{T1Param, T1CommandResult, T1ExecuteResult}, ReactiveCommandFactoryArgument{T2Param, T2CommandResult, T2ExecuteResult}, ReactiveCommandFactoryArgument{T3Param, T3CommandResult, T3ExecuteResult}, ReactiveCommandFactoryArgument{T4Param, T4CommandResult, T4ExecuteResult}, ReactiveCommandFactoryArgument{T5Param, T5CommandResult, T5ExecuteResult}, ReactiveCommandFactoryArgument{T6Param, T6CommandResult, T6ExecuteResult}, ReactiveCommandFactoryArgument{T7Param, T7CommandResult, T7ExecuteResult})"/>.
        /// </summary>
        public sealed class CreateT21Method : NetTestRegimentation.ITestMethodWithNullableParameters<
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>>
        {
            /// <inheritdoc/>
            [Theory]
            [ClassData(typeof(CreateT21MethodThrowsArgumentNullTestSource))]
            public void ThrowsArgumentNullException(
                ReactiveCommandFactoryArgument<Unit, Unit, Unit> arg1,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit> arg2,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit> arg3,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit> arg4,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit> arg5,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit> arg6,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit> arg7,
                string expectedParameterNameForException)
            {
                _ = Assert.Throws<ArgumentNullException>(
                    expectedParameterNameForException,
                    () => ExclusiveReactiveCommandsFactory.GetExclusiveCommands(
                        arg1,
                        arg2,
                        arg3,
                        arg4,
                        arg5,
                        arg6,
                        arg7));
            }
        }

        /// <summary>
        /// Unit tests for <see cref="ExclusiveReactiveCommandsFactory.GetExclusiveCommands{T1Param, T1CommandResult, T1ExecuteResult, T2Param, T2CommandResult, T2ExecuteResult, T3Param, T3CommandResult, T3ExecuteResult, T4Param, T4CommandResult, T4ExecuteResult, T5Param, T5CommandResult, T5ExecuteResult, T6Param, T6CommandResult, T6ExecuteResult, T7Param, T7CommandResult, T7ExecuteResult, T8Param, T8CommandResult, T8ExecuteResult}(ReactiveCommandFactoryArgument{T1Param, T1CommandResult, T1ExecuteResult}, ReactiveCommandFactoryArgument{T2Param, T2CommandResult, T2ExecuteResult}, ReactiveCommandFactoryArgument{T3Param, T3CommandResult, T3ExecuteResult}, ReactiveCommandFactoryArgument{T4Param, T4CommandResult, T4ExecuteResult}, ReactiveCommandFactoryArgument{T5Param, T5CommandResult, T5ExecuteResult}, ReactiveCommandFactoryArgument{T6Param, T6CommandResult, T6ExecuteResult}, ReactiveCommandFactoryArgument{T7Param, T7CommandResult, T7ExecuteResult}, ReactiveCommandFactoryArgument{T8Param, T8CommandResult, T8ExecuteResult})"/>.
        /// </summary>
        public sealed class CreateT24Method : NetTestRegimentation.ITestMethodWithNullableParameters<
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>,
            ReactiveCommandFactoryArgument<Unit, Unit, Unit>>
        {
            /// <inheritdoc/>
            [Theory]
            [ClassData(typeof(CreateT24MethodThrowsArgumentNullTestSource))]
            public void ThrowsArgumentNullException(
                ReactiveCommandFactoryArgument<Unit, Unit, Unit> arg1,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit> arg2,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit> arg3,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit> arg4,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit> arg5,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit> arg6,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit> arg7,
                ReactiveCommandFactoryArgument<Unit, Unit, Unit> arg8,
                string expectedParameterNameForException)
            {
                _ = Assert.Throws<ArgumentNullException>(
                    expectedParameterNameForException,
                    () => ExclusiveReactiveCommandsFactory.GetExclusiveCommands(
                        arg1,
                        arg2,
                        arg3,
                        arg4,
                        arg5,
                        arg6,
                        arg7,
                        arg8));
            }
        }
    }
}
