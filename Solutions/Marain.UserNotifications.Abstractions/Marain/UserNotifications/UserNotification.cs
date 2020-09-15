// <copyright file="UserNotification.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Immutable;
    using System.Diagnostics;
    using System.Text;
    using Corvus.Json;
    using Newtonsoft.Json;

    /// <summary>
    /// A single notification targetted at a specific user.
    /// </summary>
    [DebuggerDisplay("'{NotificationType}' for user '{UserId}' with timestamp '{Timestamp}'")]
    public class UserNotification
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserNotification"/> class.
        /// </summary>
        /// <param name="id">The <see cref="Id"/>.</param>
        /// <param name="notificationType">The <see cref="NotificationType" />.</param>
        /// <param name="userId">The <see cref="UserId" />.</param>
        /// <param name="timestamp">The <see cref="Timestamp" />.</param>
        /// <param name="properties">The <see cref="Properties" />.</param>
        /// <param name="metadata">The <see cref="Metadata"/>.</param>
        /// <param name="channelStatuses">The <see cref="ChannelStatuses"/>.</param>
        public UserNotification(
            string? id,
            string notificationType,
            string userId,
            DateTimeOffset timestamp,
            IPropertyBag properties,
            UserNotificationMetadata metadata,
#pragma warning disable SA1011 // Closing square brackets should be spaced correctly
            IEnumerable<UserNotificationStatus>? channelStatuses = null)
#pragma warning restore SA1011 // Closing square brackets should be spaced correctly
        {
            this.Id = id;
            this.NotificationType = notificationType ?? throw new ArgumentNullException(nameof(notificationType));
            this.UserId = userId ?? throw new ArgumentNullException(nameof(userId));
            this.Timestamp = timestamp != default ? timestamp.ToUniversalTime() : throw new ArgumentException(nameof(timestamp));
            this.Properties = properties ?? throw new ArgumentNullException(nameof(properties));
            this.Metadata = metadata ?? throw new ArgumentNullException(nameof(metadata));
            this.ChannelStatuses = channelStatuses?.ToImmutableArray() ?? ImmutableArray<UserNotificationStatus>.Empty;
        }

        /// <summary>
        /// Gets the Id of the notification. If not set, this is a new notification that has not yet been stored.
        /// </summary>
        /// <remarks>
        /// The Id will be specific to the underlying storage mechanism being used and should not be set externally.
        /// </remarks>
        public string? Id { get; }

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
        /// Gets the date and time at which the event being notified took place.
        /// </summary>
        public DateTimeOffset Timestamp { get; }

        /// <summary>
        /// Gets additional data associated with the notification. This is generally when dispatching to a delivery
        /// channel to construct a channel-specific human readable message for the notification.
        /// </summary>
        public IPropertyBag Properties { get; }

        /// <summary>
        /// Gets metadata for the notification.
        /// </summary>
        public UserNotificationMetadata Metadata { get; }

        /// <summary>
        /// Gets the notification statuses for each channel that the notification has been sent on.
        /// </summary>
        public ImmutableArray<UserNotificationStatus> ChannelStatuses { get; }

        /// <summary>
        /// Constructs a hash for the notification that can be used to determine whether two notifications are
        /// equivalent. This takes into account the notification's <see cref="NotificationType"/>, <see cref="UserId"/>,
        /// <see cref="Timestamp"/> and <see cref="Properties"/>, but does not include <see cref="Id"/> or
        /// <see cref="Metadata"/>. It is normally used to determine whether a notification already exists in storage
        /// if the service receives the same request to create a notification multiple times.
        /// </summary>
        /// <param name="serializerSettings">The JsonSerializerSettings that will be used.</param>
        /// <returns>A hash for the notification.</returns>
        public string GetIdentityHash(JsonSerializerSettings serializerSettings)
        {
            string propertiesJson = JsonConvert.SerializeObject(this.Properties, serializerSettings);
            string fingerprint = $"{this.UserId}{this.Timestamp.ToUnixTimeMilliseconds()}{this.NotificationType}{propertiesJson}";
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(fingerprint));
        }
    }
}
