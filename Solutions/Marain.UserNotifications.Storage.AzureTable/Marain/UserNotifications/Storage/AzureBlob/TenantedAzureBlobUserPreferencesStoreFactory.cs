// <copyright file="TenantedAzureBlobUserPreferencesStoreFactory.cs" company="Endjin Limited">
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
    /// An implementation of <see cref="ITenantedUserPreferencesStoreFactory"/> for <see cref="TenantedAzureBlobUserPreferencesStoreFactory"/>.
    /// </summary>
    public class TenantedAzureBlobUserPreferencesStoreFactory : ITenantedUserPreferencesStoreFactory
    {
        /// <summary>
        /// The blob definition for the store. This is used to look up the corresponding configuration from the
        /// tenant.
        /// </summary>
        public static readonly BlobStorageContainerDefinition BlobContainerDefinition = new BlobStorageContainerDefinition("userpreferences");

        private readonly ILogger logger;
        private readonly IJsonSerializerSettingsProvider serializerSettingsProvider;
        private readonly ITenantCloudBlobContainerFactory blobContainerFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="TenantedAzureBlobUserPreferencesStoreFactory"/> class.
        /// </summary>
        /// <param name="blobContainerFactory">The cloud blob factory.</param>
        /// <param name="serializerSettingsProvider">The serialization settings provider.</param>
        /// <param name="logger">The logger.</param>
        public TenantedAzureBlobUserPreferencesStoreFactory(
            ITenantCloudBlobContainerFactory blobContainerFactory,
            IJsonSerializerSettingsProvider serializerSettingsProvider,
            ILogger<TenantedAzureBlobUserPreferencesStoreFactory> logger)
        {
            this.logger = logger
                ?? throw new ArgumentNullException(nameof(logger));
            this.serializerSettingsProvider = serializerSettingsProvider
                ?? throw new ArgumentNullException(nameof(serializerSettingsProvider));
            this.blobContainerFactory = blobContainerFactory
                ?? throw new ArgumentNullException(nameof(blobContainerFactory));
        }

        /// <inheritdoc/>#
        public async Task<IUserPreferencesStore> GetUserPreferencesStoreForTenantAsync(ITenant tenant)
        {
            // Gets the blob container for the tenant or creates a new one if it does not exists
            CloudBlobContainer blob = await this.blobContainerFactory.GetBlobContainerForTenantAsync(tenant, BlobContainerDefinition).ConfigureAwait(false);

            // No need to cache these instances as they are lightweight wrappers around the container.
            return new AzureBlobUserPreferencesStore(blob, this.serializerSettingsProvider, this.logger);
        }
    }
}
