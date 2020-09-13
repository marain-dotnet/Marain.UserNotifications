// <copyright file="UpdateNotificationDeliveryStatusRequestNewStatus.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Client.Management.Requests
{
    /// <summary>
    /// Possible values for the new delivery status of a notificatino.
    /// </summary>
    public enum UpdateNotificationDeliveryStatusRequestNewStatus
    {
        /// <summary>
        /// Delivery status is not tracked by the channel.
        /// </summary>
        NotTracked = 0,

        /// <summary>
        /// The notification has not been delivered.
        /// </summary>
        Undelivered = 1,

        /// <summary>
        /// The notification has been delivered.
        /// </summary>
        Delivered = 2,
    }
}
