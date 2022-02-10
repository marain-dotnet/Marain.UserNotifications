// <copyright file="BatchDeliveryStatusUpdateRequestItem.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Management.Host.OpenApi
{
    using System;

    /// <summary>
    /// Request data item that forms part of a batch update of delivery statuses.
    /// </summary>
    public class BatchDeliveryStatusUpdateRequestItem : UpdateNotificationStatusRequestItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BatchDeliveryStatusUpdateRequestItem"/> class.
        /// </summary>
        /// <param name="notificationId">The <see cref="UpdateNotificationStatusRequestItem.NotificationId"/>.</param>
        /// <param name="newStatus">The <see cref="NewStatus" />.</param>
        /// <param name="updateTimestamp">The <see cref="UpdateNotificationStatusRequestItem.UpdateTimestamp" />.</param>
        /// <param name="deliveryChannelId">The <see cref="UpdateNotificationStatusRequestItem.DeliveryChannelId" />.</param>
        public BatchDeliveryStatusUpdateRequestItem(
            string notificationId,
            UserNotificationDeliveryStatus newStatus,
            DateTimeOffset updateTimestamp,
            string deliveryChannelId)
            : base(notificationId, updateTimestamp, deliveryChannelId)
        {
            this.NewStatus = newStatus;
        }

        /// <summary>
        /// Gets the new status for the notification.
        /// </summary>
        public UserNotificationDeliveryStatus NewStatus { get; }
    }
}