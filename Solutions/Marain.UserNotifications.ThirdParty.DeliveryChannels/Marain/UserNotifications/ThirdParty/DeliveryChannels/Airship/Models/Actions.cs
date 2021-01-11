// <copyright file="Actions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.ThirdParty.DeliveryChannels.Airship.Models
{
    using Newtonsoft.Json;

    public class Actions
    {
        [JsonProperty("open")]
        public OpenUrlAction Open { get; set; }
    }
}