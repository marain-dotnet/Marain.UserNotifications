// <copyright file="QuietTime.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.ThirdParty.DeliveryChannels.Airship.Models
{
    using Newtonsoft.Json;

    /// <summary>
    /// QuietTime Model.
    /// </summary>
    public class QuietTime
    {
        /// <summary>
        /// Gets or sets start time.
        /// </summary>
        [JsonProperty("start")]
        public string? Start { get; set; }

        /// <summary>
        /// Gets or sets end time.
        /// </summary>
        [JsonProperty("end")]
        public string? End { get; set; }
    }
}