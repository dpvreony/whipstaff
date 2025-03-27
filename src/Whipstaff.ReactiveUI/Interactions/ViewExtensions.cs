// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using ReactiveUI;

namespace Whipstaff.ReactiveUI.Interactions
{
    /// <summary>
    /// Interaction extensions for <see cref="IViewFor"/>.
    /// </summary>
    public static class ViewExtensions
    {
        /// <summary>
        /// Binds the <see cref="IInteraction{TInput,TOutput}" /> on a ViewModel to the specified handler.
        /// </summary>
        /// <param name="view">The view to bind to.</param>
        /// <param name="viewModel">The view model to bind to.</param>
        /// <param name="propertyName">The name of the property on the View Model.</param>
        /// <param name="outputFunc">Function that produces the output to bind.</param>
        /// <typeparam name="TViewModel">The type of the view model.</typeparam>
        /// <typeparam name="TView">The type of the view being bound.</typeparam>
        /// <typeparam name="TInput">The interaction's input type.</typeparam>
        /// <typeparam name="TOutput">The interaction's output type.</typeparam>
        /// <returns>An object that when disposed, disconnects the binding.</returns>
        public static IDisposable BindInteractionDirectToOutputFunc<TViewModel, TView, TInput, TOutput>(
            this TView view,
            TViewModel? viewModel,
            Expression<Func<TViewModel, IInteraction<TInput, TOutput>>> propertyName,
            Func<TInput, Task<TOutput>> outputFunc)
            where TViewModel : class
            where TView : class, IViewFor
        {
            return view.BindInteraction(
                viewModel,
                propertyName,
                context => context.ChainToOutputFuncAsync(outputFunc));
        }
    }
}
