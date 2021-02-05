// <copyright file="IAirshipClientFactory.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.ThirdParty.DeliveryChannels.Airship
{
    /// <summary>
    /// Airship Client Factory to initialise <see cref="AirshipClient"/>.
    /// </summary>
    public interface IAirshipClientFactory
    {
        /// <summary>
        /// Get <see cref="AirshipClient"/> instance.
        /// </summary>
        /// <param name="applicationKey">Airship Application Key.</param>
        /// <param name="masterSecret">Airship Secret for the provided Application Key.</param>
        /// <returns><see cref="AirshipClient"/> instance.</returns>
        AirshipClient GetAirshipClient(string applicationKey, string masterSecret);
    }
}