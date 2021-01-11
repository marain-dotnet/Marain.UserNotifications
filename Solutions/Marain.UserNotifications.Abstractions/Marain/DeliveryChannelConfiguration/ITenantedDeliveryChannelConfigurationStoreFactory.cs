// <copyright file="ITenantedDeliveryChannelConfigurationStoreFactory.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.DeliveryChannelConfiguration
{
    using System.Threading.Tasks;
    using Corvus.Tenancy;

    /// <summary>
    /// Interface for a factory that can create a tenant-specific <see cref="IDeliveryChannelConfigurationStore"/>.
    /// </summary>
    public interface ITenantedDeliveryChannelConfigurationStoreFactory
    {
        /// <summary>
        /// Retrieves a <see cref="IDeliveryChannelConfigurationStore"/> for the specified <see cref="ITenant"/>.
        /// </summary>
        /// <param name="tenant">The tenant to retrieve the store for.</param>
        /// <returns>The preferences store for the tenant.</returns>
        Task<IDeliveryChannelConfigurationStore> GetDeliveryChannelConfigurationStoreForTenantAsync(ITenant tenant);
    }
}
