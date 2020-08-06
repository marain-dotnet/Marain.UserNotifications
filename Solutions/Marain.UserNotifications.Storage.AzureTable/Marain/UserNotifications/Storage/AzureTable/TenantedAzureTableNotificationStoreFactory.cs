// <copyright file="TenantedAzureTableNotificationStoreFactory.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Storage.AzureTable
{
    using System;
    using System.Threading.Tasks;
    using Corvus.Extensions.Json;
    using Corvus.Tenancy;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// An implementation of <see cref="ITenantedNotificationStoreFactory"/> for <see cref="AzureTableNotificationStore"/>.
    /// </summary>
    public class TenantedAzureTableNotificationStoreFactory : ITenantedNotificationStoreFactory
    {
        private readonly ILogger logger;
        private readonly IJsonSerializerSettingsProvider serializerSettingsProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="TenantedAzureTableNotificationStoreFactory"/> class.
        /// </summary>
        /// <param name="serializerSettingsProvider">The serialization settings provider.</param>
        /// <param name="logger">The logger.</param>
        public TenantedAzureTableNotificationStoreFactory(
            IJsonSerializerSettingsProvider serializerSettingsProvider,
            ILogger<TenantedAzureTableNotificationStoreFactory> logger)
        {
            this.logger = logger
                ?? throw new ArgumentNullException(nameof(logger));
            this.serializerSettingsProvider = serializerSettingsProvider
                ?? throw new ArgumentNullException(nameof(serializerSettingsProvider));
        }

        /// <inheritdoc/>
        public Task<INotificationStore> GetNotificationStoreForTenantAsync(ITenant tenant)
        {
            throw new NotImplementedException();
        }
    }
}
