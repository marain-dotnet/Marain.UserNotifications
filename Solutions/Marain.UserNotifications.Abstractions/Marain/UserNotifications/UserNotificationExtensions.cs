// <copyright file="UserNotificationExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications
{
    using System;
    using System.Collections.Immutable;
    using System.Linq;

    /// <summary>
    /// Extension methods for the <see cref="UserNotification"/> class.
    /// </summary>
    public static class UserNotificationExtensions
    {
        /// <summary>
        /// Gets the current <see cref="UserNotificationDeliveryStatus"/> for the specified delivery channel.
        /// </summary>
        /// <param name="userNotification">The user notification to check.</param>
        /// <param name="deliveryChannelId">The Id of the delivery channel to retrieve the status for.</param>
        /// <returns>
        /// The delivery status for the channel. If not explicitly set for the notification, the value
        /// <see cref="UserNotificationDeliveryStatus.Unknown"/> is returned.
        /// </returns>
        public static UserNotificationDeliveryStatus GetDeliveryStatusForChannel(
            this UserNotification userNotification,
            string deliveryChannelId)
        {
            UserNotificationStatus? status = userNotification.ChannelStatuses.FirstOrDefault(x => x.DeliveryChannelId == deliveryChannelId);

            if (status is null)
            {
                return UserNotificationDeliveryStatus.Unknown;
            }

            return status.DeliveryStatus;
        }

        /// <summary>
        /// Gets the current <see cref="UserNotificationReadStatus"/> for the specified delivery channel.
        /// </summary>
        /// <param name="userNotification">The user notification to check.</param>
        /// <param name="deliveryChannelId">The Id of the delivery channel to retrieve the status for.</param>
        /// <returns>
        /// The delivery status for the channel. If not explicitly set for the notification, the value
        /// <see cref="UserNotificationReadStatus.Unknown"/> is returned.
        /// </returns>
        public static UserNotificationReadStatus GetReadStatusForChannel(
            this UserNotification userNotification,
            string deliveryChannelId)
        {
            UserNotificationStatus? status = userNotification.ChannelStatuses.FirstOrDefault(x => x.DeliveryChannelId == deliveryChannelId);

            if (status is null)
            {
                return UserNotificationReadStatus.Unknown;
            }

            return status.ReadStatus;
        }

        /// <summary>
        /// Creates an updated version of the supplied <see cref="UserNotification"/> with the delivery status for the
        /// specified channel set.
        /// </summary>
        /// <param name="notification">The notification to which the status change applies.</param>
        /// <param name="deliveryChannelId">The delivery channel that the status applies to.</param>
        /// <param name="newDeliveryStatus">The new status.</param>
        /// <param name="effectiveDateTime">The time at which the status change happened.</param>
        /// <returns>A copy of the notification with the updated delivery status.</returns>
        public static UserNotification WithChannelDeliveryStatus(
            this UserNotification notification,
            string deliveryChannelId,
            UserNotificationDeliveryStatus newDeliveryStatus,
            DateTimeOffset effectiveDateTime)
        {
            ImmutableArray<UserNotificationStatus> deliveryStatuses = notification.ChannelStatuses;
            UserNotificationStatus? existingStatusForChannel = deliveryStatuses.FirstOrDefault(s => s.DeliveryChannelId == deliveryChannelId);

            var builder = deliveryStatuses.ToBuilder();

            if (!(existingStatusForChannel is null))
            {
                builder.Remove(existingStatusForChannel);
                builder.Add(existingStatusForChannel.WithDeliveryStatus(newDeliveryStatus, effectiveDateTime.ToUniversalTime()));
            }
            else
            {
                builder.Add(new UserNotificationStatus(
                    deliveryChannelId,
                    newDeliveryStatus,
                    effectiveDateTime,
                    UserNotificationReadStatus.Unknown,
                    effectiveDateTime));
            }

            return new UserNotification(
                notification.Id,
                notification.NotificationType,
                notification.UserId,
                notification.Timestamp,
                notification.Properties,
                notification.Metadata,
                builder.ToImmutable());
        }
    }
}
