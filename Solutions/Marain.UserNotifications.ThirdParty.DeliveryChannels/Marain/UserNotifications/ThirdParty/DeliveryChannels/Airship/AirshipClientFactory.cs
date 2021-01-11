namespace Marain.UserNotifications.ThirdParty.DeliveryChannels.Airship
{
    using System.Net.Http;

    public class AirshipClientFactory : IAirshipClientFactory
    {
        private readonly HttpClient httpClient;

        public AirshipClientFactory(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public AirshipClient GetAirshipClient(string applicationKey, string masterSecret) => new AirshipClient(this.httpClient, applicationKey, masterSecret);
    }
}
