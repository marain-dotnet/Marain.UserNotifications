// <copyright file="AzureTableUserNotificationStore.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Storage.AzureTable
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Corvus.Extensions.Json;
    using Marain.UserNotifications;
    using Marain.UserNotifications.Storage.AzureTable.Internal;
    using Microsoft.Azure.Cosmos.Table;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// An implementation of <see cref="IUserNotificationStore"/> over Azure Table storage.
    /// </summary>
    public class AzureTableUserNotificationStore : IUserNotificationStore
    {
        private readonly ILogger logger;
        private readonly IJsonSerializerSettingsProvider serializerSettingsProvider;
        private readonly CloudTable table;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureTableUserNotificationStore"/> class.
        /// </summary>
        /// <param name="table">The underlying cloud table.</param>
        /// <param name="serializerSettingsProvider">The serialization settings provider.</param>
        /// <param name="logger">The logger.</param>
        public AzureTableUserNotificationStore(
            CloudTable table,
            IJsonSerializerSettingsProvider serializerSettingsProvider,
            ILogger logger)
        {
            this.logger = logger
                ?? throw new ArgumentNullException(nameof(logger));
            this.serializerSettingsProvider = serializerSettingsProvider
                ?? throw new ArgumentNullException(nameof(serializerSettingsProvider));
            this.table = table
                ?? throw new ArgumentNullException(nameof(table));
        }

        /// <inheritdoc/>
        public Task<GetNotificationsResult> GetAsync(string userId, string? sinceUserNotificationId, int maxItems)
        {
            string? afterRowKey = null;

            if (!string.IsNullOrEmpty(sinceUserNotificationId))
            {
                var decodedId = NotificationId.FromString(sinceUserNotificationId, this.serializerSettingsProvider.Instance);

                afterRowKey = decodedId.RowKey;
            }

            return this.GetInternalAsync(userId, null, afterRowKey, maxItems);
        }

        /// <inheritdoc/>
        public Task<GetNotificationsResult> GetAsync(string continuationToken)
        {
            var requestContinuationToken =
                ContinuationToken.FromString(continuationToken, this.serializerSettingsProvider.Instance);

            return this.GetInternalAsync(
                requestContinuationToken.UserId,
                requestContinuationToken.BeforeRowKey,
                requestContinuationToken.AfterRowKey,
                requestContinuationToken.MaxItems);
        }

        /// <inheritdoc/>
        public async Task<UserNotification> GetByIdAsync(string id)
        {
            var notificationId = NotificationId.FromString(id, this.serializerSettingsProvider.Instance);
            var operation = TableOperation.Retrieve<UserNotificationTableEntity>(notificationId.PartitionKey, notificationId.RowKey);
            TableResult result = await this.table.ExecuteAsync(operation).ConfigureAwait(false);

            // TODO: Check not found.
            var notification = (UserNotificationTableEntity)result.Result;

            return notification.ToNotification(this.serializerSettingsProvider.Instance);
        }

        /// <inheritdoc/>
        public async Task<UserNotification> StoreAsync(UserNotification notification)
        {
            this.logger.LogDebug(
                "Storing notification for user ",
                notification.UserId);

            await this.table.CreateIfNotExistsAsync().ConfigureAwait(false);

            var notificationEntity = UserNotificationTableEntity.FromNotification(notification, this.serializerSettingsProvider.Instance);

            var createOperation = TableOperation.Insert(notificationEntity);

            try
            {
                TableResult result = await this.table.ExecuteAsync(createOperation).ConfigureAwait(false);

                // TODO: Check result status code
                var response = (UserNotificationTableEntity)result.Result;
                return response.ToNotification(this.serializerSettingsProvider.Instance);
            }
            catch (StorageException ex) when (ex.Message == "Conflict")
            {
                throw new UserNotificationStoreConcurrencyException("Could not create the notification because a notification with the same identity hash already exists in the store.", ex);
            }
        }

        private async Task<GetNotificationsResult> GetInternalAsync(
            string userId,
            string? beforeRowKey,
            string? afterRowKey,
            int maxItems)
        {
            string filter = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, userId);

            if (!string.IsNullOrEmpty(beforeRowKey))
            {
                filter = TableQuery.CombineFilters(
                    filter,
                    TableOperators.And,
                    TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.GreaterThan, beforeRowKey));
            }

            if (!string.IsNullOrEmpty(afterRowKey))
            {
                filter = TableQuery.CombineFilters(
                    filter,
                    TableOperators.And,
                    TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.LessThan, afterRowKey));
            }

            TableQuery<UserNotificationTableEntity> query = new TableQuery<UserNotificationTableEntity>()
                .Where(filter)
                .Take(maxItems);

            TableContinuationToken? continuationToken = null;
            var results = new List<UserNotificationTableEntity>(maxItems);

            do
            {
                TableQuerySegment<UserNotificationTableEntity> result = await this.table.ExecuteQuerySegmentedAsync(
                    query,
                    continuationToken).ConfigureAwait(false);

                continuationToken = result.ContinuationToken;

                results.AddRange(result.Results);
            }
            while (results.Count < maxItems && !(continuationToken is null));

            // If we've managed to read up to the max items, then there might be more - build a continuation token
            // to return.
            ContinuationToken? responseContinuationToken = results.Count == maxItems
                ? new ContinuationToken(userId, maxItems, results[^1].RowKey, afterRowKey)
                : null;

            return new GetNotificationsResult(
                results.Select(x => x.ToNotification(this.serializerSettingsProvider.Instance)).ToArray(),
                responseContinuationToken?.AsString(this.serializerSettingsProvider.Instance));
        }
    }
}
