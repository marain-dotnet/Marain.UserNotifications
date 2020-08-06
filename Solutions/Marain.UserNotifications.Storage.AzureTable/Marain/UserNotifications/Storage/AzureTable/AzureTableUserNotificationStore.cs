// <copyright file="AzureTableUserNotificationStore.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Storage.AzureTable
{
    using System;
    using System.Threading.Tasks;
    using Corvus.Extensions.Json;
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
    }
}
