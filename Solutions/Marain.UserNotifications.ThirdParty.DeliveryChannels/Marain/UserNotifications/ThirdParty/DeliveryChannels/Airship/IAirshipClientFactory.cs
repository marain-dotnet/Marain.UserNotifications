namespace Marain.UserNotifications.ThirdParty.DeliveryChannels.Airship
{
    public interface IAirshipClientFactory
    {
        AirshipClient GetAirshipClient(string applicationKey, string masterSecret);
    }
}