// <copyright file="ChannelsResponse.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.ThirdParty.DeliveryChannels.Airship.Models
{
    using System;
    using Newtonsoft.Json;

    public class ChannelsResponse
    {
        [JsonProperty("next_page")]
        public Uri NextPage { get; set; }

        [JsonProperty("channels")]
        public Channel[] Channels { get; set; }
    }
}
