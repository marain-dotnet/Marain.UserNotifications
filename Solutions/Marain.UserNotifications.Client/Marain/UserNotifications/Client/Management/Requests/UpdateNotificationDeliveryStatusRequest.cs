// <copyright file="UpdateNotificationDeliveryStatusRequest.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Client.Management.Requests
{
    using System;

    /// <summary>
    /// A request to update the "delivered" status of a notification.
    /// </summary>
    public class UpdateNotificationDeliveryStatusRequest
    {
        /// <summary>
        /// Gets or sets the new status for the notification.
        /// </summary>
        public UpdateNotificationDeliveryStatusRequestNewStatus NewStatus { get; set; }

        /// <summary>
        /// Gets or sets the time at which the status change occurred.
        /// </summary>
        public DateTimeOffset UpdateTimestamp { get; set; }

        /// <summary>
        /// Gets or sets the Id of the delivery channel whose status is being updated.
        /// </summary>
        public string DeliveryChannelId { get; set; }
    }
}