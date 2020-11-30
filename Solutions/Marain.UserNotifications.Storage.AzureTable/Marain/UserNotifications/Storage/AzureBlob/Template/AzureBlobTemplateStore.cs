// <copyright file="AzureBlobTemplateStore.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Storage.AzureBlob
{
    using System;
    using System.Threading.Tasks;
    using Corvus.Extensions.Json;
    using Marain.UserNotifications;
    using Marain.UserPreferences;
    using Microsoft.Azure.Storage.Blob;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    /// <summary>
    /// An implementation of <see cref="ITemplateStore"/> over Azure Blob storage.
    /// </summary>
    public class AzureBlobTemplateStore : ITemplateStore
    {
        private readonly ILogger logger;
        private readonly IJsonSerializerSettingsProvider serializerSettingsProvider;
        private readonly CloudBlobContainer blobContainer;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureBlobTemplateStore"/> class.
        /// </summary>
        /// <param name="blobContainer">The blob container.</param>
        /// <param name="serializerSettingsProvider">The serialization settings provider.</param>
        /// <param name="logger">The logger.</param>
        public AzureBlobTemplateStore(
            CloudBlobContainer blobContainer,
            IJsonSerializerSettingsProvider serializerSettingsProvider,
            ILogger logger)
        {
            this.logger = logger
                ?? throw new ArgumentNullException(nameof(logger));
            this.serializerSettingsProvider = serializerSettingsProvider
                ?? throw new ArgumentNullException(nameof(serializerSettingsProvider));
            this.blobContainer = blobContainer
                ?? throw new ArgumentNullException(nameof(blobContainer));
        }

        /// <inheritdoc/>
        public async Task<NotificationTypeTemplate> StoreAsync(NotificationTypeTemplate template)
        {
            this.logger.LogDebug("Storing template for notification type ", template.NotificationType);

            // Gets the blob reference by the NotificationType
            CloudBlockBlob blockBlob = this.blobContainer.GetBlockBlobReference(template.NotificationType);

            // Serialise the TemplateWrapper object
            string templateBlob = JsonConvert.SerializeObject(template, this.serializerSettingsProvider.Instance);

            // Save the TemplateWrapper to the blob storage
            await blockBlob.UploadTextAsync(templateBlob).ConfigureAwait(false);

            return template;
        }

        /// <inheritdoc/>
        public async Task<NotificationTypeTemplate?> GetAsync(string notificationType)
        {
            // Gets the blob reference by the notificationType
            CloudBlockBlob blob = this.blobContainer.GetBlockBlobReference(notificationType);

            // Check if the blob exists
            bool exists = await blob.ExistsAsync().ConfigureAwait(false);

            if (!exists)
            {
                return null;
            }

            // Download and convert the blob text into TemplateWrapper object
            string json = await blob.DownloadTextAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<NotificationTypeTemplate>(json, this.serializerSettingsProvider.Instance);
        }
    }
}
