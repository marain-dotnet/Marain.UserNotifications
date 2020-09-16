// <copyright file="UpdateNotificationStatusRequestItem.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Management.Host.OpenApi
{
    using System;

    /// <summary>
    /// A request to update the "delivered" status of a notification.
    /// </summary>
    public class UpdateNotificationStatusRequestItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateNotificationStatusRequestItem"/> class.
        /// </summary>
        /// <param name="notificationId">The <see cref="NotificationId" />.</param>
        /// <param name="updateTimestamp">The <see cref="UpdateTimestamp" />.</param>
        /// <param name="deliveryChannelId">The <see cref="DeliveryChannelId" />.</param>
        public UpdateNotificationStatusRequestItem(
            string notificationId,
            DateTimeOffset updateTimestamp,
            string deliveryChannelId)
        {
            this.NotificationId = notificationId;
            this.UpdateTimestamp = updateTimestamp;
            this.DeliveryChannelId = deliveryChannelId;
        }

        /// <summary>
        /// Gets the Id of the notification to update.
        /// </summary>
        public string NotificationId { get; }

        /// <summary>
        /// Gets the time at which the status change occurred.
        /// </summary>
        public DateTimeOffset UpdateTimestamp { get; }

        /// <summary>
        /// Gets the Id of the delivery channel whose status is being updated.
        /// </summary>
        public string DeliveryChannelId { get; }
    }
}
