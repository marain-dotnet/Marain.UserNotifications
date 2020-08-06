// <copyright file="NotificationTableEntity.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Storage.AzureTable.Internal
{
    using System;
    using System.Text;
    using Corvus.Json;
    using Microsoft.Azure.Cosmos.Table;
    using Newtonsoft.Json;

    /// <summary>
    /// Table storage specific version of a <see cref="Notification"/>.
    /// </summary>
    public class NotificationTableEntity : TableEntity
    {
        /// <summary>
        /// Gets or sets the type of the notification.
        /// </summary>
        public string NotificationType { get; set; }

        /// <summary>
        /// Gets or sets the list of correlation Ids associated with the notification, serialized as Json.
        /// </summary>
        public string CorrelationIdsJson { get; set; }

        /// <summary>
        /// Gets or sets the date and time at which the event being notified took place.
        /// </summary>
        public DateTimeOffset NotificationTimestamp { get; set; }

        /// <summary>
        /// Gets or sets the properties of the notification, serialized as JSON.
        /// </summary>
        public string PropertiesJson { get; set; }

        /// <summary>
        /// Creates a new <see cref="NotificationTableEntity"/> from a <see cref="Notification"/>.
        /// </summary>
        /// <param name="source">The source notification.</param>
        /// <param name="serializerSettings">The serialization settings.</param>
        /// <returns>A new <see cref="NotificationTableEntity"/>.</returns>
        public static NotificationTableEntity FromNotification(Notification source, JsonSerializerSettings serializerSettings)
        {
            long reversedTimestamp = long.MaxValue - source.Timestamp.ToUnixTimeMilliseconds();
            string correlationIdsJson = JsonConvert.SerializeObject(source.Metadata.CorrelationIds, serializerSettings);
            string propertiesJson = JsonConvert.SerializeObject(source.Properties, serializerSettings);
            string hash = source.GetIdentityHash(serializerSettings);
            string rowKey = $"{reversedTimestamp}-{hash}";

            return new NotificationTableEntity
            {
                PartitionKey = source.UserId,
                RowKey = rowKey,
                NotificationType = source.NotificationType,
                NotificationTimestamp = source.Timestamp,
                CorrelationIdsJson = correlationIdsJson,
                PropertiesJson = propertiesJson,
                ETag = source.Metadata.ETag,
            };
        }

        /// <summary>
        /// Returns the entity as a <see cref="Notification"/>.
        /// </summary>
        /// <param name="serializerSettings">The serialization settings.</param>
        /// <returns>The converted <see cref="Notification"/>.</returns>
        public Notification ToNotification(JsonSerializerSettings serializerSettings)
        {
            string[] correlationIds = JsonConvert.DeserializeObject<string[]>(this.CorrelationIdsJson, serializerSettings);
            IPropertyBag properties = JsonConvert.DeserializeObject<IPropertyBag>(this.PropertiesJson, serializerSettings);
            string id = GetIdFromPartitionKeyAndRowKey(this.PartitionKey, this.RowKey, serializerSettings);

            return new Notification(
                id,
                this.NotificationType,
                this.PartitionKey,
                this.NotificationTimestamp,
                properties,
                correlationIds,
                this.ETag);
        }

        private static string GetIdFromPartitionKeyAndRowKey(string partitionKey, string rowKey, JsonSerializerSettings serializerSettings)
        {
            string serializedValues = JsonConvert.SerializeObject(new NotificationId { PartitionKey = partitionKey, RowKey = rowKey }, serializerSettings);
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(serializedValues));
        }

        private class NotificationId
        {
            public string PartitionKey { get; set; }

            public string RowKey { get; set; }
        }
    }
}
