// <copyright file="ApiResponse.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Client
{
    using System.Collections.Immutable;
    using System.Net;

    /// <summary>
    /// A response from a request to an API endpoint.
    /// </summary>
    public readonly struct ApiResponse
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResponse"/> class.
        /// </summary>
        /// <param name="statusCode">The <see cref="StatusCode"/>.</param>
        /// <param name="headers">The <see cref="Headers"/>.</param>
        public ApiResponse(HttpStatusCode statusCode, ImmutableDictionary<string, string> headers = null)
        {
            this.StatusCode = statusCode;
            this.Headers = headers ?? ImmutableDictionary<string, string>.Empty;
        }

        /// <summary>
        /// Gets the headers returned with the response.
        /// </summary>
        public ImmutableDictionary<string, string> Headers { get; }

        /// <summary>
        /// Gets the status code of the response.
        /// </summary>
        public HttpStatusCode StatusCode { get; }
    }
}
