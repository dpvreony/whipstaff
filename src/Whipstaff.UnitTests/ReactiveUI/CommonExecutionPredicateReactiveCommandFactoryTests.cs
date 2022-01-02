// Copyright (c) 2020 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using Whipstaff.ReactiveUI.ReactiveCommands;
using Xunit;

namespace Whipstaff.UnitTests.ReactiveUI
{
    /// <summary>
    /// Unit Tests for the Common Execution Predicate Reactive Command Factory.
    /// </summary>
    public static class CommonExecutionPredicateReactiveCommandFactoryTests
    {
        /// <summary>
        /// Unit Tests for the Factory method for 2 commands that share a common execution predicate without individual execution constraints.
        /// </summary>
        public sealed class GetCommandsWithCommonExecutionPredicate2CommandsMethod : NetTestRegimentation.ITestMethodWithNullableParameters<IObservable<bool>, Func<int, int>, Func<int, int>>
        {
            /// <inheritdoc />
            public void ThrowsArgumentNullException(
                IObservable<bool> arg1,
                Func<int, int> arg2,
                Func<int, int> arg3,
                string expectedParameterNameForException)
            {
                var exception = Assert.Throws<ArgumentNullException>(() => CommonExecutionPredicateReactiveCommandFactory.GetCommandsWithCommonExecutionPredicate(
                    arg1,
                    arg2,
                    arg3));

                Assert.Equal(
                    expectedParameterNameForException,
                    exception.ParamName);
            }
        }

        /// <summary>
        /// Unit Tests for the Factory method for 3 commands that share a common execution predicate without individual execution constraints.
        /// </summary>
        public sealed class GetCommandsWithCommonExecutionPredicate3CommandsMethod : NetTestRegimentation.ITestMethodWithNullableParameters<IObservable<bool>, Func<int, int>, Func<int, int>, Func<int, int>>
        {
            /// <inheritdoc />
            public void ThrowsArgumentNullException(
                IObservable<bool> arg1,
                Func<int, int> arg2,
                Func<int, int> arg3,
                Func<int, int> arg4,
                string expectedParameterNameForException)
            {
                var exception = Assert.Throws<ArgumentNullException>(() => CommonExecutionPredicateReactiveCommandFactory.GetCommandsWithCommonExecutionPredicate(
                    arg1,
                    arg2,
                    arg3,
                    arg4));

                Assert.Equal(
                    expectedParameterNameForException,
                    exception.ParamName);
            }
        }

        /// <summary>
        /// Unit Tests for the Factory method for 4 commands that share a common execution predicate without individual execution constraints.
        /// </summary>
        public sealed class GetCommandsWithCommonExecutionPredicate4CommandsMethod : NetTestRegimentation.ITestMethodWithNullableParameters<IObservable<bool>, Func<int, int>, Func<int, int>, Func<int, int>, Func<int, int>>
        {
            /// <inheritdoc />
            public void ThrowsArgumentNullException(
                IObservable<bool> arg1,
                Func<int, int> arg2,
                Func<int, int> arg3,
                Func<int, int> arg4,
                Func<int, int> arg5,
                string expectedParameterNameForException)
            {
                var exception = Assert.Throws<ArgumentNullException>(() => CommonExecutionPredicateReactiveCommandFactory.GetCommandsWithCommonExecutionPredicate(
                    arg1,
                    arg2,
                    arg3,
                    arg4,
                    arg5));

                Assert.Equal(
                    expectedParameterNameForException,
                    exception.ParamName);
            }
        }

        /// <summary>
        /// Unit Tests for the Factory method for 5 commands that share a common execution predicate without individual execution constraints.
        /// </summary>
        public sealed class GetCommandsWithCommonExecutionPredicate5CommandsMethod : NetTestRegimentation.ITestMethodWithNullableParameters<IObservable<bool>, Func<int, int>, Func<int, int>, Func<int, int>, Func<int, int>, Func<int, int>>
        {
            /// <inheritdoc />
            public void ThrowsArgumentNullException(
                IObservable<bool> arg1,
                Func<int, int> arg2,
                Func<int, int> arg3,
                Func<int, int> arg4,
                Func<int, int> arg5,
                Func<int, int> arg6,
                string expectedParameterNameForException)
            {
                var exception = Assert.Throws<ArgumentNullException>(() => CommonExecutionPredicateReactiveCommandFactory.GetCommandsWithCommonExecutionPredicate(
                    arg1,
                    arg2,
                    arg3,
                    arg4,
                    arg5,
                    arg6));

                Assert.Equal(
                    expectedParameterNameForException,
                    exception.ParamName);
            }
        }

        /// <summary>
        /// Unit Tests for the Factory method for 6 commands that share a common execution predicate without individual execution constraints.
        /// </summary>
        public sealed class GetCommandsWithCommonExecutionPredicate6CommandsMethod : NetTestRegimentation.ITestMethodWithNullableParameters<IObservable<bool>, Func<int, int>, Func<int, int>, Func<int, int>, Func<int, int>, Func<int, int>, Func<int, int>>
        {
            /// <inheritdoc />
            public void ThrowsArgumentNullException(
                IObservable<bool> arg1,
                Func<int, int> arg2,
                Func<int, int> arg3,
                Func<int, int> arg4,
                Func<int, int> arg5,
                Func<int, int> arg6,
                Func<int, int> arg7,
                string expectedParameterNameForException)
            {
                var exception = Assert.Throws<ArgumentNullException>(() => CommonExecutionPredicateReactiveCommandFactory.GetCommandsWithCommonExecutionPredicate(
                    arg1,
                    arg2,
                    arg3,
                    arg4,
                    arg5,
                    arg6,
                    arg7));

                Assert.Equal(
                    expectedParameterNameForException,
                    exception.ParamName);
            }
        }
    }
}
