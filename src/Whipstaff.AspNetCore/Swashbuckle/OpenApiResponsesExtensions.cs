// Copyright (c) 2022 DHGMS Solutions and Contributors. All rights reserved.
// This file is licensed to you under the MIT license.
// See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using Microsoft.OpenApi;

namespace Whipstaff.AspNetCore.Swashbuckle
{
    /// <summary>
    /// Extension methods for <see cref="OpenApiResponses"/>.
    /// </summary>
    public static class OpenApiResponsesExtensions
    {
        /// <summary>
        /// Adds <see cref="OpenApiSchema"/> responses for HTTP error codes.
        /// </summary>
        /// <param name="responses">The response model to popualate.</param>
        /// <param name="supportedResponseTypes">The supported response types for the api.</param>
        /// <param name="problemDetailsReferenceSchema">Schema representation for the problem details type.</param>
        public static void AssignReferenceSchemaToHttpStatusErrorCodes(
            this OpenApiResponses responses,
            IList<Microsoft.AspNetCore.Mvc.ApiExplorer.ApiResponseFormat> supportedResponseTypes,
            IOpenApiSchema problemDetailsReferenceSchema)
        {
            ArgumentNullException.ThrowIfNull(responses);

            var statusCodes = GetErrorHttpStatusCodes();

            AssignReferenceSchemaToHttpStatusCodes(
                responses,
                supportedResponseTypes,
                problemDetailsReferenceSchema,
                statusCodes);
        }

        private static void AssignReferenceSchemaToHttpStatusCodes(
            OpenApiResponses responses,
            IList<Microsoft.AspNetCore.Mvc.ApiExplorer.ApiResponseFormat> supportedResponseTypes,
            IOpenApiSchema problemDetailsReferenceSchema,
            HttpStatusCode[] httpStatusCodes)
        {
            var contentValue = new OpenApiMediaType
            {
                Schema = problemDetailsReferenceSchema
            };

            var content = supportedResponseTypes.ToDictionary(
                x => x.MediaType,
                _ => contentValue);

            foreach (var httpStatusCode in httpStatusCodes)
            {
                var key = ((int)httpStatusCode).ToString(NumberFormatInfo.InvariantInfo);
                if (responses.ContainsKey(key))
                {
                    continue;
                }

                var response = new OpenApiResponse
                {
                    Content = content,
                    Description = httpStatusCode.ToString()
                };

                responses.Add(key, response);
            }
        }

        private static HttpStatusCode[] GetErrorHttpStatusCodes()
        {
            return new[]
            {
                HttpStatusCode.BadRequest,
                HttpStatusCode.Unauthorized,
                HttpStatusCode.PaymentRequired,
                HttpStatusCode.Forbidden,
                HttpStatusCode.NotFound,
                HttpStatusCode.MethodNotAllowed,
                HttpStatusCode.NotAcceptable,
                HttpStatusCode.ProxyAuthenticationRequired,
                HttpStatusCode.RequestTimeout,
                HttpStatusCode.Conflict,
                HttpStatusCode.Gone,
                HttpStatusCode.LengthRequired,
                HttpStatusCode.PreconditionFailed,
                HttpStatusCode.RequestEntityTooLarge,
                HttpStatusCode.RequestUriTooLong,
                HttpStatusCode.UnsupportedMediaType,
                HttpStatusCode.RequestedRangeNotSatisfiable,
                HttpStatusCode.ExpectationFailed,
                HttpStatusCode.MisdirectedRequest,
                HttpStatusCode.UnprocessableEntity,
                HttpStatusCode.Locked,
                HttpStatusCode.FailedDependency,
                HttpStatusCode.UpgradeRequired,
                HttpStatusCode.PreconditionRequired,
                HttpStatusCode.TooManyRequests,
                HttpStatusCode.RequestHeaderFieldsTooLarge,
                HttpStatusCode.UnavailableForLegalReasons,
                HttpStatusCode.InternalServerError,
                HttpStatusCode.NotImplemented,
                HttpStatusCode.BadGateway,
                HttpStatusCode.ServiceUnavailable,
                HttpStatusCode.GatewayTimeout,
                HttpStatusCode.HttpVersionNotSupported,
                HttpStatusCode.VariantAlsoNegotiates,
                HttpStatusCode.InsufficientStorage,
                HttpStatusCode.LoopDetected,
                HttpStatusCode.NotExtended,
                HttpStatusCode.NetworkAuthenticationRequired
            };
        }
    }
}
