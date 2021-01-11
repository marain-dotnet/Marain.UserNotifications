namespace Marain.UserNotifications.ThirdParty.DeliveryChannels.Airship
{
    using Marain.UserNotifications.ThirdParty.DeliveryChannels.Airship.Models;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    public class AirshipClient
    {
        private string baseUrl = "https://go.urbanairship.com";

        public AirshipClient(HttpClient client, string applicationKey, string applicationSecretOrMasterSecret)
        {
            this.Client = client;
            string? textToEncode = $"{applicationKey}:{applicationSecretOrMasterSecret}";

            byte[]? plainTextBytes = Encoding.ASCII.GetBytes(textToEncode);
            string? base64String = Convert.ToBase64String(plainTextBytes);

            authenticationHeaderValue = new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", base64String);
        }

        /// <summary>
        /// Gets or Sets the AuthenticationHeaderValue.
        /// </summary>
        public System.Net.Http.Headers.AuthenticationHeaderValue authenticationHeaderValue { get; set; }

        /// <summary>
        /// Gets the HTTP client.
        /// </summary>
        public HttpClient Client { get; }

        public async Task<string> PushNotification(string namedUser, Notification notification)
        {
            var testObject = new PushObject()
            {
                Audience = new Audience()
                {
                    NamedUser = namedUser
                },
                Notification = notification,
                DeviceTypes = new List<string>() { "web" }
            };

            string? requestContent = JsonConvert.SerializeObject(testObject);

            var request = new HttpRequestMessage()
            {
                RequestUri = new Uri($"{baseUrl}/api/push"),
                Method = HttpMethod.Post,
                Content = new StringContent(requestContent)
            };

            this.Client.DefaultRequestHeaders.Authorization = authenticationHeaderValue;

            HttpResponseMessage? result = await this.Client.SendAsync(request);

            return await result.Content.ReadAsStringAsync();
        }
    }
}
