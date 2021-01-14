// <copyright file="IAirshipClient.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.ThirdParty.DeliveryChannels.Airship
{
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Threading.Tasks;
    using Marain.UserNotifications.ThirdParty.DeliveryChannels.Airship.Models;

    /// <summary>
    /// Airship Client Interface.
    /// </summary>
    public interface IAirshipClient
    {
        /// <summary>
        /// Gets or Sets the AuthenticationHeaderValue.
        /// </summary>
        AuthenticationHeaderValue AuthenticationHeaderValue { get; set; }

        /// <summary>
        /// Gets the HTTP client.
        /// </summary>
        HttpClient Client { get; }

        /// <summary>
        /// Triggers notification to be sent from <see cref="AirshipClient"/>.
        /// </summary>
        /// <param name="namedUser">The unique Id of the targetted user which is being sent this notification.</param>
        /// <param name="notification">The notification object which containts all necessary information about the triggered notificaion.</param>
        /// <returns>HttpResponse returned from the Airship Endpoint.</returns>
        Task<HttpResponseMessage> SendWebPushNotification(string namedUser, Notification notification);
    }
}