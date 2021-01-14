// <copyright file="DeliveryChannelConfiguration.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.DeliveryChannelConfiguration
{
    using System.Collections.Generic;
    using Marain.Models;

    /// <summary>
    /// Delivery channel configuration structure.
    /// </summary>
    public class DeliveryChannelConfiguration
    {
        /// <summary>
        /// Gets or Sets the desired delivery channels which are configured for the communication type.
        /// </summary>
        public Dictionary<CommunicationType, DeliveryChannel>? DeliveryChannelConfiguredPerCommunicationType { get; set; }

        /// <summary>
        /// Gets or Sets the delivery channel configuration.
        /// </summary>
        public Dictionary<DeliveryChannel, string>? DeliveryChannelKeyVaultSecretUrl { get; set; }
    }
}
