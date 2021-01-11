// <copyright file="Button.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.ThirdParty.DeliveryChannels.Airship.Models
{
    using Newtonsoft.Json;

    public class Button
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        [JsonProperty("label")]
        public string Label { get; set; }
        [JsonProperty("actions")]
        public Actions Actions { get; set; }
    }
}