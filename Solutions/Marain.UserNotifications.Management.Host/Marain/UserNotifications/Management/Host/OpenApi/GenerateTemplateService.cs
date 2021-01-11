// <copyright file="GenerateTemplateService.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Management.Host.OpenApi
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Corvus.Tenancy;
    using DotLiquid;
    using Marain.Helper;
    using Marain.Models;
    using Marain.NotificationTemplates;
    using Marain.NotificationTemplates.CommunicationTemplates;
    using Marain.Services.Tenancy;
    using Marain.UserNotifications.Management.Host.Composer;
    using Marain.UserPreferences;
    using Menes;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Implements the generate template endpoint for the management API.
    /// </summary>
    public class GenerateTemplateService : IOpenApiService
    {
        /// <summary>
        /// The operation Id for the endpoint.
        /// </summary>
        public const string GenerateTemplateOperationId = "generateTemplate";

        private readonly IMarainServicesTenancy marainServicesTenancy;
        private readonly ITenantedNotificationTemplateStoreFactory tenantedTemplateStoreFactory;
        private readonly ITenantedUserPreferencesStoreFactory tenantedUserPreferencesStoreFactory;
        private readonly ILogger<GenerateTemplateService> logger;
        private readonly IGenerateTemplateComposer generateTemplateComposer;

        /// <summary>
        /// Initializes a new instance of <see cref="GenerateTemplateService"/> class.
        /// </summary>
        /// <param name="marainServicesTenancy">Marain tenancy services.</param>
        /// <param name="tenantedTemplateStoreFactory">Template store factory.</param>
        /// <param name="tenantedUserPreferencesStoreFactory">User Preference store factory.</param>
        /// <param name="generateTemplateComposer">The composer to generate the templated notification per communication channel.</param>
        /// <param name="logger">The logger for GenerateTemplateService.</param>
        public GenerateTemplateService(
            IMarainServicesTenancy marainServicesTenancy,
            ITenantedNotificationTemplateStoreFactory tenantedTemplateStoreFactory,
            ITenantedUserPreferencesStoreFactory tenantedUserPreferencesStoreFactory,
            IGenerateTemplateComposer generateTemplateComposer,
            ILogger<GenerateTemplateService> logger)
        {
            this.marainServicesTenancy = marainServicesTenancy;
            this.tenantedTemplateStoreFactory = tenantedTemplateStoreFactory;
            this.tenantedUserPreferencesStoreFactory = tenantedUserPreferencesStoreFactory;
            this.logger = logger;
            this.generateTemplateComposer = generateTemplateComposer;
        }

        /// <summary>
        /// Generates a NotificationTypeTemplate which contains populated notification templates for different communication types for the user.
        /// </summary>
        /// <param name="context">The current OpenApi context.</param>
        /// <param name="body">The request body.</param>
        /// <returns>Confirms that the create / update operation request is successful.</returns>
        [OperationId(GenerateTemplateOperationId)]
        public async Task<OpenApiResult> GenerateTemplateAsync(
            IOpenApiContext context,
            CreateNotificationsRequest body)
        {
            // We can guarantee tenant Id is available because it's part of the Uri.
            ITenant tenant = await this.marainServicesTenancy.GetRequestingTenantAsync(context.CurrentTenantId!).ConfigureAwait(false);

            // Get the UserPreferencesStore for the tenant
            IUserPreferencesStore userPreferencesStore = await this.tenantedUserPreferencesStoreFactory.GetUserPreferencesStoreForTenantAsync(tenant).ConfigureAwait(false);

            // Get the user preference for the userId
            UserPreference? userPreference = await userPreferencesStore.GetAsync(body.UserIds[0]).ConfigureAwait(false);

            // Check if the user has set the communication channels for the incoming notification type
            if (userPreference is null)
            {
                throw new UserNotificationNotFoundException($"There is no user preference set up for this user {body.UserIds[0]} for tenant {tenant.Id}");
            }

            if (userPreference.CommunicationChannelsPerNotificationConfiguration is null)
            {
                throw new UserNotificationNotFoundException($"There are no communication channel set up for the user {body.UserIds[0]} for tenant {tenant.Id}");
            }

            if (!userPreference.CommunicationChannelsPerNotificationConfiguration.ContainsKey(body.NotificationType))
            {
                throw new UserNotificationNotFoundException($"There is no communication channel set up for the user {body.UserIds[0]} for notification type {body.NotificationType} for tenant {tenant.Id}");
            }

            List<CommunicationType>? registeredCommunicationChannels = userPreference.CommunicationChannelsPerNotificationConfiguration[body.NotificationType];

            // TODO: In the future, check if these registeredCommunicationChannels are actually usable for the current Tenant.
            if (registeredCommunicationChannels is null || registeredCommunicationChannels.Count == 0)
            {
                throw new Exception($"There are no communication channel set up for the user {body.UserIds[0]} for notification type {body.NotificationType} for tenant {tenant.Id}");
            }

            // Gets the AzureBlobTemplateStore
            INotificationTemplateStore templateStore = await this.tenantedTemplateStoreFactory.GetTemplateStoreForTenantAsync(tenant).ConfigureAwait(false);

            NotificationTemplate? responseTemplate = await this.generateTemplateComposer.GenerateTemplateAsync(templateStore, body.Properties, registeredCommunicationChannels, body.NotificationType).ConfigureAwait(false);
            return this.OkResult(responseTemplate);
        }
    }
}
