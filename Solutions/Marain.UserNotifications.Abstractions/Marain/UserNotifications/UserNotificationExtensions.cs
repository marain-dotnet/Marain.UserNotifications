// <copyright file="UserNotificationExtensions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Linq;
    using Marain.Models;

    /// <summary>
    /// Extension methods for the <see cref="UserNotification"/> class.
    /// </summary>
    public static class UserNotificationExtensions
    {
        /// <summary>
        /// Gets the current <see cref="UserNotificationStatus"/> for the specified delivery channel.
        /// </summary>
        /// <param name="userNotification">The user notification to check.</param>
        /// <param name="deliveryChannelId">The Id of the delivery channel to retrieve the status for.</param>
        /// <returns>
        /// The status for the channel, or null if not set.
        /// </returns>
        public static UserNotificationStatus GetStatusForChannel(
            this UserNotification userNotification,
            string deliveryChannelId)
        {
            if (userNotification is null)
            {
                throw new ArgumentNullException(nameof(userNotification));
            }

            if (string.IsNullOrEmpty(deliveryChannelId))
            {
                throw new ArgumentNullException(nameof(deliveryChannelId));
            }

            return userNotification.ChannelStatuses
                .SingleOrDefault(x => x.DeliveryChannelId == deliveryChannelId);
        }

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
            return userNotification.GetStatusForChannel(deliveryChannelId)?.DeliveryStatus
                ?? UserNotificationDeliveryStatus.Unknown;
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
            return userNotification.GetStatusForChannel(deliveryChannelId)?.ReadStatus
                ?? UserNotificationReadStatus.Unknown;
        }

        /// <summary>
        /// Returns a boolean indicating whether the notification has been marked as read via any channel.
        /// </summary>
        /// <param name="userNotification">The user notification to check.</param>
        /// <returns>
        /// True if there's at least one delivery channel with a <see cref="UserNotificationStatus.ReadStatus"/> of
        /// <see cref="UserNotificationReadStatus.Read"/>, false otherwise.
        /// </returns>
        public static bool HasBeenReadOnAtLeastOneChannel(
            this UserNotification userNotification)
        {
            if (userNotification is null)
            {
                throw new ArgumentNullException(nameof(userNotification));
            }

            return userNotification.ChannelStatuses.Any(s => s.ReadStatus == UserNotificationReadStatus.Read);
        }

        /// <summary>
        /// Creates an updated version of the supplied <see cref="UserNotification"/> with the delivery status for the
        /// specified channel set.
        /// </summary>
        /// <param name="notification">The notification to which the status change applies.</param>
        /// <param name="deliveryChannelId">The delivery channel that the status applies to.</param>
        /// <param name="newDeliveryStatus">The new status.</param>
        /// <param name="effectiveDateTime">The time at which the status change happened.</param>
        /// <param name="failureReason">The failure reason if the delivery channel status is failed.</param>
        /// <returns>A copy of the notification with the updated delivery status.</returns>
        public static UserNotification WithChannelDeliveryStatus(
            this UserNotification notification,
            string deliveryChannelId,
            UserNotificationDeliveryStatus newDeliveryStatus,
            DateTimeOffset effectiveDateTime,
            string? failureReason = null)
        {
            if (notification is null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            if (string.IsNullOrEmpty(deliveryChannelId))
            {
                throw new ArgumentNullException(nameof(deliveryChannelId));
            }

            ImmutableArray<UserNotificationStatus> deliveryStatuses = notification.ChannelStatuses;
            UserNotificationStatus? existingStatusForChannel = deliveryStatuses.FirstOrDefault(s => s.DeliveryChannelId == deliveryChannelId);

            var builder = deliveryStatuses.ToBuilder();

            if (!(existingStatusForChannel is null))
            {
                builder.Remove(existingStatusForChannel);
                builder.Add(existingStatusForChannel.WithDeliveryStatus(newDeliveryStatus, effectiveDateTime, failureReason));
            }
            else
            {
                builder.Add(new UserNotificationStatus(
                    deliveryChannelId,
                    newDeliveryStatus,
                    effectiveDateTime,
                    UserNotificationReadStatus.Unknown,
                    effectiveDateTime,
                    failureReason));
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

        /// <summary>
        /// Creates an updated version of the supplied <see cref="UserNotification"/> with the delivery status for the
        /// specified channel set.
        /// </summary>
        /// <param name="notification">The notification to which the status change applies.</param>
        /// <param name="deliveryChannelId">The delivery channel that the status applies to.</param>
        /// <param name="newReadStatus">The new status.</param>
        /// <param name="effectiveDateTime">The time at which the status change happened.</param>
        /// <returns>A copy of the notification with the updated delivery status.</returns>
        public static UserNotification WithChannelReadStatus(
            this UserNotification notification,
            string deliveryChannelId,
            UserNotificationReadStatus newReadStatus,
            DateTimeOffset effectiveDateTime)
        {
            if (notification is null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            if (string.IsNullOrEmpty(deliveryChannelId))
            {
                throw new ArgumentNullException(nameof(deliveryChannelId));
            }

            ImmutableArray<UserNotificationStatus> deliveryStatuses = notification.ChannelStatuses;
            UserNotificationStatus? existingStatusForChannel = deliveryStatuses.FirstOrDefault(s => s.DeliveryChannelId == deliveryChannelId);

            var builder = deliveryStatuses.ToBuilder();

            if (!(existingStatusForChannel is null))
            {
                builder.Remove(existingStatusForChannel);
                builder.Add(existingStatusForChannel.WithReadStatus(newReadStatus, effectiveDateTime));
            }
            else
            {
                builder.Add(new UserNotificationStatus(
                    deliveryChannelId,
                    UserNotificationDeliveryStatus.Unknown,
                    effectiveDateTime,
                    newReadStatus,
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

        /// <summary>
        /// Creates an updated version of the supplied UserNotification with Delivery Channel Configuration Per Communication Type.
        /// </summary>
        /// <param name="notification">The notification to which the status change applies.</param>
        /// <param name="deliveryChannelConfiguredPerCommunicationType">The delivery channel configuration which need to be applied.</param>
        /// <returns>A copy of the notification with the updated delivery status.</returns>
        public static UserNotification AddDeliveryChannelConfiguredPerCommunicationType(
            this UserNotification notification,
            Dictionary<CommunicationType, string>? deliveryChannelConfiguredPerCommunicationType)
        {
            if (notification is null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            if (deliveryChannelConfiguredPerCommunicationType is null)
            {
                throw new ArgumentNullException(nameof(deliveryChannelConfiguredPerCommunicationType));
            }

            return new UserNotification(
                notification.Id,
                notification.NotificationType,
                notification.UserId,
                notification.Timestamp,
                notification.Properties,
                notification.Metadata,
                notification.ChannelStatuses,
                deliveryChannelConfiguredPerCommunicationType);
        }
    }
}
