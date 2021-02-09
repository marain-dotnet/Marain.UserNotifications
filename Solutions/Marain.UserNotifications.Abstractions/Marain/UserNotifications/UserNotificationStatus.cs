// <copyright file="UserNotificationStatus.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications
{
    using System;
    using System.ComponentModel;
    using Newtonsoft.Json;

    /// <summary>
    /// Represents the delivery/read status of the user notification for a specific delivery channel.
    /// </summary>
    public class UserNotificationStatus
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserNotificationStatus"/> class.
        /// </summary>
        /// <param name="deliveryChannelId">The <see cref="DeliveryChannelId" />.</param>
        public UserNotificationStatus(string deliveryChannelId)
        {
            this.DeliveryChannelId = deliveryChannelId;
            this.DeliveryStatus = UserNotificationDeliveryStatus.Unknown;
            this.DeliveryStatusLastUpdated = DateTimeOffset.UtcNow;
            this.ReadStatus = UserNotificationReadStatus.Unknown;
            this.ReadStatusLastUpdated = DateTimeOffset.UtcNow;
        }

        /// <summary>
        /// Deserialization-specific constructor for initializing an instance of the <see cref="UserNotificationStatus"/> class.
        /// </summary>
        /// <param name="deliveryChannelId">The <see cref="DeliveryChannelId" />.</param>
        /// <param name="deliveryStatus">The <see cref="DeliveryStatus" />.</param>
        /// <param name="deliveryStatusLastUpdated">The <see cref="DeliveryStatusLastUpdated" />.</param>
        /// <param name="readStatus">The <see cref="ReadStatus" />.</param>
        /// <param name="readStatusLastUpdated">The <see cref="ReadStatusLastUpdated" />.</param>
        /// <param name="failureReason">The <see cref="FailureReason"/>.</param>
        [JsonConstructor]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public UserNotificationStatus(
            string deliveryChannelId,
            UserNotificationDeliveryStatus deliveryStatus,
            DateTimeOffset deliveryStatusLastUpdated,
            UserNotificationReadStatus readStatus,
            DateTimeOffset readStatusLastUpdated,
            string? failureReason = null)
        {
            this.DeliveryChannelId = deliveryChannelId;
            this.DeliveryStatus = deliveryStatus;
            this.DeliveryStatusLastUpdated = deliveryStatusLastUpdated.ToUniversalTime();
            this.ReadStatus = readStatus;
            this.ReadStatusLastUpdated = readStatusLastUpdated.ToUniversalTime();
            this.FailureReason = failureReason;
        }

        /// <summary>
        /// Gets the Id of the delivery channel that this instance relates to.
        /// </summary>
        public string DeliveryChannelId { get; }

        /// <summary>
        /// Gets the delivery status.
        /// </summary>
        public UserNotificationDeliveryStatus DeliveryStatus { get; }

        /// <summary>
        /// Gets the date/time that the delivery status was last updated.
        /// </summary>
        public DateTimeOffset DeliveryStatusLastUpdated { get; }

        /// <summary>
        /// Gets the read status.
        /// </summary>
        public UserNotificationReadStatus ReadStatus { get; }

        /// <summary>
        /// Gets the date/time that the read status was last updated.
        /// </summary>
        public DateTimeOffset ReadStatusLastUpdated { get; }

        /// <summary>
        /// Gets the failure reason if the delivery channel status is failed.
        /// </summary>
        public string? FailureReason { get; }
    }
}
