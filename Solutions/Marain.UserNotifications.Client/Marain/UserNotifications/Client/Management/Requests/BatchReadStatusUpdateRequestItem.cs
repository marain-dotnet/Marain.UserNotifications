// <copyright file="BatchReadStatusUpdateRequestItem.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Client.Management.Requests
{
    using System;

    /// <summary>
    /// Request data item that forms part of a batch update of delivery statuses.
    /// </summary>
    public class BatchReadStatusUpdateRequestItem
    {
        /// <summary>
        /// Gets or sets the new status for the notification.
        /// </summary>
        public UpdateNotificationReadStatusRequestNewStatus NewStatus { get; set; }

        /// <summary>
        /// Gets or sets the time at which the status change occurred.
        /// </summary>
        public DateTimeOffset UpdateTimestamp { get; set; }

        /// <summary>
        /// Gets or sets the Id of the delivery channel whose status is being updated.
        /// </summary>
        public string DeliveryChannelId { get; set; }

        /// <summary>
        /// Gets or sets the Id of the notification to update.
        /// </summary>
        public string NotificationId { get; set; }
    }
}