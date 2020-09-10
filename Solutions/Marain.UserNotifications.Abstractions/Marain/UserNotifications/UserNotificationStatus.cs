﻿// <copyright file="UserNotificationStatus.cs" company="Endjin Limited">
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
        [JsonConstructor]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public UserNotificationStatus(
            string deliveryChannelId,
            UserNotificationDeliveryStatus deliveryStatus,
            DateTimeOffset deliveryStatusLastUpdated,
            UserNotificationReadStatus readStatus,
            DateTimeOffset readStatusLastUpdated)
        {
            this.DeliveryChannelId = deliveryChannelId;
            this.DeliveryStatus = deliveryStatus;
            this.DeliveryStatusLastUpdated = deliveryStatusLastUpdated.ToUniversalTime();
            this.ReadStatus = readStatus;
            this.ReadStatusLastUpdated = readStatusLastUpdated.ToUniversalTime();
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
        public DateTimeOffset DeliveryStatusLastUpdated { get; private set; }

        /// <summary>
        /// Gets the read status.
        /// </summary>
        public UserNotificationReadStatus ReadStatus { get; private set; }

        /// <summary>
        /// Gets the date/time that the read status was last updated.
        /// </summary>
        public DateTimeOffset ReadStatusLastUpdated { get; private set; }

        /// <summary>
        /// Updates the delivery status to the specified value.
        /// </summary>
        /// <param name="newStatus">The new delivery status.</param>
        /// <param name="effectiveDateTime">The time at which the update occurred.</param>
        public void UpdateDeliveryStatus(UserNotificationDeliveryStatus newStatus, DateTimeOffset effectiveDateTime)
        {
            this.DeliveryStatus = newStatus;
            this.DeliveryStatusLastUpdated = effectiveDateTime.ToUniversalTime();
        }

        /// <summary>
        /// Updates the read status to the specified value.
        /// </summary>
        /// <param name="newStatus">The new read status.</param>
        /// <param name="effectiveDateTime">The time at which the update occurred.</param>
        public void UpdateReadStatus(UserNotificationReadStatus newStatus, DateTimeOffset effectiveDateTime)
        {
            this.ReadStatus = newStatus;
            this.ReadStatusLastUpdated = effectiveDateTime.ToUniversalTime();
        }
    }
}