// <copyright file="UserNotificationsManagementClient.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Client.Management
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Net.Http;
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
        /// <param name="httpClient">The HTTP client to use for requests. Should be initialised with the service base Url.</param>
        public UserNotificationsManagementClient(HttpClient httpClient)
            : base(httpClient)
        {
        }

        /// <inheritdoc />
        public Task<ApiResponse> BatchDeliveryStatusUpdateAsync(
            string tenantId,
            IEnumerable<BatchDeliveryStatusUpdateRequestItem> body,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(tenantId))
            {
                throw new ArgumentNullException(nameof(tenantId));
            }

            var requestUri = new Uri($"/{tenantId}/marain/usernotifications/batchdeliverystatusupdate", UriKind.Relative);

            return this.CallLongRunningOperationEndpointAsync(requestUri, HttpMethod.Post, body, cancellationToken);
        }

        /// <inheritdoc />
        public Task<ApiResponse> BatchReadStatusUpdateAsync(
            string tenantId,
            IEnumerable<BatchReadStatusUpdateRequestItem> body,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(tenantId))
            {
                throw new ArgumentNullException(nameof(tenantId));
            }

            var requestUri = new Uri($"/{tenantId}/marain/usernotifications/batchreadstatusupdate", UriKind.Relative);

            return this.CallLongRunningOperationEndpointAsync(requestUri, HttpMethod.Post, body, cancellationToken);
        }

        /// <inheritdoc />
        public Task<ApiResponse> CreateNotificationsAsync(
            string tenantId,
            CreateNotificationsRequest body,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(tenantId))
            {
                throw new ArgumentNullException(nameof(tenantId));
            }

            var requestUri = new Uri($"/{tenantId}/marain/usernotifications", UriKind.Relative);

            return this.CallLongRunningOperationEndpointAsync(requestUri, HttpMethod.Put, body, cancellationToken);
        }
    }
}
