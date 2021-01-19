// <copyright file="CreateNotificationForDeliveryChannelsRequest.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Management.Host.Models
{
    using System;
    using System.Collections.Generic;
    using Corvus.Json;
    using Marain.Models;
    using Marain.UserNotifications.Management.Host.OpenApi;

    /// <summary>
    /// Request body for the <see cref="CreateNotificationForDeliveryChannelsService.CreateNotificationForDeliveryChannelsRequestAsync(Menes.IOpenApiContext, CreateNotificationForDeliveryChannelsRequest)"/>
    /// endpoint.
    /// </summary>
    public class CreateNotificationForDeliveryChannelsRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateNotificationForDeliveryChannelsRequest"/> class.
        /// </summary>
        /// <param name="notificationType">The <see cref="NotificationType" />.</param>
        /// <param name="userIds">The <see cref="UserIds" />.</param>
        /// <param name="timestamp">The <see cref="Timestamp" />.</param>
        /// <param name="deliveryChannelConfiguredPerCommunicationType">The <see cref="DeliveryChannelConfiguredPerCommunicationType"/>.</param>
        /// <param name="properties">The <see cref="Properties" />.</param>
        /// <param name="correlationIds">The <see cref="CorrelationIds" />.</param>
        public CreateNotificationForDeliveryChannelsRequest(
            string notificationType,
            string[] userIds,
            DateTime timestamp,
            IPropertyBag properties,
            string[] correlationIds,
            Dictionary<CommunicationType, DeliveryChannel>? deliveryChannelConfiguredPerCommunicationType = null)
        {
            this.NotificationType = notificationType;
            this.UserIds = userIds;
            this.Timestamp = timestamp;
            this.Properties = properties;
            this.CorrelationIds = correlationIds;
            this.DeliveryChannelConfiguredPerCommunicationType = deliveryChannelConfiguredPerCommunicationType;
        }

        /// <summary>
        /// Gets the type of the notification. These types are defined by the consuming application, so can be
        /// arbitrary strings. It is strongly recommended that you namespace and version these types.
        /// </summary>
        /// <example><c>contoso.foosystem.barcategory.actualnotification.v1</c>.</example>
        /// <example><c>facebook.friendrequests.received</c>.</example>
        public string NotificationType { get; }

        /// <summary>
        /// Gets the Ids of the users that this notification is for.
        /// </summary>
        public string[] UserIds { get; }

        /// <summary>
        /// Gets a list of correlation Ids associated with the notification.
        /// </summary>
        public string[] CorrelationIds { get; }

        /// <summary>
        /// Gets the date and time at which the event being notified took place.
        /// </summary>
        public DateTime Timestamp { get; }

        /// <summary>
        /// Gets the desired delivery channels which are configured for the communication type.
        /// </summary>
        public Dictionary<CommunicationType, DeliveryChannel>? DeliveryChannelConfiguredPerCommunicationType { get; }

        /// <summary>
        /// Gets additional data associated with the notification. This is generally used by a delivery channel
        /// to construct a human readable message for the notification.
        /// </summary>
        public IPropertyBag Properties { get; }
    }
}
