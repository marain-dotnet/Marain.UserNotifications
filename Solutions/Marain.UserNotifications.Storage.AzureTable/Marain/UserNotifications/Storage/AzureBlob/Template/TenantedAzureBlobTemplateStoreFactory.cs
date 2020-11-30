// <copyright file="TenantedAzureBlobTemplateStoreFactory.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Storage.AzureBlob
{
    using System;
    using System.Threading.Tasks;
    using Corvus.Azure.Storage.Tenancy;
    using Corvus.Extensions.Json;
    using Corvus.Tenancy;
    using Microsoft.Azure.Storage.Blob;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// An implementation of <see cref="ITenantedTemplateStoreFactory"/> for <see cref="TenantedAzureBlobTemplateStoreFactory"/>.
    /// </summary>
    public class TenantedAzureBlobTemplateStoreFactory : ITenantedTemplateStoreFactory
    {
        /// <summary>
        /// The blob definition for the store. This is used to look up the corresponding configuration from.
        /// tenant.
        /// </summary>
        public static readonly BlobStorageContainerDefinition BlobContainerDefinition = new BlobStorageContainerDefinition("templates");

        private readonly ILogger logger;
        private readonly IJsonSerializerSettingsProvider serializerSettingsProvider;
        private readonly ITenantCloudBlobContainerFactory blobContainerFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ITemplateStore"/> class.
        /// </summary>
        /// <param name="blobContainerFactory">The cloud blob factory.</param>
        /// <param name="serializerSettingsProvider">The serialization settings provider.</param>
        /// <param name="logger">The logger.</param>
        public TenantedAzureBlobTemplateStoreFactory(
            ITenantCloudBlobContainerFactory blobContainerFactory,
            IJsonSerializerSettingsProvider serializerSettingsProvider,
            ILogger<TenantedAzureBlobTemplateStoreFactory> logger)
        {
            this.logger = logger
                ?? throw new ArgumentNullException(nameof(logger));
            this.serializerSettingsProvider = serializerSettingsProvider
                ?? throw new ArgumentNullException(nameof(serializerSettingsProvider));
            this.blobContainerFactory = blobContainerFactory
                ?? throw new ArgumentNullException(nameof(blobContainerFactory));
        }

        /// <inheritdoc/>
        public async Task<ITemplateStore> GetTemplateStoreForTenantAsync(ITenant tenant)
        {
            // Gets the blob container for the tenant or creates a new one if it does not exists
            CloudBlobContainer blob = await this.blobContainerFactory.GetBlobContainerForTenantAsync(tenant, BlobContainerDefinition).ConfigureAwait(false);

            // No need to cache these instances as they are lightweight wrappers around the container.
            return new AzureBlobTemplateStore(blob, this.serializerSettingsProvider, this.logger);
        }
    }
}
