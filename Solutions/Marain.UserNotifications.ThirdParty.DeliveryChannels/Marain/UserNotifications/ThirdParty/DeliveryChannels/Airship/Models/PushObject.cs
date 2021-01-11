// <copyright file="PushObject.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.ThirdParty.DeliveryChannels.Airship.Models
{
    using Newtonsoft.Json;
    using System.Collections.Generic;

    public class PushObject
    {
        [JsonProperty("notification")]
        public Notification Notification { get; set; }

        [JsonProperty("audience")]
        public Audience Audience { get; set; }

        // web, android, ios
        [JsonProperty("device_types")]
        public List<string> DeviceTypes { get; set; }

    }
}
