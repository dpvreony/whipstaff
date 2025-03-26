// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using ReactiveUI;

#if ARGUMENT_NULL_EXCEPTION_SHIM
using ArgumentNullException = Whipstaff.Runtime.Exceptions.ArgumentNullException;
#endif

namespace Whipstaff.ReactiveUI.Interactions
{
    /// <summary>
    /// Helper methods for <see cref="IInteractionContext{TInput,TOutput}"/>.
    /// </summary>
    public static class InteractionContextExtensions
    {
        /// <summary>
        /// Chains an interaction context to trigger a function and map the result to the interaction output.
        /// </summary>
        /// <typeparam name="TInput">The type for the input passed into interaction.</typeparam>
        /// <typeparam name="TOutput">The output returned from the interaction.</typeparam>
        /// <param name="interactionContext">The interaction that takes an input and passes an output.</param>
        /// <param name="outputFunc">Function to apply to the output from the interaction.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task ChainToOutputFuncAsync<TInput, TOutput>(
            this IInteractionContext<TInput, TOutput> interactionContext,
            Func<TInput, Task<TOutput>> outputFunc)
        {
            ArgumentNullException.ThrowIfNull(interactionContext);
            ArgumentNullException.ThrowIfNull(outputFunc);

            var output = await outputFunc(interactionContext.Input);

            interactionContext.SetOutput(output);
        }
    }
}
