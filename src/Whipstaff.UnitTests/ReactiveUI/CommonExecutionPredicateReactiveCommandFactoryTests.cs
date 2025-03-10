// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Reactive.Subjects;
using Whipstaff.ReactiveUI.ReactiveCommands;
using Xunit;

namespace Whipstaff.UnitTests.ReactiveUI
{
    /// <summary>
    /// Unit Tests for the Common Execution Predicate Reactive Command Factory.
    /// </summary>
    public static class CommonExecutionPredicateReactiveCommandFactoryTests
    {
        private static Subject<bool> BasicObservable() => new();

        private static Func<int, int> BasicFunc() => input => input;

        /// <summary>
        /// Unit Tests for the Factory method for 2 commands that share a common execution predicate without individual execution constraints.
        /// </summary>
        public sealed class GetCommandsWithCommonExecutionPredicate2CommandsMethod : NetTestRegimentation.ITestMethodWithNullableParameters<IObservable<bool>, Func<int, int>, Func<int, int>>
        {
            /// <inheritdoc />
            [Theory]
            [ClassData(typeof(ThrowsArgumentNullExceptionTestSource))]
            public void ThrowsArgumentNullException(
                IObservable<bool>? arg1,
                Func<int, int>? arg2,
                Func<int, int>? arg3,
                string expectedParameterNameForException)
            {
                var exception = Assert.Throws<ArgumentNullException>(() => CommonExecutionPredicateReactiveCommandFactory.GetCommandsWithCommonExecutionPredicate(
                    arg1!,
                    arg2!,
                    arg3!));

                Assert.Equal(
                    expectedParameterNameForException,
                    exception.ParamName);
            }

            /// <summary>
            /// Test source for <see cref="ThrowsArgumentNullException" />.
            /// </summary>
            public sealed class ThrowsArgumentNullExceptionTestSource
                : TheoryData<IObservable<bool>?, Func<int, int>?, Func<int, int>?, string>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="ThrowsArgumentNullExceptionTestSource"/> class.
                /// </summary>
                public ThrowsArgumentNullExceptionTestSource()
                {
#pragma warning disable CA2000 // Dispose objects before losing scope
                    Add(
                        null,
                        BasicFunc(),
                        BasicFunc(),
                        "canExecuteObservable");

                    Add(
                        BasicObservable(),
                        null,
                        BasicFunc(),
                        "commandFunc1");

                    Add(
                        BasicObservable(),
                        BasicFunc(),
                        null,
                        "commandFunc2");
#pragma warning restore CA2000 // Dispose objects before losing scope
                }
            }
        }

        /// <summary>
        /// Unit Tests for the Factory method for 3 commands that share a common execution predicate without individual execution constraints.
        /// </summary>
        public sealed class GetCommandsWithCommonExecutionPredicate3CommandsMethod : NetTestRegimentation.ITestMethodWithNullableParameters<IObservable<bool>, Func<int, int>, Func<int, int>, Func<int, int>>
        {
            /// <inheritdoc />
            [Theory]
            [ClassData(typeof(ThrowsArgumentNullExceptionTestSource))]
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

