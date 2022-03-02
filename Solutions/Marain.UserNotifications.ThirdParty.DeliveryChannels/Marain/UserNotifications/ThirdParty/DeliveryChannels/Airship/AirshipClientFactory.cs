// <copyright file="AirshipClientFactory.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.ThirdParty.DeliveryChannels.Airship
{
    using System.Net.Http;

    /// <summary>
    /// Airship Client Factory to initialise <see cref="AirshipClient"/>.
    /// </summary>
    public class AirshipClientFactory : IAirshipClientFactory
    {
        private readonly HttpClient httpClient;

        /// <summary>
        /// Constructor for the <see cref="AirshipClientFactory"/>.
        /// </summary>
        /// <param name="httpClient">Singleton Instance for the HttpClient.</param>
        public AirshipClientFactory(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        /// <inheritdoc/>
        public AirshipClient GetAirshipClient(string applicationKey, string masterSecret) => new(this.httpClient, applicationKey, masterSecret);
    }
}