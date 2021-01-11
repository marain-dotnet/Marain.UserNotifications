// <copyright file="Channel.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.ThirdParty.DeliveryChannels.Airship.Models
{
    using System;
    using System.Collections.Generic;
    using Newtonsoft.Json;

    public class Channel
    {
        [JsonProperty("channel_id")]
        public Guid ChannelId { get; set; }

        [JsonProperty("device_type")]
        public string DeviceType { get; set; }

        [JsonProperty("push_address")]
        public string PushAddress { get; set; }

        [JsonProperty("opt_in")]
        public bool OptIn { get; set; }

        [JsonProperty("installed")]
        public bool Installed { get; set; }

        [JsonProperty("background")]
        public bool Background { get; set; }

        [JsonProperty("created")]
        public DateTimeOffset Created { get; set; }

        [JsonProperty("last_registration")]
        public DateTimeOffset LastRegistration { get; set; }

        [JsonProperty("named_user_id")]
        public string NamedUserId { get; set; }

        [JsonProperty("alias")]
        public string Alias { get; set; }

        [JsonProperty("tags")]
        public string[] Tags { get; set; }

        [JsonProperty("tag_groups")]
        public Dictionary<string, string> TagGroups { get; set; }

        [JsonProperty("ios")]
        public Ios Ios { get; set; }
    }
}
