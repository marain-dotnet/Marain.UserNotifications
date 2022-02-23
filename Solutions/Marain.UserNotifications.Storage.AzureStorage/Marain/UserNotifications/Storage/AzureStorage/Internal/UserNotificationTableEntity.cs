// <copyright file="UserNotificationTableEntity.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

#nullable disable
namespace Marain.UserNotifications.Storage.AzureStorage.Internal
{
    using System;
    using Azure;
    using Azure.Data.Tables;
    using Corvus.Json;
    using Newtonsoft.Json;

    /// <summary>
    /// Table storage specific version of a <see cref="UserNotification"/>.
    /// </summary>
    public class UserNotificationTableEntity : ITableEntity
    {
        /// <summary>
        /// Gets or sets the entity's ETag.
        /// </summary>
        /// Value:
        /// An Azure.Data.Tables.TableEntity.ETag containing the ETag value for the entity.
        public ETag ETag { get; set; }

        /// <summary>
        /// Gets or sets the Timestamp property.
        /// The Timestamp property is a DateTimeOffset value that is maintained on the server
        /// side to record the time an entity was last modified. The Table service uses the
        /// Timestamp property internally to provide optimistic concurrency. The value of
        /// Timestamp is a monotonically increasing value, meaning that each time the entity
        /// is modified, the value of Timestamp increases for that entity. This property
        /// should not be set on insert or update operations (the value will be ignored).
        /// </summary>
        ///
        /// Value:
        /// A System.DateTimeOffset containing the timestamp of the entity.
        public DateTimeOffset? Timestamp { get; set; }

        /// <summary>
        ///     Gets or sets the row key is a unique identifier for an entity within a given partition. Together,
        ///     the <see cref="Azure.Data.Tables.TableEntity.PartitionKey"/> and RowKey uniquely identify an
        ///     entity within a table.
        /// </summary>
        /// Value:
        /// A string containing the row key for the entity.
        public string RowKey { get; set; }

        /// <summary>
        /// Gets or sets the partition key.
        /// The partition key is a unique identifier for the partition within a given table
        /// and forms the first part of an entity's primary key.
        /// </summary>
        ///
        /// Value:
        /// A string containing the partition key for the entity.
        ///
        public string PartitionKey { get; set; }

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
        /// Gets or sets the channel delivery statuses, serialized as JSON.
        /// </summary>
        public string ChannelDeliveryStatusesJson { get; set; }

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
            string channelDeliveryStatusesJson = JsonConvert.SerializeObject(source.ChannelStatuses, serializerSettings);
            byte[] identityHash = source.GetIdentityHash(serializerSettings);
            string rowKey = $"{reversedTimestamp:D21}-{GetHexadecimalString(identityHash)}";

            return new UserNotificationTableEntity
            {
                PartitionKey = source.UserId,
                RowKey = rowKey,
                NotificationType = source.NotificationType,
                NotificationTimestamp = source.Timestamp,
                CorrelationIdsJson = correlationIdsJson,
                PropertiesJson = propertiesJson,
                ChannelDeliveryStatusesJson = channelDeliveryStatusesJson,
                ETag = new ETag(source.Metadata.ETag),
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
            UserNotificationStatus[] channelDeliveryStatuses = JsonConvert.DeserializeObject<UserNotificationStatus[]>(this.ChannelDeliveryStatusesJson, serializerSettings);
            string id = new NotificationId(this.PartitionKey, this.RowKey).AsString(serializerSettings);

            return new UserNotification(
                id,
                this.NotificationType,
                this.PartitionKey,
                this.NotificationTimestamp,
                properties,
                new UserNotificationMetadata(correlationIds, this.ETag.ToString("H")),
                channelDeliveryStatuses);
        }

        private static string GetHexadecimalString(byte[] identityHash)
        {
            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            var sBuilder = new System.Text.StringBuilder(identityHash.Length * 2);

            // Loop through each byte of the hashed data
            // and format each one as a hexadecimal string.
            for (int i = 0; i < identityHash.Length; i++)
            {
                sBuilder.AppendFormat(identityHash[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }
    }
}