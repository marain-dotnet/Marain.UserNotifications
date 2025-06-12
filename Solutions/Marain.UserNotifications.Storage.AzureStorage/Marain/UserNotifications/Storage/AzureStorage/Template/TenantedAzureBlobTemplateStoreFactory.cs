// <copyright file="TenantedAzureBlobTemplateStoreFactory.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Storage.AzureStorage
{
    using System;
    using System.Threading.Tasks;
    using Azure.Storage.Blobs;
    using Corvus.Json.Serialization;
    using Corvus.Storage.Azure.BlobStorage.Tenancy;
    using Corvus.Tenancy;
    using Marain.NotificationTemplates;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// An implementation of <see cref="ITenantedNotificationTemplateStoreFactory"/> for <see cref="TenantedAzureBlobTemplateStoreFactory"/>.
    /// </summary>
    public class TenantedAzureBlobTemplateStoreFactory : ITenantedNotificationTemplateStoreFactory
    {
        /// <summary>
        /// The blob definition for the store. This is used to look up the corresponding configuration from.
        /// tenant.
        /// </summary>
        public const string BlobContainerName = "templates";
        private const string TemplatesV2ConfigurationKey = "StorageConfiguration__" + BlobContainerName;
        private const string TemplatesV3ConfigurationKey = "Marain:UserNotifications:BlobContainerConfiguration:Templates";

        private readonly ILogger logger;
        private readonly IJsonSerializerOptionsProvider jsonSerializerOptionsProvider;
        private readonly IBlobContainerSourceWithTenantLegacyTransition blobContainerFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="INotificationTemplateStore"/> class.
        /// </summary>
        /// <param name="blobContainerFactory">The cloud blob factory.</param>
        /// <param name="jsonSerializerOptionsProvider">The serialization options provider.</param>
        /// <param name="logger">The logger.</param>
        public TenantedAzureBlobTemplateStoreFactory(
            IBlobContainerSourceWithTenantLegacyTransition blobContainerFactory,
            IJsonSerializerOptionsProvider jsonSerializerOptionsProvider,
            ILogger<TenantedAzureBlobTemplateStoreFactory> logger)
        {
            this.logger = logger
                ?? throw new ArgumentNullException(nameof(logger));
            this.jsonSerializerOptionsProvider = jsonSerializerOptionsProvider
                ?? throw new ArgumentNullException(nameof(jsonSerializerOptionsProvider));
            this.blobContainerFactory = blobContainerFactory
                ?? throw new ArgumentNullException(nameof(blobContainerFactory));
        }

        /// <inheritdoc/>
        public async Task<INotificationTemplateStore> GetTemplateStoreForTenantAsync(ITenant tenant)
        {
            // Gets the blob container for the tenant or creates a new one if it does not exist
            BlobContainerClient blob = await this.blobContainerFactory.GetBlobContainerClientFromTenantAsync(
                tenant, TemplatesV2ConfigurationKey, TemplatesV3ConfigurationKey, BlobContainerName).ConfigureAwait(false);

            // No need to cache these instances as they are lightweight wrappers around the container.
            return new AzureBlobTemplateStore(blob, this.jsonSerializerOptionsProvider, this.logger);
        }
    }
}