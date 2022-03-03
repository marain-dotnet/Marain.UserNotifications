// <copyright file="ApiResponse{T}.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Client
{
    using System.Collections.Immutable;
    using System.Net;

    /// <summary>
    /// A response with body from an API endpoint.
    /// </summary>
    /// <typeparam name="T">The type of the response body.</typeparam>
    public readonly struct ApiResponse<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApiResponse{T}"/> class.
        /// </summary>
        /// <param name="statusCode">The <see cref="ApiResponse.StatusCode"/>.</param>
        /// <param name="body">The <see cref="Body"/>.</param>
        /// <param name="headers">The <see cref="ApiResponse.Headers"/>.</param>
        public ApiResponse(HttpStatusCode statusCode, in T body, ImmutableDictionary<string, string> headers = null)
        {
            this.StatusCode = statusCode;
            this.Headers = headers ?? ImmutableDictionary<string, string>.Empty;
            this.Body = body;
        }

        /// <summary>
        /// Gets the headers returned with the response.
        /// </summary>
        public ImmutableDictionary<string, string> Headers { get; }

        /// <summary>
        /// Gets the status code of the response.
        /// </summary>
        public HttpStatusCode StatusCode { get; }

        /// <summary>
        /// Gets the deserialized response body.
        /// </summary>
        public T Body { get; }
    }
}