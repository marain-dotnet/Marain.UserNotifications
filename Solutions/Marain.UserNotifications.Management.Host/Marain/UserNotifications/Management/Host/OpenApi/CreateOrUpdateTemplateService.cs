// <copyright file="CreateOrUpdateTemplateService.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Management.Host.OpenApi
{
    using System;
    using System.Threading.Tasks;
    using Corvus.Tenancy;
    using Marain.Models;
    using Marain.NotificationTemplates;
    using Marain.NotificationTemplates.CommunicationTemplates;
    using Marain.Services.Tenancy;
    using Menes;
    using Menes.Exceptions;
    using Microsoft.Azure.Storage;

    /// <summary>
    /// Implements the create or update template endpoint for the management API.
    /// </summary>
    public class CreateOrUpdateTemplateService : IOpenApiService
    {
        /// <summary>
        /// The operation Id for the endpoint.
        /// </summary>
        public const string CreateOrUpdateTemplateOperationId = "createOrUpdateTemplate";

        private readonly IMarainServicesTenancy marainServicesTenancy;
        private readonly ITenantedNotificationTemplateStoreFactory tenantedTemplateStoreFactory;

        /// <summary>
        /// Initializes a new instance of <see cref="CreateOrUpdateTemplateService"/> class.
        /// </summary>
        /// <param name="marainServicesTenancy">Marain tenancy services.</param>
        /// <param name="tenantedTemplateStoreFactory">Template store factory.</param>
        public CreateOrUpdateTemplateService(
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
        /// <param name="etag">The ETag.</param>
        /// <returns>Confirms that the create / update operation request is successful.</returns>
        [OperationId(CreateOrUpdateTemplateOperationId)]
        public async Task<OpenApiResult> CreateTemplateAsync(
            IOpenApiContext context,
            ICommunicationTemplate body,
            [OpenApiParameter("If-None-Match")]
            string etag)
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

            try
            {
                if (body is EmailTemplate emailTemplate)
                {
                    emailTemplate.ETag = etag;
                    await store.CreateOrUpdate(body.NotificationType, CommunicationType.Email, emailTemplate.ETag, emailTemplate).ConfigureAwait(false);
                }
                else if (body is SmsTemplate smsTemplate)
                {
                    smsTemplate.ETag = etag;
                    await store.CreateOrUpdate(body.NotificationType, CommunicationType.Sms, smsTemplate.ETag, smsTemplate).ConfigureAwait(false);
                }
                else if (body is WebPushTemplate webPushTemplate)
                {
                    webPushTemplate.ETag = etag;
                    await store.CreateOrUpdate(body.NotificationType, CommunicationType.WebPush, webPushTemplate.ETag, webPushTemplate).ConfigureAwait(false);
                }
                else
                {
                    // this should be removed in future updates
                    throw new OpenApiNotFoundException($"The template for ContentType: {body.ContentType} is not a valid content type");
                }
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