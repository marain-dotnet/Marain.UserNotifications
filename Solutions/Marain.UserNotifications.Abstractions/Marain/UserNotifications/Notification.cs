// <copyright file="Notification.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications
{
    using System;
    using Corvus.Json;

    /// <summary>
    /// A single notification targetted at a specific user.
    /// </summary>
    public class Notification
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Notification"/> class.
        /// </summary>
        /// <param name="notificationId">The <see cref="NotificationId"/>.</param>
        /// <param name="notificationType">The <see cref="NotificationType" />.</param>
        /// <param name="userId">The <see cref="UserId" />.</param>
        /// <param name="timestamp">The <see cref="Timestamp" />.</param>
        /// <param name="properties">The <see cref="Properties" />.</param>
        /// <param name="correlationIds">The <see cref="CorrelationIds" />.</param>
        public Notification(
            string? notificationId,
            string notificationType,
            string userId,
            DateTime timestamp,
            IPropertyBag properties,
            string[] correlationIds)
        {
            this.NotificationId = notificationId;
            this.NotificationType = notificationType;
            this.UserId = userId;
            this.Timestamp = timestamp;
            this.Properties = properties;
            this.CorrelationIds = correlationIds;
        }

        /// <summary>
        /// Gets the Id of the notification. If not set, this is a new notification that has not yet been stored.
        /// </summary>
        /// <remarks>
        /// The Id will be specific to the underlying storage mechanism being used and should never be set or modified
        /// externally.
        /// </remarks>
        public string? NotificationId { get; }

        /// <summary>
        /// Gets the type of the notification. These types are defined by the consuming application, so can be
        /// arbitrary strings. It is strongly recommended that you namespace and version these types.
        /// </summary>
        /// <example><c>contoso.foosystem.barcategory.actualnotification.v1</c>.</example>
        /// <example><c>facebook.friendrequests.received</c>.</example>
        public string NotificationType { get; }

        /// <summary>
        /// Gets the Id of the user that this notification is for.
        /// </summary>
        public string UserId { get; }

        /// <summary>
        /// Gets a list of correlation Ids associated with the notification.
        /// </summary>
        public string[] CorrelationIds { get; }

        /// <summary>
        /// Gets the date and time at which the event being notified took place.
        /// </summary>
        public DateTime Timestamp { get; }

        /// <summary>
        /// Gets additional data associated with the notification. This is generally used by a delivery channel
        /// to construct a human readable message for the notification.
        /// </summary>
        public IPropertyBag Properties { get; }
    }
}
