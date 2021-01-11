// <copyright file="CreateOrUpdateDeliveryChannelConfigurationService.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Management.Host.OpenApi
{
    using System.Threading.Tasks;
    using Corvus.Tenancy;
    using Marain.DeliveryChannelConfiguration;
    using Marain.Services.Tenancy;
    using Menes;

    /// <summary>
    /// Implements the create or update delivery channel configuration endpoint for the management API.
    /// </summary>
    public class CreateOrUpdateDeliveryChannelConfigurationService : IOpenApiService
    {
        /// <summary>
        /// The operation Id for the endpoint.
        /// </summary>
        public const string CreateOrUpdateDeliveryChannelConfigurationOperationId = "createOrUpdateDeliveryChannelConfiguration";

        private readonly IMarainServicesTenancy marainServicesTenancy;
        private readonly ITenantedDeliveryChannelConfigurationStoreFactory tenantedDeliveryChannelConfigurationStoreFactory;

        /// <summary>
        /// Initializes a new instance of <see cref="CreateOrUpdateDeliveryChannelConfigurationService"/> class.
        /// </summary>
        /// <param name="marainServicesTenancy">Marain tenancy services.</param>
        /// <param name="tenantedDeliveryChannelConfigurationStoreFactory">Delivery Channel Configuration store factory.</param>
        public CreateOrUpdateDeliveryChannelConfigurationService(
            IMarainServicesTenancy marainServicesTenancy,
            ITenantedDeliveryChannelConfigurationStoreFactory tenantedDeliveryChannelConfigurationStoreFactory)
        {
            this.marainServicesTenancy = marainServicesTenancy
                ?? throw new System.ArgumentNullException(nameof(marainServicesTenancy));
            this.tenantedDeliveryChannelConfigurationStoreFactory = tenantedDeliveryChannelConfigurationStoreFactory
                ?? throw new System.ArgumentNullException(nameof(tenantedDeliveryChannelConfigurationStoreFactory));
        }

        /// <summary>
        /// Create or update a delivery channel configuration.
        /// </summary>
        /// <param name="context">The current OpenApi context.</param>
        /// <param name="body">The request body.</param>
        /// <returns>Confirms that the create / update operation request is successful.</returns>
        [OperationId(CreateOrUpdateDeliveryChannelConfigurationOperationId)]
        public async Task<OpenApiResult> CreateOrUpdateDeliveryChannelConfigurationAsync(
            IOpenApiContext context,
            DeliveryChannelConfiguration body)
        {
            // We can guarantee tenant Id is available because it's part of the Uri.
            ITenant tenant = await this.marainServicesTenancy.GetRequestingTenantAsync(context.CurrentTenantId!).ConfigureAwait(false);

            // Gets the DeliveryChannelConfigurationStore
            IDeliveryChannelConfigurationStore? store = await this.tenantedDeliveryChannelConfigurationStoreFactory.GetDeliveryChannelConfigurationStoreForTenantAsync(tenant).ConfigureAwait(false);

            // Save the delivery channel configuration object in the blob
            await store.CreateOrUpdate(context.CurrentTenantId!, body).ConfigureAwait(false);

            return this.OkResult();
        }
    }
}
