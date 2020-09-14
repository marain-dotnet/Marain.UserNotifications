// <copyright file="UpdateNotificationDeliveryStatusRequest.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Management.Host.OpenApi
{
    using System;
    using Marain.UserNotifications;

    /// <summary>
    /// A request to update the "delivered" status of a notification.
    /// </summary>
    public class UpdateNotificationDeliveryStatusRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateNotificationDeliveryStatusRequest"/> class.
        /// </summary>
        /// <param name="newStatus">The <see cref="NewStatus" />.</param>
        /// <param name="updateTimestamp">The <see cref="UpdateTimestamp" />.</param>
        /// <param name="deliveryChannelId">The <see cref="DeliveryChannelId" />.</param>
        public UpdateNotificationDeliveryStatusRequest(
            UserNotificationDeliveryStatus newStatus,
            DateTimeOffset updateTimestamp,
            string deliveryChannelId)
        {
            this.NewStatus = newStatus;
            this.UpdateTimestamp = updateTimestamp;
            this.DeliveryChannelId = deliveryChannelId;
        }

        /// <summary>
        /// Gets the new status for the notification.
        /// </summary>
        public UserNotificationDeliveryStatus NewStatus { get; }

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
