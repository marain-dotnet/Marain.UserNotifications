// <copyright file="AzureTableUserNotificationStore.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Storage.AzureStorage
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;

    using Azure;
    using Azure.Data.Tables;
    using Corvus.Json.Serialization;
    using Marain.UserNotifications;
    using Marain.UserNotifications.Storage.AzureStorage.Internal;

    using Microsoft.Extensions.Logging;

    /// <summary>
    /// An implementation of <see cref="IUserNotificationStore"/> over Azure Table storage.
    /// </summary>
    public class AzureTableUserNotificationStore : IUserNotificationStore
    {
        private readonly ILogger logger;
        private readonly IJsonSerializerOptionsProvider serializerOptionsProvider;
        private readonly TableClient table;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureTableUserNotificationStore"/> class.
        /// </summary>
        /// <param name="table">The underlying cloud table.</param>
        /// <param name="serializerOptionsProvider">The serialization options provider.</param>
        /// <param name="logger">The logger.</param>
        public AzureTableUserNotificationStore(
            TableClient table,
            IJsonSerializerOptionsProvider serializerOptionsProvider,
            ILogger logger)
        {
            this.logger = logger
                ?? throw new ArgumentNullException(nameof(logger));
            this.serializerOptionsProvider = serializerOptionsProvider
                ?? throw new ArgumentNullException(nameof(serializerOptionsProvider));
            this.table = table
                ?? throw new ArgumentNullException(nameof(table));
        }

        /// <inheritdoc/>
        public Task<GetNotificationsResult> GetAsync(string userId, string? sinceUserNotificationId, int maxItems)
        {
            string? afterRowKey = null;

            if (!string.IsNullOrEmpty(sinceUserNotificationId))
            {
                var decodedId = NotificationId.FromString(sinceUserNotificationId, this.serializerOptionsProvider.Instance);

                afterRowKey = decodedId.RowKey;
            }

            return this.GetInternalAsync(userId, null, afterRowKey, maxItems);
        }

        /// <inheritdoc/>
        public Task<GetNotificationsResult> GetAsync(string userId, string continuationToken)
        {
            var requestContinuationToken =
                ContinuationToken.FromString(continuationToken, this.serializerOptionsProvider.Instance);

            if (userId != requestContinuationToken.UserId)
            {
                throw new ArgumentException("The supplied continuation token was not generated for user '{userId}'");
            }

            return this.GetInternalAsync(
                requestContinuationToken.UserId,
                requestContinuationToken.BeforeRowKey,
                requestContinuationToken.AfterRowKey,
                requestContinuationToken.MaxItems);
        }

        /// <inheritdoc/>
        public async Task<UserNotification> GetByIdAsync(string id)
        {
            var notificationId = NotificationId.FromString(id, this.serializerOptionsProvider.Instance);
            try
            {
                Response<UserNotificationTableEntity> result = await this.table.GetEntityAsync<UserNotificationTableEntity>(notificationId.PartitionKey, notificationId.RowKey).ConfigureAwait(false);
                return result.Value.ToNotification(this.serializerOptionsProvider.Instance);
            }
            catch (RequestFailedException ex)
            {
                if (ex.Status == 404)
                {
                    throw new UserNotificationNotFoundException(id);
                }

                throw new AzureTableUserNotificationStoreException($"Unexpected response code '{ex.Status}' from table storage when attempting to retrieve the notification with Id '{id}'");
            }
        }

        /// <inheritdoc/>
        public async Task<UserNotification> StoreAsync(UserNotification notification)
        {
            this.logger.LogDebug(
                "Storing notification for user {0}",
                notification.UserId);

            await this.table.CreateIfNotExistsAsync().ConfigureAwait(false);

            if (string.IsNullOrEmpty(notification.Metadata.ETag))
            {
                // We're inserting this notification
                return await this.InsertAsync(notification).ConfigureAwait(false);
            }

            return await this.UpdateAsync(notification).ConfigureAwait(false);
        }

        private async Task<UserNotification> InsertAsync(UserNotification notification)
        {
            var notificationEntity = UserNotificationTableEntity.FromNotification(notification, this.serializerOptionsProvider.Instance);

            try
            {
                Response result = await this.table.AddEntityAsync(notificationEntity).ConfigureAwait(false);

                // TODO: Check result status code
                if (result.Headers.ETag.HasValue)
                {
                    notificationEntity.ETag = result.Headers.ETag.Value;
                }

                return notificationEntity.ToNotification(this.serializerOptionsProvider.Instance);
            }
            catch (RequestFailedException ex)
            {
                throw new UserNotificationStoreConcurrencyException("Could not create the notification because a notification with the same identity hash already exists in the store.", ex);
            }
        }

        private async Task<UserNotification> UpdateAsync(UserNotification notification)
        {
            var notificationEntity = UserNotificationTableEntity.FromNotification(notification, this.serializerOptionsProvider.Instance);

            try
            {
                Response result = await this.table.UpdateEntityAsync(notificationEntity, notificationEntity.ETag).ConfigureAwait(false);

                if (result.Headers.ETag.HasValue)
                {
                    notificationEntity.ETag = result.Headers.ETag.Value;
                }

                return notificationEntity.ToNotification(this.serializerOptionsProvider.Instance);
            }
            catch (RequestFailedException ex)
            {
                if (ex.Status == 404)
                {
                    throw new UserNotificationNotFoundException(notification.Id ?? "{null}");
                }
                else if (ex.Status == (int)HttpStatusCode.PreconditionFailed)
                {
                    throw new UserNotificationStoreConcurrencyException($"Could not update the notification with Id '{notification.Id}' because it has been modified by another process.", ex);
                }

                throw;
            }
        }

        private async Task<GetNotificationsResult> GetInternalAsync(
            string userId,
            string? beforeRowKey,
            string? afterRowKey,
            int maxItems)
        {
            string filter = $"PartitionKey eq '{userId}'";

            if (!string.IsNullOrEmpty(beforeRowKey))
            {
                filter += $" and RowKey gt '{beforeRowKey}'";
            }

            if (!string.IsNullOrEmpty(afterRowKey))
            {
                filter = filter += $" and RowKey lt '{afterRowKey}'";
            }

            AsyncPageable<UserNotificationTableEntity> query = this.table.QueryAsync<UserNotificationTableEntity>(filter, maxItems);

            var results = new List<UserNotificationTableEntity>(maxItems);
            await foreach (UserNotificationTableEntity notification in query.Take(maxItems))
            {
                results.Add(notification);
            }

            // If we've managed to read up to the max items, then there might be more - build a continuation token
            // to return.
            ContinuationToken? responseContinuationToken = results.Count >= maxItems
                ? new ContinuationToken(userId, maxItems, results[^1].RowKey, afterRowKey)
                : null;

            return new GetNotificationsResult(
              results.Select(x => x.ToNotification(this.serializerOptionsProvider.Instance)).ToArray(),
              responseContinuationToken?.AsString(this.serializerOptionsProvider.Instance));
        }
    }
}