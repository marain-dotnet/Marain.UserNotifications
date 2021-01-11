// <copyright file="GetDeliveryChannelConfigurationService.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.Management.Host.OpenApi
{
    using System;
    using System.Threading.Tasks;
    using Corvus.Tenancy;
    using Marain.DeliveryChannelConfiguration;
    using Marain.Services.Tenancy;
    using Menes;
    using Menes.Exceptions;

    /// <summary>
    /// Implements the get Delivery Channel Configuration endpoint for the management API.
    /// </summary>
    public class GetDeliveryChannelConfigurationService : IOpenApiService
    {
        /// <summary>
        /// The operation Id for the endpoint.
        /// </summary>
        public const string GetDeliveryChannelConfigurationOperationId = "getDeliveryChannelConfiguration";

        private readonly IMarainServicesTenancy marainServicesTenancy;
        private readonly ITenantedDeliveryChannelConfigurationStoreFactory tenantedDeliveryChannelConfigurationStoreFactory;

        /// <summary>
        /// Initializes a new instance of <see cref="GetDeliveryChannelConfigurationService"/> class.
        /// </summary>
        /// <param name="marainServicesTenancy">Marain tenancy services.</param>
        /// <param name="tenantedDeliveryChannelConfigurationStoreFactory">Delivery Channel Configuration store factory..</param>
        public GetDeliveryChannelConfigurationService(
            IMarainServicesTenancy marainServicesTenancy,
            ITenantedDeliveryChannelConfigurationStoreFactory tenantedDeliveryChannelConfigurationStoreFactory)
        {
            this.marainServicesTenancy = marainServicesTenancy
                ?? throw new ArgumentNullException(nameof(marainServicesTenancy));
            this.tenantedDeliveryChannelConfigurationStoreFactory = tenantedDeliveryChannelConfigurationStoreFactory
                ?? throw new ArgumentNullException(nameof(tenantedDeliveryChannelConfigurationStoreFactory));
        }

        /// <summary>
        /// Gets a Delivery Channel Configuration object.
        /// </summary>
        /// <param name="context">The current OpenApi context.</param>
        /// <returns>Gets the Delivery Channel Configuration object.</returns>
        [OperationId(GetDeliveryChannelConfigurationOperationId)]
        public async Task<OpenApiResult> GetDeliveryChannelConfigurationAsync(
            IOpenApiContext context)
        {
            // We can guarantee tenant Id is available because it's part of the Uri.
            ITenant tenant = await this.marainServicesTenancy.GetRequestingTenantAsync(context.CurrentTenantId!).ConfigureAwait(false);

            IDeliveryChannelConfigurationStore? store = await this.tenantedDeliveryChannelConfigurationStoreFactory.GetDeliveryChannelConfigurationStoreForTenantAsync(tenant).ConfigureAwait(false);

            // Gets the DeliveryChannelConfiguration
            DeliveryChannelConfiguration? deliveryChannelConfigurationObject = await store.GetAsync(context.CurrentTenantId!).ConfigureAwait(false);

            if (deliveryChannelConfigurationObject is null)
            {
                throw new OpenApiNotFoundException($"The Delivery Channel Configuration for TenantId: {context.CurrentTenantId} was not found.");
            }

            return this.OkResult(deliveryChannelConfigurationObject, "application/json");
        }
    }
}
