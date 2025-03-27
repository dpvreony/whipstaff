// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using NetTestRegimentation.XUnit.Theories.ArgumentNullException;
using ReactiveUI;
using Rocks;
using Whipstaff.ReactiveUI.Interactions;
using Xunit;

namespace Whipstaff.UnitTests.ReactiveUI.Interactions
{
    /// <summary>
    /// Unit tests for <see cref="InteractionContextExtensions"/>.
    /// </summary>
    public static class InteractionContextExtensionsTests
    {
        /// <summary>
        /// Unit Tests for <see cref="InteractionContextExtensions.ChainToOutputFuncAsync{TInput, TOutput}(IInteractionContext{TInput,TOutput}, Func{TInput, Task{TOutput}})"/>.
        /// </summary>
        public sealed class ChainToOutputFuncAsyncMethod : NetTestRegimentation.ITestAsyncMethodWithNullableParameters<IInteractionContext<int, int>, Func<int, Task<int>>>
        {
            /// <inheritdoc/>
            [Theory]
            [ClassData(typeof(ThrowsArgumentNullExceptionAsyncTestSource))]
            public async Task ThrowsArgumentNullExceptionAsync(
                IInteractionContext<int, int>? arg1,
                Func<int, Task<int>>? arg2,
                string expectedParameterNameForException)
            {
                _ = await Assert.ThrowsAsync<ArgumentNullException>(expectedParameterNameForException, () => arg1!.ChainToOutputFuncAsync(arg2!));
            }

            /// <summary>
            /// Test that the method chaining takes place and the output is set.
            /// </summary>
            /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
            [Fact]
            public async Task HandlesResultAsync()
            {
                var output = 0;
                var isHandled = false;
                var interactionContextExpectation = new IOutputContextExpectations<int, int>();

                var properties = interactionContextExpectation.Properties;
                var getters = properties.Getters;

                _ = getters.Input().ReturnValue(1);
                _ = getters.IsHandled().Callback(() => isHandled);

                var methods = interactionContextExpectation.Methods;

                _ = methods.SetOutput(Arg.Any<int>()).Callback(input =>
                {
                    output = input;
                    isHandled = true;
                });

                _ = methods.GetOutput().Callback(() => output);

                var interactionContext = interactionContextExpectation.Instance();

                await interactionContext.ChainToOutputFuncAsync(input => Task.FromResult(input + 101));
                Assert.True(interactionContext.IsHandled);
                Assert.Equal(102, interactionContext.GetOutput());
            }

            /// <summary>
            /// Test source for <see cref="ThrowsArgumentNullExceptionAsync" />.
            /// </summary>
            public sealed class ThrowsArgumentNullExceptionAsyncTestSource : NetTestRegimentation.XUnit.Theories.
                ArgumentNullException.ArgumentNullExceptionTheoryData<IInteractionContext<int, int>,
                    Func<int, Task<int>>>
            {
                /// <summary>
                /// Initializes a new instance of the <see cref="ThrowsArgumentNullExceptionAsyncTestSource"/> class.
                /// </summary>
                public ThrowsArgumentNullExceptionAsyncTestSource()
                    : base(
                        new NamedParameterInput<IInteractionContext<int, int>>(
                            "interactionContext",
                            () => new IOutputContextExpectations<int, int>().Instance()),
                        new NamedParameterInput<Func<int, Task<int>>>(
                            "outputFunc",
                            () => input => Task.FromResult(input + 1)))
                {
                }
            }
        }
    }
}
