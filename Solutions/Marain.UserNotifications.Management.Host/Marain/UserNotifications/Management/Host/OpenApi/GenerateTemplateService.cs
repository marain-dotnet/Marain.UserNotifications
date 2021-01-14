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
        private readonly ILogger<GenerateTemplateService> logger;
        private readonly IGenerateTemplateComposer generateTemplateComposer;

        /// <summary>
        /// Initializes a new instance of <see cref="GenerateTemplateService"/> class.
        /// </summary>
        /// <param name="marainServicesTenancy">Marain tenancy services.</param>
        /// <param name="tenantedTemplateStoreFactory">Template store factory.</param>
        /// <param name="generateTemplateComposer">The composer to generate the templated notification per communication channel.</param>
        /// <param name="logger">The logger for GenerateTemplateService.</param>
        public GenerateTemplateService(
            IMarainServicesTenancy marainServicesTenancy,
            ITenantedNotificationTemplateStoreFactory tenantedTemplateStoreFactory,
            IGenerateTemplateComposer generateTemplateComposer,
            ILogger<GenerateTemplateService> logger)
        {
            this.marainServicesTenancy = marainServicesTenancy;
            this.tenantedTemplateStoreFactory = tenantedTemplateStoreFactory;
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

            var registeredCommunicationChannels = new List<CommunicationType>() { CommunicationType.WebPush, CommunicationType.Email, CommunicationType.Sms };

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
