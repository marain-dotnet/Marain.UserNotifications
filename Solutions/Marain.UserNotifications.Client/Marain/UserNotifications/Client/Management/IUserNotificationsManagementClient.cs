// <copyright file="IUserNotificationsManagementClient.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Client.Management
{
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Marain.UserNotifications.Client.Management.Requests;

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
            CancellationToken cancellationToken);

        /// <summary>Updates delivery statuses for a batch of user notifications.</summary>
        /// <param name="tenantId">The tenant within which the request should operate.</param>
        /// <param name="body">The update batch.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A task representing the operation status.</returns>
        Task<ApiResponse> BatchDeliveryStatusUpdateAsync(
            string tenantId,
            IEnumerable<BatchDeliveryStatusUpdateRequestItem> body,
            CancellationToken cancellationToken);

        /// <summary>Updates read statuses for a batch of user notifications.</summary>
        /// <param name="tenantId">The tenant within which the request should operate.</param>
        /// <param name="body">The update batch.</param>
        /// <param name="cancellationToken">A cancellation token that can be used by other objects or threads to receive notice of cancellation.</param>
        /// <returns>A task representing the operation status.</returns>
        Task<ApiResponse> BatchReadStatusUpdateAsync(
            string tenantId,
            IEnumerable<BatchReadStatusUpdateRequestItem> body,
            CancellationToken cancellationToken);
    }
}
