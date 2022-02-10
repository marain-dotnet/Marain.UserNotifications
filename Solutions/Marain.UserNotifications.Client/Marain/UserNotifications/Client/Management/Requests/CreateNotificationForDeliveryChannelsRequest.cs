// <copyright file="CreateNotificationForDeliveryChannelsRequest.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Client.Management.Requests
{
    using System;
    using System.Collections.Generic;
    using Corvus.Json;
    using Marain.UserNotifications.Client.Management.Resources;

    /// <summary>
    /// Request to create a new notification with communication type and configured delivery channel.
    /// endpoint.
    /// </summary>
    public class CreateNotificationForDeliveryChannelsRequest
    {
        /// <summary>
        /// Gets or sets the type of the notification. These types are defined by the consuming application, so can be
        /// arbitrary strings. It is strongly recommended that you namespace and version these types.
        /// </summary>
        /// <example><c>contoso.foosystem.barcategory.actualnotification.v1</c>.</example>
        /// <example><c>facebook.friendrequests.received</c>.</example>
        public string NotificationType { get; set; }

        /// <summary>
        /// Gets or sets the Ids of the users that this notification is for.
        /// </summary>
        public string[] UserIds { get; set; }

        /// <summary>
        /// Gets or sets a list of correlation Ids associated with the notification.
        /// </summary>
        public string[] CorrelationIds { get; set; }

        /// <summary>
        /// Gets or sets the date and time at which the event being notified took place.
        /// </summary>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the desired delivery channels which are configured for the communication type.
        /// </summary>
        public Dictionary<string, string> DeliveryChannelConfiguredPerCommunicationType { get; set; }

        /// <summary>
        /// Gets or sets additional data associated with the notification. This is generally used by a delivery channel
        /// to construct a human readable message for the notification.
        /// </summary>
        public IDictionary<string, object> Properties { get; set; }
    }
}