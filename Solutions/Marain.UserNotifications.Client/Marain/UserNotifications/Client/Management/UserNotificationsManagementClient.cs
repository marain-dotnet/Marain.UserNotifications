// <copyright file="UserNotificationsManagementClient.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Client.Management
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using Marain.UserNotifications.Client.ApiDeliveryChannel.Resources;
    using Marain.UserNotifications.Client.Management.Requests;
    using Marain.UserNotifications.Client.Management.Resources;

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

        /// <inheritdoc />
        public async Task<ApiResponse<NotificationTemplate>> GetNotificationTemplate(string tenantId, string notificationType, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(tenantId))
            {
                throw new ArgumentNullException(nameof(tenantId));
            }

            if (string.IsNullOrEmpty(notificationType))
            {
                throw new ArgumentNullException(nameof(notificationType));
            }

            Uri requestUri = this.ConstructUri($"/{tenantId}/marain/usernotifications/templates?notificationType={notificationType}");

            HttpRequestMessage request = this.BuildRequest(HttpMethod.Get, requestUri);

            HttpResponseMessage response = await this.SendRequestAndThrowOnFailure(request, cancellationToken).ConfigureAwait(false);

            using Stream contentStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            NotificationTemplate result = await JsonSerializer.DeserializeAsync<NotificationTemplate>(contentStream, this.SerializerOptions).ConfigureAwait(false);

            return new ApiResponse<NotificationTemplate>(
                response.StatusCode,
                result);
        }

        /// <inheritdoc />
        public async Task<ApiResponse> SetNotificationTemplate(string tenantId, NotificationTemplate notificationTemplate, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(tenantId))
            {
                throw new ArgumentNullException(nameof(tenantId));
            }

            if (notificationTemplate == null)
            {
                throw new ArgumentNullException(nameof(notificationTemplate));
            }

            Uri requestUri = this.ConstructUri($"/{tenantId}/marain/usernotifications/templates");

            HttpRequestMessage request = this.BuildRequest(HttpMethod.Put, requestUri, notificationTemplate);

            HttpResponseMessage response = await this.SendRequestAndThrowOnFailure(request, cancellationToken).ConfigureAwait(false);

            return new ApiResponse(response.StatusCode);
        }

        /// <inheritdoc />
        public async Task<ApiResponse<UserPreference>> GetUserPreference(string tenantId, string userId, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(tenantId))
            {
                throw new ArgumentNullException(nameof(tenantId));
            }

            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            Uri requestUri = this.ConstructUri($"{tenantId}/marain/usernotifications/userpreference?userId={userId}");

            HttpRequestMessage request = this.BuildRequest(HttpMethod.Get, requestUri);

            HttpResponseMessage response = await this.SendRequestAndThrowOnFailure(request, cancellationToken).ConfigureAwait(false);

            using Stream contentStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            UserPreference result = await JsonSerializer.DeserializeAsync<UserPreference>(contentStream, this.SerializerOptions).ConfigureAwait(false);

            return new ApiResponse<UserPreference>(
                response.StatusCode,
                result);
        }

        /// <inheritdoc />
        public async Task<ApiResponse> SetUserPreference(string tenantId, UserPreference userPreference, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(tenantId))
            {
                throw new ArgumentNullException(nameof(tenantId));
            }

            if (userPreference == null)
            {
                throw new ArgumentNullException(nameof(userPreference));
            }

            Uri requestUri = this.ConstructUri($"{tenantId}/marain/usernotifications/userpreference");

            HttpRequestMessage request = this.BuildRequest(HttpMethod.Put, requestUri, userPreference);

            HttpResponseMessage response = await this.SendRequestAndThrowOnFailure(request, cancellationToken).ConfigureAwait(false);

            return new ApiResponse(response.StatusCode);
        }

        /// <inheritdoc />
        public async Task<ApiResponse<NotificationTemplate>> GenerateNotificationTemplate(string tenantId, CreateNotificationsRequest createNotificationsRequest, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(tenantId))
            {
                throw new ArgumentNullException(nameof(tenantId));
            }

            if (createNotificationsRequest == null)
            {
                throw new ArgumentNullException(nameof(createNotificationsRequest));
            }

            Uri requestUri = this.ConstructUri($"{tenantId}/marain/usernotifications/templates/generate");

            HttpRequestMessage request = this.BuildRequest(HttpMethod.Put, requestUri, createNotificationsRequest);

            HttpResponseMessage response = await this.SendRequestAndThrowOnFailure(request, cancellationToken).ConfigureAwait(false);

            using Stream contentStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            NotificationTemplate result = await JsonSerializer.DeserializeAsync<NotificationTemplate>(contentStream, this.SerializerOptions).ConfigureAwait(false);

            return new ApiResponse<NotificationTemplate>(
                response.StatusCode,
                result);
        }
    }
}
