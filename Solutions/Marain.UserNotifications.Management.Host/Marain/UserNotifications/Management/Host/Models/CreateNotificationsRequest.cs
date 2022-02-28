// <copyright file="CreateNotificationsRequest.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Management.Host.Models
{
    using System;
    using Corvus.Json;
    using Marain.UserNotifications.Management.Host.OpenApi;

    /// <summary>
    /// Request body for the <see cref="CreateNotificationsService.CreateNotificationsAsync(Menes.IOpenApiContext, CreateNotificationsRequest)"/>
    /// endpoint.
    /// </summary>
    public class CreateNotificationsRequest
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateNotificationsRequest"/> class.
        /// </summary>
        /// <param name="notificationType">The <see cref="NotificationType" />.</param>
        /// <param name="userIds">The <see cref="UserIds" />.</param>
        /// <param name="timestamp">The <see cref="Timestamp" />.</param>
        /// <param name="properties">The <see cref="Properties" />.</param>
        /// <param name="correlationIds">The <see cref="CorrelationIds" />.</param>
        public CreateNotificationsRequest(
            string notificationType,
            string[] userIds,
            DateTime timestamp,
            IPropertyBag properties,
            string[] correlationIds)
        {
            this.NotificationType = notificationType;
            this.UserIds = userIds;
            this.Timestamp = timestamp;
            this.Properties = properties;
            this.CorrelationIds = correlationIds;
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
        /// Gets additional data associated with the notification. This is generally used by a delivery channel
        /// to construct a human readable message for the notification.
        /// </summary>
        public IPropertyBag Properties { get; }
    }
}