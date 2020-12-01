// <copyright file="CreateTemplateService.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Management.Host.OpenApi
{
    using System.Threading.Tasks;
    using Corvus.Tenancy;
    using Marain.Services.Tenancy;
    using Marain.UserPreferences;
    using Menes;

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
            this.marainServicesTenancy = marainServicesTenancy;
            this.tenantedTemplateStoreFactory = tenantedTemplateStoreFactory;
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
            NotificationTemplate body)
        {
            // We can guarantee tenant Id is available because it's part of the Uri.
            ITenant tenant = await this.marainServicesTenancy.GetRequestingTenantAsync(context.CurrentTenantId!).ConfigureAwait(false);

            // Gets the AzureBlobTemplateStore
            INotificationTemplateStore store = await this.tenantedTemplateStoreFactory.GetTemplateStoreForTenantAsync(tenant).ConfigureAwait(false);

            // Save the TemplateWrapper object in the blob
            await store.StoreAsync(body).ConfigureAwait(false);

            return this.OkResult();
        }
    }
}
