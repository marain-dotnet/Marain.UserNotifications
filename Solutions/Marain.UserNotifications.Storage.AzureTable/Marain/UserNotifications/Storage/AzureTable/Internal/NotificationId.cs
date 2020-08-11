// <copyright file="NotificationId.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Storage.AzureTable.Internal
{
    using System;
    using System.Text;
    using Newtonsoft.Json;

    /// <summary>
    /// An internal helper class for working with notification Ids.
    /// </summary>
    internal class NotificationId
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationId"/> class.
        /// </summary>
        /// <param name="partitionKey">The <see cref="PartitionKey"/>.</param>
        /// <param name="rowKey">The <see cref="RowKey"/>.</param>
        public NotificationId(string partitionKey, string rowKey)
        {
            this.PartitionKey = partitionKey;
            this.RowKey = rowKey;
        }

        /// <summary>
        /// Gets the partition key for the <see cref="UserNotificationTableEntity"/>.
        /// </summary>
        public string PartitionKey { get; }

        /// <summary>
        /// Gets the row key for the <see cref="UserNotificationTableEntity"/>.
        /// </summary>
        public string RowKey { get; }

        /// <summary>
        /// Creates a new instance of <see cref="NotificationId"/> from the provided string.
        /// </summary>
        /// <param name="serializedNotificationId">The serialized notification Id.</param>
        /// <param name="serializerSettings">The <see cref="JsonSerializerSettings"/> that were used to create the Id.</param>
        /// <returns>The deserialized <see cref="NotificationId"/>.</returns>
        public static NotificationId FromString(string serializedNotificationId, JsonSerializerSettings serializerSettings)
        {
            string idJson = Encoding.UTF8.GetString(Convert.FromBase64String(serializedNotificationId));
            return JsonConvert.DeserializeObject<NotificationId>(idJson, serializerSettings);
        }

        /// <summary>
        /// Converts the <see cref="NotificationId"/> into a serialized and encoded string.
        /// </summary>
        /// <param name="serializerSettings">The <see cref="JsonSerializerSettings"/>.</param>
        /// <returns>The notification Id as a string.</returns>
        public string AsString(JsonSerializerSettings serializerSettings)
        {
            string serializedValues = JsonConvert.SerializeObject(this, serializerSettings);
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(serializedValues));
        }
    }
}
