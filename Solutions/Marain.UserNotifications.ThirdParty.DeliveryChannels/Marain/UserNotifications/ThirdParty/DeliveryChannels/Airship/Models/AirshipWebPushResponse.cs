// <copyright file="AirshipWebPushResponse.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.ThirdParty.DeliveryChannels.Airship.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Newtonsoft.Json;

    /// <summary>
    /// Response sent by the airship push api.
    /// </summary>
    public class AirshipWebPushResponse
    {
        /// <summary>
        /// Gets or sets an array of identifiers used for reporting. Each ID represents a localized message.
        /// </summary>
        [JsonProperty("localized_ids")]
        public List<string>? LocalizedIds { get; set; }

        /// <summary>
        /// Gets or sets an array of push IDs, each uniquely indentifying a push. Max items: 100 Min items: 1.
        /// </summary>
        [JsonProperty("push_ids")]
        public List<string>? PushIds { get; set; }

        /// <summary>
        /// Gets or sets a unique string identifying the operation, useful for reporting and troubleshooting.
        /// </summary>
        [JsonProperty("operation_id")]
        public string? OperationId { get; set; }

        /// <summary>
        /// Gets or sets success.
        /// </summary>
        [JsonProperty("ok")]
        public string? OkResult { get; set; }
    }
}
