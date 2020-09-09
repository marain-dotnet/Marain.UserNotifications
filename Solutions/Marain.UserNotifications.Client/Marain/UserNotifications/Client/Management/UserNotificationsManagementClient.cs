// <copyright file="UserNotificationsManagementClient.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Client.Management
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using Marain.UserNotifications.Client.Management.Requests;

    /// <summary>
    /// Client for the user notifications management service.
    /// </summary>
    public class UserNotificationsManagementClient : ClientBase, IUserNotificationsManagementClient
    {
        /// <summary>
        /// Creates a new instance of the <see cref="UserNotificationsManagementClient"/> class.
        /// </summary>
        /// <param name="baseUrl">The base Url for the service.</param>
        /// <param name="serializerOptions">The Json serializer options.</param>
        public UserNotificationsManagementClient(string baseUrl, JsonSerializerOptions serializerOptions)
            : base(baseUrl, serializerOptions)
        {
        }

        /// <summary>
        /// Sends a batch of requests to update notification statuses.
        /// </summary>
        /// <param name="tenantId">The requesting tenant Id.</param>
        /// <param name="body">The request body.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="ApiResponse"/>.</returns>
        public async Task<ApiResponse> BatchDeliveryStatusUpdateAsync(string tenantId, IEnumerable<BatchDeliveryStatusUpdateRequestItem> body, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(tenantId))
            {
                throw new ArgumentNullException(nameof(tenantId));
            }

            if (body is null)
            {
                throw new ArgumentNullException(nameof(body));
            }

            var requestUri = new Uri(this.BaseUrl, $"/{tenantId}/marain/usernotifications/batchdeliverystatusupdate");

            HttpRequestMessage request = this.BuildRequest(HttpMethod.Post, requestUri, body);

            HttpResponseMessage response = await this.HttpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            var headers = new Dictionary<string, string>
            {
                { "Location", response.Headers.Location.ToString() },
            };

            return new ApiResponse(response.StatusCode, headers);
        }

        /// <summary>
        /// Creates a new notification for one or more users.
        /// </summary>
        /// <param name="tenantId">The requesting tenant Id.</param>
        /// <param name="body">The request body.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A <see cref="ApiResponse"/>.</returns>
        public async Task<ApiResponse> CreateNotificationsAsync(string tenantId, CreateNotificationsRequest body, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(tenantId))
            {
                throw new ArgumentNullException(nameof(tenantId));
            }

            if (body is null)
            {
                throw new ArgumentNullException(nameof(body));
            }

            var requestUri = new Uri(this.BaseUrl, $"/{tenantId}/marain/usernotifications");

            HttpRequestMessage request = this.BuildRequest(HttpMethod.Put, requestUri, body);

            HttpResponseMessage response = await this.HttpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            var headers = new Dictionary<string, string>
            {
                { "Location", response.Headers.Location.ToString() },
            };

            return new ApiResponse(response.StatusCode, headers);
        }
    }
}
