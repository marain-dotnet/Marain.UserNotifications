// <copyright file="IApiClient.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Client
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Base interface for API clients.
    /// </summary>
    public interface IApiClient
    {
        /// <summary>
        /// Gets or sets the base Uri for the service.
        /// </summary>
        Uri BaseUrl { get; set; }

        /// <summary>
        /// Gets data from the API using a link returned from a previous request.
        /// </summary>
        /// <typeparam name="T">The expected type of the response body.</typeparam>
        /// <param name="relativePath">The Url to request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The response.</returns>
        Task<ApiResponse<T>> GetPathAsync<T>(string relativePath, CancellationToken cancellationToken = default);
    }
}