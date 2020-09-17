// <copyright file="IUserNotificationsApiDeliveryChannelClient.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Client.ApiDeliveryChannel
{
    using System.Threading;
    using System.Threading.Tasks;
    using Marain.UserNotifications.Client.ApiDeliveryChannel.Resources;

    /// <summary>
    /// Interface for the client for the API delivery channel.
    /// </summary>
    public interface IUserNotificationsApiDeliveryChannelClient
    {
        /// <summary>
        /// Retrieves notifications for a user.
        /// </summary>
        /// <param name="tenantId">The tenant Id for the request.</param>
        /// <param name="userId">The Id of the user to retrieve notifications for.</param>
        /// <param name="sinceNotificationId">If supplied, only more recent notifications than specified will be returned.</param>
        /// <param name="maxItems">The maxiumum number of notifications to return.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The list of notifications.</returns>
        Task<ApiResponse<PagedNotificationListResource>> GetUserNotificationsAsync(
            string tenantId,
            string userId,
            string sinceNotificationId,
            int? maxItems,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves notifications for a user from a link that was returned with a response from
        /// <see cref="GetUserNotificationsAsync(string, string, string, int?, CancellationToken)"/>.
        /// </summary>
        /// <param name="link">The link to use to retrieve notifications.</param>
        /// <returns>The list of notifications.</returns>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <remarks>
        /// It's expected that clients will retrieve this link from either the "next" or "newer" link relations.
        /// </remarks>
        Task<ApiResponse<PagedNotificationListResource>> GetUserNotificationsByLinkAsync(
            string link,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Retrieves a single notification.
        /// </summary>
        /// <param name="tenantId">The tenant Id for the request.</param>
        /// <param name="notificationId">The Id of the requested notification.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The requested notification.</returns>
        Task<ApiResponse<NotificationResource>> GetNotificationAsync(
            string tenantId,
            string notificationId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Marks a notification as read using a link obtained from a <see cref="NotificationResource" /> using
        /// the "mark-read" link relation.
        /// </summary>
        /// <param name="link">The link to use.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The response details.</returns>
        Task<ApiResponse> MarkNotificationAsReadByLinkAsync(
            string link,
            CancellationToken cancellationToken = default);
    }
}