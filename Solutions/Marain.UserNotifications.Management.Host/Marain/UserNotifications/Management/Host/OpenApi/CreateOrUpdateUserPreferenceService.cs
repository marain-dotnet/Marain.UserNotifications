// <copyright file="CreateOrUpdateUserPreferenceService.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Management.Host.OpenApi
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using Corvus.Tenancy;
    using Marain.Operations.Client.OperationsControl;
    using Marain.Services.Tenancy;
    using Marain.UserPreferences;
    using Menes;
    using Menes.Exceptions;
    using Microsoft.Azure.Storage;

    /// <summary>
    /// Implements the create user preferences endpoint for the management API.
    /// </summary>
    public class CreateOrUpdateUserPreferenceService : IOpenApiService
    {
        /// <summary>
        /// The operation Id for the endpoint.
        /// </summary>
        public const string CreateOrUpdateUserPreferenceOperationId = "createOrUpdateUserPreference";

        private readonly IMarainServicesTenancy marainServicesTenancy;
        private readonly ITenantedUserPreferencesStoreFactory tenantedUserPreferencesStoreFactory;

        /// <summary>
        /// Initializes a new instance of <see cref="CreateOrUpdateUserPreferenceService"/> class.
        /// </summary>
        /// <param name="marainServicesTenancy">Marain tenancy services.</param>
        /// <param name="tenantedUserPreferencesStoreFactory">User preferences store factory.</param>
        public CreateOrUpdateUserPreferenceService(
            IMarainServicesTenancy marainServicesTenancy,
            ITenantedUserPreferencesStoreFactory tenantedUserPreferencesStoreFactory)
        {
            this.marainServicesTenancy = marainServicesTenancy;
            this.tenantedUserPreferencesStoreFactory = tenantedUserPreferencesStoreFactory;
        }

        /// <summary>
        /// Create a user preference.
        /// </summary>
        /// <param name="context">The current OpenApi context.</param>
        /// <param name="body">The request body.</param>
        /// <param name="etag">The ETag.</param>
        /// <returns>Confirms that the create / update operation request is successful.</returns>
        [OperationId(CreateOrUpdateUserPreferenceOperationId)]
        public async Task<OpenApiResult> CreateOrUpdateUserPreferenceAsync(
            IOpenApiContext context,
            UserPreference body,
            [OpenApiParameter("If-None-Match")]
            string etag)
        {
            // We can guarantee tenant Id is available because it's part of the Uri.
            ITenant tenant = await this.marainServicesTenancy.GetRequestingTenantAsync(context.CurrentTenantId!).ConfigureAwait(false);

            // Gets the AzureBlobUserPreferencesStore
            IUserPreferencesStore store = await this.tenantedUserPreferencesStoreFactory.GetUserPreferencesStoreForTenantAsync(tenant).ConfigureAwait(false);

            try
            {
                // Add the etag to user preference object
                UserPreference? updatedUserPreference = body.AddETag(body, etag);

                // Save the UserPreference object in the blob
                await store.CreateOrUpdate(updatedUserPreference).ConfigureAwait(false);
            }
            catch (StorageException e)
            {
                if (e?.RequestInformation?.HttpStatusCode == (int)System.Net.HttpStatusCode.PreconditionFailed)
                {
                    throw new OpenApiBadRequestException("Precondition failure. Blob's ETag does not match ETag provided.");
                }

                throw;
            }

            return this.OkResult();
        }
    }
}
