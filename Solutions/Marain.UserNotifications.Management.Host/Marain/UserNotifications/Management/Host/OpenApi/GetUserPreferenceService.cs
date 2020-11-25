// <copyright file="GetUserPreferenceService.cs" company="Endjin Limited">
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

    /// <summary>
    /// Implements the get user preferences endpoint for the management API.
    /// </summary>
    public class GetUserPreferenceService : IOpenApiService
    {
        /// <summary>
        /// The operation Id for the endpoint.
        /// </summary>
        public const string GetUserPreferenceOperationId = "getUserPreference";

        private readonly IMarainServicesTenancy marainServicesTenancy;
        private readonly ITenantedUserPreferencesStoreFactory tenantedUserPreferencesStoreFactory;

        /// <summary>
        /// Initializes a new instance of <see cref="GetUserPreferenceService"/> class.
        /// </summary>
        /// <param name="marainServicesTenancy">Marain tenancy services.</param>
        /// <param name="tenantedUserPreferencesStoreFactory">User preferences store factory.</param>
        public GetUserPreferenceService(
            IMarainServicesTenancy marainServicesTenancy,
            ITenantedUserPreferencesStoreFactory tenantedUserPreferencesStoreFactory)
        {
            this.marainServicesTenancy = marainServicesTenancy;
            this.tenantedUserPreferencesStoreFactory = tenantedUserPreferencesStoreFactory;
        }

        /// <summary>
        /// Gets a user preference.
        /// </summary>
        /// <param name="context">The current OpenApi context.</param>
        /// <param name="userId">The userId of the user.</param>
        /// <returns>Gets the user preference object.</returns>
        [OperationId(GetUserPreferenceOperationId)]
        public async Task<OpenApiResult> GetUserPreferenceAsync(
            IOpenApiContext context,
            string userId)
        {
            // We can guarantee tenant Id is available because it's part of the Uri.
            ITenant tenant = await this.marainServicesTenancy.GetRequestingTenantAsync(context.CurrentTenantId!).ConfigureAwait(false);

            IUserPreferencesStore store = await this.tenantedUserPreferencesStoreFactory.GetUserPreferencesStoreForTenantAsync(tenant).ConfigureAwait(false);

            // Gets the user preference by userId
            UserPreference? userObject = await store.GetAsync(userId).ConfigureAwait(false);

            if (userObject == null)
            {
                throw new OpenApiNotFoundException($"The user preference for userId: {userId} was not found.");
            }

            return this.OkResult(userObject!);
        }
    }
}
