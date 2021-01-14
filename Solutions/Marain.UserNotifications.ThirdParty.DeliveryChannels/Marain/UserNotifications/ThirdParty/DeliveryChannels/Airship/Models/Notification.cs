// <copyright file="Notification.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>
namespace Marain.UserNotifications.ThirdParty.DeliveryChannels.Airship.Models
{
    using Newtonsoft.Json;

    /// <summary>
    /// Base Notification Object.
    /// </summary>
    public class Notification
    {
        /// <summary>
        /// Gets or sets alert.
        /// </summary>
        [JsonProperty("alert")]
        public string? Alert { get; set; }

        /// <summary>
        /// Gets or sets web Alert.
        /// </summary>
        [JsonProperty("web")]
        public WebAlert? Web { get; set; }

        /// <summary>
        /// Gets or sets actions.
        /// </summary>
        [JsonProperty("actions")]
        public Actions? Actions { get; set; }
    }
}