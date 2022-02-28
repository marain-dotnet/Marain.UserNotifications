// <copyright file="Airship.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.ThirdParty.DeliveryChannels.KeyVaultSecretModels
{
    /// <summary>
    /// Airship object for serialisation into concrete object.
    /// </summary>
    public class Airship
    {
        /// <summary>
        /// Gets or Sets the application key of the configured platform.
        /// </summary>
        public string? ApplicationKey { get; set; }

        /// <summary>
        /// Gets or Sets the application secret.
        /// </summary>
        public string? ApplicationSecret { get; set; }

        /// <summary>
        /// Gets or Sets the master secret.
        /// </summary>
        public string? MasterSecret { get; set; }
    }
}