            /// <summary>
            /// Test source for <see cref="ThrowsArgumentNullException" />.
            /// </summary>
            public sealed class ThrowsArgumentNullExceptionTestSource
                : TheoryData<IObservable<bool>?, Func<int, int>?, Func<int, int>?, Func<int, int>?, string>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="ThrowsArgumentNullExceptionTestSource"/> class.
                /// </summary>
                public ThrowsArgumentNullExceptionTestSource()
                {
#pragma warning disable CA2000 // Dispose objects before losing scope
                    Add(
                        null,
                        BasicFunc(),
                        BasicFunc(),
                        BasicFunc(),
                        "canExecuteObservable");

                    Add(
                        BasicObservable(),
                        null,
                        BasicFunc(),
                        BasicFunc(),
                        "commandFunc1");

                    Add(
                        BasicObservable(),
                        BasicFunc(),
                        null,
                        BasicFunc(),
                        "commandFunc2");

                    Add(
                        BasicObservable(),
                        BasicFunc(),
                        BasicFunc(),
                        null,
                        "commandFunc3");
#pragma warning restore CA2000 // Dispose objects before losing scope
                }
            }
        }

        /// <summary>
        /// Unit Tests for the Factory method for 4 commands that share a common execution predicate without individual execution constraints.
        /// </summary>
        public sealed class GetCommandsWithCommonExecutionPredicate4CommandsMethod : NetTestRegimentation.
            ITestMethodWithNullableParameters<IObservable<bool>, Func<int, int>, Func<int, int>, Func<int, int>,
                Func<int, int>>
        {
            /// <inheritdoc />
            [Theory]
            [ClassData(typeof(ThrowsArgumentNullExceptionTestSource))]
            public void ThrowsArgumentNullException(
                IObservable<bool> arg1,
                Func<int, int> arg2,
                Func<int, int> arg3,
                Func<int, int> arg4,
                Func<int, int> arg5,
                string expectedParameterNameForException)
            {
                var exception = Assert.Throws<ArgumentNullException>(() =>
                    CommonExecutionPredicateReactiveCommandFactory.GetCommandsWithCommonExecutionPredicate(
                        arg1,
                        arg2,
                        arg3,
                        arg4,
                        arg5));

                Assert.Equal(
                    expectedParameterNameForException,
                    exception.ParamName);
            }

            /// <summary>
            /// Test source for <see cref="ThrowsArgumentNullException" />.
            /// </summary>
            public sealed class ThrowsArgumentNullExceptionTestSource
                : TheoryData<IObservable<bool>?, Func<int, int>?, Func<int, int>?, Func<int, int>?, Func<int, int>?,
                    string>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="ThrowsArgumentNullExceptionTestSource"/> class.
                /// </summary>
                public ThrowsArgumentNullExceptionTestSource()
                {
#pragma warning disable CA2000 // Dispose objects before losing scope
                    Add(
                        null,
                        BasicFunc(),
                        BasicFunc(),
                        BasicFunc(),
                        BasicFunc(),
                        "canExecuteObservable");

                    Add(
                        BasicObservable(),
                        null,
                        BasicFunc(),
                        BasicFunc(),
                        BasicFunc(),
                        "commandFunc1");

                    Add(
                        BasicObservable(),
                        BasicFunc(),
                        null,
                        BasicFunc(),
                        BasicFunc(),
                        "commandFunc2");

                    Add(
                        BasicObservable(),
                        BasicFunc(),
                        BasicFunc(),
                        null,
                        BasicFunc(),
                        "commandFunc3");

                    Add(
                        BasicObservable(),
                        BasicFunc(),
                        BasicFunc(),
                        BasicFunc(),
                        null,
                        "commandFunc4");
#pragma warning restore CA2000 // Dispose objects before losing scope
                }
            }
        }

        /// <summary>
        /// Unit Tests for the Factory method for 5 commands that share a common execution predicate without individual execution constraints.
        /// </summary>
        public sealed class GetCommandsWithCommonExecutionPredicate5CommandsMethod : NetTestRegimentation.ITestMethodWithNullableParameters<IObservable<bool>, Func<int, int>, Func<int, int>, Func<int, int>, Func<int, int>, Func<int, int>>
        {
            /// <inheritdoc />
            [Theory]
            [ClassData(typeof(ThrowsArgumentNullExceptionTestSource))]
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

            /// <summary>
            /// Test source for <see cref="ThrowsArgumentNullException" />.
            /// </summary>
            public sealed class ThrowsArgumentNullExceptionTestSource
                : TheoryData<IObservable<bool>?, Func<int, int>?, Func<int, int>?, Func<int, int>?, Func<int, int>?, Func<int, int>?, string>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="ThrowsArgumentNullExceptionTestSource"/> class.
                /// </summary>
                public ThrowsArgumentNullExceptionTestSource()
                {
#pragma warning disable CA2000 // Dispose objects before losing scope
                    Add(
                        null,
                        BasicFunc(),
                        BasicFunc(),
                        BasicFunc(),
                        BasicFunc(),
                        BasicFunc(),
                        "canExecuteObservable");

                    Add(
                        BasicObservable(),
                        null,
                        BasicFunc(),
                        BasicFunc(),
                        BasicFunc(),
                        BasicFunc(),
                        "commandFunc1");

                    Add(
                        BasicObservable(),
                        BasicFunc(),
                        null,
                        BasicFunc(),
                        BasicFunc(),
                        BasicFunc(),
                        "commandFunc2");

                    Add(
                        BasicObservable(),
                        BasicFunc(),
                        BasicFunc(),
                        null,
                        BasicFunc(),
                        BasicFunc(),
                        "commandFunc3");

                    Add(
                        BasicObservable(),
                        BasicFunc(),
                        BasicFunc(),
                        BasicFunc(),
                        null,
                        BasicFunc(),
                        "commandFunc4");

                    Add(
                        BasicObservable(),
                        BasicFunc(),
                        BasicFunc(),
                        BasicFunc(),
                        BasicFunc(),
                        null,
                        "commandFunc5");
#pragma warning restore CA2000 // Dispose objects before losing scope
                }
            }
        }

        /// <summary>
        /// Unit Tests for the Factory method for 6 commands that share a common execution predicate without individual execution constraints.
        /// </summary>
        public sealed class GetCommandsWithCommonExecutionPredicate6CommandsMethod : NetTestRegimentation.ITestMethodWithNullableParameters<IObservable<bool>, Func<int, int>, Func<int, int>, Func<int, int>, Func<int, int>, Func<int, int>, Func<int, int>>
        {
            /// <inheritdoc />
            [Theory]
            [ClassData(typeof(ThrowsArgumentNullExceptionTestSource))]
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

            /// <summary>
            /// Test source for <see cref="ThrowsArgumentNullException" />.
            /// </summary>
            public sealed class ThrowsArgumentNullExceptionTestSource
                : TheoryData<IObservable<bool>?, Func<int, int>?, Func<int, int>?, Func<int, int>?, Func<int, int>?, Func<int, int>?, Func<int, int>?, string>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="ThrowsArgumentNullExceptionTestSource"/> class.
                /// </summary>
                public ThrowsArgumentNullExceptionTestSource()
                {
#pragma warning disable CA2000 // Dispose objects before losing scope
                    Add(
                        null,
                        BasicFunc(),
                        BasicFunc(),
                        BasicFunc(),
                        BasicFunc(),
                        BasicFunc(),
                        BasicFunc(),
                        "canExecuteObservable");

                    Add(
                        BasicObservable(),
                        null,
                        BasicFunc(),
                        BasicFunc(),
                        BasicFunc(),
                        BasicFunc(),
                        BasicFunc(),
                        "commandFunc1");

                    Add(
                        BasicObservable(),
                        BasicFunc(),
                        null,
                        BasicFunc(),
                        BasicFunc(),
                        BasicFunc(),
                        BasicFunc(),
                        "commandFunc2");

                    Add(
                        BasicObservable(),
                        BasicFunc(),
                        BasicFunc(),
                        null,
                        BasicFunc(),
                        BasicFunc(),
                        BasicFunc(),
                        "commandFunc3");

                    Add(
                        BasicObservable(),
                        BasicFunc(),
                        BasicFunc(),
                        BasicFunc(),
                        null,
                        BasicFunc(),
                        BasicFunc(),
                        "commandFunc4");

                    Add(
                        BasicObservable(),
                        BasicFunc(),
                        BasicFunc(),
                        BasicFunc(),
                        BasicFunc(),
                        null,
                        BasicFunc(),
                        "commandFunc5");

                    Add(
                        BasicObservable(),
                        BasicFunc(),
                        BasicFunc(),
                        BasicFunc(),
                        BasicFunc(),
                        BasicFunc(),
                        null,
                        "commandFunc6");
#pragma warning restore CA2000 // Dispose objects before losing scope
                }
            }
        }
    }
}
