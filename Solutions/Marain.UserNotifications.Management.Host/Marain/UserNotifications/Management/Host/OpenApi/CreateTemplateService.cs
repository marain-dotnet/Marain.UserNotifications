// <copyright file="CreateTemplateService.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Management.Host.OpenApi
{
    using System;
    using System.Threading.Tasks;
    using Corvus.Tenancy;
    using Marain.NotificationTemplates;
    using Marain.NotificationTemplates.CommunicationTemplates;
    using Marain.Services.Tenancy;
    using Marain.UserPreferences;
    using Menes;
    using Menes.Exceptions;

    /// <summary>
    /// Implements the create template endpoint for the management API.
    /// </summary>
    public class CreateTemplateService : IOpenApiService
    {
        /// <summary>
        /// The operation Id for the endpoint.
        /// </summary>
        public const string CreateTemplateOperationId = "createTemplate";

        private readonly IMarainServicesTenancy marainServicesTenancy;
        private readonly ITenantedNotificationTemplateStoreFactory tenantedTemplateStoreFactory;

        /// <summary>
        /// Initializes a new instance of <see cref="CreateTemplateService"/> class.
        /// </summary>
        /// <param name="marainServicesTenancy">Marain tenancy services.</param>
        /// <param name="tenantedTemplateStoreFactory">Template store factory.</param>
        public CreateTemplateService(
            IMarainServicesTenancy marainServicesTenancy,
            ITenantedNotificationTemplateStoreFactory tenantedTemplateStoreFactory)
        {
            this.marainServicesTenancy = marainServicesTenancy
                ?? throw new ArgumentNullException(nameof(marainServicesTenancy));
            this.tenantedTemplateStoreFactory = tenantedTemplateStoreFactory
                ?? throw new ArgumentNullException(nameof(tenantedTemplateStoreFactory));
        }

        /// <summary>
        /// Create and updates a template.
        /// </summary>
        /// <param name="context">The current OpenApi context.</param>
        /// <param name="body">The request body.</param>
        /// <returns>Confirms that the create / update operation request is successful.</returns>
        [OperationId(CreateTemplateOperationId)]
        public async Task<OpenApiResult> CreateTemplateAsync(
            IOpenApiContext context,
            ICommunicationTemplate body)
        {
            if (string.IsNullOrWhiteSpace(body.NotificationType))
            {
                throw new OpenApiNotFoundException("The NotificationType was not found in the object");
            }

            if (string.IsNullOrWhiteSpace(body.ContentType))
            {
                throw new OpenApiNotFoundException("The ContentType was not found in the object");
            }

            // We can guarantee tenant Id is available because it's part of the Uri.
            ITenant tenant = await this.marainServicesTenancy.GetRequestingTenantAsync(context.CurrentTenantId!).ConfigureAwait(false);

            // Gets the AzureBlobTemplateStore
            INotificationTemplateStore store = await this.tenantedTemplateStoreFactory.GetTemplateStoreForTenantAsync(tenant).ConfigureAwait(false);

            // I don't like this
            // It also means that if we need to add another type, it will have to be done manually
            if (body is EmailTemplate emailTemplate)
            {
                await store.StoreAsync<EmailTemplate>(body.NotificationType!, CommunicationType.Email, emailTemplate).ConfigureAwait(false);
            }
            else if (body is SmsTemplate smsTemplate)
            {
                await store.StoreAsync<SmsTemplate>(body.NotificationType!, CommunicationType.Sms, smsTemplate).ConfigureAwait(false);
            }
            else if (body is WebPushTemplate webPushTemplate)
            {
                await store.StoreAsync<WebPushTemplate>(body.NotificationType!, CommunicationType.WebPush, webPushTemplate).ConfigureAwait(false);
            }
            else
            {
                // this should be removed in future updates
                throw new OpenApiNotFoundException($"The template for ContentType: {body.ContentType} is not a valid content type");
            }

            return this.OkResult();
        }
    }
}
