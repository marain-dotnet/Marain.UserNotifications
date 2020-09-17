// <copyright file="UserNotificationsApiDeliveryChannelClient.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Client.ApiDeliveryChannel
{
    using System;
    using System.IO;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading;
    using System.Threading.Tasks;
    using Marain.UserNotifications.Client.ApiDeliveryChannel.Resources;

    /// <summary>
    /// Client for the API delivery channel.
    /// </summary>
    public class UserNotificationsApiDeliveryChannelClient : ClientBase, IUserNotificationsApiDeliveryChannelClient
    {
        /// <summary>
        /// Creates a new instance of the <see cref="UserNotificationsApiDeliveryChannelClient"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client to use for requests. Should be initialised with the service base Url.</param>
        public UserNotificationsApiDeliveryChannelClient(HttpClient httpClient)
            : base(httpClient)
        {
        }

        /// <inheritdoc/>
        public async Task<ApiResponse<NotificationResource>> GetNotificationAsync(
            string tenantId,
            string notificationId,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(tenantId))
            {
                throw new ArgumentNullException(nameof(tenantId));
            }

            if (string.IsNullOrEmpty(notificationId))
            {
                throw new ArgumentNullException(nameof(notificationId));
            }

            Uri requestUri = this.ConstructUri($"/{tenantId}/marain/usernotifications/{notificationId}");

            HttpRequestMessage request = this.BuildRequest(HttpMethod.Get, requestUri);

            HttpResponseMessage response = await this.SendRequestAndThrowOnFailure(request, cancellationToken).ConfigureAwait(false);

            using Stream contentStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            NotificationResource result = await JsonSerializer.DeserializeAsync<NotificationResource>(contentStream, this.SerializerOptions).ConfigureAwait(false);

            return new ApiResponse<NotificationResource>(
                response.StatusCode,
                result);
        }

        /// <inheritdoc />
        public async Task<ApiResponse<PagedNotificationListResource>> GetUserNotificationsAsync(
            string tenantId,
            string userId,
            string sinceNotificationId,
            int? maxItems,
            CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(tenantId))
            {
                throw new ArgumentNullException(nameof(tenantId));
            }

            if (string.IsNullOrEmpty(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            Uri requestUri = this.ConstructUri(
                $"/{tenantId}/marain/users/{userId}/notifications",
                ("sinceNotificationId", sinceNotificationId),
                ("maxItems", maxItems?.ToString()));

            HttpRequestMessage request = this.BuildRequest(HttpMethod.Get, requestUri);

            HttpResponseMessage response = await this.SendRequestAndThrowOnFailure(request, cancellationToken).ConfigureAwait(false);

            using Stream contentStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            PagedNotificationListResource result = await JsonSerializer.DeserializeAsync<PagedNotificationListResource>(contentStream, this.SerializerOptions).ConfigureAwait(false);

            return new ApiResponse<PagedNotificationListResource>(
                response.StatusCode,
                result);
        }

        /// <inheritdoc />
        public Task<ApiResponse<PagedNotificationListResource>> GetUserNotificationsByLinkAsync(
            string link,
            CancellationToken cancellationToken = default)
        {
            return this.GetPathAsync<PagedNotificationListResource>(link, cancellationToken);
        }

        /// <inheritdoc />
        public Task<ApiResponse> MarkNotificationAsReadByLinkAsync(string link, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(link))
            {
                throw new ArgumentNullException(nameof(link));
            }

            return this.CallLongRunningOperationEndpointAsync(new Uri(link, UriKind.Relative), HttpMethod.Post);
        }
    }
}
