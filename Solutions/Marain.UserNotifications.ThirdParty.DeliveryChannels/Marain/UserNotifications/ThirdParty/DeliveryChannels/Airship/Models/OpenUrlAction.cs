// <copyright file="OpenUrlAction.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.ThirdParty.DeliveryChannels.Airship.Models
{
    using Newtonsoft.Json;

    public class OpenUrlAction
    {
        [JsonProperty("content")]
        public string Content { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }
    }
}