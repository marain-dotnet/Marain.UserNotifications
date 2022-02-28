// <copyright file="AzureBlobTemplateStore.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Storage.AzureStorage
{
    using System;
    using System.Net;
    using System.Threading.Tasks;
    using Azure;
    using Azure.Storage.Blobs;
    using Azure.Storage.Blobs.Models;
    using Corvus.Extensions.Json;
    using Marain.Models;
    using Marain.NotificationTemplates;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    /// <summary>
    /// An implementation of <see cref="INotificationTemplateStore"/> over Azure Blob storage.
    /// </summary>
    public class AzureBlobTemplateStore : INotificationTemplateStore
    {
        private readonly ILogger logger;
        private readonly IJsonSerializerSettingsProvider serializerSettingsProvider;
        private readonly BlobContainerClient blobContainer;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureBlobTemplateStore"/> class.
        /// </summary>
        /// <param name="blobContainer">The blob container.</param>
        /// <param name="serializerSettingsProvider">The serialization settings provider.</param>
        /// <param name="logger">The logger.</param>
        public AzureBlobTemplateStore(
            BlobContainerClient blobContainer,
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
        public async Task<(T Template, string? ETag)> GetAsync<T>(string notificationType, CommunicationType communicationType)
        {
            // Gets the blob reference by the notificationType
            BlobClient blob = this.blobContainer.GetBlobClient(this.GetBlockBlobName(notificationType, communicationType));

            try
            {
                // Download and convert the blob text into TemplateWrapper object
                Response<BlobDownloadResult> response = await blob.DownloadContentAsync().ConfigureAwait(false);
                string json = response.Value.Content.ToString();
                T dynamicObject = JsonConvert.DeserializeObject<T>(json, this.serializerSettingsProvider.Instance);
                string etag = response.Value.Details.ETag.ToString("H");
                return (dynamicObject, etag);
            }
            catch (RequestFailedException ex)
            when (ex.Status == 404)
            {
                throw new NotificationTemplateNotFoundException(this.GetBlockBlobName(notificationType, communicationType));
            }
        }

        /// <inheritdoc/>
        public async Task<T> CreateOrUpdate<T>(string notificationType, CommunicationType communicationType, string? eTag, T template)
        {
            this.logger.LogDebug("CreateOrUpdate: Storing template for notification type ", notificationType);

            // Gets the blob reference by the NotificationType
            BlobClient blockBlob = this.blobContainer.GetBlobClient(this.GetBlockBlobName(notificationType, communicationType));

            // Serialise the TemplateWrapper object
            string templateBlob = JsonConvert.SerializeObject(template, this.serializerSettingsProvider.Instance);
            BlobRequestConditions uploadConditions = string.IsNullOrWhiteSpace(eTag)
                ? new BlobRequestConditions { IfNoneMatch = ETag.All }
                : new BlobRequestConditions { IfMatch = new ETag(eTag) };

            try
            {
                Response<BlobContentInfo> response = await  blockBlob.UploadAsync(
                        BinaryData.FromString(templateBlob),
                        new BlobUploadOptions { Conditions = uploadConditions });
            }
            catch (RequestFailedException ex)
            when (ex.Status == (int)HttpStatusCode.PreconditionFailed)
            {
                throw new NotificationTemplateStoreConcurrencyException(
                    string.IsNullOrWhiteSpace(eTag)
                        ? "Template already in store"
                        : "Template in store did not match ETag",
                    ex);
            }

            this.logger.LogDebug("CreateOrUpdate: Notification template updated successfully ", notificationType);

            return template;
        }

        private string GetBlockBlobName(string notificationType, CommunicationType communicationType)
        {
            return $"{notificationType}:{communicationType}";
        }
    }
}