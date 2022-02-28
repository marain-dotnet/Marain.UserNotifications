// <copyright file="IUserNotificationStore.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for a service that can store and retrieve notifications.
    /// </summary>
    public interface IUserNotificationStore
    {
        /// <summary>
        /// Stores the given notification.
        /// </summary>
        /// <param name="notification">The notification to store.</param>
        /// <returns>The stored notification.</returns>
        Task<UserNotification> StoreAsync(UserNotification notification);

        /// <summary>
        /// Retrieves notifications for the specified user.
        /// </summary>
        /// <param name="userId">The user to retrieve notifications for.</param>
        /// <param name="sinceUserNotificationId">If supplied, only user notifications newer than that with the given
        /// Id will be returned.</param>
        /// <param name="maxItems">The maximum number of items to return.</param>
        /// <returns>The user notifications.</returns>
        Task<GetNotificationsResult> GetAsync(string userId, string? sinceUserNotificationId, int maxItems);

        /// <summary>
        /// Retrieves notifications for the specified user.
        /// </summary>
        /// <param name="userId">The Id of the user making the request.</param>
        /// <param name="continuationToken">A continuation token from a previous call to <see cref="GetAsync(string, string?, int)"/>.</param>
        /// <returns>The user notifications.</returns>
        Task<GetNotificationsResult> GetAsync(string userId, string continuationToken);

        /// <summary>
        /// Retrieves the user notification with the given Id.
        /// </summary>
        /// <param name="id">The Id of the notification to retrieve.</param>
        /// <returns>The requested user notification.</returns>
        Task<UserNotification> GetByIdAsync(string id);
    }
}