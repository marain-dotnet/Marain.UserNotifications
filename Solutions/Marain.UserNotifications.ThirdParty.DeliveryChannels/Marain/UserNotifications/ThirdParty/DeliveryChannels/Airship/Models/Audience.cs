// <copyright file="Audience.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.ThirdParty.DeliveryChannels.Airship.Models
{
    using Newtonsoft.Json;

    public class Audience
    {
        [JsonProperty("named_user")]
        public string NamedUser { get; set; }
    }
}