// <copyright file="BatchReadStatusUpdateRequestItem.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Management.Host.OpenApi
{
    using System;

    /// <summary>
    /// Request data item that forms part of a batch update of delivery statuses.
    /// </summary>
    public class BatchReadStatusUpdateRequestItem : UpdateNotificationStatusRequestItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BatchReadStatusUpdateRequestItem"/> class.
        /// </summary>
        /// <param name="notificationId">The <see cref="UpdateNotificationStatusRequestItem.NotificationId"/>.</param>
        /// <param name="newStatus">The <see cref="NewStatus" />.</param>
        /// <param name="updateTimestamp">The <see cref="UpdateNotificationStatusRequestItem.UpdateTimestamp" />.</param>
        /// <param name="deliveryChannelId">The <see cref="UpdateNotificationStatusRequestItem.DeliveryChannelId" />.</param>
        public BatchReadStatusUpdateRequestItem(
            string notificationId,
            UserNotificationReadStatus newStatus,
            DateTimeOffset updateTimestamp,
            string deliveryChannelId)
            : base(notificationId, updateTimestamp, deliveryChannelId)
        {
            this.NewStatus = newStatus;
        }

        /// <summary>
        /// Gets the new status for the notification.
        /// </summary>
        public UserNotificationReadStatus NewStatus { get; }
    }
}