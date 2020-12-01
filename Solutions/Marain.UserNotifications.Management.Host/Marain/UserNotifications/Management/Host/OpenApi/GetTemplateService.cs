// <copyright file="GetTemplateService.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Management.Host.OpenApi
{
    using System.Threading.Tasks;
    using Corvus.Tenancy;
    using DotLiquid;
    using Marain.NotificationTemplate.NotificationTemplate;
    using Marain.Services.Tenancy;
    using Marain.UserPreferences;
    using Menes;
    using Menes.Exceptions;

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

        /// <summary>
        /// Initializes a new instance of <see cref="GetTemplateService"/> class.
        /// </summary>
        /// <param name="marainServicesTenancy">Marain tenancy services.</param>
        /// <param name="tenantedTemplateStoreFactory">Template store factory.</param>
        public GetTemplateService(
            IMarainServicesTenancy marainServicesTenancy,
            ITenantedNotificationTemplateStoreFactory tenantedTemplateStoreFactory)
        {
            this.marainServicesTenancy = marainServicesTenancy;
            this.tenantedTemplateStoreFactory = tenantedTemplateStoreFactory;
        }

        /// <summary>
        /// Gets a template.
        /// </summary>
        /// <param name="context">The current OpenApi context.</param>
        /// <param name="notificationType">The notificationtype for which the template is being fetched for.</param>
        /// <returns>Gets the TemplateWrapper object.</returns>
        [OperationId(GetTemplateOperationId)]
        public async Task<OpenApiResult> GetTemplateAsync(
            IOpenApiContext context,
            string notificationType)
        {
            // We can guarantee tenant Id is available because it's part of the Uri.
            ITenant tenant = await this.marainServicesTenancy.GetRequestingTenantAsync(context.CurrentTenantId!).ConfigureAwait(false);

            INotificationTemplateStore store = await this.tenantedTemplateStoreFactory.GetTemplateStoreForTenantAsync(tenant).ConfigureAwait(false);

            // Gets the template by notificationType
            NotificationTemplate? templateObj = await store.GetAsync(notificationType).ConfigureAwait(false);

            if (templateObj == null)
            {
                throw new OpenApiNotFoundException($"The template for notificationType: {notificationType} was not found.");
            }

            return this.OkResult(templateObj!);
        }
    }
}
