// <copyright file="CreateNotificationsRequest.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Client.Management.Requests
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Request to create a new notification.
    /// </summary>
    public class CreateNotificationsRequest
    {
        /// <summary>
        /// Gets or sets the type of the notification. These types are defined by the consuming application, so can be
        /// arbitrary strings. It is strongly recommended that you version these types, as shown in the example.
        /// </summary>
        public string NotificationType { get; set; }

        /// <summary>
        /// Gets or sets the Ids of the users that this notification is for.
        /// </summary>
        public string[] UserIds { get; set; }

        /// <summary>
        /// Gets or sets the list of correlation Ids for the notification.
        /// </summary>
        public string[] CorrelationIds { get; set; }

        /// <summary>
        /// Gets or sets the notification data.
        /// </summary>
        public IDictionary<string, object> Properties { get; set; }

        /// <summary>
        /// Gets or sets the date and time at which the event being notified took place.
        /// </summary>
        public DateTimeOffset Timestamp { get; set; }
    }
}