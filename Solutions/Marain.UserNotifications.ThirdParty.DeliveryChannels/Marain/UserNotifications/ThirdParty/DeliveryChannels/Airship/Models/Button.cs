// <copyright file="Button.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.ThirdParty.DeliveryChannels.Airship.Models
{
    using Newtonsoft.Json;

    /// <summary>
    /// Button Model.
    /// </summary>
    public class Button
    {
        /// <summary>
        /// Gets or sets id of the button.
        /// </summary>
        [JsonProperty("id")]
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets button label.
        /// </summary>
        [JsonProperty("label")]
        public string? Label { get; set; }

        /// <summary>
        /// Gets or sets button action.
        /// </summary>
        [JsonProperty("actions")]
        public Actions? Actions { get; set; }
    }
}