// <copyright file="UserNotificationTableEntity.cs" company="Endjin Limited">
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
    /// Table storage specific version of a <see cref="UserNotification"/>.
    /// </summary>
    public class UserNotificationTableEntity : TableEntity
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
        /// Creates a new <see cref="UserNotificationTableEntity"/> from a <see cref="UserNotification"/>.
        /// </summary>
        /// <param name="source">The source notification.</param>
        /// <param name="serializerSettings">The serialization settings.</param>
        /// <returns>A new <see cref="UserNotificationTableEntity"/>.</returns>
        public static UserNotificationTableEntity FromNotification(UserNotification source, JsonSerializerSettings serializerSettings)
        {
            // We're going to use a "reversed" timestamp in the row key. Our standard query will be to get recent
            // notifications for a user in descending order of timestamp, potentially since the previous set of
            // notifications was retrieved. Using the reversed timestamp for the row key means that this will be the
            // default sort order. This means that when we look for new notifications since a specified time, the
            // resulting partition scan will be able to complete as soon as it encounters a record with an earlier
            // timestamp, or when it hits the maximum requested items.
            long reversedTimestamp = long.MaxValue - source.Timestamp.ToUnixTimeMilliseconds();
            string correlationIdsJson = JsonConvert.SerializeObject(source.Metadata.CorrelationIds, serializerSettings);
            string propertiesJson = JsonConvert.SerializeObject(source.Properties, serializerSettings);
            string hash = source.GetIdentityHash(serializerSettings);
            string rowKey = $"{reversedTimestamp:D21}-{hash}";

            return new UserNotificationTableEntity
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
        /// Returns the entity as a <see cref="UserNotification"/>.
        /// </summary>
        /// <param name="serializerSettings">The serialization settings.</param>
        /// <returns>The converted <see cref="UserNotification"/>.</returns>
        public UserNotification ToNotification(JsonSerializerSettings serializerSettings)
        {
            string[] correlationIds = JsonConvert.DeserializeObject<string[]>(this.CorrelationIdsJson, serializerSettings);
            IPropertyBag properties = JsonConvert.DeserializeObject<IPropertyBag>(this.PropertiesJson, serializerSettings);
            string id = GetIdFromPartitionKeyAndRowKey(this.PartitionKey, this.RowKey, serializerSettings);

            return new UserNotification(
                id,
                this.NotificationType,
                this.PartitionKey,
                this.NotificationTimestamp,
                properties,
                new UserNotificationMetadata(correlationIds, this.ETag));
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
