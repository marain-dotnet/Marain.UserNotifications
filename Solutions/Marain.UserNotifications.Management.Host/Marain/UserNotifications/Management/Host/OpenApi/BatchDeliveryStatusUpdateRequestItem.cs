// <copyright file="BatchDeliveryStatusUpdateRequestItem.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Management.Host.OpenApi
{
    using System;

    /// <summary>
    /// Request data item that forms part of a batch update of delivery statuses.
    /// </summary>
    public class BatchDeliveryStatusUpdateRequestItem : UpdateNotificationDeliveryStatusRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BatchDeliveryStatusUpdateRequestItem"/> class.
        /// </summary>
        /// <param name="notificationId">The <see cref="NotificationId"/>.</param>
        /// <param name="newStatus">The <see cref="UpdateNotificationDeliveryStatusRequest.NewStatus" />.</param>
        /// <param name="updateTimestamp">The <see cref="UpdateNotificationDeliveryStatusRequest.UpdateTimestamp" />.</param>
        /// <param name="deliveryChannelId">The <see cref="UpdateNotificationDeliveryStatusRequest.DeliveryChannelId" />.</param>
        public BatchDeliveryStatusUpdateRequestItem(
            string notificationId,
            UserNotificationDeliveryStatus newStatus,
            DateTimeOffset updateTimestamp,
            string deliveryChannelId)
            : base(newStatus, updateTimestamp, deliveryChannelId)
        {
            this.NotificationId = notificationId;
        }

        /// <summary>
        /// Gets the Id of the notification to update.
        /// </summary>
        public string NotificationId { get; }
    }
}
