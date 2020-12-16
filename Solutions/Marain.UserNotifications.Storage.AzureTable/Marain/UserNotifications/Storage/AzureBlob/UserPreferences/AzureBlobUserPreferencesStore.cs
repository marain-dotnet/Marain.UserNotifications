// <copyright file="AzureBlobUserPreferencesStore.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Storage.AzureBlob
{
    using System;
    using System.Threading.Tasks;
    using Corvus.Extensions.Json;
    using Marain.UserNotifications;
    using Marain.UserPreferences;
    using Microsoft.Azure.Storage;
    using Microsoft.Azure.Storage.Blob;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;

    /// <summary>
    /// An implementation of <see cref="IUserPreferencesStore"/> over Azure Blob storage.
    /// </summary>
    public class AzureBlobUserPreferencesStore : IUserPreferencesStore
    {
        private readonly ILogger logger;
        private readonly IJsonSerializerSettingsProvider serializerSettingsProvider;
        private readonly CloudBlobContainer blobContainer;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzureBlobUserPreferencesStore"/> class.
        /// </summary>
        /// <param name="blobContainer">The blob container.</param>
        /// <param name="serializerSettingsProvider">The serialization settings provider.</param>
        /// <param name="logger">The logger.</param>
        public AzureBlobUserPreferencesStore(
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
        public async Task<UserPreference?> GetAsync(string userId)
        {
            this.logger.LogDebug("GetAsync UserPreference called for user ", userId);

            // Gets the blob reference by the userId
            CloudBlockBlob blockBlob = this.blobContainer.GetBlockBlobReference(userId);

            // Check if the blob exists
            bool exists = await blockBlob.ExistsAsync().ConfigureAwait(false);

            if (!exists)
            {
                return null;
            }

            // Download and convert the blob text into UserPreference object
            string json = await blockBlob.DownloadTextAsync().ConfigureAwait(false);
            UserPreference? userPreferenceObject = JsonConvert.DeserializeObject<UserPreference>(json, this.serializerSettingsProvider.Instance);
            return userPreferenceObject.AddETag(userPreferenceObject, blockBlob.Properties.ETag);
        }

        /// <inheritdoc/>
        public async Task<UserPreference> CreateOrUpdate(UserPreference userPreference)
        {
            this.logger.LogDebug("CreateOrUpdate UserPreference called for user ", userPreference.UserId);

            // Gets the blob reference by the userId
            CloudBlockBlob blockBlob = this.blobContainer.GetBlockBlobReference(userPreference.UserId);

            // Check if the blob exists
            bool exists = await blockBlob.ExistsAsync().ConfigureAwait(false);

            // Serialise the userPreference object
            string userPreferenceBlob = JsonConvert.SerializeObject(userPreference, this.serializerSettingsProvider.Instance);

            if (exists && string.IsNullOrWhiteSpace(userPreference.ETag))
            {
                throw new StorageException("ETag was not present in the UserPreference object.");
            }

            if (exists)
            {
                // Update the user preference
                await blockBlob.UploadTextAsync(userPreferenceBlob, null, accessCondition: AccessCondition.GenerateIfMatchCondition(userPreference.ETag), null, null).ConfigureAwait(false);
                this.logger.LogDebug("CreateOrUpdate: User preference updated successfully ", userPreference.UserId);
            }
            else
            {
                // Create the user preference
                await blockBlob.UploadTextAsync(userPreferenceBlob, null, accessCondition: AccessCondition.GenerateIfNotExistsCondition(), null, null).ConfigureAwait(false);
                this.logger.LogDebug("CreateOrUpdate: User preference created successfully ", userPreference.UserId);
            }

            UserPreference? fetchStoredUserPreference = await this.GetAsync(userPreference.UserId).ConfigureAwait(false);
            return fetchStoredUserPreference!;
        }
    }
}
