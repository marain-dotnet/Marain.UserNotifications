// <copyright file="TenantedAzureTableUserNotificationStoreFactory.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Storage.AzureStorage
{
    using System;
    using System.Threading.Tasks;
    using Azure.Data.Tables;
    using Corvus.Extensions.Json;
    using Corvus.Storage.Azure.TableStorage.Tenancy;
    using Corvus.Tenancy;
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
        public const string TableName =  "usernotifications";
        private const string TemplatesV2ConfigurationKey = "StorageConfiguration__Table__" + TableName;
        private const string TemplatesV3ConfigurationKey = "Marain:UserNotifications:TableConfiguration:UserNotifications";

        private readonly ILogger logger;
        private readonly IJsonSerializerSettingsProvider serializerSettingsProvider;
        private readonly ITableSourceWithTenantLegacyTransition tableFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="TenantedAzureTableUserNotificationStoreFactory"/> class.
        /// </summary>
        /// <param name="tableFactory">The cloud table factory.</param>
        /// <param name="serializerSettingsProvider">The serialization settings provider.</param>
        /// <param name="logger">The logger.</param>
        public TenantedAzureTableUserNotificationStoreFactory(
            ITableSourceWithTenantLegacyTransition tableFactory,
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
            TableClient table =
                await this.tableFactory.GetTableClientFromTenantAsync(
                    tenant, TemplatesV2ConfigurationKey, TemplatesV3ConfigurationKey, TableName).ConfigureAwait(false);

            // No need to cache these instances as they are lightweight wrappers around the container.
            return new AzureTableUserNotificationStore(table, this.serializerSettingsProvider, this.logger);
        }
    }
}