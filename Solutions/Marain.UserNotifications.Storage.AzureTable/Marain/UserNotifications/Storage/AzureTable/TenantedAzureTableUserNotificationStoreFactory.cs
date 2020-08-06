// <copyright file="TenantedAzureTableUserNotificationStoreFactory.cs" company="Endjin Limited">
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
    /// An implementation of <see cref="ITenantedUserNotificationStoreFactory"/> for <see cref="AzureTableUserNotificationStore"/>.
    /// </summary>
    public class TenantedAzureTableUserNotificationStoreFactory : ITenantedUserNotificationStoreFactory
    {
        private readonly ILogger logger;
        private readonly IJsonSerializerSettingsProvider serializerSettingsProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="TenantedAzureTableUserNotificationStoreFactory"/> class.
        /// </summary>
        /// <param name="serializerSettingsProvider">The serialization settings provider.</param>
        /// <param name="logger">The logger.</param>
        public TenantedAzureTableUserNotificationStoreFactory(
            IJsonSerializerSettingsProvider serializerSettingsProvider,
            ILogger<TenantedAzureTableUserNotificationStoreFactory> logger)
        {
            this.logger = logger
                ?? throw new ArgumentNullException(nameof(logger));
            this.serializerSettingsProvider = serializerSettingsProvider
                ?? throw new ArgumentNullException(nameof(serializerSettingsProvider));
        }

        /// <inheritdoc/>
        public Task<IUserNotificationStore> GetNotificationStoreForTenantAsync(ITenant tenant)
        {
            throw new NotImplementedException();
        }
    }
}
