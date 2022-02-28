// <copyright file="NotificationStatusDeliveryStatus.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Client.Management.Resources
{
    /// <summary>
    /// The delivery status of a notification.
    /// </summary>
    public enum NotificationStatusDeliveryStatus
    {
        /// <summary>
        /// The delivery channel has not updated the delivery status.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The delivery channel is not able to track the delivery status.
        /// </summary>
        NotTracked = 1,

        /// <summary>
        /// The notification is undelivered.
        /// </summary>
        Undelivered = 2,

        /// <summary>
        /// The notification is delivered.
        /// </summary>
        Delivered = 3,
    }
}