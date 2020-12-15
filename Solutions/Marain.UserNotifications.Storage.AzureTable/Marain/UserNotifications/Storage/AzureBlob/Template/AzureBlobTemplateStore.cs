// <copyright file="AzureBlobTemplateStore.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Storage.AzureBlob
{
    using System;
    using System.Threading.Tasks;
    using Corvus.Extensions.Json;
    using Marain.NotificationTemplates;
    using Marain.UserPreferences;
    using Microsoft.Azure.Storage.Blob;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    /// <summary>
    /// An implementation of <see cref="INotificationTemplateStore"/> over Azure Blob storage.
    /// </summary>
    public class AzureBlobTemplateStore : INotificationTemplateStore
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
        public async Task<T> StoreAsync<T>(string notificationType, CommunicationType communicationType, T template)
        {
            this.logger.LogDebug("Storing template for notification type ", notificationType);

            // Gets the blob reference by the NotificationType
            CloudBlockBlob blockBlob = this.blobContainer.GetBlockBlobReference(this.GetBlockBlobName(notificationType, communicationType));

            // Serialise the TemplateWrapper object
            string templateBlob = JsonConvert.SerializeObject(template, this.serializerSettingsProvider.Instance);

            // Save the TemplateWrapper to the blob storage
            await blockBlob.UploadTextAsync(templateBlob).ConfigureAwait(false);

            return template;
        }

        /// <inheritdoc/>
        public async Task<T?> GetAsync<T>(string notificationType, CommunicationType communicationType)
        {
            // Gets the blob reference by the notificationType
            CloudBlockBlob blob = this.blobContainer.GetBlockBlobReference(this.GetBlockBlobName(notificationType, communicationType));

            // Check if the blob exists
            bool exists = await blob.ExistsAsync().ConfigureAwait(false);

            if (!exists)
            {
                return default(T);
            }

            // Download and convert the blob text into TemplateWrapper object
            string json = await blob.DownloadTextAsync().ConfigureAwait(false);
            return JsonConvert.DeserializeObject<T>(json, this.serializerSettingsProvider.Instance);
        }

        private string GetBlockBlobName(string notificationType, CommunicationType communicationType)
        {
            return $"{notificationType}:{communicationType.ToString()}";
        }
    }
}
