// <copyright file="OpenUrlAction.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.ThirdParty.DeliveryChannels.Airship.Models
{
    using Newtonsoft.Json;

    /// <summary>
    /// OpenUrlAction Model.
    /// </summary>
    public class OpenUrlAction
    {
        /// <summary>
        /// Gets or sets content.
        /// </summary>
        [JsonProperty("content")]
        public string? Content { get; set; }

        /// <summary>
        /// Gets or sets type.
        /// </summary>
        [JsonProperty("type")]
        public string? Type { get; set; }
    }
}