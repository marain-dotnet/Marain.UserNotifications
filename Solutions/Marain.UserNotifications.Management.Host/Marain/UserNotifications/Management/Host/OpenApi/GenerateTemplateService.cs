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
    using Marain.NotificationTemplates;
    using Marain.NotificationTemplates.CommunicationTemplates;
    using Marain.Services.Tenancy;
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

        /// <summary>
        /// Initializes a new instance of <see cref="GenerateTemplateService"/> class.
        /// </summary>
        /// <param name="marainServicesTenancy">Marain tenancy services.</param>
        /// <param name="tenantedTemplateStoreFactory">Template store factory.</param>
        /// <param name="tenantedUserPreferencesStoreFactory">User Preference store factory.</param>
        /// <param name="logger">The logger for GenerateTemplateService.</param>
        public GenerateTemplateService(
            IMarainServicesTenancy marainServicesTenancy,
            ITenantedNotificationTemplateStoreFactory tenantedTemplateStoreFactory,
            ITenantedUserPreferencesStoreFactory tenantedUserPreferencesStoreFactory,
            ILogger<GenerateTemplateService> logger)
        {
            this.marainServicesTenancy = marainServicesTenancy;
            this.tenantedTemplateStoreFactory = tenantedTemplateStoreFactory;
            this.tenantedUserPreferencesStoreFactory = tenantedUserPreferencesStoreFactory;
            this.logger = logger;
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

            EmailTemplate? emailTemplate = null;
            SmsTemplate? smsTemplate = null;
            WebPushTemplate? webPushTemplate = null;
            Dictionary<string, object> existingProperties = PropertyBagHelpers.GetDictionaryFromPropertyBag(body.Properties);

            foreach (CommunicationType channel in registeredCommunicationChannels)
            {
                switch (channel)
                {
                    case CommunicationType.Email:
                        try
                        {
                            (EmailTemplate, string?) emailRawTemplate = await templateStore.GetAsync<EmailTemplate>(body.NotificationType, CommunicationType.Email).ConfigureAwait(false);
                            string? emailBody = await this.GenerateTemplateForFieldAsync(emailRawTemplate.Item1.Body, existingProperties).ConfigureAwait(false);
                            string? emailSubject = await this.GenerateTemplateForFieldAsync(emailRawTemplate.Item1.Subject, existingProperties).ConfigureAwait(false);

                            emailTemplate = new EmailTemplate()
                            {
                                NotificationType = body.NotificationType,
                                Body = emailBody,
                                Subject = emailSubject,
                                Important = false,
                            };
                        }
                        catch (Exception)
                        {
                            this.logger.LogError("The template for the communication type Email doesn't exist");
                        }

                        break;
                    case CommunicationType.Sms:
                        try
                        {
                            (SmsTemplate, string?) smsRawTemplate = await templateStore.GetAsync<SmsTemplate>(body.NotificationType, CommunicationType.Sms).ConfigureAwait(false);
                            string? smsBody = await this.GenerateTemplateForFieldAsync(smsRawTemplate.Item1.Body!, existingProperties).ConfigureAwait(false);

                            smsTemplate = new SmsTemplate() { NotificationType = body.NotificationType, Body = smsBody };
                        }
                        catch (Exception)
                        {
                            this.logger.LogError("The template for the communication type Sms doesn't exist");
                        }

                        break;
                    case CommunicationType.WebPush:
                        try
                        {
                            (WebPushTemplate, string?) webPushRawTemplate = await templateStore.GetAsync<WebPushTemplate>(body.NotificationType, CommunicationType.WebPush).ConfigureAwait(false);
                            string? webPushTitle = await this.GenerateTemplateForFieldAsync(webPushRawTemplate.Item1.Title, existingProperties).ConfigureAwait(false);
                            string? webPushBody = await this.GenerateTemplateForFieldAsync(webPushRawTemplate.Item1.Body, existingProperties).ConfigureAwait(false);
                            string? webPushImage = await this.GenerateTemplateForFieldAsync(webPushRawTemplate.Item1.Image, existingProperties).ConfigureAwait(false);

                            webPushTemplate = new WebPushTemplate()
                            {
                                NotificationType = body.NotificationType,
                                Body = webPushBody,
                                Title = webPushTitle,
                                Image = webPushImage,
                            };
                        }
                        catch (Exception)
                        {
                            this.logger.LogError("The template for the communication type WebPush doesn't exist");
                        }

                        break;
                }
            }

            var responseTemplate = new NotificationTemplate(
                body.NotificationType,
                smsTemplate: smsTemplate,
                emailTemplate: emailTemplate,
                webPushTemplate: webPushTemplate);

            // and replace with the tags inside the template with the ones received from the property bag in the CreateNotificationsRequest
            return this.OkResult(responseTemplate);
        }

        /// <summary>
        /// Generate a template for a single field.
        /// </summary>
        /// <param name="templateBody">A string with handlebars. </param>
        /// <param name="properties">A dictionary of all properties that can be used to render the templateBody string. </param>
        /// <returns>A rendered string. </returns>
        private async Task<string?> GenerateTemplateForFieldAsync(string? templateBody, Dictionary<string, object> properties)
        {
            if (string.IsNullOrEmpty(templateBody))
            {
                return null;
            }

            var template = Template.Parse(templateBody);
            return await template.RenderAsync(Hash.FromDictionary(properties)).ConfigureAwait(false);
        }
    }
}
