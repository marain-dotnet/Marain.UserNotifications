// <copyright file="IDeliveryChannelConfigurationStore.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.DeliveryChannelConfiguration
{
    using System.Threading.Tasks;

    /// <summary>
    /// Interface for a service that can store and retrieve delivery channel configuration.
    /// </summary>
    public interface IDeliveryChannelConfigurationStore
    {
        /// <summary>
        /// Retrieves delivery channel configuration for the specified tenant.
        /// </summary>
        /// <param name="tenantId">The tenantId for which the configuration will be retrieved for.</param>
        /// <returns>The user notifications.</returns>
        Task<DeliveryChannelConfiguration?> GetAsync(string tenantId);

        /// <summary>
        /// Stores the given delivery channel configuration.
        /// </summary>
        /// <param name="tenantId">The tenantId for the request.</param>
        /// <param name="deliveryChannelConfiguration">The delivery channel configuration.</param>
        /// <returns>The stored configuration.</returns>
        Task<DeliveryChannelConfiguration> CreateOrUpdate(string tenantId, DeliveryChannelConfiguration deliveryChannelConfiguration);
    }
}
