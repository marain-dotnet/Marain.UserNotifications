// <copyright file="PushObject.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.ThirdParty.DeliveryChannels.Airship.Models
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// PushObject Model.
    /// </summary>
    public class PushObject
    {
        /// <summary>
        /// Gets or sets notification model.
        /// </summary>
        [JsonProperty("notification")]
        public Notification? Notification { get; set; }

        /// <summary>
        /// Gets or sets audience.
        /// </summary>
        [JsonProperty("audience")]
        public Audience? Audience { get; set; }

        /// <summary>
        /// Gets or sets web, android, ios.
        /// </summary>
        [JsonProperty("device_types")]
        public List<string>? DeviceTypes { get; set; }
    }
}
