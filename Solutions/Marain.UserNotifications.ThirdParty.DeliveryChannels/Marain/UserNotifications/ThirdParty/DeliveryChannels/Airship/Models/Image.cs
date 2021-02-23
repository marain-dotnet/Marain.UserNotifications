// <copyright file="Image.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.ThirdParty.DeliveryChannels.Airship.Models
{
    using Newtonsoft.Json;

    /// <summary>
    /// Image Model.
    /// </summary>
    public class Image
    {
        /// <summary>
        /// Gets or sets url of the image which will be present in the notification.
        /// </summary>
        [JsonProperty("url")]
        public string? Url { get; set; }
    }
}