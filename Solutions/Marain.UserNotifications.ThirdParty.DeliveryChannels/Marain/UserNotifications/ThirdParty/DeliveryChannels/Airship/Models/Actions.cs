// <copyright file="Actions.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.ThirdParty.DeliveryChannels.Airship.Models
{
    using Newtonsoft.Json;

    /// <summary>
    /// Actions Model.
    /// </summary>
    public class Actions
    {
        /// <summary>
        /// Gets or sets open url action.
        /// </summary>
        [JsonProperty("open")]
        public OpenUrlAction? Open { get; set; }
    }
}