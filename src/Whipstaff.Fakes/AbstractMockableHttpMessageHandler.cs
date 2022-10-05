// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Whipstaff.Testing
{
    /// <summary>
    /// Abstraction of <see cref="HttpMessageHandler"/> to allow easier mocking.
    /// </summary>
    /// <remarks>
    /// This should move to NetTestRegimentation.
    /// </remarks>
    public abstract class AbstractMockableHttpMessageHandler : HttpMessageHandler
    {
        /// <summary>
        /// Allows mocking the sending of Http Requests.
        /// </summary>
        /// <param name="request">Http Request.</param>
        /// <returns>Http Response.</returns>
        public abstract HttpResponseMessage Send(HttpRequestMessage request);

        /// <inheritdoc/>
        protected sealed override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            return Task.FromResult(Send(request));
        }
    }
}
