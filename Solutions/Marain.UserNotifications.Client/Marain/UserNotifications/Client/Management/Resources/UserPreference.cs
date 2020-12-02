// <copyright file="UserPreference.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Client.Management.Resources
{
    using System;
    using System.Collections.Generic;
    using System.Text.Json.Serialization;
    using Newtonsoft.Json.Converters;

    /// <summary>
    /// The user preference structure.
    /// </summary>
    public class UserPreference
    {
        /// <summary>
        /// Gets or Sets the UserId of the owner.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or Sets the Email of the owner.
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or Sets the PhoneNumber of the owner.
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or Sets the CommunicationChannelsPerNotificationConfiguration.
        /// This will be notification: list of communication channels configured.
        /// </summary>
        [JsonConverter(typeof(StringEnumConverter))]
        public Dictionary<string, List<CommunicationType>> CommunicationChannelsPerNotificationConfiguration { get; set; }

        /// <summary>
        /// Gets or Sets the date and time at which the user preferences were last updated.
        /// </summary>
        public DateTimeOffset Timestamp { get; set; }
    }
}
