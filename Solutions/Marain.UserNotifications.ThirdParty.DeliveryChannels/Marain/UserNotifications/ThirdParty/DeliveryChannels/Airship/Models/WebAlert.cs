// <copyright file="WebAlert.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.ThirdParty.DeliveryChannels.Airship.Models
{
    using System.Collections.Generic;
    using Newtonsoft.Json;

    /// <summary>
    /// Web Alert Model.
    /// </summary>
    public class WebAlert
    {
        /// <summary>
        /// Gets or sets alert.
        /// </summary>
        [JsonProperty("alert")]
        [JsonRequired]
        public string? Alert { get; set; }

        /// <summary>
        /// Gets or sets title.
        /// </summary>
        [JsonProperty("title")]
        [JsonRequired]
        public string? Title { get; set; }

        /// <summary>
        /// Gets or sets providing this object allows a user to be sent an
        /// image inside a notification.
        /// </summary>
        [JsonProperty("image")]
        public Image? Image { get; set; }

        /// <summary>
        /// Gets or sets buttons.
        /// </summary>
        [JsonProperty("buttons")]
        public IList<Button>? Buttons { get; set; }
    }
}