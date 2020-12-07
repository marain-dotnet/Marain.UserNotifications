// <copyright file="IUserNotificationsManagementClient.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Client.Management
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Marain.UserNotifications.Client.Management.Requests;
    using Marain.UserNotifications.Client.Management.Resources;

    /// <summary>
    /// Interface for the management client.
    /// </summary>
    public interface IUserNotificationsManagementClient
    {
        /// <summary>Create a new notification for one or more users.</summary>
        /// <param name="tenantId">The tenant within which the request should operate.</param>
        /// <param name="body">The new notifications.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A task representing the operation status.</returns>
        Task<ApiResponse> CreateNotificationsAsync(
            string tenantId,
            CreateNotificationsRequest body,
            CancellationToken cancellationToken = default);

        /// <summary>Updates delivery statuses for a batch of user notifications.</summary>
        /// <param name="tenantId">The tenant within which the request should operate.</param>
        /// <param name="body">The update batch.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A task representing the operation status.</returns>
        Task<ApiResponse> BatchDeliveryStatusUpdateAsync(
            string tenantId,
            IEnumerable<BatchDeliveryStatusUpdateRequestItem> body,
            CancellationToken cancellationToken = default);

        /// <summary>Updates read statuses for a batch of user notifications.</summary>
        /// <param name="tenantId">The tenant within which the request should operate.</param>
        /// <param name="body">The update batch.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A task representing the operation status.</returns>
        Task<ApiResponse> BatchReadStatusUpdateAsync(
            string tenantId,
            IEnumerable<BatchReadStatusUpdateRequestItem> body,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Gets the UserPreference for the provided tenantId and userId.
        /// </summary>
        /// <param name="tenantId">The tenant within which the request should operate.</param>
        /// <param name="userId">The id of the user.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The UserPreference object.</returns>
        Task<ApiResponse<UserPreference>> GetUserPreference(
            string tenantId,
            string userId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Set the UserPrefence object for the provided tenantId and userId.
        /// </summary>
        /// <param name="tenantId">The tenant within which the request should operate.</param>
        /// <param name="body">The user preference object.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>The new user preference object.</returns>
        Task<ApiResponse> SetUserPreference(
            string tenantId,
            UserPreference body,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get the notification template for a certain notification.
        /// </summary>
        /// <param name="tenantId">The tenant within which the request should operate.</param>
        /// <param name="notificationType">The notification type of the stored template.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A Notification template.</returns>
        Task<ApiResponse<NotificationTemplate>> GetNotificationTemplate(
            string tenantId,
            string notificationType,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Set a notification template for a certain notification type.
        /// </summary>
        /// <param name="tenantId">The tenant within which the request should operate.</param>
        /// <param name="notificationTemplate">The template of the notification.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A Notification template.</returns>
        Task<ApiResponse> SetNotificationTemplate(
            string tenantId,
            NotificationTemplate notificationTemplate,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Get the generated notification template for a certain notification.
        /// </summary>
        /// <param name="tenantId">The tenant within which the request should operate.</param>
        /// <param name="createNotificationsRequest">The notification request object that will be applied to different communication templates.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A Notification template.</returns>
        Task<ApiResponse<NotificationTemplate>> GenerateNotificationTemplate(
            string tenantId,
            CreateNotificationsRequest createNotificationsRequest,
            CancellationToken cancellationToken = default);
    }
}
