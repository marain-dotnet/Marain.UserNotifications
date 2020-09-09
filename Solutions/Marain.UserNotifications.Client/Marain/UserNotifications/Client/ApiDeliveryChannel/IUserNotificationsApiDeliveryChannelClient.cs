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
    public interface IUserNotificationsApiDeliveryChannelClient : IApiClient
    {
        /// <summary>
        /// Retrieves notifications for a user.
        /// </summary>
        /// <param name="tenantId">The tenant Id for the request.</param>
        /// <param name="userId">The Id of the user to retrieve notifications for.</param>
        /// <param name="sinceNotificationId">If supplied, only more recent notifications than specified will be returned.</param>
        /// <param name="maxItems">The maxiumum number of notifications to return.</param>
        /// <param name="continuationToken">A continuation token from a previous request.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The list of notifications.</returns>
        Task<ApiResponse<PagedNotificationListResource>> GetUserNotificationsAsync(
            string tenantId,
            string userId,
            string sinceNotificationId,
            int? maxItems,
            string continuationToken,
            CancellationToken cancellationToken = default);
    }
}