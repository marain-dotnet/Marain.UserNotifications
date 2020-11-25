// <copyright file="CreateUserPreferenceService.cs" company="Endjin Limited">
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

    /// <summary>
    /// Implements the create user preferences endpoint for the management API.
    /// </summary>
    public class CreateUserPreferenceService : IOpenApiService
    {
        /// <summary>
        /// The operation Id for the endpoint.
        /// </summary>
        public const string CreateUserPreferenceOperationId = "createUserPreference";

        private readonly IMarainServicesTenancy marainServicesTenancy;
        private readonly ITenantedUserPreferencesStoreFactory tenantedUserPreferencesStoreFactory;

        /// <summary>
        /// Initializes a new instance of <see cref="CreateUserPreferenceService"/> class.
        /// </summary>
        /// <param name="marainServicesTenancy">Marain tenancy services.</param>
        /// <param name="tenantedUserPreferencesStoreFactory">User preferences store factory.</param>
        public CreateUserPreferenceService(
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
        /// <returns>Confirms that the create / update operation request is successful.</returns>
        [OperationId(CreateUserPreferenceOperationId)]
        public async Task<OpenApiResult> CreateUserPreferenceAsync(
            IOpenApiContext context,
            UserPreference body)
        {
            // We can guarantee tenant Id is available because it's part of the Uri.
            ITenant tenant = await this.marainServicesTenancy.GetRequestingTenantAsync(context.CurrentTenantId!).ConfigureAwait(false);

            // Gets the AzureBlobUserPreferencesStore
            IUserPreferencesStore store = await this.tenantedUserPreferencesStoreFactory.GetUserPreferencesStoreForTenantAsync(tenant).ConfigureAwait(false);

            // Save the UserPreference object in the blob
            await store.StoreAsync(body).ConfigureAwait(false);

            return this.OkResult();
        }
    }
}
