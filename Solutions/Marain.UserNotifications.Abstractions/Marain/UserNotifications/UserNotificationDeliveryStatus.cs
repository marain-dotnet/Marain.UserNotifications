// <copyright file="UserNotificationDeliveryStatus.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications
{
    /// <summary>
    /// Potential values for <see cref="UserNotificationStatus.DeliveryStatus" />.
    /// </summary>
    public enum UserNotificationDeliveryStatus
    {
        /// <summary>
        /// The status has not been set.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// The delivery channel is not able to track notification delivery statuses.
        /// </summary>
        NotTracked = 1,

        /// <summary>
        /// The notification has not been delivered yet.
        /// </summary>
        Undelivered = 2,

        /// <summary>
        /// The notification has been delivered.
        /// </summary>
        Delivered = 3,
    }
}