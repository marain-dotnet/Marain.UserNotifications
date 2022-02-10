// <copyright file="TenantedAzureTableUserNotificationStoreFactory.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Storage.AzureStorage
{
    using System;
    using System.Threading.Tasks;
    using Corvus.Azure.Storage.Tenancy;
    using Corvus.Extensions.Json;
    using Corvus.Tenancy;
    using Microsoft.Azure.Cosmos.Table;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// An implementation of <see cref="ITenantedUserNotificationStoreFactory"/> for <see cref="AzureTableUserNotificationStore"/>.
    /// </summary>
    public class TenantedAzureTableUserNotificationStoreFactory : ITenantedUserNotificationStoreFactory
    {
        /// <summary>
        /// The table definition for the store. This is used to look up the corresponding configuration from the
        /// tenant.
        /// </summary>
        public static readonly TableStorageTableDefinition TableDefinition = new TableStorageTableDefinition("usernotifications");

        private readonly ILogger logger;
        private readonly IJsonSerializerSettingsProvider serializerSettingsProvider;
        private readonly ITenantCloudTableFactory tableFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="TenantedAzureTableUserNotificationStoreFactory"/> class.
        /// </summary>
        /// <param name="tableFactory">The cloud table factory.</param>
        /// <param name="serializerSettingsProvider">The serialization settings provider.</param>
        /// <param name="logger">The logger.</param>
        public TenantedAzureTableUserNotificationStoreFactory(
            ITenantCloudTableFactory tableFactory,
            IJsonSerializerSettingsProvider serializerSettingsProvider,
            ILogger<TenantedAzureTableUserNotificationStoreFactory> logger)
        {
            this.logger = logger
                ?? throw new ArgumentNullException(nameof(logger));
            this.serializerSettingsProvider = serializerSettingsProvider
                ?? throw new ArgumentNullException(nameof(serializerSettingsProvider));
            this.tableFactory = tableFactory
                ?? throw new ArgumentNullException(nameof(tableFactory));
        }

        /// <inheritdoc/>#
        public async Task<IUserNotificationStore> GetUserNotificationStoreForTenantAsync(ITenant tenant)
        {
            CloudTable table =
                await this.tableFactory.GetTableForTenantAsync(tenant, TableDefinition).ConfigureAwait(false);

            // No need to cache these instances as they are lightweight wrappers around the container.
            return new AzureTableUserNotificationStore(table, this.serializerSettingsProvider, this.logger);
        }
    }
}