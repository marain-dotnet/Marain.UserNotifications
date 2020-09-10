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
            this.DeliveryStatusLastUpdatedUtc = DateTimeOffset.UtcNow;
            this.ReadStatus = UserNotificationReadStatus.Unknown;
            this.ReadStatusLastUpdatedUtc = DateTimeOffset.UtcNow;
        }

        /// <summary>
        /// Deserialization-specific constructor for initializing an instance of the <see cref="UserNotificationStatus"/> class.
        /// </summary>
        /// <param name="deliveryChannelId">The <see cref="DeliveryChannelId" />.</param>
        /// <param name="deliveryStatus">The <see cref="DeliveryStatus" />.</param>
        /// <param name="deliveryStatusLastUpdatedUtc">The <see cref="DeliveryStatusLastUpdatedUtc" />.</param>
        /// <param name="readStatus">The <see cref="ReadStatus" />.</param>
        /// <param name="readStatusLastUpdatedUtc">The <see cref="ReadStatusLastUpdatedUtc" />.</param>
        [JsonConstructor]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public UserNotificationStatus(
            string deliveryChannelId,
            UserNotificationDeliveryStatus deliveryStatus,
            DateTimeOffset deliveryStatusLastUpdatedUtc,
            UserNotificationReadStatus readStatus,
            DateTimeOffset readStatusLastUpdatedUtc)
        {
            this.DeliveryChannelId = deliveryChannelId;
            this.DeliveryStatus = deliveryStatus;
            this.DeliveryStatusLastUpdatedUtc = deliveryStatusLastUpdatedUtc;
            this.ReadStatus = readStatus;
            this.ReadStatusLastUpdatedUtc = readStatusLastUpdatedUtc;
        }

        /// <summary>
        /// Gets the Id of the delivery channel that this instance relates to.
        /// </summary>
        public string DeliveryChannelId { get; }

        /// <summary>
        /// Gets the delivery status.
        /// </summary>
        public UserNotificationDeliveryStatus DeliveryStatus { get; private set; }

        /// <summary>
        /// Gets the date/time that the delivery status was last updated.
        /// </summary>
        public DateTimeOffset DeliveryStatusLastUpdatedUtc { get; private set; }

        /// <summary>
        /// Gets the read status.
        /// </summary>
        public UserNotificationReadStatus ReadStatus { get; private set; }

        /// <summary>
        /// Gets the date/time that the read status was last updated.
        /// </summary>
        public DateTimeOffset ReadStatusLastUpdatedUtc { get; private set; }

        /// <summary>
        /// Updates the delivery status to the specified value.
        /// </summary>
        /// <param name="newStatus">The new delivery status.</param>
        /// <param name="effectiveDateTimeUtc">The time at which the update occurred.</param>
        public void UpdateDeliveryStatus(UserNotificationDeliveryStatus newStatus, DateTimeOffset effectiveDateTimeUtc)
        {
            this.DeliveryStatus = newStatus;
            this.DeliveryStatusLastUpdatedUtc = effectiveDateTimeUtc;
        }

        /// <summary>
        /// Updates the read status to the specified value.
        /// </summary>
        /// <param name="newStatus">The new read status.</param>
        /// <param name="effectiveDateTimeUtc">The time at which the update occurred.</param>
        public void UpdateReadStatus(UserNotificationReadStatus newStatus, DateTimeOffset effectiveDateTimeUtc)
        {
            this.ReadStatus = newStatus;
            this.ReadStatusLastUpdatedUtc = effectiveDateTimeUtc;
        }
    }
}
