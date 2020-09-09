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
        /// <param name="baseUrl">The base Url for the service.</param>
        /// <param name="serializerOptions">The Json serializer options.</param>
        public UserNotificationsApiDeliveryChannelClient(string baseUrl, JsonSerializerOptions serializerOptions)
            : base(baseUrl, serializerOptions)
        {
        }

        /// <inheritdoc />
        public async Task<ApiResponse<PagedNotificationListResource>> GetUserNotificationsAsync(
            string tenantId,
            string userId,
            string sinceNotificationId,
            int? maxItems,
            string continuationToken,
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
                ("maxItems", maxItems?.ToString()),
                ("continuationToken", continuationToken));

            HttpRequestMessage request = this.BuildRequest(HttpMethod.Get, requestUri);

            HttpResponseMessage response = await this.HttpClient.SendAsync(request, cancellationToken).ConfigureAwait(false);

            response.EnsureSuccessStatusCode();

            using Stream contentStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            PagedNotificationListResource result = await JsonSerializer.DeserializeAsync<PagedNotificationListResource>(contentStream, this.SerializerOptions).ConfigureAwait(false);

            return new ApiResponse<PagedNotificationListResource>(
                response.StatusCode,
                result);
        }
    }
}
