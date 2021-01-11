// <copyright file="AzureBlobDeliveryChannelConfigurationStore.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Storage.AzureBlob
{
    using System;
    using System.Threading.Tasks;
    using Corvus.Extensions.Json;
    using Marain.DeliveryChannelConfiguration;
    using Microsoft.Azure.Storage.Blob;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    /// <summary>
    /// An implementation of <see cref="IDeliveryChannelConfigurationStore"/> over Azure Blob storage.
    /// </summary>
    public class AzureBlobDeliveryChannelConfigurationStore : IDeliveryChannelConfigurationStore
    {
        private readonly ILogger logger;
        private readonly IJsonSerializerSettingsProvider serializerSettingsProvider;
        private readonly CloudBlobContainer blobContainer;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureBlobDeliveryChannelConfigurationStore"/> class.
        /// </summary>
        /// <param name="blobContainer">The blob container.</param>
        /// <param name="serializerSettingsProvider">The serialization settings provider.</param>
        /// <param name="logger">The logger.</param>
        public AzureBlobDeliveryChannelConfigurationStore(
            CloudBlobContainer blobContainer,
            IJsonSerializerSettingsProvider serializerSettingsProvider,
            ILogger logger)
        {
            this.blobContainer = blobContainer
                ?? throw new ArgumentNullException(nameof(blobContainer));
            this.serializerSettingsProvider = serializerSettingsProvider
                ?? throw new ArgumentNullException(nameof(serializerSettingsProvider));
            this.logger = logger
               ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>
        public async Task<DeliveryChannelConfiguration?> GetAsync(string tenantId)
        {
            this.logger.LogDebug("GetAsync DeliveryChannelConfiguration called for tenant ", tenantId);

            // Gets the blob reference by the tenantId
            CloudBlockBlob blockBlob = this.blobContainer.GetBlockBlobReference(tenantId);

            // Check if the blob exists
            bool exists = await blockBlob.ExistsAsync().ConfigureAwait(false);

            if (!exists)
            {
                return null;
            }

            // Download and convert the blob text into DeliveryChannelConfiguration object
            string json = await blockBlob.DownloadTextAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<DeliveryChannelConfiguration>(json, this.serializerSettingsProvider.Instance);
        }

        /// <inheritdoc/>
        public async Task<DeliveryChannelConfiguration> CreateOrUpdate(string tenantId, DeliveryChannelConfiguration deliveryChannelConfiguration)
        {
            this.logger.LogDebug("CreateOrUpdate  Delivery Channel Config called for tenant ", tenantId);

            // Gets the blob reference by the tenanctId
            CloudBlockBlob blockBlob = this.blobContainer.GetBlockBlobReference(tenantId);

            // Serialise the deliveryChannelConfiguration object
            string deliveryChannelConfigurationObject = JsonConvert.SerializeObject(deliveryChannelConfiguration, this.serializerSettingsProvider.Instance);

            // Update the Delivery Channel Configuration
            await blockBlob.UploadTextAsync(deliveryChannelConfigurationObject).ConfigureAwait(false);
            this.logger.LogDebug("CreateOrUpdate: Delivery Channel Config request was performed successfully ", tenantId);

            return deliveryChannelConfiguration;
        }
    }
}
