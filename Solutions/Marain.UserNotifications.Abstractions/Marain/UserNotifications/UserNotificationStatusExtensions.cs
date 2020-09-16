// <copyright file="UserNotificationStatusExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications
{
    using System;

    /// <summary>
    /// Extension methods for the <see cref="UserNotificationStatus"/> class.
    /// </summary>
    public static class UserNotificationStatusExtensions
    {
        /// <summary>
        /// Creates a new instance of the class with the delivery status to the specified value.
        /// </summary>
        /// <param name="existing">The <see cref="UserNotificationStatus" /> to create a modified copy of.</param>
        /// <param name="newStatus">The new delivery status.</param>
        /// <param name="effectiveDateTime">The time at which the update occurred.</param>
        /// <returns>An updated instance of the <see cref="UserNotificationStatus"/>.</returns>
        public static UserNotificationStatus WithDeliveryStatus(
            this UserNotificationStatus existing,
            UserNotificationDeliveryStatus newStatus,
            DateTimeOffset effectiveDateTime)
        {
            return new UserNotificationStatus(
                existing.DeliveryChannelId,
                newStatus,
                effectiveDateTime.ToUniversalTime(),
                existing.ReadStatus,
                existing.ReadStatusLastUpdated);
        }

        /// <summary>
        /// Creates a new instance of the class with the read status to the specified value.
        /// </summary>
        /// <param name="existing">The <see cref="UserNotificationStatus" /> to create a modified copy of.</param>
        /// <param name="newStatus">The new read status.</param>
        /// <param name="effectiveDateTime">The time at which the update occurred.</param>
        /// <returns>An updated instance of the <see cref="UserNotificationStatus"/>.</returns>
        public static UserNotificationStatus WithReadStatus(
            this UserNotificationStatus existing,
            UserNotificationReadStatus newStatus,
            DateTimeOffset effectiveDateTime)
        {
            return new UserNotificationStatus(
                existing.DeliveryChannelId,
                existing.DeliveryStatus,
                existing.DeliveryStatusLastUpdated,
                newStatus,
                effectiveDateTime.ToUniversalTime());
        }
    }
}
