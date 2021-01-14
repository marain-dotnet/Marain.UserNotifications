// <copyright file="AirshipClient.cs" company="Endjin Limited">
// Copyright (c) Endjin Limited. All rights reserved.
// </copyright>

namespace Marain.UserNotifications.ThirdParty.DeliveryChannels.Airship
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;
    using Marain.UserNotifications.ThirdParty.DeliveryChannels.Airship.Models;
    using Newtonsoft.Json;

    /// <summary>
    /// Airship Client is responsible to trigger the push notification.
    /// </summary>
    public class AirshipClient : IAirshipClient
    {
        private string baseUrl = "https://go.urbanairship.com";

        /// <summary>
        /// Initilise the <see cref="AirshipClient"/>.
        /// </summary>
        /// <param name="client">Sington Reference for <see cref="HttpClient"/>.</param>
        /// <param name="applicationKey">Airship Application Key.</param>
        /// <param name="applicationSecretOrMasterSecret">Airship Secret for the provided Application Key.</param>
        public AirshipClient(HttpClient client, string applicationKey, string applicationSecretOrMasterSecret)
        {
            this.Client = client;
            string? textToEncode = $"{applicationKey}:{applicationSecretOrMasterSecret}";

            byte[] plainTextBytes = Encoding.ASCII.GetBytes(textToEncode);
            string? base64String = Convert.ToBase64String(plainTextBytes);

            this.AuthenticationHeaderValue = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", base64String);
        }

        /// <summary>
        /// Gets or Sets the AuthenticationHeaderValue.
        /// </summary>
        public System.Net.Http.Headers.AuthenticationHeaderValue AuthenticationHeaderValue { get; set; }

        /// <summary>
        /// Gets the HTTP client.
        /// </summary>
        public HttpClient Client { get; }

        /// <summary>
        /// Triggers notification to be sent from <see cref="AirshipClient"/>.
        /// </summary>
        /// <param name="namedUser">The unique Id of the targetted user which is being sent this notification.</param>
        /// <param name="notification">The notification object which containts all necessary information about the triggered notificaion.</param>
        /// <returns>HttpResponse returned from the Airship Endpoint.</returns>
        public async Task<string> SendWebPushNotification(string namedUser, Notification notification)
        {
            var testObject = new PushObject()
            {
                Audience = new Audience()
                {
                    NamedUser = namedUser,
                },
                Notification = notification,
                DeviceTypes = new List<string>() { "web" },
            };

            string? requestContent = JsonConvert.SerializeObject(testObject);

            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri($"{this.baseUrl}/api/push"),
                Method = HttpMethod.Post,
                Content = new StringContent(requestContent),
            };

            this.Client.DefaultRequestHeaders.Authorization = this.AuthenticationHeaderValue;

            HttpResponseMessage? result = await this.Client.SendAsync(request).ConfigureAwait(false);

            return await result.Content.ReadAsStringAsync();
        }
    }
}
