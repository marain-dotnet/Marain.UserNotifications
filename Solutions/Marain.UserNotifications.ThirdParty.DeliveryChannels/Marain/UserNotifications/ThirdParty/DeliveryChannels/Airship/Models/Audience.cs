// <copyright file="Audience.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.ThirdParty.DeliveryChannels.Airship.Models
{
    using Newtonsoft.Json;

    /// <summary>
    /// Audience Model.
    /// </summary>
    public class Audience
    {
        /// <summary>
        /// Gets or sets named user.
        /// </summary>
        [JsonProperty("named_user")]
        public string? NamedUser { get; set; }
    }
}