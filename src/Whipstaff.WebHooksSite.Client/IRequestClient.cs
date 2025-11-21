// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Threading.Tasks;
using Refit;
using Whipstaff.WebHooksSite.Client.GetListOfRequests;
using Whipstaff.WebHooksSite.Client.GetRequest;

namespace Whipstaff.WebHooksSite.Client
{
    /// <summary>
    /// Represents webhook.site request API calls.
    /// </summary>
    public interface IRequestClient
    {
        /// <summary>
        /// Deletes all requests for the session.
        /// </summary>
        /// <param name="tokenId">Unique id for the session.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Get("/token/{tokenId}/request")]
        Task DeleteAllRequestsAsync([Query(Format = "D")]Guid tokenId);

        /// <summary>
        /// Deletes a specific request for the session.
        /// </summary>
        /// <param name="tokenId">Unique id for the session.</param>
        /// <param name="requestId">Unique id for the request.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        [Get("/token/{tokenId}/request/{requestId}")]
        Task DeleteRequestAsync(
            [Query(Format = "D")] Guid tokenId,
            [Query(Format = "D")] Guid requestId);

        /// <summary>
        /// Gets a list of requests for the session.
        /// </summary>
        /// <param name="tokenId">Unique id for the session.</param>
        /// <returns>A list of requests for the session.</returns>
        [Get("/token/{token_id}/requests")]
        Task<RequestListResponseModel> GetListOfRequestsAsync([Query(Format = "D")] Guid tokenId);

        /// <summary>
        /// Gets a list of requests for the session.
        /// </summary>
        /// <param name="tokenId">Unique id for the session.</param>
        /// <param name="sorting">Sorting order for the requests.</param>
        /// <param name="perPage">Number of records per page.</param>
        /// <param name="dateFrom">Time stamp for the earliest request to return.</param>
        /// <param name="dateTo">Time stamp for the latest request to return.</param>
        /// <param name="query">Advanced query string filter.</param>
        /// <returns>A list of requests for the session.</returns>
        [Get("/token/{token_id}/requests")]
        Task<RequestListResponseModel> GetListOfRequestsAsync(
            [Query(Format = "D")] Guid tokenId,
            Sorting sorting,
            [AliasAs("per_page")] int perPage,
            [AliasAs("date_from")] DateTimeOffset dateFrom,
            [AliasAs("date_to")] DateTimeOffset dateTo,
            string query);

        /// <summary>
        /// Gets the latest request for the session.
        /// </summary>
        /// <param name="tokenId">Unique id for the session.</param>
        /// <returns>The latest request for the session.</returns>
        [Get("/token/{tokenId}/request/latest")]
        Task<RequestResponseModel> GetLatestRequestAsync([Query(Format = "D")] Guid tokenId);

        /// <summary>
        /// Gets a specific request for the session.
        /// </summary>
        /// <param name="tokenId">Unique id for the session.</param>
        /// <param name="requestId">Unique id for the request.</param>
        /// <returns>Specific request for the session.</returns>
        [Get("/token/{tokenId}/request/{requestId}")]
        Task<RequestResponseModel> GetRequestAsync(
            [Query(Format = "D")] Guid tokenId,
            [Query(Format = "D")] Guid requestId);
    }
}
