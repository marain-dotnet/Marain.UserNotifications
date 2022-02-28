// <copyright file="GetTemplateService.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Management.Host.OpenApi
{
    using System.Threading.Tasks;
    using Corvus.Tenancy;
    using Marain.Models;
    using Marain.NotificationTemplates;
    using Marain.NotificationTemplates.CommunicationTemplates;
    using Marain.Services.Tenancy;
    using Marain.UserNotifications.Management.Host.Mappers;
    using Menes;
    using Menes.Exceptions;
    using Menes.Hal;

    /// <summary>
    /// Implements the get template endpoint for the management API.
    /// </summary>
    public class GetTemplateService : IOpenApiService
    {
        /// <summary>
        /// The operation Id for the endpoint.
        /// </summary>
        public const string GetTemplateOperationId = "getTemplate";

        private readonly IMarainServicesTenancy marainServicesTenancy;
        private readonly ITenantedNotificationTemplateStoreFactory tenantedTemplateStoreFactory;
        private readonly WebPushTemplateMapper webPushTemplateMapper;
        private readonly EmailTemplateMapper emailTemplateMapper;
        private readonly SmsTemplateMapper smsTemplateMapper;

        /// <summary>
        /// Initializes a new instance of <see cref="GetTemplateService"/> class.
        /// </summary>
        /// <param name="marainServicesTenancy">Marain tenancy services.</param>
        /// <param name="tenantedTemplateStoreFactory">Template store factory.</param>
        /// <param name="webPushTemplateMapper">WebPush Template Mapper.</param>
        /// <param name="emailTemplateMapper">Email Template Mapper.</param>
        /// <param name="smsTemplateMapper"><see cref="SmsTemplateMapper"/>.</param>
        public GetTemplateService(
            IMarainServicesTenancy marainServicesTenancy,
            ITenantedNotificationTemplateStoreFactory tenantedTemplateStoreFactory,
            WebPushTemplateMapper webPushTemplateMapper,
            EmailTemplateMapper emailTemplateMapper,
            SmsTemplateMapper smsTemplateMapper)
        {
            this.marainServicesTenancy = marainServicesTenancy;
            this.tenantedTemplateStoreFactory = tenantedTemplateStoreFactory;
            this.webPushTemplateMapper = webPushTemplateMapper;
            this.emailTemplateMapper = emailTemplateMapper;
            this.smsTemplateMapper = smsTemplateMapper;
        }

        /// <summary>
        /// Gets a template.
        /// </summary>
        /// <param name="context">The current OpenApi context.</param>
        /// <param name="notificationType">The notification type for which the template is being fetched for.</param>
        /// <param name="communicationType">The communication type of the template.</param>
        /// <returns>Gets the TemplateWrapper object.</returns>
        [OperationId(GetTemplateOperationId)]
        public async Task<OpenApiResult> GetTemplateAsync(
            IOpenApiContext context,
            string notificationType,
            CommunicationType communicationType)
        {
            // We can guarantee tenant Id is available because it's part of the Uri.
            ITenant tenant = await this.marainServicesTenancy.GetRequestingTenantAsync(context.CurrentTenantId!).ConfigureAwait(false);

            INotificationTemplateStore store = await this.tenantedTemplateStoreFactory.GetTemplateStoreForTenantAsync(tenant).ConfigureAwait(false);

            HalDocument? response = null;
            string? eTag = null;

            // Gets the template by notificationType
            switch (communicationType)
            {
                case CommunicationType.Email:
                    (EmailTemplate, string?) emailTemplateWrapper = await store.GetAsync<EmailTemplate>(notificationType, communicationType).ConfigureAwait(false);

                    if (emailTemplateWrapper.Item1 is null)
                    {
                        throw new OpenApiNotFoundException($"The notification template for notificationType {notificationType} and communicationType {communicationType} was not found.");
                    }

                    // Add etag to the EmailTemplate
                    eTag = emailTemplateWrapper.Item2;
                    emailTemplateWrapper.Item1.ETag = emailTemplateWrapper.Item2;
                    response = await this.emailTemplateMapper.MapAsync(emailTemplateWrapper.Item1, context).ConfigureAwait(false);
                    break;

                case CommunicationType.Sms:
                    (SmsTemplate, string?) smsTemplateWrapper = await store.GetAsync<SmsTemplate>(notificationType, communicationType).ConfigureAwait(false);
                    if (smsTemplateWrapper.Item1 is null)
                    {
                        throw new OpenApiNotFoundException($"The notification template for notificationType {notificationType} and communicationType {communicationType} was not found.");
                    }

                    // Add etag to the SmsTemplate
                    eTag = smsTemplateWrapper.Item2;
                    smsTemplateWrapper.Item1.ETag = smsTemplateWrapper.Item2;
                    response = await this.smsTemplateMapper.MapAsync(smsTemplateWrapper.Item1, context).ConfigureAwait(false);
                    break;

                case CommunicationType.WebPush:
                    (WebPushTemplate, string?) webpushWrapper = await store.GetAsync<WebPushTemplate>(notificationType, communicationType).ConfigureAwait(false);
                    if (webpushWrapper.Item1 is null)
                    {
                        throw new OpenApiNotFoundException($"The notification template for notificationType {notificationType} and communicationType {communicationType} was not found.");
                    }

                    // Add etag to the WebPushTemplate
                    eTag = webpushWrapper.Item2;
                    webpushWrapper.Item1.ETag = webpushWrapper.Item2;
                    response = await this.webPushTemplateMapper.MapAsync(webpushWrapper.Item1, context).ConfigureAwait(false);
                    break;
            }

            if (response is null)
            {
                throw new OpenApiNotFoundException($"The notification template for notificationType {notificationType} and communicationType {communicationType} was not found.");
            }

            OpenApiResult okResult = this.OkResult(response, "application/json");
            if (!string.IsNullOrEmpty(eTag))
            {
                okResult.Results.Add("ETag", eTag);
            }

            return okResult;
        }
    }
}