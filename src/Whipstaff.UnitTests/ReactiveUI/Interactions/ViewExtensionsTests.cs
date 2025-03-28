// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using NetTestRegimentation;
using NetTestRegimentation.XUnit.Theories.ArgumentNullException;
using ReactiveUI;
using Whipstaff.ReactiveUI.Interactions;
using Xunit;

namespace Whipstaff.UnitTests.ReactiveUI.Interactions
{
    /// <summary>
    /// Unit tests for <see cref="ViewExtensions"/>.
    /// </summary>
    public static class ViewExtensionsTests
    {
        /// <summary>
        /// Unit Tests for <see cref="ViewExtensions.BindInteractionDirectToOutputFunc{TViewModel, TView, TInput, TOutput}(TView, TViewModel, Expression{Func{TViewModel, IInteraction{TInput, TOutput}}}, Func{TInput, Task{TOutput}})"/>.
        /// </summary>
        public sealed class BindInteractionDirectToOutputFuncMethod : ITestMethodWithNullableParameters<FakeView, Expression<Func<FakeViewModel, IInteraction<int, int>>>, Func<int, Task<int>>>
        {
            /// <inheritdoc/>
            [WpfTheory]
            [ClassData(typeof(BindInteractionDirectToOutputFuncMethodTestSource))]
            public void ThrowsArgumentNullException(
                FakeView? arg1,
                Expression<Func<FakeViewModel, IInteraction<int, int>>>? arg2,
                Func<int, Task<int>>? arg3,
                string expectedParameterNameForException)
            {
                if (arg1 != null)
                {
                    arg1.ViewModel = new FakeViewModel();
                }

                _ = Assert.Throws<ArgumentNullException>(
                    expectedParameterNameForException,
                    () => arg1!.BindInteractionDirectToOutputFunc(
                        arg1?.ViewModel,
                        arg2!,
                        arg3!));
            }

            /// <summary>
            /// Test that the method returns a disposable subscription.
            /// </summary>
            [WpfFact]
            public void ReturnsDisposable()
            {
                var viewModel = new FakeViewModel();
                var view = new FakeView
                {
                    ViewModel = viewModel
                };

                Assert.NotNull(view.BindInteractionDirectToOutputFunc(
                    viewModel,
                    model => model.Interaction,
                    input => Task.FromResult(input + 100)));
            }
        }

        /// <summary>
        /// Test data for <see cref="BindInteractionDirectToOutputFuncMethod"/>.
        /// </summary>
        public sealed class BindInteractionDirectToOutputFuncMethodTestSource :
            ArgumentNullExceptionTheoryData<FakeView, Expression<Func<FakeViewModel, IInteraction<int, int>>>, Func<int, Task<int>>>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="BindInteractionDirectToOutputFuncMethodTestSource"/> class.
            /// </summary>
            public BindInteractionDirectToOutputFuncMethodTestSource()
                : base(
                    new NamedParameterInput<FakeView>("view", () => new FakeView()),
                    new NamedParameterInput<Expression<Func<FakeViewModel, IInteraction<int, int>>>>("propertyName", () => vm => vm.Interaction),
                    new NamedParameterInput<Func<int, Task<int>>>("outputFunc", () => input => Task.FromResult(input + 101)))
            {
            }
        }

        /// <summary>
        /// Fake ViewModel for testing.
        /// </summary>
        public sealed class FakeViewModel : ReactiveObject
        {
            /// <summary>
            /// Gets the Interaction.
            /// </summary>
            public IInteraction<int, int> Interaction { get; } = new Interaction<int, int>();
        }

        /// <summary>
        /// Fake View for testing.
        /// </summary>
        public sealed class FakeView : ReactiveWindow<FakeViewModel>;
    }
}
