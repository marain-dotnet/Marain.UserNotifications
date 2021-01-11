// <copyright file="Ios.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.ThirdParty.DeliveryChannels.Airship.Models
{
    using Newtonsoft.Json;

    public class Ios
    {
        [JsonProperty("badge")]
        public long Badge { get; set; }

        [JsonProperty("quiettime")]
        public QuietTime QuietTime { get; set; }

        [JsonProperty("tz")]
        public string Tz { get; set; }
    }
}
