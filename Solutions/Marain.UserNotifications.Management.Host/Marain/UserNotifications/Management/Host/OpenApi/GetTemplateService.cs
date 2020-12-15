// <copyright file="GetTemplateService.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Management.Host.OpenApi
{
    using System.Threading.Tasks;
    using Corvus.Tenancy;
    using Marain.NotificationTemplates;
    using Marain.NotificationTemplates.CommunicationTemplates;
    using Marain.Services.Tenancy;
    using Marain.UserNotifications.Management.Host.Mappers;
    using Marain.UserPreferences;
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

        /// <summary>
        /// Initializes a new instance of <see cref="GetTemplateService"/> class.
        /// </summary>
        /// <param name="marainServicesTenancy">Marain tenancy services.</param>
        /// <param name="tenantedTemplateStoreFactory">Template store factory.</param>
        /// <param name="webPushTemplateMapper">WebPush Template Mapper.</param>
        /// <param name="emailTemplateMapper">Email Template Mapper.</param>
        public GetTemplateService(
            IMarainServicesTenancy marainServicesTenancy,
            ITenantedNotificationTemplateStoreFactory tenantedTemplateStoreFactory,
            WebPushTemplateMapper webPushTemplateMapper,
            EmailTemplateMapper emailTemplateMapper)
        {
            this.marainServicesTenancy = marainServicesTenancy;
            this.tenantedTemplateStoreFactory = tenantedTemplateStoreFactory;
            this.webPushTemplateMapper = webPushTemplateMapper;
            this.emailTemplateMapper = emailTemplateMapper;
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

            // Gets the template by notificationType
            switch (communicationType)
            {
                case CommunicationType.Email:
                    EmailTemplate emailTemplate = await this.GetTemplateAsync<EmailTemplate>(store, context, notificationType, communicationType).ConfigureAwait(false);

                    response = await this.emailTemplateMapper.MapAsync(emailTemplate, context).ConfigureAwait(false);
                    break;
                case CommunicationType.Sms:
                    SmsTemplate smsTemplate = await this.GetTemplateAsync<SmsTemplate>(store, context, notificationType, communicationType).ConfigureAwait(false);

                    // response = await this.smsTemplateMapper.MapAsync(smsTemplate, context).ConfigureAwait(false);
                    break;
                case CommunicationType.WebPush:
                    WebPushTemplate webpush = await this.GetTemplateAsync<WebPushTemplate>(store, context, notificationType, communicationType).ConfigureAwait(false);
                    response = await this.webPushTemplateMapper.MapAsync(webpush, context).ConfigureAwait(false);
                    break;
                default:
                    break;
            }

            if (response is null)
            {
                throw new OpenApiNotFoundException($"The notification template for notificationType {notificationType} and communicationType {communicationType.ToString()} was not found.");
            }

            return this.OkResult(response);
        }

        private async Task<T> GetTemplateAsync<T>(
            INotificationTemplateStore store,
            IOpenApiContext context,
            string notificationType,
            CommunicationType communicationType)
        {
            T? genericTemplate = await store.GetAsync<T>(notificationType, communicationType).ConfigureAwait(false);

            if (genericTemplate is null)
            {
                throw new OpenApiNotFoundException($"The notification template for notificationType {notificationType} and communicationType {communicationType.ToString()} was not found.");
            }

            return genericTemplate!;
        }
    }
}
