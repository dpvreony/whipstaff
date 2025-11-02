// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.Text.Json.Serialization;
using Whipstaff.WebHooksSite.Client.GetRequest;

namespace Whipstaff.WebHooksSite.Client.GetListOfRequests
{
    /// <summary>
    /// Model for the request.
    /// </summary>
    /// <param name="Data">Data of the requests.</param>
    /// <param name="Total">Total number of requests.</param>
    /// <param name="PerPage">Number of records per page.</param>
    /// <param name="CurrentPage">Current page being retrieved.</param>
    /// <param name="IsLastPage">Is the last page of the collection.</param>
    /// <param name="From">Starting sequence number.</param>
    /// <param name="To">Ending sequent number.</param>
    public sealed record RequestListResponseModel(
        [property: JsonPropertyName("data")] IReadOnlyList<RequestResponseModel> Data,
        [property: JsonPropertyName("total")] int Total,
        [property: JsonPropertyName("per_page")] int PerPage,
        [property: JsonPropertyName("current_page")] int CurrentPage,
        [property: JsonPropertyName("is_last_page")] bool IsLastPage,
        [property: JsonPropertyName("from")] int From,
        [property: JsonPropertyName("to")] int To);
}
